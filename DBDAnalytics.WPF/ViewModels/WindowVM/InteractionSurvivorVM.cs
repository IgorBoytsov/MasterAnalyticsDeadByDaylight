using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.Services.Realization;
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
    internal class InteractionSurvivorVM : BaseVM, IUpdatable
    {
        private readonly ISurvivorService _survivorService;
        private readonly ISurvivorPerkService _survivorPerkService;
        private readonly ISurvivorPerkCategoryService _survivorPerkCategoryService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionSurvivorVM(IWindowNavigationService windowNavigationService, 
                                     ISurvivorService survivorService,
                                     ISurvivorPerkService survivorPerkService,
                                     ISurvivorPerkCategoryService survivorPerkCategoryService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _survivorService = survivorService;
            _survivorPerkService = survivorPerkService;
            _survivorPerkCategoryService = survivorPerkCategoryService;

            GetSurvivorsWithPerks();
            GetSurvivorPerkCategories();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            if (parameter is SurvivorPerkCategoryDTO survivorPerkCategoryAdd && typeParameter == TypeParameter.AddAndNotification)
            {
                SurvivorPerkCategories.Add(survivorPerkCategoryAdd);
            }

            if (parameter is SurvivorPerkCategoryDTO survivorPerkCategoryUpdate && typeParameter == TypeParameter.UpdateAndNotification)
            {
                SurvivorPerkCategories.ReplaceItem(SurvivorPerkCategories.FirstOrDefault(x => x.IdSurvivorPerkCategory == survivorPerkCategoryUpdate.IdSurvivorPerkCategory), survivorPerkCategoryUpdate);
            }

            if (parameter is SurvivorPerkCategoryDTO survivorPerkCategoryDelete && typeParameter == TypeParameter.UpdateAndNotification)
            {
                SurvivorPerkCategories.Remove(SurvivorPerkCategories.FirstOrDefault(x => x.IdSurvivorPerkCategory == survivorPerkCategoryDelete.IdSurvivorPerkCategory));
            }
        }
        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<SurvivorWithPerksDTO> SurvivorWithPerks { get; set; } = [];

        public ObservableCollection<SurvivorPerkCategoryDTO> SurvivorPerkCategories { get; set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Выжившие и перки";

        #endregion

        #region Свойства : Selected

        private SurvivorWithPerksDTO _selectedSurvivorWithPerks;
        public SurvivorWithPerksDTO SelectedSurvivorWithPerks
        {
            get => _selectedSurvivorWithPerks;
            set
            {
                _selectedSurvivorWithPerks = value;

                if (value == null)
                    return;

                SurvivorName = value.SurvivorName;
                SurvivorImage = value.SurvivorImage;
                SurvivorDescription = value.SurvivorDescription;

                OnPropertyChanged();
            }
        }

        private SurvivorPerkDTO _selectedSurvivorPerk;
        public SurvivorPerkDTO SelectedSurvivorPerk
        {
            get => _selectedSurvivorPerk;
            set
            {
                _selectedSurvivorPerk = value;

                if (value == null)
                    return;

                PerkName = value.PerkName;
                PerkImage = value.PerkImage;
                PerkDescription = value.PerkDescription;
                SelectedSurvivorPerkCategory = SurvivorPerkCategories.FirstOrDefault(x => x.IdSurvivorPerkCategory == value.IdCategory);

                OnPropertyChanged();
            }
        }

        private SurvivorPerkCategoryDTO _selectedSurvivorPerkCategory;
        public SurvivorPerkCategoryDTO SelectedSurvivorPerkCategory
        {
            get => _selectedSurvivorPerkCategory;
            set
            {
                _selectedSurvivorPerkCategory = value;

                if (value == null)
                    return;

                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выживший

        private string _survivorName;
        public string SurvivorName
        {
            get => _survivorName;
            set
            {
                _survivorName = value;
                OnPropertyChanged();
            } 
        }

        private string _survivorDescription;
        public string SurvivorDescription
        {
            get => _survivorDescription;
            set
            {
                _survivorDescription = value;
                OnPropertyChanged();
            }
        }

        private byte[] _survivorImage;
        public byte[] SurvivorImage
        {
            get => _survivorImage;
            set
            {
                _survivorImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Перк

        private string _perkName;
        public string PerkName
        {
            get => _perkName;
            set
            {
                _perkName = value;
                OnPropertyChanged();
            }
        }

        private string _perkDescription;
        public string PerkDescription
        {
            get => _perkDescription;
            set
            {
                _perkDescription = value;
                OnPropertyChanged();
            }
        }

        private byte[] _perkImage;
        public byte[] PerkImage
        {
            get => _perkImage;
            set
            {
                _perkImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление зависимых данных

        private RelayCommand _addSurvivorPerkCategoryCommand;
        public RelayCommand AddSurvivorPerkCategoryCommand { get => _addSurvivorPerkCategoryCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionSurvivorPerkCategory, null, TypeParameter.None, true); }); }

        #endregion 

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _createSurvivorCommand;
        public RelayCommand CreateSurvivorCommand { get => _createSurvivorCommand ??= new(obj => { CreateSurvivor(); }); }

        private RelayCommand _deleteSurvivorCommand;
        public RelayCommand DeleteSurvivorCommand { get => _deleteSurvivorCommand ??= new(obj => { DeleteSurvivor(); }); }

        private RelayCommand _updateSurvivorCommand;
        public RelayCommand UpdateSurvivorCommand { get => _updateSurvivorCommand ??= new(obj => { UpdateSurvivor(); }); }


        private RelayCommand _createPerkCommand;
        public RelayCommand CreatePerkCommand { get => _createPerkCommand ??= new(obj => { CreatePerk(); }); }

        private RelayCommand _deletePerkCommand;
        public RelayCommand DeletePerkCommand { get => _deletePerkCommand ??= new(obj => { DeletePerk(); }); }

        private RelayCommand _updatePerkCommand;
        public RelayCommand UpdatePerkCommand { get => _updatePerkCommand ??= new(obj => { UpdatePerk(); }); }

        #endregion

        #region Выбор | Очистка изображение

        private RelayCommand _selectSurvivorImageCommand;
        public RelayCommand SelectSurvivorImageCommand { get => _selectSurvivorImageCommand ??= new(obj => { SelectSurvivorImage(); }); }

        private RelayCommand _clearSurvivorImageCommand;
        public RelayCommand ClearSurvivorImageCommand { get => _clearSurvivorImageCommand ??= new(obj => { SurvivorImage = null; }); }


        private RelayCommand _selectPerkImageCommand;
        public RelayCommand SelectPerkImageCommand { get => _selectPerkImageCommand ??= new(obj => { SelectPerkImage(); }); }

        private RelayCommand _clearPerkImageCommand;
        public RelayCommand ClearPerkImageCommand { get => _clearPerkImageCommand ??= new(obj => { PerkImage = null; }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение данных

        private async void GetSurvivorsWithPerks()
        {
            var survivorsWithPerks = await _survivorService.GetSurvivorsWithPerksAsync();

            foreach (var item in survivorsWithPerks)
                SurvivorWithPerks.Add(item);
        }

        private async void GetSurvivorPerkCategories()
        {
            var survivorPerkCategories = await _survivorPerkCategoryService.GetAllAsync();

            foreach (var item in survivorPerkCategories)
                SurvivorPerkCategories.Add(item);
        }

        #endregion

        // TODO : Изменить MessageBox на кастомное окно
        #region CRUD : Выживший

        private async void CreateSurvivor()
        {
            var (SurvivorDTO, Message) = await _survivorService.CreateAsync(SurvivorName, SurvivorImage, SurvivorDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            { 
                SurvivorWithPerks.Add(new SurvivorWithPerksDTO
                {
                    IdSurvivor = SurvivorDTO.IdSurvivor,
                    SurvivorName = SurvivorDTO.SurvivorName,
                    SurvivorImage = SurvivorDTO.SurvivorImage,
                    SurvivorDescription = SurvivorDTO.SurvivorDescription,
                    SurvivorPerks = new ObservableCollection<SurvivorPerkDTO>()
                });

                NotificationTransmittingValue(WindowName.AddMatch, SurvivorDTO, TypeParameter.AddAndNotification);
                ClearInputDataSurvivor();
            }
        }

        private async void UpdateSurvivor()
        {
            if (SelectedSurvivorWithPerks == null)
                return;

            var (SurvivorDTO, Message) = await _survivorService.UpdateAsync(SelectedSurvivorWithPerks.IdSurvivor, SurvivorName, SurvivorImage, SurvivorDescription);

            if (Message == string.Empty)
            {
                SurvivorWithPerks.ReplaceItem(SelectedSurvivorWithPerks,
                                              new SurvivorWithPerksDTO
                                              {
                                                  IdSurvivor = SurvivorDTO.IdSurvivor,
                                                  SurvivorName = SurvivorDTO.SurvivorName,
                                                  SurvivorImage = SurvivorDTO.SurvivorImage,
                                                  SurvivorDescription = SurvivorDTO.SurvivorDescription,
                                                  SurvivorPerks = SelectedSurvivorWithPerks.SurvivorPerks
                                              });

                NotificationTransmittingValue(WindowName.AddMatch, SurvivorDTO, TypeParameter.UpdateAndNotification);
                ClearInputDataSurvivor();
            }
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите обновить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedSurvivorWithPerksDTO = await _survivorService.ForcedUpdateAsync(SelectedSurvivorWithPerks.IdSurvivor, SurvivorName, SurvivorImage, SurvivorDescription);
                    SurvivorWithPerks.ReplaceItem(SelectedSurvivorWithPerks,
                                                  new SurvivorWithPerksDTO
                                                  {
                                                      IdSurvivor = forcedSurvivorWithPerksDTO.IdSurvivor,
                                                      SurvivorName = forcedSurvivorWithPerksDTO.SurvivorName,
                                                      SurvivorImage = forcedSurvivorWithPerksDTO.SurvivorImage,
                                                      SurvivorDescription = forcedSurvivorWithPerksDTO.SurvivorDescription,
                                                      SurvivorPerks = SelectedSurvivorWithPerks.SurvivorPerks
                                                  });

                    NotificationTransmittingValue(WindowName.AddMatch, forcedSurvivorWithPerksDTO, TypeParameter.UpdateAndNotification);
                    ClearInputDataSurvivor();
                }
            }
        }

        private async void DeleteSurvivor()
        {
            if (SelectedSurvivorWithPerks == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _survivorService.DeleteAsync(SelectedSurvivorWithPerks.IdSurvivor);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    NotificationTransmittingValue(WindowName.AddMatch, new SurvivorDTO { IdSurvivor = SelectedSurvivorWithPerks.IdSurvivor }, TypeParameter.DeleteAndNotification);
                    SurvivorWithPerks.Remove(SelectedSurvivorWithPerks);
                    ClearInputDataSurvivor();
                }
            }
        }

        #endregion

        // TODO : Изменить MessageBox на кастомное окно
        #region CRUD : Перк

        private async void CreatePerk()
        {
            if (SelectedSurvivorPerkCategory == null)
            {
                MessageBox.Show("Вы не выбрали категорию.");
                return;
            }  

            var (SurvivorPerk, Message) = await _survivorPerkService.CreateAsync(SelectedSurvivorWithPerks.IdSurvivor, 
                                                                                 PerkName, 
                                                                                 PerkImage, 
                                                                                 SelectedSurvivorPerkCategory.IdSurvivorPerkCategory, 
                                                                                 PerkDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                NotificationTransmittingValue(WindowName.AddMatch, SurvivorPerk, TypeParameter.AddAndNotification);
                SelectedSurvivorWithPerks.SurvivorPerks.Add(SurvivorPerk);
                ClearInputDataPerk();
            }
        }

        private async void UpdatePerk()
        {
            if (SelectedSurvivorWithPerks == null || SelectedSurvivorWithPerks == null)
                return;

            if (SelectedSurvivorPerkCategory == null)
            {
                MessageBox.Show("Вы не выбрали качество");
                return;
            }

            var (Perk, Message) = await _survivorPerkService.UpdateAsync(SelectedSurvivorPerk.IdSurvivorPerk,
                                                                         SelectedSurvivorPerk.IdSurvivor,
                                                                         PerkName,
                                                                         PerkImage,
                                                                         SelectedSurvivorPerkCategory.IdSurvivorPerkCategory,
                                                                         PerkDescription);

            if (Message == string.Empty)
            {
                NotificationTransmittingValue(WindowName.AddMatch, Perk, TypeParameter.UpdateAndNotification);
                SelectedSurvivorWithPerks.SurvivorPerks.ReplaceItem(SelectedSurvivorPerk, Perk);
                ClearInputDataPerk();
            }
            else
                MessageBox.Show(Message);
        }

        private async void DeletePerk()
        {
            if (SelectedSurvivorPerk == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _survivorPerkService.DeleteAsync(SelectedSurvivorPerk.IdSurvivorPerk);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    NotificationTransmittingValue(WindowName.AddMatch, SelectedSurvivorPerk, TypeParameter.DeleteAndNotification);
                    SelectedSurvivorWithPerks.SurvivorPerks.Remove(SelectedSurvivorPerk);
                    ClearInputDataSurvivor();
                }
            }
        }

        #endregion

        //TODO : Заменить прямой вызов OpenFileDialog на вызов из сервиса
        #region Выбор изображения

        private void SelectSurvivorImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                SurvivorImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        private void SelectPerkImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                PerkImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        #endregion

        #region Сброс вводимых данных

        private void ClearInputDataSurvivor()
        {
            SurvivorName = string.Empty;
            SurvivorDescription = string.Empty;
            SurvivorImage = null;
        }

        private void ClearInputDataPerk()
        {
            PerkName = string.Empty;
            PerkDescription = string.Empty;
            PerkImage = null;
        }

        #endregion
    }
}