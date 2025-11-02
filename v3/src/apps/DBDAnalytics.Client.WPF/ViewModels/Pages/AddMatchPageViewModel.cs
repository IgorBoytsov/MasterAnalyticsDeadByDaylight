using DBDAnalytics.CatalogService.Client.ApiClients.Characters.Killer;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.Survivor;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Item;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Offering;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameEvent;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameMode;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Associations;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Patch;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Platform;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.TypeDeath;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.WhoPlacedMap;
using DBDAnalytics.Client.WPF.Enums;
using DBDAnalytics.Client.WPF.ViewModels.Components;
using DBDAnalytics.Client.WPF.ViewModels.Components.Loadouts;
using DBDAnalytics.FIleStorageService.Client;
using DBDAnalytics.MatchService.Client.ApiClients;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Enums;
using DBDAnalytics.Shared.Contracts.Requests.MatchService;
using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using DBDAnalytics.Shared.Contracts.Responses.Match;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using DBDAnalytics.UserService.Client.Services;
using Shared.Utilities;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Helpers;
using Shared.WPF.Services;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;

namespace DBDAnalytics.Client.WPF.ViewModels.Pages
{
    internal sealed class AddMatchPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileDialogService _fileDialogService;

        private readonly IKillerReadOnlyService _killerService;
        private readonly IKillerAddonReadOnlyApiServiceFactory _killerAddonApiServiceFactory;
        private readonly CatalogService.Client.ApiClients.Loadout.Perk.IKillerPerkReadOnlyService _killerPerkReadOnlyService;

        private readonly ISurvivorReadOnlyService _survivorService;
        private readonly CatalogService.Client.ApiClients.Loadout.Perk.ISurvivorPerkReadOnlyService _survivorPerkReadOnlyService;
        private readonly IItemReadOnlyApiService _itemService;
        private readonly IItemAddonReadOnlyApiServiceFactory _itemAddonApiServiceFactory;
        private readonly ITypeDeathDeadOnlyService _typeDeathService;

        private readonly IOfferingReadOnlyService _offeringService;

        private readonly IMapReadOnlyService _mapService;
        private readonly IPatchReadOnlyService _patchService;
        private readonly IPlatformReadOnlyService _platformService;
        private readonly IPlayerAssociationReadOnlyService _playerAssociationService;
        private readonly IGameModeReadOnlyService _gameModeService;
        private readonly IGameEventReadOnlyService _gameEventService;
        private readonly IWhoPlacedMapReadOnlyService _whoPlacedMapService;

        private readonly IMatchService _matchService;
        private readonly IAuthorizationService _authorizationService;

