using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
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
    internal class InteractionMeasurementVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IMeasurementService _measurementService;
        private readonly IMapService _mapService;

        public InteractionMeasurementVM(IWindowNavigationService windowNavigationService, IMeasurementService measurementService, IMapService mapService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _measurementService = measurementService;
            _mapService = mapService;

            GetMeasurementWithMaps();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<MeasurementWithMapsDTO> MeasurementWithMapsDTOs { get; set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Игровые измерение";

        #endregion

        #region Свойства : Selected

        private MeasurementWithMapsDTO _selectedMeasurementWithMapsDTO;
        public MeasurementWithMapsDTO SelectedMeasurementWithMapsDTO
        {
            get => _selectedMeasurementWithMapsDTO;
            set
            {
                _selectedMeasurementWithMapsDTO = value;
                if (value == null)
                    return;

                MeasurementName = value.MeasurementName;
                MeasurementDescription = value.MeasurementDescription;

                OnPropertyChanged();
            }
        }

        private MapDTO _selectedMap;
        public MapDTO SelectedMap
        {
            get => _selectedMap;
            set
            {
                _selectedMap = value;
                if (value == null)
                    return;

                MapName = value.MapName;
                MapImage = value.MapImage;
                MapDescription = value.MapDescription;

                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Измерение

        private string _measurementName;
        public string MeasurementName
        {
            get => _measurementName;
            set
            {
                _measurementName = value;
                OnPropertyChanged();
            }
        }

        private string _measurementDescription;
        public string MeasurementDescription
        {
            get => _measurementDescription;
            set
            {
                _measurementDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Карты  

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

        private byte[] _mapImage;
        public byte[] MapImage
        {
            get => _mapImage;
            set
            {
                _mapImage = value;
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

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _createMeasurementCommand;
        public RelayCommand CreateMeasurementCommand { get => _createMeasurementCommand ??= new(obj => { CreateMeasurement(); }); }

        private RelayCommand _deleteMeasurementCommand;
        public RelayCommand DeleteMeasurementCommand { get => _deleteMeasurementCommand ??= new(obj => { DeleteMeasurement(); }); }

        private RelayCommand _updateMeasurementCommand;
        public RelayCommand UpdateMeasurementCommand { get => _updateMeasurementCommand ??= new(obj => { UpdateMeasurement(); }); }


        private RelayCommand _createMapCommand;
        public RelayCommand CreateMapCommand { get => _createMapCommand ??= new(obj => { CreateMap(); }); }

        private RelayCommand _deleteMapCommand;
        public RelayCommand DeleteMapCommand { get => _deleteMapCommand ??= new(obj => { DeleteMap(); }); }

        private RelayCommand _updateMapCommand;
        public RelayCommand UpdateMapCommand { get => _updateMapCommand ??= new(obj => { UpdateMap(); }); }

        #endregion

        #region Выбор изображения | Очистка выбранного изображениея

        private RelayCommand _selectMapImageCommand;
        public RelayCommand SelectMapImageCommand { get => _selectMapImageCommand ??= new(obj => { SelectMapImage(); }); }

        private RelayCommand _clearMapImageCommand;
        public RelayCommand ClearMapImageCommand { get => _clearMapImageCommand ??= new(obj => { MapImage = null; }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private async void GetMeasurementWithMaps()
        {
            var measurementsWithMaps = await _measurementService.GetMeasurementsWithMapsAsync();

            foreach (var measurementWithMaps in measurementsWithMaps)
                MeasurementWithMapsDTOs.Add(measurementWithMaps);
        }

        // TODO : Изменить MessageBox на кастомное окно
        #region CRUD : Измерение

        private async void CreateMeasurement()
        {
            var (MeasurementDTO, Message) = await _measurementService.CreateAsync(MeasurementName, MeasurementDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                MeasurementWithMapsDTOs.Add(new MeasurementWithMapsDTO
                {
                    IdMeasurement = MeasurementDTO.IdMeasurement,
                    MeasurementDescription = MeasurementDTO.MeasurementDescription,
                    MeasurementName = MeasurementDTO.MeasurementName,
                    Maps = new ObservableCollection<MapDTO>()
                });
                ClearInputDataMeasurement();
            }
        }

        private async void UpdateMeasurement()
        {
            if (SelectedMeasurementWithMapsDTO == null)
                return;

            var (MeasurementDTO, Message) = await _measurementService.UpdateAsync(SelectedMeasurementWithMapsDTO.IdMeasurement, MeasurementName, MeasurementDescription);

            if (Message == string.Empty)
            {
                MeasurementWithMapsDTOs.ReplaceItem(SelectedMeasurementWithMapsDTO,
                                                    new MeasurementWithMapsDTO
                                                    {
                                                        IdMeasurement = MeasurementDTO.IdMeasurement,
                                                        MeasurementDescription = MeasurementDTO.MeasurementDescription,
                                                        MeasurementName = MeasurementDTO.MeasurementName,
                                                        Maps = SelectedMeasurementWithMapsDTO.Maps
                                                    });
                ClearInputDataMeasurement();
            }
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите обновить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedMeasurementWithMapsDTO = await _measurementService.ForcedUpdateAsync(SelectedMeasurementWithMapsDTO.IdMeasurement, MeasurementName, MeasurementDescription);
                    MeasurementWithMapsDTOs.ReplaceItem(SelectedMeasurementWithMapsDTO,
                                                        new MeasurementWithMapsDTO
                                                        {
                                                            IdMeasurement = MeasurementDTO.IdMeasurement,
                                                            MeasurementDescription = MeasurementDTO.MeasurementDescription,
                                                            MeasurementName = MeasurementDTO.MeasurementName,
                                                            Maps = SelectedMeasurementWithMapsDTO.Maps
                                                        });
                    ClearInputDataMeasurement();
                }
            }
        }

        private async void DeleteMeasurement()
        {
            if (SelectedMeasurementWithMapsDTO == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _measurementService.DeleteAsync(SelectedMeasurementWithMapsDTO.IdMeasurement);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    MeasurementWithMapsDTOs.Remove(SelectedMeasurementWithMapsDTO);
                    ClearInputDataMeasurement();
                }
            }          
        }

        #endregion
        // TODO : Изменить MessageBox на кастомное окно
        #region CRUD : Карта

        private async void CreateMap()
        {
            if (SelectedMeasurementWithMapsDTO == null)
                return;

            var (MapDTO, Message) = await _mapService.CreateAsync(idMeasurement: SelectedMeasurementWithMapsDTO.IdMeasurement,
                                                                  mapName: MapName,
                                                                  mapImage: MapImage,
                                                                  mapDescription: MapDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                SelectedMeasurementWithMapsDTO.Maps.Add(MapDTO);
                NotificationTransmittingValue(WindowName.AddMatch, MapDTO, TypeParameter.AddAndNotification);
                ClearInputDataMap();
            }
        }

        private async void UpdateMap()
        {
            if (SelectedMeasurementWithMapsDTO == null || SelectedMap == null)
                return;

            var (MapDTO, Message) = await _mapService.UpdateAsync(idMap: SelectedMap.IdMap,
                                                                        idMeasurement: SelectedMeasurementWithMapsDTO.IdMeasurement,
                                                                        mapName: MapName,
                                                                        mapImage: MapImage,
                                                                        mapDescription: MapDescription);

            if (Message == string.Empty)
            {
                SelectedMeasurementWithMapsDTO.Maps.ReplaceItem(SelectedMap, MapDTO);
                NotificationTransmittingValue(WindowName.AddMatch, MapDTO, TypeParameter.UpdateAndNotification);
                ClearInputDataMap();
            }
            else
                MessageBox.Show(Message);
        }

        private async void DeleteMap()
        {
            if (SelectedMeasurementWithMapsDTO == null || SelectedMap == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _mapService.DeleteAsync(SelectedMap.IdMap);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    NotificationTransmittingValue(WindowName.AddMatch, SelectedMap, TypeParameter.DeleteAndNotification);
                    SelectedMeasurementWithMapsDTO.Maps.Remove(SelectedMap);
                    ClearInputDataMap();
                }
            } 
        }

        #endregion
        //TODO : Заменить OpenFileDialog на сервис
        #region Выбор изображение

        private void SelectMapImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                MapImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        #endregion

        #region Очистка полей

        private void ClearInputDataMeasurement()
        {
            SelectedMap = null;
            MeasurementName = string.Empty;
            MeasurementDescription = string.Empty;
        }

        private void ClearInputDataMap()
        {
            MapName = string.Empty;
            MapImage = null;
            MapDescription = string.Empty;
        }

        #endregion
    }
}