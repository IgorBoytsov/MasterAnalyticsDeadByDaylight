using DBDAnalytics.MatchService.Application.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Read
{
    internal sealed class MatchDetailsReadConfiguration : IEntityTypeConfiguration<MatchDetailsView>
    {
        public void Configure(EntityTypeBuilder<MatchDetailsView> builder)
        {
            //builder.ToView("V_MatchDetails");
            //builder.HasKey(m => m.MatchId);

            // 1. Указываем, что эта сущность сопоставлена с представлением (VIEW) и имеет первичный ключ.
            builder.ToView("v_match_details");
            builder.HasKey(m => m.MatchId);

            // 2. Конфигурируем свойство 'KillerDetails'
            //    Это одиночный вложенный объект, который хранится в JSON-колонке.
            builder.OwnsOne(m => m.KillerDetails, ownedBuilder =>
            {
                // .ToJson() указывает EF Core сериализовать/десериализовать весь объект KillerDetails
                // из колонки "KillerDetails" в базе данных.
                ownedBuilder.ToJson();
            });

            // 3. Конфигурируем свойство 'SurvivorDetails'
            //    Это коллекция вложенных объектов, которая хранится в JSON-колонке.
            builder.OwnsMany(m => m.SurvivorDetails, ownedBuilder =>
            {
                // 3a. ВАЖНЫЙ ШАГ: Внутри конфигурации для SurvivorDetails мы должны указать,
                //     что его свойство 'Item' также является вложенным (owned) типом.
                //     Без этой строки EF Core будет считать 'SurvivorItem' отдельной сущностью и требовать ключ.
                ownedBuilder.OwnsOne(survivorDetail => survivorDetail.Item);

                // 3b. .ToJson() указывает EF Core сериализовать/десериализовать всю коллекцию SurvivorDetails
                //     из колонки "SurvivorDetails" в базе данных. Эта команда применится ко всему графу
                //     объекта SurvivorDetails, включая его вложенный объект Item.
                ownedBuilder.ToJson();
            });
        }
    }
}