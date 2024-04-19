using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Xunit.Abstractions;

namespace IntegrationTest;

    public class PostIntegrationTest: TestBase<Program,PostDbContext>
    {
        public PostIntegrationTest(TestFixture<Program, PostDbContext> integrationTestFixture, ITestOutputHelper outputHelper = null) : base(integrationTestFixture, outputHelper)
        {
        }
    }
