using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    class AddAdditionalDataWindowViewModel : BaseViewModel
    {
        #region Свойства 

        public ObservableCollection<GameMode> GameModeList { get; set; } = [];

        private GameMode _selectedGameModeItem;
        public GameMode SelectedGameModeItem
        {
            get => _selectedGameModeItem;
            set
            {
                _selectedGameModeItem = value;
                if (value == null) { return; }
                GameMode = value.GameModeName;
                GameModeDescription = value.GameModeDescription;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameEvent> GameEventList { get; set; } = [];

        private GameEvent _selectedGameEventItem;
        public GameEvent SelectedGameEventItem
        {
            get => _selectedGameEventItem;
            set
            {
                _selectedGameEventItem = value;
                if (value == null) { return; }
                GameEvent = value.GameEventName;
                GameEventDescription = value.GameEventDescription;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Platform> PlatformList { get; set; } = [];

        private Platform _selectedPlatformItem;
        public Platform SelectedPlatformItem
        {
            get => _selectedPlatformItem;
            set
            {
                _selectedPlatformItem = value;
                if (value == null) { return; }
                Platform = value.PlatformName;
                PlatformDescription = value.PlatformDescription;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<PlayerAssociation> AssociationList { get; set; } = [];

        private PlayerAssociation _selectedPlayerAssociationItem;
        public PlayerAssociation SelectedAssociationItem
        {
            get => _selectedPlayerAssociationItem;
            set
            {
                _selectedPlayerAssociationItem = value;
                if (value == null) { return; }
                PlayerAssociation = value.PlayerAssociationName;
                PlayerAssociationDescription = value.PlayerAssociationDescription;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Patch> PatchList { get; set; } = [];

        private Patch _selectedPatchItem;
        public Patch SelectedPatchItem
        {
            get => _selectedPatchItem;
            set
            {
                _selectedPatchItem = value;
                if (value == null) { return; }
                PatchNumber = value.PatchNumber;
                PatchDateRelease = value.PatchDateRelease.ToDateTime(TimeOnly.MinValue);
                OnPropertyChanged();
            }
        }


        public ObservableCollection<TypeDeath> DeathList { get; set; } = [];

        private TypeDeath _selectedTypeDeath;
        public TypeDeath SelectedTypeDeathItem
        {
            get => _selectedTypeDeath;
            set
            {
                _selectedTypeDeath = value;
                if (value == null) { return; }
                TypeDeath = value.TypeDeathName;
                TypeDeathDescription = value.TypeDeathDescription;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Role> GameRoleList { get; set; } = [];

        private Role _selectedRole;
        public Role SelectedRoleItem
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if (value == null) { return; }
                Role = value.RoleName;
                TypeDeathDescription = value.RoleDescription;
                OnPropertyChanged();

            }
        }


        public ObservableCollection<Measurement> MeasurementList { get; set; } = [];

        private Measurement _selectedMeasurementItem;
        public Measurement SelectedMeasurementItem
        {
            get => _selectedMeasurementItem;
            set
            {
                _selectedMeasurementItem = value;
                if (value == null) { return; }
                Measurement = value.MeasurementName;
                MeasurementDescription = value.MeasurementDescription;
                OnPropertyChanged();

            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _gameMode;
        public string GameMode
        {
            get => _gameMode;
            set
            {
                _gameMode = value;
                OnPropertyChanged();
            }
        }
        
        private string _gameModeDescription;
        public string GameModeDescription
        {
            get => _gameModeDescription;
            set
            {
                _gameModeDescription = value;
                OnPropertyChanged();
            }
        }

        private string _gameEvent;
        public string GameEvent
        {
            get => _gameEvent;
            set
            {
                if (_gameEvent != value)
                {
                    _gameEvent = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _gameEventDescription;
        public string GameEventDescription
        {
            get => _gameEventDescription;
            set
            {
                _gameEventDescription = value;
                OnPropertyChanged();
            }
        }

        private string _platform;
        public string Platform
        {
            get => _platform;
            set
            {
                _platform = value;
                OnPropertyChanged();
            }
        }

        private string _platformDescription;
        public string PlatformDescription
        {
            get => _platformDescription;
            set
            {
                _platformDescription = value;
                OnPropertyChanged();
            }
        }

        private string _playerAssociation;
        public string PlayerAssociation
        {
            get => _playerAssociation;
            set
            {
                _playerAssociation = value;
                OnPropertyChanged();
            }
        }

        private string _playerAssociationDescription;
        public string PlayerAssociationDescription
        {
            get => _playerAssociationDescription;
            set
            {
                _playerAssociationDescription = value;
                OnPropertyChanged();
            }
        }

        private string _patchNumber;
        public string PatchNumber
        {
            get => _patchNumber;
            set
            {
                _patchNumber = value;
                OnPropertyChanged();
            }
        }

        private DateTime _patchDateRelease;
        public DateTime PatchDateRelease
        {
            get => _patchDateRelease;
            set
            {
                _patchDateRelease = value;
                OnPropertyChanged();
            }
        }

        private string _typeDeath;
        public string TypeDeath
        {
            get => _typeDeath;
            set
            {
                _typeDeath = value;
                OnPropertyChanged();
            }
        }

        private string _typeDeathDescription;
        public string TypeDeathDescription
        {
            get => _typeDeathDescription;
            set
            {
                _typeDeathDescription = value;
                OnPropertyChanged();
            }
        }

        private string _role;
        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged();
            }
        }

        private string roleDescription;
        public string RoleDescription
        {
            get => roleDescription;
            set
            {
                roleDescription = value;
                OnPropertyChanged();
            }
        }

        private string _measurement;
        public string Measurement
        {
            get => _measurement;
            set
            {
                _measurement = value;
                OnPropertyChanged();
            }
        }

        private string _measurementDescription;
        public string MeasurementDescription
        {
            get => _measurementDescription;
            set
            {
                _measurementDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private readonly IDialogService _dialogService;

        public AddAdditionalDataWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            GetAndUpdateData();
            Title = "Добавление базовых данных";
            PatchDateRelease = DateTime.Now;
        }

        #region Команды

        private RelayCommand _saveGameModeCommand;
        public RelayCommand SaveGameModeCommand { get => _saveGameModeCommand ??= new(obj => AddGameMode()); }

        private RelayCommand _saveGameEventCommand;
        public RelayCommand SaveGameEventCommand { get => _saveGameEventCommand ??= new(obj => AddGameEvent()); }

        private RelayCommand _savePlatformCommand;
        public RelayCommand SavePlatformCommand { get => _savePlatformCommand ??= new(obj => AddPlatform()); }

        private RelayCommand _savePlayerAssociationCommand;
        public RelayCommand SavePlayerAssociationCommand { get => _savePlayerAssociationCommand ??= new(obj => AddPlayerAssociation()); }

        private RelayCommand _savePatchCommand;
        public RelayCommand SavePatchCommand { get => _savePatchCommand ??= new(obj => AddPatch()); }

        private RelayCommand _saveTypeDeathCommand;
        public RelayCommand SaveTypeDeathCommand { get => _saveTypeDeathCommand ??= new(obj => AddTypeDeath()); }

        private RelayCommand _saveRoleCommand;
        public RelayCommand SaveRoleCommand { get => _saveRoleCommand ??= new(obj => AddRole()); }

        private RelayCommand _saveMeasurementCommand;
        public RelayCommand SaveMeasurementCommand { get => _saveMeasurementCommand ??= new(obj => AddMeasurement()); }


        private RelayCommand _deleteGameModeItemCommand;
        public RelayCommand DeleteGameModeItemCommand { get => _deleteGameModeItemCommand ??= new(async obj =>
        {
            if (SelectedGameModeItem == null) return;

            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedGameModeItem.GameModeName}»?", 
                "Предупреждение об удаление.", 
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedGameModeItem);
                GetGameModeData();
            } 
            else return;
        }); }

        private RelayCommand _deleteGameEvenItemCommand;
        public RelayCommand DeleteGameEventItemCommand { get => _deleteGameEvenItemCommand ??= new(async obj => 
        {
            if (SelectedGameEventItem == null) return;

            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedGameEventItem.GameEventName}»? Это может привести к удалению связанных записей!",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedGameEventItem);
                GetGameEventData();
            } 
            else return;
        }); }

        private RelayCommand _deletePlatformItemCommand;
        public RelayCommand DeletePlatformItemCommand { get => _deletePlatformItemCommand ??= new(async obj =>
        {
            if (SelectedPlatformItem == null) return;

            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedPlatformItem.PlatformName}»? Это может привести к удалению связанных записей!",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedPlatformItem);
                GetPlatformData();
            }
            else return;
        }); }

        private RelayCommand _deletePlayerAssociationItemCommand;
        public RelayCommand DeletePlayerAssociationItemCommand { get => _deletePlayerAssociationItemCommand ??= new(async obj =>
        {
            if (SelectedAssociationItem == null) return;

            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedAssociationItem.PlayerAssociationName}»? Это может привести к удалению связанных записей!",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedAssociationItem);
                GetPlayerAssociationData();
            }
            else return;
        }); }

        private RelayCommand _deletePatchItemCommand;
        public RelayCommand DeletePatchItemCommand { get => _deletePatchItemCommand ??= new(async obj =>
        {
            if (SelectedPatchItem == null) return;

            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedPatchItem.PatchNumber}»? Это может привести к удалению связанных записей!",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedPatchItem);
                GetPatchData();
            }
            else return;
        }); }

        private RelayCommand _deleteTypeDeathItemCommand;
        public RelayCommand DeleteTypeDeathItemCommand { get => _deleteTypeDeathItemCommand ??= new(async obj =>
        {
            if (SelectedTypeDeathItem == null) return;

            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedTypeDeathItem.TypeDeathName}»? Это может привести к удалению связанных записей!",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedTypeDeathItem);
                GetTypeDeathData();
            }
            else return;
        }); }

        private RelayCommand _deleteRoleItemCommand;
        public RelayCommand DeleteRoleItemCommand { get => _deleteRoleItemCommand ??= new(async obj =>
        {
            if (SelectedRoleItem == null) return;

            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedRoleItem.RoleName}»? Это может привести к удалению связанных записей!",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedRoleItem);
                GetRoleData();
            }
            else return;
        }); }
        
        private RelayCommand _deleteMeasurementItemCommand;
        public RelayCommand DeleteMeasurementItemCommand { get => _deleteMeasurementItemCommand ??= new(async obj =>
        {
            if (SelectedMeasurementItem == null) return;

            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedMeasurementItem.MeasurementName}»? Это может привести к удалению связанных записей!",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedMeasurementItem);
                GetMeasurementData();
            }
            else return;
        }); }


        private RelayCommand _updateGameModeItemCommand;
        public RelayCommand UpdateGameModeItemCommand { get => _updateGameModeItemCommand ??= new(obj => UpdateGameModeItem()); }

        private RelayCommand _updateGameEvenItemCommand;
        public RelayCommand UpdateGameEventItemCommand { get => _updateGameEvenItemCommand ??= new(obj => UpdateGameEventItem()); }

        private RelayCommand _updatePlatformItemCommand;
        public RelayCommand UpdatePlatformItemCommand { get => _updatePlatformItemCommand ??= new(obj => UpdatePlatformItem()); }

        private RelayCommand _updatePlayerAssociationItemCommand;
        public RelayCommand UpdatePlayerAssociationItemCommand { get => _updatePlayerAssociationItemCommand ??= new(obj => UpdatePlayerAssociationItem()); }

        private RelayCommand _updatePatchItemCommand;
        public RelayCommand UpdatePatchItemCommand { get => _updatePatchItemCommand ??= new(obj => UpdatePatchItem()); }

        private RelayCommand _updateTypeDeathItemCommand;
        public RelayCommand UpdateTypeDeathItemCommand { get => _updateTypeDeathItemCommand ??= new(obj => UpdateTypeDeathItem()); }

        private RelayCommand _updateRoleItemCommand;
        public RelayCommand UpdateRoleItemCommand { get => _updateRoleItemCommand ??= new(obj => UpdateRoleItem()); } 
        
        private RelayCommand _updateMeasurementItemCommand;
        public RelayCommand UpdateMeasurementItemCommand { get => _updateMeasurementItemCommand ??= new(obj => UpdateMeasurementItem()); }

        #endregion

        #region Методы получение данных из БД

        private void GetAndUpdateData()
        {
            GetGameModeData();
            GetGameEventData();
            GetPlatformData();
            GetPlayerAssociationData();
            GetPatchData();
            GetTypeDeathData();
            GetRoleData();
            GetMeasurementData();
        }

        private async void GetGameModeData()
        {
            GameModeList.Clear();
            GameMode = string.Empty;
            GameModeDescription = string.Empty;
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var gameevents = await context.GameModes.ToListAsync();
                foreach (var item in gameevents)
                {
                    GameModeList.Add(item);
                }
            }
        }

        private async void GetGameEventData()
        {
            GameEventList.Clear();
            GameEvent = string.Empty;
            GameEventDescription = string.Empty;
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var gameevents = await context.GameEvents.ToListAsync();
                foreach (var item in gameevents)
                {
                    GameEventList.Add(item);
                }
            }
        }

        private async void GetPlatformData()
        {
            PlatformList.Clear();
            Platform = string.Empty;
            PlatformDescription = string.Empty;
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var platforms = await context.Platforms.ToListAsync();
                foreach (var item in platforms)
                {
                    PlatformList.Add(item);
                }
            }
        }

        private async void GetPlayerAssociationData()
        {
            AssociationList.Clear();
            PlayerAssociation = string.Empty;
            PlayerAssociationDescription = string.Empty;
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var associations = await context.PlayerAssociations.ToListAsync();
                foreach (var item in associations)
                {
                    AssociationList.Add(item);
                }
            }
        }

        private async void GetPatchData()
        {
            PatchList.Clear();
            PatchNumber = string.Empty;
            PatchDateRelease = DateTime.MinValue;
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var patches = await context.Patches.ToListAsync();
                foreach (var item in patches)
                {
                    PatchList.Add(item);
                }
            }
        }

        private async void GetTypeDeathData()
        {
            DeathList.Clear();
            TypeDeath = string.Empty;
            TypeDeathDescription = string.Empty;
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var typedeaths = await context.TypeDeaths.ToListAsync();
                foreach (var item in typedeaths)
                {
                    DeathList.Add(item);
                }
            }
        }

        private async void GetRoleData()
        {
            GameRoleList.Clear();
            Role = string.Empty;
            RoleDescription = string.Empty;
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var roles = await context.Roles.ToListAsync();
                foreach (var item in roles)
                {
                    GameRoleList.Add(item);
                }
            }
        }
        
        private async void GetMeasurementData()
        {
            MeasurementList.Clear();
            Measurement = string.Empty;
            MeasurementDescription = string.Empty;
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var measurements = await context.Measurements.ToListAsync();
                foreach (var item in measurements)
                {
                    MeasurementList.Add(item);
                }
            }
        }

        #endregion

        #region Методы добавление данных в БД

        private void AddGameMode()
        {
            var newGameMode = new GameMode { GameModeName = GameMode, GameModeDescription = GameModeDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.GameModes.Any(GM => GM.GameModeName.ToLower() == newGameMode.GameModeName.ToLower());

                if (exists || string.IsNullOrEmpty(GameMode))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали","Ошибка добавления",TypeMessage.Warning);
                }
                else
                {
                    context.GameModes.Add(newGameMode);
                    context.SaveChanges();
                    GameModeList.Clear();
                    GetGameModeData();
                    GameMode = string.Empty;
                }
            }
        }

        private void AddGameEvent()
        {
            var newGameEvent = new GameEvent { GameEventName = GameEvent, GameEventDescription = GameEventDescription};

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.GameEvents.Any(GE => GE.GameEventName.ToLower() == newGameEvent.GameEventName.ToLower());

                if (exists || string.IsNullOrEmpty(GameEvent))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Ошибка добавления", TypeMessage.Warning);
                }
                else
                {
                    context.GameEvents.Add(newGameEvent);
                    context.SaveChanges();
                    GameEventList.Clear();
                    GetGameEventData();
                    GameEvent = string.Empty;
                }
            }
        }

        private void AddPlatform()
        {
            var newPlatform = new Platform { PlatformName = Platform, PlatformDescription = PlatformDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Platforms.Any(P => P.PlatformName.ToLower() == newPlatform.PlatformName.ToLower());

                if (exists || string.IsNullOrEmpty(Platform))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Ошибка добавления", TypeMessage.Warning);
                }
                else
                {
                    context.Platforms.Add(newPlatform);
                    context.SaveChanges();
                    PlatformList.Clear();
                    GetPlatformData();
                    Platform = string.Empty;
                }
            }
        }

        private void AddPlayerAssociation()
        {
            var newPlayerAssociation = new PlayerAssociation { PlayerAssociationName = PlayerAssociation, PlayerAssociationDescription = PlayerAssociationDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.PlayerAssociations.Any(P => P.PlayerAssociationName.ToLower() == newPlayerAssociation.PlayerAssociationName.ToLower());

                if (exists || string.IsNullOrEmpty(PlayerAssociation))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Ошибка добавления", TypeMessage.Warning);
                }
                else
                {
                    context.PlayerAssociations.Add(newPlayerAssociation);
                    context.SaveChanges();
                    AssociationList.Clear();
                    GetPlayerAssociationData();
                    PlayerAssociation = string.Empty;
                }
            }
        }

        private void AddPatch()
        {
            var newPatch = new Patch { PatchNumber = PatchNumber, PatchDateRelease = DateOnly.FromDateTime(PatchDateRelease) };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Patches.Any(P => P.PatchNumber == newPatch.PatchNumber);

                if (exists || string.IsNullOrEmpty(PatchNumber))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Ошибка добавления", TypeMessage.Warning);
                }
                else
                {
                    context.Patches.Add(newPatch);
                    context.SaveChanges();
                    PatchList.Clear();
                    GetPatchData();
                    PatchNumber = string.Empty;
                }
            }
        }

        private void AddTypeDeath()
        {
            var newTypeDeath = new TypeDeath { TypeDeathName = TypeDeath, TypeDeathDescription = TypeDeathDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.TypeDeaths.Any(TP => TP.TypeDeathName == newTypeDeath.TypeDeathName);

                if (exists || string.IsNullOrEmpty(TypeDeath))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Ошибка добавления", TypeMessage.Warning);
                }
                else
                {
                    context.TypeDeaths.Add(newTypeDeath);
                    context.SaveChanges();
                    DeathList.Clear();
                    GetTypeDeathData();
                    TypeDeath = string.Empty;
                }
            }
        }

        private void AddRole()
        {
            var newRole = new Role { RoleName = Role, RoleDescription = RoleDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Roles.Any(R => R.RoleName == newRole.RoleName);

                if (exists || string.IsNullOrEmpty(Role))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Ошибка добавления", TypeMessage.Warning);
                }
                else
                {
                    context.Roles.Add(newRole);
                    context.SaveChanges();
                    GameRoleList.Clear();
                    GetRoleData();
                    Role = string.Empty;
                }
            }
        }
        
        private void AddMeasurement()
        {
            var newMeasurement = new Measurement { MeasurementName = Measurement, MeasurementDescription = MeasurementDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Measurements.Any(R => R.MeasurementName == newMeasurement.MeasurementName);

                if (exists || string.IsNullOrEmpty(Measurement))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Ошибка добавления", TypeMessage.Warning);
                }
                else
                {
                    context.Measurements.Add(newMeasurement);
                    context.SaveChanges();
                    MeasurementList.Clear();
                    GetMeasurementData();
                    Measurement = string.Empty;
                }
            }
        }
        #endregion

        #region Методы редактирования данных из БД

        private void UpdateGameModeItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedGameModeItem == null)
                {
                    return;
                }

                var entityToUpdate = context.GameModes.Find(SelectedGameModeItem.IdGameMode);

                if (entityToUpdate != null)
                {
                    bool exists = exists = context.GameModes.Any(x => x.GameModeName.ToLower() == GameMode.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{SelectedGameModeItem.GameModeName}» на «{GameMode}» и «{SelectedGameModeItem.GameModeDescription}» на «{GameModeDescription}»",
                            $"Надпись с именем «{SelectedGameModeItem.GameModeName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.GameModeName = GameMode;
                            entityToUpdate.GameModeDescription = GameModeDescription;
                            context.SaveChanges();
                            GameModeList.Clear();
                            GetGameModeData();
                            SelectedGameModeItem = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.GameModeName = GameMode;
                        entityToUpdate.GameModeDescription = GameModeDescription;
                        context.SaveChanges();
                        GameModeList.Clear();
                        GetGameModeData();
                        SelectedGameModeItem = null;
                    }
                }
            }
        }

        private void UpdateGameEventItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedGameEventItem == null)
                {
                    return;
                }

                var entityToUpdate = context.GameEvents.Find(SelectedGameEventItem.IdGameEvent);

                if (entityToUpdate != null)
                {
                    bool exists = context.GameEvents.Any(x => x.GameEventName.ToLower() == GameEvent.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{SelectedGameEventItem.GameEventName}» на «{GameEvent}» и «{SelectedGameEventItem.GameEventDescription}» на «{GameEventDescription}»",
                            $"Надпись с именем «{SelectedGameEventItem.GameEventName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.GameEventName = GameEvent;
                            entityToUpdate.GameEventDescription = GameEventDescription;
                            context.SaveChanges();
                            GetGameEventData();
                            SelectedGameEventItem = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.GameEventName = GameEvent;
                        entityToUpdate.GameEventDescription = GameEventDescription;
                        context.SaveChanges();
                        GetGameEventData();
                        SelectedGameEventItem = null;
                    }
                }
            }
        }

        private void UpdatePlatformItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedPlatformItem == null)
                {
                    return;
                }

                var entityToUpdate = context.Platforms.Find(SelectedPlatformItem.IdPlatform);

                if (entityToUpdate != null)
                {
                    bool exists = context.Platforms.Any(x => x.PlatformName.ToLower() == Platform.ToLower());              

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{SelectedPlatformItem.PlatformName}» на «{Platform}» и «{SelectedPlatformItem.PlatformDescription}» на «{PlatformDescription}»",
                            $"Надпись с именем «{SelectedPlatformItem.PlatformName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.PlatformName = Platform;
                            entityToUpdate.PlatformDescription = PlatformDescription;
                            context.SaveChanges();
                            GetPlatformData();
                            SelectedPlatformItem = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.PlatformName = Platform;
                        entityToUpdate.PlatformDescription = PlatformDescription;
                        context.SaveChanges();
                        GetPlatformData();
                        SelectedPlatformItem = null;
                    }
                }
            }
        }

        private void UpdatePlayerAssociationItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedAssociationItem == null)
                {
                    return;
                }

                var entityToUpdate = context.PlayerAssociations.Find(SelectedAssociationItem.IdPlayerAssociation);

                if (entityToUpdate != null)
                {
                    bool exists = context.PlayerAssociations.Any(x => x.PlayerAssociationName.ToLower() == PlayerAssociation.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{SelectedAssociationItem.PlayerAssociationName}» на «{PlayerAssociation}» и «{SelectedAssociationItem.PlayerAssociationDescription}» на «{PlayerAssociationDescription}»",
                            $"Надпись с именем «{SelectedAssociationItem.PlayerAssociationName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.PlayerAssociationName = PlayerAssociation;
                            entityToUpdate.PlayerAssociationDescription = PlayerAssociationDescription;
                            context.SaveChanges();
                            GetPlayerAssociationData();
                            SelectedAssociationItem = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.PlayerAssociationName = PlayerAssociation;
                        entityToUpdate.PlayerAssociationDescription = PlayerAssociationDescription;
                        context.SaveChanges();
                        GetPlayerAssociationData();
                        SelectedAssociationItem = null;
                    }
                }
            }
        }

        private void UpdatePatchItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedPatchItem == null)
                {
                    return;
                }

                var entityToUpdate = context.Patches.Find(SelectedPatchItem.IdPatch);

                if (entityToUpdate != null)
                {
                    bool exists = context.Patches.Any(x => x.PatchNumber.ToLower() == PatchNumber.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{SelectedPatchItem.PatchNumber}» на «{PatchNumber}» и «{SelectedPatchItem.PatchDateRelease}» на «{PatchDateRelease}»",
                            $"Надпись с именем «{SelectedPatchItem.PatchNumber}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.PatchNumber = PatchNumber;
                            entityToUpdate.PatchDateRelease = DateOnly.FromDateTime(PatchDateRelease);
                            context.SaveChanges();
                            GetPatchData();
                            SelectedPatchItem = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.PatchNumber = PatchNumber;
                        entityToUpdate.PatchDateRelease = DateOnly.FromDateTime(PatchDateRelease);
                        context.SaveChanges();
                        GetPatchData();
                        SelectedPatchItem = null;
                    }
                }
            }
        }

        private void UpdateTypeDeathItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedTypeDeathItem == null)
                {
                    return;
                }

                var entityToUpdate = context.TypeDeaths.Find(SelectedTypeDeathItem.IdTypeDeath);

                if (entityToUpdate != null)
                {
                    bool exists = context.TypeDeaths.Any(x => x.TypeDeathName.ToLower() == TypeDeath.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{SelectedTypeDeathItem.TypeDeathName}» на «{TypeDeath}» и «{SelectedTypeDeathItem.TypeDeathDescription}» на «{TypeDeathDescription}»",
                            $"Надпись с именем «{SelectedTypeDeathItem.TypeDeathName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.TypeDeathName = TypeDeath;
                            entityToUpdate.TypeDeathDescription = TypeDeathDescription;
                            context.SaveChanges();
                            GetTypeDeathData();
                            SelectedTypeDeathItem = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.TypeDeathName = TypeDeath;
                        entityToUpdate.TypeDeathDescription = TypeDeathDescription;
                        context.SaveChanges();
                        GetTypeDeathData();
                        SelectedTypeDeathItem = null;
                    }
                }
            }
        }

        private void UpdateRoleItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedRoleItem == null)
                {
                    return;
                }

                var entityToUpdate = context.Roles.Find(SelectedRoleItem.IdRole);

                if (entityToUpdate != null)
                {
                    bool exists = context.Roles.Any(x => x.RoleName.ToLower() == Role.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{SelectedRoleItem.RoleName}» на «{Role}» и «{SelectedRoleItem.RoleDescription}» на «{RoleDescription}»",
                            $"Надпись с именем «{SelectedRoleItem.RoleName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.RoleName = Role;
                            entityToUpdate.RoleDescription = RoleDescription;
                            context.SaveChanges();
                            GetRoleData();
                            SelectedRoleItem = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.RoleName = Role;
                        entityToUpdate.RoleDescription = RoleDescription;
                        context.SaveChanges();
                        GetRoleData();
                        SelectedRoleItem = null;
                    }
                }
            }
        }

        private void UpdateMeasurementItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedMeasurementItem == null)
                {
                    return;
                }

                var entityToUpdate = context.Measurements.Find(SelectedMeasurementItem.IdMeasurement);

                if (entityToUpdate != null)
                {
                    bool exists = context.Measurements.Any(x => x.MeasurementName.ToLower() == Measurement.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{SelectedMeasurementItem.MeasurementName}» на «{Measurement}» и «{SelectedMeasurementItem.MeasurementDescription}» на «{MeasurementDescription}»",
                            $"Надпись с именем «{SelectedMeasurementItem.MeasurementName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.MeasurementName = Measurement;
                            entityToUpdate.MeasurementDescription = MeasurementDescription;
                            context.SaveChanges();
                            GetMeasurementData();
                            SelectedMeasurementItem = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.MeasurementName = Measurement;
                        entityToUpdate.MeasurementDescription = MeasurementDescription;
                        context.SaveChanges();
                        GetMeasurementData();
                        SelectedMeasurementItem = null;
                    }
                }
            }
        }

        #endregion

    }
}
