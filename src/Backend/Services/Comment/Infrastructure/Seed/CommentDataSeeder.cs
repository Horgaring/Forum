using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public class CommentDataSeeder : IDataSeeder
{
    private readonly CommentDbContext _dbContext;

    public CommentDataSeeder(CommentDbContext Db)
    {
        _dbContext = Db;
    }

    public async Task SeedAllAsync()
    {
        await SeedCommentAsync();
    }

    private async Task SeedCommentAsync()
    {
        
            await _dbContext.Comment.AddAsync(cInitialData.Comment);
            await _dbContext.SaveChangesAsync();
            
        
    }

}