        public AddMatchPageViewModel(
            IFileStorageService fileStorageService,
            IFileDialogService fileDialogService,

            IKillerReadOnlyService killerService,
            IKillerAddonReadOnlyApiServiceFactory killerAddonApiServiceFactory,
            CatalogService.Client.ApiClients.Loadout.Perk.IKillerPerkReadOnlyService killerPerkReadOnlyService,

            ISurvivorReadOnlyService survivorService,
            CatalogService.Client.ApiClients.Loadout.Perk.ISurvivorPerkReadOnlyService survivorPerkReadOnlyService,
            IItemReadOnlyApiService itemService,
            IItemAddonReadOnlyApiServiceFactory itemAddonApiServiceFactory,
            ITypeDeathDeadOnlyService typeDeathService,

            IOfferingReadOnlyService offeringService,

            IMapReadOnlyService mapService,
            IPatchReadOnlyService patchService,
            IPlatformReadOnlyService platformService,
            IPlayerAssociationReadOnlyService playerAssociationService,
            IGameModeReadOnlyService gameModeService,
            IGameEventReadOnlyService gameEventService,
            IWhoPlacedMapReadOnlyService whoPlacedMapService,
            
            IMatchService matchService,
            IAuthorizationService authorizationService)
        {
            _fileStorageService = fileStorageService;
            _fileDialogService = fileDialogService;

            _killerService = killerService;
            _killerAddonApiServiceFactory = killerAddonApiServiceFactory;
            _killerPerkReadOnlyService = killerPerkReadOnlyService;

            _survivorService = survivorService;
            _survivorPerkReadOnlyService = survivorPerkReadOnlyService;
            _itemService = itemService;
            _itemAddonApiServiceFactory = itemAddonApiServiceFactory;
            _typeDeathService = typeDeathService;

            _offeringService = offeringService;

            _mapService = mapService;
            _patchService = patchService;
            _platformService = platformService;
            _playerAssociationService = playerAssociationService;
            _gameModeService = gameModeService;
            _gameEventService = gameEventService;
            _whoPlacedMapService = whoPlacedMapService;

            _matchService = matchService;
            _authorizationService = authorizationService;

            KillerAddonsView = CollectionViewSource.GetDefaultView(_killerAddons);
            KillerPerksView = CollectionViewSource.GetDefaultView(_killerPerks);
            KillerOfferingsView = CollectionViewSource.GetDefaultView(_killerOfferings);

            Killer = new KillerLoadoutViewModel();
            FirstSurvivor = new SurvivorLoadoutViewModel(SurvivorTarget.First);
            SecondSurvivor = new SurvivorLoadoutViewModel(SurvivorTarget.Second);
            ThirdSurvivor = new SurvivorLoadoutViewModel(SurvivorTarget.Third);
            FourthSurvivor = new SurvivorLoadoutViewModel(SurvivorTarget.Fourth);
            Match = new MatchLoadoutViewModel(Patches, GameModes, GameEvents, WhoPlacedMaps);

            ImagesPopup = new PopupController(PopupPlacementMode.CustomRightUp, () => Images.Count > 0);

            ApplyKillerSorting();
            ApplySurvivorSorting();

            SubscribeCollectionEvents();
            SubscribeKillerEvents();
            SubscribeSurvivorsEvents();

            InitializeCommand();
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsBusy = true;

            try
            {
                await GetMapsAsync();
                await GetPatchesAsync();
                await GetGameModesAsync();
                await GetGameEventsAsync();
                await GetWhoPlacedMapsAsync();

                await GetPlatforms();
                await GetOfferings();
                await GetPlayerAssociations();

                await GetKillers();
                await GetKillerPerks();

                await GetSurvivors();
                await GetSurvivorPerks();
                await GetItems();
                await GetTypeDeaths();

                Match.SetDefaultValue();

                IsInitialize = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при инициализации страницы: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        public ObservableCollection<string> Images { get; private set; } = [];

        #region Коллекции: [Killers] - Все списки нужные для описания киллера

        public ICollectionView KillerAddonsView {  get; private set; }
        public ICollectionView KillerPerksView { get; private set; }
        public ICollectionView KillerOfferingsView { get; private set; }

        public ObservableCollection<KillerViewModel> Killers { get; private set; } = [];
        private ObservableCollection<KillerAddonViewModel> _killerAddons { get; set; } = [];
        private ObservableCollection<KillerPerkViewModel> _killerPerks { get; set; } = [];
        private ObservableCollection<OfferingViewModel> _killerOfferings { get; set; } = [];

        #endregion

        #region Коллекции: [Survivors] - Все списки нужные для описания выживших

        public ObservableCollection<SurvivorViewModel> Survivors { get; } = [];
        public ObservableCollection<ItemViewModel> Items { get; } = [];

        public ObservableCollection<PlatformResponse> Platforms { get; } = [];
        public ObservableCollection<TypeDeathResponse> TypeDeaths { get; } = [];
        public ObservableCollection<PlayerAssociationResponse> PlayerAssociations { get; } = [];

        #endregion

        #region Коллкции: [Match] - Все списки нужные для описания матча

        public List<int> Prestige { get; private set; } = [.. Enumerable.Range(0, 101)];
        public List<int> RecentGenerators { get; private set; } = [.. Enumerable.Range(0, 6)];
        public List<int> Kills { get; private set; } = [.. Enumerable.Range(0, 5)];
        public List<int> Hooks { get; private set; } = [.. Enumerable.Range(0, 13)];

        public ObservableCollection<MapResponse> Maps { get; private set; } = [];
        public ObservableCollection<PatchResponse> Patches { get; private set; } = [];
        public ObservableCollection<GameModeResponse> GameModes { get; private set; } = [];
        public ObservableCollection<GameEventResponse> GameEvents { get; private set; } = [];
        public ObservableCollection<WhoPlacedMapResponse> WhoPlacedMaps { get; private set; } = [];

        #endregion 

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController ImagesPopup { get; }

        public KillerLoadoutViewModel Killer { get; }
        public SurvivorLoadoutViewModel FirstSurvivor { get; }
        public SurvivorLoadoutViewModel SecondSurvivor { get; }
        public SurvivorLoadoutViewModel ThirdSurvivor { get; }
        public SurvivorLoadoutViewModel FourthSurvivor { get; }
        public MatchLoadoutViewModel Match { get; }

        #region Свойство [SelectedPath] - Выбор изображения

        private string? _selectedPath;
        public string? SelectedPath
        {
            get => _selectedPath;
            set => SetProperty(ref _selectedPath, value);
        }

        #endregion

        #region Свойство [MainTabControlIndex] - Индекс главного таб контрола

        private int _mainTabControlIndex;
        public int MainTabControlIndex
        {
            get => _mainTabControlIndex;
            set => SetProperty(ref _mainTabControlIndex, value);
        }

        #endregion

        #region Свойство: [CurrentType]

        private int _killerTabControlIndex;
        public int KillerTabControlIndex
        {
            get => _killerTabControlIndex;
            set => SetProperty(ref _killerTabControlIndex, value);
        }

        private int _firstSurvivorTabControlIndex;
        public int FirstSurvivorTabControlIndex
        {
            get => _firstSurvivorTabControlIndex;
            set => SetProperty(ref _firstSurvivorTabControlIndex, value);
        }

        private int _secondSurvivorTabControlIndex;
        public int SecondSurvivorTabControlIndex
        {
            get => _secondSurvivorTabControlIndex;
            set => SetProperty(ref _secondSurvivorTabControlIndex, value);
        }

        private int _thirdSurvivorTabControlIndex;
        public int ThirdSurvivorTabControlIndex
        {
            get => _thirdSurvivorTabControlIndex;
            set => SetProperty(ref _thirdSurvivorTabControlIndex, value);
        }

        private int _fourthSurvivorTabControlIndex;
        public int FourthSurvivorTabControlIndex
        {
            get => _fourthSurvivorTabControlIndex;
            set => SetProperty(ref _fourthSurvivorTabControlIndex, value);
        }

        public DisplayCollectionType CurrentDisplayCollectionType
        {
            get
            {
                Func<DisplayCollectionType> func = MainTabControlIndex switch
                {
                    0 => () =>
                    {
                        return KillerTabControlIndex switch
                        {
                            0 => Killer.AddonsViewController.CurrentType,
                            1 => Killer.PerksViewController.CurrentType,
                            2 => Killer.OfferingsViewController.CurrentType,
                            _ => DisplayCollectionType.Empty
                        };
                    }
                    ,
                    1 => () =>
                    {
                        return FirstSurvivorTabControlIndex switch
                        {
                            0 => FirstSurvivor.PerksViewController.CurrentType,
                            2 => FirstSurvivor.OfferingsViewController.CurrentType,
                            _ => DisplayCollectionType.Empty
                        };
                    }
                    ,
                    2 => () =>
                    {
                        return SecondSurvivorTabControlIndex switch
                        {
                            0 => SecondSurvivor.PerksViewController.CurrentType,
                            2 => SecondSurvivor.OfferingsViewController.CurrentType,
                            _ => DisplayCollectionType.Empty
                        };
                    }
                    ,
                    3 => () =>
                    {
                        return ThirdSurvivorTabControlIndex switch
                        {
                            0 => ThirdSurvivor.PerksViewController.CurrentType,
                            2 => ThirdSurvivor.OfferingsViewController.CurrentType,
                            _ => DisplayCollectionType.Empty
                        };
                    }
                    ,
                    4 => () =>
                    {
                        return FourthSurvivorTabControlIndex switch
                        {
                            0 => FourthSurvivor.PerksViewController.CurrentType,
                            2 => FourthSurvivor.OfferingsViewController.CurrentType,
                            _ => DisplayCollectionType.Empty
                        };
                    }
                    ,
                    _ => () => DisplayCollectionType.Empty,
                };

                DisplayCollectionType displayType = func();

                return displayType;
            }
        }

        #endregion

        /*--Обработка событий-----------------------------------------------------------------------------*/

        private void SubscribeCollectionEvents()
        {
            Images.CollectionChanged += OnImagesCollectionChange;
        }

        #region Метод [OnImagesCollectionChange]

        private void OnImagesCollectionChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (ImagesPopup.ShowCommand is not null)
            {
                var command = (RelayCommand<UIElement>)ImagesPopup.ShowCommand;
                command?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        private void SubscribeKillerEvents()
        {
            Killer.PropertyChanged += OnKillerPropertyChanged;
            Killer.SelectedPerks.CollectionChanged += SelectedPerks_CollectionChanged;
            Killer.SelectedAddons.CollectionChanged += SelectedAddons_CollectionChanged;
        }

        #region Метод [OnKillerPropertyChanged]

        private void OnKillerPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (Killer is null)
                throw new Exception($"Объект {nameof(Killer)} был null");

            if (e.PropertyName == nameof(KillerLoadoutViewModel.SelectedKiller))
                _ = GetAddonsForKiller();

            if (e.PropertyName == nameof(KillerLoadoutViewModel.SelectedOffering))
                KillerOfferingsView?.Refresh();
        }

        #endregion

        #region Методы [KillerCollectionChanged]

        private void SelectedAddons_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            => KillerAddonsView?.Refresh();

        private void SelectedPerks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            => KillerPerksView?.Refresh();

        #endregion

        private void SubscribeSurvivorsEvents()
        {
            FirstSurvivor.PropertyChanged += OnSurvivorPropertyChanged;
            SecondSurvivor.PropertyChanged += OnSurvivorPropertyChanged;
            ThirdSurvivor.PropertyChanged += OnSurvivorPropertyChanged;
            FourthSurvivor.PropertyChanged += OnSurvivorPropertyChanged;
        }

        #region Методы [OnSurvivorPropertyChanged]

        private void OnSurvivorPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (FirstSurvivor is null || SecondSurvivor is null || ThirdSurvivor is null || FourthSurvivor is null)
                throw new Exception($"Один из {nameof(FirstSurvivor)}, {nameof(SecondSurvivor)}, {nameof(ThirdSurvivor)}, {nameof(FourthSurvivor)} был null");

            if (e.PropertyName == nameof(SurvivorLoadoutViewModel.SelectedItem))
            {
                Func<Task> func = MainTabControlIndex switch
                {
                    1 => () => _ = GetAddonsForItem(FirstSurvivor.SelectedItem!.Id),
                    2 => () => _ = GetAddonsForItem(SecondSurvivor.SelectedItem!.Id),
                    3 => () => _ = GetAddonsForItem(ThirdSurvivor.SelectedItem!.Id),
                    4 => () => _ = GetAddonsForItem(FourthSurvivor.SelectedItem!.Id),
                    _ => throw new ArgumentException("Такой индекс не поддерживается")
                };

                 func?.Invoke();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            SelectImagesCommand = new RelayCommand(Execute_SelectImagesCommand, CanExecute_SelectImagesCommand);
            SelectMatchImageCommand = new RelayCommand<MatchImageType>(Execute_SelectMatchImageCommand, CanExecute_SelectMatchImageCommand);

            CreateMatchCommandAsync = new RelayCommandAsync(Execute_CreateMatchCommandAsync, CanExecute_CreateMatchCommandAsync);
        }

        #region Команда [SelectImagesCommand]: Выбор изображений

        public RelayCommand? SelectImagesCommand { get; private set; }

        private void Execute_SelectImagesCommand()
        {
            var imagesPath = _fileDialogService.OpenMultipleFilesDialog("Выберите изображения", "Image Files|*.jpg;*.jpeg;*.png;*.gif");

            if (imagesPath is null)
                return;

            foreach (var path in imagesPath)
                Images.Add(path);
        }

        private bool CanExecute_SelectImagesCommand() => true;

        #endregion

        #region Команда [SelectMatchImageCommand]: Выбор изображение начала матча

        public RelayCommand<MatchImageType>? SelectMatchImageCommand { get; private set; }

        private void Execute_SelectMatchImageCommand(MatchImageType imageType)
        {
            Action action = imageType switch
            {
                MatchImageType.Result => () =>
                {
                    Killer.KillerMatchImage = ImageHelper.ToBitmapImageFromBitmap(ImageHelper.CropImage(SelectedPath!, 70, 935, 920, 125));
                    FirstSurvivor.MatchImage = ImageHelper.ToBitmapImageFromBitmap(ImageHelper.CropImage(SelectedPath!, 70, 425, 920, 125));
                    SecondSurvivor.MatchImage = ImageHelper.ToBitmapImageFromBitmap(ImageHelper.CropImage(SelectedPath!, 70, 550, 920, 125));
                    ThirdSurvivor.MatchImage = ImageHelper.ToBitmapImageFromBitmap(ImageHelper.CropImage(SelectedPath!, 70, 680, 920, 125));
                    FourthSurvivor.MatchImage = ImageHelper.ToBitmapImageFromBitmap(ImageHelper.CropImage(SelectedPath!, 70, 810, 920, 125));
                    Match.ResultMatchImage = ImageHelper.ToBitmapImageFromBitmap(ImageHelper.CropImage(SelectedPath!, 70, 200, 980, 850));
                }
                ,
                MatchImageType.StartMatch => () => Match.StartMatchImage = ImageHelper.ToBitmapImageFromBitmap(ImageHelper.CropImage(SelectedPath!, 0, 900, 1100, 500))
                ,
                MatchImageType.EndMatch => () => Match.EndMatchImage = ImageHelper.ToBitmapImageFromBitmap(ImageHelper.CropImage(SelectedPath!, 0, 250, 600, 900))
                ,
                _ => throw new ArgumentException($"Типа {imageType} не поддерживается")
            };

            action?.Invoke();
        }

        private bool CanExecute_SelectMatchImageCommand(MatchImageType imageType) => true;

        #endregion

        #region Команда [CreateMatchCommandAsync]: Создание матча

        public RelayCommandAsync? CreateMatchCommandAsync { get; private set; }

        private async Task Execute_CreateMatchCommandAsync()
        {
            if (Match is null || Killer is null || FirstSurvivor is null || SecondSurvivor is null || ThirdSurvivor is null || FourthSurvivor is null)
                throw new Exception($"Какое то из полей был null");

            var errors = GetMatchValidationErrors();

            if (errors.Count != 0)
            {
                var sb = new StringBuilder();

                for (int i = 0; i < errors.Count; i++)
                    sb.AppendLine($"{i + 1}) {errors[i]}");

                MessageBox.Show(sb.ToString());
                return;
            }

            var result = await _matchService.AddAsync(CreateMatch());

            result.Switch(
                onSuccess: Match.SetDefaultValue,
                onFailure: errors =>
                {
                    var sb = new StringBuilder();

                    for (int i = 0; i < errors.Count; i++)
                        MessageBox.Show($"{i + 1}) {errors[i]}");
                });
        }

        private bool CanExecute_CreateMatchCommandAsync() => true;

        /*Вспомогательные методы*/

        private List<string> GetMatchValidationErrors()
        {
            var validator = new RuleValidator();

            validator
                .Required(Killer.SelectedKiller, "Вы не выбрали киллера")
                .Required(Killer.SelectedOffering, "Вы не выбрали подношение для убийцы")
                .Required(Killer.PlayerAssociation, "Вы не выбрали ассоциацию для убийцы")
                .Required(Killer.Platform, "Вы не выбрали платформу для убийцы")
                .MinCount(Killer.SelectedPerks, 4, "Вы не выбрали нужное кол-во перков у киллера")
                .MinCount(Killer.SelectedAddons, 2, "Вы не выбрали нужное кол-во улучшений у киллера");

            var survivorsToValidate = new[]
            {
                (Survivor: FirstSurvivor, Name: "первого выжившего"),
                (Survivor: SecondSurvivor, Name: "второго выжившего"),
                (Survivor: ThirdSurvivor, Name: "третьего выжившего"),
                (Survivor: FourthSurvivor, Name: "четвертого выжившего"),
            };

            foreach (var s in survivorsToValidate)
            {
                validator
                    .Required(s.Survivor.SelectedSurvivor, $"Вы не выбрали {s.Name}")
                    .Required(s.Survivor.SelectedOffering, $"Вы не выбрали подношение у {s.Name}")
                    .Required(s.Survivor.PlayerAssociation, $"Вы не выбрали ассоциацию у {s.Name}")
                    .Required(s.Survivor.Platform, $"Вы не выбрали платформу у {s.Name}")
                    .Required(s.Survivor.TypeDeath, $"Вы не выбрали тип смерти {s.Name}")
                    .Required(s.Survivor.SelectedItem, $"Вы не выбрали предмет {s.Name}")
                    .MinCount(s.Survivor.SelectedAddons, 2, $"Вы не выбрали нужное кол-во улучшений к предмету у {s.Name}");
            }

            validator
                .Required(Match.GameMode, "Вы не выбрали игровой режим")
                .Required(Match.GameEvent, "Вы не выбрали игровой ивент")
                .Required(Match.Map, "Вы не выбрали карту")
                .Required(Match.WhoPlacedMap, "Вы не выбрали кто поставил карту")
                .Required(Match.WhoPlacedMapWin, "Вы не выбрали чья карта выпала")
                .Required(Match.Patch, "Вы не выбрали патч");

            return validator.Validate();
        }

        private CreateMatchesRequest CreateMatch()
            => new(
                _authorizationService.CurrentUser!.Id,
                Match.GameMode!.Id, Match.GameEvent!.Id,
                Match.Map!.Id, Match.WhoPlacedMap!.Id, Match.WhoPlacedMapWin!.Id,
                Match.Patch!.Id,
                Match.CountKills, Match.CountHooks, Match.RecentGenerators, DateTime.UtcNow.ToString() /*Match.Date.ToString()!*/, TimeSpan.FromHours(0.23).ToString() /*Match.Duration.ToString()!*/,
                CreateKillersData(), CreateSurvivorsData());

        private List<CreateMatchKillerDataRequest> CreateKillersData()
            => 
            [
                new CreateMatchKillerDataRequest(
                    Killer.SelectedKiller!.Id, Killer.SelectedOffering!.Id,
                    Killer.PlayerAssociation!.Id, Killer.Platform!.Id,
                    Killer.Prestige, Killer.Score, Killer.Experience, Killer.NumberOfBloodDrops,
                    Killer.IsBot, Killer.IsAnanyms,
                    [.. Killer.SelectedPerks.Select(kp => kp.Id)],
                    [.. Killer.SelectedAddons.Select(ka => ka.Id)])
            ];

        private List<CreateMatchSurvivorDataRequest> CreateSurvivorsData()
            => 
            [
                new CreateMatchSurvivorDataRequest(
                    FirstSurvivor.SelectedSurvivor!.Id, FirstSurvivor.SelectedOffering!.Id,
                    FirstSurvivor.PlayerAssociation!.Id, FirstSurvivor.Platform!.Id, FirstSurvivor.TypeDeath!.Id,
                    FirstSurvivor.Prestige, FirstSurvivor.Score, FirstSurvivor.Experience, FirstSurvivor.NumberOfBloodDrops,
                    FirstSurvivor.IsBot, FirstSurvivor.IsAnanyms,
                    [.. FirstSurvivor.SelectedPerks.Select(sp => sp.Id)],
                    [
                        new(FirstSurvivor.SelectedItem!.Id, [.. FirstSurvivor.SelectedAddons.Select(ia => ia.Id)])
                    ]),

                new CreateMatchSurvivorDataRequest(
                    SecondSurvivor.SelectedSurvivor!.Id, SecondSurvivor.SelectedOffering!.Id,
                    SecondSurvivor.PlayerAssociation!.Id, SecondSurvivor.Platform!.Id, SecondSurvivor.TypeDeath!.Id,
                    SecondSurvivor.Prestige, SecondSurvivor.Score, SecondSurvivor.Experience, SecondSurvivor.NumberOfBloodDrops,
                    SecondSurvivor.IsBot, SecondSurvivor.IsAnanyms,
                    [.. SecondSurvivor.SelectedPerks.Select(sp => sp.Id)],
                    [
                        new(SecondSurvivor.SelectedItem!.Id, [.. SecondSurvivor.SelectedAddons.Select(ia => ia.Id)])
                    ]),

                new CreateMatchSurvivorDataRequest(
                    ThirdSurvivor.SelectedSurvivor!.Id, ThirdSurvivor.SelectedOffering!.Id,
                    ThirdSurvivor.PlayerAssociation!.Id, ThirdSurvivor.Platform!.Id, ThirdSurvivor.TypeDeath!.Id,
                    ThirdSurvivor.Prestige, ThirdSurvivor.Score, ThirdSurvivor.Experience, ThirdSurvivor.NumberOfBloodDrops,
                    ThirdSurvivor.IsBot, ThirdSurvivor.IsAnanyms,
                    [.. ThirdSurvivor.SelectedPerks.Select(sp => sp.Id)],
                    [
                        new(ThirdSurvivor.SelectedItem!.Id, [.. ThirdSurvivor.SelectedAddons.Select(ia => ia.Id)])
                    ]),

                new CreateMatchSurvivorDataRequest(
                    FourthSurvivor.SelectedSurvivor!.Id, FourthSurvivor.SelectedOffering!.Id,
                    FourthSurvivor.PlayerAssociation!.Id, FourthSurvivor.Platform!.Id, FourthSurvivor.TypeDeath!.Id,
                    FourthSurvivor.Prestige, FourthSurvivor.Score, FourthSurvivor.Experience, FourthSurvivor.NumberOfBloodDrops,
                    FourthSurvivor.IsBot, FourthSurvivor.IsAnanyms,
                    [.. FourthSurvivor.SelectedPerks.Select(sp => sp.Id)],
                    [
                        new(FourthSurvivor.SelectedItem!.Id, [.. FourthSurvivor.SelectedAddons.Select(ia => ia.Id)])
                    ]),
            ];

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Методы [GetAll]: Получение данных для киллера

        private async Task GetKillers()
        {
            foreach (var killer in await _killerService.GetAllAsync())
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerPortraits}/{killer.KillerImageKey}");
                var killerVm = new KillerViewModel(killer);
                killerVm.SetImage(await ImageHelper.ImageFromUrlAsync(url));

                Killers.Add(killerVm);
            }

            Killer.SelectedKiller = Killers.FirstOrDefault();
        }

