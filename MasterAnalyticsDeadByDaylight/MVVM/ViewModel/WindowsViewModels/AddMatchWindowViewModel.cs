using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class AddMatchWindowViewModel : BaseViewModel, IUpdatable
    {
        #region Блок "Коллекции данных из БД"

        #region Общие коллекции 

        /*-Коллекции : Общие-*/
        private readonly List<Offering> _offering = [];
        private readonly List<GameMode> _gameModes = [];
        private readonly List<GameEvent> _gameEvents = [];
        private readonly List<Map> _maps = [];
        private readonly List<Patch> _patches = [];
        private readonly List<Platform> _platforms = [];
        private readonly List<PlayerAssociation> _playerAssociations = [];
        private readonly List<Role> _roles = [];
        private readonly List<WhoPlacedMap> _whoPlacedMaps = [];

        public ObservableCollection<GameMode> GameModes { get; set; } = [];
        public ObservableCollection<GameEvent> GameEvents { get; set; } = [];
        public ObservableCollection<Map> Maps { get; set; } = [];
        public ObservableCollection<Patch> Patches { get; set; } = [];
        public ObservableCollection<Platform> Platforms { get; set; } = [];
        public ObservableCollection<WhoPlacedMap> WhoPlacedMaps { get; set; } = [];

        #endregion

        #region Коллекции для Убийц   

        /*-Коллекции : Киллеры-*/
        private readonly List<Killer> _killers = [];
        private readonly List<KillerAddon> _killerAddons = [];
        private readonly List<KillerPerk> _killerPerks = [];
        private readonly List<KillerBuild> _killerBuilds = [];

        public ObservableCollection<Killer> Killers { get; set; } = [];
        public ObservableCollection<KillerAddon> KillerAddons { get; set; } = [];
        public ObservableCollection<KillerPerk> KillerPerks { get; set; } = [];
        public ObservableCollection<Offering> KillerOfferings { get; set; } = [];
        public ObservableCollection<PlayerAssociation> KillerAssociations { get; set; } = [];
        public ObservableCollection<Role> KillerRoles { get; set; } = [];
        public ObservableCollection<KillerBuild> KillerBuilds { get; set; } = [];

        #endregion

        #region Коллекции для выживших      

        /*-Коллекции : Выжившие-*/
        private readonly List<Survivor> _survivors = [];
        private readonly List<SurvivorPerk> _survivorPerks = [];
        private readonly List<Item> _items = [];
        private readonly List<ItemAddon> _itemAddons = [];
        private readonly List<TypeDeath> _typeDeaths = [];

        public ObservableCollection<Survivor> Survivors { get; set; } = [];
        public ObservableCollection<SurvivorPerk> SurvivorPerks { get; set; } = [];
        public ObservableCollection<Item> Items { get; set; } = [];
        public ObservableCollection<ItemAddon> FirstSurvivorItemAddons { get; set; } = [];
        public ObservableCollection<ItemAddon> SecondSurvivorItemAddons { get; set; } = [];
        public ObservableCollection<ItemAddon> ThirdSurvivorItemAddons { get; set; } = [];
        public ObservableCollection<ItemAddon> FourthSurvivorItemAddons { get; set; } = [];
        public ObservableCollection<Offering> FirstSurvivorOfferings { get; set; } = [];
        public ObservableCollection<Offering> SecondSurvivorOfferings { get; set; } = [];
        public ObservableCollection<Offering> ThirdSurvivorOfferings { get; set; } = [];
        public ObservableCollection<Offering> FourthSurvivorOfferings { get; set; } = [];
        public ObservableCollection<PlayerAssociation> SurvivorAssociations { get; set; } = [];
        public ObservableCollection<Role> SurvivorRoles { get; set; } = [];
        public ObservableCollection<TypeDeath> TypeDeaths { get; set; } = [];

        #endregion

        #endregion

        #region Сообщения

        private const string InvalidNumberMessageDescription = "Не корректное значение";

        #endregion

        public static string Title { get; } = "Добавить запись";

        private readonly IServiceProvider _serviceProvider;

        private readonly ICustomDialogService _dialogService;
        private readonly IDataService _dataService;

        public AddMatchWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dialogService = _serviceProvider.GetService<ICustomDialogService>();
            _dataService = _serviceProvider.GetService<IDataService>();

            IssOpenKillerBuildPopup = false;
            GetAllData();
            LoadAllData();
        }

        public void Update(object value)
        {

        }

        #region Блок "Убийца"

        #region Основная информация

        /// <summary>
        /// Выбор "Убийцы" из выпадающего списка. 
        /// При выборе "Убийцы" из списка персонажей в ListView ( Список улучшений ) 
        /// загружаются аддоны, которые относятся к выбранному "Убийце"
        /// </summary>      
        private Killer _selectedKiller;
        public Killer SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                _selectedKiller = value;
                //GetKillerAddonListData();
                LoadKillerAddons();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выбор платформы на которой играет "Убийца"
        /// </summary> 
        private Platform _selectedKillerPlatform;
        public Platform SelectedKillerPlatform
        {
            get => _selectedKillerPlatform;
            set
            {
                _selectedKillerPlatform = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Престиж "Убийцы"
        /// </summary> 
        private int _killerPrestige;
        public int KillerPrestige
        {
            get => _killerPrestige;
            set
            {
                if (!CheckPrestige(value)) MessageHelper.PrestigeMessage();
                else
                {
                    _killerPrestige = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Счет очков "Убийцы" по окончанию игры
        /// </summary>
        private int _killerAccount;
        public int KillerAccount
        {
            get => _killerAccount;
            set
            {
                if (value < 0) { return; }
                _killerAccount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Какой режим приватности был у "Убийцы"? Анонимный или нет
        /// </summary>
        private bool _killerAnonymousMode;
        public bool KillerAnonymousMode
        {
            get => _killerAnonymousMode;
            set
            {
                _killerAnonymousMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вышел ли убийца до окончания игры или нет
        /// </summary>
        private bool _killerBot;
        public bool KillerBot
        {
            get => _killerBot;
            set
            {
                _killerBot = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Перки - Свойства

        /// <summary>
        /// Свойство для отслеживание выбранного перка "Убийцы" из списка (ListView)
        /// </summary>
        private KillerPerk _selectedKillerPerk;
        public KillerPerk SelectedKillerPerk
        {
            get => _selectedKillerPerk;
            set
            {
                if (value != null)
                {
                    _selectedKillerPerk = value;
                    OnPropertyChanged();
                }
                else { return; }
            }
        }

        /// <summary>
        /// Свойство для хранение информации о первом выбранном перке "Убийцы"
        /// </summary>
        private KillerPerk _selectedKillerFirstPerk;
        public KillerPerk SelectedKillerFirstPerk
        {
            get => _selectedKillerFirstPerk;
            set
            {
                if (value != null)
                {
                    _selectedKillerFirstPerk = value;
                    SelectedKillerFirstPerkName = value.PerkName;
                }
                else { SelectedKillerFirstPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором выбранном перке "Убийцы"
        /// </summary>
        private KillerPerk _selectedKillerSecondPerk;
        public KillerPerk SelectedKillerSecondPerk
        {
            get => _selectedKillerSecondPerk;
            set
            {
                if (value != null)
                {
                    _selectedKillerSecondPerk = value;
                    SelectedKillerSecondPerkName = value.PerkName;
                }
                else { SelectedKillerSecondPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о третьем выбранном перке "Убийцы"
        /// </summary>
        private KillerPerk _selectedKillerThirdPerk;
        public KillerPerk SelectedKillerThirdPerk
        {
            get => _selectedKillerThirdPerk;
            set
            {
                if (value != null)
                {
                    _selectedKillerThirdPerk = value;
                    SelectedKillerThirdPerkName = value.PerkName;
                }
                else { SelectedKillerThirdPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о четвертом выбранном перке "Убийцы"
        /// </summary>
        private KillerPerk _selectedKillerFourthPerk;
        public KillerPerk SelectedKillerFourthPerk
        {
            get => _selectedKillerFourthPerk;
            set
            {
                if (value != null)
                {
                    _selectedKillerFourthPerk = value;
                    SelectedKillerFourthPerkName = value.PerkName;
                }
                else { SelectedKillerFourthPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия первого выбранного перка "Убийцы"
        /// </summary> 
        private string _selectedKillerFirstPerkName;
        public string SelectedKillerFirstPerkName
        {
            get => _selectedKillerFirstPerkName;
            set
            {
                _selectedKillerFirstPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия второго выбранного перка "Убийцы"
        /// </summary>
        private string _selectedKillerSecondPerkName;
        public string SelectedKillerSecondPerkName
        {
            get => _selectedKillerSecondPerkName;
            set
            {
                _selectedKillerSecondPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия третьего выбранного перка "Убийцы"
        /// </summary>
        private string _selectedKillerThirdPerkName;
        public string SelectedKillerThirdPerkName
        {
            get => _selectedKillerThirdPerkName;
            set
            {
                _selectedKillerThirdPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия четвертого выбранного перка "Убийцы"
        /// </summary>
        private string _selectedKillerFourthPerkName;
        public string SelectedKillerFourthPerkName
        {
            get => _selectedKillerFourthPerkName;
            set
            {
                _selectedKillerFourthPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку перков "Убийцы"
        /// </summary
        private string _searchKillerPerk;
        public string SearchKillerPerk
        {
            get => _searchKillerPerk;
            set
            {
                _searchKillerPerk = value;
                SearchKillerPerks();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Перки - Команды

        /// <summary>
        /// Команда выбора перка в 1 позицию для ContextMenu у элемента ListView KillerPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedKillerFirstPerkCommand;
        public RelayCommand SelectedKillerFirstPerkCommand
        {
            get => _selectedKillerFirstPerkCommand ??= new(obj =>
            {
                SelectedKillerFirstPerk = SelectedKillerPerk;
                SelectedKillerFirstPerkName = SelectedKillerPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора перка в 2 позицию для ContextMenu у элемента ListView KillerPerk
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedKillerSecondPerkCommand;
        public RelayCommand SelectedKillerSecondPerkCommand
        {
            get => _selectedKillerSecondPerkCommand ??= new(obj =>
            {
                SelectedKillerSecondPerk = SelectedKillerPerk;
                SelectedKillerSecondPerkName = SelectedKillerPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора перка в 3 позицию для ContextMenu у элемента ListView KillerPerk
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedKillerThirdPerkCommand;
        public RelayCommand SelectedKillerThirdPerkCommand
        {
            get => _selectedKillerThirdPerkCommand ??= new(obj =>
            {
                SelectedKillerThirdPerk = SelectedKillerPerk;
                SelectedKillerThirdPerkName = SelectedKillerPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора перка в 4 позицию для ContextMenu у элемента ListView KillerPerk
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedKillerFourthPerkCommand;
        public RelayCommand SelectedKillerFourthPerkCommand
        {
            get => _selectedKillerFourthPerkCommand ??= new(obj =>
            {
                SelectedKillerFourthPerk = SelectedKillerPerk;
                SelectedKillerFourthPerkName = SelectedKillerPerk.PerkName;
            });
        }

        #endregion

        #region Билд - Аддоны - Свойства

        /// <summary>
        /// Свойство для отслеживание выбранного улучшения "Убийцы" из списка
        /// </summary>
        private KillerAddon _selectedKillerAddon;
        public KillerAddon SelectedKillerAddon
        {
            get => _selectedKillerAddon;
            set
            {
                _selectedKillerAddon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отслеживание первого выбранного улучшения "Убийцы" 
        /// </summary>
        private KillerAddon _selectedKillerFirstAddon;
        public KillerAddon SelectedKillerFirstAddon
        {
            get => _selectedKillerFirstAddon;
            set
            {
                if (value != null)
                {
                    _selectedKillerFirstAddon = value;
                    SelectedKillerFirstAddonName = value.AddonName;
                }
                else { SelectedKillerFirstAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отслеживание второго выбранного улучшения "Убийцы" 
        /// </summary>
        private KillerAddon _selectedKillerSecondAddon;
        public KillerAddon SelectedKillerSecondAddon
        {
            get => _selectedKillerSecondAddon;
            set
            {
                if (value != null)
                {
                    _selectedKillerSecondAddon = value;
                    SelectedKillerSecondAddonName = value.AddonName;
                }
                else { SelectedKillerSecondAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия первого выбранного улучшения "Убийцы"
        /// </summary>
        private string _selectedKillerFirstAddonName;
        public string SelectedKillerFirstAddonName
        {
            get => _selectedKillerFirstAddonName;
            set
            {
                _selectedKillerFirstAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия второго выбранного улучшения "Убийцы"
        /// </summary>
        private string _selectedKillerSecondAddonName;
        public string SelectedKillerSecondAddonName
        {
            get => _selectedKillerSecondAddonName;
            set
            {
                _selectedKillerSecondAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений
        /// </summary
        private string _searchKillerAddon;
        public string SearchKillerAddon
        {
            get => _searchKillerAddon;
            set
            {
                _searchKillerAddon = value;
                SearchKillerAddons();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Аддоны Команды

        /// <summary>
        /// Команда выбора аддона в 1 позицию для ContextMenu у элемента ListView KillerPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedKillerFirstAddonCommand;
        public RelayCommand SelectedKillerFirstAddonCommand { get => _selectedKillerFirstAddonCommand ??= new(obj => { SelectedKillerFirstAddon = SelectedKillerAddon; }); }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView KillerPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedKillerSecondAddonCommand;
        public RelayCommand SelectedKillerSecondAddonCommand { get => _selectedKillerSecondAddonCommand ??= new(obj => { SelectedKillerSecondAddon = SelectedKillerAddon; }); }

        #endregion

        #region Билд - Подношения Свойства 

        /// <summary>
        /// Свойство для отслеживание выбранного подношение "Убийцы" из списка (ListView)
        /// </summary>
        private Offering _selectedKillerOffering;
        public Offering SelectedKillerOffering
        {
            get => _selectedKillerOffering;
            set
            {
                if (value != null)
                {
                    _selectedKillerOffering = value;
                    SelectedKillerOfferingName = value.OfferingName;
                }
                else { SelectedKillerOfferingName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойства для поиска по списку подношений (ListView)
        /// </summary>
        private string _searchKillerOffering;
        public string SearchKillerOffering
        {
            get => _searchKillerOffering;
            set
            {
                _searchKillerOffering = value;
                SearchKillerOfferings();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойства для отображение название подношения в TextBox
        /// </summary>
        private string _selectedKillerOfferingName;
        public string SelectedKillerOfferingName
        {
            get => _selectedKillerOfferingName;
            set
            {
                _selectedKillerOfferingName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о выбранной роли. Нужно для того, что б загрузить нужные список подношений
        /// </summary>
        private Role _selectedRoleKiller;
        public Role SelectedRoleKiller
        {
            get => _selectedRoleKiller;
            set
            {
                _selectedRoleKiller = value;
                //GetKillerOfferingListData();
                LoadKillerOfferings();
                OnPropertyChanged();

            }
        }

        #endregion

        #region Ассоциация Свойства

        /// <summary>
        /// Выбор с кем ассоциируется "Убийца". "Я" или "Рандомный игрок\Противник"
        /// </summary>
        private PlayerAssociation _selectedKillerPlayerAssociation;
        public PlayerAssociation SelectedKillerPlayerAssociation
        {
            get => _selectedKillerPlayerAssociation;
            set
            {
                _selectedKillerPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Popup - Билд киллера

        private bool _isOpenKillerBuildPopup;
        public bool IssOpenKillerBuildPopup
        {
            get => _isOpenKillerBuildPopup;
            set
            {
                _isOpenKillerBuildPopup = value;
                OnPropertyChanged();
            }
        }

        private string _nameBuild;
        public string NameBuild
        {
            get => _nameBuild;
            set
            {
                _nameBuild = value;
                OnPropertyChanged();
            }
        }

        private bool _considerAddon;
        public bool ConsiderAddon
        {
            get => _considerAddon;
            set
            {
                _considerAddon = value;
                OnPropertyChanged();
            }
        }

        private bool _considerKiller;
        public bool ConsiderKiller
        {
            get => _considerKiller;
            set
            {
                _considerKiller = value;
                OnPropertyChanged();
            }
        }

        private KillerBuild _selectedKillerBuild;
        public KillerBuild SelectedKillerBuild
        {
            get => _selectedKillerBuild;
            set
            {
                if (_selectedKillerBuild != value)
                {
                    _selectedKillerBuild = value;
                    OnPropertyChanged();
                    SelectBuild(value);
                }
            }
        }

        private RelayCommand _openSelectKillerBuildPopupCommand;
        public RelayCommand OpenSelectKillerBuildPopupCommand { get => _openSelectKillerBuildPopupCommand ??= new(obj => { IssOpenKillerBuildPopup = true; }); }

        private RelayCommand _addKillerBuildCommand;
        public RelayCommand AddKillerBuildCommand { get => _addKillerBuildCommand ??= new(obj => { AddKillerBuild(); }); }

        private RelayCommand _deleteKillerBuildCommand;
        public RelayCommand DeleteKillerBuildCommand { get => _deleteKillerBuildCommand ??= new(async obj =>
        {
            if (SelectedKillerBuild == null)
            {
                return;
            }
            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedKillerBuild.Name}»? При удаление данном записи, могут быть связанные записи в других таблицах, что может привести к проблемам.",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedKillerBuild);
                LoadKillerBuilds();
            }
            else
            {
                return;
            }

        }); }

        private void AddKillerBuild()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedKiller != null &&
                    SelectedKillerFirstPerk != null &&
                    SelectedKillerSecondPerk != null &&
                    SelectedKillerThirdPerk != null &&
                    SelectedKillerFourthPerk != null &&
                    SelectedKillerFirstAddon != null &&
                    SelectedKillerSecondAddon != null)
                {
                    KillerBuild killerBuild = new()
                    {
                        Name = NameBuild,
                        IdKiller = SelectedKiller.IdKiller,
                        IdPerk1 = SelectedKillerFirstPerk.IdKillerPerk,
                        IdPerk2 = SelectedKillerSecondPerk.IdKillerPerk,
                        IdPerk3 = SelectedKillerThirdPerk.IdKillerPerk,
                        IdPerk4 = SelectedKillerFourthPerk.IdKillerPerk,
                        IdAddon1 = SelectedKillerFirstAddon.IdKillerAddon,
                        IdAddon2 = SelectedKillerSecondAddon.IdKillerAddon,
                    };

                    context.KillerBuilds.Add(killerBuild);
                    context.SaveChanges();
                    NameBuild = string.Empty;
                    LoadKillerBuilds();
                }
                else
                {
                    _dialogService.ShowMessage("Если у вас что то выбрано, либо в билде нету перка, аддона, то выберите <<Отсутствует>>");
                }
            }
        }

        private void SelectBuild(KillerBuild value)
        {
            if (value != null)
            {
                SelectedKillerFirstPerk = KillerPerks.FirstOrDefault(x => x.IdKillerPerk == value.IdPerk1);
                SelectedKillerSecondPerk = KillerPerks.FirstOrDefault(x => x.IdKillerPerk == value.IdPerk2);
                SelectedKillerThirdPerk = KillerPerks.FirstOrDefault(x => x.IdKillerPerk == value.IdPerk3);
                SelectedKillerFourthPerk = KillerPerks.FirstOrDefault(x => x.IdKillerPerk == value.IdPerk4);

                if (ConsiderKiller == true)
                {
                    SelectedKiller = Killers.FirstOrDefault(x => x.IdKiller == value.IdKiller);
                }
                if (ConsiderAddon == true)
                {
                    SelectedKiller = Killers.FirstOrDefault(x => x.IdKiller == value.IdKiller);
                    SelectedKillerFirstAddon = _dataService.GetById<KillerAddon>(value.IdAddon1, "IdKillerAddon");
                    SelectedKillerSecondAddon = _dataService.GetById<KillerAddon>(value.IdAddon2, "IdKillerAddon"); ;
                }
            }
        }

        #endregion

        #endregion

        #region Блок "Выжившие"

        #region Общие свойства для отслеживание выбранных Перков \ Улучшений к предметам у всех четверых выживших

        /// <summary>
        /// Свойство для отслеживание выбранного перка выжившего из списка (ListView)
        /// </summary>
        private SurvivorPerk _selectedSurvivorPerk;
        public SurvivorPerk SelectedSurvivorPerk
        {
            get => _selectedSurvivorPerk;
            set
            {
                if (value != null)
                {
                    _selectedSurvivorPerk = value;
                    OnPropertyChanged();
                }
                else { return; }

            }
        }

        /// <summary>
        /// Свойство для поиска по списку перков выжившего
        /// </summary
        private string _searchSurvivorPerk;
        public string SearchSurvivorPerk
        {
            get => _searchSurvivorPerk;
            set
            {
                _searchSurvivorPerk = value;
                SearchSurvivorPerks();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о выбранном улучшение предмета для ListView ItemAddon
        /// </summary>
        private ItemAddon _selectedItemAddon;
        public ItemAddon SelectedItemAddon
        {
            get => _selectedItemAddon;
            set
            {
                _selectedItemAddon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Основная информация Выжившего №1

        /// <summary>
        /// Свойство для отслеживание первого выбранного выжившего
        /// </summary>
        private Survivor _selectedFirstSurvivor;
        public Survivor SelectedFirstSurvivor
        {
            get => _selectedFirstSurvivor;
            set
            {
                _selectedFirstSurvivor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Отслеживание платформы на которой играет первый выживший 
        /// </summary>
        private Platform _selectedFirstSurvivorPlatform;
        public Platform SelectedFirstSurvivorPlatform
        {
            get => _selectedFirstSurvivorPlatform;
            set
            {
                _selectedFirstSurvivorPlatform = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Престиж первого выжившего
        /// </summary>
        private int _firstSurvivorPrestige;
        public int FirstSurvivorPrestige
        {
            get => _firstSurvivorPrestige;
            set
            {
                if (!CheckPrestige(value)) MessageHelper.PrestigeMessage();
                else
                {
                    _firstSurvivorPrestige = value;
                    OnPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Как был убит первый выживший 
        /// </summary>
        private TypeDeath _selectedFirstSurvivorTypeDeath;
        public TypeDeath SelectedFirstSurvivorTypeDeath
        {
            get => _selectedFirstSurvivorTypeDeath;
            set
            {
                _selectedFirstSurvivorTypeDeath = value;
                CheckKills();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Счет очков первого выжившего по окончанию игры
        /// </summary>
        private int _firstSurvivorAccount;
        public int FirstSurvivorAccount
        {
            get => _firstSurvivorAccount;
            set
            {
                _firstSurvivorAccount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Какой режим приватности был у первого выжившего? Анонимный или нет
        /// </summary>
        private bool _firstSurvivorAnonymousMode;
        public bool FirstSurvivorAnonymousMode
        {
            get => _firstSurvivorAnonymousMode;
            set
            {
                _firstSurvivorAnonymousMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вышел ли первый выживший до окончания игры или нет
        /// </summary>
        private bool _firstSurvivorBot;
        public bool FirstSurvivorBot
        {
            get => _firstSurvivorBot;
            set
            {
                _firstSurvivorBot = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Перки Выжившего №1 - Свойства

        /// <summary>
        /// Свойство для хранение информации о первом выбранном перке у первого выжившего
        /// </summary>
        private SurvivorPerk _selectedFirstSurvivorFirstPerk;
        public SurvivorPerk SelectedFirstSurvivorFirstPerk
        {
            get => _selectedFirstSurvivorFirstPerk;
            set
            {
                if (value != null)
                {
                    _selectedFirstSurvivorFirstPerk = value;
                    SelectedFirstSurvivorFirstPerkName = value.PerkName;
                }
                else { SelectedFirstSurvivorFirstPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором выбранном перке у первого выжившего
        /// </summary>
        private SurvivorPerk _selectedFirstSurvivorSecondPerk;
        public SurvivorPerk SelectedFirstSurvivorSecondPerk
        {
            get => _selectedFirstSurvivorSecondPerk;
            set
            {
                if (value != null)
                {
                    _selectedFirstSurvivorSecondPerk = value;
                    SelectedFirstSurvivorSecondPerkName = value.PerkName;
                }
                else { SelectedFirstSurvivorSecondPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о третьем выбранном перке у первого выжившего
        /// </summary>
        private SurvivorPerk _selectedFirstSurvivorThirdPerk;
        public SurvivorPerk SelectedFirstSurvivorThirdPerk
        {
            get => _selectedFirstSurvivorThirdPerk;
            set
            {
                if (value != null)
                {
                    _selectedFirstSurvivorThirdPerk = value;
                    SelectedFirstSurvivorThirdPerkName = value.PerkName;
                }
                else { SelectedFirstSurvivorThirdPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о четвертом выбранном перке у первого выжившего
        /// </summary>
        private SurvivorPerk _selectedFirstSurvivorFourthPerk;
        public SurvivorPerk SelectedFirstSurvivorFourthPerk
        {
            get => _selectedFirstSurvivorFourthPerk;
            set
            {
                if (value != null)
                {
                    _selectedFirstSurvivorFourthPerk = value;
                    SelectedFirstSurvivorFourthPerkName = value.PerkName;
                }
                else { SelectedFirstSurvivorFourthPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия первого выбранного перка у первого выжившего
        /// </summary> 
        private string _selectedFirstSurvivorFirstPerkName;
        public string SelectedFirstSurvivorFirstPerkName
        {
            get => _selectedFirstSurvivorFirstPerkName;
            set
            {
                _selectedFirstSurvivorFirstPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия второго выбранного перка у первого выжившего
        /// </summary> 
        private string _selectedFirstSurvivorSecondPerkName;
        public string SelectedFirstSurvivorSecondPerkName
        {
            get => _selectedFirstSurvivorSecondPerkName;
            set
            {
                _selectedFirstSurvivorSecondPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия третьего выбранного перка у первого выжившего
        /// </summary> 
        private string _selectedFirstSurvivorThirdPerkName;
        public string SelectedFirstSurvivorThirdPerkName
        {
            get => _selectedFirstSurvivorThirdPerkName;
            set
            {
                _selectedFirstSurvivorThirdPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия четвертого выбранного перка у первого выжившего
        /// </summary> 
        private string _selectedFirstSurvivorFourthPerkName;
        public string SelectedFirstSurvivorFourthPerkName
        {
            get => _selectedFirstSurvivorFourthPerkName;
            set
            {
                _selectedFirstSurvivorFourthPerkName = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        #region Билд - Перки Выжившего №1 - Команды

        /// <summary>
        /// Команда выбора аддона в 1 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorFirstPerkCommand;
        public RelayCommand SelectedFirstSurvivorFirstPerkCommand
        { get => _selectedFirstSurvivorFirstPerkCommand ??= new(obj => { SelectedFirstSurvivorFirstPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorSecondPerkCommand;
        public RelayCommand SelectedFirstSurvivorSecondPerkCommand { get => _selectedFirstSurvivorSecondPerkCommand ??= new(obj => { SelectedFirstSurvivorSecondPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 3 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorThirdPerkCommand;
        public RelayCommand SelectedFirstSurvivorThirdPerkCommand { get => _selectedFirstSurvivorThirdPerkCommand ??= new(obj => { SelectedFirstSurvivorThirdPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 4 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorFourthPerkCommand;
        public RelayCommand SelectedFirstSurvivorFourthPerkCommand { get => _selectedFirstSurvivorFourthPerkCommand ??= new(obj => { SelectedFirstSurvivorFourthPerk = SelectedSurvivorPerk; }); }


        #endregion

        #region Билд - Предметы Выжившего №1 - Свойства

        /// <summary>
        /// Свойство для выбора предмета у первого выжившего
        /// </summary>
        private Item _selectedFirstSurvivorItem;
        public Item SelectedFirstSurvivorItem
        {
            get => _selectedFirstSurvivorItem;
            set
            {
                _selectedFirstSurvivorItem = value;
                SearchFirstSurvivorItemAddon = string.Empty;
                LoadFirstSurvivorItemAddons();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о первом улучшение предмета
        /// </summary>
        private ItemAddon _selectedFirstSurvivorFirstItemAddon;
        public ItemAddon SelectedFirstSurvivorFirstItemAddon
        {
            get => _selectedFirstSurvivorFirstItemAddon;
            set
            {
                if (value != null)
                {
                    _selectedFirstSurvivorFirstItemAddon = value;
                    SelectedFirstSurvivorFirstItemAddonName = value.ItemAddonName;
                }
                else { SelectedFirstSurvivorFirstItemAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором улучшение предмета
        /// </summary>
        private ItemAddon _selectedFirstSurvivorSecondItemAddon;
        public ItemAddon SelectedFirstSurvivorSecondItemAddon
        {
            get => _selectedFirstSurvivorSecondItemAddon;
            set
            {
                if (value != null)
                {
                    _selectedFirstSurvivorSecondItemAddon = value;
                    SelectedFirstSurvivorSecondItemAddonName = value.ItemAddonName;
                }
                else { SelectedFirstSurvivorSecondItemAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение информации о первом улучшение предмета у первого выжившего
        /// </summary>
        private string _selectedFirstSurvivorFirstItemAddonName;
        public string SelectedFirstSurvivorFirstItemAddonName
        {
            get => _selectedFirstSurvivorFirstItemAddonName;
            set
            {
                _selectedFirstSurvivorFirstItemAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение информации о втором улучшение предмета у первого выжившего
        /// </summary>
        private string _selectedFirstSurvivorSecondItemAddonName;
        public string SelectedFirstSurvivorSecondItemAddonName
        {
            get => _selectedFirstSurvivorSecondItemAddonName;
            set
            {
                _selectedFirstSurvivorSecondItemAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений для предмета у первого выжившего
        /// </summary
        private string _searchFirstSurvivorItemAddon;
        public string SearchFirstSurvivorItemAddon
        {
            get => _searchFirstSurvivorItemAddon;
            set
            {
                _searchFirstSurvivorItemAddon = value;
                FirstSurvivorItemAddons.Clear();
                SearchFirstSurvivorItemAddons();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Предметы Выжившего №1 - Команды

        /// <summary>
        /// Команда выбора улучшение предмета в 1 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedListViewSurvivorFirstItemAddonCommand { get => _selectedFirstSurvivorFirstItemAddonCommand ??= new(obj => { SelectedFirstSurvivorFirstItemAddon = SelectedItemAddon; }); }

        /// <summary>
        /// Команда выбора улучшение предмета в 2 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedListViewSurvivorSecondItemAddonCommand { get => _selectedFirstSurvivorSecondItemAddonCommand ??= new(obj => { SelectedFirstSurvivorSecondItemAddon = SelectedItemAddon; }); }

        #endregion

        #region Билд - Подношение Выжившего №1 - Свойства

        /// <summary>
        /// Свойство для хранение информации о выбранном подношение у первого выжившего
        /// </summary>
        private Offering _selectedFirstSurvivorOffering;
        public Offering SelectedFirstSurvivorOffering
        {
            get => _selectedFirstSurvivorOffering;
            set
            {
                if (value != null)
                {
                    _selectedFirstSurvivorOffering = value;
                    SelectedFirstSurvivorOfferingName = value.OfferingName;
                }
                else { SelectedFirstSurvivorOfferingName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойства для отображение название подношения в TextBox
        /// </summary>
        private string _selectedFirstSurvivorOfferingName;
        public string SelectedFirstSurvivorOfferingName
        {
            get => _selectedFirstSurvivorOfferingName;
            set
            {
                _selectedFirstSurvivorOfferingName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о выбранной роли. Нужно для того, что б загрузить нужные список подношений
        /// </summary>
        private Role _selectedRoleFirstSurvivor;
        public Role SelectedRoleFirstSurvivor
        {
            get => _selectedRoleFirstSurvivor;
            set
            {
                _selectedRoleFirstSurvivor = value;
                LoadFirstSurvivorOfferings();
                OnPropertyChanged();

            }
        }

        /// <summary>
        /// Свойства для поиска по списку подношений (ListView)
        /// </summary>
        private string _searchFirstSurvivorOffering;
        public string SearchFirstSurvivorOffering
        {
            get => _searchFirstSurvivorOffering;
            set
            {
                _searchFirstSurvivorOffering = value;
                SearchFirstSurvivorOfferings();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Ассоциация - Выжившего №1 - СВойства

        // <summary>
        /// Выбор с кем ассоциируется первый выживший. "Я", "Напарник", "Рандом"
        /// </summary>
        private PlayerAssociation _selectedFirstSurvivorPlayerAssociation;
        public PlayerAssociation SelectedFirstSurvivorPlayerAssociation
        {
            get => _selectedFirstSurvivorPlayerAssociation;
            set
            {
                _selectedFirstSurvivorPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Основная информация Выжившего №2

        /// <summary>
        /// Отслеживание второго выбранного выжившего
        /// </summary>
        private Survivor _selectedSecondSurvivor;
        public Survivor SelectedSecondSurvivor
        {
            get => _selectedSecondSurvivor;
            set
            {
                _selectedSecondSurvivor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Отслеживание платформы на которой играет второго выживший 
        /// </summary>
        private Platform _selectedSecondSurvivorPlatform;
        public Platform SelectedSecondSurvivorPlatform
        {
            get => _selectedSecondSurvivorPlatform;
            set
            {
                _selectedSecondSurvivorPlatform = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Престиж второго выжившего
        /// </summary>
        private int _secondSurvivorPrestige;
        public int SecondSurvivorPrestige
        {
            get => _secondSurvivorPrestige;
            set
            {
                if (!CheckPrestige(value)) MessageHelper.PrestigeMessage();
                else
                {
                    _secondSurvivorPrestige = value;
                    OnPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Как был убит второй выживший 
        /// </summary>
        private TypeDeath _selectedSecondSurvivorTypeDeath;
        public TypeDeath SelectedSecondSurvivorTypeDeath
        {
            get => _selectedSecondSurvivorTypeDeath;
            set
            {
                _selectedSecondSurvivorTypeDeath = value;
                CheckKills();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Счет очков второго выжившего по окончанию игры
        /// </summary>
        private int _secondSurvivorAccount;
        public int SecondSurvivorAccount
        {
            get => _secondSurvivorAccount;
            set
            {
                _secondSurvivorAccount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Какой режим приватности был у второго выжившего? Анонимный или нет
        /// </summary>
        private bool _secondSurvivorAnonymousMode;
        public bool SecondSurvivorAnonymousMode
        {
            get => _secondSurvivorAnonymousMode;
            set
            {
                _secondSurvivorAnonymousMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вышел ли выживший до окончания игры или нет
        /// </summary>
        private bool _secondSurvivorBot;
        public bool SecondSurvivorBot
        {
            get => _secondSurvivorBot;
            set
            {
                _secondSurvivorBot = value;
                OnPropertyChanged();
            }
        }

        #endregion  

        #region Билд - Перки Выжившего №2 - Свойства

        /// <summary>
        /// Свойство для хранение информации о первом выбранном перке 
        /// </summary>
        private SurvivorPerk _selectedSecondSurvivorFirstPerk;
        public SurvivorPerk SelectedSecondSurvivorFirstPerk
        {
            get => _selectedSecondSurvivorFirstPerk;
            set
            {
                if (value != null)
                {
                    _selectedSecondSurvivorFirstPerk = value;
                    SelectedSecondSurvivorFirstPerkName = value.PerkName;
                }
                else { SelectedSecondSurvivorFirstPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором выбранном перке
        /// </summary>
        private SurvivorPerk _selectedSecondSurvivorSecondPerk;
        public SurvivorPerk SelectedSecondSurvivorSecondPerk
        {
            get => _selectedSecondSurvivorSecondPerk;
            set
            {
                if (value != null)
                {
                    _selectedSecondSurvivorSecondPerk = value;
                    SelectedSecondSurvivorSecondPerkName = value.PerkName;
                }
                else { SelectedSecondSurvivorSecondPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о третьем выбранном перке
        /// </summary>
        private SurvivorPerk _selectedSecondSurvivorThirdPerk;
        public SurvivorPerk SelectedSecondSurvivorThirdPerk
        {
            get => _selectedSecondSurvivorThirdPerk;
            set
            {
                if (value != null)
                {
                    _selectedSecondSurvivorThirdPerk = value;
                    SelectedSecondSurvivorThirdPerkName = value.PerkName;
                }
                else { SelectedSecondSurvivorThirdPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о четвертом выбранном перке
        /// </summary>
        private SurvivorPerk _selectedSecondSurvivorFourthPerk;
        public SurvivorPerk SelectedSecondSurvivorFourthPerk
        {
            get => _selectedSecondSurvivorFourthPerk;
            set
            {
                if (value != null)
                {
                    _selectedSecondSurvivorFourthPerk = value;
                    SelectedSecondSurvivorFourthPerkName = value.PerkName;
                }
                else { SelectedSecondSurvivorFourthPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия первого выбранного перка
        /// </summary> 
        private string _selectedSecondSurvivorFirstPerkName;
        public string SelectedSecondSurvivorFirstPerkName
        {
            get => _selectedSecondSurvivorFirstPerkName;
            set
            {
                _selectedSecondSurvivorFirstPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия второго выбранного перка
        /// </summary> 
        private string _selectedSecondSurvivorSecondPerkName;
        public string SelectedSecondSurvivorSecondPerkName
        {
            get => _selectedSecondSurvivorSecondPerkName;
            set
            {
                _selectedSecondSurvivorSecondPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия третьего выбранного перка
        /// </summary> 
        private string _selectedSecondSurvivorThirdPerkName;
        public string SelectedSecondSurvivorThirdPerkName
        {
            get => _selectedSecondSurvivorThirdPerkName;
            set
            {
                _selectedSecondSurvivorThirdPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия четвертого выбранного перка 
        /// </summary> 
        private string _selectedSecondSurvivorFourthPerkName;
        public string SelectedSecondSurvivorFourthPerkName
        {
            get => _selectedSecondSurvivorFourthPerkName;
            set
            {
                _selectedSecondSurvivorFourthPerkName = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        #region Билд - Перки Выжившего №2 - Команды

        /// <summary>
        /// Команда выбора аддона в 1 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorFirstPerkCommand;
        public RelayCommand SelectedSecondSurvivorFirstPerkCommand { get => _selectedSecondSurvivorFirstPerkCommand ??= new(obj => { SelectedSecondSurvivorFirstPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorSecondPerkCommand;
        public RelayCommand SelectedSecondSurvivorSecondPerkCommand { get => _selectedSecondSurvivorSecondPerkCommand ??= new(obj => { SelectedSecondSurvivorSecondPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 3 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorThirdPerkCommand;
        public RelayCommand SelectedSecondSurvivorThirdPerkCommand { get => _selectedSecondSurvivorThirdPerkCommand ??= new(obj => { SelectedSecondSurvivorThirdPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 4 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorFourthPerkCommand;
        public RelayCommand SelectedSecondSurvivorFourthPerkCommand { get => _selectedSecondSurvivorFourthPerkCommand ??= new(obj => { SelectedSecondSurvivorFourthPerk = SelectedSurvivorPerk; }); }

        #endregion

        #region Билд - Предметы Выжившего №2 - Свойства

        /// <summary>
        /// Свойство для выбора предмета у второго выжившего
        /// </summary>
        private Item _selectedSecondSurvivorItem;
        public Item SelectedSecondSurvivorItem
        {
            get => _selectedSecondSurvivorItem;
            set
            {
                _selectedSecondSurvivorItem = value;
                SearchSecondSurvivorItemAddon = string.Empty;
                LoadSecondSurvivorItemAddons();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о первом улучшение предмета
        /// </summary>
        private ItemAddon _selectedSecondSurvivorFirstItemAddon;
        public ItemAddon SelectedSecondSurvivorFirstItemAddon
        {
            get => _selectedSecondSurvivorFirstItemAddon;
            set
            {
                if (value != null)
                {
                    _selectedSecondSurvivorFirstItemAddon = value;
                    SelectedSecondSurvivorFirstItemAddonName = value.ItemAddonName;
                }
                else { SelectedSecondSurvivorFirstItemAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором улучшение предмета
        /// </summary>
        private ItemAddon _selectedSecondSurvivorSecondItemAddon;
        public ItemAddon SelectedSecondSurvivorSecondItemAddon
        {
            get => _selectedSecondSurvivorSecondItemAddon;
            set
            {
                if (value != null)
                {
                    _selectedSecondSurvivorSecondItemAddon = value;
                    SelectedSecondSurvivorSecondItemAddonName = value.ItemAddonName;
                }
                else { SelectedSecondSurvivorSecondItemAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение информации о первом улучшение предмета у второго выжившего
        /// </summary>
        private string _selectedSecondSurvivorFirstItemAddonName;
        public string SelectedSecondSurvivorFirstItemAddonName
        {
            get => _selectedSecondSurvivorFirstItemAddonName;
            set
            {
                _selectedSecondSurvivorFirstItemAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение информации о втором улучшение предмета у второго выжившего
        /// </summary>
        private string _selectedSecondSurvivorSecondItemAddonName;
        public string SelectedSecondSurvivorSecondItemAddonName
        {
            get => _selectedSecondSurvivorSecondItemAddonName;
            set
            {
                _selectedSecondSurvivorSecondItemAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений для предмета у второго выжившего
        /// </summary
        private string _searchSecondSurvivorItemAddon;
        public string SearchSecondSurvivorItemAddon
        {
            get => _searchSecondSurvivorItemAddon;
            set
            {
                _searchSecondSurvivorItemAddon = value;
                SecondSurvivorItemAddons.Clear();
                SearchSecondSurvivorItemAddons();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Предметы Выжившего №2 - Команды

        /// <summary>
        /// Команда выбора улучшение предмета в 1 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedSecondSurvivorFirstItemAddonCommand { get => _selectedSecondSurvivorFirstItemAddonCommand ??= new(obj => { SelectedSecondSurvivorFirstItemAddon = SelectedItemAddon; }); }

        /// <summary>
        /// Команда выбора улучшение предмета в 2 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedSecondSurvivorSecondItemAddonCommand { get => _selectedSecondSurvivorSecondItemAddonCommand ??= new(obj => { SelectedSecondSurvivorSecondItemAddon = SelectedItemAddon; }); }

        #endregion

        #region Билд - Подношение Выжившего №2 - Свойства

        /// <summary>
        /// Свойство для хранение информации о выбранном подношение у первого выжившего
        /// </summary>
        private Offering _selectedSecondSurvivorOffering;
        public Offering SelectedSecondSurvivorOffering
        {
            get => _selectedSecondSurvivorOffering;
            set
            {
                if (value != null)
                {
                    _selectedSecondSurvivorOffering = value;
                    SelectedSecondSurvivorOfferingName = value.OfferingName;
                }
                else { SelectedSecondSurvivorOfferingName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойства для отображение название подношения в TextBox
        /// </summary>
        private string _selectedSecondSurvivorOfferingName;
        public string SelectedSecondSurvivorOfferingName
        {
            get => _selectedSecondSurvivorOfferingName;
            set
            {
                _selectedSecondSurvivorOfferingName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о выбранной роли. Нужно для того, что б загрузить нужные список подношений
        /// </summary>
        private Role _selectedRoleSecondSurvivor;
        public Role SelectedRoleSecondSurvivor
        {
            get => _selectedRoleSecondSurvivor;
            set
            {
                _selectedRoleSecondSurvivor = value;
                LoadSecondSurvivorOfferings();
                OnPropertyChanged();

            }
        }

        /// <summary>
        /// Свойства для поиска по списку подношений (ListView)
        /// </summary>
        private string _searchSecondSSurvivorOffering;
        public string SearchSecondSurvivorOffering
        {
            get => _searchSecondSSurvivorOffering;
            set
            {
                _searchSecondSSurvivorOffering = value;
                SearchSecondSurvivorOfferings();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Ассоциация - Выжившего №2 - СВойства

        // <summary>
        /// Выбор с кем ассоциируется первый выживший. "Я", "Напарник", "Рандом"
        /// </summary>
        private PlayerAssociation _selectedSecondSurvivorPlayerAssociation;
        public PlayerAssociation SelectedSecondSurvivorPlayerAssociation
        {
            get => _selectedSecondSurvivorPlayerAssociation;
            set
            {
                _selectedSecondSurvivorPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Основная информация Выжившего №3

        /// <summary>
        /// Отслеживание третьего выбранного выжившего
        /// </summary>
        private Survivor _selectedThirdSurvivor;
        public Survivor SelectedThirdSurvivor
        {
            get => _selectedThirdSurvivor;
            set
            {
                _selectedThirdSurvivor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Отслеживание платформы на которой играет второго выживший 
        /// </summary>
        private Platform _selectedThirdSurvivorPlatform;
        public Platform SelectedThirdSurvivorPlatform
        {
            get => _selectedThirdSurvivorPlatform;
            set
            {
                _selectedThirdSurvivorPlatform = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Престиж третьего выжившего
        /// </summary>
        private int _thirdSurvivorPrestige;
        public int ThirdSurvivorPrestige
        {
            get => _thirdSurvivorPrestige;
            set
            {
                if (!CheckPrestige(value)) MessageHelper.PrestigeMessage();
                else
                {
                    _thirdSurvivorPrestige = value;
                    OnPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Как был убит третьего выживший 
        /// </summary>
        private TypeDeath _selectedThirdSurvivorTypeDeath;
        public TypeDeath SelectedThirdSurvivorTypeDeath
        {
            get => _selectedThirdSurvivorTypeDeath;
            set
            {
                _selectedThirdSurvivorTypeDeath = value;
                CheckKills();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Счет очков у третьего выжившего по окончанию игры
        /// </summary>
        private int _thirdSurvivorAccount;
        public int ThirdSurvivorAccount
        {
            get => _thirdSurvivorAccount;
            set
            {
                _thirdSurvivorAccount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Какой режим приватности был у третьего выжившего? Анонимный или нет
        /// </summary>
        private bool _thirdSurvivorAnonymousMode;
        public bool ThirdSurvivorAnonymousMode
        {
            get => _thirdSurvivorAnonymousMode;
            set
            {
                _thirdSurvivorAnonymousMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вышел ли выживший до окончания игры или нет
        /// </summary>
        private bool _thirdSurvivorBot;
        public bool ThirdSurvivorBot
        {
            get => _thirdSurvivorBot;
            set
            {
                _thirdSurvivorBot = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        #region Билд - Перки Выжившего №3 - Свойства

        /// <summary>
        /// Свойство для хранение информации о первом выбранном перке 
        /// </summary>
        private SurvivorPerk _selectedThirdSurvivorFirstPerk;
        public SurvivorPerk SelectedThirdSurvivorFirstPerk
        {
            get => _selectedThirdSurvivorFirstPerk;
            set
            {
                if (value != null)
                {
                    _selectedThirdSurvivorFirstPerk = value;
                    SelectedThirdSurvivorFirstPerkName = value.PerkName;
                }
                else { SelectedThirdSurvivorFirstPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором выбранном перке
        /// </summary>
        private SurvivorPerk _selectedThirdSurvivorSecondPerk;
        public SurvivorPerk SelectedThirdSurvivorSecondPerk
        {
            get => _selectedThirdSurvivorSecondPerk;
            set
            {
                if (value != null)
                {
                    _selectedThirdSurvivorSecondPerk = value;
                    SelectedThirdSurvivorSecondPerkName = value.PerkName;
                }
                else { SelectedThirdSurvivorSecondPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о третьем выбранном перке
        /// </summary>
        private SurvivorPerk _selectedThirdSurvivorThirdPerk;
        public SurvivorPerk SelectedThirdSurvivorThirdPerk
        {
            get => _selectedThirdSurvivorThirdPerk;
            set
            {
                if (value != null)
                {
                    _selectedThirdSurvivorThirdPerk = value;
                    SelectedThirdSurvivorThirdPerkName = value.PerkName;
                }
                else { SelectedThirdSurvivorThirdPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о четвертом выбранном перке
        /// </summary>
        private SurvivorPerk _selectedThirdSurvivorFourthPerk;
        public SurvivorPerk SelectedThirdSurvivorFourthPerk
        {
            get => _selectedThirdSurvivorFourthPerk;
            set
            {
                if (value != null)
                {
                    _selectedThirdSurvivorFourthPerk = value;
                    SelectedThirdSurvivorFourthPerkName = value.PerkName;
                }
                else { SelectedThirdSurvivorFourthPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия первого выбранного перка
        /// </summary> 
        private string _selectedThirdSurvivorFirstPerkName;
        public string SelectedThirdSurvivorFirstPerkName
        {
            get => _selectedThirdSurvivorFirstPerkName;
            set
            {
                _selectedThirdSurvivorFirstPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия второго выбранного перка
        /// </summary> 
        private string _selectedThirdSurvivorSecondPerkName;
        public string SelectedThirdSurvivorSecondPerkName
        {
            get => _selectedThirdSurvivorSecondPerkName;
            set
            {
                _selectedThirdSurvivorSecondPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия третьего выбранного перка
        /// </summary> 
        private string _selectedThirdSurvivorThirdPerkName;
        public string SelectedThirdSurvivorThirdPerkName
        {
            get => _selectedThirdSurvivorThirdPerkName;
            set
            {
                _selectedThirdSurvivorThirdPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия четвертого выбранного перка 
        /// </summary> 
        private string _selectedThirdSurvivorFourthPerkName;
        public string SelectedThirdSurvivorFourthPerkName
        {
            get => _selectedThirdSurvivorFourthPerkName;
            set
            {
                _selectedThirdSurvivorFourthPerkName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Перки Выжившего №3 - Команды

        /// <summary>
        /// Команда выбора аддона в 1 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorFirstPerkCommand;
        public RelayCommand SelectedThirdSurvivorFirstPerkCommand { get => _selectedThirdSurvivorFirstPerkCommand ??= new(obj => { SelectedThirdSurvivorFirstPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorSecondPerkCommand;
        public RelayCommand SelectedThirdSurvivorSecondPerkCommand { get => _selectedThirdSurvivorSecondPerkCommand ??= new(obj => { SelectedThirdSurvivorSecondPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 3 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorThirdPerkCommand;
        public RelayCommand SelectedThirdSurvivorThirdPerkCommand { get => _selectedThirdSurvivorThirdPerkCommand ??= new(obj => { SelectedThirdSurvivorThirdPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 4 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorFourthPerkCommand;
        public RelayCommand SelectedThirdSurvivorFourthPerkCommand { get => _selectedThirdSurvivorFourthPerkCommand ??= new(obj => { SelectedThirdSurvivorFourthPerk = SelectedSurvivorPerk; }); }

        #endregion

        #region Билд - Предметы Выжившего №3 - Свойства

        /// <summary>
        /// Свойство для выбора предмета у третьего выжившего
        /// </summary>
        private Item _selectedThirdSurvivorItem;
        public Item SelectedThirdSurvivorItem
        {
            get => _selectedThirdSurvivorItem;
            set
            {
                _selectedThirdSurvivorItem = value;
                SearchThirdSurvivorItemAddon = string.Empty;
                LoadThirdSurvivorItemAddons();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о первом улучшение предмета
        /// </summary>
        private ItemAddon _selectedThirdSurvivorFirstItemAddon;
        public ItemAddon SelectedThirdSurvivorFirstItemAddon
        {
            get => _selectedThirdSurvivorFirstItemAddon;
            set
            {
                if (value != null)
                {
                    _selectedThirdSurvivorFirstItemAddon = value;
                    SelectedThirdSurvivorFirstItemAddonName = value.ItemAddonName;
                }
                else { SelectedThirdSurvivorFirstItemAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором улучшение предмета
        /// </summary>
        private ItemAddon _selectedThirdSurvivorSecondItemAddon;
        public ItemAddon SelectedThirdSurvivorSecondItemAddon
        {
            get => _selectedThirdSurvivorSecondItemAddon;
            set
            {
                if (value != null)
                {
                    _selectedThirdSurvivorSecondItemAddon = value;
                    SelectedThirdSurvivorSecondItemAddonName = value.ItemAddonName;
                }
                else { SelectedThirdSurvivorSecondItemAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение информации о первом улучшение предмета у второго выжившего
        /// </summary>
        private string _selectedThirdSurvivorFirstItemAddonName;
        public string SelectedThirdSurvivorFirstItemAddonName
        {
            get => _selectedThirdSurvivorFirstItemAddonName;
            set
            {
                _selectedThirdSurvivorFirstItemAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение информации о втором улучшение предмета у второго выжившего
        /// </summary>
        private string _selectedThirdSurvivorSecondItemAddonName;
        public string SelectedThirdSurvivorSecondItemAddonName
        {
            get => _selectedThirdSurvivorSecondItemAddonName;
            set
            {
                _selectedThirdSurvivorSecondItemAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений для предмета у третьего выжившего
        /// </summary
        private string _searchThirdSurvivorItemAddon;
        public string SearchThirdSurvivorItemAddon
        {
            get => _searchThirdSurvivorItemAddon;
            set
            {
                _searchThirdSurvivorItemAddon = value;
                ThirdSurvivorItemAddons.Clear();
                SearchThirdSurvivorItemAddons();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Предметы Выжившего №3 - Команды

        /// <summary>
        /// Команда выбора улучшение предмета в 1 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedThirdSurvivorFirstItemAddonCommand { get => _selectedThirdSurvivorFirstItemAddonCommand ??= new(obj => { SelectedThirdSurvivorFirstItemAddon = SelectedItemAddon; }); }

        /// <summary>
        /// Команда выбора улучшение предмета в 2 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedThirdSurvivorSecondItemAddonCommand { get => _selectedThirdSurvivorSecondItemAddonCommand ??= new(obj => { SelectedThirdSurvivorSecondItemAddon = SelectedItemAddon; }); }

        #endregion

        #region Билд - Подношение Выжившего №3 - Свойства

        /// <summary>
        /// Свойство для хранение информации о выбранном подношение у первого выжившего
        /// </summary>
        private Offering _selectedThirdSurvivorOffering;
        public Offering SelectedThirdSurvivorOffering
        {
            get => _selectedThirdSurvivorOffering;
            set
            {
                if (value != null)
                {
                    _selectedThirdSurvivorOffering = value;
                    SelectedThirdSurvivorOfferingName = value.OfferingName;
                }
                else { SelectedThirdSurvivorOfferingName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойства для отображение название подношения в TextBox
        /// </summary>
        private string _selectedThirdSurvivorOfferingName;
        public string SelectedThirdSurvivorOfferingName
        {
            get => _selectedThirdSurvivorOfferingName;
            set
            {
                _selectedThirdSurvivorOfferingName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о выбранной роли. Нужно для того, что б загрузить нужные список подношений
        /// </summary>
        private Role _selectedRoleThirdSurvivor;
        public Role SelectedRoleThirdSurvivor
        {
            get => _selectedRoleThirdSurvivor;
            set
            {
                _selectedRoleThirdSurvivor = value;
                LoadThirdSurvivorOfferings();
                OnPropertyChanged();

            }
        }

        /// <summary>
        /// Свойства для поиска по списку подношений (ListView)
        /// </summary>
        private string _searchThirdSurvivorOffering;
        public string SearchThirdSurvivorOffering
        {
            get => _searchThirdSurvivorOffering;
            set
            {
                _searchThirdSurvivorOffering = value;
                SearchThirdSurvivorOfferings();
                OnPropertyChanged();
            }
        }

        #endregion 

        #region Ассоциация - Выжевшего №3 - Свойства

        // <summary>
        /// Выбор с кем ассоциируется первый выживший. "Я", "Напарник", "Рандом"
        /// </summary>
        private PlayerAssociation _selectedThirdSurvivorPlayerAssociation;
        public PlayerAssociation SelectedThirdSurvivorPlayerAssociation
        {
            get => _selectedThirdSurvivorPlayerAssociation;
            set
            {
                _selectedThirdSurvivorPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        #endregion 


        #region Основная информация Выжившего №4

        /// <summary>
        /// Отслеживание четвертого выбранного выжившего
        /// </summary>
        private Survivor _selectedFourthSurvivor;
        public Survivor SelectedFourthSurvivor
        {
            get => _selectedFourthSurvivor;
            set
            {
                _selectedFourthSurvivor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Отслеживание платформы на которой играет четвертый выживший 
        /// </summary>
        private Platform _selectedFourthSurvivorPlatform;
        public Platform SelectedFourthSurvivorPlatform
        {
            get => _selectedFourthSurvivorPlatform;
            set
            {
                _selectedFourthSurvivorPlatform = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Престиж четвертый выжившего
        /// </summary>
        private int _fourthSurvivorPrestige;
        public int FourthSurvivorPrestige
        {
            get => _fourthSurvivorPrestige;
            set
            {
                if (!CheckPrestige(value)) MessageHelper.PrestigeMessage();
                else
                {
                    _fourthSurvivorPrestige = value;
                    OnPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Как был убит четвертый выживший 
        /// </summary>
        private TypeDeath _selectedFourthSurvivorTypeDeath;
        public TypeDeath SelectedFourthSurvivorTypeDeath
        {
            get => _selectedFourthSurvivorTypeDeath;
            set
            {
                _selectedFourthSurvivorTypeDeath = value;
                CheckKills();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Счет очков у четвертого выжившего по окончанию игры
        /// </summary>
        private int _fourthSurvivorAccount;
        public int FourthSurvivorAccount
        {
            get => _fourthSurvivorAccount;
            set
            {
                _fourthSurvivorAccount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Какой режим приватности был у четвертого выжившего? Анонимный или нет
        /// </summary>
        private bool _fourthSurvivorAnonymousMode;
        public bool FourthSurvivorAnonymousMode
        {
            get => _fourthSurvivorAnonymousMode;
            set
            {
                _fourthSurvivorAnonymousMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вышел ли выживший до окончания игры или нет
        /// </summary>
        private bool _fourthSurvivorBot;
        public bool FourthSurvivorBot
        {
            get => _fourthSurvivorBot;
            set
            {
                _fourthSurvivorBot = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Перки Выжившего №4 - Свойства

        /// <summary>
        /// Свойство для хранение информации о первом выбранном перке 
        /// </summary>
        private SurvivorPerk _selectedFourthSurvivorFirstPerk;
        public SurvivorPerk SelectedFourthSurvivorFirstPerk
        {
            get => _selectedFourthSurvivorFirstPerk;
            set
            {
                if (value != null)
                {
                    _selectedFourthSurvivorFirstPerk = value;
                    SelectedFourthSurvivorFirstPerkName = value.PerkName;
                }
                else { SelectedFourthSurvivorFirstPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором выбранном перке
        /// </summary>
        private SurvivorPerk _selectedFourthSurvivorSecondPerk;
        public SurvivorPerk SelectedFourthSurvivorSecondPerk
        {
            get => _selectedFourthSurvivorSecondPerk;
            set
            {
                if (value != null)
                {
                    _selectedFourthSurvivorSecondPerk = value;
                    SelectedFourthSurvivorSecondPerkName = value.PerkName;
                }
                else { SelectedFourthSurvivorSecondPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о третьем выбранном перке
        /// </summary>
        private SurvivorPerk _selectedFourthSurvivorThirdPerk;
        public SurvivorPerk SelectedFourthSurvivorThirdPerk
        {
            get => _selectedFourthSurvivorThirdPerk;
            set
            {
                if (value != null)
                {
                    _selectedFourthSurvivorThirdPerk = value;
                    SelectedFourthSurvivorThirdPerkName = value.PerkName;
                }
                else { SelectedFourthSurvivorThirdPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о четвертом выбранном перке
        /// </summary>
        private SurvivorPerk _selectedFourthSurvivorFourthPerk;
        public SurvivorPerk SelectedFourthSurvivorFourthPerk
        {
            get => _selectedFourthSurvivorFourthPerk;
            set
            {
                if (value != null)
                {
                    _selectedFourthSurvivorFourthPerk = value;
                    SelectedFourthSurvivorFourthPerkName = value.PerkName;
                }
                else { SelectedFourthSurvivorFourthPerkName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия первого выбранного перка
        /// </summary> 
        private string _selectedFourthSurvivorFirstPerkName;
        public string SelectedFourthSurvivorFirstPerkName
        {
            get => _selectedFourthSurvivorFirstPerkName;
            set
            {
                _selectedFourthSurvivorFirstPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия второго выбранного перка
        /// </summary> 
        private string _selectedFourthSurvivorSecondPerkName;
        public string SelectedFourthSurvivorSecondPerkName
        {
            get => _selectedFourthSurvivorSecondPerkName;
            set
            {
                _selectedFourthSurvivorSecondPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия третьего выбранного перка
        /// </summary> 
        private string _selectedFourthSurvivorThirdPerkName;
        public string SelectedFourthSurvivorThirdPerkName
        {
            get => _selectedFourthSurvivorThirdPerkName;
            set
            {
                _selectedFourthSurvivorThirdPerkName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение названия четвертого выбранного перка 
        /// </summary> 
        private string _selectedFourthSurvivorFourthPerkName;
        public string SelectedFourthSurvivorFourthPerkName
        {
            get => _selectedFourthSurvivorFourthPerkName;
            set
            {
                _selectedFourthSurvivorFourthPerkName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Перки Выжившего №4 - Команды

        /// <summary>
        /// Команда выбора аддона в 1 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorFirstPerkCommand;
        public RelayCommand SelectedFourthSurvivorFirstPerkCommand { get => _selectedFourthSurvivorFirstPerkCommand ??= new(obj => { SelectedFourthSurvivorFirstPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorSecondPerkCommand;
        public RelayCommand SelectedFourthSurvivorSecondPerkCommand { get => _selectedFourthSurvivorSecondPerkCommand ??= new(obj => { SelectedFourthSurvivorSecondPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 3 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorThirdPerkCommand;
        public RelayCommand SelectedFourthSurvivorThirdPerkCommand { get => _selectedFourthSurvivorThirdPerkCommand ??= new(obj => { SelectedFourthSurvivorThirdPerk = SelectedSurvivorPerk; }); }

        /// <summary>
        /// Команда выбора аддона в 4 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorFourthPerkCommand;
        public RelayCommand SelectedFourthSurvivorFourthPerkCommand { get => _selectedFourthSurvivorFourthPerkCommand ??= new(obj => { SelectedFourthSurvivorFourthPerk = SelectedSurvivorPerk; }); }

        #endregion

        #region Билд - Предметы Выжившего №4 - Свойства

        /// <summary>
        /// Свойство для выбора предмета у третьего выжившего
        /// </summary>
        private Item _selectedFourthSurvivorItem;
        public Item SelectedFourthSurvivorItem
        {
            get => _selectedFourthSurvivorItem;
            set
            {
                _selectedFourthSurvivorItem = value;
                SearchFourthSurvivorItemAddon = string.Empty;
                LoadFourthSurvivorItemAddons();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о первом улучшение предмета
        /// </summary>
        private ItemAddon _selectedFourthSurvivorFirstItemAddon;
        public ItemAddon SelectedFourthSurvivorFirstItemAddon
        {
            get => _selectedFourthSurvivorFirstItemAddon;
            set
            {
                if (value != null)
                {
                    _selectedFourthSurvivorFirstItemAddon = value;
                    SelectedFourthSurvivorFirstItemAddonName = value.ItemAddonName;
                }
                else { SelectedFourthSurvivorFirstItemAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о втором улучшение предмета
        /// </summary>
        private ItemAddon _selectedFourthSurvivorSecondItemAddon;
        public ItemAddon SelectedFourthSurvivorSecondItemAddon
        {
            get => _selectedFourthSurvivorSecondItemAddon;
            set
            {
                if (value != null)
                {
                    _selectedFourthSurvivorSecondItemAddon = value;
                    SelectedFourthSurvivorSecondItemAddonName = value.ItemAddonName;
                }
                else { SelectedFourthSurvivorSecondItemAddonName = string.Empty; }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение информации о первом улучшение предмета у второго выжившего
        /// </summary>
        private string _selectedFourthSurvivorFirstItemAddonName;
        public string SelectedFourthSurvivorFirstItemAddonName
        {
            get => _selectedFourthSurvivorFirstItemAddonName;
            set
            {
                _selectedFourthSurvivorFirstItemAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для отображение информации о втором улучшение предмета у второго выжившего
        /// </summary>
        private string _selectedFourthSurvivorSecondItemAddonName;
        public string SelectedFourthSurvivorSecondItemAddonName
        {
            get => _selectedFourthSurvivorSecondItemAddonName;
            set
            {
                _selectedFourthSurvivorSecondItemAddonName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений для предмета у четвертого выжившего
        /// </summary
        private string _searchFourthSurvivorItemAddon;
        public string SearchFourthSurvivorItemAddon
        {
            get => _searchFourthSurvivorItemAddon;
            set
            {
                _searchFourthSurvivorItemAddon = value;
                FourthSurvivorItemAddons.Clear();
                SearchFourthSurvivorItemAddons();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Предметы Выжившего №4 - Команды

        /// <summary>
        /// Команда выбора улучшение предмета в 1 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedFourthSurvivorFirstItemAddonCommand { get => _selectedFourthSurvivorFirstItemAddonCommand ??= new(obj => { SelectedFourthSurvivorFirstItemAddon = SelectedItemAddon; }); }

        /// <summary>
        /// Команда выбора улучшение предмета в 2 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedFourthSurvivorSecondItemAddonCommand { get => _selectedFourthSurvivorSecondItemAddonCommand ??= new(obj => { SelectedFourthSurvivorSecondItemAddon = SelectedItemAddon; }); }

        #endregion

        #region Билд - Подношение Выжившего №4 - Свойства

        /// <summary>
        /// Свойство для хранение информации о выбранном подношение у первого выжившего
        /// </summary>
        private Offering _selectedFourthSurvivorOffering;
        public Offering SelectedFourthSurvivorOffering
        {
            get => _selectedFourthSurvivorOffering;
            set
            {
                if (value != null)
                {
                    _selectedFourthSurvivorOffering = value;
                    SelectedFourthSurvivorOfferingName = value.OfferingName;
                }
                else { SelectedFourthSurvivorOfferingName = string.Empty; }
                OnPropertyChanged();
            }

        }

        /// <summary>
        /// Свойства для отображение название подношения в TextBox
        /// </summary>
        private string _selectedFourthSurvivorOfferingName;
        public string SelectedFourthSurvivorOfferingName
        {
            get => _selectedFourthSurvivorOfferingName;
            set
            {
                _selectedFourthSurvivorOfferingName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о выбранной роли. Нужно для того, что б загрузить нужные список подношений
        /// </summary>
        private Role _selectedRoleFourthSurvivor;
        public Role SelectedRoleFourthSurvivor
        {
            get => _selectedRoleFourthSurvivor;
            set
            {
                _selectedRoleFourthSurvivor = value;
                LoadFourthSurvivorOfferings();
                OnPropertyChanged();

            }
        }

        /// <summary>
        /// Свойства для поиска по списку подношений (ListView)
        /// </summary>
        private string _searchFourthSurvivorOffering;
        public string SearchFourthSurvivorOffering
        {
            get => _searchFourthSurvivorOffering;
            set
            {
                _searchFourthSurvivorOffering = value;
                SearchFourthSurvivorOfferings();
                OnPropertyChanged();
            }
        }

        #endregion 

        #region Ассоциация - Выжевшего №4 - Свойства

        // <summary>
        /// Выбор с кем ассоциируется первый выживший. "Я", "Напарник", "Рандом"
        /// </summary>
        private PlayerAssociation _selectedFourthSurvivorPlayerAssociation;
        public PlayerAssociation SelectedFourthSurvivorPlayerAssociation
        {
            get => _selectedFourthSurvivorPlayerAssociation;
            set
            {
                _selectedFourthSurvivorPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Popup

        #endregion

        #endregion

        #region Блок "Игра"

        #region Основная информация

        /// <summary>
        /// Время проведение матча. Получается автоматически из данных скриншота.
        /// </summary>
        private DateTime _selectedDateTimeGameMatch;
        public DateTime SelectedDateTimeGameMatch
        {
            get => _selectedDateTimeGameMatch;
            set
            {
                _selectedDateTimeGameMatch = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Длительность матча. Получается автоматически из разности данных с скриншота "Начала матча" и "Конца матча".
        /// </summary>
        private string _durationMatch;
        public string DurationMatch
        {
            get => _durationMatch;
            set
            {
                _durationMatch = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Количество киллов за матч у Убийцы
        /// </summary>
        private int _сountKills;
        public int CountKills
        {
            get => _сountKills;
            set
            {
                if (value > 4 | value < 0)
                {
                    _dialogService.ShowMessage("Нельзя сделать больше 4 и меньше 0 убийств", InvalidNumberMessageDescription, TypeMessage.Warning);
                }
                else
                {
                    _сountKills = value;
                    OnPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Количество повесов за матч у Убийцы
        /// </summary>
        private int _countHooks;
        public int CountHooks
        {
            get => _countHooks;
            set
            {
                if (value > 12 | value < 0)
                {
                    _dialogService.ShowMessage("Нельзя сделать больше 12 и меньше 0 подвесов", InvalidNumberMessageDescription, TypeMessage.Warning);
                }
                else
                {
                    _countHooks = value;
                    OnPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Карта на которой был матч
        /// </summary>
        private Map _selectedMap;
        public Map SelectedMap
        {
            get => _selectedMap;
            set
            {
                _selectedMap = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Количество генераторов, которое осталось на момент окончания игры
        /// </summary>
        private int _countNumberRecentGenerators;
        public int CountNumberRecentGenerators
        {
            get => _countNumberRecentGenerators;
            set
            {
                if (value > 5 | value < 0)
                {
                    _dialogService.ShowMessage("В игре не может быть больше 5 и меньше 0 генераторов", InvalidNumberMessageDescription, TypeMessage.Warning);
                }
                else
                {
                    _countNumberRecentGenerators = value;
                    OnPropertyChanged();
                }

            }
        }

        #endregion

        #region Дополнительная информация

        private Patch _selectedPatch;
        public Patch SelectedPatch
        {
            get => _selectedPatch;
            set
            {
                _selectedPatch = value;
                OnPropertyChanged();
            }
        }

        private GameMode _selectedGameMode;
        public GameMode SelectedGameMode
        {
            get => _selectedGameMode;
            set
            {
                _selectedGameMode = value;
                OnPropertyChanged();
            }
        }

        private GameEvent _selectedGameEvent;
        public GameEvent SelectedGameEvent
        {
            get => _selectedGameEvent;
            set
            {
                _selectedGameEvent = value;
                OnPropertyChanged();
            }
        }

        private WhoPlacedMap _selectedWhoPlacedMap;
        public WhoPlacedMap SelectedWhoPlacedMap
        {
            get => _selectedWhoPlacedMap;
            set
            {
                _selectedWhoPlacedMap = value;
                OnPropertyChanged();
            }
        }

        private WhoPlacedMap _selectedWhoPlacedMapWin;
        public WhoPlacedMap SelectedWhoPlacedMapWin
        {
            get => _selectedWhoPlacedMapWin;
            set
            {
                _selectedWhoPlacedMapWin = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Описание игры - Свойства

        private string _descriptionGame;
        public string DescriptionGame
        {
            get => _descriptionGame;
            set
            {
                _descriptionGame = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Добавление записи в БД

        private RelayCommand _addMatchCommand;
        public RelayCommand AddMatchCommand { get => _addMatchCommand ??= new(obj => { AddGameStatistic(); }); }

        private void AddGameStatistic()
        {
            if (SelectedKiller != null &&
                SelectedKillerPlatform != null &&
                SelectedKillerFirstPerk != null && SelectedKillerSecondPerk != null && SelectedKillerThirdPerk != null && SelectedKillerFourthPerk != null &&
                SelectedKillerFirstAddon != null && SelectedKillerSecondPerk != null &&
                SelectedKillerOffering != null &&
                SelectedKillerPlayerAssociation != null) { }
            else
            {
                _dialogService.ShowMessage("Вы заполнили не все данные киллера. Если у Киллера нету аддонов либо перков, то нужно выбрать пункт - Отсутствует.", "Ошибка заполнения", TypeMessage.Warning);
                return;
            }

            if (SelectedFirstSurvivor != null &&
                SelectedFirstSurvivorPlatform != null &&
                SelectedFirstSurvivorTypeDeath != null &&
                SelectedFirstSurvivorFirstPerk != null && SelectedFirstSurvivorSecondPerk != null && SelectedFirstSurvivorThirdPerk != null && SelectedFirstSurvivorFourthPerk != null &&
                SelectedFirstSurvivorItem != null && SelectedFirstSurvivorFirstItemAddon != null && SelectedFirstSurvivorSecondItemAddon != null &&
                SelectedFirstSurvivorOffering != null &&
                SelectedFirstSurvivorPlayerAssociation != null) { }
            else
            {
                _dialogService.ShowMessage("Вы заполнили не все данные первого выжившего. Если у Выжившего нету какого либо снаряжение, то нужно выбрать пункт - Отсутствует.", "Ошибка заполнения", TypeMessage.Warning);
                return;
            }

            if (SelectedSecondSurvivor != null &&
                SelectedSecondSurvivorPlatform != null &&
                SelectedSecondSurvivorTypeDeath != null &&
                SelectedSecondSurvivorFirstPerk != null && SelectedSecondSurvivorSecondPerk != null && SelectedSecondSurvivorThirdPerk != null && SelectedSecondSurvivorFourthPerk != null &&
                SelectedSecondSurvivorItem != null && SelectedSecondSurvivorFirstItemAddon != null && SelectedSecondSurvivorSecondItemAddon != null &&
                SelectedSecondSurvivorOffering != null &&
                SelectedSecondSurvivorPlayerAssociation != null) { }
            else
            {
                _dialogService.ShowMessage("Вы заполнили не все данные второго выжившего. Если у Выжившего нету какого либо снаряжение, то нужно выбрать пункт - Отсутствует.", "Ошибка заполнения", TypeMessage.Warning);
                return;
            }

            if (SelectedThirdSurvivor != null &&
                SelectedThirdSurvivorPlatform != null &&
                SelectedThirdSurvivorTypeDeath != null &&
                SelectedThirdSurvivorFirstPerk != null && SelectedThirdSurvivorSecondPerk != null && SelectedThirdSurvivorThirdPerk != null && SelectedThirdSurvivorFourthPerk != null &&
                SelectedThirdSurvivorItem != null && SelectedThirdSurvivorFirstItemAddon != null && SelectedThirdSurvivorSecondItemAddon != null &&
                SelectedThirdSurvivorOffering != null &&
                SelectedThirdSurvivorPlayerAssociation != null) { }
            else
            {
                _dialogService.ShowMessage("Вы заполнили не все данные третьего выжившего. Если у Выжившего нету какого либо снаряжение, то нужно выбрать пункт - Отсутствует.", "Ошибка заполнения", TypeMessage.Warning);
                return;
            }

            if (SelectedFourthSurvivor != null &&
                SelectedFourthSurvivorPlatform != null &&
                SelectedFourthSurvivorTypeDeath != null &&
                SelectedFourthSurvivorFirstPerk != null && SelectedFourthSurvivorSecondPerk != null && SelectedFourthSurvivorThirdPerk != null && SelectedFourthSurvivorFourthPerk != null &&
                SelectedFourthSurvivorItem != null && SelectedFourthSurvivorFirstItemAddon != null && SelectedFourthSurvivorSecondItemAddon != null &&
                SelectedFourthSurvivorOffering != null &&
                SelectedFourthSurvivorPlayerAssociation != null) { }
            else
            {
                _dialogService.ShowMessage("Вы заполнили не все данные четвертого выжившего. Если у Выжившего нету какого либо снаряжение, то нужно выбрать пункт - Отсутствует.", "Ошибка заполнения", TypeMessage.Warning);
                return;
            }

            if (SelectedMap != null &&
               SelectedPatch != null &&
               SelectedGameMode != null &&
               SelectedGameEvent != null &&
               SelectedWhoPlacedMap != null &&
               SelectedWhoPlacedMapWin != null) { }
            else
            {
                _dialogService.ShowMessage("Вы заполнили не все данные в категории - Игра.", "Ошибка заполнения", TypeMessage.Warning);
                return;
            }

            if (FirstSurvivorAccount != 0 &&
               SecondSurvivorAccount != 0 &&
               ThirdSurvivorAccount != 0 &&
               FourthSurvivorAccount != 0) { }
            else
            {
                if (_dialogService.ShowMessageButtons("У какого то выжившего счет 0! Вы уверены?", "Ошибка заполнения", TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.No)
                {
                    return;
                }
            }

            if (FirstSurvivorPrestige != 0 &&
               SecondSurvivorPrestige != 0 &&
               ThirdSurvivorPrestige != 0 &&
               FourthSurvivorPrestige != 0) { }
            else
            {
                if (_dialogService.ShowMessageButtons("У какого то выжившего престиж 0! Вы уверены?", "Ошибка заполнения", TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.No)
                {
                    return;
                }
            }

            if (KillerAccount != 0) { }
            else
            {
                if (_dialogService.ShowMessageButtons("У киллера счет 0! Вы уверены?", "Ошибка заполнения", TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.No)
                {
                    return;
                }
            }

            if (KillerPrestige != 0) { }
            else
            {
                if (_dialogService.ShowMessageButtons("У киллера престиж 0! Вы уверены?", "Ошибка заполнения", TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.No)
                {
                    return;
                }
            }

            AddKillerInfo();
            AddFirstSurvivorInfo();
            AddSecondSurvivorInfo();
            AddThirdSurvivorInfo();
            AddFourthSurvivorInfo();

            var lastID = _dataService.GetAllDataInList<KillerInfo>(x => x
                .OrderByDescending(x => x.IdKillerInfo));

            var lastIDKiller = lastID.FirstOrDefault();

            SurvivorInfo firstSurvivor;
            SurvivorInfo secondSurvivor;
            SurvivorInfo thirdSurvivor;
            SurvivorInfo fourthSurvivor;

            var lastFourRecords = _dataService.GetAllDataInList<SurvivorInfo>(x => x
                .OrderByDescending(x => x.IdSurvivorInfo)
                    .Take(4)
                        .Reverse());

            firstSurvivor = lastFourRecords[0];
            secondSurvivor = lastFourRecords[1];
            thirdSurvivor = lastFourRecords[2];
            fourthSurvivor = lastFourRecords[3];

            var newGameStatistic = new GameStatistic()
            {
                IdKiller = lastIDKiller.IdKillerInfo,
                IdMap = SelectedMap.IdMap,
                IdWhoPlacedMap = SelectedWhoPlacedMap.IdWhoPlacedMap,
                IdWhoPlacedMapWin = SelectedWhoPlacedMapWin.IdWhoPlacedMap,
                IdPatch = SelectedPatch.IdPatch,
                IdGameMode = SelectedGameMode.IdGameMode,
                IdGameEvent = SelectedGameEvent.IdGameEvent,
                IdSurvivors1 = firstSurvivor.IdSurvivorInfo,
                IdSurvivors2 = secondSurvivor.IdSurvivorInfo,
                IdSurvivors3 = thirdSurvivor.IdSurvivorInfo,
                IdSurvivors4 = fourthSurvivor.IdSurvivorInfo,
                DateTimeMatch = SelectedImage.FileCreatedTime,
                GameTimeMatch = DurationMatch,
                CountKills = CountKills,
                CountHooks = CountHooks,
                NumberRecentGenerators = CountNumberRecentGenerators,
                DescriptionGame = DescriptionGame,
            };

            _dataService.Add(newGameStatistic);

            SetNullKillerData();
            SetNullSurvivorData();
            SetNullGameData();
            SetNullImage();
            SetNullBuild();
            _dialogService.ShowMessage("Данные успешно добавлены", "Успешно!", TypeMessage.Notification);
        }

        private void AddKillerInfo()
        {
            var newKillerInfo = new KillerInfo()
            {
                IdKiller = SelectedKiller.IdKiller,
                IdPerk1 = SelectedKillerFirstPerk.IdKillerPerk,
                IdPerk2 = SelectedKillerSecondPerk.IdKillerPerk,
                IdPerk3 = SelectedKillerThirdPerk.IdKillerPerk,
                IdPerk4 = SelectedKillerFourthPerk.IdKillerPerk,
                IdAddon1 = SelectedKillerFirstAddon.IdKillerAddon,
                IdAddon2 = SelectedKillerSecondAddon.IdKillerAddon,
                IdAssociation = SelectedKillerPlayerAssociation.IdPlayerAssociation,
                IdPlatform = SelectedKillerPlatform.IdPlatform,
                IdKillerOffering = SelectedKillerOffering.IdOffering,
                Prestige = KillerPrestige,
                Bot = KillerBot,
                AnonymousMode = KillerAnonymousMode,
                KillerAccount = KillerAccount,
            };
            _dataService.Add(newKillerInfo);
        }

        private void AddFirstSurvivorInfo()
        {
            var newSurvivorInfo = new SurvivorInfo()
            {
                IdSurvivor = SelectedFirstSurvivor.IdSurvivor,
                IdPerk1 = SelectedFirstSurvivorFirstPerk.IdSurvivorPerk,
                IdPerk2 = SelectedFirstSurvivorSecondPerk.IdSurvivorPerk,
                IdPerk3 = SelectedFirstSurvivorThirdPerk.IdSurvivorPerk,
                IdPerk4 = SelectedFirstSurvivorFourthPerk.IdSurvivorPerk,
                IdItem = SelectedFirstSurvivorItem.IdItem,
                IdAddon1 = SelectedFirstSurvivorFirstItemAddon.IdItemAddon,
                IdAddon2 = SelectedFirstSurvivorSecondItemAddon.IdItemAddon,
                IdTypeDeath = SelectedFirstSurvivorTypeDeath.IdTypeDeath,
                IdAssociation = SelectedFirstSurvivorPlayerAssociation.IdPlayerAssociation,
                IdPlatform = SelectedFirstSurvivorPlatform.IdPlatform,
                IdSurvivorOffering = SelectedFirstSurvivorOffering.IdOffering,
                Prestige = FirstSurvivorPrestige,
                Bot = FirstSurvivorBot,
                AnonymousMode = FirstSurvivorAnonymousMode,
                SurvivorAccount = FirstSurvivorAccount,
            };
            _dataService.Add(newSurvivorInfo);
        }

        private void AddSecondSurvivorInfo()
        {
            var newSurvivorInfo = new SurvivorInfo()
            {
                IdSurvivor = SelectedSecondSurvivor.IdSurvivor,
                IdPerk1 = SelectedSecondSurvivorFirstPerk.IdSurvivorPerk,
                IdPerk2 = SelectedSecondSurvivorSecondPerk.IdSurvivorPerk,
                IdPerk3 = SelectedSecondSurvivorThirdPerk.IdSurvivorPerk,
                IdPerk4 = SelectedSecondSurvivorFourthPerk.IdSurvivorPerk,
                IdItem = SelectedSecondSurvivorItem.IdItem,
                IdAddon1 = SelectedSecondSurvivorFirstItemAddon.IdItemAddon,
                IdAddon2 = SelectedSecondSurvivorSecondItemAddon.IdItemAddon,
                IdTypeDeath = SelectedSecondSurvivorTypeDeath.IdTypeDeath,
                IdAssociation = SelectedSecondSurvivorPlayerAssociation.IdPlayerAssociation,
                IdPlatform = SelectedSecondSurvivorPlatform.IdPlatform,
                IdSurvivorOffering = SelectedSecondSurvivorOffering.IdOffering,
                Prestige = SecondSurvivorPrestige,
                Bot = SecondSurvivorBot,
                AnonymousMode = SecondSurvivorAnonymousMode,
                SurvivorAccount = SecondSurvivorAccount,
            };
            _dataService.Add(newSurvivorInfo);
        }

        private void AddThirdSurvivorInfo()
        {
            var newSurvivorInfo = new SurvivorInfo()
            {
                IdSurvivor = SelectedThirdSurvivor.IdSurvivor,
                IdPerk1 = SelectedThirdSurvivorFirstPerk.IdSurvivorPerk,
                IdPerk2 = SelectedThirdSurvivorSecondPerk.IdSurvivorPerk,
                IdPerk3 = SelectedThirdSurvivorThirdPerk.IdSurvivorPerk,
                IdPerk4 = SelectedThirdSurvivorFourthPerk.IdSurvivorPerk,
                IdItem = SelectedThirdSurvivorItem.IdItem,
                IdAddon1 = SelectedThirdSurvivorFirstItemAddon.IdItemAddon,
                IdAddon2 = SelectedThirdSurvivorSecondItemAddon.IdItemAddon,
                IdTypeDeath = SelectedThirdSurvivorTypeDeath.IdTypeDeath,
                IdAssociation = SelectedThirdSurvivorPlayerAssociation.IdPlayerAssociation,
                IdPlatform = SelectedThirdSurvivorPlatform.IdPlatform,
                IdSurvivorOffering = SelectedThirdSurvivorOffering.IdOffering,
                Prestige = ThirdSurvivorPrestige,
                Bot = ThirdSurvivorBot,
                AnonymousMode = ThirdSurvivorAnonymousMode,
                SurvivorAccount = ThirdSurvivorAccount,
            };
            _dataService.Add(newSurvivorInfo);
        }

        private void AddFourthSurvivorInfo()
        {
            var newSurvivorInfo = new SurvivorInfo()
            {
                IdSurvivor = SelectedFourthSurvivor.IdSurvivor,
                IdPerk1 = SelectedFourthSurvivorFirstPerk.IdSurvivorPerk,
                IdPerk2 = SelectedFourthSurvivorSecondPerk.IdSurvivorPerk,
                IdPerk3 = SelectedFourthSurvivorThirdPerk.IdSurvivorPerk,
                IdPerk4 = SelectedFourthSurvivorFourthPerk.IdSurvivorPerk,
                IdItem = SelectedFourthSurvivorItem.IdItem,
                IdAddon1 = SelectedFourthSurvivorFirstItemAddon.IdItemAddon,
                IdAddon2 = SelectedFourthSurvivorSecondItemAddon.IdItemAddon,
                IdTypeDeath = SelectedFourthSurvivorTypeDeath.IdTypeDeath,
                IdAssociation = SelectedFourthSurvivorPlayerAssociation.IdPlayerAssociation,
                IdPlatform = SelectedFourthSurvivorPlatform.IdPlatform,
                IdSurvivorOffering = SelectedFourthSurvivorOffering.IdOffering,
                Prestige = FourthSurvivorPrestige,
                Bot = FourthSurvivorBot,
                AnonymousMode = FourthSurvivorAnonymousMode,
                SurvivorAccount = FourthSurvivorAccount,
            };
            _dataService.Add(newSurvivorInfo);
        }

        #endregion

        #region Методы для поиска по спискам

        private void SearchKillerPerks()
        {
            var search = _killerPerks.Where(x => x.PerkName.ToLower().Contains(SearchKillerPerk.ToLower()) ||
                            x.IdKillerNavigation.KillerName.ToLower().Contains(SearchKillerPerk.ToLower()));

            KillerPerks.Clear();

            foreach (var item in search)
            {
                KillerPerks.Add(item);
            }
        }

        private void SearchKillerAddons()
        {
            if (SelectedKiller == null)
            {
                _dialogService.ShowMessage("Вы не выбрали убийцу");
                return;
            }
            var search = _killerAddons.Where(ka => ka.IdKiller == SelectedKiller.IdKiller)
                    .Where(ka => ka.AddonName.ToLower().Contains(SearchKillerAddon.ToLower()));

            KillerAddons.Clear();

            foreach (var item in search)
            {
                KillerAddons.Add(item);
            }
        }

        private void SearchKillerOfferings()
        {
            var search = _offering.Where(off => off.IdRole == SelectedRoleKiller.IdRole)
                   .Where(off => off.OfferingName.ToLower().Contains(SearchKillerOffering.ToLower()));

            KillerOfferings.Clear();

            foreach (var item in search)
            {
                KillerOfferings.Add(item);
            }
        }

        private void SearchFirstSurvivorOfferings()
        {
            var search = _offering.Where(off => off.IdRole == SelectedRoleFirstSurvivor.IdRole)
                   .Where(off => off.OfferingName.ToLower().Contains(SearchFirstSurvivorOffering.ToLower()));

            FirstSurvivorOfferings.Clear();

            foreach (var item in search)
            {
                FirstSurvivorOfferings.Add(item);
            }
        }

        private void SearchSecondSurvivorOfferings()
        {
            var search = _offering.Where(off => off.IdRole == SelectedRoleSecondSurvivor.IdRole)
                    .Where(off => off.OfferingName.ToLower().Contains(SearchSecondSurvivorOffering.ToLower()));

            SecondSurvivorOfferings.Clear();

            foreach (var item in search)
            {
                SecondSurvivorOfferings.Add(item);
            }
        }

        private void SearchThirdSurvivorOfferings()
        {
            var search = _offering.Where(off => off.IdRole == SelectedRoleThirdSurvivor.IdRole)
                    .Where(off => off.OfferingName.ToLower().Contains(SearchThirdSurvivorOffering.ToLower()));

            ThirdSurvivorOfferings.Clear();

            foreach (var item in search)
            {
                ThirdSurvivorOfferings.Add(item);
            }
        }

        private void SearchFourthSurvivorOfferings()
        {
            var search = _offering.Where(off => off.IdRole == SelectedRoleFourthSurvivor.IdRole)
                     .Where(off => off.OfferingName.ToLower().Contains(SearchFourthSurvivorOffering.ToLower()));

            FourthSurvivorOfferings.Clear();

            foreach (var item in search)
            {
                FourthSurvivorOfferings.Add(item);
            }
        }

        private void SearchSurvivorPerks()
        {
            var search = _survivorPerks.Where(x => x.PerkName.ToLower().Contains(SearchSurvivorPerk.ToLower()) ||
                         x.IdSurvivorNavigation.SurvivorName.ToLower().Contains(SearchSurvivorPerk.ToLower()));

            SurvivorPerks.Clear();

            foreach (var item in search)
            {
                SurvivorPerks.Add(item);
            }
        }

        private void SearchFirstSurvivorItemAddons()
        {
            var search = _itemAddons.Where(x => x.IdItem == SelectedFirstSurvivorItem.IdItem)
               .Where(x => x.ItemAddonName.ToLower().Contains(SearchFirstSurvivorItemAddon.ToLower()));

            FirstSurvivorItemAddons.Clear();

            foreach (var item in search)
            {
                FirstSurvivorItemAddons.Add(item);
            }
        }

        private void SearchSecondSurvivorItemAddons()
        {
            var search = _itemAddons.Where(ia => ia.IdItem == SelectedSecondSurvivorItem.IdItem)
                   .Where(ka => ka.ItemAddonName.ToLower().Contains(SearchSecondSurvivorItemAddon.ToLower()));


            SecondSurvivorItemAddons.Clear();

            foreach (var item in search)
            {
                SecondSurvivorItemAddons.Add(item);
            }
        }

        private void SearchThirdSurvivorItemAddons()
        {
            var search = _itemAddons.Where(ia => ia.IdItem == SelectedThirdSurvivorItem.IdItem)
                     .Where(ka => ka.ItemAddonName.ToLower().Contains(SearchThirdSurvivorItemAddon.ToLower()));

            ThirdSurvivorItemAddons.Clear();

            foreach (var item in search)
            {
                ThirdSurvivorItemAddons.Add(item);
            }
        }

        private void SearchFourthSurvivorItemAddons()
        {
            var search = _itemAddons.Where(ia => ia.IdItem == SelectedFourthSurvivorItem.IdItem)
                    .Where(ka => ka.ItemAddonName.ToLower().Contains(SearchFourthSurvivorItemAddon.ToLower()));

            FourthSurvivorItemAddons.Clear();

            foreach (var item in search)
            {
                FourthSurvivorItemAddons.Add(item);
            }
        }

        #endregion

        #region Методы получение данных

        private void GetAllData()
        {
            /*-Общие-*/
            GetOfferings();
            GetGameModes();
            GetGameEvents();
            GetMaps();
            GetPatches();
            GetPlatforms();
            GetPlayerAssociations();
            GetRoles();
            GetWhoPlacedMaps();

            /*-Киллеры-*/
            GetKillers();
            GetKillerAddons();
            GetKillerPerks();
            GetKillerBuilds();

            /*-Выжившие-*/
            GetSurvivors();
            GetSurvivorPerks();
            GetItems();
            GetItemAddons();
            GetTypeDeaths();

            SelectedDateTimeGameMatch = DateTime.Now;
        }

        private void LoadAllData()
        {
            /*-Общие-*/
            LoadGameModes();
            LoadGameEvents();
            LoadMaps();
            LoadPatches();
            LoadPlatforms();
            LoadPlayerAssociations();
            LoadRoles();
            LoadWhoPlacedMaps();

            /*-Киллеры-*/
            LoadKillers();
            LoadKillerAddons();
            LoadKillerPerks();
            LoadKillerOfferings();
            LoadKillerBuilds();

            /*-Выжившие-*/
            LoadSurvivors();
            LoadItems();
            LoadSurvivorPerks();
            LoadFirstSurvivorItemAddons();
            LoadSecondSurvivorItemAddons();
            LoadThirdSurvivorItemAddons();
            LoadFourthSurvivorItemAddons();
            LoadFirstSurvivorOfferings();
            LoadSecondSurvivorOfferings();
            LoadThirdSurvivorOfferings();
            LoadFourthSurvivorOfferings();
            LoadTypeDeaths();
        }

        #region Общие данные

        /*-Получение : Общие-*/
        private void GetOfferings()
        {
            _offering.AddRange(_dataService.GetAllData<Offering>());
        }

        private void GetGameModes()
        {
            _gameModes.AddRange(_dataService.GetAllData<GameMode>());
        }

        private void GetGameEvents()
        {
            _gameEvents.AddRange(_dataService.GetAllData<GameEvent>());
        }

        private void GetMaps()
        {
            _maps.AddRange(_dataService.GetAllData<Map>());
        }

        private void GetPatches()
        {
            _patches.AddRange(_dataService.GetAllData<Patch>());
        }

        private void GetPlatforms()
        {
            _platforms.AddRange(_dataService.GetAllData<Platform>());
        }

        private void GetPlayerAssociations()
        {
            _playerAssociations.AddRange(_dataService.GetAllData<PlayerAssociation>());
        }

        private void GetRoles()
        {
            _roles.AddRange(_dataService.GetAllData<Role>());
        }

        private void GetWhoPlacedMaps()
        {
            _whoPlacedMaps.AddRange(_dataService.GetAllData<WhoPlacedMap>());
        }

        /*-Вывод : Общие-*/
        private void LoadGameModes()
        {
            GameModes.Clear();

            foreach (var item in _gameModes)
            {
                GameModes.Add(item);
            }

            SelectedGameMode = GameModes.FirstOrDefault();
        }

        private void LoadGameEvents()
        {
            GameEvents.Clear();

            foreach (var item in _gameEvents)
            {
                GameEvents.Add(item);
            }

            SelectedGameEvent = GameEvents.FirstOrDefault();
        }

        private void LoadMaps()
        {
            Maps.Clear();

            foreach (var item in _maps)
            {
                Maps.Add(item);
            }
        }

        private void LoadPatches()
        {
            Patches.Clear();

            foreach (var item in _patches)
            {
                Patches.Add(item);
            }

            SelectedPatch = Patches.LastOrDefault();
        }

        private void LoadPlatforms()
        {
            Platforms.Clear();

            foreach (var item in _platforms)
            {
                Platforms.Add(item);
            }

            SelectedKillerPlatform = Platforms.FirstOrDefault();

            SelectedFirstSurvivorPlatform = Platforms.FirstOrDefault();
            SelectedSecondSurvivorPlatform = Platforms.FirstOrDefault();
            SelectedThirdSurvivorPlatform = Platforms.FirstOrDefault();
            SelectedFourthSurvivorPlatform = Platforms.FirstOrDefault();
        }

        private void LoadPlayerAssociations()
        {
            KillerAssociations.Add(_dataService.GetById<PlayerAssociation>(1, "IdPlayerAssociation"));
            KillerAssociations.Add(_dataService.GetById<PlayerAssociation>(3, "IdPlayerAssociation"));

            foreach (var item in _playerAssociations)
            {
                SurvivorAssociations.Add(item);
            }

            SelectedKillerPlayerAssociation = KillerAssociations.FirstOrDefault();

            SelectedFirstSurvivorPlayerAssociation = SurvivorAssociations[2];
            SelectedSecondSurvivorPlayerAssociation = SurvivorAssociations[2];
            SelectedThirdSurvivorPlayerAssociation = SurvivorAssociations[2];
            SelectedFourthSurvivorPlayerAssociation = SurvivorAssociations[2];
        }

        private void LoadRoles()
        {
            KillerRoles.Add(_dataService.GetById<Role>(2, "IdRole"));
            KillerRoles.Add(_dataService.GetById<Role>(5, "IdRole"));

            SurvivorRoles.Add(_dataService.GetById<Role>(3, "IdRole"));
            SurvivorRoles.Add(_dataService.GetById<Role>(5, "IdRole"));

            SelectedRoleKiller = KillerRoles.FirstOrDefault();

            SelectedRoleFirstSurvivor = SurvivorRoles.FirstOrDefault();
            SelectedRoleSecondSurvivor = SurvivorRoles.FirstOrDefault();
            SelectedRoleThirdSurvivor = SurvivorRoles.FirstOrDefault();
            SelectedRoleFourthSurvivor = SurvivorRoles.FirstOrDefault();

        }

        private void LoadWhoPlacedMaps()
        {
            WhoPlacedMaps.Clear();

            foreach (var item in _whoPlacedMaps)
            {
                WhoPlacedMaps.Add(item);
            }

            SelectedWhoPlacedMap = WhoPlacedMaps.FirstOrDefault();
            SelectedWhoPlacedMapWin = WhoPlacedMaps.FirstOrDefault();
        }

        #endregion

        #region Данные для киллера

        /*-Получение : Киллеры-*/
        private void GetKillers()
        {
            _killers.AddRange(_dataService.GetAllData<Killer>(x => x.Skip(1)));
        }

        private void GetKillerAddons()
        {
            _killerAddons.AddRange(_dataService.GetAllData<KillerAddon>());
        }

        private void GetKillerPerks()
        {
            _killerPerks.AddRange(_dataService.GetAllData<KillerPerk>(x => x.Include(x => x.IdKillerNavigation)));
        }

        private void GetKillerBuilds()
        {
            _killerBuilds.AddRange(_dataService.GetAllData<KillerBuild>());
        }

        /*-Вывод : Киллеры-*/
        private void LoadKillers()
        {
            Killers.Clear();

            foreach (var item in _killers)
            {
                Killers.Add(item);
            }

            SelectedKiller = Killers.FirstOrDefault();
        }

        private void LoadKillerAddons()
        {
            KillerAddons.Clear();

            foreach (var item in _killerAddons.Where(x => x.IdKiller == SelectedKiller.IdKiller).OrderBy(x => x.IdRarity))
            {
                KillerAddons.Add(item);
            }

            SelectedKillerFirstAddon = null;
            SelectedKillerSecondAddon = null;
        }

        private void LoadKillerPerks()
        {
            KillerPerks.Clear();

            foreach (var item in _killerPerks)
            {
                KillerPerks.Add(item);
            }
        }

        private void LoadKillerBuilds()
        {
            KillerBuilds.Clear();

            foreach (var item in _killerBuilds)
            {
                KillerBuilds.Add(item);
            }
        }

        private void LoadKillerOfferings()
        {
            KillerOfferings.Clear();

            foreach (var item in _offering.Where(x => x.IdRole == SelectedRoleKiller.IdRole).OrderBy(x => x.IdRarity))
            {
                KillerOfferings.Add(item);
            }

            SelectedKillerOffering = KillerOfferings.LastOrDefault();
        }

        #endregion

        #region Данные для выживших

        /*-Получение : Выжившие-*/
        private void GetSurvivors()
        {
            _survivors.AddRange(_dataService.GetAllData<Survivor>(x => x.Skip(1)));
        }

        private void GetSurvivorPerks()
        {
            _survivorPerks.AddRange(_dataService.GetAllData<SurvivorPerk>(x => x.Include(x => x.IdSurvivorNavigation)));
        }

        private void GetItems()
        {
            _items.AddRange(_dataService.GetAllData<Item>());
        }

        private void GetItemAddons()
        {
            _itemAddons.AddRange(_dataService.GetAllData<ItemAddon>());
        }

        private void GetTypeDeaths()
        {
            _typeDeaths.AddRange(_dataService.GetAllData<TypeDeath>());
        }

        /*-Вывод : Выжившие-*/
        private void LoadSurvivors()
        {
            Survivors.Clear();

            foreach (var item in _survivors)
            {
                Survivors.Add(item);
            }

            SelectedFirstSurvivor = Survivors.FirstOrDefault();
            SelectedSecondSurvivor = Survivors.FirstOrDefault();
            SelectedThirdSurvivor = Survivors.FirstOrDefault();
            SelectedFourthSurvivor = Survivors.FirstOrDefault();
        }

        private void LoadSurvivorPerks()
        {
            SurvivorPerks.Clear();

            foreach (var item in _survivorPerks)
            {
                SurvivorPerks.Add(item);
            }
        }

        private void LoadItems()
        {
            Items.Clear();

            foreach (var item in _items)
            {
                Items.Add(item);
            }

            SelectedFirstSurvivorItem = Items.LastOrDefault();
            SelectedSecondSurvivorItem = Items.LastOrDefault();
            SelectedThirdSurvivorItem = Items.LastOrDefault();
            SelectedFourthSurvivorItem = Items.LastOrDefault();
        }

        private void LoadTypeDeaths()
        {
            TypeDeaths.Clear();

            foreach (var item in _typeDeaths)
            {
                TypeDeaths.Add(item);
            }

            SelectedFirstSurvivorTypeDeath = TypeDeaths.FirstOrDefault();
            SelectedSecondSurvivorTypeDeath = TypeDeaths.FirstOrDefault();
            SelectedThirdSurvivorTypeDeath = TypeDeaths.FirstOrDefault();
            SelectedFourthSurvivorTypeDeath = TypeDeaths.FirstOrDefault();
        }

        private void LoadFirstSurvivorItemAddons()
        {
            FirstSurvivorItemAddons.Clear();

            foreach (var item in _itemAddons.Where(x => x.IdItem == SelectedFirstSurvivorItem.IdItem))
            {
                FirstSurvivorItemAddons.Add(item);
            }

            if (SelectedFirstSurvivorItem.ItemName == "Отсутствует")
            {
                SelectedFirstSurvivorFirstItemAddon = FirstSurvivorItemAddons.FirstOrDefault();
                SelectedFirstSurvivorSecondItemAddon = FirstSurvivorItemAddons.FirstOrDefault();
            }
            else
            {
                SelectedFirstSurvivorFirstItemAddon = FirstSurvivorItemAddons.Reverse().FirstOrDefault();
                SelectedFirstSurvivorSecondItemAddon = FirstSurvivorItemAddons.Reverse().FirstOrDefault();
            }

        }

        private void LoadSecondSurvivorItemAddons()
        {
            SecondSurvivorItemAddons.Clear();

            foreach (var item in _itemAddons.Where(x => x.IdItem == SelectedSecondSurvivorItem.IdItem))
            {
                SecondSurvivorItemAddons.Add(item);
            }

            if (SelectedSecondSurvivorItem.ItemName == "Отсутствует")
            {
                SelectedSecondSurvivorFirstItemAddon = SecondSurvivorItemAddons.FirstOrDefault();
                SelectedSecondSurvivorSecondItemAddon = SecondSurvivorItemAddons.FirstOrDefault();
            }
            else
            {
                SelectedSecondSurvivorFirstItemAddon = SecondSurvivorItemAddons.Reverse().FirstOrDefault();
                SelectedSecondSurvivorSecondItemAddon = SecondSurvivorItemAddons.Reverse().FirstOrDefault();
            }
        }

        private void LoadThirdSurvivorItemAddons()
        {
            ThirdSurvivorItemAddons.Clear();

            foreach (var item in _itemAddons.Where(x => x.IdItem == SelectedThirdSurvivorItem.IdItem))
            {
                ThirdSurvivorItemAddons.Add(item);
            }

            if (SelectedThirdSurvivorItem.ItemName == "Отсутствует")
            {
                SelectedThirdSurvivorFirstItemAddon = ThirdSurvivorItemAddons.FirstOrDefault();
                SelectedThirdSurvivorSecondItemAddon = ThirdSurvivorItemAddons.FirstOrDefault();
            }
            else
            {
                SelectedThirdSurvivorFirstItemAddon = ThirdSurvivorItemAddons.Reverse().FirstOrDefault();
                SelectedThirdSurvivorSecondItemAddon = ThirdSurvivorItemAddons.Reverse().FirstOrDefault();
            }
        }

        private void LoadFourthSurvivorItemAddons()
        {
            FourthSurvivorItemAddons.Clear();

            foreach (var item in _itemAddons.Where(x => x.IdItem == SelectedFourthSurvivorItem.IdItem))
            {
                FourthSurvivorItemAddons.Add(item);
            }

            if (SelectedFourthSurvivorItem.ItemName == "Отсутствует")
            {
                SelectedFourthSurvivorFirstItemAddon = FourthSurvivorItemAddons.FirstOrDefault();
                SelectedFourthSurvivorSecondItemAddon = FourthSurvivorItemAddons.FirstOrDefault();
            }
            else
            {
                SelectedFourthSurvivorFirstItemAddon = FourthSurvivorItemAddons.Reverse().FirstOrDefault();
                SelectedFourthSurvivorSecondItemAddon = FourthSurvivorItemAddons.Reverse().FirstOrDefault();
            }
        }

        private void LoadFirstSurvivorOfferings()
        {
            FirstSurvivorOfferings.Clear();

            foreach (var item in _offering.Where(x => x.IdRole == SelectedRoleFirstSurvivor.IdRole).OrderBy(x => x.IdRarity))
            {
                FirstSurvivorOfferings.Add(item);
            }

            SelectedFirstSurvivorOffering = FirstSurvivorOfferings.LastOrDefault();
        }

        private void LoadSecondSurvivorOfferings()
        {
            SecondSurvivorOfferings.Clear();

            foreach (var item in _offering.Where(x => x.IdRole == SelectedRoleSecondSurvivor.IdRole).OrderBy(x => x.IdRarity))
            {
                SecondSurvivorOfferings.Add(item);
            }

            SelectedSecondSurvivorOffering = SecondSurvivorOfferings.LastOrDefault();
        }

        private void LoadThirdSurvivorOfferings()
        {
            ThirdSurvivorOfferings.Clear();

            foreach (var item in _offering.Where(x => x.IdRole == SelectedRoleThirdSurvivor.IdRole).OrderBy(x => x.IdRarity))
            {
                ThirdSurvivorOfferings.Add(item);
            }

            SelectedThirdSurvivorOffering = ThirdSurvivorOfferings.LastOrDefault();
        }

        private void LoadFourthSurvivorOfferings()
        {
            FourthSurvivorOfferings.Clear();

            foreach (var item in _offering.Where(x => x.IdRole == SelectedRoleFourthSurvivor.IdRole).OrderBy(x => x.IdRarity))
            {
                FourthSurvivorOfferings.Add(item);
            }

            SelectedFourthSurvivorOffering = FourthSurvivorOfferings.LastOrDefault();
        }

        #endregion

        #region Установка дефолтных значение

        private void SetNullKillerData()
        {
            SelectedKillerPlatform = Platforms.FirstOrDefault();

            SelectedRoleKiller = KillerRoles.FirstOrDefault();

            KillerAnonymousMode = false;
            KillerBot = false;
            KillerPrestige = 0;
            KillerAccount = 0;

            SelectedKillerFirstPerk = null;
            SelectedKillerSecondPerk = null;
            SelectedKillerThirdPerk = null;
            SelectedKillerFourthPerk = null;

            SelectedKillerFirstAddon = null;
            SelectedKillerSecondAddon = null;
        }

        private void SetNullSurvivorData()
        {
            /* Выбор платформы, на которой играют выжившие на Steam */

            Platform steam = Platforms.FirstOrDefault();

            SelectedFirstSurvivorPlatform = steam;
            SelectedSecondSurvivorPlatform = steam;
            SelectedThirdSurvivorPlatform = steam;
            SelectedFourthSurvivorPlatform = steam;

            /* Выбор типа смерти игрока в матче в "От крюка" */

            TypeDeath onHock = TypeDeaths.FirstOrDefault();

            SelectedFirstSurvivorTypeDeath = onHock;
            SelectedSecondSurvivorTypeDeath = onHock;
            SelectedThirdSurvivorTypeDeath = onHock;
            SelectedFourthSurvivorTypeDeath = onHock;

            /* Установка престижей игроков на выживших в 0 */
            FirstSurvivorPrestige = 0;
            SecondSurvivorPrestige = 0;
            ThirdSurvivorPrestige = 0;
            FourthSurvivorPrestige = 0;

            /* Установка счета игроков на выживших в 0 */
            FirstSurvivorAccount = 0;
            SecondSurvivorAccount = 0;
            ThirdSurvivorAccount = 0;
            FourthSurvivorAccount = 0;

            /* Установка анонимного режима игроков на выживших в "Не анонимный режим" */
            FirstSurvivorAnonymousMode = false;
            SecondSurvivorAnonymousMode = false;
            ThirdSurvivorAnonymousMode = false;
            FourthSurvivorAnonymousMode = false;

            /* Установка информации о выходе игроков на выживших из мматча во время игры "Отрицательно" */
            FirstSurvivorBot = false;
            SecondSurvivorBot = false;
            ThirdSurvivorBot = false;
            FourthSurvivorBot = false;

            /* Сброс первого перка игроков на выживших */
            SelectedFirstSurvivorFirstPerk = null;
            SelectedFirstSurvivorSecondPerk = null;
            SelectedFirstSurvivorThirdPerk = null;
            SelectedFirstSurvivorFourthPerk = null;

            /* Сброс второго перка игроков на выживших */
            SelectedSecondSurvivorFirstPerk = null;
            SelectedSecondSurvivorSecondPerk = null;
            SelectedSecondSurvivorThirdPerk = null;
            SelectedSecondSurvivorFourthPerk = null;

            /* Сброс третьего перка игроков на выживших */
            SelectedThirdSurvivorFirstPerk = null;
            SelectedThirdSurvivorSecondPerk = null;
            SelectedThirdSurvivorThirdPerk = null;
            SelectedThirdSurvivorFourthPerk = null;

            /* Сброс четвертого перка игроков на выживших */
            SelectedFourthSurvivorFirstPerk = null;
            SelectedFourthSurvivorSecondPerk = null;
            SelectedFourthSurvivorThirdPerk = null;
            SelectedFourthSurvivorFourthPerk = null;

            /* Выбор предмета по умолчанию у выживших */

            Item emptyItem = Items.FirstOrDefault(x => x.ItemName == "Отсутствует");

            SelectedFirstSurvivorItem = emptyItem;
            SelectedSecondSurvivorItem = emptyItem;
            SelectedThirdSurvivorItem = emptyItem;
            SelectedFourthSurvivorItem = emptyItem;

            /* Сброс подношений игроков на выживших */

            Role firstRole = SurvivorRoles.FirstOrDefault();

            SelectedRoleFirstSurvivor = firstRole;
            SelectedRoleSecondSurvivor = firstRole;
            SelectedRoleThirdSurvivor = firstRole;
            SelectedRoleFourthSurvivor = firstRole;
        }

        private void SetNullGameData()
        {
            WhoPlacedMap whoPlacedMap = WhoPlacedMaps.FirstOrDefault();
            CheckKills();
            CountHooks = 0;
            SelectedMap = null;
            CountNumberRecentGenerators = 0;
            SelectedWhoPlacedMap = whoPlacedMap;
            SelectedWhoPlacedMapWin = whoPlacedMap;
            DescriptionGame = string.Empty;
        }

        private void SetNullImage()
        {
            ResultMatchImage = null;
            StartMatchImage = null;
            EndMatchImage = null;

            ResultMatchKillerImage = null;

            ResultMatchFirstSurvivorImage = null;
            ResultMatchSecondSurvivorImage = null;
            ResultMatchThirdSurvivorImage = null;
            ResultMatchFourthSurvivorImage = null;
        }

        private void SetNullBuild()
        {
            SelectedKillerBuild = null;
        }

        #endregion

        #endregion

        #region Блок "Изображение"

        #region Свойства

        public ObservableCollection<ImageInformation> ImagesList { get; set; } = [];

        private ImageInformation _selectedImage;
        public ImageInformation SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
            }
        }

        private string _pathToFolder;
        public string PathToFolder
        {
            get => _pathToFolder;
            set
            {
                _pathToFolder = value;
            }
        }

        private string _pathsText;
        public string PathsText
        {
            get => _pathsText;
            set
            {
                _pathsText = value;
            }
        }

        private BitmapImage _resultMatchImage;
        public BitmapImage ResultMatchImage
        {
            get => _resultMatchImage;
            set
            {
                _resultMatchImage = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _resultMatchFirstSurvivorImage;
        public BitmapImage ResultMatchFirstSurvivorImage
        {
            get => _resultMatchFirstSurvivorImage;
            set
            {
                _resultMatchFirstSurvivorImage = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _resultMatchSecondSurvivorImage;
        public BitmapImage ResultMatchSecondSurvivorImage
        {
            get => _resultMatchSecondSurvivorImage;
            set
            {
                _resultMatchSecondSurvivorImage = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _resultMatchThirdSurvivorImage;
        public BitmapImage ResultMatchThirdSurvivorImage
        {
            get => _resultMatchThirdSurvivorImage;
            set
            {
                _resultMatchThirdSurvivorImage = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _resultMatchFourthSurvivorImage;
        public BitmapImage ResultMatchFourthSurvivorImage
        {
            get => _resultMatchFourthSurvivorImage;
            set
            {
                _resultMatchFourthSurvivorImage = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _resultMatchKillerImage;
        public BitmapImage ResultMatchKillerImage
        {
            get => _resultMatchKillerImage;
            set
            {
                _resultMatchKillerImage = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _startMatchImage;
        public BitmapImage StartMatchImage
        {
            get => _startMatchImage;
            set
            {
                _startMatchImage = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _endMatchImage;
        public BitmapImage EndMatchImage
        {
            get => _endMatchImage;
            set
            {
                _endMatchImage = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Команды

        private RelayCommand _loadImageCommand;
        public RelayCommand LoadImageCommand { get => _loadImageCommand ??= new(async obj => { await GetImageAsync(); }); }

        private RelayCommand _loadResultMatchImageCommand;
        public RelayCommand LoadResultMatchImageCommand
        {
            get => _loadResultMatchImageCommand ??= new(async obj =>
        {
            var result = await ImageHelper.GetResultMatchImageAsync(SelectedImage.PathImage);

            ResultMatchKillerImage = result.Killer;
            ResultMatchFirstSurvivorImage = result.FirstSurvivor;
            ResultMatchSecondSurvivorImage = result.SecondSurvivor;
            ResultMatchThirdSurvivorImage = result.ThirdSurvivor;
            ResultMatchFourthSurvivorImage = result.FourthSurvivor;
            ResultMatchImage = result.ResultMatchImage;

            EndTime = SelectedImage.FileCreatedTime;
            SubstactTime();
            SelectedDateTimeGameMatch = SelectedImage.FileCreatedTime;
        });
        }

        private RelayCommand _loadStartMatchImageCommand;
        public RelayCommand LoadStartMatchImageCommand
        {
            get => _loadStartMatchImageCommand ??= new(async obj =>
        {
            StartMatchImage = await ImageHelper.GetStartMatchImageAsync(SelectedImage.PathImage);
            StartTime = SelectedImage.FileCreatedTime;
            SubstactTime();
        });
        }

        private RelayCommand _loadEndMatchImageCommand;
        public RelayCommand LoadEndMatchImageCommand { get => _loadEndMatchImageCommand ??= new(async obj => { EndMatchImage = await ImageHelper.GetEndMatchImageAsync(SelectedImage.PathImage); }); }

        private RelayCommand _clearImageListCommand;
        public RelayCommand ClearImageListCommand { get => _clearImageListCommand ??= new(obj => { ImagesList.Clear(); }); }

        private RelayCommand _deleteSelectedImageCommand;
        public RelayCommand DeleteSelectedImageCommand { get => _deleteSelectedImageCommand ??= new(obj => { DeleteSelectImage(); }); }

        #endregion

        // TODO: Добавить в настройки выбор пути к папке с скриншотами
        #region Методы 

        private async Task GetImageAsync()
        {
            string[] files = Directory.GetFiles(@"D:\Steam\userdata\189964443\760\remote\381210\screenshots");

            ImagesList.Clear();

            foreach (var item in files)
            {
                FileInfo fileInfo = new(item);
                ImagesList.Add(new ImageInformation
                {
                    PathImage = item,
                    ResizeImage = await GetResizeImageFromDirectoryTumbnails(Path.GetFileName(item)),
                    FileName = Path.GetFileName(item),
                    FileCreatedTime = fileInfo.CreationTime
                });
            }
        }

        private static async Task<string> GetResizeImageFromDirectoryTumbnails(string searchImageName)
        {
            return await Task.Run(() =>
            {
                string[] TumbnailsImagePaths = Directory.GetFiles(@"D:\Steam\userdata\189964443\760\remote\381210\screenshots\thumbnails")
                    .Where(file => Path.GetFileName(file) == searchImageName).ToArray();

                if (TumbnailsImagePaths.Length > 0)
                {
                    return TumbnailsImagePaths[0];
                }
                else
                {
                    return null;
                }

            });
        }

        private void SubstactTime()
        {

            string startTimeString = StartTime.ToString();
            string endTimeString = EndTime.ToString();

            DateTime startTime = DateTime.Parse(startTimeString);
            DateTime endTime = DateTime.Parse(endTimeString);

            TimeSpan timePlayed = endTime - startTime;

            double differenceInHours = timePlayed.TotalHours;
            double differenceInMunutes = timePlayed.TotalMinutes;
            double differenceInSeconds = timePlayed.TotalSeconds;

            DurationMatch = $"{timePlayed}";
        }

        private async void DeleteSelectImage()
        {
            if (_dialogService.ShowMessageButtons("Вы точно хотите удалить изображение с устройства?",
                                                  "Предупреждение!",
                                                  TypeMessage.Warning,
                                                  MessageButtons.YesNoCancel) == MessageButtons.Yes)
            {
                if (File.Exists(SelectedImage.PathImage))
                {
                    File.Delete(SelectedImage.PathImage);
                    await GetImageAsync();
                    ResultMatchImage = null;
                    StartMatchImage = null;
                    EndMatchImage = null;
                }
            }
        }

        #endregion

        #endregion

        #region Проверки

        #region Проверка коректности введеного числа престижа

        private static bool CheckPrestige(int number)
        {
            if (number > 100 | number < 0) return false;
            else return true;
        }

        #endregion

        #region Проверка кол-во килов

        private void CheckKills()
        {
            if (SelectedFirstSurvivorTypeDeath != null || SelectedSecondSurvivorTypeDeath != null || SelectedThirdSurvivorTypeDeath != null || SelectedFourthSurvivorTypeDeath == null)
            {
                TypeDeath Escaped = TypeDeaths.FirstOrDefault(x => x.IdTypeDeath == 5);

                var selectedTypes = new List<TypeDeath>()
            {
                SelectedFirstSurvivorTypeDeath,
                SelectedSecondSurvivorTypeDeath,
                SelectedThirdSurvivorTypeDeath,
                SelectedFourthSurvivorTypeDeath
            };
                CountKills = selectedTypes.Count(type => type != Escaped);
            }
            else return;
        }

        #endregion

        #endregion
    }
}