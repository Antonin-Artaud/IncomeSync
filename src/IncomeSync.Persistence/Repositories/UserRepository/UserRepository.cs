using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using IncomeSync.Core.Shared.Entities.UserEntity;
using IncomeSync.Core.Shared.Exceptions.UserExceptions;
using Microsoft.EntityFrameworkCore;

namespace IncomeSync.Persistence.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _ctx;

    public UserRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<UserResponse> InsertUserAsync(CreateUserRequest createUserRequest)
    {
        var user = await _ctx.Users.AddAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = createUserRequest.Email
        });


        await _ctx.SaveChangesAsync();
        
        return new UserResponse
        {
            Id = user.Entity.Id,
            Email = user.Entity.Email
        };
    }

    public async Task<UserResponse?> FindUserByIdAsync(Guid id)
    {
        var userFound = await _ctx.Users.FirstOrDefaultAsync(_ => _.Id == id);

        if (userFound is null)
        {
            return null;
        }
        
        return new UserResponse
        {
            Id = userFound.Id,
            Email = userFound.Email
        };
    }

    public async Task<UserResponse?> FindUserByMailAsync(string mail)
    {
        var userFound = await _ctx.Users.FirstOrDefaultAsync(_ => _.Email == mail);

        if (userFound is null)
        {
            return null;
        }
        
        return new UserResponse
        {
            Id = userFound.Id,
            Email = userFound.Email
        };
    }

    public Task<UserDeleteResponse> DeleteUserByAsync(CreateUserRequest createUserRequest)
    {
        throw new NotImplementedException();
    }
}