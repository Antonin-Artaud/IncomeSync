﻿using IncomeSync.Core.Shared.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace IncomeSync.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; init; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}