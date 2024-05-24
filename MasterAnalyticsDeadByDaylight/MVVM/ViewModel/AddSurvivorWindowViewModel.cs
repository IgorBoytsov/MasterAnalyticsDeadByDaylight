using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
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

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    public class AddSurvivorWindowViewModel : BaseViewModel
    {
        #region Свойства

        public ObservableCollection<Survivor> SurvivorList {  get; set; }

        private Survivor _selectedSurvivorItem;
        public Survivor SelectedSurvivorItem
        {
            get => _selectedSurvivorItem;
            set
            {
                _selectedSurvivorItem = value;
                if (value == null) { return; }
                ImageSurvivor = value.SurvivorImage;
                TextBoxSurvivorName = value.SurvivorName;
                TextBoxSurvivorDescription = value.SurvivorDescription;
                OnPropertyChanged();
            }
        }

        private string _textBoxSurvivorName;
        public string TextBoxSurvivorName
        {
            get => _textBoxSurvivorName;
            set
            {
                _textBoxSurvivorName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxSurvivorDescription;
        public string TextBoxSurvivorDescription
        {
            get => _textBoxSurvivorDescription;
            set
            {
                _textBoxSurvivorDescription = value;
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
        #endregion

        public AddSurvivorWindowViewModel()
        {
            SurvivorList = new ObservableCollection<Survivor>();
            GetSurvivorData();
        }

        #region Команды

        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand { get => _selectImageCommand ??= new(obj => { SelectImageSurvivor(); }); }

        private RelayCommand _saveSurvivorCommand;
        public RelayCommand SaveSurvivorCommand { get => _saveSurvivorCommand ??= new(obj => { SaveSurvivor(); }); }

        private RelayCommand _updateSurvivorCommand;
        public RelayCommand UpdateSurvivorCommand { get => _updateSurvivorCommand ??= new(obj => { UpdateSurvivor(); }); }

        private RelayCommand _deleteSurvivorCommand;
        public RelayCommand DeleteSurvivorCommand { get => _deleteSurvivorCommand ??= new(obj => { DeleteSurvivor(); }); }

        #endregion

        #region Методы

        private async void GetSurvivorData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var survivors = await context.Survivors.ToListAsync();
                SurvivorList.Clear();

                foreach (var item in survivors)
                {
                    SurvivorList.Add(item);
                }
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
            var newSurvivor = new Survivor() { SurvivorName = TextBoxSurvivorName, SurvivorImage = ImageSurvivor, SurvivorDescription = TextBoxSurvivorDescription};

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Survivors.Any(survivor => survivor.SurvivorName.ToLower() == newSurvivor.SurvivorName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(TextBoxSurvivorName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    var survivor = context.Survivors.Add(newSurvivor);
                    context.SaveChanges();
                    GetSurvivorData();
                    TextBoxSurvivorName = string.Empty;
                    TextBoxSurvivorDescription = string.Empty;
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
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedSurvivorItem.SurvivorName} на {TextBoxSurvivorName} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.SurvivorName = TextBoxSurvivorName;
                        entityToUpdate.SurvivorImage = ImageSurvivor;
                        entityToUpdate.SurvivorDescription = TextBoxSurvivorDescription;
                        context.SaveChanges();

                        GetSurvivorData();

                        SelectedSurvivorItem = null;
                        TextBoxSurvivorName = string.Empty;
                        TextBoxSurvivorDescription = string.Empty;
                        ImageSurvivor = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
            }
        }

        private void DeleteSurvivor()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.Survivors.Find(SelectedSurvivorItem.IdSurvivor);
                if (entityToDelete != null)
                {
                    context.Survivors.Remove(entityToDelete);
                    GetSurvivorData();
                }
            }
            
        }
        #endregion
    }
}
