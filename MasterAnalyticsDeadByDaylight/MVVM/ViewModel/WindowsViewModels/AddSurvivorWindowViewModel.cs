using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
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

        private readonly IDialogService _dialogService;

        public AddSurvivorWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            Title = "Список выживших";
            GetSurvivorData();
        }

        #region Команды

        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand { get => _selectImageCommand ??= new(obj => { SelectImageSurvivor(); }); }

        private RelayCommand _clearImageCommand;
        public RelayCommand ClearImageCommand { get => _clearImageCommand ??= new(obj => { ImageSurvivor = null; }); }

        private RelayCommand _saveSurvivorCommand;
        public RelayCommand SaveSurvivorCommand { get => _saveSurvivorCommand ??= new(obj => { SaveSurvivor(); }); }

        private RelayCommand _updateSurvivorCommand;
        public RelayCommand UpdateSurvivorCommand { get => _updateSurvivorCommand ??= new(obj => { UpdateSurvivor(); }); }

        private RelayCommand _deleteSurvivorCommand;
        public RelayCommand DeleteSurvivorCommand { get => _deleteSurvivorCommand ??= new(async obj => 
        {
            if (SelectedSurvivorItem == null)
            {
                return;
            }
            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedSurvivorItem.SurvivorName}»? При удаление данном записи, могут удалится связанные записи в других таблицах.",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedSurvivorItem);
                GetSurvivorData();
            }
            else
            {
                return;
            }
        }); }

        #endregion

        #region Методы

        private async void GetSurvivorData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var survivors = await context.Survivors.Skip(1).ToListAsync();
                SurvivorList.Clear();

                foreach (var item in survivors)
                {
                    SurvivorList.Add(item);
                }
                SurvivorList.Add(context.Survivors.FirstOrDefault(x => x.SurvivorName == "Общий"));
            }
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

        private void SaveSurvivor()
        {
            var newSurvivor = new Survivor() { SurvivorName = SurvivorName, SurvivorImage = ImageSurvivor, SurvivorDescription = SurvivorDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Survivors.Any(survivor => survivor.SurvivorName.ToLower() == newSurvivor.SurvivorName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(SurvivorName))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Совпадение имени");
                }
                else
                {
                    var survivor = context.Survivors.Add(newSurvivor);
                    context.SaveChanges();
                    GetSurvivorData();
                    SurvivorName = string.Empty;
                    SurvivorDescription = string.Empty;
                    ImageSurvivor = null;
                    SelectedSurvivorItem = null;
                }
            }
        }

        private void UpdateSurvivor()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedSurvivorItem == null)
                {
                    return;
                }

                var entityToUpdate = context.Survivors.Find(SelectedSurvivorItem.IdSurvivor);

                if (entityToUpdate != null)
                {
                    bool exists = context.Survivors.Any(x => x.SurvivorName.ToLower() == SurvivorName.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена имени с «{SelectedSurvivorItem.SurvivorName}» на «{SurvivorName}» ?\n"+$"А так же замена описание с «{SelectedSurvivorItem.SurvivorDescription}» на «{SurvivorDescription}» ?",
                            $"Надпись с именем «{SelectedSurvivorItem.SurvivorName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.SurvivorName = SurvivorName;
                            entityToUpdate.SurvivorImage = ImageSurvivor;
                            entityToUpdate.SurvivorDescription = SurvivorDescription;
                            context.SaveChanges();

                            GetSurvivorData();

                            SelectedSurvivorItem = null;
                            SurvivorName = string.Empty;
                            SurvivorDescription = string.Empty;
                            ImageSurvivor = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.SurvivorName = SurvivorName;
                        entityToUpdate.SurvivorImage = ImageSurvivor;
                        entityToUpdate.SurvivorDescription = SurvivorDescription;
                        context.SaveChanges();

                        GetSurvivorData();

                        SelectedSurvivorItem = null;
                        SurvivorName = string.Empty;
                        SurvivorDescription = string.Empty;
                        ImageSurvivor = null;
                    }
                   
                }
            }
        }
 
        #endregion
    }
}
