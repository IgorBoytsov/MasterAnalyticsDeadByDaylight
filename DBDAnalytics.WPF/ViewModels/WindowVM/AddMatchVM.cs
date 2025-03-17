using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Helpers;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class AddMatchVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IGameStatisticService _gameStatisticService;
        private readonly IMatchAttributeService _matchAttributeService;
        private readonly IOfferingService _offeringService;
        private readonly ITypeDeathService _typeDeathService;
        private readonly IMapService _mapService;
        private readonly IWhoPlacedMapService _whoPlacedMapService;
        private readonly IGameModeService _gameModeService;
        private readonly IGameEventService _gameEventService;
        private readonly IPlatformService _platformService;
        private readonly IAssociationService _associationService;
        private readonly IPatchService _patchService;
        private readonly IRoleService _roleService;
        private readonly IKillerInfoService _killerInfoService;
        private readonly IKillerAddonService _killerAddonService;
        private readonly IKillerPerkService _killerPerkService;
        private readonly ISurvivorInfoService _survivorInfoService;
        private readonly ISurvivorPerkService _survivorPerkService;
        private readonly IItemService _itemService;
        private readonly IItemAddonService _itemAddonService;
        private readonly IKillerService _killerService;
        private readonly ISurvivorService _survivorService;

        public AddMatchVM(IWindowNavigationService windowNavigationService, IPageNavigationService pageNavigationService,
                          IGameStatisticService gameStatisticService, IMatchAttributeService matchAttributeService,
                          IOfferingService offeringService, ITypeDeathService typeDeathService, IMapService mapService, IWhoPlacedMapService whoPlacedMapService,
                          IGameModeService gameModeService, IGameEventService gameEventService, IPlatformService platformService, IAssociationService associationService, IPatchService patchService, IRoleService roleService,
                          IKillerService killerService, IKillerInfoService killerInfoService, IKillerAddonService killerAddonService, IKillerPerkService killerPerkService,
                          ISurvivorService survivorService, ISurvivorInfoService survivorInfoService, ISurvivorPerkService survivorPerkService,
                          IItemService itemService, IItemAddonService itemAddonService) : base(windowNavigationService, pageNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _gameStatisticService = gameStatisticService;
            _matchAttributeService = matchAttributeService;
            _offeringService = offeringService;
            _typeDeathService = typeDeathService;
            _mapService = mapService;
            _whoPlacedMapService = whoPlacedMapService;
            _gameModeService = gameModeService;
            _gameEventService = gameEventService;
            _platformService = platformService;
            _associationService = associationService;
            _patchService = patchService;
            _roleService = roleService;
            _killerInfoService = killerInfoService;
            _killerAddonService = killerAddonService;
            _killerPerkService = killerPerkService;
            _survivorInfoService = survivorInfoService;
            _survivorPerkService = survivorPerkService;
            _itemService = itemService;
            _itemAddonService = itemAddonService;
            _killerService = killerService;
            _survivorService = survivorService;

            Title = "Добавление матча";

            GetData();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            if (typeParameter == TypeParameter.AddAndNotification) GettingAddAndNotificationData(parameter);

            if (typeParameter == TypeParameter.UpdateAndNotification) GettingUpdateAndNotificationData(parameter);

            if (typeParameter == TypeParameter.DeleteAndNotification) GettingDeleteAndNotificationData(parameter);
        }

        #region Обработка получаемых значений с других окон | страниц

        private void GettingAddAndNotificationData(object value)
        {
            Action action = value switch
            {
                KillerDTO => () => Killers.Add((KillerDTO)value)
                ,
                KillerPerkDTO => () => KillerPerks.Add((KillerPerkDTO)value)
                ,
                PlatformDTO => () => Platforms.Add((PlatformDTO)value)
                ,
                TypeDeathDTO => () => TypeDeaths.Add((TypeDeathDTO)value)
                ,
                MapDTO => () => Maps.Add((MapDTO)value)
                ,
                PatchDTO => () => Patches.Add((PatchDTO)value)
                ,
                GameModeDTO => () => GameModes.Add((GameModeDTO)value)
                ,
                GameEventDTO => () => GameEvents.Add((GameEventDTO)value)
                ,
                WhoPlacedMapDTO => () => WhoPlacedMaps.Add((WhoPlacedMapDTO)value)
                ,
                SurvivorDTO => () => Survivors.Add((SurvivorDTO)value)
                ,
                SurvivorPerkDTO => () => SurvivorPerks.Add((SurvivorPerkDTO)value)
                ,
                ItemDTO => () => Items.Add((ItemDTO)value)
                ,
                MatchAttributeDTO => () => MatchAttributes.Add((MatchAttributeDTO)value)
                ,
                KillerAddonDTO => () =>
                {
                    _killerAddons.Add((KillerAddonDTO)value);
                    LoadKillerAddons();
                }
                ,
                OfferingDTO => () =>
                {
                    _offerings.Add((OfferingDTO)value);

                    LoadFirstSurvivorOffering();
                    LoadSecondSurvivorOffering();
                    LoadThirdSurvivorOffering();
                    LoadFourthSurvivorOffering();

                    LoadKillerOffering();
                }
                ,
                ItemAddonDTO => () =>
                {
                    _itemAddons.Add((ItemAddonDTO)value);

                    LoadFirstSurvivorItemAddon();
                    LoadSecondSurvivorItemAddon();
                    LoadThirdSurvivorItemAddon();
                    LoadFourthSurvivorItemAddon();
                }
                ,
                PlayerAssociationDTO => () =>
                {
                    KillerPlayerAssociations.Add((PlayerAssociationDTO)value);
                    GetPlayerAssociations();
                }
                ,
                RoleDTO => () =>
                {
                    _roles.Add((RoleDTO)value);
                    LoadSurvivorRoles();
                    LoadKillerRoles();
                }
                ,
                _ => () => throw new Exception("Такой тип не обрабатывается.")
            };

            action?.Invoke();
        }

        private void GettingUpdateAndNotificationData(object value)
        {
            Action action = value switch
            {
                KillerDTO => () => Killers.ReplaceItem(Killers.FirstOrDefault(x => x.IdKiller == ((KillerDTO)value).IdKiller), (KillerDTO)value) 
                ,
                KillerPerkDTO => () => KillerPerks.ReplaceItem(KillerPerks.FirstOrDefault(x => x.IdKillerPerk == ((KillerPerkDTO)value).IdKillerPerk), (KillerPerkDTO)value)
                ,
                PlatformDTO => () => Platforms.ReplaceItem(Platforms.FirstOrDefault(x => x.IdPlatform == ((PlatformDTO)value).IdPlatform), (PlatformDTO)value)
                ,
                TypeDeathDTO => () => TypeDeaths.ReplaceItem(TypeDeaths.FirstOrDefault(x => x.IdTypeDeath == ((TypeDeathDTO)value).IdTypeDeath), (TypeDeathDTO)value)
                ,
                MapDTO => () => Maps.ReplaceItem(Maps.FirstOrDefault(x => x.IdMap == ((MapDTO)value).IdMap), (MapDTO)value)
                ,
                PatchDTO => () => Patches.ReplaceItem(Patches.FirstOrDefault(x => x.IdPatch == ((PatchDTO)value).IdPatch), (PatchDTO)value)
                ,
                GameModeDTO => () => GameModes.ReplaceItem(GameModes.FirstOrDefault(x => x.IdGameMode == ((GameModeDTO)value).IdGameMode), (GameModeDTO)value)
                ,
                GameEventDTO => () => GameEvents.ReplaceItem(GameEvents.FirstOrDefault(x => x.IdGameEvent == ((GameEventDTO)value).IdGameEvent), (GameEventDTO)value)
                ,
                WhoPlacedMapDTO => () => WhoPlacedMaps.ReplaceItem(WhoPlacedMaps.FirstOrDefault(x => x.IdWhoPlacedMap == ((WhoPlacedMapDTO)value).IdWhoPlacedMap), (WhoPlacedMapDTO)value)
                ,
                SurvivorDTO => () => Survivors.ReplaceItem(Survivors.FirstOrDefault(x => x.IdSurvivor == ((SurvivorDTO)value).IdSurvivor), (SurvivorDTO)value)
                ,
                SurvivorPerkDTO => () => SurvivorPerks.ReplaceItem(SurvivorPerks.FirstOrDefault(x => x.IdSurvivorPerk == ((SurvivorPerkDTO)value).IdSurvivorPerk), (SurvivorPerkDTO)value)
                ,
                ItemDTO => () => Items.ReplaceItem(Items.FirstOrDefault(x => x.IdItem == ((ItemDTO)value).IdItem), (ItemDTO)value)
                ,
                MatchAttributeDTO => () => MatchAttributes.ReplaceItem(MatchAttributes.FirstOrDefault(x => x.IdMatchAttribute == ((MatchAttributeDTO)value).IdMatchAttribute), (MatchAttributeDTO)value)
                ,
                KillerAddonDTO => () =>
                {
                    _killerAddons.ReplaceItem(_killerAddons.FirstOrDefault(x => x.IdKillerAddon == ((KillerAddonDTO)value).IdKillerAddon), (KillerAddonDTO)value);
                    LoadKillerAddons();
                }
                ,
                OfferingDTO => () =>
                {
                    _offerings.ReplaceItem(_offerings.FirstOrDefault(x => x.IdOffering == ((OfferingDTO)value).IdOffering), (OfferingDTO)value);

                    LoadFirstSurvivorOffering();
                    LoadSecondSurvivorOffering();
                    LoadThirdSurvivorOffering();
                    LoadFourthSurvivorOffering();

                    LoadKillerOffering();
                }
                ,
                ItemAddonDTO => () =>
                {
                    _itemAddons.ReplaceItem(_itemAddons.FirstOrDefault(x => x.IdItemAddon == ((ItemAddonDTO)value).IdItemAddon), (ItemAddonDTO)value);

                    LoadFirstSurvivorItemAddon();
                    LoadSecondSurvivorItemAddon();
                    LoadThirdSurvivorItemAddon();
                    LoadFourthSurvivorItemAddon();
                }
                ,
                PlayerAssociationDTO => () =>
                {
                    KillerPlayerAssociations.ReplaceItem(KillerPlayerAssociations.FirstOrDefault(x => x.IdPlayerAssociation == ((PlayerAssociationDTO)value).IdPlayerAssociation), (PlayerAssociationDTO)value);
                    GetPlayerAssociations();
                }
                ,
                RoleDTO => () =>
                {
                    _roles.ReplaceItem(_roles.FirstOrDefault(x => x.IdRole == ((RoleDTO)value).IdRole), (RoleDTO)value);
                    LoadSurvivorRoles();
                    LoadKillerRoles();
                }
                ,
                _ => () => throw new Exception("Такой тип не обрабатывается.")
            };

            action?.Invoke();
        }

        private void GettingDeleteAndNotificationData(object value)
        {
            Action action = value switch
            {
                KillerDTO => () => Killers.Remove(Killers.FirstOrDefault(x => x.IdKiller == ((KillerDTO)value).IdKiller))
                ,
                KillerPerkDTO => () => KillerPerks.Remove(KillerPerks.FirstOrDefault(x => x.IdKillerPerk == ((KillerPerkDTO)value).IdKillerPerk))
                ,
                PlatformDTO => () => Platforms.Remove(Platforms.FirstOrDefault(x => x.IdPlatform == ((PlatformDTO)value).IdPlatform))
                ,
                TypeDeathDTO => () => TypeDeaths.Remove(TypeDeaths.FirstOrDefault(x => x.IdTypeDeath == ((TypeDeathDTO)value).IdTypeDeath))
                ,
                MapDTO => () => Maps.Remove(Maps.FirstOrDefault(x => x.IdMap == ((MapDTO)value).IdMap))
                ,
                PatchDTO => () => Patches.Remove(Patches.FirstOrDefault(x => x.IdPatch == ((PatchDTO)value).IdPatch))
                ,
                GameModeDTO => () => GameModes.Remove(GameModes.FirstOrDefault(x => x.IdGameMode == ((GameModeDTO)value).IdGameMode))
                ,
                GameEventDTO => () => GameEvents.Remove(GameEvents.FirstOrDefault(x => x.IdGameEvent == ((GameEventDTO)value).IdGameEvent))
                ,
                WhoPlacedMapDTO => () => WhoPlacedMaps.Remove(WhoPlacedMaps.FirstOrDefault(x => x.IdWhoPlacedMap == ((WhoPlacedMapDTO)value).IdWhoPlacedMap))
                ,
                SurvivorDTO => () => Survivors.Remove(Survivors.FirstOrDefault(x => x.IdSurvivor == ((SurvivorDTO)value).IdSurvivor))
                ,
                SurvivorPerkDTO => () => SurvivorPerks.Remove(SurvivorPerks.FirstOrDefault(x => x.IdSurvivorPerk == ((SurvivorPerkDTO)value).IdSurvivorPerk))
                ,
                ItemDTO => () => Items.Remove(Items.FirstOrDefault(x => x.IdItem == ((ItemDTO)value).IdItem))
                ,
                MatchAttributeDTO => () => MatchAttributes.Remove(MatchAttributes.FirstOrDefault(x => x.IdMatchAttribute == ((MatchAttributeDTO)value).IdMatchAttribute))
                ,
                KillerAddonDTO => () =>
                {
                    _killerAddons.Remove(_killerAddons.FirstOrDefault(x => x.IdKillerAddon == ((KillerAddonDTO)value).IdKillerAddon));
                    LoadKillerAddons();
                }
                ,
                OfferingDTO => () =>
                {
                    _offerings.Remove(_offerings.FirstOrDefault(x => x.IdOffering == ((OfferingDTO)value).IdOffering));

                    LoadFirstSurvivorOffering();
                    LoadSecondSurvivorOffering();
                    LoadThirdSurvivorOffering();
                    LoadFourthSurvivorOffering();

                    LoadKillerOffering();
                }
                ,
                ItemAddonDTO => () =>
                {
                    _itemAddons.Remove(_itemAddons.FirstOrDefault(x => x.IdItemAddon == ((ItemAddonDTO)value).IdItemAddon));

                    LoadFirstSurvivorItemAddon();
                    LoadSecondSurvivorItemAddon();
                    LoadThirdSurvivorItemAddon();
                    LoadFourthSurvivorItemAddon();
                }
                ,
                PlayerAssociationDTO => () =>
                {
                    KillerPlayerAssociations.Remove(KillerPlayerAssociations.FirstOrDefault(x => x.IdPlayerAssociation == ((PlayerAssociationDTO)value).IdPlayerAssociation));
                    GetPlayerAssociations();
                }
                ,
                RoleDTO => () =>
                {
                    _roles.Remove(_roles.FirstOrDefault(x => x.IdRole == ((RoleDTO)value).IdRole));
                    LoadSurvivorRoles();
                    LoadKillerRoles();
                }
                ,
                _ => () => throw new Exception("Такой тип не обрабатывается.")
            };

            action?.Invoke();
        }

        #endregion

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Свойство : Title

        public string Title { get; set; }

        #endregion

        #region Общие : Collecrions 

        private List<OfferingDTO> _offerings = [];

        private List<RoleDTO> _roles = [];

        private List<PlayerAssociationDTO> _playerAssociations = [];

        public ObservableCollection<PlatformDTO> Platforms { get; private set; } = [];

        #endregion

        #region Список изображение матча : Collections | Selected

        public ObservableCollection<ImageInfo> ImagePaths { get; private set; } = [];

        private ImageInfo _selectedImagePath;
        public ImageInfo SelectedImagePath
        {
            get => _selectedImagePath;
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Киллеры--*/

        #region Основная информация : Collections | Selected | Prop

        public ObservableCollection<KillerDTO> Killers { get; private set; } = [];

        public ObservableCollection<PlayerAssociationDTO> KillerPlayerAssociations { get; private set; } = [];

        private KillerDTO _selectedKiller;
        public KillerDTO SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                _selectedKiller = value;

                if (value == null)
                {
                    KillerAddons.Clear();
                    return;
                }

                SelectedKillerFirstAddon = null;
                SelectedKillerSecondAddon = null;

                LoadKillerAddons();
                OnPropertyChanged();
            }
        }

        private PlatformDTO _selectedKillerPlatform;
        public PlatformDTO SelectedKillerPlatform
        {
            get => _selectedKillerPlatform;
            set
            {
                _selectedKillerPlatform = value;
                OnPropertyChanged();
            }
        }

        private PlayerAssociationDTO _selectedKillerPlayerAssociation;
        public PlayerAssociationDTO SelectedKillerPlayerAssociation
        {
            get => _selectedKillerPlayerAssociation;
            set
            {
                _selectedKillerPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        private int _killerPrestige;
        public int KillerPrestige
        {
            get => _killerPrestige;
            set
            {
                // TODO : Заменить MessageBox на кастомное окно или аналог.
                if (!CheckPrestige(value)) MessageBox.Show("Престиж не может быть больше 100!");
                else
                {
                    _killerPrestige = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _killerAccount;
        public int KillerAccount
        {
            get => _killerAccount;
            set
            {
                // TODO : Заменить MessageBox на кастомное окно или аналог.
                if (value < 0) MessageBox.Show("Престиж не может быть больше 100!");
                else
                {
                    _killerAccount = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #region Перки и аддоны : Collections | Selected

        private List<KillerPerkDTO> _killerPerks { get; set; } = [];
        public ObservableCollection<KillerPerkDTO> KillerPerks { get; private set; } = [];

        private List<KillerAddonDTO> _killerAddons { get; set; } = [];
        public ObservableCollection<KillerAddonDTO> KillerAddons{ get; private set; } = [];

        public ObservableCollection<OfferingDTO> KillerOfferings { get; private set; } = [];

        private KillerPerkDTO _selectedKillerPerk;
        public KillerPerkDTO SelectedKillerPerk
        {
            get => _selectedKillerPerk;
            set
            {
                _selectedKillerPerk = value;
                OnPropertyChanged();
            }
        } 
        
        private KillerAddonDTO _selectedKillerAddon;
        public KillerAddonDTO SelectedKillerAddon
        {
            get => _selectedKillerAddon;
            set
            {
                _selectedKillerAddon = value;
                OnPropertyChanged();
            }
        }  
        
        private KillerPerkDTO _selectedKillerFirstPerk;
        public KillerPerkDTO SelectedKillerFirstPerk
        {
            get => _selectedKillerFirstPerk;
            set
            {
                _selectedKillerFirstPerk = value;
                OnPropertyChanged();
            }
        }

        private KillerPerkDTO _selectedKillerSecondPerk;
        public KillerPerkDTO SelectedKillerSecondPerk
        {
            get => _selectedKillerSecondPerk;
            set
            {
                _selectedKillerSecondPerk = value;
                OnPropertyChanged();
            }
        }

        private KillerPerkDTO _selectedKillerThirdPerk;
        public KillerPerkDTO SelectedKillerThirdPerk
        {
            get => _selectedKillerThirdPerk;
            set
            {
                _selectedKillerThirdPerk = value;
                OnPropertyChanged();
            }
        } 
        
        private KillerPerkDTO _selectedKillerFourthPerk;
        public KillerPerkDTO SelectedKillerFourthPerk
        {
            get => _selectedKillerFourthPerk;
            set
            {
                _selectedKillerFourthPerk = value;
                OnPropertyChanged();
            }
        }

        private KillerAddonDTO _selectedKillerFirstAddon;
        public KillerAddonDTO SelectedKillerFirstAddon
        {
            get => _selectedKillerFirstAddon;
            set
            {
                _selectedKillerFirstAddon = value;
                OnPropertyChanged();
            }
        }

        private KillerAddonDTO _selectedKillerSecondAddon;
        public KillerAddonDTO SelectedKillerSecondAddon
        {
            get => _selectedKillerSecondAddon;
            set
            {
                _selectedKillerSecondAddon = value;
                OnPropertyChanged();
            }
        }

        private string _searchKillerPerkInput;
        public string SearchKillerPerkInput
        {
            get => _searchKillerPerkInput;
            set
            {
                _searchKillerPerkInput = value;
                SearchKillerPerk();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Подношение : Collection | Selected

        public ObservableCollection<RoleDTO> KillerRoles { get; set; } = [];

        private OfferingDTO _selectedKillerOffering;
        public OfferingDTO SelectedKillerOffering
        {
            get => _selectedKillerOffering;
            set
            {
                _selectedKillerOffering = value;
                OnPropertyChanged();
            }
        }

        private RoleDTO _selectedKillerRole;
        public RoleDTO SelectedKillerRole
        {
            get => _selectedKillerRole;
            set
            {
                _selectedKillerRole = value;

                LoadKillerOffering();
                OnPropertyChanged();
            }
        }


        #endregion

        /*--Выжившие--*/

        #region Общая ифнормация : Collection

        public List<ItemAddonDTO> _itemAddons { get; set; } = [];

        public ObservableCollection<SurvivorDTO> Survivors { get; set; } = [];

        private List<SurvivorPerkDTO> _survivorPerks { get; set; } = [];
        public ObservableCollection<SurvivorPerkDTO> SurvivorPerks { get; set; } = [];

        public ObservableCollection<ItemDTO> Items { get; set; } = [];

        public ObservableCollection<TypeDeathDTO> TypeDeaths { get; private set; } = [];

        public ObservableCollection<PlayerAssociationDTO> SurvivorPlayerAssociations { get; private set; } = [];

        public ObservableCollection<RoleDTO> SurvivorRoles { get; private set; } = [];

        #endregion

        #region Выжившие - Общие : Collections | Selected | Prop

        private SurvivorPerkDTO _selectedSurvivorPerk;
        public SurvivorPerkDTO SelectedSurvivorPerk
        {
            get => _selectedSurvivorPerk;
            set
            {
                _selectedSurvivorPerk = value;
                OnPropertyChanged();
            }
        }
        
        private OfferingDTO _selectedSurvivorOffering;
        public OfferingDTO SelectedSurvivorOffering
        {
            get => _selectedSurvivorOffering;
            set
            {
                _selectedSurvivorOffering = value;
                OnPropertyChanged();
            }
        }
        
        private ItemAddonDTO _selectedItemAddon;
        public ItemAddonDTO SelectedItemAddon
        {
            get => _selectedItemAddon;
            set
            {
                _selectedItemAddon = value;
                OnPropertyChanged();
            }
        }

        private string _searchSurvivorPerkInput;
        public string SearchSurvivorPerkInput
        {
            get => _searchSurvivorPerkInput;
            set
            {
                _searchSurvivorPerkInput = value;
                SearchSurvivorPerk();
                OnPropertyChanged();
            }
        }
        #endregion

        #region Выживший №1 - Основная информация : Selected | Prop

        private SurvivorDTO _selectedFirstSurvivor;
        public SurvivorDTO SelectedFirstSurvivor
        {
            get => _selectedFirstSurvivor;
            set
            {
                _selectedFirstSurvivor = value;
                OnPropertyChanged();
            }
        }

        private PlatformDTO _selectedFirstSurvivorPlatform;
        public PlatformDTO SelectedFirstSurvivorPlatform
        {
            get => _selectedFirstSurvivorPlatform;
            set
            {
                _selectedFirstSurvivorPlatform = value;
                OnPropertyChanged();
            }
        }

        private TypeDeathDTO _selectedFirstSurvivorTypeDeath;
        public TypeDeathDTO SelectedFirstSurvivorTypeDeath
        {
            get => _selectedFirstSurvivorTypeDeath;
            set
            {
                _selectedFirstSurvivorTypeDeath = value;
                CheckKills();
                OnPropertyChanged();
            }
        }

        private PlayerAssociationDTO _selectedFirstSurvivorPlayerAssociation;
        public PlayerAssociationDTO SelectedFirstSurvivorPlayerAssociation
        {
            get => _selectedFirstSurvivorPlayerAssociation;
            set
            {
                _selectedFirstSurvivorPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        private int _firstSurvivorPrestige;
        public int FirstSurvivorPrestige
        {
            get => _firstSurvivorPrestige;
            set
            {
                // TODO : Заменить MessageBox на кастомное окно или аналог.
                if (!CheckPrestige(value)) MessageBox.Show("Престиж не может быть больше 100!");
                else
                {
                    _firstSurvivorPrestige = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #endregion

        #region Выживший №1 - Перки : Selected | Prop

        private SurvivorPerkDTO _selectedFirstSurvivorFirstPerk;
        public SurvivorPerkDTO SelectedFirstSurvivorFirstPerk
        {
            get => _selectedFirstSurvivorFirstPerk;
            set
            {
                _selectedFirstSurvivorFirstPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedFirstSurvivorSecondPerk;
        public SurvivorPerkDTO SelectedFirstSurvivorSecondPerk
        {
            get => _selectedFirstSurvivorSecondPerk;
            set
            {
                _selectedFirstSurvivorSecondPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedFirstSurvivorThirdPerk;
        public SurvivorPerkDTO SelectedFirstSurvivorThirdPerk
        {
            get => _selectedFirstSurvivorThirdPerk;
            set
            {
                _selectedFirstSurvivorThirdPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedFirstSurvivorFourthPerk;
        public SurvivorPerkDTO SelectedFirstSurvivorFourthPerk
        {
            get => _selectedFirstSurvivorFourthPerk;
            set
            {
                _selectedFirstSurvivorFourthPerk = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выживший №1 - Предмет + аддоны : Collections | Selected

        public ObservableCollection<ItemAddonDTO> FirstSurvivorItemAddons { get; set; } = [];

        private ItemDTO _selectedFirstSurvivorItem;
        public ItemDTO SelectedFirstSurvivorItem
        {
            get => _selectedFirstSurvivorItem;
            set
            {
                _selectedFirstSurvivorItem = value;
                LoadFirstSurvivorItemAddon();
                OnPropertyChanged();
            }
        }
        
        private ItemAddonDTO _selectedFirstSurvivorFirstItemAddon;
        public ItemAddonDTO SelectedFirstSurvivorFirstItemAddon
        {
            get => _selectedFirstSurvivorFirstItemAddon;
            set
            {
                _selectedFirstSurvivorFirstItemAddon = value;
                OnPropertyChanged();
            }
        } 

        private ItemAddonDTO _selectedFirstSurvivorSecondItemAddon;
        public ItemAddonDTO SelectedFirstSurvivorSecondItemAddon
        {
            get => _selectedFirstSurvivorSecondItemAddon;
            set
            {
                _selectedFirstSurvivorSecondItemAddon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выживший №1 - Подношение : Collection | Selected

        public ObservableCollection<OfferingDTO> FirstSurvivorOfferings { get; private set; } = [];

        private OfferingDTO _selectedFirstSurvivorOffering;
        public OfferingDTO SelectedFirstSurvivorOffering
        {
            get => _selectedFirstSurvivorOffering;
            set
            {
                _selectedFirstSurvivorOffering = value;
                OnPropertyChanged();
            }
        }
        
        private RoleDTO _selectedRoleFirstSurvivor;
        public RoleDTO SelectedRoleFirstSurvivor
        {
            get => _selectedRoleFirstSurvivor;
            set
            {
                _selectedRoleFirstSurvivor = value;
                LoadFirstSurvivorOffering();
                OnPropertyChanged();
            }
        }

        #endregion


        #region Выживший №2 - Основная информация : Selected | Prop

        private SurvivorDTO _selectedSecondSurvivor;
        public SurvivorDTO SelectedSecondSurvivor
        {
            get => _selectedSecondSurvivor;
            set
            {
                _selectedSecondSurvivor = value;
                OnPropertyChanged();
            }
        }

        private PlatformDTO _selectedSecondSurvivorPlatform;
        public PlatformDTO SelectedSecondSurvivorPlatform
        {
            get => _selectedSecondSurvivorPlatform;
            set
            {
                _selectedSecondSurvivorPlatform = value;
                OnPropertyChanged();
            }
        }

        private TypeDeathDTO _selectedSecondSurvivorTypeDeath;
        public TypeDeathDTO SelectedSecondSurvivorTypeDeath
        {
            get => _selectedSecondSurvivorTypeDeath;
            set
            {
                _selectedSecondSurvivorTypeDeath = value;
                CheckKills();
                OnPropertyChanged();
            }
        }

        private PlayerAssociationDTO _selectedSecondSurvivorPlayerAssociation;
        public PlayerAssociationDTO SelectedSecondSurvivorPlayerAssociation
        {
            get => _selectedSecondSurvivorPlayerAssociation;
            set
            {
                _selectedSecondSurvivorPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        private int _secondSurvivorPrestige;
        public int SecondSurvivorPrestige
        {
            get => _secondSurvivorPrestige;
            set
            {
                // TODO : Заменить MessageBox на кастомное окно или аналог.
                if (!CheckPrestige(value)) MessageBox.Show("Престиж не может быть больше 100!");
                else
                {
                    _secondSurvivorPrestige = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #endregion

        #region Выживший №2 - Перки : Selected | Prop

        private SurvivorPerkDTO _selectedSecondSurvivorFirstPerk;
        public SurvivorPerkDTO SelectedSecondSurvivorFirstPerk
        {
            get => _selectedSecondSurvivorFirstPerk;
            set
            {
                _selectedSecondSurvivorFirstPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedSecondSurvivorSecondPerk;
        public SurvivorPerkDTO SelectedSecondSurvivorSecondPerk
        {
            get => _selectedSecondSurvivorSecondPerk;
            set
            {
                _selectedSecondSurvivorSecondPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedSecondSurvivorThirdPerk;
        public SurvivorPerkDTO SelectedSecondSurvivorThirdPerk
        {
            get => _selectedSecondSurvivorThirdPerk;
            set
            {
                _selectedSecondSurvivorThirdPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedSecondSurvivorFourthPerk;
        public SurvivorPerkDTO SelectedSecondSurvivorFourthPerk
        {
            get => _selectedSecondSurvivorFourthPerk;
            set
            {
                _selectedSecondSurvivorFourthPerk = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выживший №2 - Предмет + аддоны : Collections | Selected

        public ObservableCollection<ItemAddonDTO> SecondSurvivorItemAddons { get; set; } = [];

        private ItemDTO _selectedSecondSurvivorItem;
        public ItemDTO SelectedSecondSurvivorItem
        {
            get => _selectedSecondSurvivorItem;
            set
            {
                _selectedSecondSurvivorItem = value;
                LoadSecondSurvivorItemAddon();
                OnPropertyChanged();
            }
        }

        private ItemAddonDTO _selectedSecondSurvivorFirstItemAddon;
        public ItemAddonDTO SelectedSecondSurvivorFirstItemAddon
        {
            get => _selectedSecondSurvivorFirstItemAddon;
            set
            {
                _selectedSecondSurvivorFirstItemAddon = value;
                OnPropertyChanged();
            }
        }

        private ItemAddonDTO _selectedSecondSurvivorSecondItemAddon;
        public ItemAddonDTO SelectedSecondSurvivorSecondItemAddon
        {
            get => _selectedSecondSurvivorSecondItemAddon;
            set
            {
                _selectedSecondSurvivorSecondItemAddon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выживший №2 - Подношение : Collection | Selected

        public ObservableCollection<OfferingDTO> SecondSurvivorOfferings { get; private set; } = [];

        private OfferingDTO _selectedSecondSurvivorOffering;
        public OfferingDTO SelectedSecondSurvivorOffering
        {
            get => _selectedSecondSurvivorOffering;
            set
            {
                _selectedSecondSurvivorOffering = value;
                OnPropertyChanged();
            }
        }

        private RoleDTO _selectedRoleSecondSurvivor;
        public RoleDTO SelectedRoleSecondSurvivor
        {
            get => _selectedRoleSecondSurvivor;
            set
            {
                _selectedRoleSecondSurvivor = value;
                LoadSecondSurvivorOffering();
                OnPropertyChanged();
            }
        }

        #endregion


        #region Выживший №3 - Основная информация : Selected | Prop

        private SurvivorDTO _selectedThirdSurvivor;
        public SurvivorDTO SelectedThirdSurvivor
        {
            get => _selectedThirdSurvivor;
            set
            {
                _selectedThirdSurvivor = value;
                OnPropertyChanged();
            }
        }

        private PlatformDTO _selectedThirdSurvivorPlatform;
        public PlatformDTO SelectedThirdSurvivorPlatform
        {
            get => _selectedThirdSurvivorPlatform;
            set
            {
                _selectedThirdSurvivorPlatform = value;
                OnPropertyChanged();
            }
        }

        private TypeDeathDTO _selectedThirdSurvivorTypeDeath;
        public TypeDeathDTO SelectedThirdSurvivorTypeDeath
        {
            get => _selectedThirdSurvivorTypeDeath;
            set
            {
                _selectedThirdSurvivorTypeDeath = value;
                CheckKills();
                OnPropertyChanged();
            }
        }

        private PlayerAssociationDTO _selectedThirdSurvivorPlayerAssociation;
        public PlayerAssociationDTO SelectedThirdSurvivorPlayerAssociation
        {
            get => _selectedThirdSurvivorPlayerAssociation;
            set
            {
                _selectedThirdSurvivorPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        private int _thirdSurvivorPrestige;
        public int ThirdSurvivorPrestige
        {
            get => _thirdSurvivorPrestige;
            set
            {
                // TODO : Заменить MessageBox на кастомное окно или аналог.
                if (!CheckPrestige(value)) MessageBox.Show("Престиж не может быть больше 100!");
                else
                {
                    _thirdSurvivorPrestige = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #endregion

        #region Выживший №3 - Перки : Selected | Prop

        private SurvivorPerkDTO _selectedThirdSurvivorFirstPerk;
        public SurvivorPerkDTO SelectedThirdSurvivorFirstPerk
        {
            get => _selectedThirdSurvivorFirstPerk;
            set
            {
                _selectedThirdSurvivorFirstPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedThirdSurvivorSecondPerk;
        public SurvivorPerkDTO SelectedThirdSurvivorSecondPerk
        {
            get => _selectedThirdSurvivorSecondPerk;
            set
            {
                _selectedThirdSurvivorSecondPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedThirdSurvivorThirdPerk;
        public SurvivorPerkDTO SelectedThirdSurvivorThirdPerk
        {
            get => _selectedThirdSurvivorThirdPerk;
            set
            {
                _selectedThirdSurvivorThirdPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedThirdSurvivorFourthPerk;
        public SurvivorPerkDTO SelectedThirdSurvivorFourthPerk
        {
            get => _selectedThirdSurvivorFourthPerk;
            set
            {
                _selectedThirdSurvivorFourthPerk = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выживший №3 - Предмет + аддоны : Collections | Selected

        public ObservableCollection<ItemAddonDTO> ThirdSurvivorItemAddons { get; set; } = [];

        private ItemDTO _selectedThirdSurvivorItem;
        public ItemDTO SelectedThirdSurvivorItem
        {
            get => _selectedThirdSurvivorItem;
            set
            {
                _selectedThirdSurvivorItem = value;
                LoadThirdSurvivorItemAddon();
                OnPropertyChanged();
            }
        }

        private ItemAddonDTO _selectedThirdSurvivorFirstItemAddon;
        public ItemAddonDTO SelectedThirdSurvivorFirstItemAddon
        {
            get => _selectedThirdSurvivorFirstItemAddon;
            set
            {
                _selectedThirdSurvivorFirstItemAddon = value;
                OnPropertyChanged();
            }
        }

        private ItemAddonDTO _selectedThirdSurvivorSecondItemAddon;
        public ItemAddonDTO SelectedThirdSurvivorSecondItemAddon
        {
            get => _selectedThirdSurvivorSecondItemAddon;
            set
            {
                _selectedThirdSurvivorSecondItemAddon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выживший №3 - Подношение : Collection | Selected

        public ObservableCollection<OfferingDTO> ThirdSurvivorOfferings { get; private set; } = [];

        private OfferingDTO _selectedThirdSurvivorOffering;
        public OfferingDTO SelectedThirdSurvivorOffering
        {
            get => _selectedThirdSurvivorOffering;
            set
            {
                _selectedThirdSurvivorOffering = value;
                OnPropertyChanged();
            }
        }

        private RoleDTO _selectedRoleThirdSurvivor;
        public RoleDTO SelectedRoleThirdSurvivor
        {
            get => _selectedRoleThirdSurvivor;
            set
            {
                _selectedRoleThirdSurvivor = value;
                LoadThirdSurvivorOffering();
                OnPropertyChanged();
            }
        }

        #endregion


        #region Выживший №4 - Основная информация : Selected | Prop

        private SurvivorDTO _selectedFourthSurvivor;
        public SurvivorDTO SelectedFourthSurvivor
        {
            get => _selectedFourthSurvivor;
            set
            {
                _selectedFourthSurvivor = value;
                OnPropertyChanged();
            }
        }

        private PlatformDTO _selectedFourthSurvivorPlatform;
        public PlatformDTO SelectedFourthSurvivorPlatform
        {
            get => _selectedFourthSurvivorPlatform;
            set
            {
                _selectedFourthSurvivorPlatform = value;
                OnPropertyChanged();
            }
        }

        private TypeDeathDTO _selectedFourthSurvivorTypeDeath;
        public TypeDeathDTO SelectedFourthSurvivorTypeDeath
        {
            get => _selectedFourthSurvivorTypeDeath;
            set
            {
                _selectedFourthSurvivorTypeDeath = value;
                CheckKills();
                OnPropertyChanged();
            }
        }

        private PlayerAssociationDTO _selectedFourthSurvivorPlayerAssociation;
        public PlayerAssociationDTO SelectedFourthSurvivorPlayerAssociation
        {
            get => _selectedFourthSurvivorPlayerAssociation;
            set
            {
                _selectedFourthSurvivorPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        private int _fourthSurvivorPrestige;
        public int FourthSurvivorPrestige
        {
            get => _fourthSurvivorPrestige;
            set
            {
                // TODO : Заменить MessageBox на кастомное окно или аналог.
                if (!CheckPrestige(value)) MessageBox.Show("Престиж не может быть больше 100!");
                else
                {
                    _fourthSurvivorPrestige = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #endregion

        #region Выживший №4 - Перки : Selected | Prop

        private SurvivorPerkDTO _selectedFourthSurvivorFirstPerk;
        public SurvivorPerkDTO SelectedFourthSurvivorFirstPerk
        {
            get => _selectedFourthSurvivorFirstPerk;
            set
            {
                _selectedFourthSurvivorFirstPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedFourthSurvivorSecondPerk;
        public SurvivorPerkDTO SelectedFourthSurvivorSecondPerk
        {
            get => _selectedFourthSurvivorSecondPerk;
            set
            {
                _selectedFourthSurvivorSecondPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedFourthSurvivorThirdPerk;
        public SurvivorPerkDTO SelectedFourthSurvivorThirdPerk
        {
            get => _selectedFourthSurvivorThirdPerk;
            set
            {
                _selectedFourthSurvivorThirdPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedFourthSurvivorFourthPerk;
        public SurvivorPerkDTO SelectedFourthSurvivorFourthPerk
        {
            get => _selectedFourthSurvivorFourthPerk;
            set
            {
                _selectedFourthSurvivorFourthPerk = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выживший №4 - Предмет + аддоны : Collections | Selected

        public ObservableCollection<ItemAddonDTO> FourthSurvivorItemAddons { get; set; } = [];

        private ItemDTO _selectedFourthSurvivorItem;
        public ItemDTO SelectedFourthSurvivorItem
        {
            get => _selectedFourthSurvivorItem;
            set
            {
                _selectedFourthSurvivorItem = value;
                LoadFourthSurvivorItemAddon();
                OnPropertyChanged();
            }
        }

        private ItemAddonDTO _selectedFourthSurvivorFirstItemAddon;
        public ItemAddonDTO SelectedFourthSurvivorFirstItemAddon
        {
            get => _selectedFourthSurvivorFirstItemAddon;
            set
            {
                _selectedFourthSurvivorFirstItemAddon = value;
                OnPropertyChanged();
            }
        }

        private ItemAddonDTO _selectedFourthSurvivorSecondItemAddon;
        public ItemAddonDTO SelectedFourthSurvivorSecondItemAddon
        {
            get => _selectedFourthSurvivorSecondItemAddon;
            set
            {
                _selectedFourthSurvivorSecondItemAddon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выживший №4 - Подношение : Collection | Selected

        public ObservableCollection<OfferingDTO> FourthSurvivorOfferings { get; private set; } = [];

        private OfferingDTO _selectedFourthSurvivorOffering;
        public OfferingDTO SelectedFourthSurvivorOffering
        {
            get => _selectedFourthSurvivorOffering;
            set
            {
                _selectedFourthSurvivorOffering = value;
                OnPropertyChanged();
            }
        }

        private RoleDTO _selectedRoleFourthSurvivor;
        public RoleDTO SelectedRoleFourthSurvivor
        {
            get => _selectedRoleFourthSurvivor;
            set
            {
                _selectedRoleFourthSurvivor = value;
                LoadFourthSurvivorOffering();
                OnPropertyChanged();
            }
        }

        #endregion


        #region Итог игры : Collections | Selected

        public ObservableCollection<PatchDTO> Patches { get; private set; } = [];

        public ObservableCollection<GameModeDTO> GameModes { get; private set; } = [];

        public ObservableCollection<GameEventDTO> GameEvents { get; private set; } = [];

        public ObservableCollection<MapDTO> Maps { get; private set; } = [];

        public ObservableCollection<WhoPlacedMapDTO> WhoPlacedMaps { get; private set; } = [];

        public ObservableCollection<MatchAttributeDTO> MatchAttributes { get; private set; } = [];

        private PatchDTO _selectedPatch;
        public PatchDTO SelectedPatch
        {
            get => _selectedPatch;
            set
            {
                _selectedPatch = value;
                OnPropertyChanged();
            }
        }

        private GameModeDTO _selectedGameMode;
        public GameModeDTO SelectedGameMode
        {
            get => _selectedGameMode;
            set
            {
                _selectedGameMode = value;
                OnPropertyChanged();
            }
        }

        private GameEventDTO _selectedGameEvent;
        public GameEventDTO SelectedGameEvent
        {
            get => _selectedGameEvent;
            set
            {
                _selectedGameEvent = value;
                OnPropertyChanged();
            }
        }

        private MapDTO _selectedMap;
        public MapDTO SelectedMap
        {
            get => _selectedMap;
            set
            {
                _selectedMap = value;
                OnPropertyChanged();
            }
        }

        private WhoPlacedMapDTO _selectedWhoPlacedMap;
        public WhoPlacedMapDTO SelectedWhoPlacedMap
        {
            get => _selectedWhoPlacedMap;
            set
            {
                _selectedWhoPlacedMap = value;
                OnPropertyChanged();
            }
        }

        private WhoPlacedMapDTO _selectedWhoPlacedMapWin;
        public WhoPlacedMapDTO SelectedWhoPlacedMapWin
        {
            get => _selectedWhoPlacedMapWin;
            set
            {
                _selectedWhoPlacedMapWin = value;
                OnPropertyChanged();
            }
        }
        
        private MatchAttributeDTO _selectedMatchAttribute;
        public MatchAttributeDTO SelectedMatchAttribute
        {
            get => _selectedMatchAttribute;
            set
            {
                _selectedMatchAttribute = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedDateTimeGameMatch = DateTime.Now;
        public DateTime SelectedDateTimeGameMatch
        {
            get => _selectedDateTimeGameMatch;
            set
            {
                _selectedDateTimeGameMatch = value;
                OnPropertyChanged();
            }
        }

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

        private int _сountKills;
        public int CountKills
        {
            get => _сountKills;
            set
            {
                if (value > 4 | value < 0)
                {
                    // TODO : Заменить на кастомное окно
                    MessageBox.Show("Нельзя сделать больше 4 и меньше 0 убийств");
                }
                else
                {
                    _сountKills = value;
                    OnPropertyChanged();
                }

            }
        }

        private int _countHooks;
        public int CountHooks
        {
            get => _countHooks;
            set
            {
                if (value > 12 | value < 0)
                {
                    // TODO : Заменить на кастомное окно
                    MessageBox.Show("Нельзя сделать больше 12 и меньше 0 подвесов");
                }
                else
                {
                    _countHooks = value;
                    OnPropertyChanged();
                }

            }
        }

        private int _countNumberRecentGenerators;
        public int CountNumberRecentGenerators
        {
            get => _countNumberRecentGenerators;
            set
            {
                if (value > 5 | value < 0)
                {
                    // TODO : Заменить на кастомное окно
                    MessageBox.Show("В игре не может быть больше 5 и меньше 0 генераторов");
                }
                else
                {
                    _countNumberRecentGenerators = value;
                    OnPropertyChanged();
                }

            }
        }

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

        #region Изображение для сбора данных
        
        private byte[] ResultFullMatchImage;

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

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Выбор перков убийцы 

        private RelayCommand _selectedKillerFirstPerkCommand;
        public RelayCommand SelectedKillerFirstPerkCommand { get => _selectedKillerFirstPerkCommand ??= new(obj => { SelectedKillerFirstPerk = SelectedKillerPerk; }); }

        private RelayCommand _selectedKillerSecondPerkCommand;
        public RelayCommand SelectedKillerSecondPerkCommand { get => _selectedKillerSecondPerkCommand ??= new(obj => { SelectedKillerSecondPerk = SelectedKillerPerk; }); }

        private RelayCommand _selectedKillerThirdPerkCommand;
        public RelayCommand SelectedKillerThirdPerkCommand { get => _selectedKillerThirdPerkCommand ??= new(obj => { SelectedKillerThirdPerk = SelectedKillerPerk; }); }

        private RelayCommand _selectedKillerFourthPerkCommand;
        public RelayCommand SelectedKillerFourthPerkCommand { get => _selectedKillerFourthPerkCommand ??= new(obj => { SelectedKillerFourthPerk = SelectedKillerPerk; }); }

        #endregion

        #region Выбор аддонов убийцы 

        private RelayCommand _selectedKillerFirstAddonCommand;
        public RelayCommand SelectedKillerFirstAddonCommand { get => _selectedKillerFirstAddonCommand ??= new(obj => { SelectedKillerFirstAddon = SelectedKillerAddon; }); }

        private RelayCommand _selectedKillerSecondAddonCommand;
        public RelayCommand SelectedKillerSecondAddonCommand { get => _selectedKillerSecondAddonCommand ??= new(obj => { SelectedKillerSecondAddon = SelectedKillerAddon; }); }

        #endregion


        #region Выбор перков Выжившего №1

        private RelayCommand _selectedFirstSurvivorFirstPerkCommand;
        public RelayCommand SelectedFirstSurvivorFirstPerkCommand { get => _selectedFirstSurvivorFirstPerkCommand ??= new(obj => { SelectedFirstSurvivorFirstPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedFirstSurvivorSecondPerkCommand;
        public RelayCommand SelectedFirstSurvivorSecondPerkCommand { get => _selectedFirstSurvivorSecondPerkCommand ??= new(obj => { SelectedFirstSurvivorSecondPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedFirstSurvivorThirdPerkCommand;
        public RelayCommand SelectedFirstSurvivorThirdPerkCommand { get => _selectedFirstSurvivorThirdPerkCommand ??= new(obj => { SelectedFirstSurvivorThirdPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedFirstSurvivorFourthPerkCommand;
        public RelayCommand SelectedFirstSurvivorFourthPerkCommand { get => _selectedFirstSurvivorFourthPerkCommand ??= new(obj => { SelectedFirstSurvivorFourthPerk = SelectedSurvivorPerk; }); }

        #endregion

        #region Выбор улучшений Выжившего №1

        private RelayCommand _selectedFirstSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedFirstSurvivorFirstItemAddonCommand { get => _selectedFirstSurvivorFirstItemAddonCommand ??= new(obj => { SelectedFirstSurvivorFirstItemAddon = SelectedItemAddon; }); }

        private RelayCommand _selectedFirstSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedFirstSurvivorSecondItemAddonCommand { get => _selectedFirstSurvivorSecondItemAddonCommand ??= new(obj => { SelectedFirstSurvivorSecondItemAddon = SelectedItemAddon; }); }

        #endregion

        #region Выбор подношение Выжившего №1

        private RelayCommand _selectedFirstSurvivorOfferingCommand;
        public RelayCommand SelectedFirstSurvivorOfferingCommand { get => _selectedFirstSurvivorOfferingCommand ??= new(obj => { SelectedFirstSurvivorOffering = SelectedSurvivorOffering; }); }

        #endregion


        #region Выбор перков Выжившего №2

        private RelayCommand _selectedSecondSurvivorFirstPerkCommand;
        public RelayCommand SelectedSecondSurvivorFirstPerkCommand { get => _selectedSecondSurvivorFirstPerkCommand ??= new(obj => { SelectedSecondSurvivorFirstPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedSecondSurvivorSecondPerkCommand;
        public RelayCommand SelectedSecondSurvivorSecondPerkCommand { get => _selectedSecondSurvivorSecondPerkCommand ??= new(obj => { SelectedSecondSurvivorSecondPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedSecondSurvivorThirdPerkCommand;
        public RelayCommand SelectedSecondSurvivorThirdPerkCommand { get => _selectedSecondSurvivorThirdPerkCommand ??= new(obj => { SelectedSecondSurvivorThirdPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedSecondSurvivorFourthPerkCommand;
        public RelayCommand SelectedSecondSurvivorFourthPerkCommand { get => _selectedSecondSurvivorFourthPerkCommand ??= new(obj => { SelectedSecondSurvivorFourthPerk = SelectedSurvivorPerk; }); }

        #endregion

        #region Выбор улучшений Выжившего №2

        private RelayCommand _selectedSecondSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedSecondSurvivorFirstItemAddonCommand { get => _selectedSecondSurvivorFirstItemAddonCommand ??= new(obj => { SelectedSecondSurvivorFirstItemAddon = SelectedItemAddon; }); }

        private RelayCommand _selectedSecondSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedSecondSurvivorSecondItemAddonCommand { get => _selectedSecondSurvivorSecondItemAddonCommand ??= new(obj => { SelectedSecondSurvivorSecondItemAddon = SelectedItemAddon; }); }

        #endregion

        #region Выбор подношение Выжившего №2

        private RelayCommand _selectedSecondSurvivorOfferingCommand;
        public RelayCommand SelectedSecondSurvivorOfferingCommand { get => _selectedSecondSurvivorOfferingCommand ??= new(obj => { SelectedSecondSurvivorOffering = SelectedSurvivorOffering; }); }

        #endregion


        #region Выбор перков Выжившего №3

        private RelayCommand _selectedThirdSurvivorFirstPerkCommand;
        public RelayCommand SelectedThirdSurvivorFirstPerkCommand { get => _selectedThirdSurvivorFirstPerkCommand ??= new(obj => { SelectedThirdSurvivorFirstPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedThirdSurvivorSecondPerkCommand;
        public RelayCommand SelectedThirdSurvivorSecondPerkCommand { get => _selectedThirdSurvivorSecondPerkCommand ??= new(obj => { SelectedThirdSurvivorSecondPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedThirdSurvivorThirdPerkCommand;
        public RelayCommand SelectedThirdSurvivorThirdPerkCommand { get => _selectedThirdSurvivorThirdPerkCommand ??= new(obj => { SelectedThirdSurvivorThirdPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedThirdSurvivorFourthPerkCommand;
        public RelayCommand SelectedThirdSurvivorFourthPerkCommand { get => _selectedThirdSurvivorFourthPerkCommand ??= new(obj => { SelectedThirdSurvivorFourthPerk = SelectedSurvivorPerk; }); }

        #endregion

        #region Выбор улучшений Выжившего №3

        private RelayCommand _selectedThirdSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedThirdSurvivorFirstItemAddonCommand { get => _selectedThirdSurvivorFirstItemAddonCommand ??= new(obj => { SelectedThirdSurvivorFirstItemAddon = SelectedItemAddon; }); }

        private RelayCommand _selectedThirdSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedThirdSurvivorSecondItemAddonCommand { get => _selectedThirdSurvivorSecondItemAddonCommand ??= new(obj => { SelectedThirdSurvivorSecondItemAddon = SelectedItemAddon; }); }

        #endregion       

        #region Выбор подношение Выжившего №3

        private RelayCommand _selectedThirdSurvivorOfferingCommand;
        public RelayCommand SelectedThirdSurvivorOfferingCommand { get => _selectedThirdSurvivorOfferingCommand ??= new(obj => { SelectedThirdSurvivorOffering = SelectedSurvivorOffering; }); }

        #endregion  
        

        #region Выбор перков Выжившего №4

        private RelayCommand _selectedFourthSurvivorFirstPerkCommand;
        public RelayCommand SelectedFourthSurvivorFirstPerkCommand { get => _selectedFourthSurvivorFirstPerkCommand ??= new(obj => { SelectedFourthSurvivorFirstPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedFourthSurvivorSecondPerkCommand;
        public RelayCommand SelectedFourthSurvivorSecondPerkCommand { get => _selectedFourthSurvivorSecondPerkCommand ??= new(obj => { SelectedFourthSurvivorSecondPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedFourthSurvivorThirdPerkCommand;
        public RelayCommand SelectedFourthSurvivorThirdPerkCommand { get => _selectedFourthSurvivorThirdPerkCommand ??= new(obj => { SelectedFourthSurvivorThirdPerk = SelectedSurvivorPerk; }); }

        private RelayCommand _selectedFourthSurvivorFourthPerkCommand;
        public RelayCommand SelectedFourthSurvivorFourthPerkCommand { get => _selectedFourthSurvivorFourthPerkCommand ??= new(obj => { SelectedFourthSurvivorFourthPerk = SelectedSurvivorPerk; }); }

        #endregion

        #region Выбор улучшений Выжившего №4

        private RelayCommand _selectedFourthSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedFourthSurvivorFirstItemAddonCommand { get => _selectedFourthSurvivorFirstItemAddonCommand ??= new(obj => { SelectedFourthSurvivorFirstItemAddon = SelectedItemAddon; }); }

        private RelayCommand _selectedFourthSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedFourthSurvivorSecondItemAddonCommand { get => _selectedFourthSurvivorSecondItemAddonCommand ??= new(obj => { SelectedFourthSurvivorSecondItemAddon = SelectedItemAddon; }); }

        #endregion

        #region Выбор подношение Выжившего №4

        private RelayCommand _selectedFourthSurvivorOfferingCommand;
        public RelayCommand SelectedFourthSurvivorOfferingCommand { get => _selectedFourthSurvivorOfferingCommand ??= new(obj => { SelectedFourthSurvivorOffering = SelectedSurvivorOffering; }); }

        #endregion


        #region Изображения

        private RelayCommand _selectImagesCommand;
        public RelayCommand SelectImagesCommand
        {
            get => _selectImagesCommand ??= new( async obj =>
            {
                // TODO : Заменить на сервис
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (var file in openFileDialog.FileNames)
                    {
                        var imageByte = await ImageHelper.ImageToByteArrayAsync(file);

                        var imageInfo = new ImageInfo
                        {
                            PathImage = file,
                            ImageByte = imageByte
                        };
                        ImagePaths.Add(imageInfo);
                    }                
                }
            });
        }

        private RelayCommand _loadResultMatchImageCommand;
        public RelayCommand LoadResultMatchImageCommand
        {
            get => _loadResultMatchImageCommand ??= new( obj =>
            {
                var file = new FileInfo(SelectedImagePath.PathImage);

                CropResultImage();
                SetEndTime(file);
                SetDateTimeMatch(file);
                SubstactTime();
                ResultFullMatchImage = ImageHelper.ImageToByteArray(SelectedImagePath.PathImage);
            });
        }

        private RelayCommand _loadStartMatchImageCommand;
        public RelayCommand LoadStartMatchImageCommand
        {
            get => _loadStartMatchImageCommand ??= new(obj =>
            {
                CropStartMatchImage();
                SetStartTime(new FileInfo(SelectedImagePath.PathImage));
                SubstactTime();
            });
        }

        private RelayCommand _loadEndMatchImageCommand;
        public RelayCommand LoadEndMatchImageCommand { get => _loadEndMatchImageCommand ??= new(obj => { CropEndMatchImage(); }); }

        private RelayCommand _clearImageListCommand;
        public RelayCommand ClearImageListCommand 
        { 
            get => _clearImageListCommand ??= new(obj => 
            { 
                ImagePaths.Clear(); 
                SetNullImage();
            }); 
        }

        private RelayCommand _deleteSelectedImageCommand;
        public RelayCommand DeleteSelectedImageCommand { get => _deleteSelectedImageCommand ??= new(obj => { DeleteSelectImage(); }); }

        #endregion


        #region Открытие окон для добавление зависимых данных

        private RelayCommand _openKillerWindowCommand;
        public RelayCommand OpenKillerWindowCommand { get => _openKillerWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionKiller, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openPlatformWindowCommand;
        public RelayCommand OpenPlatformWindowCommand { get => _openPlatformWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionPlatform, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openTypeDeathWindowCommand;
        public RelayCommand OpenTypeDeathWindowCommand { get => _openTypeDeathWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionTypeDeath, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openMeasurementWindowCommand;
        public RelayCommand OpenMeasurementWindowCommand { get => _openMeasurementWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionMeasurement, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openPatchWindowCommand;
        public RelayCommand OpenPatchWindowCommand { get => _openPatchWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionPatch, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openGameModeWindowCommand;
        public RelayCommand OpenGameModeWindowCommand { get => _openGameModeWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionGameMode, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openGameEventWindowCommand;
        public RelayCommand OpenGameEventWindowCommand { get => _openGameEventWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionGameEvent, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openWhoPlacedMapWindowCommand;
        public RelayCommand OpenWhoPlacedMapWindowCommand { get => _openWhoPlacedMapWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionWhoPlacedMap, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openSurvivorWindowCommand;
        public RelayCommand OpenSurvivorWindowCommand { get => _openSurvivorWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionSurvivor, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openItemWindowCommand;
        public RelayCommand OpenItemWindowCommand { get => _openItemWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionItem, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openOfferingWindowCommand;
        public RelayCommand OpenOfferingWindowCommand { get => _openOfferingWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionOffering, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openPlayerAssociationWindowCommand;
        public RelayCommand OpenPlayerAssociationWindowCommand { get => _openPlayerAssociationWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionAssociation, parameter: null, TypeParameter.None, true); }); }

        private RelayCommand _openRoleWindowCommand;
        public RelayCommand OpenRoleWindowCommand { get => _openRoleWindowCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionRole, parameter: null, TypeParameter.None, true); }); }

        #endregion

        #region Добавление матча в БД

        private RelayCommand _addMatchCommand;
        public RelayCommand AddMatchCommand { get => _addMatchCommand ??= new(obj => { AddGameStatistic(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private void GetData()
        {
            GetPlatforms();
            GetTypeDeaths();
            GetPlayerAssociations();
            GetOffering();
            GetRoles();
            GetMatchAttributes();
           
            GetSurvivors();
            GetItemAddons();
            GetItems();
            GetSurvivorPerks();

            GetKillerAddons();
            GetKillers();
            GetKillerPerks();

            LoadKillerRoles();
            LoadKillerPlayerAssociations();
            LoadSurvivorRoles();
            LoadSurvivorPlayerAssociations();

            GetMaps();
            GetPatches();
            GetGameModes();
            GetGameEvents();
            GetWhoPlacedMaps();
        }

        #region Общие : Получение данных | Заполнение списков

        private void GetPlatforms()
        {
            var platforms = _platformService.GetAll();
            foreach (var item in platforms)
            {
                Platforms.Add(item);
            }

            SelectedKillerPlatform = Platforms.FirstOrDefault();
            
            SelectedFirstSurvivorPlatform = Platforms.FirstOrDefault();
            SelectedSecondSurvivorPlatform = Platforms.FirstOrDefault();
            SelectedThirdSurvivorPlatform = Platforms.FirstOrDefault();
            SelectedFourthSurvivorPlatform = Platforms.FirstOrDefault();
        }

        private void GetRoles() => _roles = _roleService.GetAll();

        private void GetPlayerAssociations() => _playerAssociations = _associationService.GetAll();

        private void LoadKillerPlayerAssociations()
        {
            foreach (var item in _playerAssociations.Where(x => x.IdPlayerAssociation != 2 && x.IdPlayerAssociation != 4))
            {
                KillerPlayerAssociations.Add(item);
            }

            SelectedKillerPlayerAssociation = KillerPlayerAssociations.FirstOrDefault();
        }

        private void LoadSurvivorPlayerAssociations()
        {
            foreach (var item in _playerAssociations)
            {
                SurvivorPlayerAssociations.Add(item);
            }

            SelectedFirstSurvivorPlayerAssociation = SurvivorPlayerAssociations[2];
            SelectedSecondSurvivorPlayerAssociation = SurvivorPlayerAssociations[2];
            SelectedThirdSurvivorPlayerAssociation = SurvivorPlayerAssociations[2];
            SelectedFourthSurvivorPlayerAssociation = SurvivorPlayerAssociations[2];
        }

        #endregion

        /*--Киллеры--*/

        #region Киллеры : Получение данных 

        private void GetKillers()
        {
            var killers = _killerService.GetAll();
            foreach (var item in killers.Skip(1))
            {
                Killers.Add(item);
            }

            SelectedKiller = Killers.FirstOrDefault();
        }

        private void GetKillerAddons() => _killerAddons = _killerAddonService.GetAll();

        private void GetKillerPerks()
        {
            _killerPerks.Clear();
            _killerPerks = _killerPerkService.GetAll().OrderBy(x => x.PerkName).ToList();

            foreach (var item in _killerPerks)
            {
                KillerPerks.Add(item);
            }

            var emptyPerk = KillerPerks.FirstOrDefault(x => x.IdKillerPerk == 119);

            SelectedKillerFirstPerk = emptyPerk;
            SelectedKillerSecondPerk = emptyPerk;
            SelectedKillerThirdPerk = emptyPerk;
            SelectedKillerFourthPerk = emptyPerk;
        }

        private void GetOffering() => _offerings = _offeringService.GetAll();

        #endregion

        #region Киллеры : Заполнение списков

        private void LoadKillerAddons()
        {
            KillerAddons.Clear();
            foreach (var item in _killerAddons.Where(x => x.IdKiller == SelectedKiller.IdKiller).OrderBy(x => x.IdRarity))
            {
                KillerAddons.Add(item);
            }

            var emptyAddon = KillerAddons.Where(x => x.IdKiller == SelectedKiller.IdKiller).OrderByDescending(x => x.IdRarity).FirstOrDefault();

            SelectedKillerFirstAddon = emptyAddon;
            SelectedKillerSecondAddon = emptyAddon;
        }

        private void LoadKillerRoles()
        {
            KillerRoles.Clear();
            foreach (var item in _roles.Where(x => x.IdRole != 3))
            {
                KillerRoles.Add(item);
            }

            SelectedKillerRole = KillerRoles.FirstOrDefault();
        }

        private void LoadKillerOffering()
        {
            KillerOfferings.Clear();
            foreach (var item in _offerings.Where(x => x.IdRole == SelectedKillerRole.IdRole).OrderBy(x => x.IdRarity).ThenBy(x => x.OfferingName))
            {
                KillerOfferings.Add(item);
            }

            SelectedKillerOffering = KillerOfferings.LastOrDefault();
        }

        #endregion

        /*--Выжившие--*/

        #region Выжившие - Общие : Получение данных | Заполнение

        private void GetSurvivors()
        {
            var survivors = _survivorService.GetAll();

            foreach (var item in survivors.Skip(1))
            {
                Survivors.Add(item);
            }

            SelectedFirstSurvivor = Survivors.FirstOrDefault();
            SelectedSecondSurvivor = Survivors.FirstOrDefault();
            SelectedThirdSurvivor = Survivors.FirstOrDefault();
            SelectedFourthSurvivor = Survivors.FirstOrDefault();
        }

        private void GetSurvivorPerks()
        {
            _survivorPerks = _survivorPerkService.GetAll().OrderBy(x => x.PerkName).ToList();

            foreach (var item in _survivorPerks)
            {
                SurvivorPerks.Add(item);
            }

            var emptyPerk = SurvivorPerks.FirstOrDefault(x => x.IdSurvivorPerk == 138);

            SelectedFirstSurvivorFirstPerk = emptyPerk;
            SelectedFirstSurvivorSecondPerk = emptyPerk;
            SelectedFirstSurvivorThirdPerk = emptyPerk;
            SelectedFirstSurvivorFourthPerk = emptyPerk;  
            
            SelectedSecondSurvivorFirstPerk = emptyPerk;
            SelectedSecondSurvivorSecondPerk = emptyPerk;
            SelectedSecondSurvivorThirdPerk = emptyPerk;
            SelectedSecondSurvivorFourthPerk = emptyPerk;

            SelectedThirdSurvivorFirstPerk = emptyPerk;
            SelectedThirdSurvivorSecondPerk = emptyPerk;
            SelectedThirdSurvivorThirdPerk = emptyPerk;
            SelectedThirdSurvivorFourthPerk = emptyPerk;

            SelectedFourthSurvivorFirstPerk = emptyPerk;
            SelectedFourthSurvivorSecondPerk = emptyPerk;
            SelectedFourthSurvivorThirdPerk = emptyPerk;
            SelectedFourthSurvivorFourthPerk = emptyPerk;
        }

        private void GetTypeDeaths()
        {
            var typeDeaths = _typeDeathService.GetAll();

            foreach (var item in typeDeaths)
            {
                TypeDeaths.Add(item);
            }

            SelectedFirstSurvivorTypeDeath = TypeDeaths.FirstOrDefault();
            SelectedSecondSurvivorTypeDeath = TypeDeaths.FirstOrDefault();
            SelectedThirdSurvivorTypeDeath = TypeDeaths.FirstOrDefault();
            SelectedFourthSurvivorTypeDeath = TypeDeaths.FirstOrDefault();
        }

        private void GetItems()
        {
            var items = _itemService.GetAll();

            foreach (var item in items)
            {
                Items.Add(item);
            }

            SelectedFirstSurvivorItem = Items.LastOrDefault();
            SelectedSecondSurvivorItem = Items.LastOrDefault();
            SelectedThirdSurvivorItem = Items.LastOrDefault();
            SelectedFourthSurvivorItem = Items.LastOrDefault();
        }

        private void GetItemAddons() => _itemAddons = _itemAddonService.GetAll();

        #endregion

        #region Выжившие : Заполнение списков

        private void LoadSurvivorRoles()
        {
            SurvivorRoles.Clear();
            foreach (var item in _roles.Where(x => x.IdRole != 2))
            {
                SurvivorRoles.Add(item);
            }

            SelectedRoleFirstSurvivor = SurvivorRoles.FirstOrDefault();
            SelectedRoleSecondSurvivor = SurvivorRoles.FirstOrDefault();
            SelectedRoleThirdSurvivor = SurvivorRoles.FirstOrDefault();
            SelectedRoleFourthSurvivor = SurvivorRoles.FirstOrDefault();
        }

        #endregion

        #region Выживший №1 : Заполнение списков

        private void LoadFirstSurvivorItemAddon()
        {
            FirstSurvivorItemAddons.Clear();
            foreach (var item in _itemAddons.Where(x => x.IdItem == SelectedFirstSurvivorItem.IdItem))
            {
                FirstSurvivorItemAddons.Add(item);
            }

            SelectedFirstSurvivorFirstItemAddon = FirstSurvivorItemAddons.LastOrDefault();
            SelectedFirstSurvivorSecondItemAddon = FirstSurvivorItemAddons.LastOrDefault();
        }

        private void LoadFirstSurvivorOffering()
        {
            FirstSurvivorOfferings.Clear();
            foreach (var item in _offerings.Where(x => x.IdRole == SelectedRoleFirstSurvivor.IdRole).OrderBy(x => x.IdRarity).ThenBy(x => x.OfferingName))
            {
                FirstSurvivorOfferings.Add(item);
            }

            SelectedFirstSurvivorOffering = FirstSurvivorOfferings.LastOrDefault();
        }

        #endregion

        #region Выживший №2 : Заполнение списков

        private void LoadSecondSurvivorItemAddon()
        {
            SecondSurvivorItemAddons.Clear();
            foreach (var item in _itemAddons.Where(x => x.IdItem == SelectedSecondSurvivorItem.IdItem))
            {
                SecondSurvivorItemAddons.Add(item);
            }

            SelectedSecondSurvivorFirstItemAddon = SecondSurvivorItemAddons.LastOrDefault();
            SelectedSecondSurvivorSecondItemAddon = SecondSurvivorItemAddons.LastOrDefault();
        }

        private void LoadSecondSurvivorOffering()
        {
            SecondSurvivorOfferings.Clear();
            foreach (var item in _offerings.Where(x => x.IdRole == SelectedRoleSecondSurvivor.IdRole).OrderBy(x => x.IdRarity).ThenBy(x => x.OfferingName))
            {
                SecondSurvivorOfferings.Add(item);
            }

            SelectedSecondSurvivorOffering = SecondSurvivorOfferings.LastOrDefault();
        }

        #endregion

        #region Выживший №3 : Заполнение списков

        private void LoadThirdSurvivorItemAddon()
        {
            ThirdSurvivorItemAddons.Clear();
            foreach (var item in _itemAddons.Where(x => x.IdItem == SelectedThirdSurvivorItem.IdItem))
            {
                ThirdSurvivorItemAddons.Add(item);
            }

            SelectedThirdSurvivorFirstItemAddon = ThirdSurvivorItemAddons.LastOrDefault();
            SelectedThirdSurvivorSecondItemAddon = ThirdSurvivorItemAddons.LastOrDefault();
        }

        private void LoadThirdSurvivorOffering()
        {
            ThirdSurvivorOfferings.Clear();
            foreach (var item in _offerings.Where(x => x.IdRole == SelectedRoleThirdSurvivor.IdRole).OrderBy(x => x.IdRarity).ThenBy(x => x.OfferingName))
            {
                ThirdSurvivorOfferings.Add(item);
            }

            SelectedThirdSurvivorOffering = ThirdSurvivorOfferings.LastOrDefault();
        }

        #endregion

        #region Выживший №4 : Заполнение списков

        private void LoadFourthSurvivorItemAddon()
        {
            FourthSurvivorItemAddons.Clear();
            foreach (var item in _itemAddons.Where(x => x.IdItem == SelectedFourthSurvivorItem.IdItem))
            {
                FourthSurvivorItemAddons.Add(item);
            }

            SelectedFourthSurvivorFirstItemAddon = FourthSurvivorItemAddons.LastOrDefault();
            SelectedFourthSurvivorSecondItemAddon = FourthSurvivorItemAddons.LastOrDefault();
        }

        private void LoadFourthSurvivorOffering()
        {
            FourthSurvivorOfferings.Clear();
            foreach (var item in _offerings.Where(x => x.IdRole == SelectedRoleFourthSurvivor.IdRole).OrderBy(x => x.IdRarity).ThenBy(x => x.OfferingName))
            {
                FourthSurvivorOfferings.Add(item);
            }

            SelectedFourthSurvivorOffering = FourthSurvivorOfferings.LastOrDefault();
        }

        #endregion

        /*--Итог--*/

        #region Итоги игры : Получение | Заполнение данных

        private void GetMaps()
        {
            var maps = _mapService.GetAll();

            foreach (var item in maps)
            {
                Maps.Add(item);
            }
        }

        private void GetPatches()
        {
            var patches = _patchService.GetAll();

            foreach (var item in patches.OrderByDescending(x => x.IdPatch))
            {
                Patches.Add(item);
            }

            SelectedPatch = Patches.FirstOrDefault();
        }

        private async void GetGameModes()
        {
            var gameModes = await _gameModeService.GetAllAsync();

            foreach (var item in gameModes)
            {
                GameModes.Add(item);
            }

            SelectedGameMode = GameModes.FirstOrDefault();
        }  
        
        private void GetGameEvents()
        {
            var gameEvents = _gameEventService.GetAll();

            foreach (var item in gameEvents)
            {
                GameEvents.Add(item);
            }

            SelectedGameEvent = GameEvents.FirstOrDefault();
        }
        
        private void GetWhoPlacedMaps()
        {
            var whoPlacedMaps = _whoPlacedMapService.GetAll();

            foreach (var item in whoPlacedMaps)
            {
                WhoPlacedMaps.Add(item);
            }

            SelectedWhoPlacedMap = WhoPlacedMaps.FirstOrDefault();
            SelectedWhoPlacedMapWin = WhoPlacedMaps.FirstOrDefault();
        }

        private void GetMatchAttributes()
        {
            var matchAttributes = _matchAttributeService.GetAll(isHide: false);

            foreach (var item in matchAttributes)
            {
                MatchAttributes.Add(item);
            }

            SelectedMatchAttribute = MatchAttributes.FirstOrDefault();
        }

        #endregion

        #region Итоги игры : Расчет времени

        private void SubstactTime()
        {
            string startTimeString = StartTime.ToString();
            string endTimeString = EndTime.ToString();

            DateTime startTime = DateTime.Parse(startTimeString);
            DateTime endTime = DateTime.Parse(endTimeString);

            TimeSpan timePlayed = endTime - startTime;

            DurationMatch = $"{timePlayed}";
        }

        private void SetStartTime(FileInfo file) => StartTime = file.CreationTime;

        private void SetEndTime(FileInfo file) => EndTime = file.CreationTime;

        private void SetDateTimeMatch(FileInfo file) => SelectedDateTimeGameMatch = file.CreationTime;

        #endregion

        /*--Изображение матча--*/

        #region Обрезка выбираемых изображений матча

        private void CropResultImage()
        {
            ResultMatchKillerImage =         ImageHelper.GetBitmapImage(ImageHelper.CropImage(SelectedImagePath.PathImage, 90, 920, 920, 125));
            ResultMatchFirstSurvivorImage =  ImageHelper.GetBitmapImage(ImageHelper.CropImage(SelectedImagePath.PathImage, 90, 410, 920, 125));
            ResultMatchSecondSurvivorImage = ImageHelper.GetBitmapImage(ImageHelper.CropImage(SelectedImagePath.PathImage, 90, 530, 920, 125));
            ResultMatchThirdSurvivorImage =  ImageHelper.GetBitmapImage(ImageHelper.CropImage(SelectedImagePath.PathImage, 90, 665, 920, 125));
            ResultMatchFourthSurvivorImage = ImageHelper.GetBitmapImage(ImageHelper.CropImage(SelectedImagePath.PathImage, 90, 795, 920, 125));
            ResultMatchImage =               ImageHelper.GetBitmapImage(ImageHelper.CropImage(SelectedImagePath.PathImage, 90, 200, 980, 850));
        }

        private void CropStartMatchImage() => StartMatchImage = ImageHelper.GetBitmapImage(ImageHelper.CropImage(SelectedImagePath.PathImage, 0, 900, 1100, 500));

        private void CropEndMatchImage() => EndMatchImage = ImageHelper.GetBitmapImage(ImageHelper.CropImage(SelectedImagePath.PathImage, 0, 250, 600, 900));

        #endregion

        /*--Остальное--*/

        // TODO : Заменить MessageBox
        #region Добавление записи в БД

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
                MessageBox.Show("Вы заполнили не все данные киллера. Если у Киллера нету аддонов либо перков, то нужно выбрать пункт - Отсутствует.");
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
                MessageBox.Show("Вы заполнили не все данные первого выжившего. Если у Выжившего нету какого либо снаряжение, то нужно выбрать пункт - Отсутствует.");
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
                MessageBox.Show("Вы заполнили не все данные второго выжившего. Если у Выжившего нету какого либо снаряжение, то нужно выбрать пункт - Отсутствует.");
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
                MessageBox.Show("Вы заполнили не все данные третьего выжившего. Если у Выжившего нету какого либо снаряжение, то нужно выбрать пункт - Отсутствует.");
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
                MessageBox.Show("Вы заполнили не все данные четвертого выжившего. Если у Выжившего нету какого либо снаряжение, то нужно выбрать пункт - Отсутствует.");
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
                MessageBox.Show("Вы заполнили не все данные в категории - Игра.", "Ошибка заполнения");
                return;
            }
            if (SelectedMatchAttribute != null) { }
            else
            {
                MessageBox.Show("Вы не выбрали атрибут матча.", "Ошибка заполнения");
                return;
            }

            if (FirstSurvivorAccount != 0 &&
               SecondSurvivorAccount != 0 &&
               ThirdSurvivorAccount != 0 &&
               FourthSurvivorAccount != 0) { }
            else
            {
                if (MessageBox.Show("У какого то выжившего счет 0! Вы уверены?", "Ошибка заполнения", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }

            if (FirstSurvivorPrestige != 0 &&
                SecondSurvivorPrestige != 0 &&
                ThirdSurvivorPrestige != 0 &&
                FourthSurvivorPrestige != 0) { }
            else
            {
                if (MessageBox.Show("У какого то выжившего престиж 0! Вы уверены?", "Ошибка заполнения", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }

            if (KillerAccount != 0) { }
            else
            {
                if (MessageBox.Show("У киллера счет 0! Вы уверены?", "Ошибка заполнения", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }

            if (KillerPrestige != 0) { }
            else
            {
                if (MessageBox.Show("У киллера престиж 0! Вы уверены?", "Ошибка заполнения", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }

            int lastKillerInfoID = AddKillerInfo();

            if (lastKillerInfoID < 0)
                return;

            AddFirstSurvivorInfo();
            AddSecondSurvivorInfo();
            AddThirdSurvivorInfo();
            AddFourthSurvivorInfo();

            List<int> lastFourRecords = _survivorInfoService.GetLastNRecordsId(4).IdRecords;

            var (idGameStatistic, Message) = _gameStatisticService.Create(
                lastKillerInfoID,
                lastFourRecords[0],
                lastFourRecords[1],
                lastFourRecords[2],
                lastFourRecords[3],
                SelectedMap.IdMap,
                SelectedWhoPlacedMap.IdWhoPlacedMap,
                SelectedWhoPlacedMapWin.IdWhoPlacedMap,
                SelectedPatch.IdPatch,
                SelectedGameMode.IdGameMode,
                SelectedGameEvent.IdGameEvent,
                SelectedDateTimeGameMatch,
                DurationMatch,
                CountKills,
                CountHooks,
                CountNumberRecentGenerators,
                DescriptionGame,
                ResultFullMatchImage,
                SelectedMatchAttribute.IdMatchAttribute);

            var match = _gameStatisticService.Get(idGameStatistic);

            NotificationTransmittingValue(PageName.DashBoard, FrameName.MainFrame, match, TypeParameter.AddAndNotification);

            SetNullKillerData();
            SetNullSurvivorData();
            SetNullGameData();
            SetNullImage();
        }

        private int AddKillerInfo()
        {
            var (IdKillerInfo, Message) = _killerInfoService.Create(
                        SelectedKiller.IdKiller,
                        SelectedKillerFirstPerk.IdKillerPerk,
                        SelectedKillerSecondPerk.IdKillerPerk,
                        SelectedKillerThirdPerk.IdKillerPerk,
                        SelectedKillerFourthPerk.IdKillerPerk,
                        SelectedKillerFirstAddon.IdKillerAddon,
                        SelectedKillerSecondAddon.IdKillerAddon,
                        SelectedKillerPlayerAssociation.IdPlayerAssociation,
                        SelectedKillerPlatform.IdPlatform,
                        SelectedKillerOffering.IdOffering,
                        KillerPrestige,
                        KillerBot,
                        KillerAnonymousMode,
                        KillerAccount);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return -1;
            }

            return IdKillerInfo;
        }

        private void AddFirstSurvivorInfo()
        {
            _survivorInfoService.Create(
                SelectedFirstSurvivor.IdSurvivor,
                SelectedFirstSurvivorFirstPerk.IdSurvivorPerk,
                SelectedFirstSurvivorSecondPerk.IdSurvivorPerk,
                SelectedFirstSurvivorThirdPerk.IdSurvivorPerk,
                SelectedFirstSurvivorFourthPerk.IdSurvivorPerk,
                SelectedFirstSurvivorItem.IdItem,
                SelectedFirstSurvivorFirstItemAddon.IdItemAddon,
                SelectedFirstSurvivorSecondItemAddon.IdItemAddon,
                SelectedFirstSurvivorTypeDeath.IdTypeDeath,
                SelectedFirstSurvivorPlayerAssociation.IdPlayerAssociation,
                SelectedFirstSurvivorPlatform.IdPlatform,
                SelectedFirstSurvivorOffering.IdOffering,
                FirstSurvivorPrestige,
                FirstSurvivorBot,
                FirstSurvivorAnonymousMode,
                FirstSurvivorAccount);
        }

        private void AddSecondSurvivorInfo()
        {
            _survivorInfoService.Create(
                SelectedSecondSurvivor.IdSurvivor,
                SelectedSecondSurvivorFirstPerk.IdSurvivorPerk,
                SelectedSecondSurvivorSecondPerk.IdSurvivorPerk,
                SelectedSecondSurvivorThirdPerk.IdSurvivorPerk,
                SelectedSecondSurvivorFourthPerk.IdSurvivorPerk,
                SelectedSecondSurvivorItem.IdItem,
                SelectedSecondSurvivorFirstItemAddon.IdItemAddon,
                SelectedSecondSurvivorSecondItemAddon.IdItemAddon,
                SelectedSecondSurvivorTypeDeath.IdTypeDeath,
                SelectedSecondSurvivorPlayerAssociation.IdPlayerAssociation,
                SelectedSecondSurvivorPlatform.IdPlatform,
                SelectedSecondSurvivorOffering.IdOffering,
                SecondSurvivorPrestige,
                SecondSurvivorBot,
                SecondSurvivorAnonymousMode,
                SecondSurvivorAccount);
        }

        private void AddThirdSurvivorInfo()
        {
            _survivorInfoService.Create(
                SelectedThirdSurvivor.IdSurvivor,
                SelectedThirdSurvivorFirstPerk.IdSurvivorPerk,
                SelectedThirdSurvivorSecondPerk.IdSurvivorPerk,
                SelectedThirdSurvivorThirdPerk.IdSurvivorPerk,
                SelectedThirdSurvivorFourthPerk.IdSurvivorPerk,
                SelectedThirdSurvivorItem.IdItem,
                SelectedThirdSurvivorFirstItemAddon.IdItemAddon,
                SelectedThirdSurvivorSecondItemAddon.IdItemAddon,
                SelectedThirdSurvivorTypeDeath.IdTypeDeath,
                SelectedThirdSurvivorPlayerAssociation.IdPlayerAssociation,
                SelectedThirdSurvivorPlatform.IdPlatform,
                SelectedThirdSurvivorOffering.IdOffering,
                ThirdSurvivorPrestige,
                ThirdSurvivorBot,
                ThirdSurvivorAnonymousMode,
                ThirdSurvivorAccount);
        }

        private void AddFourthSurvivorInfo()
        {
             _survivorInfoService.Create(
                SelectedFourthSurvivor.IdSurvivor,
                SelectedFourthSurvivorFirstPerk.IdSurvivorPerk,
                SelectedFourthSurvivorSecondPerk.IdSurvivorPerk,
                SelectedFourthSurvivorThirdPerk.IdSurvivorPerk,
                SelectedFourthSurvivorFourthPerk.IdSurvivorPerk,
                SelectedFourthSurvivorItem.IdItem,
                SelectedFourthSurvivorFirstItemAddon.IdItemAddon,
                SelectedFourthSurvivorSecondItemAddon.IdItemAddon,
                SelectedFourthSurvivorTypeDeath.IdTypeDeath,
                SelectedFourthSurvivorPlayerAssociation.IdPlayerAssociation,
                SelectedFourthSurvivorPlatform.IdPlatform,
                SelectedFourthSurvivorOffering.IdOffering,
                FourthSurvivorPrestige,
                FourthSurvivorBot,
                FourthSurvivorAnonymousMode,
                FourthSurvivorAccount);
        }

        #endregion

        #region Поиск

        private void SearchKillerPerk()
        {
            KillerPerks.Clear();

            var killer = Killers.FirstOrDefault(x => x.KillerName.ToLower().Contains(SearchKillerPerkInput.ToLower()));
            var killerId = killer != null ? killer.IdKiller : -1;

            foreach (var item in _killerPerks.Where(x => x.PerkName.ToLower().Contains(SearchKillerPerkInput.ToLower()) ||
                                                         x.IdKiller == killerId))
            {
                KillerPerks.Add(item);
            }
        }

        private void SearchSurvivorPerk()
        {
            SurvivorPerks.Clear();

            var survivor = Survivors.FirstOrDefault(x => x.SurvivorName.ToLower().Contains(SearchSurvivorPerkInput.ToLower()));
            var survivorId = survivor != null ? survivor.IdSurvivor : -1;

            foreach (var item in _survivorPerks.Where(x => x.PerkName.ToLower().Contains(SearchSurvivorPerkInput.ToLower()) ||
                                                         x.IdSurvivor == survivorId))
            {
                SurvivorPerks.Add(item);
            }
        }

        #endregion 

        #region Установка дефолтных значение

        private void SetNullKillerData()
        {
            SelectedKillerPlatform = Platforms.FirstOrDefault();

            SelectedKillerRole = KillerRoles.FirstOrDefault();

            KillerAnonymousMode = false;
            KillerBot = false;
            KillerPrestige = 0;
            KillerAccount = 0;

            SearchKillerPerkInput = string.Empty;
            var emptyPerk = KillerPerks.FirstOrDefault(x => x.IdKillerPerk == 119);

            SelectedKillerFirstPerk = emptyPerk;
            SelectedKillerSecondPerk = emptyPerk;
            SelectedKillerThirdPerk = emptyPerk;
            SelectedKillerFourthPerk = emptyPerk;

            var emptyAddon = KillerAddons.Where(x => x.IdKiller == SelectedKiller.IdKiller).OrderByDescending(x => x.IdRarity).FirstOrDefault();

            SelectedKillerFirstAddon = emptyAddon;
            SelectedKillerSecondAddon = emptyAddon;
        }

        private void SetNullSurvivorData()
        {
            /* Выбор платформы, на которой играют выжившие на Steam */

            PlatformDTO steam = Platforms.FirstOrDefault();

            SelectedFirstSurvivorPlatform = steam;
            SelectedSecondSurvivorPlatform = steam;
            SelectedThirdSurvivorPlatform = steam;
            SelectedFourthSurvivorPlatform = steam;

            /* Выбор типа смерти игрока в матче в "От крюка" */

            TypeDeathDTO onHock = TypeDeaths.FirstOrDefault();

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

            SearchSurvivorPerkInput = string.Empty;
            var emptyPerk = SurvivorPerks.FirstOrDefault(x => x.IdSurvivorPerk == 138);

            /* Сброс первого перка игроков на выживших */
            SelectedFirstSurvivorFirstPerk = emptyPerk;
            SelectedFirstSurvivorSecondPerk = emptyPerk;
            SelectedFirstSurvivorThirdPerk = emptyPerk;
            SelectedFirstSurvivorFourthPerk = emptyPerk;

            /* Сброс второго перка игроков на выживших */
            SelectedSecondSurvivorFirstPerk = emptyPerk;
            SelectedSecondSurvivorSecondPerk = emptyPerk;
            SelectedSecondSurvivorThirdPerk = emptyPerk;
            SelectedSecondSurvivorFourthPerk = emptyPerk;

            /* Сброс третьего перка игроков на выживших */
            SelectedThirdSurvivorFirstPerk = emptyPerk;
            SelectedThirdSurvivorSecondPerk = emptyPerk;
            SelectedThirdSurvivorThirdPerk = emptyPerk;
            SelectedThirdSurvivorFourthPerk = emptyPerk;

            /* Сброс четвертого перка игроков на выживших */
            SelectedFourthSurvivorFirstPerk = emptyPerk;
            SelectedFourthSurvivorSecondPerk = emptyPerk;
            SelectedFourthSurvivorThirdPerk = emptyPerk;
            SelectedFourthSurvivorFourthPerk = emptyPerk;

            /* Выбор предмета по умолчанию у выживших */

            ItemDTO emptyItem = Items.FirstOrDefault(x => x.ItemName == "Отсутствует");

            SelectedFirstSurvivorItem = emptyItem;
            SelectedSecondSurvivorItem = emptyItem;
            SelectedThirdSurvivorItem = emptyItem;
            SelectedFourthSurvivorItem = emptyItem;

            /* Сброс подношений игроков на выживших */

            RoleDTO firstRole = SurvivorRoles.FirstOrDefault();

            SelectedRoleFirstSurvivor = firstRole;
            SelectedRoleSecondSurvivor = firstRole;
            SelectedRoleThirdSurvivor = firstRole;
            SelectedRoleFourthSurvivor = firstRole;
        }

        private void SetNullGameData()
        {
            WhoPlacedMapDTO whoPlacedMap = WhoPlacedMaps.FirstOrDefault();
            CheckKills();
            CountHooks = 0;
            SelectedMap = null;
            CountNumberRecentGenerators = 0;
            SelectedWhoPlacedMap = whoPlacedMap;
            SelectedWhoPlacedMapWin = whoPlacedMap;
            DescriptionGame = string.Empty;
            SelectedMatchAttribute = MatchAttributes.FirstOrDefault();
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

        #endregion

        #region Удаление выбранного изображение

        private void DeleteSelectImage()
        {
            // TODO : Заменить месседж бокс.
            if (MessageBox.Show("Вы точно хотите удалить изображение с устройства?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (File.Exists(SelectedImagePath.PathImage))
                {
                    var path = SelectedImagePath.PathImage;
                    ImagePaths.Remove(SelectedImagePath);
                    File.Delete(path);

                    ResultMatchFirstSurvivorImage = null;
                    ResultMatchSecondSurvivorImage = null;
                    ResultMatchThirdSurvivorImage = null;
                    ResultMatchFourthSurvivorImage = null;

                    ResultMatchImage = null;
                    StartMatchImage = null;
                    EndMatchImage = null;
                }
            }
        }

        #endregion

        #region Проверки

        private static bool CheckPrestige(int number)
        {
            if (number > 100 | number < 0) return false;
            else return true;
        }

        private void CheckKills()
        {
            if (SelectedFirstSurvivorTypeDeath != null || SelectedSecondSurvivorTypeDeath != null || SelectedThirdSurvivorTypeDeath != null || SelectedFourthSurvivorTypeDeath == null)
            {
                TypeDeathDTO Escaped = TypeDeaths.FirstOrDefault(x => x.IdTypeDeath == 5);

                var selectedTypes = new List<TypeDeathDTO>()
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

    }
}