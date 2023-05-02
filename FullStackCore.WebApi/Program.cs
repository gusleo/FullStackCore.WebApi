using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Scaffolding.Auth.Entities;
using Scaffolding.Auth.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;
using Hangfire;
using Hangfire.Dashboard;
using Core.Services.Extensions;
using FullStackCore.WebApi.Helper;
using FullStackCore.WebApi.CronJob;
using Core.Data;
using Core.Services;
using FullStackCore.WebApi.Options;
using OpenIddict.Validation.AspNetCore;
using FullStackCore.WebApi.Filters;
using FullStackCore.WebApi.Logger;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<CoreContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailConfirmation")
    .AddTokenProvider<PasswordResetTokenProvider<ApplicationUser>>("passwordReset");


// Register the OpenIddict validation components.
builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        var issuer = builder.Configuration.GetValue<string>("JwtConfig:Issuer");
        // Note: the validation handler uses OpenID Connect discovery
        // to retrieve the address of the introspection endpoint.
        options.SetIssuer(issuer);
        options.AddAudiences("resource_server");

        // Configure the validation handler to use introspection and register the client
        // credentials used when communicating with the remote introspection endpoint.
        options.UseIntrospection()
               .SetClientId("resource_server")
               .SetClientSecret("846B62D0-DEF9-4215-A99D-86E6B8DAB342");

        // Register the System.Net.Http integration.
        options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
builder.Services.AddAuthorization();

// add controller
builder.Services.AddControllers();

//add automapper
builder.Services.AddAutoMapper(typeof(CoreProfile));


#region API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
});
#endregion

#region Swagger
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(option =>
{
    #region JWT Authorization
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    #endregion
    option.OperationFilter<SwaggerDefaultValues>();
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

#region Register Option
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
#endregion

#endregion

builder.Services.AddAuthorization(options =>
{
    // example of policy
    options.AddPolicy(
        MembershipConstant.EmailBlastEditor,
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == MembershipConstant.EmailBlastEditor)
                || context.User.IsInRole(MembershipConstant.SuperAdmin) || context.User.IsInRole(MembershipConstant.Admin))
    );

});

// add dependency injection for database, repository and service layer
string connString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
builder.Services.AddHttpContextAccessor();
builder.Services.AddDatabaseLayer(connString);
builder.Services.AddServiceLayer();
//builder.Services.AddLogger();

#region Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connString));
// add hangfire server
builder.Services.AddSingleton<IDashboardAuthorizationFilter, HangfireAuthorizationFilter>();
builder.Services.AddHangfireServer();
#endregion


//builder.Services.AddHostedService<CronJobHostedService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowCredentials()
                //.WithOrigins(
                //    "https://localhost:4200")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

});

builder.Services
    .AddMvcCore(option => {
        // add custom filter
        option.Filters.Add(new ValidateModelStateFilter()); // add validate model state filter
     })
.AddApiExplorer(); // add api explorer to expose api metadata



var app = builder.Build();

// Configure the HTTP request pipeline.
bool allowSwagger = app.Environment.IsDevelopment();
if (allowSwagger)
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseDeveloperExceptionPage();

    #region Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
    #endregion

}



app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});


app.MapControllers();

app.Run();
