using AutoMapper;
using Core.Data.Entities;
using Core.Data.Infrastructure;
using Core.Service.Infrastructure;
using Core.Service.Models;
using Core.Service.Services.Base;
using Microsoft.EntityFrameworkCore;
using Scaffolding.Auth;
using Scaffolding.Service.Infrastructure;

namespace Core.Service.Services;

public class UserDetailService : GenericService<UserDetailModel, UserDetail>, IUserDetailService
{
    public UserDetailService(ICoreUnitOfWork coreUnitOfWork, IAuthenticationService authService, IMapper mapper) : base(coreUnitOfWork, authService, mapper)
    {
    }

    public async Task<Response<PaginationSet<UserDetailModel>>> FindUserAsync(string clue, int page, int pageSize)
    {
        var response = InitSuccessResponse(page, pageSize, MessageConstant.Load);
        var result = await _coreUnitOfWork.UserDetailRepository
                        .FindByAsync(x => EF.Functions.Like(x.FirstName, $"%{clue}%") || EF.Functions.Like(x.LastName, $"%{clue}%"),
                        page, pageSize, x => x.User);
        response.Item = GetModelFromEntity(result);
        return response;

    }

    public async Task<Response<UserDetailModel>> GetDetailAsync(Guid userId)
    {
        var response = InitErrorResponse();
        var result = await _coreUnitOfWork.UserDetailRepository.GetSingleAsync(x => x.User.Id == userId, includeProperties: x => x.User);
        
        if(result == null)
        {
            response.Message = MessageConstant.NotFound;
            return response;
        }

        response = InitSuccessResponse(MessageConstant.Load);
        response.Item = GetModelFromEntity(result);
        return response;
    }
}