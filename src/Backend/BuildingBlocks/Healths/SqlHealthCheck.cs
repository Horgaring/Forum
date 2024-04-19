using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BuildingBlocks.Healths;

public class SqlHealthCheck : IHealthCheck
{
    private readonly ISqlconnectionfactory _sqlconnectionfactory;

    public SqlHealthCheck(ISqlconnectionfactory sqlconnectionfactory)
    {
        _sqlconnectionfactory = sqlconnectionfactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            using var conn = _sqlconnectionfactory.Create();
            
            conn.Open();
            conn.ExecuteScalarAsync("SELECT 1");

            return HealthCheckResult.Healthy();
        }
        catch (System.Exception e)
        {
            return HealthCheckResult.Unhealthy(exception: e);
        }
    }
}