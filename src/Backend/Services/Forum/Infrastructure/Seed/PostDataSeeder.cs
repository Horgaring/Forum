
using BuildingBlocks.TestBase;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.Seed;

/// <summary>
/// Data Seeder for Post
/// </summary>
public class PostDataSeeder : IDataSeeder
{
    private readonly PostDbContext _dbContext;

    public PostDataSeeder(PostDbContext Db)
    {
        _dbContext = Db;
    }

    public async Task SeedAllAsync()
    {
        await SeedcustomerAsync();
        await _dbContext.SaveChangesAsync();
    }

    

    private async Task SeedcustomerAsync()
    {
        var cusromerId = InitialData.CustomerId;
        var group = InitialData.Group;
        var post = InitialData.Post;
        group.Owner = cusromerId;
        group.Followers = new List<CustomerId>(){cusromerId};
        post.User = cusromerId;
        post.Group = group;
        _dbContext.CustomersId.Add(cusromerId);
        _dbContext.Groups.Add(group);
        _dbContext.Post.Add(post);
    }
    

    
}


