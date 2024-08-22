using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.View.Pages;
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
                Task.Run(SearchMap);
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

        public AddMapWindowViewModel(IDialogService service)
        {
            _dialogService = service;
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
                if (SelectedMapItem == null)
                {
                    return;
                }
                if (_dialogService.ShowMessageButtons(
                    $"Вы точно хотите удалить «{SelectedMapItem.MapName}»? При удаление данном записи, могут быть связанные записи в других таблицах, что может привести к проблемам.",
                    "Предупреждение об удаление.",
                    TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
                {
                    await DataBaseHelper.DeleteEntityAsync(SelectedMapItem);
                    GetMapData();
                }
                else
                {
                    return;
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
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var maps = await context.Maps.ToListAsync();
                foreach (var item in maps)
                {
                    MapList.Add(item);
                }
            }
        }

        private async void GetMeasurementData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var measurements = await context.Measurements.ToListAsync();
                foreach (var item in measurements)
                {
                    MeasurementList.Add(item);
                }
            }
        }

        private void AddMap()
        {
            var newMap = new Map { MapName = MapName, MapImage = ImageMap, MapDescription = MapDescription, IdMeasurement = SelectedMeasurementItem.IdMeasurement };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Maps.Any(map => map.MapName.ToLower() == newMap.MapName.ToLower());

                if (exists || string.IsNullOrEmpty(MapName))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Совпадение имени");
                }
                else
                {
                    context.Maps.Add(newMap);
                    context.SaveChanges();
                    MapList.Clear();
                    GetMapData();
                    MapName = string.Empty;
                    MapDescription = string.Empty;
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
                    bool exists = context.Maps.Any(x => x.MapName.ToLower() == MapName.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                            $"Вы точно хотите обновить ее? Если да, то будет произведена замена имени с «{SelectedMapItem.MapName}» на «{MapName}» ?",
                            $"Надпись с именем «{SelectedMapItem.MapName}» уже существует.",
                            TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.MapName = MapName;
                            entityToUpdate.MapDescription = MapDescription;
                            entityToUpdate.MapImage = ImageMap;
                            entityToUpdate.IdMeasurement = SelectedMeasurementItem.IdMeasurement;
                            context.SaveChanges();
                            MapList.Clear();
                            GetMapData();
                            SelectedMapItem = null;
                            MapName = string.Empty;
                            MapDescription = string.Empty;
                            ImageMap = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.MapName = MapName;
                        entityToUpdate.MapDescription = MapDescription;
                        entityToUpdate.MapImage = ImageMap;
                        entityToUpdate.IdMeasurement = SelectedMeasurementItem.IdMeasurement;
                        context.SaveChanges();
                        MapList.Clear();
                        GetMapData();
                        SelectedMapItem = null;
                        MapName = string.Empty;
                        MapDescription = string.Empty;
                        ImageMap = null;
                    }    
                }
            }
        }

        private async Task SearchMap()
        {
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var search = context.Maps.Include(x => x.IdMeasurementNavigation).Where(map => map.MapName.Contains(MapSearch))
                        .Union(context.Maps.Include(x => x.IdMeasurementNavigation).Where(x => x.IdMeasurementNavigation.MeasurementName.Contains(MapSearch)));

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        MapList.Clear();
                        foreach (var item in search)
                        {
                            MapList.Add(item);
                        }
                    });
                }
            });
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
