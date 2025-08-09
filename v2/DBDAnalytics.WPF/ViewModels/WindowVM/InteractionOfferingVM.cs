using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Helpers;
using DBDAnalytics.WPF.Interfaces;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class InteractionOfferingVM : BaseVM, IUpdatable
    {
        private readonly IOfferingCategoryService _offeringCategoryService;
        private readonly IOfferingService _offeringService;
        private readonly IRarityService _rarityService;
        private readonly IRoleService _roleService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionOfferingVM(IWindowNavigationService windowNavigationService, 
                                     IOfferingCategoryService offeringCategoryService,
                                     IOfferingService offeringService,
                                     IRarityService rarityService,
                                     IRoleService roleService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _rarityService = rarityService;
            _roleService = roleService;
            _offeringCategoryService = offeringCategoryService;
            _offeringService = offeringService;

            GetOffering();
            GetCategory();
            GetRarity();
            GetRole();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            #region Добавление | Обновление | Изменение объекта Rarity из окна RarityWindow

            if (parameter is RarityDTO rarityAddDTO && typeParameter == TypeParameter.AddAndNotification)
            {
                Rarities.Add(rarityAddDTO);
                return;
            }

            if (parameter is RarityDTO RarityUpdateDTO && typeParameter == TypeParameter.UpdateAndNotification)
            {
                Rarities.ReplaceItem(Rarities.FirstOrDefault(x => x.IdRarity == RarityUpdateDTO.IdRarity), RarityUpdateDTO);
                return;
            }

            if (parameter is int idRarity && typeParameter == TypeParameter.DeleteAndNotification)
            {
                Rarities.Remove(Rarities.FirstOrDefault(x => x.IdRarity == idRarity));
            }

            #endregion 

            #region Добавление | Обновление | Изменение объекта OfferingCategory из окна OfferingCategoryWindow 

            if (parameter is OfferingCategoryDTO offeringCategoryAddDTO && typeParameter == TypeParameter.AddAndNotification)
            {
                OfferingCategories.Add(offeringCategoryAddDTO);
                return;
            }

            if (parameter is OfferingCategoryDTO offeringCategoryUpdateDTO  && typeParameter == TypeParameter.UpdateAndNotification)
            {
                OfferingCategories.ReplaceItem(OfferingCategories.FirstOrDefault(x => x.IdCategory == offeringCategoryUpdateDTO.IdCategory), offeringCategoryUpdateDTO);
                return;
            }

            if (parameter is int idOfferingCategory && typeParameter == TypeParameter.DeleteAndNotification)
            {
                OfferingCategories.Remove(OfferingCategories.FirstOrDefault(x => x.IdCategory == idOfferingCategory));
            }

            #endregion

            #region Добавление | Обновление | Изменение объекта Role из окна RoleWindow 

            if (parameter is RoleDTO RoleAddDTO && typeParameter == TypeParameter.AddAndNotification)
            {
                Roles.Add(RoleAddDTO);
                return;
            }

            if (parameter is RoleDTO RoleUpdate && typeParameter == TypeParameter.UpdateAndNotification)
            {
                Roles.ReplaceItem(Roles.FirstOrDefault(x => x.IdRole == RoleUpdate.IdRole), RoleUpdate);
                return;
            }

            if (parameter is int idRole && typeParameter == TypeParameter.DeleteAndNotification)
            {
                Roles.Remove(Roles.FirstOrDefault(x => x.IdRole == idRole));
            }

            #endregion
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<OfferingDTO> Offerings { get; set; } = [];

        public ObservableCollection<OfferingCategoryDTO> OfferingCategories { get; set; } = [];

        public ObservableCollection<RoleDTO> Roles { get; set; } = [];

        public ObservableCollection<RarityDTO> Rarities { get; set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Игровые ассоциации";

        #endregion

        #region Свойства : Selected

        private OfferingDTO _selectedOffering;
        public OfferingDTO SelectedOffering
        {
            get => _selectedOffering;
            set
            {
                _selectedOffering = value;

                if (value == null)
                    return;

                OfferingName = value.OfferingName;
                OfferingImage = value.OfferingImage;

                SelectedRole = Roles.FirstOrDefault(x => x.IdRole == value.IdRole);
                SelectedOfferingCategory = OfferingCategories.FirstOrDefault(x => x.IdCategory == value.IdCategory);
                SelectedRarity = Rarities.FirstOrDefault(x => x.IdRarity == value.IdRarity);

                OnPropertyChanged();
            }
        }

        private OfferingCategoryDTO _selectedOfferingCategory;
        public OfferingCategoryDTO SelectedOfferingCategory
        {
            get => _selectedOfferingCategory;
            set
            {
                _selectedOfferingCategory = value;
                OnPropertyChanged();
            }
        }

        private RarityDTO _selectedRarity;
        public RarityDTO SelectedRarity
        {
            get => _selectedRarity;
            set
            {
                _selectedRarity = value;
                OnPropertyChanged();
            }
        }

        private RoleDTO _selectedRole;
        public RoleDTO SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Offering

        private string _offeringName;
        public string OfferingName
        {
            get => _offeringName;
            set
            {
                _offeringName = value;
                OnPropertyChanged();
            }
        }

        private byte[] _offeringImage;
        public byte[] OfferingImage
        {
            get => _offeringImage;
            set
            {
                _offeringImage = value;
                OnPropertyChanged();
            }
        }

        private string _offeringDescription;
        public string OfferingDescription
        {
            get => _offeringDescription;
            set
            {
                _offeringDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Открытие окон с взоимодействием с зависимыми данными

        private RelayCommand _addRarityCommand;
        public RelayCommand AddRarityCommand { get => _addRarityCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionRarity, null, TypeParameter.None, true); }); }

        private RelayCommand _addOfferingCategoryCommand;
        public RelayCommand AddROfferingCategoryCommand { get => _addOfferingCategoryCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionOfferingCategory, null, TypeParameter.None, true); }); }

        private RelayCommand _addRoleCommand;
        public RelayCommand AddRoleCommand { get => _addRoleCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionRole, null, TypeParameter.None, true); }); }

        #endregion

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _createOfferingCommand;
        public RelayCommand CreateOfferingCommand { get => _createOfferingCommand ??= new(obj => { CreateOffering(); }); }

        private RelayCommand _updateOfferingCommand;
        public RelayCommand UpdateOfferingCommand { get => _updateOfferingCommand ??= new(obj => { UpdateOffering(); }); }

        private RelayCommand _deleteOfferingCommand;
        public RelayCommand DeleteOfferingCommand { get => _deleteOfferingCommand ??= new(obj => { DeleteOffering(); }); }

        #endregion

        #region Выбор изображения | Очистка выбранного изображениея

        private RelayCommand _selectOfferingImageCommand;
        public RelayCommand SelectOfferingImageCommand { get => _selectOfferingImageCommand ??= new(obj => { SelectOfferingImage(); }); }

        private RelayCommand _clearOfferingImageCommand;
        public RelayCommand ClearOfferingImageCommand { get => _clearOfferingImageCommand ??= new(obj => { OfferingImage = null; }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение и заполнение данных

        private async void GetOffering()
        {
            var offerings = await _offeringService.GetAllAsync();

            foreach (var item in offerings)
            {
                Offerings.Add(item);
            }
        }

        private async void GetCategory()
        {
            var categories = await _offeringCategoryService.GetAllAsync();

            foreach (var item in categories)
            {
                OfferingCategories.Add(item);
            }
        }

        private async void GetRarity()
        {
            var rarities = await _rarityService.GetAllAsync();

            foreach (var item in rarities)
            {
                Rarities.Add(item);
            }
        }

        private async void GetRole()
        {
            var roles = await _roleService.GetAllAsync();

            foreach (var item in roles)
            {
                Roles.Add(item);
            }
        }

        #endregion

        //TODO : Заменить MessageBox на кастомное окно
        #region CRUD

        private async void CreateOffering()
        {              
            if (SelectedOfferingCategory == null)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }

            if (SelectedRole == null)
            {
                MessageBox.Show("Выберите роль.");
                return;
            }

            if (SelectedRarity == null)
            {
                MessageBox.Show("Выберите качество.");
                return;
            }          

            var (Offering, Message) = await _offeringService.CreateAsync(SelectedRole.IdRole, 
                                                                         SelectedOfferingCategory.IdCategory, 
                                                                         SelectedRarity.IdRarity, 
                                                                         OfferingName, 
                                                                         OfferingImage, 
                                                                         OfferingDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                Offerings.Add(Offering);
                NotificationTransmittingValue(WindowName.AddMatch, Offering, TypeParameter.AddAndNotification);
                ClearInputDataOffering();
            }
        }

        private async void UpdateOffering()
        {
            if (SelectedOffering == null)
                return;

            var (Offering, Message) = await _offeringService.UpdateAsync(SelectedOffering.IdOffering, 
                                                                         SelectedRole.IdRole, 
                                                                         SelectedOfferingCategory.IdCategory,
                                                                         SelectedRarity.IdRarity, 
                                                                         OfferingName, 
                                                                         OfferingImage, 
                                                                         OfferingDescription);

            if (Message == string.Empty)
            {
                Offerings.ReplaceItem(SelectedOffering, Offering);
                NotificationTransmittingValue(WindowName.AddMatch, Offering, TypeParameter.UpdateAndNotification);
                ClearInputDataOffering();
            }
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите обновить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedOffering = await _offeringService.ForcedUpdateAsync(SelectedOffering.IdOffering,
                                                                                  SelectedRole.IdRole,
                                                                                  SelectedOfferingCategory.IdCategory,
                                                                                  SelectedRarity.IdRarity,
                                                                                  OfferingName,
                                                                                  OfferingImage,
                                                                                  OfferingDescription);

                    Offerings.ReplaceItem(SelectedOffering, forcedOffering);
                    NotificationTransmittingValue(WindowName.AddMatch, forcedOffering, TypeParameter.UpdateAndNotification);
                    ClearInputDataOffering();
                }
            }
        }

        private async void DeleteOffering()
        {
            if (SelectedOffering == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _offeringService.DeleteAsync(SelectedOffering.IdOffering);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    NotificationTransmittingValue(WindowName.AddMatch, SelectedOffering, TypeParameter.DeleteAndNotification);
                    Offerings.Remove(SelectedOffering);
                    ClearInputDataOffering();
                }
            }
        }

        #endregion

        //TODO : Заменить прямой вызов OpenFileDialog на вызов из сервиса
        #region Выбор изображения

        private void SelectOfferingImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                OfferingImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        #endregion

        #region Сброс вводимых данных

        private void ClearInputDataOffering()
        {
            SelectedOfferingCategory = null;
            SelectedRarity = null;
            SelectedRole = null;
            OfferingName = string.Empty;
            OfferingImage = null;
            OfferingDescription = string.Empty;
        }


        #endregion
    }
}