using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    public class AddMatchWindowViewModel : BaseViewModel
    {

        #region Коллекции для Убийц   
        
        public ObservableCollection<Killer> KillerList { get; set; }

        public ObservableCollection<KillerAddon> KillerAddonList { get; set; }

        public ObservableCollection<KillerPerk> KillerPerkList { get; set; }

        public ObservableCollection<KillerInfo> KillerInfoList { get; set; }

        #endregion

        #region Коллекции для выживших   

        public ObservableCollection<Survivor> SurvivorList { get; set; }

        public ObservableCollection<SurvivorPerk> SurvivorPerkList { get; set; }

        public ObservableCollection<SurvivorInfo> SurvivorInfoList { get; set; }

        public ObservableCollection<TypeDeath> TypeDeathList { get; set; }

        public ObservableCollection<Item> ItemList { get; set; }

        public ObservableCollection<ItemAddon> ItemAddonList { get; set; }

        #endregion

        #region Общие коллекции 

        public ObservableCollection<GameMode> GameModeList { get; set; }

        public ObservableCollection<GameEvent> GameEventList { get; set; }

        public ObservableCollection<Map> MapList { get; set; }

        public ObservableCollection<Offering> OfferingList { get; set; }

        public ObservableCollection<Patch> PatchList { get; set; }

        public ObservableCollection<Platform> PlatformList { get; set; }

        public ObservableCollection<PlayerAssociation> PlayerAssociationList { get; set; }

        public ObservableCollection<Role> RoleList { get; set; }

        public ObservableCollection<WhoPlacedMap> WhoPlacedMapList { get; set; }

        #endregion

        #region Свойства для Убийц

        private Killer _selectedKiller;
        public Killer SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                if (_selectedKiller != null)
                {
                    _selectedKiller = value;
                }
            }
        }

        private Platform _selectedKillerPlatform;
        public Platform SelectedKillerPlatform
        {
            get => _selectedKillerPlatform;
            set
            {
                if (_selectedKillerPlatform != null)
                {
                    _selectedKillerPlatform = value;
                }
            }
        }

        private KillerPerk _selectedKillerFirstPerk;
        public KillerPerk SelectedKillerFirstPerk
        {
            get => _selectedKillerFirstPerk;
            set
            {
                if (_selectedKillerFirstPerk != null)
                {
                    _selectedKillerFirstPerk = value;
                }
            }
        }

        private KillerPerk _selectedKillerSecondPerk;
        public KillerPerk SelectedKillerSecondPerk
        {
            get => _selectedKillerSecondPerk;
            set
            {
                if (_selectedKillerSecondPerk != null)
                {
                    _selectedKillerSecondPerk = value;
                }
            }
        }
        
        private KillerPerk _selectedKillerThirdPerk;
        public KillerPerk SelectedKillerThirdPerk
        {
            get => _selectedKillerThirdPerk;
            set
            {
                if (_selectedKillerThirdPerk != null)
                {
                    _selectedKillerThirdPerk = value;
                }
            }
        }
        
        private KillerPerk _selectedKillerFourthPerk;
        public KillerPerk SelectedKillerFourthPerk
        {
            get => _selectedKillerFourthPerk;
            set
            {
                if (_selectedKillerFourthPerk != null)
                {
                    _selectedKillerFourthPerk = value;
                }
            }
        }

        private KillerAddon _selectedKillerFirstAddon;
        public KillerAddon SelectedKillerFirstAddon
        {
            get => _selectedKillerFirstAddon;
            set
            {
                if (_selectedKillerFirstAddon != null)
                {
                    _selectedKillerFirstAddon = value;
                }
            }
        }

        private KillerAddon _selectedKillerSecondAddon;
        public KillerAddon SelectedKillerSecondAddon
        {
            get => _selectedKillerSecondAddon;
            set
            {
                if (_selectedKillerSecondAddon != null)
                {
                    _selectedKillerSecondAddon = value;
                }
            }
        }

        private Offering _selectedKillerOffering;
        public Offering SelectedKillerOffering
        {
            get => _selectedKillerOffering;
            set
            {
                if (_selectedKillerOffering != null)
                {
                    _selectedKillerOffering = value;
                }
            }
        }

        private int _killerInfoID;
        public int KillerInfoID
        {
            get => _killerInfoID;
            set
            {
                _killerInfoID = value;
            }
        }

        private int _killerInfoPrestige;
        public int KillerInfoPrestige
        {
            get => _killerInfoPrestige;
            set
            {
                _killerInfoPrestige = value;
            }
        }

        private int _killerInfoAccount;
        public int KillerInfoAccount
        {
            get => _killerInfoAccount;
            set
            {
                _killerInfoAccount = value;
            }
        }

        private bool _killerInfoBot;
        public bool KillerInfoBot
        {
            get => _killerInfoBot;
            set
            {
                _killerInfoBot = value;
            }
        }

        private bool _killerInfoAnonymousMode;
        public bool KillerInfoAnonymousMode
        {
            get => _killerInfoAnonymousMode;
            set
            {
                _killerInfoAnonymousMode = value;
                if (value == true)
                {
                    
                }
            }
        }

        private PlayerAssociation _killerPlayerAssociation;
        public PlayerAssociation KillerPlayerAssociation
        {
            get => _killerPlayerAssociation;
            set
            {
                if (_killerPlayerAssociation != null)
                {
                    _killerPlayerAssociation = value;
                }
            }
        }

        #endregion

        #region Свойства для первого вижившего   

        private Survivor _selectedFirstSurvivor;
        public Survivor SelectedFirstSurvivor
        {
            get => _selectedFirstSurvivor;
            set
            {
                if (_selectedFirstSurvivor != null)
                {
                    _selectedFirstSurvivor = value;
                }
            }
        }

        private Platform _selectedFirstSurvivorPlatform;
        public Platform SelectedFirstSurvivorPlatform
        {
            get => _selectedFirstSurvivorPlatform;
            set
            {
                if (_selectedFirstSurvivorPlatform != null)
                {
                    _selectedFirstSurvivorPlatform = value;
                }
            }
        }

        private TypeDeath _selectedFirstSurvivorTypeDeath;
        public TypeDeath SelectedFirstSurvivorTypeDeath
        {
            get => _selectedFirstSurvivorTypeDeath;
            set
            {
                if (_selectedFirstSurvivorTypeDeath != null)
                {
                    _selectedFirstSurvivorTypeDeath = value;
                }
            }
        }

        private SurvivorPerk _selectedFirstSurvivorFirstPerk;
        public SurvivorPerk SelectedFirstSurvivorFirstPerk
        {
            get => _selectedFirstSurvivorFirstPerk;
            set
            {
                if (_selectedFirstSurvivorFirstPerk != null)
                {
                    _selectedFirstSurvivorFirstPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedFirstSurvivorSecondPerk;
        public SurvivorPerk SelectedFirstSurvivorSecondPerk
        {
            get => _selectedFirstSurvivorSecondPerk;
            set
            {
                if (_selectedFirstSurvivorSecondPerk != null)
                {
                    _selectedFirstSurvivorSecondPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedFirstSurvivorThirdPerk;
        public SurvivorPerk SelectedFirstSurvivorThirdPerk
        {
            get => _selectedFirstSurvivorThirdPerk;
            set
            {
                if (_selectedFirstSurvivorThirdPerk != null)
                {
                    _selectedFirstSurvivorThirdPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedFirstSurvivorFourthPerk;
        public SurvivorPerk SelectedFirstSurvivorFourthPerk
        {
            get => _selectedFirstSurvivorFourthPerk;
            set
            {
                if (_selectedFirstSurvivorFourthPerk != null)
                {
                    _selectedFirstSurvivorFourthPerk = value;
                }
            }
        }

        private Item _selectedFirstSurvivorItem;
        public Item SelectedFirstSurvivorItem
        {
            get => _selectedFirstSurvivorItem;
            set
            {
                if (_selectedFirstSurvivorItem != null)
                {
                    _selectedFirstSurvivorItem = value;
                }
            }
        }

        private ItemAddon _selectedFirstSurvivorFirstItemAddon;
        public ItemAddon SelectedFirstSurvivorFirstItemAddon
        {
            get => _selectedFirstSurvivorFirstItemAddon;
            set
            {
                if (_selectedFirstSurvivorFirstItemAddon != null)
                {
                    _selectedFirstSurvivorFirstItemAddon = value;
                }
            }
        }

        private ItemAddon _selectedFirstSurvivorSecondItemAddon;
        public ItemAddon SelectedFirstSurvivorSecondItemAddon
        {
            get => _selectedFirstSurvivorSecondItemAddon;
            set
            {
                if (_selectedFirstSurvivorSecondItemAddon != null)
                {
                    _selectedFirstSurvivorSecondItemAddon = value;
                }
            }
        }

        private Offering _selectedFirstSurvivorOffering;
        public Offering SelectedFirstSurvivorOffering
        {
            get => _selectedFirstSurvivorOffering;
            set
            {
                if (_selectedFirstSurvivorOffering != null)
                {
                    _selectedFirstSurvivorOffering = value;
                }
            }
        }

        private PlayerAssociation _selectedFirstSurvivorPlayerAssociation;
        public PlayerAssociation SelectedFirstSurvivorPlayerAssociation
        {
            get => _selectedFirstSurvivorPlayerAssociation;
            set
            {
                if (_selectedFirstSurvivorPlayerAssociation != null)
                {
                    _selectedFirstSurvivorPlayerAssociation = value;
                }
            }
        }

        private int _firstSurvivorPrestige;
        public int FirstSurvivorPrestige
        {
            get => _firstSurvivorPrestige;
            set
            {
                _firstSurvivorPrestige = value;
            }
        }

        private int _firstSurvivorAccount;
        public int FirstSurvivorAccount
        {
            get => _firstSurvivorAccount;
            set
            {
                _firstSurvivorAccount = value;
            }
        }

        private bool _firstSurvivorBot;
        public bool FirstSurvivorBot
        {
            get => _firstSurvivorBot;
            set
            {
                _firstSurvivorBot = value;
            }
        }

        private bool _firstSurvivorAnonymousMode;
        public bool FirstSurvivorAnonymousMode
        {
            get => _firstSurvivorAnonymousMode;
            set
            {
                _firstSurvivorAnonymousMode = value;
            }
        }

        #endregion

        #region Свойства для второго вижившего      

        private Survivor _selectedSecondSurvivor;
        public Survivor SelectedSecondSurvivor
        {
            get => _selectedSecondSurvivor;
            set
            {
                if (_selectedSecondSurvivor != null)
                {
                    _selectedSecondSurvivor = value;
                }
            }
        }

        private Platform _selectedSecondSurvivorPlatform;
        public Platform SelectedSecondSurvivorPlatform
        {
            get => _selectedSecondSurvivorPlatform;
            set
            {
                if (_selectedSecondSurvivorPlatform != null)
                {
                    _selectedSecondSurvivorPlatform = value;
                }
            }
        }

        private TypeDeath _selectedSecondSurvivorTypeDeath;
        public TypeDeath SelectedSecondSurvivorTypeDeath
        {
            get => _selectedSecondSurvivorTypeDeath;
            set
            {
                if (_selectedSecondSurvivorTypeDeath != null)
                {
                    _selectedSecondSurvivorTypeDeath = value;
                }
            }
        }

        private SurvivorPerk _selectedSecondSurvivorFirstPerk;
        public SurvivorPerk SelectedSecondSurvivorFirstPerk
        {
            get => _selectedSecondSurvivorFirstPerk;
            set
            {
                if (_selectedSecondSurvivorFirstPerk != null)
                {
                    _selectedSecondSurvivorFirstPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedSecondSurvivorSecondPerk;
        public SurvivorPerk SelectedSecondSurvivorSecondPerk
        {
            get => _selectedSecondSurvivorSecondPerk;
            set
            {
                if (_selectedSecondSurvivorSecondPerk != null)
                {
                    _selectedSecondSurvivorSecondPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedSecondSurvivorThirdPerk;
        public SurvivorPerk SelectedSecondSurvivorThirdPerk
        {
            get => _selectedSecondSurvivorThirdPerk;
            set
            {
                if (_selectedSecondSurvivorThirdPerk != null)
                {
                    _selectedSecondSurvivorThirdPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedSecondSurvivorFourthPerk;
        public SurvivorPerk SelectedSecondSurvivorFourthPerk
        {
            get => _selectedSecondSurvivorFourthPerk;
            set
            {
                if (_selectedSecondSurvivorFourthPerk != null)
                {
                    _selectedSecondSurvivorFourthPerk = value;
                }
            }
        }

        private Item _selectedSecondSurvivorItem;
        public Item SelectedSecondSurvivorItem
        {
            get => _selectedSecondSurvivorItem;
            set
            {
                if (_selectedSecondSurvivorItem != null)
                {
                    _selectedSecondSurvivorItem = value;
                }
            }
        }

        private ItemAddon _selectedSecondSurvivorFirstItemAddon;
        public ItemAddon SelectedSecondSurvivorFirstItemAddon
        {
            get => _selectedSecondSurvivorFirstItemAddon;
            set
            {
                if (_selectedSecondSurvivorFirstItemAddon != null)
                {
                    _selectedSecondSurvivorFirstItemAddon = value;
                }
            }
        }

        private ItemAddon _selectedSecondSurvivorSecondItemAddon;
        public ItemAddon SelectedSecondSurvivorSecondItemAddon
        {
            get => _selectedSecondSurvivorSecondItemAddon;
            set
            {
                if (_selectedSecondSurvivorSecondItemAddon != null)
                {
                    _selectedSecondSurvivorSecondItemAddon = value;
                }
            }
        }

        private Offering _selectedSecondSurvivorOffering;
        public Offering SelectedSecondSurvivorOffering
        {
            get => _selectedSecondSurvivorOffering;
            set
            {
                if (_selectedSecondSurvivorOffering != null)
                {
                    _selectedSecondSurvivorOffering = value;
                }
            }
        }

        private PlayerAssociation _selectedSecondSurvivorPlayerAssociation;
        public PlayerAssociation SelectedSecondSurvivorPlayerAssociation
        {
            get => _selectedSecondSurvivorPlayerAssociation;
            set
            {
                if (_selectedSecondSurvivorPlayerAssociation != null)
                {
                    _selectedSecondSurvivorPlayerAssociation = value;
                }
            }
        }

        private int _secondSurvivorPrestige;
        public int SecondSurvivorPrestige
        {
            get => _secondSurvivorPrestige;
            set
            {
                _secondSurvivorPrestige = value;
            }
        }

        private int _secondSurvivorAccount;
        public int SecondSurvivorAccount
        {
            get => _secondSurvivorAccount;
            set
            {
                _secondSurvivorAccount = value;
            }
        }

        private bool _secondSurvivorBot;
        public bool SecondSurvivorBot
        {
            get => _secondSurvivorBot;
            set
            {
                _secondSurvivorBot = value;
            }
        }

        private bool _secondSurvivorAnonymousMode;
        public bool SecondSurvivorAnonymousMode
        {
            get => _secondSurvivorAnonymousMode;
            set
            {
                _secondSurvivorAnonymousMode = value;
            }
        }

        #endregion

        #region Свойства для третьего вижившего

        private Survivor _selectedThirdSurvivor;
        public Survivor SelectedThirdSurvivor
        {
            get => _selectedThirdSurvivor;
            set
            {
                if (_selectedThirdSurvivor != null)
                {
                    _selectedThirdSurvivor = value;
                }
            }
        }

        private Platform _selectedThirdSurvivorPlatform;
        public Platform SelectedThirdSurvivorPlatform
        {
            get => _selectedThirdSurvivorPlatform;
            set
            {
                if (_selectedThirdSurvivorPlatform != null)
                {
                    _selectedThirdSurvivorPlatform = value;
                }
            }
        }

        private TypeDeath _selectedThirdSurvivorTypeDeath;
        public TypeDeath SelectedThirdSurvivorTypeDeath
        {
            get => _selectedThirdSurvivorTypeDeath;
            set
            {
                if (_selectedThirdSurvivorTypeDeath != null)
                {
                    _selectedThirdSurvivorTypeDeath = value;
                }
            }
        }

        private SurvivorPerk _selectedThirdSurvivorFirstPerk;
        public SurvivorPerk SelectedThirdSurvivorFirstPerk
        {
            get => _selectedThirdSurvivorFirstPerk;
            set
            {
                if (_selectedThirdSurvivorFirstPerk != null)
                {
                    _selectedThirdSurvivorFirstPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedThirdSurvivorSecondPerk;
        public SurvivorPerk SelectedThirdSurvivorSecondPerk
        {
            get => _selectedThirdSurvivorSecondPerk;
            set
            {
                if (_selectedThirdSurvivorSecondPerk != null)
                {
                    _selectedThirdSurvivorSecondPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedThirdSurvivorThirdPerk;
        public SurvivorPerk SelectedThirdSurvivorThirdPerk
        {
            get => _selectedThirdSurvivorThirdPerk;
            set
            {
                if (_selectedThirdSurvivorThirdPerk != null)
                {
                    _selectedThirdSurvivorThirdPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedThirdSurvivorFourthPerk;
        public SurvivorPerk SelectedThirdSurvivorFourthPerk
        {
            get => _selectedThirdSurvivorFourthPerk;
            set
            {
                if (_selectedThirdSurvivorFourthPerk != null)
                {
                    _selectedThirdSurvivorFourthPerk = value;
                }
            }
        }

        private Item _selectedThirdSurvivorItem;
        public Item SelectedThirdSurvivorItem
        {
            get => _selectedThirdSurvivorItem;
            set
            {
                if (_selectedThirdSurvivorItem != null)
                {
                    _selectedThirdSurvivorItem = value;
                }
            }
        }

        private ItemAddon _selectedThirdSurvivorFirstItemAddon;
        public ItemAddon SelectedThirdSurvivorFirstItemAddon
        {
            get => _selectedThirdSurvivorFirstItemAddon;
            set
            {
                if (_selectedThirdSurvivorFirstItemAddon != null)
                {
                    _selectedThirdSurvivorFirstItemAddon = value;
                }
            }
        }

        private ItemAddon _selectedThirdSurvivorSecondItemAddon;
        public ItemAddon SelectedThirdSurvivorSecondItemAddon
        {
            get => _selectedThirdSurvivorSecondItemAddon;
            set
            {
                if (_selectedThirdSurvivorSecondItemAddon != null)
                {
                    _selectedThirdSurvivorSecondItemAddon = value;
                }
            }
        }

        private Offering _selectedThirdSurvivorOffering;
        public Offering SelectedThirdSurvivorOffering
        {
            get => _selectedThirdSurvivorOffering;
            set
            {
                if (_selectedThirdSurvivorOffering != null)
                {
                    _selectedThirdSurvivorOffering = value;
                }
            }
        }

        private PlayerAssociation _selectedThirdSurvivorPlayerAssociation;
        public PlayerAssociation SelectedThirdSurvivorPlayerAssociation
        {
            get => _selectedThirdSurvivorPlayerAssociation;
            set
            {
                if (_selectedThirdSurvivorPlayerAssociation != null)
                {
                    _selectedThirdSurvivorPlayerAssociation = value;
                }
            }
        }

        private int _thirdSurvivorPrestige;
        public int ThirdSurvivorPrestige
        {
            get => _thirdSurvivorPrestige;
            set
            {
                _thirdSurvivorPrestige = value;
            }
        }

        private int _thirdSurvivorAccount;
        public int ThirdSurvivorAccount
        {
            get => _thirdSurvivorAccount;
            set
            {
                _thirdSurvivorAccount = value;
            }
        }

        private bool _thirdSurvivorBot;
        public bool ThirdSurvivorBot
        {
            get => _thirdSurvivorBot;
            set
            {
                _thirdSurvivorBot = value;
            }
        }

        private bool _thirdSurvivorAnonymousMode;
        public bool ThirdSurvivorAnonymousMode
        {
            get => _thirdSurvivorAnonymousMode;
            set
            {
                _thirdSurvivorAnonymousMode = value;
            }
        }

        #endregion

        #region Свойства для четвертого вижившего

        private Survivor _selectedFourthSurvivor;
        public Survivor SelectedFourthSurvivor
        {
            get => _selectedFourthSurvivor;
            set
            {
                if (_selectedFourthSurvivor != null)
                {
                    _selectedFourthSurvivor = value;
                }
            }
        }

        private Platform _selectedFourthSurvivorPlatform;
        public Platform SelectedFourthSurvivorPlatform
        {
            get => _selectedFourthSurvivorPlatform;
            set
            {
                if (_selectedFourthSurvivorPlatform != null)
                {
                    _selectedFourthSurvivorPlatform = value;
                }
            }
        }

        private TypeDeath _selectedFourthSurvivorTypeDeath;
        public TypeDeath SelectedFourthSurvivorTypeDeath
        {
            get => _selectedFourthSurvivorTypeDeath;
            set
            {
                if (_selectedFourthSurvivorTypeDeath != null)
                {
                    _selectedFourthSurvivorTypeDeath = value;
                }
            }
        }

        private SurvivorPerk _selectedFourthSurvivorFirstPerk;
        public SurvivorPerk SelectedFourthSurvivorFirstPerk
        {
            get => _selectedFourthSurvivorFirstPerk;
            set
            {
                if (_selectedFourthSurvivorFirstPerk != null)
                {
                    _selectedFourthSurvivorFirstPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedFourthSurvivorSecondPerk;
        public SurvivorPerk SelectedFourthSurvivorSecondPerk
        {
            get => _selectedFourthSurvivorSecondPerk;
            set
            {
                if (_selectedFourthSurvivorSecondPerk != null)
                {
                    _selectedFourthSurvivorSecondPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedFourthSurvivorThirdPerk;
        public SurvivorPerk SelectedFourthSurvivorThirdPerk
        {
            get => _selectedFourthSurvivorThirdPerk;
            set
            {
                if (_selectedFourthSurvivorThirdPerk != null)
                {
                    _selectedFourthSurvivorThirdPerk = value;
                }
            }
        }

        private SurvivorPerk _selectedFourthSurvivorFourthPerk;
        public SurvivorPerk SelectedFourthSurvivorFourthPerk
        {
            get => _selectedFourthSurvivorFourthPerk;
            set
            {
                if (_selectedFourthSurvivorFourthPerk != null)
                {
                    _selectedFourthSurvivorFourthPerk = value;
                }
            }
        }

        private Item _selectedFourthSurvivorItem;
        public Item SelectedFourthSurvivorItem
        {
            get => _selectedFourthSurvivorItem;
            set
            {
                if (_selectedFourthSurvivorItem != null)
                {
                    _selectedFourthSurvivorItem = value;
                }
            }
        }

        private ItemAddon _selectedFourthSurvivorFirstItemAddon;
        public ItemAddon SelectedFourthSurvivorFirstItemAddon
        {
            get => _selectedFourthSurvivorFirstItemAddon;
            set
            {
                if (_selectedFourthSurvivorFirstItemAddon != null)
                {
                    _selectedFourthSurvivorFirstItemAddon = value;
                }
            }
        }

        private ItemAddon _selectedFourthSurvivorSecondItemAddon;
        public ItemAddon SelectedFourthSurvivorSecondItemAddon
        {
            get => _selectedFourthSurvivorSecondItemAddon;
            set
            {
                if (_selectedFourthSurvivorSecondItemAddon != null)
                {
                    _selectedFourthSurvivorSecondItemAddon = value;
                }
            }
        }

        private Offering _selectedFourthSurvivorOffering;
        public Offering SelectedFourthSurvivorOffering
        {
            get => _selectedFourthSurvivorOffering;
            set
            {
                if (_selectedFourthSurvivorOffering != null)
                {
                    _selectedFourthSurvivorOffering = value;
                }
            }
        }

        private PlayerAssociation _selectedFourthSurvivorPlayerAssociation;
        public PlayerAssociation SelectedFourthSurvivorPlayerAssociation
        {
            get => _selectedFourthSurvivorPlayerAssociation;
            set
            {
                if (_selectedFourthSurvivorPlayerAssociation != null)
                {
                    _selectedFourthSurvivorPlayerAssociation = value;
                }
            }
        }

        private int _fourthSurvivorPrestige;
        public int FourthSurvivorPrestige
        {
            get => _fourthSurvivorPrestige;
            set
            {
                _fourthSurvivorPrestige = value;
            }
        }

        private int _fourthSurvivorAccount;
        public int FourthSurvivorAccount
        {
            get => _fourthSurvivorAccount;
            set
            {
                _fourthSurvivorAccount = value;
            }
        }

        private bool _fourthSurvivorBot;
        public bool FourthSurvivorBot
        {
            get => _fourthSurvivorBot;
            set
            {
                _fourthSurvivorBot = value;
            }
        }

        private bool _fourthSurvivorAnonymousMode;
        public bool FourthSurvivorAnonymousMode
        {
            get => _fourthSurvivorAnonymousMode;
            set
            {
                _fourthSurvivorAnonymousMode = value;
            }
        }

        #endregion

        #region Общие свойства

        private DateTime _selectedDateTimeMatch;
        public DateTime SelectedDateTimeMatch
        {
            get => _selectedDateTimeMatch;
            set
            {
                _selectedDateTimeMatch = value;
            }
        }

        private DateTime _selectedGameTimeMatch;
        public DateTime SelectedGameTimeMatch
        {
            get => _selectedGameTimeMatch;
            set
            {
                _selectedGameTimeMatch = value;
            }
        }

        private int _countKills;
        public int CountKills
        {
            get => _countKills;
            set
            {
                _countKills = value;
            }
        }

        private int _countHooks;
        public int CountHooks
        {
            get => _countHooks;
            set
            {
                _countHooks = value;
            }
        }

        private int _countNumberRecentGenerators;
        public int CountNumberRecentGenerators
        {
            get => _countNumberRecentGenerators;
            set
            {
                _countNumberRecentGenerators = value;
            }
        }

        private Patch _selectedPatch;
        public Patch SelectedPatch
        {
            get => _selectedPatch;
            set
            {
                if (_selectedPatch != null)
                {
                    _selectedPatch = value;
                }              
            }
        }

        private GameMode _selectedGameMode;
        public GameMode SelectedGameMode
        {
            get => _selectedGameMode;
            set
            {
                if (_selectedGameMode != null)
                {
                    _selectedGameMode = value;
                }
            }
        }

        private GameEvent _selectedGameEvent;
        public GameEvent SelectedGameEvent
        {
            get => _selectedGameEvent;
            set
            {
                if (_selectedGameEvent != null)
                {
                    _selectedGameEvent = value;
                }
            }
        }

        private WhoPlacedMap _selectedWhoPlacedMap;
        public WhoPlacedMap SelectedWhoPlacedMap
        {
            get => _selectedWhoPlacedMap;
            set
            {
                if (_selectedWhoPlacedMap != null)
                {
                    _selectedWhoPlacedMap = value;
                }
            }
        }

        private WhoPlacedMap _selectedWhoPlacedMapWin;
        public WhoPlacedMap SelectedWhoPlacedMapWin
        {
            get => _selectedWhoPlacedMapWin;
            set
            {
                if (_selectedWhoPlacedMapWin != null)
                {
                    _selectedWhoPlacedMapWin = value;
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
            }
        }

        #endregion

        #region Свойства изображений

        public ObservableCollection<ImageInformation> CompressedImagesList { get; set; } = [];

        public ObservableCollection<string> NormalImagesList { get; set; } = [];

        #endregion

        #region Общие свойства

        public string Title { get; set; } = "Добавить запись";

        #endregion

        public AddMatchWindowViewModel()
        {
            DeclareCollections();
            GetAllData();
        }

        #region Команды

        #endregion

        #region Методы получение данных  - заполнение списков

        private void DeclareCollections()
        {
            GameModeList = [];
            GameEventList = [];
            MapList = [];
            OfferingList = [];
            PatchList = [];
            PlatformList = [];
            PlayerAssociationList = [];
            RoleList = [];
            WhoPlacedMapList = [];

            KillerList = [];
            KillerAddonList = [];
            KillerPerkList = [];
            KillerInfoList = [];

            SurvivorList = [];
            SurvivorPerkList = [];
            SurvivorInfoList = [];
            TypeDeathList = [];
            ItemList = [];
            ItemAddonList = [];
        }

        private void GetAllData()
        {
            GetGameModeListData();
            GetGameEventListData();
            GetMapListData();
            GetOfferingListData();
            GetPatchListData();
            GetPlatformListData();
            GetPlayerAssociationListData();
            GetRoleListData();
            GetWhoPlacedMapListData();

            GetKillerListData();
            GetKillerAddonListData();
            GetKillerPerkListData();
            GetKillerInfoListData();

            GetSurvivorListData();
            GetSurvivorPerkListData();
            GetSurvivorInfoListData();
            GetTypeDeathListData();
            GetItemListData();
            GetItemAddonListData();
        }

        private void GetGameModeListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.GameModes.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            GameModeList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetGameEventListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.GameEvents.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            GameEventList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetMapListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.Maps.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            MapList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetOfferingListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.Offerings.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            OfferingList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetPatchListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.Patches.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            PatchList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetPlatformListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.Platforms.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            PlatformList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetPlayerAssociationListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.PlayerAssociations.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            PlayerAssociationList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetRoleListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.Roles.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            RoleList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetWhoPlacedMapListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.WhoPlacedMaps.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            WhoPlacedMapList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetKillerListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.Killers.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            KillerList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetKillerAddonListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.KillerAddons.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            KillerAddonList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetKillerPerkListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.KillerPerks.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            KillerPerkList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetKillerInfoListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.KillerInfos.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            KillerInfoList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetSurvivorListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.Survivors.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            SurvivorList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetSurvivorPerkListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.SurvivorPerks.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            SurvivorPerkList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetSurvivorInfoListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.SurvivorInfos.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            SurvivorInfoList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetTypeDeathListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.TypeDeaths.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            TypeDeathList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetItemListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var entities = context.Items.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            ItemList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetItemAddonListData()
        {

            new Thread(() =>
            {
                using (var context = new MasterAnalyticsDeadByDaylightDbContext())
                {
                    var entities = context.ItemAddons.ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var entity in entities)
                        {
                            ItemAddonList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        #endregion 

        ///// <summary>
        ///// Загрузка сжатых изображений из папки "thumbnails", которая находится в основной папки с скриншотами игры. 
        ///// </summary>
        ///// <returns></returns>
        //private async Task UploadCompressedImages()
        //{
        //    string[] compressedImagePaths = Directory.GetFiles(@"D:\Steam\userdata\189964443\760\remote\381210\screenshots\thumbnails");

        //    List<Task<ImageInformation>> tasks = new List<Task<ImageInformation>>();

        //    foreach (var path in compressedImagePaths)
        //    {
        //        tasks.Add(Task.Run(() => GetImageInformation(path)));
        //    }

        //    var imageInfoList = await Task.WhenAll(tasks);
        //    foreach (var item in imageInfoList)
        //    {
        //        CompressedImagesList.Add(item);
        //    }

        //    UploadNormalImage();
        //}

        //private Task<BitmapImage> RemoveHalfPixel(string path)
        //{
        //    //Получить путь к изображению
        //    //Проверить является ли файл изображением
        //    //Превратить его в Bitmap
        //    Bitmap bitmap = new(path);
        //    //Сжать в меньший размер( в 2-4 раза)
        //    //Вернуть изображение в формате BitmapImage

        //    BitmapImage bitmapImage = new();

        //    return Task.Run(() => 
        //    {
        //        return bitmapImage;
        //    });
        //}

        ///// <summary>
        ///// Получение информации о изображение.
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //private ImageInformation GetImageInformation(string path)
        //{
        //    try
        //    {   
        //        using (FileStream fs = File.OpenRead(path))
        //        {
        //            return new ImageInformation
        //            {
        //                PathImage = path,
        //                FileName = Path.GetFileName(path),
        //                //Image = bitmap
        //            };                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка загрузки: {path}. Ошибка: {ex.Message}");
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Создание списка с изображениями нормального разрешение. Которые находятся в основной папке с скриншотами.
        ///// </summary>
        //private void UploadNormalImage()
        //{
        //    string[] NormalImagePaths = Directory.GetFiles(@"D:\Steam\userdata\189964443\760\remote\381210\screenshots");

        //    foreach (var item in NormalImagePaths)
        //    {
        //        NormalImagesList.Add(item);
        //    }
        //}

        //private RelayCommand _loadImageCommand;
        //public RelayCommand LoadImageCommand { get => _loadImageCommand ??= new(async obj => await UploadCompressedImages()); }

    }
}
