using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.AppOptions;
using NashFridayStore.Infrastructure.Data;

// Disable parallel parallel test conflict with 1 sqlite database
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace NashFridayStore.IntegrationTests.Commons;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(AppCts.Environment.Testing);

        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<StoreDbContext>));

            services.Remove(dbContextDescriptor!);

            ServiceDescriptor? dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor!);

            // Remove the DbContext service itself to avoid duplicate registrations
            // If not, the EF for sqlserver is used, but we want sqlite

            // Register sqlite in-memory connection
            services.AddSingleton<DbConnection>((service) =>
            {
                string sqliteTestingConnectionString = service.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.DatabaseTesting;
                var connection = new SqliteConnection(sqliteTestingConnectionString);
                connection.Open();

                return connection;
            });

            // Register DbContext using Sqlite
            services.AddDbContext<StoreDbContext>((service, options) =>
            {
                DbConnection connection = service.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);

                // Ignore migrations pending warning for SQLite in-memory testing, as we will ensure the database is created before each test
                options.ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
        });
    }
}
