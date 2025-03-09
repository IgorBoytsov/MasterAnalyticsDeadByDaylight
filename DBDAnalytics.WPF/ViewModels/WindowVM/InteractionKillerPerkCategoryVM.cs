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
    internal class InteractionKillerPerkCategoryVM : BaseVM, IUpdatable
    {
        private readonly IKillerPerkCategoryService _killerPerkCategoryService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionKillerPerkCategoryVM(IWindowNavigationService windowNavigationService, IKillerPerkCategoryService killerPerkCategoryService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _killerPerkCategoryService = killerPerkCategoryService;

            GetKillerPerkCategory();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<KillerPerkCategoryDTO> KillerPerkCategories { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Категории перков у убийц.";

        #endregion

        #region Свойства : Selected | KillerPerkCategoryName | KillerPerkCategoryDescription

        private KillerPerkCategoryDTO _selectedKillerPerkCategory;
        public KillerPerkCategoryDTO SelectedKillerPerkCategory
        {
            get => _selectedKillerPerkCategory;
            set
            {
                if (_selectedKillerPerkCategory != value)
                {
                    _selectedKillerPerkCategory = value;

                    KillerPerkCategoryName = value?.CategoryName;

                    OnPropertyChanged();
                }
            }
        }

        private string _killerPerkCategoryName;
        public string KillerPerkCategoryName
        {
            get => _killerPerkCategoryName;
            set
            {
                _killerPerkCategoryName = value;
                OnPropertyChanged();
            }
        }
        
        private string _killerPerkCategoryDescription;
        public string KillerPerkCategoryDescription
        {
            get => _killerPerkCategoryDescription;
            set
            {
                _killerPerkCategoryDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addKillerPerkCategoryCommand;
        public RelayCommand AddKillerPerkCategoryCommand { get => _addKillerPerkCategoryCommand ??= new(obj => { AddKillerPerkCategory(); }); }

        private RelayCommand _deleteKillerPerkCategoryCommand;
        public RelayCommand DeleteKillerPerkCategoryCommand { get => _deleteKillerPerkCategoryCommand ??= new(obj => { DeleteKillerPerkCategory(); }); }

        private RelayCommand _updateKillerPerkCategoryCommand;
        public RelayCommand UpdateKillerPerkCategoryCommand { get => _updateKillerPerkCategoryCommand ??= new(obj => { UpdateKillerPerkCategory(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/
        
        private async void GetKillerPerkCategory()
        {
            var killerPerkCategories = await _killerPerkCategoryService.GetAllAsync();

            foreach (var killerPerkCategory in killerPerkCategories)
                KillerPerkCategories.Add(killerPerkCategory);
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddKillerPerkCategory()
        {
            var newKillerPerkCategoryDTO = await _killerPerkCategoryService.CreateAsync(KillerPerkCategoryName, KillerPerkCategoryDescription);

            if (newKillerPerkCategoryDTO.Message != string.Empty)
            {
                MessageBox.Show(newKillerPerkCategoryDTO.Message);
                return;
            }
            else
            {
                NotificationTransmittingValue(WindowName.InteractionKiller, newKillerPerkCategoryDTO.KillerPerkCategoryDTO, TypeParameter.AddAndNotification);
                KillerPerkCategories.Add(newKillerPerkCategoryDTO.KillerPerkCategoryDTO);
            }
                
        }

        private async void UpdateKillerPerkCategory()
        {
            if (SelectedKillerPerkCategory == null)
                return;

            var (KillerPerkCategoryDTO, Message) = await _killerPerkCategoryService.UpdateAsync(SelectedKillerPerkCategory.IdKillerPerkCategory, KillerPerkCategoryName, KillerPerkCategoryDescription);

            if (Message == string.Empty)
            {
                NotificationTransmittingValue(WindowName.InteractionKiller, KillerPerkCategoryDTO, TypeParameter.UpdateAndNotification);
                KillerPerkCategories.ReplaceItem(SelectedKillerPerkCategory, KillerPerkCategoryDTO);
            }
            else
                MessageBox.Show(Message);
        }

        private async void DeleteKillerPerkCategory()
        {
            if (SelectedKillerPerkCategory == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _killerPerkCategoryService.DeleteAsync(SelectedKillerPerkCategory.IdKillerPerkCategory);

                if (IsDeleted == true)
                {
                    NotificationTransmittingValue(WindowName.InteractionKiller, SelectedKillerPerkCategory, TypeParameter.DeleteAndNotification);
                    KillerPerkCategories.Remove(SelectedKillerPerkCategory);
                }
                else
                    MessageBox.Show(Message);
            }
        }

        #endregion
    }
}