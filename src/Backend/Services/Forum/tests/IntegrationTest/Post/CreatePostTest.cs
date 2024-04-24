using System.Security.Cryptography.X509Certificates;
using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Api;
using Application.Requests;
using Application.Requests.Post;
using Domain.Entities;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;
using Infrastructure.Seed;
using Microsoft.Extensions.DependencyInjection;
using Shop.Migrations;

namespace IntegrationTest.Post;

public class CreatePostTest:  PostIntegrationTest
{
    public  CreatePostTest(
        TestFixture<Program, PostDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_create_new_post_to_db()
    {
        var command = new CreatePostRequest()
        {
            GroupId = InitialData.Group.Id,
            User = InitialData.CustomerId,
            Description = "Test2",
            Title = "Test2",
        };
    

        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));

        Assert.Null(exception);
    }
}

