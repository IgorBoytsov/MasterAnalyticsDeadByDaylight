using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Utils.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    public class RandomizerPageViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly DataManager _dataManager;

        private readonly Random random = new();

        public RandomizerPageViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dataManager = _serviceProvider.GetRequiredService<DataManager>();

            RandomizeKiller();
            RandomizeSurvivor();
        }

        public void Update(object value)
        {
            
        }

        public int MaxWidth { get; set; } = 1200;

        #region Свойства : Киллер

        private Killer _randomKiller;
        public Killer RandomKiller
        {
            get => _randomKiller;
            set
            {
                _randomKiller = value;
                OnPropertyChanged();
            }
        }

        private KillerAddon _firstRandomKillerAddon;
        public KillerAddon FirstRandomKillerAddon
        {
            get => _firstRandomKillerAddon;
            set
            {
                _firstRandomKillerAddon = value;
                OnPropertyChanged();
            }
        }

        private KillerAddon _secondRandomKillerAddon;
        public KillerAddon SecondRandomKillerAddon
        {
            get => _secondRandomKillerAddon;
            set
            {
                _secondRandomKillerAddon = value;
                OnPropertyChanged();
            }
        }

        private Offering _randomKillerOffering;
        public Offering RandomKillerOffering
        {
            get => _randomKillerOffering;
            set
            {
                _randomKillerOffering = value;
                OnPropertyChanged();
            }
        }

        private KillerPerk _firstRandomKillerPerk;
        public KillerPerk FirstRandomKillerPerk
        {
            get => _firstRandomKillerPerk;
            set
            {
                _firstRandomKillerPerk = value;
                OnPropertyChanged();
            }
        }

        private KillerPerk _secondRandomKillerPerk;
        public KillerPerk SecondRandomKillerPerk
        {
            get => _secondRandomKillerPerk;
            set
            {
                _secondRandomKillerPerk = value;
                OnPropertyChanged();
            }
        }

        private KillerPerk _thirdRandomKillerPerk;
        public KillerPerk ThirdRandomKillerPerk
        {
            get => _thirdRandomKillerPerk;
            set
            {
                _thirdRandomKillerPerk = value;
                OnPropertyChanged();
            }
        }

        private KillerPerk _fourthRandomKillerPerk;
        public KillerPerk FourthRandomKillerPerk
        {
            get => _fourthRandomKillerPerk;
            set
            {
                _fourthRandomKillerPerk = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выживший

        private Survivor _randomSurvivor;
        public Survivor RandomSurvivor
        {
            get => _randomSurvivor;
            set
            {
                _randomSurvivor = value;
                OnPropertyChanged();
            }
        }

        private Offering _randomSurvivorOffering;
        public Offering RandomSurvivorOffering
        {
            get => _randomSurvivorOffering;
            set
            {
                _randomSurvivorOffering = value;
                OnPropertyChanged();
            }
        }

        private Item _randomSurvivorItem;
        public Item RandomSurvivorItem
        {
            get => _randomSurvivorItem;
            set
            {
                _randomSurvivorItem = value;
                OnPropertyChanged();
            }
        }

        private ItemAddon _firstRandomItemAddon;
        public ItemAddon FirstRandomItemAddon
        {
            get => _firstRandomItemAddon;
            set
            {
                _firstRandomItemAddon = value;
                OnPropertyChanged();
            }
        }

        private ItemAddon _secondRandomItemAddon;
        public ItemAddon SecondRandomItemAddon
        {
            get => _secondRandomItemAddon;
            set
            {
                _secondRandomItemAddon = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerk _firstRandomSurvivorPerk;
        public SurvivorPerk FirstRandomSurvivorPerk
        {
            get => _firstRandomSurvivorPerk;
            set
            {
                _firstRandomSurvivorPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerk _secondRandomSurvivorPerk;
        public SurvivorPerk SecondRandomSurvivorPerk
        {
            get => _secondRandomSurvivorPerk;
            set
            {
                _secondRandomSurvivorPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerk _thirdRandomSurvivorPerk;
        public SurvivorPerk ThirdRandomSurvivorPerk
        {
            get => _thirdRandomSurvivorPerk;
            set
            {
                _thirdRandomSurvivorPerk = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerk _fourthRandomSurvivorPerk;
        public SurvivorPerk FourthRandomSurvivorPerk
        {
            get => _fourthRandomSurvivorPerk;
            set
            {
                _fourthRandomSurvivorPerk = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private RelayCommand _randomizeKillerCommand;
        public RelayCommand RandomizeKillerCommand { get => _randomizeKillerCommand ??= new(obj => { RandomizeKiller(); }); }

        private RelayCommand _randomizeSurvivorCommand;
        public RelayCommand RandomizeSurvivorCommand { get => _randomizeSurvivorCommand ??= new(obj => { RandomizeSurvivor(); }); }

        private void RandomizeKiller()
        {
            var killers = _dataManager.Killers.Skip(1).ToList();
            RandomKiller = killers[random.Next(killers.Count)];

            var killerAddons = _dataManager.KillerAddons
                .Where(x => x.IdKiller == RandomKiller.IdKiller)
                .ToList();

            FirstRandomKillerAddon = GetRandomAndRemove(killerAddons);
            SecondRandomKillerAddon = GetRandomAndRemove(killerAddons);

            var killerAndGeneralOffering = _dataManager.Offerings.Where(x => x.IdRole != _dataManager._SURVIVOR_ROLE).ToList();

            RandomKillerOffering = killerAndGeneralOffering[random.Next(killerAndGeneralOffering.Count)];

            var killerPerks = _dataManager.KillerPerks.ToList();

            FirstRandomKillerPerk = GetRandomAndRemove(killerPerks);
            SecondRandomKillerPerk = GetRandomAndRemove(killerPerks);
            ThirdRandomKillerPerk = GetRandomAndRemove(killerPerks);
            FourthRandomKillerPerk = GetRandomAndRemove(killerPerks);  
        }

        private void RandomizeSurvivor()
        {
            var survivors = _dataManager.Survivors.Skip(1).ToList();
            RandomSurvivor = survivors[random.Next(survivors.Count)];

            RandomSurvivorItem = _dataManager.Items.ToList()[random.Next(_dataManager.Items.ToList().Count)];

            var itemsAddons = _dataManager.ItemAddons
                .Where(x => x.IdItem == RandomSurvivorItem.IdItem)
                .ToList();

            FirstRandomItemAddon = GetRandomAndRemove(itemsAddons);
            SecondRandomItemAddon = GetRandomAndRemove(itemsAddons);

            var killerAndGeneralOffering = _dataManager.Offerings.Where(x => x.IdRole != _dataManager._SURVIVOR_ROLE).ToList();

            RandomKillerOffering = _dataManager.Offerings.Where(x => x.IdRole != _dataManager._KILLER_ROLE).ToList()[random.Next(_dataManager.Offerings.Count(x => x.IdRole != _dataManager._KILLER_ROLE))];

            var survivorPerks = _dataManager.SurvivorPerks.ToList();

            FirstRandomSurvivorPerk = GetRandomAndRemove(survivorPerks);
            SecondRandomSurvivorPerk = GetRandomAndRemove(survivorPerks);
            ThirdRandomSurvivorPerk = GetRandomAndRemove(survivorPerks);
            FourthRandomSurvivorPerk = GetRandomAndRemove(survivorPerks);
        }

        private T GetRandomAndRemove<T>(List<T> list)
        {
            if (list.Count == 0)
                return default;

            int index = random.Next(list.Count);
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }
    }
}
