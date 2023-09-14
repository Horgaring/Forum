using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.Seed;

public class PostDataSeeder : IDataSeeder
{
    private readonly PostDbContext _dbContext;

    public PostDataSeeder(PostDbContext Db)
    {
        _dbContext = Db;
    }

    public async Task SeedAllAsync()
    {
        await SeedPostAsync();
    }

    private async Task SeedPostAsync()
    {
        if (!await _dbContext.Post.AnyAsync())
        {
            await _dbContext.Post.AddRangeAsync(InitialData.Post);
            await _dbContext.SaveChangesAsync();
            
        }
    }

}


