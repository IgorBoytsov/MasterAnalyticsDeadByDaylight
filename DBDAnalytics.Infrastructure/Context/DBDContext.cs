using System;
using System.Collections.Generic;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.Infrastructure.Context;

public partial class DBDContext : DbContext
{
    public DBDContext()
    {
    }

    public DBDContext(DbContextOptions<DBDContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GameEvent> GameEvents { get; set; }

    public virtual DbSet<GameMode> GameModes { get; set; }

    public virtual DbSet<GameStatistic> GameStatistics { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemAddon> ItemAddons { get; set; }

    public virtual DbSet<Killer> Killers { get; set; }

    public virtual DbSet<KillerAddon> KillerAddons { get; set; }

    public virtual DbSet<KillerBuild> KillerBuilds { get; set; }

    public virtual DbSet<KillerInfo> KillerInfos { get; set; }

    public virtual DbSet<KillerPerk> KillerPerks { get; set; }

    public virtual DbSet<KillerPerkCategory> KillerPerkCategories { get; set; }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<MatchAttribute> MatchAttributes { get; set; }

    public virtual DbSet<Measurement> Measurements { get; set; }

    public virtual DbSet<Offering> Offerings { get; set; }

    public virtual DbSet<OfferingCategory> OfferingCategories { get; set; }

    public virtual DbSet<Patch> Patches { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<PlayerAssociation> PlayerAssociations { get; set; }

    public virtual DbSet<Rarity> Rarities { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Survivor> Survivors { get; set; }

    public virtual DbSet<SurvivorBuild> SurvivorBuilds { get; set; }

    public virtual DbSet<SurvivorInfo> SurvivorInfos { get; set; }

    public virtual DbSet<SurvivorPerk> SurvivorPerks { get; set; }

    public virtual DbSet<SurvivorPerkCategory> SurvivorPerkCategories { get; set; }

    public virtual DbSet<TypeDeath> TypeDeaths { get; set; }

    public virtual DbSet<WhoPlacedMap> WhoPlacedMaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LIGHTPLAY;Database=MasterAnalyticsDeadByDaylightDB;Trusted_Connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameEvent>(entity =>
        {
            entity.HasKey(e => e.IdGameEvent);

            entity.ToTable("GameEvent");

            entity.Property(e => e.IdGameEvent).HasColumnName("id_GameEvent");
        });

        modelBuilder.Entity<GameMode>(entity =>
        {
            entity.HasKey(e => e.IdGameMode);

            entity.ToTable("GameMode");

            entity.Property(e => e.IdGameMode).HasColumnName("id_GameMode");
        });

        modelBuilder.Entity<GameStatistic>(entity =>
        {
            entity.HasKey(e => e.IdGameStatistic);

            entity.ToTable("GameStatistic");

            entity.Property(e => e.IdGameStatistic).HasColumnName("id_GameStatistic");
            entity.Property(e => e.DateTimeMatch).HasColumnType("datetime");
            entity.Property(e => e.IdGameEvent).HasColumnName("id_GameEvent");
            entity.Property(e => e.IdGameMode).HasColumnName("id_GameMode");
            entity.Property(e => e.IdKiller).HasColumnName("id_Killer");
            entity.Property(e => e.IdMap).HasColumnName("id_Map");
            entity.Property(e => e.IdMatchAttribute).HasColumnName("idMatchAttribute");
            entity.Property(e => e.IdPatch).HasColumnName("id_Patch");
            entity.Property(e => e.IdSurvivors1).HasColumnName("id_Survivors_1");
            entity.Property(e => e.IdSurvivors2).HasColumnName("id_Survivors_2");
            entity.Property(e => e.IdSurvivors3).HasColumnName("id_Survivors_3");
            entity.Property(e => e.IdSurvivors4).HasColumnName("id_Survivors_4");
            entity.Property(e => e.IdWhoPlacedMap).HasColumnName("id_WhoPlacedMap");
            entity.Property(e => e.IdWhoPlacedMapWin).HasColumnName("id_WhoPlacedMapWin");

            entity.HasOne(d => d.IdGameEventNavigation).WithMany(p => p.GameStatistics)
                .HasForeignKey(d => d.IdGameEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_GameEvent");

            entity.HasOne(d => d.IdGameModeNavigation).WithMany(p => p.GameStatistics)
                .HasForeignKey(d => d.IdGameMode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_GameMode");

            entity.HasOne(d => d.IdKillerNavigation).WithMany(p => p.GameStatistics)
                .HasForeignKey(d => d.IdKiller)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_KillerInfo");

            entity.HasOne(d => d.IdMapNavigation).WithMany(p => p.GameStatistics)
                .HasForeignKey(d => d.IdMap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_Map");

            entity.HasOne(d => d.IdMatchAttributeNavigation).WithMany(p => p.GameStatistics)
                .HasForeignKey(d => d.IdMatchAttribute)
                .HasConstraintName("FK_GameStatistic_MatchAttribute");

            entity.HasOne(d => d.IdPatchNavigation).WithMany(p => p.GameStatistics)
                .HasForeignKey(d => d.IdPatch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_Patch");

            entity.HasOne(d => d.IdSurvivors1Navigation).WithMany(p => p.GameStatisticIdSurvivors1Navigations)
                .HasForeignKey(d => d.IdSurvivors1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_SurvivorInfo");

            entity.HasOne(d => d.IdSurvivors2Navigation).WithMany(p => p.GameStatisticIdSurvivors2Navigations)
                .HasForeignKey(d => d.IdSurvivors2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_SurvivorInfo1");

            entity.HasOne(d => d.IdSurvivors3Navigation).WithMany(p => p.GameStatisticIdSurvivors3Navigations)
                .HasForeignKey(d => d.IdSurvivors3)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_SurvivorInfo2");

            entity.HasOne(d => d.IdSurvivors4Navigation).WithMany(p => p.GameStatisticIdSurvivors4Navigations)
                .HasForeignKey(d => d.IdSurvivors4)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_SurvivorInfo3");

            entity.HasOne(d => d.IdWhoPlacedMapNavigation).WithMany(p => p.GameStatisticIdWhoPlacedMapNavigations)
                .HasForeignKey(d => d.IdWhoPlacedMap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_WhoPlacedMap");

            entity.HasOne(d => d.IdWhoPlacedMapWinNavigation).WithMany(p => p.GameStatisticIdWhoPlacedMapWinNavigations)
                .HasForeignKey(d => d.IdWhoPlacedMapWin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameStatistic_WhoPlacedMap1");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.IdItem);

            entity.ToTable("Item");

            entity.Property(e => e.IdItem).HasColumnName("id_Item");
        });

        modelBuilder.Entity<ItemAddon>(entity =>
        {
            entity.HasKey(e => e.IdItemAddon);

            entity.ToTable("ItemAddon");

            entity.Property(e => e.IdItemAddon).HasColumnName("id_ItemAddon");
            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.IdRarity).HasColumnName("id_Rarity");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.ItemAddons)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemAddon_Item");

            entity.HasOne(d => d.IdRarityNavigation).WithMany(p => p.ItemAddons)
                .HasForeignKey(d => d.IdRarity)
                .HasConstraintName("FK_ItemAddon_Rarity");
        });

        modelBuilder.Entity<Killer>(entity =>
        {
            entity.HasKey(e => e.IdKiller).HasName("PK_KillerList");

            entity.ToTable("Killer");

            entity.Property(e => e.IdKiller).HasColumnName("id_Killer");
        });

        modelBuilder.Entity<KillerAddon>(entity =>
        {
            entity.HasKey(e => e.IdKillerAddon);

            entity.ToTable("KillerAddon");

            entity.Property(e => e.IdKillerAddon).HasColumnName("id_KillerAddon");
            entity.Property(e => e.IdKiller).HasColumnName("id_Killer");
            entity.Property(e => e.IdRarity).HasColumnName("id_Rarity");

            entity.HasOne(d => d.IdKillerNavigation).WithMany(p => p.KillerAddons)
                .HasForeignKey(d => d.IdKiller)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerAddon_Killer");

            entity.HasOne(d => d.IdRarityNavigation).WithMany(p => p.KillerAddons)
                .HasForeignKey(d => d.IdRarity)
                .HasConstraintName("FK_KillerAddon_Rarity");
        });

        modelBuilder.Entity<KillerBuild>(entity =>
        {
            entity.HasKey(e => e.IdBuild);

            entity.ToTable("KillerBuild");

            entity.Property(e => e.IdBuild).HasColumnName("id_Build");
            entity.Property(e => e.IdAddon1).HasColumnName("id_Addon_1");
            entity.Property(e => e.IdAddon2).HasColumnName("id_Addon_2");
            entity.Property(e => e.IdKiller).HasColumnName("id_Killer");
            entity.Property(e => e.IdPerk1).HasColumnName("id_Perk_1");
            entity.Property(e => e.IdPerk2).HasColumnName("id_Perk_2");
            entity.Property(e => e.IdPerk3).HasColumnName("id_Perk_3");
            entity.Property(e => e.IdPerk4).HasColumnName("id_Perk_4");

            entity.HasOne(d => d.IdAddon1Navigation).WithMany(p => p.KillerBuildIdAddon1Navigations)
                .HasForeignKey(d => d.IdAddon1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerBuild_KillerAddon");

            entity.HasOne(d => d.IdAddon2Navigation).WithMany(p => p.KillerBuildIdAddon2Navigations)
                .HasForeignKey(d => d.IdAddon2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerBuild_KillerAddon1");

            entity.HasOne(d => d.IdKillerNavigation).WithMany(p => p.KillerBuilds)
                .HasForeignKey(d => d.IdKiller)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerBuild_Killer");

            entity.HasOne(d => d.IdPerk1Navigation).WithMany(p => p.KillerBuildIdPerk1Navigations)
                .HasForeignKey(d => d.IdPerk1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerBuild_KillerPerk");

            entity.HasOne(d => d.IdPerk2Navigation).WithMany(p => p.KillerBuildIdPerk2Navigations)
                .HasForeignKey(d => d.IdPerk2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerBuild_KillerPerk1");

            entity.HasOne(d => d.IdPerk3Navigation).WithMany(p => p.KillerBuildIdPerk3Navigations)
                .HasForeignKey(d => d.IdPerk3)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerBuild_KillerPerk2");

            entity.HasOne(d => d.IdPerk4Navigation).WithMany(p => p.KillerBuildIdPerk4Navigations)
                .HasForeignKey(d => d.IdPerk4)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerBuild_KillerPerk3");
        });

        modelBuilder.Entity<KillerInfo>(entity =>
        {
            entity.HasKey(e => e.IdKillerInfo);

            entity.ToTable("KillerInfo");

            entity.Property(e => e.IdKillerInfo).HasColumnName("id_KillerInfo");
            entity.Property(e => e.IdAddon1).HasColumnName("id_Addon_1");
            entity.Property(e => e.IdAddon2).HasColumnName("id_Addon_2");
            entity.Property(e => e.IdAssociation).HasColumnName("id_Association");
            entity.Property(e => e.IdKiller).HasColumnName("id_Killer");
            entity.Property(e => e.IdKillerOffering).HasColumnName("id_KillerOffering");
            entity.Property(e => e.IdPerk1).HasColumnName("id_Perk_1");
            entity.Property(e => e.IdPerk2).HasColumnName("id_Perk_2");
            entity.Property(e => e.IdPerk3).HasColumnName("id_Perk_3");
            entity.Property(e => e.IdPerk4).HasColumnName("id_Perk_4");
            entity.Property(e => e.IdPlatform).HasColumnName("id_Platform");

            entity.HasOne(d => d.IdAddon1Navigation).WithMany(p => p.KillerInfoIdAddon1Navigations)
                .HasForeignKey(d => d.IdAddon1)
                .HasConstraintName("FK_KillerInfo_KillerAddon");

            entity.HasOne(d => d.IdAddon2Navigation).WithMany(p => p.KillerInfoIdAddon2Navigations)
                .HasForeignKey(d => d.IdAddon2)
                .HasConstraintName("FK_KillerInfo_KillerAddon1");

            entity.HasOne(d => d.IdAssociationNavigation).WithMany(p => p.KillerInfos)
                .HasForeignKey(d => d.IdAssociation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerInfo_PlayerAssociation");

            entity.HasOne(d => d.IdKillerNavigation).WithMany(p => p.KillerInfos)
                .HasForeignKey(d => d.IdKiller)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerInfo_Killer");

            entity.HasOne(d => d.IdKillerOfferingNavigation).WithMany(p => p.KillerInfos)
                .HasForeignKey(d => d.IdKillerOffering)
                .HasConstraintName("FK_KillerInfo_Offering");

            entity.HasOne(d => d.IdPerk1Navigation).WithMany(p => p.KillerInfoIdPerk1Navigations)
                .HasForeignKey(d => d.IdPerk1)
                .HasConstraintName("FK_KillerInfo_KillerPerk");

            entity.HasOne(d => d.IdPerk2Navigation).WithMany(p => p.KillerInfoIdPerk2Navigations)
                .HasForeignKey(d => d.IdPerk2)
                .HasConstraintName("FK_KillerInfo_KillerPerk1");

            entity.HasOne(d => d.IdPerk3Navigation).WithMany(p => p.KillerInfoIdPerk3Navigations)
                .HasForeignKey(d => d.IdPerk3)
                .HasConstraintName("FK_KillerInfo_KillerPerk2");

            entity.HasOne(d => d.IdPerk4Navigation).WithMany(p => p.KillerInfoIdPerk4Navigations)
                .HasForeignKey(d => d.IdPerk4)
                .HasConstraintName("FK_KillerInfo_KillerPerk3");

            entity.HasOne(d => d.IdPlatformNavigation).WithMany(p => p.KillerInfos)
                .HasForeignKey(d => d.IdPlatform)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerInfo_Platform");
        });

        modelBuilder.Entity<KillerPerk>(entity =>
        {
            entity.HasKey(e => e.IdKillerPerk);

            entity.ToTable("KillerPerk");

            entity.Property(e => e.IdKillerPerk).HasColumnName("id_KillerPerk");
            entity.Property(e => e.IdCategory).HasColumnName("id_Category");
            entity.Property(e => e.IdKiller).HasColumnName("id_Killer");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.KillerPerks)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_KillerPerk_KillerPerkCategory");

            entity.HasOne(d => d.IdKillerNavigation).WithMany(p => p.KillerPerks)
                .HasForeignKey(d => d.IdKiller)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KillerPerk_Killer");
        });

        modelBuilder.Entity<KillerPerkCategory>(entity =>
        {
            entity.HasKey(e => e.IdKillerPerkCategory);

            entity.ToTable("KillerPerkCategory");

            entity.Property(e => e.IdKillerPerkCategory).HasColumnName("id_KillerPerkCategory");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.IdMap);

            entity.ToTable("Map");

            entity.Property(e => e.IdMap).HasColumnName("id_Map");
            entity.Property(e => e.IdMeasurement).HasColumnName("id_measurement");

            entity.HasOne(d => d.IdMeasurementNavigation).WithMany(p => p.Maps)
                .HasForeignKey(d => d.IdMeasurement)
                .HasConstraintName("FK_Map_Measurement");
        });

        modelBuilder.Entity<MatchAttribute>(entity =>
        {
            entity.HasKey(e => e.IdMatchAttribute);

            entity.ToTable("MatchAttribute");

            entity.Property(e => e.IdMatchAttribute).HasColumnName("id_MatchAttribute");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsHide).HasColumnName("isHide");
        });

        modelBuilder.Entity<Measurement>(entity =>
        {
            entity.HasKey(e => e.IdMeasurement);

            entity.ToTable("Measurement");

            entity.Property(e => e.IdMeasurement).HasColumnName("id_measurement");
            entity.Property(e => e.MeasurementName).HasMaxLength(50);
        });

        modelBuilder.Entity<Offering>(entity =>
        {
            entity.HasKey(e => e.IdOffering);

            entity.ToTable("Offering");

            entity.Property(e => e.IdOffering).HasColumnName("id_Offering");
            entity.Property(e => e.IdCategory).HasColumnName("id_Category");
            entity.Property(e => e.IdRarity).HasColumnName("id_Rarity");
            entity.Property(e => e.IdRole).HasColumnName("id_Role");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Offerings)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_Offering_OfferingCategory");

            entity.HasOne(d => d.IdRarityNavigation).WithMany(p => p.Offerings)
                .HasForeignKey(d => d.IdRarity)
                .HasConstraintName("FK_Offering_Rarity");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Offerings)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offering_Role");
        });

        modelBuilder.Entity<OfferingCategory>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.ToTable("OfferingCategory");

            entity.Property(e => e.IdCategory).HasColumnName("id_Category");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Patch>(entity =>
        {
            entity.HasKey(e => e.IdPatch);

            entity.ToTable("Patch");

            entity.Property(e => e.IdPatch).HasColumnName("id_Patch");
            entity.Property(e => e.PatchNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.IdPlatform);

            entity.ToTable("Platform");

            entity.Property(e => e.IdPlatform).HasColumnName("id_Platform");
            entity.Property(e => e.PlatformName).HasMaxLength(50);
        });

        modelBuilder.Entity<PlayerAssociation>(entity =>
        {
            entity.HasKey(e => e.IdPlayerAssociation);

            entity.ToTable("PlayerAssociation");

            entity.Property(e => e.IdPlayerAssociation).HasColumnName("id_PlayerAssociation");
            entity.Property(e => e.PlayerAssociationName).HasMaxLength(50);
        });

        modelBuilder.Entity<Rarity>(entity =>
        {
            entity.HasKey(e => e.IdRarity);

            entity.ToTable("Rarity");

            entity.Property(e => e.IdRarity).HasColumnName("idRarity");
            entity.Property(e => e.RarityName).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole);

            entity.ToTable("Role");

            entity.Property(e => e.IdRole).HasColumnName("id_Role");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Survivor>(entity =>
        {
            entity.HasKey(e => e.IdSurvivor);

            entity.ToTable("Survivor");

            entity.Property(e => e.IdSurvivor).HasColumnName("id_Survivor");
        });

        modelBuilder.Entity<SurvivorBuild>(entity =>
        {
            entity.HasKey(e => e.IdBuild);

            entity.ToTable("SurvivorBuild");

            entity.Property(e => e.IdBuild).HasColumnName("id_Build");
            entity.Property(e => e.IdAddonItem1).HasColumnName("id_AddonItem_1");
            entity.Property(e => e.IdAddonItem2).HasColumnName("id_AddonItem_2");
            entity.Property(e => e.IdItem).HasColumnName("id_Item");
            entity.Property(e => e.IdPerk1).HasColumnName("id_Perk_1");
            entity.Property(e => e.IdPerk2).HasColumnName("id_Perk_2");
            entity.Property(e => e.IdPerk3).HasColumnName("id_Perk_3");
            entity.Property(e => e.IdPerk4).HasColumnName("id_Perk_4");

            entity.HasOne(d => d.IdAddonItem1Navigation).WithMany(p => p.SurvivorBuildIdAddonItem1Navigations)
                .HasForeignKey(d => d.IdAddonItem1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorBuild_ItemAddon");

            entity.HasOne(d => d.IdAddonItem2Navigation).WithMany(p => p.SurvivorBuildIdAddonItem2Navigations)
                .HasForeignKey(d => d.IdAddonItem2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorBuild_ItemAddon1");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.SurvivorBuilds)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorBuild_Item");

            entity.HasOne(d => d.IdPerk1Navigation).WithMany(p => p.SurvivorBuildIdPerk1Navigations)
                .HasForeignKey(d => d.IdPerk1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorBuild_SurvivorPerk");

            entity.HasOne(d => d.IdPerk2Navigation).WithMany(p => p.SurvivorBuildIdPerk2Navigations)
                .HasForeignKey(d => d.IdPerk2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorBuild_SurvivorPerk1");

            entity.HasOne(d => d.IdPerk3Navigation).WithMany(p => p.SurvivorBuildIdPerk3Navigations)
                .HasForeignKey(d => d.IdPerk3)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorBuild_SurvivorPerk2");

            entity.HasOne(d => d.IdPerk4Navigation).WithMany(p => p.SurvivorBuildIdPerk4Navigations)
                .HasForeignKey(d => d.IdPerk4)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorBuild_SurvivorPerk3");
        });

        modelBuilder.Entity<SurvivorInfo>(entity =>
        {
            entity.HasKey(e => e.IdSurvivorInfo);

            entity.ToTable("SurvivorInfo");

            entity.Property(e => e.IdSurvivorInfo).HasColumnName("id_SurvivorInfo");
            entity.Property(e => e.IdAddon1).HasColumnName("id_Addon_1");
            entity.Property(e => e.IdAddon2).HasColumnName("id_Addon_2");
            entity.Property(e => e.IdAssociation).HasColumnName("id_Association");
            entity.Property(e => e.IdItem).HasColumnName("id_Item");
            entity.Property(e => e.IdPerk1).HasColumnName("id_Perk_1");
            entity.Property(e => e.IdPerk2).HasColumnName("id_Perk_2");
            entity.Property(e => e.IdPerk3).HasColumnName("id_Perk_3");
            entity.Property(e => e.IdPerk4).HasColumnName("id_Perk_4");
            entity.Property(e => e.IdPlatform).HasColumnName("id_Platform");
            entity.Property(e => e.IdSurvivor).HasColumnName("id_Survivor");
            entity.Property(e => e.IdSurvivorOffering).HasColumnName("id_SurvivorOffering");
            entity.Property(e => e.IdTypeDeath).HasColumnName("id_Type_death");

            entity.HasOne(d => d.IdAddon1Navigation).WithMany(p => p.SurvivorInfoIdAddon1Navigations)
                .HasForeignKey(d => d.IdAddon1)
                .HasConstraintName("FK_SurvivorInfo_ItemAddon");

            entity.HasOne(d => d.IdAddon2Navigation).WithMany(p => p.SurvivorInfoIdAddon2Navigations)
                .HasForeignKey(d => d.IdAddon2)
                .HasConstraintName("FK_SurvivorInfo_ItemAddon1");

            entity.HasOne(d => d.IdAssociationNavigation).WithMany(p => p.SurvivorInfos)
                .HasForeignKey(d => d.IdAssociation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorInfo_PlayerAssociation");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.SurvivorInfos)
                .HasForeignKey(d => d.IdItem)
                .HasConstraintName("FK_SurvivorInfo_Item");

            entity.HasOne(d => d.IdPerk1Navigation).WithMany(p => p.SurvivorInfoIdPerk1Navigations)
                .HasForeignKey(d => d.IdPerk1)
                .HasConstraintName("FK_SurvivorInfo_SurvivorPerk");

            entity.HasOne(d => d.IdPerk2Navigation).WithMany(p => p.SurvivorInfoIdPerk2Navigations)
                .HasForeignKey(d => d.IdPerk2)
                .HasConstraintName("FK_SurvivorInfo_SurvivorPerk1");

            entity.HasOne(d => d.IdPerk3Navigation).WithMany(p => p.SurvivorInfoIdPerk3Navigations)
                .HasForeignKey(d => d.IdPerk3)
                .HasConstraintName("FK_SurvivorInfo_SurvivorPerk2");

            entity.HasOne(d => d.IdPerk4Navigation).WithMany(p => p.SurvivorInfoIdPerk4Navigations)
                .HasForeignKey(d => d.IdPerk4)
                .HasConstraintName("FK_SurvivorInfo_SurvivorPerk3");

            entity.HasOne(d => d.IdPlatformNavigation).WithMany(p => p.SurvivorInfos)
                .HasForeignKey(d => d.IdPlatform)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorInfo_Platform");

            entity.HasOne(d => d.IdSurvivorNavigation).WithMany(p => p.SurvivorInfos)
                .HasForeignKey(d => d.IdSurvivor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorInfo_Survivor");

            entity.HasOne(d => d.IdSurvivorOfferingNavigation).WithMany(p => p.SurvivorInfos)
                .HasForeignKey(d => d.IdSurvivorOffering)
                .HasConstraintName("FK_SurvivorInfo_Offering");

            entity.HasOne(d => d.IdTypeDeathNavigation).WithMany(p => p.SurvivorInfos)
                .HasForeignKey(d => d.IdTypeDeath)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorInfo_TypeDeath");
        });

        modelBuilder.Entity<SurvivorPerk>(entity =>
        {
            entity.HasKey(e => e.IdSurvivorPerk);

            entity.ToTable("SurvivorPerk");

            entity.Property(e => e.IdSurvivorPerk).HasColumnName("id_SurvivorPerk");
            entity.Property(e => e.IdCategory).HasColumnName("id_Category");
            entity.Property(e => e.IdSurvivor).HasColumnName("id_Survivor");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.SurvivorPerks)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_SurvivorPerk_SurvivorPerkCategory");

            entity.HasOne(d => d.IdSurvivorNavigation).WithMany(p => p.SurvivorPerks)
                .HasForeignKey(d => d.IdSurvivor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SurvivorPerk_Survivor");
        });

        modelBuilder.Entity<SurvivorPerkCategory>(entity =>
        {
            entity.HasKey(e => e.IdSurvivorPerkCategory);

            entity.ToTable("SurvivorPerkCategory");

            entity.Property(e => e.IdSurvivorPerkCategory).HasColumnName("id_SurvivorPerkCategory");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<TypeDeath>(entity =>
        {
            entity.HasKey(e => e.IdTypeDeath);

            entity.ToTable("TypeDeath");

            entity.Property(e => e.IdTypeDeath).HasColumnName("id_TypeDeath");
            entity.Property(e => e.TypeDeathName).HasMaxLength(50);
        });

        modelBuilder.Entity<WhoPlacedMap>(entity =>
        {
            entity.HasKey(e => e.IdWhoPlacedMap);

            entity.ToTable("WhoPlacedMap");

            entity.Property(e => e.IdWhoPlacedMap).HasColumnName("id_WhoPlacedMap");
            entity.Property(e => e.WhoPlacedMapName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
