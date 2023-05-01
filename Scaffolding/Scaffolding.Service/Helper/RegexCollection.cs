namespace Scaffolding.Service.Helper;

public static class RegexCollection
{
    public const string NOT_ALLOW_SPECIAL_CHAR = @"^[a-zA-Z''-'\s]{1,40}$";
    public const string URL_FORMAT_ALLOWED = @"^[a-zA-Z0-9-]+$";
}
