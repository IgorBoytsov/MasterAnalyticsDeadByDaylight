using DBDAnalytics.CatalogService.Application.AutoMappers.Profiles;
using DBDAnalytics.CatalogService.Application.Features.Killers.Create;
using DBDAnalytics.CatalogService.Application.Ioc;
using DBDAnalytics.CatalogService.Infrastructure.Ioc;

namespace DBDAnalytics.CatalogService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateKillerCommand).Assembly));
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = builder.Configuration["AutoMapper:AutoMapperKey"];
            }, typeof(KillerProfile).Assembly);

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);


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