using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Extensions
{
    public static class MigrationBuilderExtensions
    {
        /// <summary>
        /// Создает View из вложенного SQL-ресурса.
        /// </summary>
        public static void CreateView(this MigrationBuilder migrationBuilder, string viewName)
        {
            var resourcePrefix = "DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Read.Sql.Views";
            var resourceName = $"{resourcePrefix}.{viewName}.sql";

            var sqlScript = GetSqlFromResource(resourceName);
            migrationBuilder.Sql(sqlScript);
        }

        /// <summary>
        /// Удаляет View.
        /// </summary>
        public static void DropView(this MigrationBuilder migrationBuilder, string viewName)
            => migrationBuilder.Sql($@"DROP VIEW IF EXISTS public.{viewName};");

        private static string GetSqlFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Не удалось найти встроенный ресурс: {resourceName}");

            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}