using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using System.Collections.ObjectModel;

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
                if (value == null) { return; }
                _selectedGameModeItem = value;
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
                if (value == null) { return; }
                _selectedGameEventItem = value;
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
                if (value == null) { return; }
                _selectedPlatformItem = value;
                Platform = value.PlatformName;
                PlatformDescription = value.PlatformDescription;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<PlayerAssociation> AssociationList { get; set; } = [];

        private PlayerAssociation _selectedAssociationItem;
        public PlayerAssociation SelectedAssociationItem
        {
            get => _selectedAssociationItem;
            set
            {
                if (value == null) { return; }
                _selectedAssociationItem = value;
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
                if (value == null) { return; }
                _selectedPatchItem = value;
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
                if (value == null) { return; }
                _selectedTypeDeath = value;
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
                if (value == null) { return; }
                _selectedRole = value;
                Role = value.RoleName;
                RoleDescription = value.RoleDescription;
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
                if (value == null) { return; }
                _selectedMeasurementItem = value;
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
                _title = value;
                OnPropertyChanged();
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
                _gameEvent = value;
                OnPropertyChanged();
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

        private static IDataService _dataService;

        public AddAdditionalDataWindowViewModel(IDataService dataService)
        {
            _dataService = dataService;
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

            if (MessageHelper.MessageDelete(SelectedGameModeItem.GameModeName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedGameModeItem);
                GetGameModeData();
            } 
            else return;
        }); }

        private RelayCommand _deleteGameEvenItemCommand;
        public RelayCommand DeleteGameEventItemCommand { get => _deleteGameEvenItemCommand ??= new(async obj => 
        {
            if (SelectedGameEventItem == null) return;

            if (MessageHelper.MessageDelete(SelectedGameEventItem.GameEventName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedGameEventItem);
                GetGameEventData();
            } 
            else return;
        }); }

        private RelayCommand _deletePlatformItemCommand;
        public RelayCommand DeletePlatformItemCommand { get => _deletePlatformItemCommand ??= new(async obj =>
        {
            if (SelectedPlatformItem == null) return;

            if (MessageHelper.MessageDelete(SelectedPlatformItem.PlatformName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedPlatformItem);
                GetPlatformData();
            }
            else return;
        }); }

        private RelayCommand _deletePlayerAssociationItemCommand;
        public RelayCommand DeletePlayerAssociationItemCommand { get => _deletePlayerAssociationItemCommand ??= new(async obj =>
        {
            if (SelectedAssociationItem == null) return;

            if (MessageHelper.MessageDelete(SelectedAssociationItem.PlayerAssociationName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedAssociationItem);
                GetPlayerAssociationData();
            }
            else return;
        }); }

        private RelayCommand _deletePatchItemCommand;
        public RelayCommand DeletePatchItemCommand { get => _deletePatchItemCommand ??= new(async obj =>
        {
            if (SelectedPatchItem == null) return;

            if (MessageHelper.MessageDelete(SelectedPatchItem.PatchNumber) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedPatchItem);
                GetPatchData();
            }
            else return;
        }); }

        private RelayCommand _deleteTypeDeathItemCommand;
        public RelayCommand DeleteTypeDeathItemCommand { get => _deleteTypeDeathItemCommand ??= new(async obj =>
        {
            if (SelectedTypeDeathItem == null) return;

            if (MessageHelper.MessageDelete(SelectedTypeDeathItem.TypeDeathName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedTypeDeathItem);
                GetTypeDeathData();
            }
            else return;
        }); }

        private RelayCommand _deleteRoleItemCommand;
        public RelayCommand DeleteRoleItemCommand { get => _deleteRoleItemCommand ??= new(async obj =>
        {
            if (SelectedRoleItem == null) return;

            if (MessageHelper.MessageDelete(SelectedRoleItem.RoleName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedRoleItem);
                GetRoleData();
            }
            else return;
        }); }
        
        private RelayCommand _deleteMeasurementItemCommand;
        public RelayCommand DeleteMeasurementItemCommand { get => _deleteMeasurementItemCommand ??= new(async obj =>
        {
            if (SelectedMeasurementItem == null) return;

            if (MessageHelper.MessageDelete(SelectedMeasurementItem.MeasurementName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedMeasurementItem);
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
            SetNullGameMode();
            var gameMode = await _dataService.GetAllDataAsync<GameMode>();

            foreach (var item in gameMode)
            {
                GameModeList.Add(item);
            }
        }

        private async void GetGameEventData()
        {
            SetNullGameEvent();
            var gameEvents = await _dataService.GetAllDataAsync<GameEvent>();

            foreach (var item in gameEvents)
            {
                GameEventList.Add(item);
            }
        }

        private async void GetPlatformData()
        {
            SetNullPlatform();
            var platforms = await _dataService.GetAllDataAsync<Platform>();

            foreach (var item in platforms)
            {
                PlatformList.Add(item);
            }
        }

        private async void GetPlayerAssociationData()
        {
            SetNullPlayerAssociation();
            var associations = await _dataService.GetAllDataAsync<PlayerAssociation>();

            foreach (var item in associations)
            {
                AssociationList.Add(item);
            }
        }

        private async void GetPatchData()
        {
            SetNullPatch();
            var patches = await _dataService.GetAllDataAsync<Patch>();

            foreach (var item in patches)
            {
                PatchList.Add(item);
            }
        }

        private async void GetTypeDeathData()
        {
           SetNullTypeDeath();
            var typeDeaths = await _dataService.GetAllDataAsync<TypeDeath>();

            foreach (var item in typeDeaths)
            {
                DeathList.Add(item);
            }
        }

        private async void GetRoleData()
        {
            SetNullGameRole();
            var roles = await _dataService.GetAllDataAsync<Role>();

            foreach (var item in roles)
            {
                GameRoleList.Add(item);
            }
        }
        
        private async void GetMeasurementData()
        {
            SetNullMeasurement();
            var measurements = await _dataService.GetAllDataAsync<Measurement>();

            foreach (var item in measurements)
            {
                MeasurementList.Add(item);
            }
        }

        #endregion

        #region Методы добавление данных в БД

        private async void AddGameMode()
        {
            var newGameMode = new GameMode { GameModeName = GameMode, GameModeDescription = GameModeDescription };

            bool exists = await _dataService.ExistsAsync<GameMode>(GM => GM.GameModeName.ToLower() == newGameMode.GameModeName.ToLower());

            if (exists || string.IsNullOrEmpty(GameMode)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newGameMode);
                GetGameModeData();
            }
        }
      
        private async void AddGameEvent()
        {
            var newGameEvent = new GameEvent { GameEventName = GameEvent, GameEventDescription = GameEventDescription};

            bool exists = await _dataService.ExistsAsync<GameEvent>(GE => GE.GameEventName.ToLower() == newGameEvent.GameEventName.ToLower());

            if (exists || string.IsNullOrEmpty(GameEvent)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newGameEvent);
                GetGameEventData();
            }
        }

        private async void AddPlatform()
        {
            var newPlatform = new Platform { PlatformName = Platform, PlatformDescription = PlatformDescription };

            bool exists = await _dataService.ExistsAsync<Platform>(P => P.PlatformName.ToLower() == newPlatform.PlatformName.ToLower());

            if (exists || string.IsNullOrEmpty(Platform)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newPlatform);
                GetPlatformData();
            }
        }

        private async void AddPlayerAssociation()
        {
            var newPlayerAssociation = new PlayerAssociation { PlayerAssociationName = PlayerAssociation, PlayerAssociationDescription = PlayerAssociationDescription };

            bool exists = await _dataService.ExistsAsync<PlayerAssociation>(P => P.PlayerAssociationName.ToLower() == newPlayerAssociation.PlayerAssociationName.ToLower());

            if (exists || string.IsNullOrEmpty(PlayerAssociation)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newPlayerAssociation);
                GetPlayerAssociationData();
            }
        }

        private async void AddPatch()
        {
            var newPatch = new Patch { PatchNumber = PatchNumber, PatchDateRelease = DateOnly.FromDateTime(PatchDateRelease) };

            bool exists = await _dataService.ExistsAsync<Patch>(P => P.PatchNumber == newPatch.PatchNumber);

            if (exists || string.IsNullOrEmpty(PatchNumber)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newPatch);
                GetPatchData();
            }
        }

        private async void AddTypeDeath()
        {
            var newTypeDeath = new TypeDeath { TypeDeathName = TypeDeath, TypeDeathDescription = TypeDeathDescription };

            bool exists = await _dataService.ExistsAsync<TypeDeath>(TP => TP.TypeDeathName == newTypeDeath.TypeDeathName);

            if (exists || string.IsNullOrEmpty(TypeDeath)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync<TypeDeath>(newTypeDeath);
                GetTypeDeathData();
            }
        }

        private async void AddRole()
        {
            var newRole = new Role { RoleName = Role, RoleDescription = RoleDescription };

            bool exists = await _dataService.ExistsAsync<Role>(R => R.RoleName == newRole.RoleName);

            if (exists || string.IsNullOrEmpty(Role)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newRole);
                GetRoleData();
            }
        }
        
        private async void AddMeasurement()
        {
            var newMeasurement = new Measurement { MeasurementName = Measurement, MeasurementDescription = MeasurementDescription };

            bool exists = await _dataService.ExistsAsync<Measurement>(R => R.MeasurementName == newMeasurement.MeasurementName);

            if (exists || string.IsNullOrEmpty(Measurement)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newMeasurement);
                GetMeasurementData();
            }
        }

        #endregion

        #region Методы редактирования данных из БД

        private async void UpdateGameModeItem()
        {
            if (SelectedGameModeItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<GameMode>(SelectedGameModeItem.IdGameMode);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<GameMode>(x => x.GameModeName.ToLower() == GameMode.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedGameModeItem.GameModeName, GameMode, SelectedGameModeItem.GameModeDescription, GameModeDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.GameModeName = GameMode;
                        entityToUpdate.GameModeDescription = GameModeDescription;
                        await _dataService.UpdateAsync(entityToUpdate);
                        GetGameModeData();
                    }
                }
                else
                {
                    entityToUpdate.GameModeName = GameMode;
                    entityToUpdate.GameModeDescription = GameModeDescription;
                    await _dataService.UpdateAsync(entityToUpdate);
                    GetGameModeData();
                }
            }
        }

        private async void UpdateGameEventItem()
        {
            if (SelectedGameEventItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<GameEvent>(SelectedGameEventItem.IdGameEvent);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<GameEvent>(x => x.GameEventName.ToLower() == GameEvent.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedGameEventItem.GameEventName, GameEvent, SelectedGameEventItem.GameEventDescription, GameEventDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.GameEventName = GameEvent;
                        entityToUpdate.GameEventDescription = GameEventDescription;
                        await _dataService.UpdateAsync(entityToUpdate);
                        GetGameEventData();
                    }
                }
                else
                {
                    entityToUpdate.GameEventName = GameEvent;
                    entityToUpdate.GameEventDescription = GameEventDescription;
                    await _dataService.UpdateAsync(entityToUpdate);
                    GetGameEventData();
                }
            }
        }

        private async void UpdatePlatformItem()
        {
            if (SelectedPlatformItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<Platform>(SelectedPlatformItem.IdPlatform);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<Platform>(x => x.PlatformName.ToLower() == Platform.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedPlatformItem.PlatformName, Platform, SelectedPlatformItem.PlatformDescription, PlatformDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.PlatformName = Platform;
                        entityToUpdate.PlatformDescription = PlatformDescription;
                        await _dataService.UpdateAsync(entityToUpdate);
                        GetPlatformData();
                    }
                }
                else
                {
                    entityToUpdate.PlatformName = Platform;
                    entityToUpdate.PlatformDescription = PlatformDescription;
                    await _dataService.UpdateAsync(entityToUpdate);
                    GetPlatformData();
                }
            }
        }

        private async void UpdatePlayerAssociationItem()
        {
            if (SelectedAssociationItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<PlayerAssociation>(SelectedAssociationItem.IdPlayerAssociation);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<PlayerAssociation>(x => x.PlayerAssociationName.ToLower() == PlayerAssociation.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedAssociationItem.PlayerAssociationName, PlayerAssociation, SelectedAssociationItem.PlayerAssociationDescription, PlayerAssociationDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.PlayerAssociationName = PlayerAssociation;
                        entityToUpdate.PlayerAssociationDescription = PlayerAssociationDescription;
                        await _dataService.UpdateAsync(entityToUpdate);
                        GetPlayerAssociationData();
                    }
                }
                else
                {
                    entityToUpdate.PlayerAssociationName = PlayerAssociation;
                    entityToUpdate.PlayerAssociationDescription = PlayerAssociationDescription;
                    await _dataService.UpdateAsync(entityToUpdate);
                    GetPlayerAssociationData();
                }
            }
        }

        private async void UpdatePatchItem()
        {
            if (SelectedPatchItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<Patch>(SelectedPatchItem.IdPatch);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<Patch>(x => x.PatchNumber.ToLower() == PatchNumber.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedPatchItem.PatchNumber, PatchNumber, SelectedPatchItem.PatchDateRelease.ToString(), PatchDateRelease.ToString()) == MessageButtons.Yes)
                    {
                        entityToUpdate.PatchNumber = PatchNumber;
                        entityToUpdate.PatchDateRelease = DateOnly.FromDateTime(PatchDateRelease);
                        await _dataService.UpdateAsync(entityToUpdate);
                        GetPatchData();
                    }
                }
                else
                {
                    entityToUpdate.PatchNumber = PatchNumber;
                    entityToUpdate.PatchDateRelease = DateOnly.FromDateTime(PatchDateRelease);
                    await _dataService.UpdateAsync(entityToUpdate);
                    GetPatchData();
                }
            }
        }

        private async void UpdateTypeDeathItem()
        {
            if (SelectedTypeDeathItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<TypeDeath>(SelectedTypeDeathItem.IdTypeDeath);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<TypeDeath>(x => x.TypeDeathName.ToLower() == TypeDeath.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedTypeDeathItem.TypeDeathName, TypeDeath, SelectedTypeDeathItem.TypeDeathDescription, TypeDeathDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.TypeDeathName = TypeDeath;
                        entityToUpdate.TypeDeathDescription = TypeDeathDescription;
                        await _dataService.UpdateAsync(entityToUpdate);
                        GetTypeDeathData();
                    }
                }
                else
                {
                    entityToUpdate.TypeDeathName = TypeDeath;
                    entityToUpdate.TypeDeathDescription = TypeDeathDescription;
                    await _dataService.UpdateAsync(entityToUpdate);
                    GetTypeDeathData();
                }
            }
        }

        private async void UpdateRoleItem()
        {
            if (SelectedRoleItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<Role>(SelectedRoleItem.IdRole);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<Role>(x => x.RoleName.ToLower() == Role.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedRoleItem.RoleName, Role, SelectedRoleItem.RoleDescription, RoleDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.RoleName = Role;
                        entityToUpdate.RoleDescription = RoleDescription;
                        await _dataService.UpdateAsync<Role>(entityToUpdate);
                        GetRoleData();
                    }
                }
                else
                {
                    entityToUpdate.RoleName = Role;
                    entityToUpdate.RoleDescription = RoleDescription;
                    await _dataService.UpdateAsync<Role>(entityToUpdate);
                    GetRoleData();
                }
            }
        }

        private async void UpdateMeasurementItem()
        {
            if (SelectedMeasurementItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<Measurement>(SelectedMeasurementItem.IdMeasurement);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<Measurement>(x => x.MeasurementName.ToLower() == Measurement.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedMeasurementItem.MeasurementName, Measurement, SelectedMeasurementItem.MeasurementDescription, MeasurementDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.MeasurementName = Measurement;
                        entityToUpdate.MeasurementDescription = MeasurementDescription;
                        await _dataService.UpdateAsync(entityToUpdate);
                        GetMeasurementData();
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
                    await _dataService.UpdateAsync(entityToUpdate);
                    GetMeasurementData();
                }
            }
        }

        #endregion

        #region Методы обнуление выводимых данных

        private void SetNullGameMode()
        {
            GameModeList.Clear();
            GameMode = string.Empty;
            GameModeDescription = string.Empty;
            SelectedGameModeItem = null;
        }

        private void SetNullGameEvent()
        {
            GameEventList.Clear();
            GameEvent = string.Empty;
            GameEventDescription = string.Empty;
            SelectedGameEventItem = null;
        } 
        
        private void SetNullPlatform()
        {
            PlatformList.Clear();
            Platform = string.Empty;
            PlatformDescription = string.Empty;
            SelectedPlatformItem = null;
        }

        private void SetNullPlayerAssociation()
        {
            AssociationList.Clear();
            PlayerAssociation = string.Empty;
            PlayerAssociationDescription = string.Empty;
            SelectedAssociationItem = null;
        }
        
        private void SetNullPatch()
        {
            PatchList.Clear();
            PatchNumber = string.Empty;
            PatchDateRelease = DateTime.MinValue;
            SelectedPatchItem = null;
        }
        
        private void SetNullTypeDeath()
        {
            DeathList.Clear();
            TypeDeath = string.Empty;
            TypeDeathDescription = string.Empty;
            SelectedTypeDeathItem = null;
        } 
        
        private void SetNullGameRole()
        {
            GameRoleList.Clear();
            Role = string.Empty;
            RoleDescription = string.Empty;
            SelectedRoleItem = null;
        } 
        
        private void SetNullMeasurement()
        {
            MeasurementList.Clear();
            Measurement = string.Empty;
            MeasurementDescription = string.Empty;
            SelectedMeasurementItem = null;
        }

        #endregion

    }
}
