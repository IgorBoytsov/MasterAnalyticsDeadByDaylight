
using DBDAnalytics.MatchService.Application.Abstractions.Common;
using DBDAnalytics.MatchService.Application.Features.Matches.Create;
using DBDAnalytics.MatchService.Infrastructure.Ioc;
using DBDAnalytics.MatchService.Infrastructure.Persistence.Contexts;

namespace DBDAnalytics.MatchService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateMatchCommand).Assembly));
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<WriteDbContext>());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
