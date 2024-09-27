using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class AddSurvivorWindowViewModel : BaseViewModel
    {
        #region Свойства

        public ObservableCollection<Survivor> SurvivorList { get; set; } = [];

        private Survivor _selectedSurvivorItem;
        public Survivor SelectedSurvivorItem
        {
            get => _selectedSurvivorItem;
            set
            {
                _selectedSurvivorItem = value;
                if (value == null) { return; }
                ImageSurvivor = value.SurvivorImage;
                SurvivorName = value.SurvivorName;
                SurvivorDescription = value.SurvivorDescription;
                OnPropertyChanged();
            }
        }

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

        private byte[] _imageSurvivor;
        public byte[] ImageSurvivor
        {
            get => _imageSurvivor;
            set
            {
                _imageSurvivor = value;
                OnPropertyChanged();
            }
        } 
        
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }


        #endregion

        private readonly ICustomDialogService _dialogService;
        private readonly IDataService _dataService;

        public AddSurvivorWindowViewModel(ICustomDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;
            Title = "Список выживших";
            GetSurvivorData();
        }

        #region Команды

        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand { get => _selectImageCommand ??= new(obj => { SelectImageSurvivor(); }); }

        private RelayCommand _clearImageCommand;
        public RelayCommand ClearImageCommand { get => _clearImageCommand ??= new(obj => { ImageSurvivor = null; }); }

        private RelayCommand _saveSurvivorCommand;
        public RelayCommand SaveSurvivorCommand { get => _saveSurvivorCommand ??= new(obj => { AddSurvivor(); }); }

        private RelayCommand _updateSurvivorCommand;
        public RelayCommand UpdateSurvivorCommand { get => _updateSurvivorCommand ??= new(obj => { UpdateSurvivor(); }); }

        private RelayCommand _deleteSurvivorCommand;
        public RelayCommand DeleteSurvivorCommand { get => _deleteSurvivorCommand ??= new(async obj => 
        {
            if (SelectedSurvivorItem == null) return;
            if (MessageHelper.MessageDelete(SelectedSurvivorItem.SurvivorName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedSurvivorItem);
                GetSurvivorData();
            }
        }); }

        #endregion

        #region Методы

        private async void GetSurvivorData()
        {
            var survivors = await _dataService.GetAllDataAsync<Survivor>();

            SurvivorList.Clear();

            foreach (var item in survivors.Skip(1))
            {
                SurvivorList.Add(item);
            }
            SurvivorList.Add(survivors.FirstOrDefault(x => x.SurvivorName == "Общий"));
        }

        private void SelectImageSurvivor()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    ImageSurvivor = ImageHelper.ImageToByteArray(image);
                }
            }
        }

        private async void AddSurvivor()
        {
            var newSurvivor = new Survivor() { SurvivorName = SurvivorName, SurvivorImage = ImageSurvivor, SurvivorDescription = SurvivorDescription };

            bool exists = await _dataService.ExistsAsync<Survivor>(x => x.SurvivorName.ToLower() == newSurvivor.SurvivorName.ToLower());

            if (exists || string.IsNullOrWhiteSpace(SurvivorName)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newSurvivor);
                GetSurvivorData();
                SurvivorName = string.Empty;
                SurvivorDescription = string.Empty;
                ImageSurvivor = null;
                SelectedSurvivorItem = null;
            }
        }

        private async void UpdateSurvivor()
        {
            if (SelectedSurvivorItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<Survivor>(SelectedSurvivorItem.IdSurvivor);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<Survivor>(x => x.SurvivorName.ToLower() == SurvivorName.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedSurvivorItem.SurvivorName, SurvivorName, SelectedSurvivorItem.SurvivorDescription, SurvivorDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.SurvivorName = SurvivorName;
                        entityToUpdate.SurvivorImage = ImageSurvivor;
                        entityToUpdate.SurvivorDescription = SurvivorDescription;
                        await _dataService.UpdateAsync(entityToUpdate);

                        GetSurvivorData();

                        SelectedSurvivorItem = null;
                        SurvivorName = string.Empty;
                        SurvivorDescription = string.Empty;
                        ImageSurvivor = null;
                    }
                }
                else
                {
                    entityToUpdate.SurvivorName = SurvivorName;
                    entityToUpdate.SurvivorImage = ImageSurvivor;
                    entityToUpdate.SurvivorDescription = SurvivorDescription; 
                    await _dataService.UpdateAsync(entityToUpdate);

                    GetSurvivorData();

                    SelectedSurvivorItem = null;
                    SurvivorName = string.Empty;
                    SurvivorDescription = string.Empty;
                    ImageSurvivor = null;
                }

            }
        }
 
        #endregion
    }
}
