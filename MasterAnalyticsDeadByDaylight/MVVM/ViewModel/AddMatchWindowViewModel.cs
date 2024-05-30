using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows;

using System.Windows.Media.Imaging;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    public class AddMatchWindowViewModel : BaseViewModel
    {
        #region Блок "Коллекции данных из БД"

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

        public ObservableCollection<Offering> SurvivorOfferingList { get; set; }

        #endregion

        #region Общие коллекции 

        public ObservableCollection<GameMode> GameModeList { get; set; }

        public ObservableCollection<GameEvent> GameEventList { get; set; }

        public ObservableCollection<Map> MapList { get; set; }

        public ObservableCollection<Offering> KillerOfferingList { get; set; }

        public ObservableCollection<Patch> PatchList { get; set; }

        public ObservableCollection<Platform> PlatformList { get; set; }

        public ObservableCollection<PlayerAssociation> PlayerAssociationList { get; set; }

        public ObservableCollection<Role> RoleList { get; set; }

        public ObservableCollection<WhoPlacedMap> WhoPlacedMapList { get; set; }

        #endregion

        #endregion

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
                if (value == null) { return; }
                _selectedKiller = value;
                GetKillerAddonListData();
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
                if (_selectedKillerPlatform != null)
                {
                    _selectedKillerPlatform = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Престиж "Убийцы"
        /// </summary> 
        private int _killerInfoPrestige;
        public int KillerInfoPrestige
        {
            get => _killerInfoPrestige;
            set
            {
                if (value >= 100 | value < 0)
                {
                    MessageBox.Show("Престиже меньше 0 и выше 100 не бывает!");
                }
                else
                {
                    _killerInfoPrestige = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Счет очков "Убийцы" по окончанию игры
        /// </summary>
        private int _killerInfoAccount;
        public int KillerInfoAccount
        {
            get => _killerInfoAccount;
            set
            {
                _killerInfoAccount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Какой режим приватности был у "Убийцы"? Анонимный или нет
        /// </summary>
        private bool _killerInfoAnonymousMode;
        public bool KillerInfoAnonymousMode
        {
            get => _killerInfoAnonymousMode;
            set
            {
                _killerInfoAnonymousMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вышел ли убийца до окончания игры или нет
        /// </summary>
        private bool _killerInfoBot;
        public bool KillerInfoBot
        {
            get => _killerInfoBot;
            set
            {
                _killerInfoBot = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Билд - Перки - Свойства

        /// <summary>
        /// Свойство для отслеживание выбранного перка "Убийцы" из списка (ListView)
        /// </summary>
        private KillerPerk _selectedListViewKillerPerk;
        public KillerPerk SelectedListViewKillerPerk
        {
            get => _selectedListViewKillerPerk;
            set
            {
                _selectedListViewKillerPerk = value;
                OnPropertyChanged();
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
                if (value == null) { return; }
                _selectedKillerFirstPerk = value;
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
                if (value == null) { return; }
                _selectedKillerSecondPerk = value;
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
                if (value == null) { return; }
                _selectedKillerThirdPerk = value;
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
                if (value == null) { return; }
                _selectedKillerFourthPerk = value;
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
        private string _textBoxSearchKillerPerk;
        public string TextBoxSearchKillerPerk
        {
            get => _textBoxSearchKillerPerk;
            set
            {
                _textBoxSearchKillerPerk = value;
                SearchKillerPerk();
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
                SelectedKillerFirstPerk = SelectedListViewKillerPerk;
                SelectedKillerFirstPerkName = SelectedListViewKillerPerk.PerkName;
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
                SelectedKillerSecondPerk = SelectedListViewKillerPerk;
                SelectedKillerSecondPerkName = SelectedListViewKillerPerk.PerkName;
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
                SelectedKillerThirdPerk = SelectedListViewKillerPerk;
                SelectedKillerThirdPerkName = SelectedListViewKillerPerk.PerkName;
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
                SelectedKillerFourthPerk = SelectedListViewKillerPerk;
                SelectedKillerFourthPerkName = SelectedListViewKillerPerk.PerkName;
            });
        }

        #endregion

        #region Билд - Аддоны - Свойства

        /// <summary>
        /// Свойство для отслеживание выбранного улучшения "Убийцы" из списка (ListView)
        /// </summary>
        private KillerAddon _selectedListViewKillerAddon;
        public KillerAddon SelectedListViewKillerAddon
        {
            get => _selectedListViewKillerAddon;
            set
            {
                _selectedListViewKillerAddon = value;
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
                _selectedKillerFirstAddon = value;
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
                _selectedKillerSecondAddon = value;
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
        private string _textBoxSearchKillerAddon;
        public string TextBoxSearchKillerAddon
        {
            get => _textBoxSearchKillerAddon;
            set
            {
                _textBoxSearchKillerAddon = value;
                SearchKillerAddon();
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
        public RelayCommand SelectedKillerFirstAddonCommand
        {
            get => _selectedKillerFirstAddonCommand ??= new(obj =>
            {
                SelectedKillerFirstAddon = SelectedListViewKillerAddon;
                SelectedKillerFirstAddonName = SelectedListViewKillerAddon.AddonName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView KillerPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedKillerSecondAddonCommand;
        public RelayCommand SelectedKillerSecondAddonCommand
        {
            get => _selectedKillerSecondAddonCommand ??= new(obj =>
            {
                SelectedKillerSecondAddon = SelectedListViewKillerAddon;
                SelectedKillerSecondAddonName = SelectedListViewKillerAddon.AddonName;
            });
        }

        #endregion

        #region Билд - Подношения Свойства

        /// <summary>
        /// Свойство для отслеживание выбранного подношение "Убийцы" из списка (ListView)
        /// </summary>
        private Offering _selectedListViewKillerOffering;
        public Offering SelectedListViewKillerOffering
        {
            get => _selectedListViewKillerOffering;
            set
            {
                if (value == null) { return; }
                _selectedListViewKillerOffering = value;
                SelectedKillerOfferingName = value.OfferingName;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойства для поиска по списку подношений (ListView)
        /// </summary>
        private string _textBoxSearchKillerOffering;
        public string TextBoxSearchKillerOffering
        {
            get => _textBoxSearchKillerOffering;
            set
            {
                _textBoxSearchKillerOffering = value;
                SearchKillerOffering();
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

        // <summary>
        /// Выбор роли, по которой в ListView будут загружены соответствующие подношение
        /// </summary>
        private Role _selectedRoleKiller;
        public Role SelectedRoleKiller
        {
            get => _selectedRoleKiller;
            set
            {
                _selectedRoleKiller = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о выбранной роли. Нужно для того, что б загрузить нужные список подношений
        /// </summary>
        private Role _selectedComboBoxRoleKiller;
        public Role SelectedComboBoxRoleKiller
        {
            get => _selectedComboBoxRoleKiller;
            set
            {
                _selectedComboBoxRoleKiller = value;
                GetKillerOfferingListData();
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
                if (value == null) { return; }
                _selectedKillerPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Блок "Выжившие"

        #region Общие свойства для отслеживание выбранных элементов в ListView

        /// <summary>
        /// Свойство для отслеживание первого выбранного выжившего
        /// </summary>
        private Survivor _selectedFirstSurvivor;
        public Survivor SelectedFirstSurvivor
        {
            get => _selectedFirstSurvivor;
            set
            {
                if (value == null) { return; }
                _selectedFirstSurvivor = value;
            }
        }

        /// <summary>
        /// Свойство для отслеживание выбранного перка выжившего из списка (ListView)
        /// </summary>
        private SurvivorPerk _selectedListViewSurvivorPerk;
        public SurvivorPerk SelectedListViewSurvivorPerk
        {
            get => _selectedListViewSurvivorPerk;
            set
            {
                if (value == null) { return; }
                _selectedListViewSurvivorPerk = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку перков выжившего
        /// </summary
        private string _textBoxSearchSurvivorPerk;
        public string TextBoxSearchSurvivorPerk
        {
            get => _textBoxSearchSurvivorPerk;
            set
            {
                _textBoxSearchSurvivorPerk = value;
                SearchSurvivorPerk();
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Свойство для хранение информации о выбранном улучшение предмета для ListView ItemAddon
        /// </summary>
        private ItemAddon _selectedListViewItemAddon;
        public ItemAddon SelectedListViewItemAddon
        {
            get => _selectedListViewItemAddon;
            set
            {
                if (value == null) { return; }
                _selectedListViewItemAddon = value;
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений для предмета у первого выжившего
        /// </summary
        private string _textBoxSearchFirstSurvivorItemAddon;
        public string TextBoxSearchFirstSurvivorItemAddon
        {
            get => _textBoxSearchFirstSurvivorItemAddon;
            set
            {
                _textBoxSearchFirstSurvivorItemAddon = value;
                ItemAddonList.Clear();
                SearchFirstSurvivorItemAddon();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений для предмета у второго выжившего
        /// </summary
        private string _textBoxSearchSecondSurvivorItemAddon;
        public string TextBoxSearchSecondSurvivorItemAddon
        {
            get => _textBoxSearchSecondSurvivorItemAddon;
            set
            {
                _textBoxSearchSecondSurvivorItemAddon = value;
                ItemAddonList.Clear();
                SearchSecondSurvivorItemAddon();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений для предмета у третьего выжившего
        /// </summary
        private string _textBoxSearchThirdSurvivorItemAddon;
        public string TextBoxSearchThirdSurvivorItemAddon
        {
            get => _textBoxSearchThirdSurvivorItemAddon;
            set
            {
                _textBoxSearchThirdSurvivorItemAddon = value;
                ItemAddonList.Clear();
                SearchThirdSurvivorItemAddon();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для поиска по списку улучшений для предмета у четвертого выжившего
        /// </summary
        private string _textBoxSearchFourthSurvivorItemAddon;
        public string TextBoxSearchFourthSurvivorItemAddon
        {
            get => _textBoxSearchFourthSurvivorItemAddon;
            set
            {
                _textBoxSearchFourthSurvivorItemAddon = value;
                ItemAddonList.Clear();
                SearchFourthSurvivorItemAddon();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Свойство для хранение информации о выбранной роли. Нужно для того, что б загрузить нужные список подношений
        /// </summary>
        private Role _selectedComboBoxRoleSurvivor;
        public Role SelectedComboBoxRoleSurvivor
        {
            get => _selectedComboBoxRoleSurvivor;
            set
            {
                _selectedComboBoxRoleSurvivor = value;
                GetSurvivorOfferingListData();
                OnPropertyChanged();

            }
        }

        /// <summary>
        /// Свойства для поиска по списку подношений (ListView)
        /// </summary>
        private string _textBoxSearchSurvivorOffering;
        public string TextBoxSearchSurvivorOffering
        {
            get => _textBoxSearchSurvivorOffering;
            set
            {
                _textBoxSearchSurvivorOffering = value;
                SearchSurvivorOffering();
                OnPropertyChanged();
            }
        }


        #endregion

        #region Основная информация Выжившего №1

        /// <summary>
        /// Отслеживание платформы на которой играет первый выживший 
        /// </summary>
        private Platform _selectedFirstSurvivorPlatform;
        public Platform SelectedFirstSurvivorPlatform
        {
            get => _selectedFirstSurvivorPlatform;
            set
            {
                if (value == null) { return; }
                _selectedFirstSurvivorPlatform = value;
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
                _firstSurvivorPrestige = value;
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
                if (value == null) { return; }
                _selectedFirstSurvivorTypeDeath = value;
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
                if (value == null) { return; }
                _selectedFirstSurvivorFirstPerk = value;
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
                if (value == null) { return; }
                _selectedFirstSurvivorSecondPerk = value;
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
                if (value == null) { return; }
                _selectedFirstSurvivorThirdPerk = value;
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
                if (value == null) { return; }
                _selectedFirstSurvivorFourthPerk = value;
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
        {
            get => _selectedFirstSurvivorFirstPerkCommand ??= new(obj =>
            {
                SelectedFirstSurvivorFirstPerk = SelectedListViewSurvivorPerk;
                SelectedFirstSurvivorFirstPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorSecondPerkCommand;
        public RelayCommand SelectedFirstSurvivorSecondPerkCommand
        {
            get => _selectedFirstSurvivorSecondPerkCommand ??= new(obj =>
            {
                SelectedFirstSurvivorSecondPerk = SelectedListViewSurvivorPerk;
                SelectedFirstSurvivorSecondPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 3 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorThirdPerkCommand;
        public RelayCommand SelectedFirstSurvivorThirdPerkCommand
        {
            get => _selectedFirstSurvivorThirdPerkCommand ??= new(obj =>
            {
                SelectedFirstSurvivorThirdPerk = SelectedListViewSurvivorPerk;
                SelectedFirstSurvivorThirdPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 4 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorFourthPerkCommand;
        public RelayCommand SelectedFirstSurvivorFourthPerkCommand
        {
            get => _selectedFirstSurvivorFourthPerkCommand ??= new(obj =>
            {
                SelectedFirstSurvivorFourthPerk = SelectedListViewSurvivorPerk;
                SelectedFirstSurvivorFourthPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }


        #endregion

        #region Билд - Предметы Выжившего №1 - Свойства

        /// <summary>
        /// Свойство для выбора предмета у первого выжившего
        /// </summary>
        private Item _selectedComboBoxFirstSurvivorItem;
        public Item SelectedComboBoxFirstSurvivorItem
        {
            get => _selectedComboBoxFirstSurvivorItem;
            set
            {
                if (value == null) { return; }
                _selectedComboBoxFirstSurvivorItem = value;
                GetFirstSurvivorItemAddonListData();
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
                if (value == null) { return; }
                _selectedFirstSurvivorFirstItemAddon = value;
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
                if (value == null) { return; }
                _selectedFirstSurvivorSecondItemAddon = value;
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

        #endregion

        #region Билд - Предметы Выжившего №1 - Команды

        /// <summary>
        /// Команда выбора улучшение предмета в 1 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedListViewSurvivorFirstItemAddonCommand
        {
            get => _selectedFirstSurvivorFirstItemAddonCommand ??= new(obj =>
            {
                SelectedFirstSurvivorFirstItemAddon = SelectedListViewItemAddon;
                SelectedFirstSurvivorFirstItemAddonName = SelectedListViewItemAddon.ItemAddonName;
            });
        }

        /// <summary>
        /// Команда выбора улучшение предмета в 2 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFirstSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedListViewSurvivorSecondItemAddonCommand
        {
            get => _selectedFirstSurvivorSecondItemAddonCommand ??= new(obj =>
            {
                SelectedFirstSurvivorSecondItemAddon = SelectedListViewItemAddon;
                SelectedFirstSurvivorSecondItemAddonName = SelectedListViewItemAddon.ItemAddonName;
            });
        }

        #endregion

        #region Билд - Подношение Выжившего №1 - Свойства

        /// <summary>
        /// Свойство для хранение информации о выбранном подношение у первого выжившего
        /// </summary>
        private Offering _selectedListViewFirstSurvivorOffering;
        public Offering SelectedListViewFirstSurvivorOffering
        {
            get => _selectedListViewFirstSurvivorOffering;
            set
            {
                if (value == null) { return; }
                _selectedListViewFirstSurvivorOffering = value;
                SelectedFirstSurvivorOfferingName = value.OfferingName;
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
                if (value == null) { return; }
                _selectedFirstSurvivorPlayerAssociation = value;
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
                if (value == null) { return; }
                _selectedSecondSurvivor = value;
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
                if (value == null) { return; }
                _selectedSecondSurvivorPlatform = value;
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
                _secondSurvivorPrestige = value;
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
                if (value == null) { return; }
                _selectedSecondSurvivorTypeDeath = value;
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
                if (value == null) { return; }
                _selectedSecondSurvivorFirstPerk = value;
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
                if (value == null) { return; }
                _selectedSecondSurvivorSecondPerk = value;
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
                if (value == null) { return; }
                _selectedSecondSurvivorThirdPerk = value;
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
                if (value == null) { return; }
                _selectedSecondSurvivorFourthPerk = value;
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
        public RelayCommand SelectedSecondSurvivorFirstPerkCommand
        {
            get => _selectedSecondSurvivorFirstPerkCommand ??= new(obj =>
            {
                SelectedSecondSurvivorFirstPerk = SelectedListViewSurvivorPerk;
                SelectedSecondSurvivorFirstPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorSecondPerkCommand;
        public RelayCommand SelectedSecondSurvivorSecondPerkCommand
        {
            get => _selectedSecondSurvivorSecondPerkCommand ??= new(obj =>
            {
                SelectedSecondSurvivorSecondPerk = SelectedListViewSurvivorPerk;
                SelectedSecondSurvivorSecondPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 3 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorThirdPerkCommand;
        public RelayCommand SelectedSecondSurvivorThirdPerkCommand
        {
            get => _selectedSecondSurvivorThirdPerkCommand ??= new(obj =>
            {
                SelectedSecondSurvivorThirdPerk = SelectedListViewSurvivorPerk;
                SelectedSecondSurvivorThirdPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 4 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorFourthPerkCommand;
        public RelayCommand SelectedSecondSurvivorFourthPerkCommand
        {
            get => _selectedSecondSurvivorFourthPerkCommand ??= new(obj =>
            {
                SelectedSecondSurvivorFourthPerk = SelectedListViewSurvivorPerk;
                SelectedSecondSurvivorFourthPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        #endregion

        #region Билд - Предметы Выжившего №2 - Свойства

        /// <summary>
        /// Свойство для выбора предмета у второго выжившего
        /// </summary>
        private Item _selectedComboBoxSecondSurvivorItem;
        public Item SelectedComboBoxSecondSurvivorItem
        {
            get => _selectedComboBoxSecondSurvivorItem;
            set
            {
                if (value == null) { return; }
                _selectedComboBoxSecondSurvivorItem = value;
                GetSecondSurvivorItemAddonListData();
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
                if (value == null) { return; }
                _selectedSecondSurvivorFirstItemAddon = value;
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
                if (value == null) { return; }
                _selectedSecondSurvivorSecondItemAddon = value;
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

        #endregion

        #region Билд - Предметы Выжившего №2 - Команды

        /// <summary>
        /// Команда выбора улучшение предмета в 1 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedSecondSurvivorFirstItemAddonCommand
        {
            get => _selectedSecondSurvivorFirstItemAddonCommand ??= new(obj =>
            {
                SelectedSecondSurvivorFirstItemAddon = SelectedListViewItemAddon;
                SelectedSecondSurvivorFirstItemAddonName = SelectedListViewItemAddon.ItemAddonName;
            });
        }

        /// <summary>
        /// Команда выбора улучшение предмета в 2 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedSecondSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedSecondSurvivorSecondItemAddonCommand
        {
            get => _selectedSecondSurvivorSecondItemAddonCommand ??= new(obj =>
            {
                SelectedSecondSurvivorSecondItemAddon = SelectedListViewItemAddon;
                SelectedSecondSurvivorSecondItemAddonName = SelectedListViewItemAddon.ItemAddonName;
            });
        }

        #endregion

        #region Билд - Подношение Выжившего №2 - Свойства

        /// <summary>
        /// Свойство для хранение информации о выбранном подношение у первого выжившего
        /// </summary>
        private Offering _selectedListViewSecondSurvivorOffering;
        public Offering SelectedListViewSecondSurvivorOffering
        {
            get => _selectedListViewSecondSurvivorOffering;
            set
            {
                if (value == null) { return; }
                _selectedListViewSecondSurvivorOffering = value;
                SelectedSecondSurvivorOfferingName = value.OfferingName;
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
                if (value == null) { return; }
                _selectedSecondSurvivorPlayerAssociation = value;
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
                if (value == null) { return; }
                _selectedThirdSurvivor = value;
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
                if (value == null) { return; }
                _selectedThirdSurvivorPlatform = value;
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
                _thirdSurvivorPrestige = value;
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
                if (value == null) { return; }
                _selectedThirdSurvivorTypeDeath = value;
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
                if (value == null) { return; }
                _selectedThirdSurvivorFirstPerk = value;
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
                if (value == null) { return; }
                _selectedThirdSurvivorSecondPerk = value;
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
                if (value == null) { return; }
                _selectedThirdSurvivorThirdPerk = value;
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
                if (value == null) { return; }
                _selectedThirdSurvivorFourthPerk = value;
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
        public RelayCommand SelectedThirdSurvivorFirstPerkCommand
        {
            get => _selectedThirdSurvivorFirstPerkCommand ??= new(obj =>
            {
                SelectedThirdSurvivorFirstPerk = SelectedListViewSurvivorPerk;
                SelectedThirdSurvivorFirstPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorSecondPerkCommand;
        public RelayCommand SelectedThirdSurvivorSecondPerkCommand
        {
            get => _selectedThirdSurvivorSecondPerkCommand ??= new(obj =>
            {
                SelectedThirdSurvivorSecondPerk = SelectedListViewSurvivorPerk;
                SelectedThirdSurvivorSecondPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 3 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorThirdPerkCommand;
        public RelayCommand SelectedThirdSurvivorThirdPerkCommand
        {
            get => _selectedThirdSurvivorThirdPerkCommand ??= new(obj =>
            {
                SelectedThirdSurvivorThirdPerk = SelectedListViewSurvivorPerk;
                SelectedThirdSurvivorThirdPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 4 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorFourthPerkCommand;
        public RelayCommand SelectedThirdSurvivorFourthPerkCommand
        {
            get => _selectedThirdSurvivorFourthPerkCommand ??= new(obj =>
            {
                SelectedThirdSurvivorFourthPerk = SelectedListViewSurvivorPerk;
                SelectedThirdSurvivorFourthPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        #endregion

        #region Билд - Предметы Выжившего №3 - Свойства

        /// <summary>
        /// Свойство для выбора предмета у третьего выжившего
        /// </summary>
        private Item _selectedComboBoxThirdSurvivorItem;
        public Item SelectedComboBoxThirdSurvivorItem
        {
            get => _selectedComboBoxThirdSurvivorItem;
            set
            {
                if (value == null) { return; }
                _selectedComboBoxThirdSurvivorItem = value;
                GetThirdSurvivorItemAddonListData();
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
                if (value == null) { return; }
                _selectedThirdSurvivorFirstItemAddon = value;
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
                if (value == null) { return; }
                _selectedThirdSurvivorSecondItemAddon = value;
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

        #endregion

        #region Билд - Предметы Выжившего №3 - Команды

        /// <summary>
        /// Команда выбора улучшение предмета в 1 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedThirdSurvivorFirstItemAddonCommand
        {
            get => _selectedThirdSurvivorFirstItemAddonCommand ??= new(obj =>
            {
                SelectedThirdSurvivorFirstItemAddon = SelectedListViewItemAddon;
                SelectedThirdSurvivorFirstItemAddonName = SelectedListViewItemAddon.ItemAddonName;
            });
        }

        /// <summary>
        /// Команда выбора улучшение предмета в 2 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedThirdSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedThirdSurvivorSecondItemAddonCommand
        {
            get => _selectedThirdSurvivorSecondItemAddonCommand ??= new(obj =>
            {
                SelectedThirdSurvivorSecondItemAddon = SelectedListViewItemAddon;
                SelectedThirdSurvivorSecondItemAddonName = SelectedListViewItemAddon.ItemAddonName;
            });
        }

        #endregion

        #region Билд - Подношение Выжившего №3 - Свойства

        /// <summary>
        /// Свойство для хранение информации о выбранном подношение у первого выжившего
        /// </summary>
        private Offering _selectedListViewThirdSurvivorOffering;
        public Offering SelectedListViewThirdSurvivorOffering
        {
            get => _selectedListViewThirdSurvivorOffering;
            set
            {
                if (value == null) { return; }
                _selectedListViewThirdSurvivorOffering = value;
                SelectedThirdSurvivorOfferingName = value.OfferingName;
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
                if (value == null) { return; }
                _selectedThirdSurvivorPlayerAssociation = value;
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
                if (value == null) { return; }
                _selectedFourthSurvivor = value;
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
                if (value == null) { return; }
                _selectedFourthSurvivorPlatform = value;
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
                _fourthSurvivorPrestige = value;
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
                if (value == null) { return; }
                _selectedFourthSurvivorTypeDeath = value;
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
                if (value == null) { return; }
                _selectedFourthSurvivorFirstPerk = value;
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
                if (value == null) { return; }
                _selectedFourthSurvivorSecondPerk = value;
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
                if (value == null) { return; }
                _selectedFourthSurvivorThirdPerk = value;
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
                if (value == null) { return; }
                _selectedFourthSurvivorFourthPerk = value;
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
        public RelayCommand SelectedFourthSurvivorFirstPerkCommand
        {
            get => _selectedFourthSurvivorFirstPerkCommand ??= new(obj =>
            {
                SelectedFourthSurvivorFirstPerk = SelectedListViewSurvivorPerk;
                SelectedFourthSurvivorFirstPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 2 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorSecondPerkCommand;
        public RelayCommand SelectedFourthSurvivorSecondPerkCommand
        {
            get => _selectedFourthSurvivorSecondPerkCommand ??= new(obj =>
            {
                SelectedFourthSurvivorSecondPerk = SelectedListViewSurvivorPerk;
                SelectedFourthSurvivorSecondPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 3 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorThirdPerkCommand;
        public RelayCommand SelectedFourthSurvivorThirdPerkCommand
        {
            get => _selectedFourthSurvivorThirdPerkCommand ??= new(obj =>
            {
                SelectedFourthSurvivorThirdPerk = SelectedListViewSurvivorPerk;
                SelectedFourthSurvivorThirdPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        /// <summary>
        /// Команда выбора аддона в 4 позицию для ContextMenu у элемента ListView SurvivorPerk.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorFourthPerkCommand;
        public RelayCommand SelectedFourthSurvivorFourthPerkCommand
        {
            get => _selectedFourthSurvivorFourthPerkCommand ??= new(obj =>
            {
                SelectedFourthSurvivorFourthPerk = SelectedListViewSurvivorPerk;
                SelectedFourthSurvivorFourthPerkName = SelectedListViewSurvivorPerk.PerkName;
            });
        }

        #endregion

        #region Билд - Предметы Выжившего №4 - Свойства

        /// <summary>
        /// Свойство для выбора предмета у третьего выжившего
        /// </summary>
        private Item _selectedComboBoxFourthSurvivorItem;
        public Item SelectedComboBoxFourthSurvivorItem
        {
            get => _selectedComboBoxFourthSurvivorItem;
            set
            {
                if (value == null) { return; }
                _selectedComboBoxFourthSurvivorItem = value;
                GetFourthSurvivorItemAddonListData();
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
                if (value == null) { return; }
                _selectedFourthSurvivorFirstItemAddon = value;
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
                if (value == null) { return; }
                _selectedFourthSurvivorSecondItemAddon = value;
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

        #endregion

        #region Билд - Предметы Выжившего №4 - Команды

        /// <summary>
        /// Команда выбора улучшение предмета в 1 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorFirstItemAddonCommand;
        public RelayCommand SelectedFourthSurvivorFirstItemAddonCommand
        {
            get => _selectedFourthSurvivorFirstItemAddonCommand ??= new(obj =>
            {
                SelectedFourthSurvivorFirstItemAddon = SelectedListViewItemAddon;
                SelectedFourthSurvivorFirstItemAddonName = SelectedListViewItemAddon.ItemAddonName;
            });
        }

        /// <summary>
        /// Команда выбора улучшение предмета в 2 позицию для ContextMenu у элемента ListView ItemAddon.
        /// После выбора выводит название перка в соответствующий TextBox
        /// </summary>
        private RelayCommand _selectedFourthSurvivorSecondItemAddonCommand;
        public RelayCommand SelectedFourthSurvivorSecondItemAddonCommand
        {
            get => _selectedFourthSurvivorSecondItemAddonCommand ??= new(obj =>
            {
                SelectedFourthSurvivorSecondItemAddon = SelectedListViewItemAddon;
                SelectedFourthSurvivorSecondItemAddonName = SelectedListViewItemAddon.ItemAddonName;
            });
        }

        #endregion

        #region Билд - Подношение Выжившего №4 - Свойства

        /// <summary>
        /// Свойство для хранение информации о выбранном подношение у первого выжившего
        /// </summary>
        private Offering _selectedListViewFourthSurvivorOffering;
        public Offering SelectedListViewFourthSurvivorOffering
        {
            get => _selectedListViewFourthSurvivorOffering;
            set
            {
                if (value == null) { return; }
                _selectedListViewFourthSurvivorOffering = value;
                SelectedFourthSurvivorOfferingName = value.OfferingName;
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
                if (value == null) { return; }
                _selectedFourthSurvivorPlayerAssociation = value;
            }
        }

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
        private DateTime _selectedDurationMatch;
        public DateTime SelectedDurationMatch
        {
            get => _selectedDurationMatch;
            set
            {
                _selectedDurationMatch = value;
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
                    MessageBox.Show("Нельзя сдлеать больше 4 и меньше 0 убийств");
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
                    MessageBox.Show("Нельзя сдлеать больше 12 и меньше 0 повесов");
                }
                else
                {
                    _countHooks = value;
                    OnPropertyChanged();
                }

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
                    MessageBox.Show("В игре не может быть больше 5 и меньше 0 генераторов");
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
                if (value == null) { return; }
                _selectedPatch = value;
            }
        }

        private Patch _selectedGameMode;
        public Patch SelectedGameMode
        {
            get => _selectedGameMode;
            set
            {
                if (value == null) { return; }
                _selectedGameMode = value;
            }
        }

        private Patch _selectedGameEvent;
        public Patch SelectedGameEvent
        {
            get => _selectedGameEvent;
            set
            {
                if (value == null) { return; }
                _selectedGameEvent = value;
            }
        }

        private Patch _selectedWhoPlacedMap;
        public Patch SelectedWhoPlacedMap
        {
            get => _selectedWhoPlacedMap;
            set
            {
                if (value == null) { return; }
                _selectedWhoPlacedMap = value;
            }
        }

        private Patch _selectedWhoPlacedMapWin;
        public Patch SelectedWhoPlacedMapWin
        {
            get => _selectedWhoPlacedMapWin;
            set
            {
                if (value == null) { return; }
                _selectedWhoPlacedMapWin = value;
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
            }
        }

        #endregion

        #endregion

        #region Свойства для выбора ID Киллера и 4 Выживших

        private int _killerInfoID;
        public int KillerInfoID
        {
            get => _killerInfoID;
            set
            {
                _killerInfoID = value;
                OnPropertyChanged();
            }
        }

        private int _firstSurvivorInfoID;
        public int firstSurvivorInfoID
        {
            get => _firstSurvivorInfoID;
            set
            {
                _firstSurvivorInfoID = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства окна

        public string Title { get; set; } = "Добавить запись";

        #endregion

        #region Свойства для скрытия \ показа ListView 

        private Visibility _listViewKillerPerkVisibility;
        public Visibility ListViewKillerPerkVisibility
        {
            get => _listViewKillerPerkVisibility;
            set
            {
                _listViewKillerPerkVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _contentButtonListViewKillerPerkVisibility;
        public string ContentButtonListViewKillerPerkVisibility
        {
            get => _contentButtonListViewKillerPerkVisibility;
            set
            {
                _contentButtonListViewKillerPerkVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _listViewKillerAddonVisibility;
        public Visibility ListViewKillerAddonVisibility
        {
            get => _listViewKillerAddonVisibility;
            set
            {
                _listViewKillerAddonVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _contentButtonListViewKillerAddonVisibility;
        public string ContentButtonListViewKillerAddonVisibility
        {
            get => _contentButtonListViewKillerAddonVisibility;
            set
            {
                _contentButtonListViewKillerAddonVisibility = value;
                OnPropertyChanged();
            }
        }


        private Visibility _listViewKillerOfferingVisibility;
        public Visibility ListViewKillerOfferingVisibility
        {
            get => _listViewKillerOfferingVisibility;
            set
            {
                _listViewKillerOfferingVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _contentButtonListViewKillerOfferingVisibility;
        public string ContentButtonListViewKillerOfferingVisibility
        {
            get => _contentButtonListViewKillerOfferingVisibility;
            set
            {
                _contentButtonListViewKillerOfferingVisibility = value;
                OnPropertyChanged();
            }
        }


        private Visibility _listViewSurvivorPerkVisibility;
        public Visibility ListViewSurvivorPerkVisibility
        {
            get => _listViewSurvivorPerkVisibility;
            set
            {
                _listViewSurvivorPerkVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _contentButtonListViewSurvivorPerkVisibility;
        public string ContentButtonListViewSurvivorPerkVisibility
        {
            get => _contentButtonListViewSurvivorPerkVisibility;
            set
            {
                _contentButtonListViewSurvivorPerkVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _listViewSurvivorItemAddonVisibility;
        public Visibility ListViewSurvivorItemAddonVisibility
        {
            get => _listViewSurvivorItemAddonVisibility;
            set
            {
                _listViewSurvivorItemAddonVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _contentButtonListViewSurvivorItemAddonVisibility;
        public string ContentButtonListViewSurvivorItemAddonVisibility
        {
            get => _contentButtonListViewSurvivorItemAddonVisibility;
            set
            {
                _contentButtonListViewSurvivorItemAddonVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _listViewSurvivorOfferingVisibility;
        public Visibility ListViewSurvivorOfferingVisibility
        {
            get => _listViewSurvivorOfferingVisibility;
            set
            {
                _listViewSurvivorOfferingVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _contentButtonListViewSurvivorOfferingVisibility;
        public string ContentButtonListViewSurvivorOfferingVisibility
        {
            get => _contentButtonListViewSurvivorOfferingVisibility;
            set
            {
                _contentButtonListViewSurvivorOfferingVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Команды для скрытия \ показа ListView 

        private RelayCommand _listViewKillerPerkVisibilityCommand;
        public RelayCommand ListViewKillerPerkVisibilityCommand
        {
            get => _listViewKillerPerkVisibilityCommand ??= new(obj =>
            {
                if (ListViewKillerPerkVisibility == Visibility.Collapsed)
                {
                    ListViewKillerPerkVisibility = Visibility.Visible;
                    ContentButtonListViewKillerPerkVisibility = "Скрыть";
                }
                else
                {
                    ListViewKillerPerkVisibility = Visibility.Collapsed;
                    ContentButtonListViewKillerPerkVisibility = "Показать";
                }
            });
        }

        private RelayCommand _listViewKillerAddonVisibilityCommand;
        public RelayCommand ListViewKillerAddonVisibilityCommand
        {
            get => _listViewKillerAddonVisibilityCommand ??= new(obj =>
            {
                if (ListViewKillerAddonVisibility == Visibility.Collapsed)
                {
                    ListViewKillerAddonVisibility = Visibility.Visible;
                    ContentButtonListViewKillerAddonVisibility = "Скрыть";
                }
                else
                {
                    ListViewKillerAddonVisibility = Visibility.Collapsed;
                    ContentButtonListViewKillerAddonVisibility = "Показать";
                }
            });
        }

        private RelayCommand _listViewKillerOfferingVisibilityCommand;
        public RelayCommand ListViewKillerOfferingVisibilityCommand
        {
            get => _listViewKillerOfferingVisibilityCommand ??= new(obj =>
            {
                if (ListViewKillerOfferingVisibility == Visibility.Collapsed)
                {
                    ListViewKillerOfferingVisibility = Visibility.Visible;
                    ContentButtonListViewKillerOfferingVisibility = "Скрыть";
                }
                else
                {
                    ListViewKillerOfferingVisibility = Visibility.Collapsed;
                    ContentButtonListViewKillerOfferingVisibility = "Показать";
                }
            });
        }


        private RelayCommand _listViewSurvivorPerkVisibilityCommand;
        public RelayCommand ListViewSurvivorPerkVisibilityCommand
        {
            get => _listViewSurvivorPerkVisibilityCommand ??= new(obj =>
            {
                if (ListViewSurvivorPerkVisibility == Visibility.Collapsed)
                {
                    ListViewSurvivorPerkVisibility = Visibility.Visible;
                    ContentButtonListViewSurvivorPerkVisibility = "Скрыть";
                }
                else
                {
                    ListViewSurvivorPerkVisibility = Visibility.Collapsed;
                    ContentButtonListViewSurvivorPerkVisibility = "Показать";
                }
            });
        }

        private RelayCommand _listViewSurvivorItemAddonVisibilityCommand;
        public RelayCommand ListViewSurvivorItemAddonVisibilityCommand
        {
            get => _listViewSurvivorItemAddonVisibilityCommand ??= new(obj =>
            {
                if (ListViewSurvivorItemAddonVisibility == Visibility.Collapsed)
                {
                    ListViewSurvivorItemAddonVisibility = Visibility.Visible;
                    ContentButtonListViewSurvivorItemAddonVisibility = "Скрыть";
                }
                else
                {
                    ListViewSurvivorItemAddonVisibility = Visibility.Collapsed;
                    ContentButtonListViewSurvivorItemAddonVisibility = "Показать";
                }
            });
        }

        private RelayCommand _listViewSurvivorOfferingVisibilityCommand;
        public RelayCommand ListViewSurvivorOfferingVisibilityCommand
        {
            get => _listViewSurvivorOfferingVisibilityCommand ??= new(obj =>
            {
                if (ListViewSurvivorOfferingVisibility == Visibility.Collapsed)
                {
                    ListViewSurvivorOfferingVisibility = Visibility.Visible;
                    ContentButtonListViewSurvivorOfferingVisibility = "Скрыть";
                }
                else
                {
                    ListViewSurvivorOfferingVisibility = Visibility.Collapsed;
                    ContentButtonListViewSurvivorOfferingVisibility = "Показать";
                }
            });
        }

        #endregion 

        public AddMatchWindowViewModel()
        {
            SetContentButtonForVisibilityListView();
            SetVisibilityListView();

            DeclareCollections();
            GetAllData();
        }

        #region Методы для поиска по LisrView

        private async void SearchKillerPerk()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var search = await context.KillerPerks
                    .Where(kp => kp.PerkName.Contains(TextBoxSearchKillerPerk))
                    .ToListAsync();

                KillerPerkList.Clear();

                foreach (var item in search)
                {
                    KillerPerkList.Add(item);
                }
            }
        }

        private async void SearchKillerAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedKiller == null)
                {
                    MessageBox.Show("Вы не выбрали убийцу");
                    return;
                }
                var search = await context.KillerAddons
                    .Where(ka => ka.IdKiller == SelectedKiller.IdKiller)
                    .Where(ka => ka.AddonName.Contains(TextBoxSearchKillerAddon))
                    .ToListAsync();

                KillerAddonList.Clear();

                foreach (var item in search)
                {
                    KillerAddonList.Add(item);
                }
            }
        }

        private async void SearchKillerOffering()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var search = await context.Offerings
                    .Where(off => off.IdRole == SelectedComboBoxRoleKiller.IdRole)
                    .Where(off => off.OfferingName.Contains(TextBoxSearchKillerOffering))
                    .ToListAsync();

                KillerOfferingList.Clear();

                foreach (var item in search)
                {
                    KillerOfferingList.Add(item);
                }
            }
        }

        private async void SearchSurvivorOffering()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var search = await context.Offerings
                    .Where(off => off.IdRole == SelectedComboBoxRoleSurvivor.IdRole)
                    .Where(off => off.OfferingName.Contains(TextBoxSearchSurvivorOffering))
                    .ToListAsync();

                SurvivorOfferingList.Clear();

                foreach (var item in search)
                {
                    SurvivorOfferingList.Add(item);
                }
            }
        }

        private async void SearchSurvivorPerk()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var search = await context.SurvivorPerks
                    .Where(sp => sp.PerkName.Contains(TextBoxSearchSurvivorPerk))
                    .ToListAsync();

                SurvivorPerkList.Clear();

                foreach (var item in search)
                {
                    SurvivorPerkList.Add(item);
                }
            }
        }

        private async void SearchFirstSurvivorItemAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedComboBoxFirstSurvivorItem == null)
                {
                    return;
                }
                var search = await context.ItemAddons
                    .Where(ia => ia.IdItem == SelectedComboBoxFirstSurvivorItem.IdItem)
                    .Where(ka => ka.ItemAddonName.Contains(TextBoxSearchFirstSurvivorItemAddon))
                    .ToListAsync();

                ItemAddonList.Clear();

                foreach (var item in search)
                {
                    ItemAddonList.Add(item);
                }
            }
        }

        private async void SearchSecondSurvivorItemAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedComboBoxSecondSurvivorItem == null)
                {
                    return;
                }
                var search = await context.ItemAddons
                    .Where(ia => ia.IdItem == SelectedComboBoxSecondSurvivorItem.IdItem)
                    .Where(ka => ka.ItemAddonName.Contains(TextBoxSearchSecondSurvivorItemAddon))
                    .ToListAsync();

                ItemAddonList.Clear();

                foreach (var item in search)
                {
                    ItemAddonList.Add(item);
                }
            }
        }

        private async void SearchThirdSurvivorItemAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedComboBoxThirdSurvivorItem == null)
                {
                    return;
                }
                var search = await context.ItemAddons
                    .Where(ia => ia.IdItem == SelectedComboBoxThirdSurvivorItem.IdItem)
                    .Where(ka => ka.ItemAddonName.Contains(TextBoxSearchThirdSurvivorItemAddon))
                    .ToListAsync();

                ItemAddonList.Clear();

                foreach (var item in search)
                {
                    ItemAddonList.Add(item);
                }
            }
        }

        private async void SearchFourthSurvivorItemAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedComboBoxFourthSurvivorItem == null)
                {
                    return;
                }
                var search = await context.ItemAddons
                    .Where(ia => ia.IdItem == SelectedComboBoxFourthSurvivorItem.IdItem)
                    .Where(ka => ka.ItemAddonName.Contains(TextBoxSearchFourthSurvivorItemAddon))
                    .ToListAsync();

                ItemAddonList.Clear();

                foreach (var item in search)
                {
                    ItemAddonList.Add(item);
                }
            }
        }

        #endregion

        #region Методы получение данных  - заполнение списков

        private void DeclareCollections()
        {
            GameModeList = [];
            GameEventList = [];
            MapList = [];
            KillerOfferingList = [];
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
            SurvivorOfferingList = [];
            TypeDeathList = [];
            ItemList = [];
            ItemAddonList = [];
        }

        private void GetAllData()
        {
            GetGameModeListData();
            GetGameEventListData();
            GetMapListData();
            GetPatchListData();
            GetPlatformListData();
            GetPlayerAssociationListData();
            GetRoleListData();
            GetWhoPlacedMapListData();

            GetKillerListData();
            GetKillerAddonListData();
            GetKillerPerkListData();
            GetKillerInfoListData();
            GetKillerOfferingListData();

            GetSurvivorListData();
            GetSurvivorPerkListData();
            GetSurvivorInfoListData();
            GetTypeDeathListData();
            GetItemListData();
            GetSurvivorOfferingListData();

            GetFirstSurvivorItemAddonListData();
            GetSecondSurvivorItemAddonListData();
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

        private void GetKillerOfferingListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    if (SelectedComboBoxRoleKiller == null)
                    {
                        return;
                    }
                    var entities = context.Offerings
                    .Where(off => off.IdRole == SelectedComboBoxRoleKiller.IdRole)
                    .ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        KillerOfferingList.Clear();
                        foreach (var entity in entities)
                        {
                            KillerOfferingList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        private void GetSurvivorOfferingListData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    if (SelectedComboBoxRoleSurvivor == null)
                    {
                        return;
                    }
                    var entities = context.Offerings
                    .Where(off => off.IdRole == SelectedComboBoxRoleSurvivor.IdRole)
                    .ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        SurvivorOfferingList.Clear();
                        foreach (var entity in entities)
                        {
                            SurvivorOfferingList.Add(entity);
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
                    var entities = context.Killers.Skip(1).ToList();

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
                    if (SelectedKiller == null)
                    {
                        var entities = context.KillerAddons.ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            KillerAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                KillerAddonList.Add(entity);
                            }
                        });
                    }
                    else
                    {
                        var entities = context.KillerAddons.Where(ka => ka.IdKiller == SelectedKiller.IdKiller).ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            KillerAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                KillerAddonList.Add(entity);
                            }
                        });
                    }
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

        private void GetFirstSurvivorItemAddonListData()
        {

            new Thread(() =>
            {
                using (var context = new MasterAnalyticsDeadByDaylightDbContext())
                {
                    if (SelectedComboBoxFirstSurvivorItem == null)
                    {
                        var entities = context.ItemAddons.ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ItemAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                ItemAddonList.Add(entity);
                            }
                        });
                    }
                    else
                    {
                        var entities = context.ItemAddons.Where(ia => ia.IdItem == SelectedComboBoxFirstSurvivorItem.IdItem).ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ItemAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                ItemAddonList.Add(entity);
                            }
                        });
                    }

                }
            }).Start();
        }

        private void GetSecondSurvivorItemAddonListData()
        {

            new Thread(() =>
            {
                using (var context = new MasterAnalyticsDeadByDaylightDbContext())
                {
                    if (SelectedComboBoxSecondSurvivorItem == null)
                    {
                        var entities = context.ItemAddons.ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ItemAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                ItemAddonList.Add(entity);
                            }
                        });
                    }
                    else
                    {
                        var entities = context.ItemAddons.Where(ia => ia.IdItem == SelectedComboBoxSecondSurvivorItem.IdItem).ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ItemAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                ItemAddonList.Add(entity);
                            }
                        });
                    }

                }
            }).Start();
        }

        private void GetThirdSurvivorItemAddonListData()
        {

            new Thread(() =>
            {
                using (var context = new MasterAnalyticsDeadByDaylightDbContext())
                {
                    if (SelectedComboBoxThirdSurvivorItem == null)
                    {
                        var entities = context.ItemAddons.ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ItemAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                ItemAddonList.Add(entity);
                            }
                        });
                    }
                    else
                    {
                        var entities = context.ItemAddons.Where(ia => ia.IdItem == SelectedComboBoxThirdSurvivorItem.IdItem).ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ItemAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                ItemAddonList.Add(entity);
                            }
                        });
                    }

                }
            }).Start();
        }

        private void GetFourthSurvivorItemAddonListData()
        {

            new Thread(() =>
            {
                using (var context = new MasterAnalyticsDeadByDaylightDbContext())
                {
                    if (SelectedComboBoxFourthSurvivorItem == null)
                    {
                        var entities = context.ItemAddons.ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ItemAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                ItemAddonList.Add(entity);
                            }
                        });
                    }
                    else
                    {
                        var entities = context.ItemAddons.Where(ia => ia.IdItem == SelectedComboBoxFourthSurvivorItem.IdItem).ToList();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ItemAddonList.Clear();
                            foreach (var entity in entities)
                            {
                                ItemAddonList.Add(entity);
                            }
                        });
                    }

                }
            }).Start();
        }

        #endregion

        #region Методы для установки Visibility у ListView 

        private void SetVisibilityListView()
        {
            ListViewKillerPerkVisibility = Visibility.Collapsed;
            ListViewKillerAddonVisibility = Visibility.Collapsed;
            ListViewKillerOfferingVisibility = Visibility.Collapsed;

            ListViewSurvivorPerkVisibility = Visibility.Collapsed;
            ListViewSurvivorItemAddonVisibility = Visibility.Collapsed;
            ListViewSurvivorOfferingVisibility = Visibility.Collapsed;
        }

        private void SetContentButtonForVisibilityListView()
        {
            ContentButtonListViewKillerPerkVisibility = "Показать";
            ContentButtonListViewKillerAddonVisibility = "Показать";
            ContentButtonListViewKillerOfferingVisibility = "Показать";

            ContentButtonListViewSurvivorPerkVisibility = "Показать";
            ContentButtonListViewSurvivorItemAddonVisibility = "Показать";
            ContentButtonListViewSurvivorOfferingVisibility = "Показать";
        }
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
                if (value == null) { return; }
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

        #region Команды

        private RelayCommand _loadImageCommand;
        public RelayCommand LoadImageCommand { get => _loadImageCommand ??= new(async obj => { await GetImageAsync(); }); }

        private RelayCommand _loadResultMatchImageCommand;
        public RelayCommand LoadResultMatchImageCommand { get => _loadResultMatchImageCommand ??= new(async obj => { ResultMatchImage = await ImageHelper.GetResultMatchImageAsync(SelectedImage.PathImage); }); }

        private RelayCommand _loadStartMatchImageCommand;
        public RelayCommand LoadStartMatchImageCommand { get => _loadStartMatchImageCommand ??= new(async obj => { StartMatchImage = await ImageHelper.GetStartMatchImageAsync(SelectedImage.PathImage); }); }

        private RelayCommand _loadEndMatchImageCommand;
        public RelayCommand LoadEndMatchImageCommand { get => _loadEndMatchImageCommand ??= new(async obj => { EndMatchImage = await ImageHelper.GetEndMatchImageAsync(SelectedImage.PathImage); }); }
        
        private RelayCommand _clearImageListCommandCommand;
        public RelayCommand ClearImageListCommandCommand { get => _clearImageListCommandCommand ??= new(obj => { ImagesList.Clear(); }); }

        #endregion

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
                    FileCreatedTime = fileInfo.CreationTime.ToString()
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

        //private async Task GetImageAsync()
        //{
        //    OpenFileDialog openFileDialog = new()
        //    {
        //        Multiselect = true,
        //        Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
        //        InitialDirectory = @"D:\Steam\userdata\189964443\760\remote\381210\screenshots"
        //    };

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        foreach (var item in openFileDialog.FileNames)
        //        {
        //            FileInfo fileInfo = new(item);
        //            ImagesList.Add(new ImageInformation
        //            {
        //                PathImage = item,
        //                ResizeImage = await GetResizeImageFromDirectoryTumbnails(Path.GetFileName(item)),
        //                FileName = Path.GetFileName(item),
        //                FileCreatedTime = fileInfo.CreationTime.ToString()
        //            });
        //        }
        //    }
        //}

        #endregion

        #endregion  

    }
}
