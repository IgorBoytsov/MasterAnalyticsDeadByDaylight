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
    internal class InteractionSurvivorPerkCategoryVM : BaseVM, IUpdatable
    {
        private readonly ISurvivorPerkCategoryService _survivorPerkCategoryService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionSurvivorPerkCategoryVM(IWindowNavigationService windowNavigationService, ISurvivorPerkCategoryService survivorPerkCategoryService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _survivorPerkCategoryService = survivorPerkCategoryService;

            GetSurvivorPerkCategories();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<SurvivorPerkCategoryDTO> SurvivorPerkCategories { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Категории перков выживших";

        #endregion

        #region Свойства : Selected | CategoryName| CategoryDescription

        private SurvivorPerkCategoryDTO _selectedSurvivorPerkCategory;
        public SurvivorPerkCategoryDTO SelectedSurvivorPerkCategory
        {
            get => _selectedSurvivorPerkCategory;
            set
            {
                if (_selectedSurvivorPerkCategory != value)
                {
                    _selectedSurvivorPerkCategory = value;

                    Category = value?.CategoryName;

                    OnPropertyChanged();
                }
            }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        private string _categoryDescription;
        public string CategoryDescription
        {
            get => _categoryDescription;
            set
            {
                _categoryDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addSurvivorPerkCategoryCommand;
        public RelayCommand AddSurvivorPerkCategoryCommand { get => _addSurvivorPerkCategoryCommand ??= new(obj => { AddSurvivorPerkCategory(); }); }

        private RelayCommand _deleteSurvivorPerkCategoryCommand;
        public RelayCommand DeleteSurvivorPerkCategoryCommand { get => _deleteSurvivorPerkCategoryCommand ??= new(obj => { DeleteSurvivorPerkCategory(); }); }

        private RelayCommand _updateSurvivorPerkCategoryCommand;
        public RelayCommand UpdateSurvivorPerkCategoryCommand { get => _updateSurvivorPerkCategoryCommand ??= new(obj => { UpdateSurvivorPerkCategory(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private async void GetSurvivorPerkCategories()
        {
            var survivorPerkCategories = await _survivorPerkCategoryService.GetAllAsync();

            foreach (var survivorPerkCategory in survivorPerkCategories)
                SurvivorPerkCategories.Add(survivorPerkCategory);
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddSurvivorPerkCategory()
        {
            var newSurvivorPerkCategoryDTO = await _survivorPerkCategoryService.CreateAsync(Category, CategoryDescription);

            if (newSurvivorPerkCategoryDTO.Message != string.Empty)
            {
                MessageBox.Show(newSurvivorPerkCategoryDTO.Message);
                return;
            }
            else
            {
                NotificationTransmittingValue(WindowName.InteractionSurvivor, newSurvivorPerkCategoryDTO.SurvivorPerkCategoryDTO, TypeParameter.AddAndNotification);
                SurvivorPerkCategories.Add(newSurvivorPerkCategoryDTO.SurvivorPerkCategoryDTO);
            }
                
        }

        private async void UpdateSurvivorPerkCategory()
        {
            if (SelectedSurvivorPerkCategory == null)
                return;

            var (SurvivorPerkCategoryDTO, Message) = await _survivorPerkCategoryService.UpdateAsync(SelectedSurvivorPerkCategory.IdSurvivorPerkCategory, Category, CategoryDescription);

            if (Message == string.Empty)
            {
                NotificationTransmittingValue(WindowName.InteractionSurvivor, SurvivorPerkCategoryDTO, TypeParameter.UpdateAndNotification);
                SurvivorPerkCategories.ReplaceItem(SelectedSurvivorPerkCategory, SurvivorPerkCategoryDTO);
            }
            else
                MessageBox.Show(Message);
        }

        private async void DeleteSurvivorPerkCategory()
        {
            if (SelectedSurvivorPerkCategory == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _survivorPerkCategoryService.DeleteAsync(SelectedSurvivorPerkCategory.IdSurvivorPerkCategory);

                if (IsDeleted == true)
                {
                    NotificationTransmittingValue(WindowName.InteractionSurvivor, SelectedSurvivorPerkCategory.IdSurvivorPerkCategory, TypeParameter.DeleteAndNotification);
                    SurvivorPerkCategories.Remove(SelectedSurvivorPerkCategory);
                }
                else
                    MessageBox.Show(Message);
            }
        }

        #endregion
    }
}