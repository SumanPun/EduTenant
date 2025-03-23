using Infrastructure;
using Infrastructure.Persistence;
using Application;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddApplicationServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            // Database Initilizer
            await app.Services.AddInitilizeDatabaseAsync();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.MapControllers();

            app.UseInfrastructure();

            app.Run();
        }
    }
}
