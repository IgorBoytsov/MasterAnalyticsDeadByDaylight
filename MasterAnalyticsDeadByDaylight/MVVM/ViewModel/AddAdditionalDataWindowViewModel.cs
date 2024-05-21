using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Net.NetworkInformation;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    class AddAdditionalDataWindowViewModel : BaseViewModel
    {
        #region Свойства 

        public ObservableCollection<GameMode> GameModeList { get; set; }

        private GameMode _selectedGameModeItem;
        public GameMode SelectedGameModeItem
        {
            get => _selectedGameModeItem;
            set
            {
                _selectedGameModeItem = value;
                if (value == null) { return; }
                TextBoxGameModeName = value.GameModeName;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<GameEvent> GameEventList { get; set; }

        private GameEvent _selectedGameEventItem;
        public GameEvent SelectedGameEventItem
        {
            get => _selectedGameEventItem;
            set
            {
                _selectedGameEventItem = value;
                if (value == null) { return; }
                TextBoxGameEventName = value.GameEventName;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Platform> PlatformList { get; set; }

        private Platform _selectedPlatformItem;
        public Platform SelectedPlatformItem
        {
            get => _selectedPlatformItem;
            set
            {
                _selectedPlatformItem = value;
                if (value == null) { return; }
                TextBoxPlatformName = value.PlatformName;
                OnPropertyChanged();

            }
        }


        public ObservableCollection<PlayerAssociation> AssociationList { get; set; }

        private PlayerAssociation _selectedPlayerAssociationItem;
        public PlayerAssociation SelectedAssociationItem
        {
            get => _selectedPlayerAssociationItem;
            set
            {
                _selectedPlayerAssociationItem = value;
                if (value == null) { return; }
                TextBoxPlayerAssociationName = value.PlayerAssociationName;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Patch> PatchList { get; set; }

        private Patch _selectedPatchItem;
        public Patch SelectedPatchItem
        {
            get => _selectedPatchItem;
            set
            {
                _selectedPatchItem = value;
                if (value == null) { return; }
                TextBoxPatchNumber = value.PatchNumber;
                DatePickerPatchDateRelease = value.PatchDateRelease.ToDateTime(TimeOnly.MinValue);
                OnPropertyChanged();
            }
        }


        public ObservableCollection<TypeDeath> DeathList { get; set; }

        private TypeDeath _selectedTypeDeath;
        public TypeDeath SelectedTypeDeathItem
        {
            get => _selectedTypeDeath;
            set
            {
                _selectedTypeDeath = value;
                if (value == null) { return; }
                TextBoxTypeDeath = value.TypeDeathName;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Role> GameRoleList { get; set; }

        private Role _selectedRole;
        public Role SelectedRoleItem
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if (value == null) { return; }
                TextBoxRole = value.RoleName;
                OnPropertyChanged();

            }
        }


        private string _titel;
        public string Titel
        {
            get => _titel;
            set
            {
                if (_titel != value)
                {
                    _titel = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _textBoxGameModeName;
        public string TextBoxGameModeName
        {
            get => _textBoxGameModeName;
            set
            {
                _textBoxGameModeName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxGameEventName;
        public string TextBoxGameEventName
        {
            get => _textBoxGameEventName;
            set
            {
                if (_textBoxGameEventName != value)
                {
                    _textBoxGameEventName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _textBoxPlatformName;
        public string TextBoxPlatformName
        {
            get => _textBoxPlatformName;
            set
            {
                _textBoxPlatformName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxPlayerAssociationName;
        public string TextBoxPlayerAssociationName
        {
            get => _textBoxPlayerAssociationName;
            set
            {
                _textBoxPlayerAssociationName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxPatchNumber;
        public string TextBoxPatchNumber
        {
            get => _textBoxPatchNumber;
            set
            {
                _textBoxPatchNumber = value;
                OnPropertyChanged();
            }
        }

        private DateTime _datePickerPatchDateRelease;
        public DateTime DatePickerPatchDateRelease
        {
            get => _datePickerPatchDateRelease;
            set
            {
                _datePickerPatchDateRelease = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxTypeDeath;
        public string TextBoxTypeDeath
        {
            get => _textBoxTypeDeath;
            set
            {
                _textBoxTypeDeath = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxTextBoxRole;
        public string TextBoxRole
        {
            get => _textBoxTextBoxRole;
            set
            {
                _textBoxTextBoxRole = value;
                OnPropertyChanged();
            }
        }


        #endregion

        public AddAdditionalDataWindowViewModel()
        {
            GetAndUpdateData();
            Titel = "Добавление базовых данных";
            DatePickerPatchDateRelease = DateTime.Now;
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


        private RelayCommand _deleteGameModeItemCommand;
        public RelayCommand DeleteGameModeItemCommand { get => _deleteGameModeItemCommand ??= new(obj => DeleteGameModeItem()); }

        private RelayCommand _deleteGameEvenItemCommand;
        public RelayCommand DeleteGameEventItemCommand { get => _deleteGameEvenItemCommand ??= new(obj => DeleteGameEventItem()); }

        private RelayCommand _deletePlatformItemCommand;
        public RelayCommand DeletePlatformItemCommand { get => _deletePlatformItemCommand ??= new(obj => DeletePlatformItem()); }

        private RelayCommand _deletePlayerAssociationItemCommand;
        public RelayCommand DeletePlayerAssociationItemCommand { get => _deletePlayerAssociationItemCommand ??= new(obj => DeleteAssociationItem()); }

        private RelayCommand _deletePatchItemCommand;
        public RelayCommand DeletePatchItemCommand { get => _deletePatchItemCommand ??= new(obj => DeletePatchItem()); }

        private RelayCommand _deleteTypeDeathItemCommand;
        public RelayCommand DeleteTypeDeathItemCommand { get => _deleteTypeDeathItemCommand ??= new(obj => DeleteTypeDeathItem()); }

        private RelayCommand _deleteRoleItemCommand;
        public RelayCommand DeleteRoleItemCommand { get => _deleteRoleItemCommand ??= new(obj => DeleteRoleItem()); }


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

        #endregion

        #region Методы получение данных из БД

        private void GetAndUpdateData()
        {
            RefList();
            GetGameModeData();
            GetGameEventData();
            GetPlatformData();
            GetPlayerAssociationData();
            GetPatchData();
            GetTypeDeathData();
            GetRoleData();
        }

        private void RefList()
        {
            GameModeList = new ObservableCollection<GameMode>();
            GameEventList = new ObservableCollection<GameEvent>();
            PlatformList = new ObservableCollection<Platform>();
            AssociationList = new ObservableCollection<PlayerAssociation>();
            PatchList = new ObservableCollection<Patch>();
            DeathList = new ObservableCollection<TypeDeath>();
            GameRoleList = new ObservableCollection<Role>();
        }

        private async void GetGameModeData()
        {
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
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var roles = await context.Roles.ToListAsync();
                foreach (var item in roles)
                {
                    GameRoleList.Add(item);
                }
            }
        }

        #endregion

        #region Методы добавление данных в БД

        private void AddGameMode()
        {
            var newGameMode = new GameMode { GameModeName = TextBoxGameModeName };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.GameModes.Any(GM => GM.GameModeName.ToLower() == newGameMode.GameModeName.ToLower());

                if (exists || string.IsNullOrEmpty(TextBoxGameModeName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.GameModes.Add(newGameMode);
                    context.SaveChanges();
                    GameModeList.Clear();
                    GetGameModeData();
                    TextBoxGameModeName = string.Empty;
                }
            }
        }

        private void AddGameEvent()
        {
            var newGameEvent = new GameEvent { GameEventName = TextBoxGameEventName };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.GameEvents.Any(GE => GE.GameEventName.ToLower() == newGameEvent.GameEventName.ToLower());

                if (exists || string.IsNullOrEmpty(TextBoxGameEventName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.GameEvents.Add(newGameEvent);
                    context.SaveChanges();
                    GameEventList.Clear();
                    GetGameEventData();
                    TextBoxGameEventName = string.Empty;
                }
            }
        }

        private void AddPlatform()
        {
            var newPlatform = new Platform { PlatformName = TextBoxPlatformName };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Platforms.Any(P => P.PlatformName.ToLower() == newPlatform.PlatformName.ToLower());

                if (exists || string.IsNullOrEmpty(TextBoxPlatformName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.Platforms.Add(newPlatform);
                    context.SaveChanges();
                    PlatformList.Clear();
                    GetPlatformData();
                    TextBoxPlatformName = string.Empty;
                }
            }
        }

        private void AddPlayerAssociation()
        {
            var newPlayerAssociation = new PlayerAssociation { PlayerAssociationName = TextBoxPlayerAssociationName };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.PlayerAssociations.Any(P => P.PlayerAssociationName.ToLower() == newPlayerAssociation.PlayerAssociationName.ToLower());

                if (exists || string.IsNullOrEmpty(TextBoxPlayerAssociationName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.PlayerAssociations.Add(newPlayerAssociation);
                    context.SaveChanges();
                    AssociationList.Clear();
                    GetPlayerAssociationData();
                    TextBoxPlayerAssociationName = string.Empty;
                }
            }
        }

        private void AddPatch()
        {
            var newPatch = new Patch { PatchNumber = TextBoxPatchNumber, PatchDateRelease = DateOnly.FromDateTime(DatePickerPatchDateRelease) };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Patches.Any(P => P.PatchNumber == newPatch.PatchNumber);

                if (exists || string.IsNullOrEmpty(TextBoxPatchNumber))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.Patches.Add(newPatch);
                    context.SaveChanges();
                    PatchList.Clear();
                    GetPatchData();
                    TextBoxPatchNumber = string.Empty;
                }
            }
        }

        private void AddTypeDeath()
        {
            var newTypeDeath = new TypeDeath { TypeDeathName = TextBoxTypeDeath };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.TypeDeaths.Any(TP => TP.TypeDeathName == newTypeDeath.TypeDeathName);

                if (exists || string.IsNullOrEmpty(TextBoxTypeDeath))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.TypeDeaths.Add(newTypeDeath);
                    context.SaveChanges();
                    DeathList.Clear();
                    GetTypeDeathData();
                    TextBoxTypeDeath = string.Empty;
                }
            }
        }

        private void AddRole()
        {
            var newRole = new Role { RoleName = TextBoxRole };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Roles.Any(R => R.RoleName == newRole.RoleName);

                if (exists || string.IsNullOrEmpty(TextBoxRole))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.Roles.Add(newRole);
                    context.SaveChanges();
                    GameRoleList.Clear();
                    GetRoleData();
                    TextBoxRole = string.Empty;
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
                    if (entityToUpdate.GameModeName == TextBoxGameModeName)
                    {
                        MessageBox.Show("Нельзя менять текст на такой же");
                        return;
                    }

                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedGameModeItem.GameModeName} на {TextBoxGameModeName} ?", 
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.GameModeName = TextBoxGameModeName;
                        context.SaveChanges();
                        GameModeList.Clear();
                        GetGameModeData();
                        SelectedGameModeItem = null;
                        TextBoxGameModeName = string.Empty;
                    }       
                }
                else { MessageBox.Show("Нечего обновлять"); }
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
                    if (entityToUpdate.GameEventName == TextBoxGameEventName)
                    {
                        MessageBox.Show("Нельзя менять текст на такой же");
                        return;
                    }

                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedGameEventItem.GameEventName} на {TextBoxGameEventName} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.GameEventName = TextBoxGameEventName;
                        context.SaveChanges();
                        GameEventList.Clear();
                        GetGameEventData();
                        SelectedGameEventItem = null;
                        TextBoxGameEventName = string.Empty;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
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
                    if (entityToUpdate.PlatformName == TextBoxPlatformName)
                    {
                        MessageBox.Show("Нельзя менять текст на такой же");
                        return;
                    }

                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedPlatformItem.PlatformName} на {TextBoxPlatformName} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.PlatformName = TextBoxPlatformName;
                        context.SaveChanges();
                        PlatformList.Clear();
                        GetPlatformData();
                        SelectedPlatformItem = null;
                        TextBoxPlatformName = string.Empty;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
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
                    if (entityToUpdate.PlayerAssociationName == TextBoxPlayerAssociationName)
                    {
                        MessageBox.Show("Нельзя менять текст на такой же");
                        return;
                    }

                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedAssociationItem.PlayerAssociationName} на {TextBoxPlayerAssociationName} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.PlayerAssociationName = TextBoxPlayerAssociationName;
                        context.SaveChanges();
                        AssociationList.Clear();
                        GetPlayerAssociationData();
                        SelectedAssociationItem = null;
                        TextBoxPlayerAssociationName = string.Empty;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
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
                    if (entityToUpdate.PatchNumber == TextBoxPatchNumber)
                    {
                        MessageBox.Show("Нельзя менять текст на такой же");
                        return;
                    }

                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedPatchItem.PatchNumber} на {TextBoxPatchNumber} и {SelectedPatchItem.PatchDateRelease} на {DatePickerPatchDateRelease} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.PatchNumber = TextBoxPatchNumber;
                        entityToUpdate.PatchDateRelease = DateOnly.FromDateTime(DatePickerPatchDateRelease);
                        context.SaveChanges();
                        PatchList.Clear();
                        GetPatchData();
                        SelectedPatchItem = null;
                        DatePickerPatchDateRelease = DateTime.MinValue;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
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
                    if (entityToUpdate.TypeDeathName == TextBoxTypeDeath)
                    {
                        MessageBox.Show("Нельзя менять текст на такой же");
                        return;
                    }

                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedTypeDeathItem.TypeDeathName} на {TextBoxTypeDeath} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.TypeDeathName = TextBoxTypeDeath;
                        context.SaveChanges();
                        DeathList.Clear();
                        GetTypeDeathData();
                        SelectedTypeDeathItem = null;
                        TextBoxTypeDeath = string.Empty;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
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
                    if (entityToUpdate.RoleName == TextBoxRole)
                    {
                        MessageBox.Show("Нельзя менять текст на такой же");
                        return;
                    }

                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedRoleItem.RoleName} на {TextBoxRole} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.RoleName = TextBoxRole;
                        context.SaveChanges();
                        GameRoleList.Clear();
                        GetRoleData();
                        SelectedRoleItem = null;
                        TextBoxRole = string.Empty;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
            }
        }
        #endregion

        #region Методы удаления данных из БД

        private void DeleteGameModeItem()
        {          
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.GameModes.Find(SelectedGameModeItem.IdGameMode);
                if (entityToDelete != null)
                {
                    context.GameModes.Remove(entityToDelete);
                    context.SaveChanges();
                    GameModeList.Clear();
                    GetGameModeData();
                }             
            }
        }

        private void DeleteGameEventItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.GameEvents.Find(SelectedGameEventItem.IdGameEvent);
                if (entityToDelete != null)
                {
                    context.GameEvents.Remove(entityToDelete);
                    context.SaveChanges();
                    GameEventList.Clear();
                    GetGameEventData();
                }
            }
        }

        private void DeletePlatformItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.Platforms.Find(SelectedPlatformItem.IdPlatform);
                if (entityToDelete !=null)
                {
                    context.Remove(entityToDelete);
                    context.SaveChanges();
                    PlatformList.Clear();
                    GetPlatformData();
                }
            }
        }

        private void DeleteAssociationItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.PlayerAssociations.Find(SelectedAssociationItem.IdPlayerAssociation);
                if (entityToDelete != null)
                {
                    context.PlayerAssociations.Remove(entityToDelete);
                    context.SaveChanges();
                    AssociationList.Clear();
                    GetPlayerAssociationData();
                }
            }
        }

        private void DeletePatchItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.Patches.Find(SelectedPatchItem.IdPatch);
                if (entityToDelete != null)
                {
                    context.Patches.Remove(entityToDelete);
                    context.SaveChanges();
                    PatchList.Clear();
                    GetPatchData();
                }
            }
        }

        private void DeleteTypeDeathItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.TypeDeaths.Find(SelectedTypeDeathItem.IdTypeDeath);
                if (entityToDelete != null)
                {
                    context.TypeDeaths.Find(entityToDelete);
                    context.SaveChanges();
                    DeathList.Clear();
                    GetTypeDeathData();
                }
            }
        }

        private void DeleteRoleItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.Roles.Find(SelectedRoleItem.IdRole);
                if (entityToDelete != null)
                {
                    context.Roles.Remove(entityToDelete);
                    context.SaveChanges();
                    GameRoleList.Clear();
                    GetRoleData();
                }
            }
        }
        #endregion
    }
}
