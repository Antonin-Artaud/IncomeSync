using IncomeSync.Core.Shared.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace IncomeSync.Persistence.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _ctx;

    public UserRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task InsertUserAsync(UserEntity userEntity)
    {
        await _ctx.Users.AddAsync(userEntity);
        await _ctx.SaveChangesAsync();
    }

    public async Task<UserEntity?> FindUserByIdAsync(Guid id)
    {
        return await _ctx.Users.FirstOrDefaultAsync(_ => _.Id == id);
    }

    public async Task<UserEntity?> FindUserByMailAsync(string email)
    {
        return await _ctx.Users.FirstOrDefaultAsync(_ => _.Email == email);
    }

    public Task DeleteUserByAsync(UserEntity userEntity)
    {
        throw new NotImplementedException();
    }
}