        private async Task GetAddonsForKiller()
        {
            _killerAddons.Clear();

            var service = _killerAddonApiServiceFactory.Create(Killer.SelectedKiller!.Id);

            foreach (var addon in await service.GetAllAsync())
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerAddons(Killer.SelectedKiller!.Name)}/{addon.ImageKey}");
                var addonVm = new KillerAddonViewModel(addon);
                addonVm.SetImage(await ImageHelper.ImageFromUrlAsync(url));

                _killerAddons.Add(addonVm);
            }
        }

        private async Task GetKillerPerks()
        {
            foreach (var killerPerk in await _killerPerkReadOnlyService.GetAllAsync())
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerPerks(Killers.FirstOrDefault(k => k.Id == killerPerk.KillerId)?.Name!)}/{killerPerk.ImageKey}");
                var killerPerkVm = new KillerPerkViewModel(killerPerk);
                killerPerkVm.SetImage(await ImageHelper.ImageFromUrlAsync(url));

                _killerPerks.Add(killerPerkVm);
            }
        }

        #endregion

        #region Методы [GetAll]: Получение данных для выживших

        private async Task GetSurvivors()
        {
            foreach (var survivors in await _survivorService.GetAllAsync())
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.SurvivorPortraits}/{survivors.ImageKey}");
                var survivorVm = new SurvivorViewModel(survivors);
                survivorVm.SetImage(await ImageHelper.ImageFromUrlAsync(url));

                Survivors.Add(survivorVm);
            }

            FirstSurvivor.SelectedSurvivor = Survivors.FirstOrDefault();
            SecondSurvivor.SelectedSurvivor = Survivors.FirstOrDefault();
            ThirdSurvivor.SelectedSurvivor = Survivors.FirstOrDefault();
            FourthSurvivor.SelectedSurvivor = Survivors.FirstOrDefault();
        }

        private async Task GetSurvivorPerks()
        {
            List<SurvivorPerkViewModel> perks = [];

            foreach (var survivorPerk in await _survivorPerkReadOnlyService.GetAllAsync())
            {
                var url = string.Empty;
                try
                {
                    url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.SurvivorPerks(Survivors.FirstOrDefault(s => s.Id == survivorPerk.SurvivorId)?.Name!)}/{survivorPerk.ImageKey}");
                }
                catch (Exception)
                {

                }

                var survivorPerkVm = new SurvivorPerkViewModel(survivorPerk);
                survivorPerkVm.SetImage(await ImageHelper.ImageFromUrlAsync(url));
                perks.Add(survivorPerkVm);
            }

            FirstSurvivor.LoadPerks(perks);
            SecondSurvivor.LoadPerks(perks);
            ThirdSurvivor.LoadPerks(perks);
            FourthSurvivor.LoadPerks(perks);
        }

        private async Task GetItems()
        {
            foreach (var item in await _itemService.GetAllAsync())
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.Items}/{item.ImageKey}");
                var itemVm = new ItemViewModel(item);
                itemVm.SetImage(await ImageHelper.ImageFromUrlAsync(url));
                Items.Add(itemVm);
            }
        }

        private async Task GetAddonsForItem(string itemId)
        {
            async Task<List<ItemAddonViewModel>> GetAddons(string id)
            {
                List<ItemAddonViewModel> addons = [];

                var service = _itemAddonApiServiceFactory.Create(itemId);

                var selectedItemName = Items.FirstOrDefault(i => i.Id == itemId)?.Name;

                foreach (var addon in await service.GetAllAsync())
                {
                    var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.ItemAddons(selectedItemName!)}/{addon.ImageKey}");
                    var addonVm = new ItemAddonViewModel(addon);
                    addonVm.SetImage(await ImageHelper.ImageFromUrlAsync(url));
                    addons.Add(addonVm);
                }

                return addons;
            }

            Func<Task> func = MainTabControlIndex switch
            {
                1 => async () => FirstSurvivor.LoadItemAddons(await GetAddons(itemId)),
                2 => async () => SecondSurvivor.LoadItemAddons(await GetAddons(itemId)),
                3 => async () => ThirdSurvivor.LoadItemAddons(await GetAddons(itemId)),
                4 => async () => FourthSurvivor.LoadItemAddons(await GetAddons(itemId)),
                _ => throw new ArgumentException("Такой индекс не поддерживается")
            };

            await func.Invoke();
        }

        private async Task GetPlatforms()
        {
            foreach (var platform in await _platformService.GetAllAsync())
                Platforms.Add(platform);
        }

        private async Task GetTypeDeaths()
        {
            foreach (var typeDeath in await _typeDeathService.GetAllAsync())
                TypeDeaths.Add(typeDeath);
        }

        private async Task GetPlayerAssociations()
        {
            foreach (var association in await _playerAssociationService.GetAllAsync())
                PlayerAssociations.Add(association);
        }

        #endregion

        #region Метод [GetAll]: Получение подношений

        private async Task GetOfferings()
        {
            foreach (var offering in await _offeringService.GetAllForKillerAsync())
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.GetOfferingPathForRole((Roles)offering.RoleId)}/{offering.ImageKey}");
                var offeringVm = new OfferingViewModel(offering);
                offeringVm.SetImage(await ImageHelper.ImageFromUrlAsync(url));

                _killerOfferings.Add(offeringVm);
            }

            List<OfferingViewModel> survivorOfferings = [];

            foreach (var offering in await _offeringService.GetAllForSurvivorAsync())
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.GetOfferingPathForRole((Roles)offering.RoleId)}/{offering.ImageKey}");
                var image = await ImageHelper.ImageFromUrlAsync(url);

                survivorOfferings.Add(CreateOfferingViewModel(offering, image));
            }

            FirstSurvivor.LoadOfferings(survivorOfferings);
            SecondSurvivor.LoadOfferings(survivorOfferings);
            ThirdSurvivor.LoadOfferings(survivorOfferings);
            FourthSurvivor.LoadOfferings(survivorOfferings);
        }

        private static OfferingViewModel CreateOfferingViewModel(OfferingResponse model, ImageSource? image)
        {
            var offeringVm = new OfferingViewModel(model);
            offeringVm.SetImage(image);
            return offeringVm;
        }

        #endregion

        #region Методы [GetAll]: Получение данных для матча

        private async Task GetMapsAsync()
        {
            foreach (var map in await _mapService.GetAllAsync())
                Maps.Add(map);
        }

        private async Task GetGameModesAsync()
        {
            foreach (var gameMode in await _gameModeService.GetAllAsync())
                GameModes.Add(gameMode);
        }

        private async Task GetGameEventsAsync()
        {
            foreach (var gameEvent in await _gameEventService.GetAllAsync())
                GameEvents.Add(gameEvent);
        }

        private async Task GetPatchesAsync()
        {
            foreach (var patch in await _patchService.GetAllAsync())
                Patches.Add(patch);
        }

        private async Task GetWhoPlacedMapsAsync()
        {
            foreach (var wpm in await _whoPlacedMapService.GetAllAsync())
                WhoPlacedMaps.Add(wpm);
        }

        #endregion

        private void ApplyKillerSorting()
        {
            ApplyKillerAddonsSorting();
            ApplyKillerPerksSorting();
            ApplyKillerOfferingsSorting();
        }

        #region Методы [Sortings-Killer]: Сортировка ICollectionVIew для киллеров

        private void ApplyKillerAddonsSorting()
        {
            if (KillerAddonsView != null)
            {
                KillerAddonsView.SortDescriptions.Clear();
                KillerAddonsView.SortDescriptions.Add(new SortDescription(nameof(KillerAddonViewModel.IsPinned), ListSortDirection.Descending));
                KillerAddonsView.SortDescriptions.Add(new SortDescription(nameof(KillerAddonViewModel.Name), ListSortDirection.Ascending));
            }
        }

        private void ApplyKillerPerksSorting()
        {
            if (KillerPerksView != null)
            {
                KillerPerksView.SortDescriptions.Clear();
                KillerPerksView.SortDescriptions.Add(new SortDescription(nameof(KillerPerkViewModel.IsPinned), ListSortDirection.Descending));
                KillerPerksView.SortDescriptions.Add(new SortDescription(nameof(KillerPerkViewModel.Name), ListSortDirection.Ascending));
            }
        }

        private void ApplyKillerOfferingsSorting()
        {
            if (KillerOfferingsView != null)
            {
                KillerOfferingsView.SortDescriptions.Clear();
                KillerOfferingsView.SortDescriptions.Add(new SortDescription(nameof(OfferingViewModel.IsPinned), ListSortDirection.Descending));
                KillerOfferingsView.SortDescriptions.Add(new SortDescription(nameof(OfferingViewModel.Name), ListSortDirection.Ascending));
            }
        }

        #endregion

        private void ApplySurvivorSorting()
        {
            ApplyFirstSurvivorPerksSorting();
            ApplySecondSurvivorPerksSorting();
            ApplyThirdSurvivorPerksSorting();
            ApplyFourthSurvivorPerksSorting();

            ApplyItemAddonsFirstSurvivorSorting();
            ApplyItemAddonsSecondSurvivorSorting();
            ApplyItemAddonsThirdSurvivorSorting();
            ApplyItemAddonsFourthSurvivorSorting();

            ApplyFirstSurvivorOfferingsViewSorting();
            ApplySecondSurvivorOfferingsViewSorting();
            ApplyThirdSurvivorOfferingsViewSorting();
            ApplyFourthSurvivorOfferingsViewSorting();
        }

        #region Методы [Sortings-Survivor]: Сортировка ICollectionView для выживших

        // Перки
        private void ApplyFirstSurvivorPerksSorting()
        {
            if (FirstSurvivor.PerksView != null)
            {
                FirstSurvivor.PerksView.SortDescriptions.Clear();
                FirstSurvivor.PerksView.SortDescriptions.Add(new SortDescription(nameof(SurvivorPerkViewModel.IsPinnedFirst), ListSortDirection.Descending));
                FirstSurvivor.PerksView.SortDescriptions.Add(new SortDescription(nameof(SurvivorPerkViewModel.Name), ListSortDirection.Ascending));
            }
        }

        private void ApplySecondSurvivorPerksSorting()
        {
            if (SecondSurvivor.PerksView != null)
            {
                SecondSurvivor.PerksView.SortDescriptions.Clear();
                SecondSurvivor.PerksView.SortDescriptions.Add(new SortDescription(nameof(SurvivorPerkViewModel.IsPinnedSecond), ListSortDirection.Descending));
                SecondSurvivor.PerksView.SortDescriptions.Add(new SortDescription(nameof(SurvivorPerkViewModel.Name), ListSortDirection.Ascending));
            }
        }

        private void ApplyThirdSurvivorPerksSorting()
        {
            if (ThirdSurvivor.PerksView != null)
            {
                ThirdSurvivor.PerksView.SortDescriptions.Clear();
                ThirdSurvivor.PerksView.SortDescriptions.Add(new SortDescription(nameof(SurvivorPerkViewModel.IsPinnedThird), ListSortDirection.Descending));
                ThirdSurvivor.PerksView.SortDescriptions.Add(new SortDescription(nameof(SurvivorPerkViewModel.Name), ListSortDirection.Ascending));
            }
        }

        private void ApplyFourthSurvivorPerksSorting()
        {
            if (FourthSurvivor.PerksView != null)
            {
                FourthSurvivor.PerksView.SortDescriptions.Clear();
                FourthSurvivor.PerksView.SortDescriptions.Add(new SortDescription(nameof(SurvivorPerkViewModel.IsPinnedFourth), ListSortDirection.Descending));
                FourthSurvivor.PerksView.SortDescriptions.Add(new SortDescription(nameof(SurvivorPerkViewModel.Name), ListSortDirection.Ascending));
            }
        }

        // Улучшения к предметам
        private void ApplyItemAddonsFirstSurvivorSorting()
        {
            if (FirstSurvivor.ItemAddonsView != null)
            {
                FirstSurvivor.ItemAddonsView.SortDescriptions.Clear();
                FirstSurvivor.ItemAddonsView.SortDescriptions.Add(new SortDescription("IsPinnedFirst", ListSortDirection.Descending));
                FirstSurvivor.ItemAddonsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void ApplyItemAddonsSecondSurvivorSorting()
        {
            if (SecondSurvivor.ItemAddonsView != null)
            {
                SecondSurvivor.ItemAddonsView.SortDescriptions.Clear();
                SecondSurvivor.ItemAddonsView.SortDescriptions.Add(new SortDescription("IsPinnedSecond", ListSortDirection.Descending));
                SecondSurvivor.ItemAddonsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void ApplyItemAddonsThirdSurvivorSorting()
        {
            if (ThirdSurvivor.ItemAddonsView != null)
            {
                ThirdSurvivor.ItemAddonsView.SortDescriptions.Clear();
                ThirdSurvivor.ItemAddonsView.SortDescriptions.Add(new SortDescription("IsPinnedThird", ListSortDirection.Descending));
                ThirdSurvivor.ItemAddonsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void ApplyItemAddonsFourthSurvivorSorting()
        {
            if (FourthSurvivor.ItemAddonsView != null)
            {
                FourthSurvivor.ItemAddonsView.SortDescriptions.Clear();
                FourthSurvivor.ItemAddonsView.SortDescriptions.Add(new SortDescription("IsPinnedFourth", ListSortDirection.Descending));
                FourthSurvivor.ItemAddonsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        // Подношения
        private void ApplyFirstSurvivorOfferingsViewSorting()
        {
            if (FirstSurvivor.OfferingsView != null)
            {
                FirstSurvivor.OfferingsView.SortDescriptions.Clear();
                FirstSurvivor.OfferingsView.SortDescriptions.Add(new SortDescription("IsPinnedFirst", ListSortDirection.Descending));
                FirstSurvivor.OfferingsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void ApplySecondSurvivorOfferingsViewSorting()
        {
            if (SecondSurvivor.OfferingsView != null)
            {
                SecondSurvivor.OfferingsView.SortDescriptions.Clear();
                SecondSurvivor.OfferingsView.SortDescriptions.Add(new SortDescription("IsPinnedSecond", ListSortDirection.Descending));
                SecondSurvivor.OfferingsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void ApplyThirdSurvivorOfferingsViewSorting()
        {
            if (ThirdSurvivor.OfferingsView != null)
            {
                ThirdSurvivor.OfferingsView.SortDescriptions.Clear();
                ThirdSurvivor.OfferingsView.SortDescriptions.Add(new SortDescription("IsPinnedThird", ListSortDirection.Descending));
                ThirdSurvivor.OfferingsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void ApplyFourthSurvivorOfferingsViewSorting()
        {
            if (FourthSurvivor.OfferingsView != null)
            {
                FourthSurvivor.OfferingsView.SortDescriptions.Clear();
                FourthSurvivor.OfferingsView.SortDescriptions.Add(new SortDescription("IsPinnedFourth", ListSortDirection.Descending));
                FourthSurvivor.OfferingsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        #endregion
    }
}