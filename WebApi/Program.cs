using Infrastructure;
using Infrastructure.Persistence;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            // Database Initilizer
            await app.Services.AddInitilizeDatabaseAsync();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseInfrastructure();

            app.Run();
        }
    }
}
