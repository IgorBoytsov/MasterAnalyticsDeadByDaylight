using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class AddMapWindowViewModel : BaseViewModel
    {
        #region Свойства

        public ObservableCollection<Map> MapList { get; set; }

        private Map _selectedMapItem;
        public Map SelectedMapItem
        {
            get => _selectedMapItem;
            set
            {
                _selectedMapItem = value;
                if (value == null) { return; }
                TextBoxMapName = value.MapName;
                TextBoxMapDescription = value.MapDescription;
                ImageMap = value.MapImage;
                OnPropertyChanged();
            }
        }

        private string _textBoxMapName;
        public string TextBoxMapName
        {
            get => _textBoxMapName;
            set
            {
                _textBoxMapName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxMapDescription;
        public string TextBoxMapDescription
        {
            get => _textBoxMapDescription;
            set
            {
                _textBoxMapDescription = value;
                OnPropertyChanged();
            }
        }

        private byte[] _imageMap;
        public byte[] ImageMap
        {
            get => _imageMap;
            set
            {
                _imageMap = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxSearch;
        public string TextBoxSearch
        {
            get => _textBoxSearch;
            set
            {
                _textBoxSearch = value;
                SearchMap();
                OnPropertyChanged();
            }
        }

        #endregion

        public AddMapWindowViewModel()
        {
            GetMapData();
            RefList();
        }

        #region Команды

        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand { get => _selectImageCommand ??= new(obj => SelectImage()); }

        private RelayCommand _saveMapItemCommand;
        public RelayCommand SaveMapItemCommand { get => _saveMapItemCommand ??= new(obj => AddMap()); }

        private RelayCommand _deleteMapItemCommand;
        public RelayCommand DeleteMapItemCommand { get => _deleteMapItemCommand ??= new(obj => DeleteMap()); }

        private RelayCommand _updateMapItemCommand;
        public RelayCommand UpdateMapItemCommand { get => _updateMapItemCommand ??= new(obj => UpdateMap()); }

        #endregion

        #region Методы 

        private void RefList()
        {
            MapList = new ObservableCollection<Map>();
        }

        private async void GetMapData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var maps = await context.Maps.ToListAsync();
                foreach (var item in maps)
                {
                    MapList.Add(item);
                }
            }
        }

        private void AddMap()
        {
            var newMap = new Map { MapName = TextBoxMapName, MapImage = ImageMap, MapDescription = TextBoxMapDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Maps.Any(map => map.MapName.ToLower() == newMap.MapName.ToLower());

                if (exists || string.IsNullOrEmpty(TextBoxMapName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.Maps.Add(newMap);
                    context.SaveChanges();
                    MapList.Clear();
                    GetMapData();
                    TextBoxMapName = string.Empty;
                    TextBoxMapDescription = string.Empty;
                    ImageMap = null;
                }
            }
        }

        private void UpdateMap()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedMapItem == null)
                {
                    return;
                }

                var entityToUpdate = context.Maps.Find(SelectedMapItem.IdMap);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedMapItem.MapName} на {TextBoxMapName} ? \n и {SelectedMapItem.MapDescription} на {TextBoxMapDescription} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.MapName = TextBoxMapName;
                        entityToUpdate.MapDescription = TextBoxMapDescription;
                        entityToUpdate.MapImage = ImageMap;
                        context.SaveChanges();
                        MapList.Clear();
                        GetMapData();
                        SelectedMapItem = null;
                        TextBoxMapName = string.Empty;
                        TextBoxMapDescription = string.Empty;
                        ImageMap = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
            }
        }

        private void DeleteMap()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.Maps.Find(SelectedMapItem.IdMap);
                if (entityToDelete != null)
                {
                    context.Maps.Remove(entityToDelete);
                    context.SaveChanges();
                    MapList.Clear();
                    GetMapData();
                }
            }
        }

        private void SearchMap()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var search = context.Maps.Where(map => map.MapName.Contains(TextBoxSearch));
                MapList.Clear();

                foreach (var item in search)
                {
                    MapList.Add(item);
                }
            }
        }

        private void SelectImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    ImageMap = ImageToByteArray(image);
                }
            }
        }

        private static byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream memoryStream = new())
            {
                image.Save(memoryStream, ImageFormat.Png);

                return memoryStream.ToArray();
            }
        }

        #endregion
    }


}
