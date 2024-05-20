using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    class AddAdditionalDataWindowViewModel : BaseViewModel
    {
        #region Свойства 

        public ObservableCollection<GameMode> GameModeList {  get; set; }

        public GameMode SelectedGameModeItem { get; set; }


        public ObservableCollection<GameEvent> GameEventList { get; set; } /*= ["Отсутствует", "Годовщина", "Новый год"];*/

        public GameEvent SelectedGameEventItem { get; set; }


        public ObservableCollection<Platform> PlatformList { get; set; } /*= ["Стим", "Консоль/Эпик"];*/

        public Platform SelectedPlatformItem { get; set; }


        public ObservableCollection<PlayerAssociation> AssociationList { get; set; } /*= ["Я", "Тиммейт", "Рандом"];*/

        public PlayerAssociation SelectedAssociationItem { get; set; }


        public ObservableCollection<Patch> PatchList { get; set; } /*= ["8.0.0", "7.7.1", "7.7.0a"];*/

        public Patch SelectedPatchItem { get; set; }


        public ObservableCollection<TypeDeath> DeathList { get; set; } /*= ["От крюка", "От земли", "От мементо"];*/

        public TypeDeath SelectedTypeDeathItem { get; set; }


        public ObservableCollection<Role> GameRoleList { get; set; } /*= ["Убийца", "Выживший"];*/

        public Role SelectedRoleItem { get; set; }


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
                _textBoxGameEventName = value;
                OnPropertyChanged();
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

        #endregion

        #region Методы получение данных из БД

        private void GetAndUpdateData()
        {
            RefList();
            GetGameModeList();
            GetEventData();
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

        private async void GetGameModeList()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var gamemodes = await context.GameModes.ToListAsync();
                foreach (var item in gamemodes)
                {
                    GameModeList.Add(item);
                }
            }
        }

        private async void GetEventData()
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
        //
        private void AddGameMode()
        {
            var newGameMode = new GameMode {GameModeName = TextBoxGameModeName };

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
                    GetGameModeList();
                    TextBoxGameModeName = string.Empty;
                }
            } 
        }

        private void AddGameEvent()
        {
            var newGameEvent = new GameEvent { GameEventName = TextBoxGameEventName};

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
                    GetEventData();
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
            var newPlayerAssociation = new PlayerAssociation { PlayerAssociationName = TextBoxPlayerAssociationName};

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
            var newPatch = new Patch { PatchNumber = TextBoxPatchNumber, PatchDateRelease = DateOnly.FromDateTime(DatePickerPatchDateRelease)};

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
            var newTypeDeath = new TypeDeath { TypeDeathName = TextBoxTypeDeath};

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
            var newRole = new Role { RoleName = TextBoxRole};

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
    }
}
