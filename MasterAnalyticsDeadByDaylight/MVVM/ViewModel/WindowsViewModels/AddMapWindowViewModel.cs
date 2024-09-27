using LiveCharts.Wpf;
using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
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

        public ObservableCollection<Map> MapList { get; set; } = [];

        public ObservableCollection<Measurement> MeasurementList { get; set; } = [];

        private Map _selectedMapItem;
        public Map SelectedMapItem
        {
            get => _selectedMapItem;
            set
            {
                _selectedMapItem = value;
                if (value == null) { return; }
                MapName = value.MapName;
                MapDescription = value.MapDescription;
                ImageMap = value.MapImage;
                SelectedMeasurementItem = MeasurementList.FirstOrDefault(m => m.IdMeasurement == value.IdMeasurement);
                OnPropertyChanged();
            }
        }

        private Measurement _selectedMeasurementItem;
        public Measurement SelectedMeasurementItem
        {
            get => _selectedMeasurementItem;
            set
            {
                _selectedMeasurementItem = value;
                OnPropertyChanged();
            }
        }

        private string _mapName;
        public string MapName
        {
            get => _mapName;
            set
            {
                _mapName = value;
                OnPropertyChanged();
            }
        }

        private string _mapDescription;
        public string MapDescription
        {
            get => _mapDescription;
            set
            {
                _mapDescription = value;
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

        private string _mapSearch;
        public string MapSearch
        {
            get => _mapSearch;
            set
            {
                _mapSearch = value;
                SearchMap();
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

        public AddMapWindowViewModel(ICustomDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;
            Title = "Добавление карт";
            GetMapData();
            GetMeasurementData();
        }

        #region Команды

        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand { get => _selectImageCommand ??= new(obj => SelectImage()); }

        private RelayCommand _saveMapItemCommand;
        public RelayCommand SaveMapItemCommand { get => _saveMapItemCommand ??= new(obj => AddMap()); }

        private RelayCommand _deleteMapItemCommand;
        public RelayCommand DeleteMapItemCommand
        {
            get => _deleteMapItemCommand ??= new(async obj =>
            {
                if (SelectedMapItem == null) return;

                if (MessageHelper.MessageDelete(SelectedMapItem.MapName) == MessageButtons.Yes)
                {
                    await _dataService.RemoveAsync(SelectedMapItem);
                    GetMapData();
                }
            });
        }

        private RelayCommand _updateMapItemCommand;
        public RelayCommand UpdateMapItemCommand { get => _updateMapItemCommand ??= new(obj => UpdateMap()); }

        private RelayCommand _clearImageCommand;
        public RelayCommand ClearImageCommand { get => _clearImageCommand ??= new(obj => { ImageMap = null; }); }

        #endregion

        #region Методы 

        private async void GetMapData()
        {
            var maps = await _dataService.GetAllDataAsync<Map>();
            foreach (var item in maps)
            {
                MapList.Add(item);
            }
        }

        private async void GetMeasurementData()
        {
            var measurements = await _dataService.GetAllDataAsync<Measurement>();
            foreach (var item in measurements)
            {
                MeasurementList.Add(item);
            }
        }

        private async void AddMap()
        {
            var newMap = new Map { MapName = MapName, MapImage = ImageMap, MapDescription = MapDescription, IdMeasurement = SelectedMeasurementItem.IdMeasurement };

            bool exists = await _dataService.ExistsAsync<Map>(x => x.MapName.ToLower() == newMap.MapName.ToLower());

            if (exists || string.IsNullOrEmpty(MapName))
            {
                MessageHelper.MessageExist();
            }
            else
            {
                await _dataService.AddAsync(newMap);
                MapList.Clear();
                GetMapData();
                MapName = string.Empty;
                MapDescription = string.Empty;
                ImageMap = null;
            }
        }
 
        private async void UpdateMap()
        {
            if (SelectedMapItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<Map>(SelectedMapItem.IdMap);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<Map>(x => x.MapName.ToLower() == MapName.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedMapItem.MapName, MapName, SelectedMapItem.MapDescription, MapDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.MapName = MapName;
                        entityToUpdate.MapDescription = MapDescription;
                        entityToUpdate.MapImage = ImageMap;
                        entityToUpdate.IdMeasurement = SelectedMeasurementItem.IdMeasurement;
                        await _dataService.UpdateAsync(entityToUpdate);
                        MapList.Clear();
                        GetMapData();
                        SelectedMapItem = null;
                        MapName = string.Empty;
                        MapDescription = string.Empty;
                        ImageMap = null;
                    }
                }
                else
                {
                    entityToUpdate.MapName = MapName;
                    entityToUpdate.MapDescription = MapDescription;
                    entityToUpdate.MapImage = ImageMap;
                    entityToUpdate.IdMeasurement = SelectedMeasurementItem.IdMeasurement;
                    await _dataService.UpdateAsync(entityToUpdate);
                    MapList.Clear();
                    GetMapData();
                    SelectedMapItem = null;
                    MapName = string.Empty;
                    MapDescription = string.Empty;
                    ImageMap = null;
                }
            }
        }

        private async void SearchMap()
        {
            var search = await _dataService.GetAllDataAsync<Map>(x => x
            .Include(x => x.IdMeasurementNavigation)
            .Where(x => x.MapName.Contains(MapSearch) || 
            x.IdMeasurementNavigation.MeasurementName.Contains(MapSearch)));

            MapList.Clear();
            foreach (var item in search)
            {
                MapList.Add(item);
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
