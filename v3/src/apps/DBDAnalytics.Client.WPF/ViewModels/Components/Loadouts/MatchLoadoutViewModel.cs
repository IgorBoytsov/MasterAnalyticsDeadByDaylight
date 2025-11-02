using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using DBDAnalytics.Shared.Contracts.Responses.Match;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media.Imaging;

namespace DBDAnalytics.Client.WPF.ViewModels.Components.Loadouts
{
    sealed class MatchLoadoutViewModel : BaseViewModel
    {
        private readonly IReadOnlyCollection<PatchResponse> _patches;
        private readonly IReadOnlyCollection<GameModeResponse> _gameModes;
        private readonly IReadOnlyCollection<GameEventResponse> _gameEvents;
        private readonly IReadOnlyCollection<WhoPlacedMapResponse> _whoPlacedMaps;

        public MatchLoadoutViewModel(
            IReadOnlyCollection<PatchResponse> patches,
            IReadOnlyCollection<GameModeResponse> gameModes,
            IReadOnlyCollection<GameEventResponse> gameEvents,
            IReadOnlyCollection<WhoPlacedMapResponse> whoPlacedMaps)
        {
            _patches = patches;
            _gameModes = gameModes;
            _gameEvents = gameEvents;
            _whoPlacedMaps = whoPlacedMaps;
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private TimeSpan? _duration;
        public TimeSpan? Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private int _countKills;
        public int CountKills
        {
            get => _countKills;
            set => SetProperty(ref _countKills, value);
        }

        private int _countHooks;
        public int CountHooks
        {
            get => _countHooks;
            set => SetProperty(ref _countHooks, value);
        }

        private int _recentGenerators;
        public int RecentGenerators
        {
            get => _recentGenerators;
            set => SetProperty(ref _recentGenerators, value);
        }

        private MapResponse? _map;
        public MapResponse? Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        private PatchResponse? _patch;
        public PatchResponse? Patch
        {
            get => _patch;
            set => SetProperty(ref _patch, value);
        }

        private GameModeResponse? _gameMode;
        public GameModeResponse GameMode
        {
            get => _gameMode;
            set => SetProperty(ref _gameMode, value);
        }

        private GameEventResponse? _gameEvent;
        public GameEventResponse? GameEvent
        {
            get => _gameEvent;
            set => SetProperty(ref _gameEvent, value);
        }

        private WhoPlacedMapResponse? _whoPlacedMap;
        public WhoPlacedMapResponse? WhoPlacedMap
        {
            get => _whoPlacedMap; 
            set => SetProperty(ref _whoPlacedMap, value);
        }

        private WhoPlacedMapResponse? _whoPlacedMapWin;
        public WhoPlacedMapResponse? WhoPlacedMapWin
        {
            get => _whoPlacedMapWin; 
            set => SetProperty(ref _whoPlacedMapWin, value);
        }

        private BitmapImage? _startMatchImage;
        public BitmapImage? StartMatchImage
        {
            get => _startMatchImage;
            set => SetProperty(ref _startMatchImage, value);
        }

        private BitmapImage? _endMatchImage;
        public BitmapImage? EndMatchImage
        {
            get => _endMatchImage;
            set => SetProperty(ref _endMatchImage, value);
        }

        private BitmapImage? _resultMatchImage;
        public BitmapImage? ResultMatchImage
        {
            get => _resultMatchImage;
            set => SetProperty(ref _resultMatchImage, value);
        }

        public void SetDefaultValue()
        {
            Date = DateTime.Now;
            Duration = TimeSpan.Zero;
            CountKills = 0;
            CountHooks = 0;
            RecentGenerators = 0;

            Map = null;
            Patch = _patches.OrderBy(p => p.Date).FirstOrDefault(p => DateTime.Parse(p.Date).Date <= DateTime.Now.Date);
            GameMode = _gameModes.FirstOrDefault()!;
            GameEvent = _gameEvents.FirstOrDefault();
            WhoPlacedMap = _whoPlacedMaps.FirstOrDefault();
            WhoPlacedMapWin = _whoPlacedMaps.FirstOrDefault();

            StartMatchImage = null;
            EndMatchImage = null;
            ResultMatchImage = null;
        }
    }
}