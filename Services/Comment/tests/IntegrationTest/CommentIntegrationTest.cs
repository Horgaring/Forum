using Api;
using BuildingBlocks.TestBase;
using Infrastructure.Context;

using Xunit.Abstractions;

namespace IntegrationTest;

    public class CommentIntegrationTest: TestBase<Program,CommentDbContext>
    {
        public CommentIntegrationTest(TestFixture<Program, CommentDbContext> integrationTestFixture, ITestOutputHelper outputHelper = null) : base(integrationTestFixture, outputHelper)
        {
        }
    }
