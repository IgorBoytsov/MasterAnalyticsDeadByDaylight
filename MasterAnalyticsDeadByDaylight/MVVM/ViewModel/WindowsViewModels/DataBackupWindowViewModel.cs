using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class DataBackupWindowViewModel : BaseViewModel
    {

        #region Свойства
 
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Backup> BackupsTable { get; set; } = [];

        private List<GameStatistic> GameStatisticsTable { get; set; } = [];

        private List<KillerInfo> KillerInfosTable { get; set; } = [];

        private List<Killer> KillersTable { get; set; } = [];

        private List<KillerPerk> KillerPerksTable {  get; set; } = [];

        private List<KillerAddon> KillersAddonsTable {  get; set; } = [];

        private List<SurvivorInfo> SurvivorInfosTable {  get; set; } = [];

        private List<Survivor> SurvivorsTable { get; set; } = [];

        private List<SurvivorPerk> SurvivorPerksTable { get; set; } = [];

        private List<Item> ItemsTable { get; set; } = [];

        private List<ItemAddon> ItemAddonsTable { get; set; } = [];

        private List<Offering> OfferingsTable { get; set; } = [];

        private List<Rarity> RarityTable { get; set; } = [];

        private List<Role> RoleTable { get; set; } = [];

        private List<Map> MapsTable { get; set; } = [];

        private List<Measurement> MeasurementsTable { get; set; } = [];

        private List<WhoPlacedMap> WhoPlacedMapsTable { get; set; } = [];

        private List<Patch> PatchsTable { get; set; } = [];

        private List<GameMode> GameModesTable { get; set; } = [];

        private List<GameEvent> GameEventsTable { get; set; } = [];

        private List<TypeDeath> TypeDeathTable { get; set; } = [];

        private List<PlayerAssociation> PlayerAssociationsTable { get; set; } = [];

        private List<Platform> PlatformsTable { get; set; } = [];

        private Backup _selectedBackup;
        public Backup SelectedBackup
        {
            get => _selectedBackup;
            set
            {
                _selectedBackup = value;
                OnPropertyChanged();
            }
        }

        private string _saveFilePath;
        public string SaveFilePath
        {
            get => _saveFilePath;
            set
            {
                _saveFilePath = value;
                OnPropertyChanged();
            }
        }

        private bool _jsonCheckBox;
        public bool JsonCheckBox
        {
            get => _jsonCheckBox;
            set
            {
                if (_jsonCheckBox != value)
                {
                    _jsonCheckBox = value;
                    if (XMLCheckBox == true)
                    {
                        XMLCheckBox = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private bool _xmlCheckBox;
        public bool XMLCheckBox
        {
            get => _xmlCheckBox;
            set
            {
                if (_xmlCheckBox != value)
                {
                    _xmlCheckBox = value;
                    if (JsonCheckBox == true)
                    {
                        JsonCheckBox = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private bool _excelCheckBox;
        public bool ExcelCheckBox
        {
            get => _excelCheckBox;
            set
            {
                if (_excelCheckBox != value)
                {
                    _excelCheckBox = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public DataBackupWindowViewModel()
        {
            Title = "Создание копии данных";
            FillBackupsList();
        }

        #region Команды 
        
        private RelayCommand _getPathDirectoryCommand;
        public RelayCommand GetPathDirectoryCommand { get => _getPathDirectoryCommand ??= new(obj => { GetPathDirectory(); });} 
        
        private RelayCommand _selectAllCommand;
        public RelayCommand SelectAllCommand { get => _selectAllCommand ??= new(obj => { SelectAll(); });} 
        
        private RelayCommand _unselectAllCommand;
        public RelayCommand UnselectAllCommand { get => _unselectAllCommand ??= new(obj => { UnselectAll(); });} 
        
        private RelayCommand _startSelectedBackupCommand;
        public RelayCommand StartSelectedBackupCommand { get => _startSelectedBackupCommand ??= new(async obj => 
        {
            await GetData();
            await StartBackupAsync();
        });}

        private RelayCommand _selectCheckBoxCommand;
        public RelayCommand SelectCheckBoxCommand => _selectCheckBoxCommand ??= new RelayCommand(SelectBackupParameter);

        #endregion

        #region Получение пути к каталогу

        private void GetPathDirectory()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Выберите папку для сохранения файлов";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SaveFilePath = dialog.SelectedPath;
                }
            }
        }

        #endregion

        #region Создание файлов

        private void SelectBackupParameter(object parameter)
        {
            if (parameter is Backup backup)
            {
                if (backup.Status == "Выбрано")
                {
                    UpdateStatusEmpty(backup);
                }
                else
                {
                    UpdateStatusSelected(backup);
                }
            }
        }

        private async Task StartBackupAsync()
        {
            if (string.IsNullOrWhiteSpace(SaveFilePath))
            {
                System.Windows.MessageBox.Show("Не указан путь сохранения");
            }
            if (JsonCheckBox == true)
            {       
                await StartSelectedJsonBackupAsync();
            }
            if (XMLCheckBox == true)
            {           
                await StartSelectedXMLBackupAsync();
            }
        }

        private async Task StartSelectedJsonBackupAsync()
        {
            var backupActions = new Dictionary<string, Func<Task>>
            {
                { "Общая статистика", () => FileHelper.CreateJsonBackupFileAsync(GameStatisticsTable, SaveFilePath + @"\Общая статистика.json") },
                { "Информация по убийце", () => FileHelper.CreateJsonBackupFileAsync(KillerInfosTable, SaveFilePath + @"\Информация по убийце.json") },
                { "Список убийц", () => FileHelper.CreateJsonBackupFileAsync(KillersTable, SaveFilePath + @"\Список убийц.json") },
                { "Перки убийцы", () => FileHelper.CreateJsonBackupFileAsync(KillerPerksTable, SaveFilePath + @"\Перки убийцы.json") },
                { "Аддоны убийц", () => FileHelper.CreateJsonBackupFileAsync(KillersAddonsTable, SaveFilePath + @"\Аддоны убийц.json") },
                { "Информация по выжившем", () => FileHelper.CreateJsonBackupFileAsync(SurvivorInfosTable, SaveFilePath + @"\Информация по выжившем.json") },
                { "Список выживших", () => FileHelper.CreateJsonBackupFileAsync(SurvivorsTable, SaveFilePath + @"\Список выживших.json") },
                { "Перки выживших", () => FileHelper.CreateJsonBackupFileAsync(SurvivorPerksTable, SaveFilePath + @"\Перки выживших.json") },
                { "Предметы", () => FileHelper.CreateJsonBackupFileAsync(ItemsTable, SaveFilePath + @"\Предметы.json") },
                { "Аддоны для предметов", () => FileHelper.CreateJsonBackupFileAsync(ItemAddonsTable, SaveFilePath + @"\Аддоны для предметов.json") },
                { "Подношение", () => FileHelper.CreateJsonBackupFileAsync(OfferingsTable, SaveFilePath + @"\Подношение.json") },
                { "Редкость", () => FileHelper.CreateJsonBackupFileAsync(RarityTable, SaveFilePath + @"\Редкость.json") },
                { "Игровая роль", () => FileHelper.CreateJsonBackupFileAsync(RoleTable, SaveFilePath + @"\Игровая роль.json") },
                { "Список карт", () => FileHelper.CreateJsonBackupFileAsync(MapsTable, SaveFilePath + @"\Список карт.json") },
                { "Измерение для карт", () => FileHelper.CreateJsonBackupFileAsync(MeasurementsTable, SaveFilePath + @"\Измерение для карт.json") },
                { "Кто поставил карту", () => FileHelper.CreateJsonBackupFileAsync(WhoPlacedMapsTable, SaveFilePath + @"\Кто поставил карту.json") },
                { "Патч", () => FileHelper.CreateJsonBackupFileAsync(PatchsTable, SaveFilePath + @"\Патч.json") },
                { "Игровой режим", () => FileHelper.CreateJsonBackupFileAsync(GameModesTable, SaveFilePath + @"\Игровой режим.json") },
                { "Игровой ивент", () => FileHelper.CreateJsonBackupFileAsync(GameEventsTable, SaveFilePath + @"\Игровой ивент.json") },
                { "Тип смерти", () => FileHelper.CreateJsonBackupFileAsync(TypeDeathTable, SaveFilePath + @"Тип смерти.json") },
                { "Игровая ассоциация", () => FileHelper.CreateJsonBackupFileAsync(PlayerAssociationsTable, SaveFilePath + @"\Игровая ассоциация.json") },
                { "Платформа", () => FileHelper.CreateJsonBackupFileAsync(PlatformsTable, SaveFilePath + @"\Платформа.json") }
            };

            foreach (var item in BackupsTable)
            {
                if (item.IsCheck)
                {
                    UpdateStatusInProcess(item);
                    if (backupActions.TryGetValue(item.Name, out var action))
                    {
                        await action();
                        UpdateStatusDone(item);
                    }
                }
            }
        }

        private async Task StartSelectedXMLBackupAsync()
        {
            var backupActions = new Dictionary<string, Func<Task>>
            {
                { "Общая статистика", () => FileHelper.CreateXmlBackupFileAsync(GameStatisticsTable, SaveFilePath + @"\Общая статистика.xml") },
                { "Информация по убийце", () => FileHelper.CreateXmlBackupFileAsync(KillerInfosTable, SaveFilePath + @"\Информация по убийце.xml") },
                { "Список убийц", () => FileHelper.CreateXmlBackupFileAsync(KillersTable, SaveFilePath + @"\Список убийц.xml") },
                { "Перки убийцы", () => FileHelper.CreateXmlBackupFileAsync(KillerPerksTable, SaveFilePath + @"\Перки убийцы.xml") },
                { "Аддоны убийц", () => FileHelper.CreateXmlBackupFileAsync(KillersAddonsTable, SaveFilePath + @"\Аддоны убийц.xml") },
                { "Информация по выжившем", () => FileHelper.CreateXmlBackupFileAsync(SurvivorInfosTable, SaveFilePath + @"\Информация по выжившем.xml") },
                { "Список выживших", () => FileHelper.CreateXmlBackupFileAsync(SurvivorsTable, SaveFilePath + @"\Список выживших.xml") },
                { "Перки выживших", () => FileHelper.CreateXmlBackupFileAsync(SurvivorPerksTable, SaveFilePath + @"\Перки выживших.xml") },
                { "Предметы", () => FileHelper.CreateXmlBackupFileAsync(ItemsTable, SaveFilePath + @"\Предметы.xml") },
                { "Аддоны для предметов", () => FileHelper.CreateXmlBackupFileAsync(ItemAddonsTable, SaveFilePath + @"\Аддоны для предметов.xml") },
                { "Подношение", () => FileHelper.CreateXmlBackupFileAsync(OfferingsTable, SaveFilePath + @"\Подношение.xml") },
                { "Редкость", () => FileHelper.CreateXmlBackupFileAsync(RarityTable, SaveFilePath + @"\Редкость.xml") },
                { "Игровая роль", () => FileHelper.CreateXmlBackupFileAsync(RoleTable, SaveFilePath + @"\Игровая роль.xml") },
                { "Список карт", () => FileHelper.CreateXmlBackupFileAsync(MapsTable, SaveFilePath + @"\Список карт.xml") },
                { "Измерение для карт", () => FileHelper.CreateXmlBackupFileAsync(MeasurementsTable, SaveFilePath + @"\Измерение для карт.xml") },
                { "Кто поставил карту", () => FileHelper.CreateXmlBackupFileAsync(WhoPlacedMapsTable, SaveFilePath + @"\Кто поставил карту.xml") },
                { "Патч", () => FileHelper.CreateXmlBackupFileAsync(PatchsTable, SaveFilePath + @"\Патч.xml") },
                { "Игровой режим", () => FileHelper.CreateXmlBackupFileAsync(GameModesTable, SaveFilePath + @"\Игровой режим.xml") },
                { "Игровой ивент", () => FileHelper.CreateXmlBackupFileAsync(GameEventsTable, SaveFilePath + @"\Игровой ивент.xml") },
                { "Тип смерти", () => FileHelper.CreateXmlBackupFileAsync(TypeDeathTable, SaveFilePath + @"Тип смерти.xml") },
                { "Игровая ассоциация", () => FileHelper.CreateXmlBackupFileAsync(PlayerAssociationsTable, SaveFilePath + @"\Игровая ассоциация.xml") },
                { "Платформа", () => FileHelper.CreateXmlBackupFileAsync(PlatformsTable, SaveFilePath + @"\Платформа.xml") }
            };

            foreach (var item in BackupsTable)
            {
                if (item.IsCheck)
                {
                    UpdateStatusInProcess(item);
                    if (backupActions.TryGetValue(item.Name, out var action))
                    {
                        await action();
                        UpdateStatusDone(item);
                    }
                }
            }
        }

        #endregion

        #region Обновление статуса

        private void SelectAll()
        {
            foreach (var item in BackupsTable)
            {
                item.IsCheck = true;
                UpdateStatusSelected(item);
            }
        }

        private void UnselectAll()
        {
            foreach (var item in BackupsTable)
            {
                item.IsCheck = false;
                UpdateStatusEmpty(item);
            }
        }

        private void UpdateStatusEmpty(Backup backup)
        {
            var entityToUpdate = BackupsTable.FirstOrDefault(x => x.Name == backup.Name);
            if (entityToUpdate != null)
            {
                entityToUpdate.Status = string.Empty;
                entityToUpdate.IsCheck = false;
            }
        }

        private void UpdateStatusSelected(Backup backup)
        {
            var entityToUpdate = BackupsTable.FirstOrDefault(x => x.Name == backup.Name);
            if (entityToUpdate != null)
            {
                entityToUpdate.Status = "Выбрано";
                entityToUpdate.IsCheck = true;
            }
        }

        private void UpdateStatusInProcess(Backup backup)
        {
            var entityToUpdate = BackupsTable.FirstOrDefault(x => x.Name == backup.Name);
            if (entityToUpdate != null)
            {
                entityToUpdate.Status = "В процессе...";
            }
        }

        private void UpdateStatusDone(Backup backup)
        {
            var entityToUpdate = BackupsTable.FirstOrDefault(x => x.Name == backup.Name);
            if (entityToUpdate != null)
            {
                entityToUpdate.Status = "Выполнено";
                entityToUpdate.IsCheck = true;
            }
        }

        #endregion

        #region Получение / заполнение списков

        private void FillBackupsList()
        {
            BackupsTable.Add( new Backup { IsCheck = false, Name = "Общая статистика" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Информация по убийце" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Список убийц" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Перки убийцы" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Аддоны убийц" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Информация по выжившем" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Список выживших" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Перки выживших" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Предметы" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Аддоны для предметов" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Подношение" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Редкость" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Игровая роль" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Список карт" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Измерение для карт" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Кто поставил карту" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Патч" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Игровой режим" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Игровой ивент" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Тип смерти" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Игровая ассоциация" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Платформа" });
        }

        private async Task GetData()
        {
            await GetGameStatisticData();

            await GetKillerInfoData();
            await GetKillerData();
            await GetKillerPerkData();
            await GetKillerAddonData();

            await GetSurvivorInfoData();
            await GetSurvivorData();
            await GetSurvivorPerkData();

            await GetItemData();
            await GetItemAddonData();

            await GetOfferingData();
            await GetRarityData();
            await GetRoleData();

            await GetMapData();
            await GetMeasurementData();
            await GetWhoPlacedMapData();
            
            await GetPatchData();
            await GetGameModeData();
            await GetGameEventData();

            await GetTypeDeathsData();
            await GetPlayerAssociationData();
            await GetPlatformData();
        }

        private async Task GetGameStatisticData()
        {
            GameStatisticsTable.Clear();
            GameStatisticsTable = await GameStatisticData();
        }

        private async Task GetKillerInfoData()
        {
            KillerInfosTable.Clear();
            KillerInfosTable = await KillerInfoData();
        }

        private async Task GetKillerData()
        {
            KillersTable.Clear();
            KillersTable = await KillerData();
        }

        private async Task GetKillerPerkData()
        {
            KillerPerksTable.Clear();
            KillerPerksTable = await KillerPerkData();
        }

        private async Task GetKillerAddonData()
        {
            KillersAddonsTable.Clear();
            KillersAddonsTable = await KillerAddonData();
        }

        private async Task GetSurvivorInfoData()
        {
            SurvivorInfosTable.Clear();
            SurvivorInfosTable = await SurvivorInfoData();
        }

        private async Task GetSurvivorData()
        {
            SurvivorsTable.Clear();
            SurvivorsTable = await SurvivorData();
        }

        private async Task GetSurvivorPerkData()
        {
            SurvivorPerksTable.Clear();
            SurvivorPerksTable = await SurvivorPerkData();
        }

        private async Task GetItemData()
        {
            ItemsTable.Clear();
            ItemsTable = await ItemData();
        }

        private async Task GetItemAddonData()
        {
            ItemAddonsTable.Clear();
            ItemAddonsTable = await ItemAddonData();
        }

        private async Task GetOfferingData()
        {
            OfferingsTable.Clear();
            OfferingsTable = await OfferingData();
        }

        private async Task GetRarityData()
        {
            RarityTable.Clear();
            RarityTable = await RarityData();
        }

        private async Task GetRoleData()
        {
            RoleTable.Clear();
            RoleTable = await RoleData();
        }

        private async Task GetMapData()
        {
            MapsTable.Clear();
            MapsTable = await MapData();
        }

        private async Task GetMeasurementData()
        {
            MeasurementsTable.Clear();
            MeasurementsTable = await MeasurementData();
        }

        private async Task GetWhoPlacedMapData()
        {
            WhoPlacedMapsTable.Clear();
            WhoPlacedMapsTable = await WhoPlacedMapData();
        }

        private async Task GetPatchData()
        {
            PatchsTable.Clear();
            PatchsTable = await PatchData();
        }

        private async Task GetGameModeData()
        {
            GameModesTable.Clear();
            GameModesTable = await GameModeData();
        } 
        
        private async Task GetGameEventData()
        {
            GameEventsTable.Clear();
            GameEventsTable = await GameEventData();
        }

        private async Task GetTypeDeathsData()
        {
            TypeDeathTable.Clear();
            TypeDeathTable = await TypeDeathData();
        }

        private async Task GetPlayerAssociationData()
        {
            PlayerAssociationsTable.Clear();
            PlayerAssociationsTable = await PlayerAssociationData();
        }

        private async Task GetPlatformData()
        {
            PlatformsTable.Clear();
            PlatformsTable = await PlatformData();
        }

        #endregion

        #region Методы получение данных

        public static async Task<List<GameStatistic>> GameStatisticData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var gameStatistic = await context.GameStatistics.ToListAsync();

                return gameStatistic;
            }
        }

        public static async Task<List<KillerInfo>> KillerInfoData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var killerInfos = await context.KillerInfos.ToListAsync();

                return killerInfos;
            }
        }

        public static async Task<List<Killer>> KillerData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var killers = await context.Killers.ToListAsync();

                return killers;
            }
        }

        public static async Task<List<KillerPerk>> KillerPerkData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var killerPerks = await context.KillerPerks.ToListAsync();

                return killerPerks;
            }
        }

        public static async Task<List<KillerAddon>> KillerAddonData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var killerAddons = await context.KillerAddons.ToListAsync();

                return killerAddons;
            }
        }

        public static async Task<List<SurvivorInfo>> SurvivorInfoData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var survivorInfos = await context.SurvivorInfos.ToListAsync();

                return survivorInfos;
            }
        }

        public static async Task<List<Survivor>> SurvivorData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var survivors = await context.Survivors.ToListAsync();

                return survivors;
            }
        }

        public static async Task<List<SurvivorPerk>> SurvivorPerkData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var survivorPerks = await context.SurvivorPerks.ToListAsync();

                return survivorPerks;
            }
        }

        public static async Task<List<Item>> ItemData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var items = await context.Items.ToListAsync();

                return items;
            }
        }

        public static async Task<List<ItemAddon>> ItemAddonData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var itemAddons = await context.ItemAddons.ToListAsync();

                return itemAddons;
            }
        }

        public static async Task<List<Offering>> OfferingData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var offering = await context.Offerings.ToListAsync();

                return offering;
            }
        }

        public static async Task<List<Rarity>> RarityData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var rarities = await context.Rarities.ToListAsync();

                return rarities;
            }
        }

        public static async Task<List<Role>> RoleData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var roles = await context.Roles.ToListAsync();

                return roles;
            }
        }

        public static async Task<List<Map>> MapData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var maps = await context.Maps.ToListAsync();

                return maps;
            }
        }

        public static async Task<List<Measurement>> MeasurementData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var measurements = await context.Measurements.ToListAsync();

                return measurements;
            }
        }

        public static async Task<List<WhoPlacedMap>> WhoPlacedMapData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var whoPlacedMaps = await context.WhoPlacedMaps.ToListAsync();

                return whoPlacedMaps;
            }
        }

        public static async Task<List<Patch>> PatchData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var patches = await context.Patches.ToListAsync();

                return patches;
            }
        }

        public static async Task<List<GameMode>> GameModeData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var gameModes = await context.GameModes.ToListAsync();

                return gameModes;
            }
        }

        public static async Task<List<GameEvent>> GameEventData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var gameEvents = await context.GameEvents.ToListAsync();

                return gameEvents;
            }
        }

        public static async Task<List<TypeDeath>> TypeDeathData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var typeDeaths = await context.TypeDeaths.ToListAsync();

                return typeDeaths;
            }
        }

        public static async Task<List<PlayerAssociation>> PlayerAssociationData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var playerAssociations = await context.PlayerAssociations.ToListAsync();

                return playerAssociations;
            }
        }

        public static async Task<List<Platform>> PlatformData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var platforms = await context.Platforms.ToListAsync();

                return platforms;
            }
        }

        #endregion

    }
}
