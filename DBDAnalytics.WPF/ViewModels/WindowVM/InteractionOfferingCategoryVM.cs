using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class InteractionOfferingCategoryVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _navigationService;
        private readonly IOfferingCategoryService _offeringCategoryService;


        public InteractionOfferingCategoryVM(IWindowNavigationService windowNavigationService, IOfferingCategoryService offeringCategoryService) : base(windowNavigationService)
        {
            _navigationService = windowNavigationService;
            _offeringCategoryService = offeringCategoryService;

            GetOfferingCategory();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<OfferingCategoryDTO> OfferingCategories { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Категории подношений.";

        #endregion

        #region Свойства : Selected | OfferingCategoryName

        private OfferingCategoryDTO _selectedOfferingCategory;
        public OfferingCategoryDTO SelectedOfferingCategory
        {
            get => _selectedOfferingCategory;
            set
            {
                if (_selectedOfferingCategory != value)
                {
                    _selectedOfferingCategory = value;

                    OfferingCategoryName = value?.CategoryName;

                    OnPropertyChanged();
                }
            }
        }

        private string _offeringCategoryName;
        public string OfferingCategoryName
        {
            get => _offeringCategoryName;
            set
            {
                _offeringCategoryName = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _createOfferingCategoryCommand;
        public RelayCommand CreateOfferingCategoryCommand { get => _createOfferingCategoryCommand ??= new(obj => { CreateOfferingCategory(); }); }

        private RelayCommand _deleteOfferingCategoryCommand;
        public RelayCommand DeleteOfferingCategoryCommand { get => _deleteOfferingCategoryCommand ??= new(obj => { DeleteOfferingCategory(); }); }

        private RelayCommand _updateOfferingCategoryCommand;
        public RelayCommand UpdateOfferingCategoryCommand { get => _updateOfferingCategoryCommand ??= new(obj => { UpdateOfferingCategory(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private async void GetOfferingCategory()
        {
            var offeringCategories = await _offeringCategoryService.GetAllAsync();

            foreach (var offeringCategory in offeringCategories)
                OfferingCategories.Add(offeringCategory);
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void CreateOfferingCategory()
        {
            var newOfferingCategoryDTO = await _offeringCategoryService.CreateAsync(OfferingCategoryName);

            if (newOfferingCategoryDTO.Message != string.Empty)
            {
                MessageBox.Show(newOfferingCategoryDTO.Message);
                return;
            }
            else
            {
                NotificationTransmittingValue(WindowName.InteractionOffering, newOfferingCategoryDTO.OfferingCategoryDTO, TypeParameter.AddAndNotification);
                OfferingCategories.Add(newOfferingCategoryDTO.OfferingCategoryDTO);
            }
                
        }

        private async void UpdateOfferingCategory()
        {
            if (SelectedOfferingCategory == null)
                return;

            var (offeringCategoryDTO, Message) = await _offeringCategoryService.UpdateAsync(SelectedOfferingCategory.IdCategory, OfferingCategoryName);

            if (Message == string.Empty)
            {
                NotificationTransmittingValue(WindowName.InteractionOffering, offeringCategoryDTO, TypeParameter.UpdateAndNotification);
                OfferingCategories.ReplaceItem(SelectedOfferingCategory, offeringCategoryDTO);
            }
            else
                MessageBox.Show(Message);
        }

        private async void DeleteOfferingCategory()
        {
            if (SelectedOfferingCategory == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _offeringCategoryService.DeleteAsync(SelectedOfferingCategory.IdCategory);

                if (IsDeleted == true)
                {
                    NotificationTransmittingValue(WindowName.InteractionOffering, SelectedOfferingCategory.IdCategory, TypeParameter.DeleteAndNotification);
                    OfferingCategories.Remove(SelectedOfferingCategory);
                }
                else
                    MessageBox.Show(Message);
            }
        }

        #endregion
    }
}
