using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
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

        private List<KillerBuild> KillersBuildsTable {  get; set; } = [];

        private List<SurvivorInfo> SurvivorInfosTable {  get; set; } = [];

        private List<Survivor> SurvivorsTable { get; set; } = [];

        private List<SurvivorBuild> SurvivorBuildsTable { get; set; } = [];

        private List<SurvivorPerk> SurvivorPerksTable { get; set; } = [];

        private List<Item> ItemsTable { get; set; } = [];

        private List<ItemAddon> ItemAddonsTable { get; set; } = [];

        private List<Offering> OfferingsTable { get; set; } = [];

        private List<OfferingCategory> OfferingsCategoriesTable { get; set; } = [];

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

        private readonly ICustomDialogService _dialogService;
        private readonly IDataService _dataService;

        public DataBackupWindowViewModel(ICustomDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;
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
                _dialogService.ShowMessage("Не указан путь сохранения");
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
                { "Билды убийцы", () => FileHelper.CreateJsonBackupFileAsync(KillersBuildsTable, SaveFilePath + @"\Билды убийцы.json") },
                { "Информация по выжившем", () => FileHelper.CreateJsonBackupFileAsync(SurvivorInfosTable, SaveFilePath + @"\Информация по выжившем.json") },
                { "Список выживших", () => FileHelper.CreateJsonBackupFileAsync(SurvivorsTable, SaveFilePath + @"\Список выживших.json") },
                { "Перки выживших", () => FileHelper.CreateJsonBackupFileAsync(SurvivorPerksTable, SaveFilePath + @"\Перки выживших.json") },
                { "Билды выжившего", () => FileHelper.CreateJsonBackupFileAsync(SurvivorBuildsTable, SaveFilePath + @"\Билды выжившего.json") },
                { "Предметы", () => FileHelper.CreateJsonBackupFileAsync(ItemsTable, SaveFilePath + @"\Предметы.json") },
                { "Аддоны для предметов", () => FileHelper.CreateJsonBackupFileAsync(ItemAddonsTable, SaveFilePath + @"\Аддоны для предметов.json") },
                { "Подношение", () => FileHelper.CreateJsonBackupFileAsync(OfferingsTable, SaveFilePath + @"\Подношение.json") },
                { "Категории подношений", () => FileHelper.CreateJsonBackupFileAsync(OfferingsCategoriesTable, SaveFilePath + @"\Категории Подношений.json") },
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
                { "Билды убийцы", () => FileHelper.CreateXmlBackupFileAsync(KillersBuildsTable, SaveFilePath + @"\Билды убийцы.xml") },
                { "Информация по выжившем", () => FileHelper.CreateXmlBackupFileAsync(SurvivorInfosTable, SaveFilePath + @"\Информация по выжившем.xml") },
                { "Список выживших", () => FileHelper.CreateXmlBackupFileAsync(SurvivorsTable, SaveFilePath + @"\Список выживших.xml") },
                { "Перки выживших", () => FileHelper.CreateXmlBackupFileAsync(SurvivorPerksTable, SaveFilePath + @"\Перки выживших.xml") },
                { "Билды выжившего", () => FileHelper.CreateXmlBackupFileAsync(SurvivorBuildsTable, SaveFilePath + @"\Билды выжившего.xml") },
                { "Предметы", () => FileHelper.CreateXmlBackupFileAsync(ItemsTable, SaveFilePath + @"\Предметы.xml") },
                { "Аддоны для предметов", () => FileHelper.CreateXmlBackupFileAsync(ItemAddonsTable, SaveFilePath + @"\Аддоны для предметов.xml") },
                { "Подношение", () => FileHelper.CreateXmlBackupFileAsync(OfferingsTable, SaveFilePath + @"\Подношение.xml") },
                { "Категории подношений", () => FileHelper.CreateXmlBackupFileAsync(OfferingsCategoriesTable, SaveFilePath + @"\Категории подношений.xml") },
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
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Билды убийцы" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Информация по выжившем" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Список выживших" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Перки выживших" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Билды выжившего" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Предметы" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Аддоны для предметов" });

            BackupsTable.Add(new Backup { IsCheck = false, Name = "Подношение" });
            BackupsTable.Add(new Backup { IsCheck = false, Name = "Категории подношений" });
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
            await GetKillerBuildData();

            await GetSurvivorInfoData();
            await GetSurvivorData();
            await GetSurvivorPerkData();
            await GetSurvivorBuildData();

            await GetItemData();
            await GetItemAddonData();

            await GetOfferingData();
            await GetOfferingCategoryData();
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

        private async Task GetKillerBuildData()
        {
            KillersBuildsTable.Clear();
            KillersBuildsTable = await KillerBuildData();
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

        private async Task GetSurvivorBuildData()
        {
            SurvivorBuildsTable.Clear();
            SurvivorBuildsTable = await SurvivorBuildData();
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

         private async Task GetOfferingCategoryData()
        {
            OfferingsCategoriesTable.Clear();
            OfferingsCategoriesTable = await OfferingCategoryData();
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

        public async Task<List<GameStatistic>> GameStatisticData()
        {
            return await _dataService.GetAllDataInListAsync<GameStatistic>();
        }

        public async Task<List<KillerInfo>> KillerInfoData()
        {
            return await _dataService.GetAllDataInListAsync<KillerInfo>();
        }

        public async Task<List<Killer>> KillerData()
        {
            return await _dataService.GetAllDataInListAsync<Killer>();
        }

        public async Task<List<KillerPerk>> KillerPerkData()
        {
            return await _dataService.GetAllDataInListAsync<KillerPerk>();
        }

        public async Task<List<KillerAddon>> KillerAddonData()
        {
            return await _dataService.GetAllDataInListAsync<KillerAddon>();
        }

        public async Task<List<KillerBuild>> KillerBuildData()
        {
            return await _dataService.GetAllDataInListAsync<KillerBuild>();
        }

        public async Task<List<SurvivorInfo>> SurvivorInfoData()
        {
            return await _dataService.GetAllDataInListAsync<SurvivorInfo>();
        }

        public async Task<List<Survivor>> SurvivorData()
        {
            return await _dataService.GetAllDataInListAsync<Survivor>();
        }

        public async Task<List<SurvivorBuild>> SurvivorBuildData()
        {
            return await _dataService.GetAllDataInListAsync<SurvivorBuild>();
        }

        public async Task<List<SurvivorPerk>> SurvivorPerkData()
        {
            return await _dataService.GetAllDataInListAsync<SurvivorPerk>();
        }

        public async Task<List<Item>> ItemData()
        {
            return await _dataService.GetAllDataInListAsync<Item>();
        }

        public async Task<List<ItemAddon>> ItemAddonData()
        {
            return await _dataService.GetAllDataInListAsync<ItemAddon>();
        }

        public async Task<List<Offering>> OfferingData()
        {
            return await _dataService.GetAllDataInListAsync<Offering>();
        }

        public async Task<List<OfferingCategory>> OfferingCategoryData()
        {
            return await _dataService.GetAllDataInListAsync<OfferingCategory>();
        }
        public async Task<List<Rarity>> RarityData()
        {
            return await _dataService.GetAllDataInListAsync<Rarity>();
        }

        public async Task<List<Role>> RoleData()
        {
            return await _dataService.GetAllDataInListAsync<Role>();
        }

        public async Task<List<Map>> MapData()
        {
            return await _dataService.GetAllDataInListAsync<Map>();
        }

        public async Task<List<Measurement>> MeasurementData()
        {
            return await _dataService.GetAllDataInListAsync<Measurement>();
        }

        public async Task<List<WhoPlacedMap>> WhoPlacedMapData()
        {
            return await _dataService.GetAllDataInListAsync<WhoPlacedMap>();
        }

        public async Task<List<Patch>> PatchData()
        {
            return await _dataService.GetAllDataInListAsync<Patch>();
        }

        public async Task<List<GameMode>> GameModeData()
        {
            return await _dataService.GetAllDataInListAsync<GameMode>();
        }

        public async Task<List<GameEvent>> GameEventData()
        {
            return await _dataService.GetAllDataInListAsync<GameEvent>();
        }

        public async Task<List<TypeDeath>> TypeDeathData()
        {
            return await _dataService.GetAllDataInListAsync<TypeDeath>();
        }

        public async Task<List<PlayerAssociation>> PlayerAssociationData()
        {
            return await _dataService.GetAllDataInListAsync<PlayerAssociation>();
        }

        public async Task<List<Platform>> PlatformData()
        {
            return await _dataService.GetAllDataInListAsync<Platform>();
        }

        #endregion

    }
}
