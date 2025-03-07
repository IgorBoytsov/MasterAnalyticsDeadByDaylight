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
    internal class InteractionItemVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IItemService _itemService;
        private readonly IItemAddonService _itemAddonService;
        private readonly IRarityService _rarityService;

        public InteractionItemVM(IWindowNavigationService windowNavigationService, 
                                 IItemService itemService, 
                                 IItemAddonService itemAddonService,
                                 IRarityService rarityService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _itemService = itemService;
            _itemAddonService = itemAddonService;
            _rarityService = rarityService;

            foreach (var item in _rarityService.GetAll())
            {
                RaritiesDTO.Add(item);
            }

            Title = "Предметы и улучшения.";

            GetItemWithAddons();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            if (parameter is RarityDTO rarityAddDTO && typeParameter == TypeParameter.AddAndNotification)
            {
                RaritiesDTO.Add(rarityAddDTO);
                return;
            }

            if (parameter is ValueTuple<RarityDTO, string> parameters && typeParameter == TypeParameter.UpdateAndNotification)
            {
                (RarityDTO rarityUpdateDTO, string oldName) = parameters;

                RaritiesDTO.ReplaceItem(RaritiesDTO.FirstOrDefault(x => x.RarityName == oldName), rarityUpdateDTO);
                return;
            }

            if (parameter is int idRarity && typeParameter == TypeParameter.DeleteAndNotification)
            {
                RaritiesDTO.Remove(RaritiesDTO.FirstOrDefault(x => x.IdRarity == idRarity));
            }
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекция

        public ObservableCollection<ItemWithAddonsDTO> ItemWithAddonsDTOs { get; set; } = [];

        public ObservableCollection<RarityDTO> RaritiesDTO { get; set; } = [];

        #endregion

        #region Свойство : Title

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

        #region Свойства : Selected

        private ItemWithAddonsDTO _selectedItemWithAddonsDTO;
        public ItemWithAddonsDTO SelectedItemWithAddonsDTO
        {
            get => _selectedItemWithAddonsDTO;
            set
            {
                _selectedItemWithAddonsDTO = value;
                if (value == null)
                    return;

                ItemName = value.ItemName;
                ItemDescription = value.ItemDescription;
                ItemImage = value.ItemImage;

                ClearInputDataItemAddon();

                OnPropertyChanged();
            }
        }

        private ItemAddonDTO _selectedItemAddonDTO;
        public ItemAddonDTO SelectedItemAddonDTO
        {
            get => _selectedItemAddonDTO;
            set
            {
                _selectedItemAddonDTO = value;
                if (value == null)
                {
                    SelectedRarityDTO = null;
                    return;
                }
                    
                SelectedRarityDTO = RaritiesDTO.FirstOrDefault(x => x.IdRarity == value.IdRarity);
                ItemAddonName = value.ItemAddonName;
                ItemAddonDescription = value.ItemAddonDescription;
                ItemAddonImage = value.ItemAddonImage;

                OnPropertyChanged();
            }
        }
        
        #endregion

        #region Свойства : Предмета

        private string _itemName;
        public string ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                OnPropertyChanged();
            }
        }

        private string _itemDescription;
        public string ItemDescription
        {
            get => _itemDescription;
            set
            {
                _itemDescription = value;
                OnPropertyChanged();
            }
        }

        private byte[] _itemImage;
        public byte[] ItemImage
        {
            get => _itemImage;
            set
            {
                _itemImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Улучшение предмета

        private RarityDTO _selectedRarityDTO;
        public RarityDTO SelectedRarityDTO
        {
            get => _selectedRarityDTO;
            set
            {
                _selectedRarityDTO = value;
                OnPropertyChanged();
            }
        }

        private string _itemAddonName;
        public string ItemAddonName
        {
            get => _itemAddonName;
            set
            {
                _itemAddonName = value;
                OnPropertyChanged();
            }
        }

        private string _itemAddonDescription;
        public string ItemAddonDescription
        {
            get => _itemAddonDescription;
            set
            {
                _itemAddonDescription = value;
                OnPropertyChanged();
            }
        }

        private byte[] _itemAddonImage;
        public byte[] ItemAddonImage
        {
            get => _itemAddonImage;
            set
            {
                _itemAddonImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _createItemCommand;
        public RelayCommand CreateItemCommand { get => _createItemCommand ??= new(obj => { CreateItem(); }); }
        
        private RelayCommand _updateItemCommand;
        public RelayCommand UpdateItemCommand { get => _updateItemCommand ??= new(obj => { UpdateItem(); }); }
        
        private RelayCommand _deleteItemCommand;
        public RelayCommand DeleteItemCommand { get => _deleteItemCommand ??= new(obj => { DeleteItem(); }); }


        private RelayCommand _createItemAddonCommand;
        public RelayCommand CreateItemAddonCommand { get => _createItemAddonCommand ??= new(obj => { CreateItemAddon(); }); }

        private RelayCommand _updateItemAddonCommand;
        public RelayCommand UpdateItemAddonCommand { get => _updateItemAddonCommand ??= new(obj => { UpdateItemAddon(); }); }

        private RelayCommand _deleteItemAddonCommand;
        public RelayCommand DeleteItemAddonCommand { get => _deleteItemAddonCommand ??= new(obj => { DeleteItemAddon(); }); }

        #endregion

        #region Открытие добавление качества

        private RelayCommand _addRarityCommand;
        public RelayCommand AddRarityCommand { get => _addRarityCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionRarity, null, TypeParameter.None, true); }); }

        #endregion

        #region Выбор изображения | Очистка выбранного изображениея

        private RelayCommand _selectItemImageCommand;
        public RelayCommand SelectItemImageCommand { get => _selectItemImageCommand ??= new(obj => { SelectItemImage(); }); }

        private RelayCommand _clearItemImageCommand;
        public RelayCommand ClearItemImageCommand { get => _clearItemImageCommand ??= new(obj => { ItemImage = null; }); }

        private RelayCommand _selectItemAddonImageCommand;
        public RelayCommand SelectItemAddonImageCommand { get => _selectItemAddonImageCommand ??= new(obj => { SelectItemAddonImage(); }); }

        private RelayCommand _clearItemAddonImageCommand;
        public RelayCommand ClearItemAddonImageCommand { get => _clearItemAddonImageCommand ??= new(obj => { ItemAddonImage = null; }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/
        
        private async void GetItemWithAddons()
        {
            var itemWithAddons = await _itemService.GetItemsWithAddonsAsync();

            foreach (var item in itemWithAddons)
            {
                ItemWithAddonsDTOs.Add(item);
            }
        }

        //TODO : Заменить MessageBox на кастомное окно
        #region CRUD : Предмет

        private async void CreateItem()
        {
            var (ItemDTO, Message) = await _itemService.CreateAsync(ItemName, ItemImage, ItemDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                ItemWithAddonsDTOs.Add(new ItemWithAddonsDTO 
                { 
                    IdItem = ItemDTO.IdItem, 
                    ItemName = ItemDTO.ItemName, 
                    ItemImage = ItemDTO.ItemImage, 
                    ItemDescription = ItemDTO.ItemDescription, 
                    ItemAddons = new ObservableCollection<ItemAddonDTO>() 
                });

                ClearInputDataItem();
            }
        }

        private async void UpdateItem()
        {
            if (SelectedItemWithAddonsDTO == null)
                return;

            var (ItemDTO, Message) = await _itemService.UpdateAsync(SelectedItemWithAddonsDTO.IdItem, ItemName, ItemImage, ItemDescription);

            if (Message == string.Empty)
            {
                ItemWithAddonsDTOs.ReplaceItem(SelectedItemWithAddonsDTO,
                                               new ItemWithAddonsDTO
                                               {
                                                   IdItem = ItemDTO.IdItem,
                                                   ItemName = ItemDTO.ItemName,
                                                   ItemImage = ItemDTO.ItemImage,
                                                   ItemDescription = ItemDTO.ItemDescription,
                                                   ItemAddons = SelectedItemWithAddonsDTO.ItemAddons,
                                               });
                ClearInputDataItem();
            }
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите обновить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedItemWithAddonDTO = await _itemService.ForcedUpdateAsync(SelectedItemWithAddonsDTO.IdItem, ItemName, ItemImage, ItemDescription);
                    ItemWithAddonsDTOs.ReplaceItem(SelectedItemWithAddonsDTO,
                                                   new ItemWithAddonsDTO
                                                   {
                                                       IdItem = ItemDTO.IdItem,
                                                       ItemName = ItemDTO.ItemName,
                                                       ItemImage = ItemDTO.ItemImage,
                                                       ItemDescription = ItemDTO.ItemDescription,
                                                       ItemAddons = SelectedItemWithAddonsDTO.ItemAddons,
                                                   });
                    ClearInputDataItem();
                }
            }
        }

        private async void DeleteItem()
        {
            if (SelectedItemWithAddonsDTO == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _itemService.DeleteAsync(SelectedItemWithAddonsDTO.IdItem);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    ItemWithAddonsDTOs.Remove(SelectedItemWithAddonsDTO);
                    ClearInputDataItem();
                }
            }
        }

        #endregion

        #region CRUD : Улучшение

        private async void CreateItemAddon()
        {
            if (SelectedItemWithAddonsDTO == null)
            {
                MessageBox.Show("Не выбрал предмет.");
                return;
            }

            if (SelectedRarityDTO == null)
            {
                MessageBox.Show("Не выбрали качество.");
                return;
            }

            var (ItemAddonDTO, Message) = await _itemAddonService.CreateAsync(SelectedItemWithAddonsDTO.IdItem, SelectedRarityDTO.IdRarity, ItemAddonName, ItemAddonImage, ItemAddonDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                SelectedItemWithAddonsDTO.ItemAddons.Add(ItemAddonDTO);
                ClearInputDataItemAddon();
            } 
        }

        private async void UpdateItemAddon()
        {
            if (SelectedItemWithAddonsDTO == null || SelectedItemAddonDTO == null)
                return;

            if (SelectedRarityDTO == null)
            {
                MessageBox.Show("Вы не выбрали редкость");
                return;
            }

            var (ItemAddonDTO, Message) = await _itemAddonService.UpdateAsync(SelectedItemAddonDTO.IdItemAddon, 
                                                                              SelectedItemWithAddonsDTO.IdItem, 
                                                                              SelectedRarityDTO.IdRarity, 
                                                                              ItemAddonName, 
                                                                              ItemAddonImage, 
                                                                              ItemAddonDescription);

            if (Message == string.Empty)
            {      
                SelectedItemWithAddonsDTO.ItemAddons.ReplaceItem(SelectedItemAddonDTO, ItemAddonDTO);
                ClearInputDataItemAddon();
            }
            else
                MessageBox.Show(Message);
        }

        private async void DeleteItemAddon()
        {
            if (SelectedItemWithAddonsDTO == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _itemAddonService.DeleteAsync(SelectedItemAddonDTO.IdItemAddon);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    SelectedItemWithAddonsDTO.ItemAddons.Remove(SelectedItemAddonDTO);
                    ClearInputDataItemAddon();
                }
            }
        }

        #endregion

        //TODO : Заменить прямой вызов OpenFileDialog на вызов из сервиса
        #region Выбор изображения

        private void SelectItemImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                ItemImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        private void SelectItemAddonImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                ItemAddonImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        #endregion

        #region Очистка полей

        private void ClearInputDataItem()
        {
            SelectedItemAddonDTO = null;
            ItemName = string.Empty;
            ItemImage = null;
            ItemDescription = string.Empty;
        }

        private void ClearInputDataItemAddon()
        {
            ItemAddonName = string.Empty;
            ItemAddonImage = null;
            ItemAddonDescription = string.Empty;
        }

        #endregion

    }
}