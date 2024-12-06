using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    class AddItemWindowViewModel : BaseViewModel, IUpdatable
    {
        #region Свойства

        public ObservableCollection<Item> ItemList { get; set; } = [];

        private Item _selectedItem;
        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == null) { return; }
                _selectedItem = value;

                ItemName = value.ItemName;
                ImageItem = value.ItemImage;
                ImageItemAddon = null;
                GetItemAddonData();

                ItemAddonName = string.Empty;
                ItemAddonDescription = string.Empty;

                OnPropertyChanged();
            }
        }

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

        private byte[] _imageItem;
        public byte[] ImageItem
        {
            get => _imageItem;
            set
            {
                _imageItem = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ItemAddon> ItemAddonList { get; set; } = [];

        private ItemAddon _selectedItemAddon;
        public ItemAddon SelectedItemAddon
        {
            get => _selectedItemAddon;
            set
            {
                if (value == null) { return; }
                _selectedItemAddon = value;

                ItemAddonName = value.ItemAddonName;
                ItemAddonDescription = value.ItemAddonDescription;
                ImageItemAddon = value.ItemAddonImage;
                SelectedRarity = RarityList.FirstOrDefault(r => r.IdRarity == value.IdRarity);
                ComboBoxSelectedItem = ItemList.FirstOrDefault(r => r.IdItem == value.IdItem);
                OnPropertyChanged();
            }
        }

        private Item _comboBoxSelectedItem;
        public Item ComboBoxSelectedItem
        {
            get => _comboBoxSelectedItem;
            set
            {
                _comboBoxSelectedItem = value;
                if (value == null) { return; }
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

        private byte[] _imageItemAddon;
        public byte[] ImageItemAddon
        {
            get => _imageItemAddon;
            set
            {
                _imageItemAddon = value;
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

        public ObservableCollection<Rarity> RarityList { get; set; } = [];

        private Rarity _selectedRarity;
        public Rarity SelectedRarity
        {
            get => _selectedRarity;
            set
            {
                _selectedRarity = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dateService;

        public AddItemWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dateService = _serviceProvider.GetService<IDataService>();


            Title = "Добавление предмета";

            GetItemData();
            GetItemAddonData();
            GetRarityData();
        }

        public void Update(object value)
        {

        }

        #region Команды

        private RelayCommand _selectImageItemCommand;
        public RelayCommand SelectImageItemCommand { get => _selectImageItemCommand ??= new(obj => { SelectImageItem(); }); } 
        
        private RelayCommand _clearItemImageCommand;
        public RelayCommand ClearItemImageCommand { get => _clearItemImageCommand ??= new(obj => { ImageItem = null; }); }

        private RelayCommand _selectImageItemAddonCommand;
        public RelayCommand SelectImageItemAddonCommand { get => _selectImageItemAddonCommand ??= new(obj => { SelectImageItemAddon(); }); }

        private RelayCommand _clearAddonImageCommand;
        public RelayCommand ClearAddonImageCommand { get => _clearAddonImageCommand ??= new(obj => { ImageItemAddon = null; }); }


        private RelayCommand _saveItemCommand;
        public RelayCommand SaveItemCommand { get => _saveItemCommand ??= new(obj => { AddItem(); }); }

        private RelayCommand _updateItemCommand;
        public RelayCommand UpdateItemCommand { get => _updateItemCommand ??= new(obj => { UpdateItem(); }); }

        private RelayCommand _deleteItemCommand;
        public RelayCommand DeleteItemCommand { get => _deleteItemCommand ??= new(obj => { DeleteItem(); }); }


        private RelayCommand _saveItemAddonCommand;
        public RelayCommand SaveItemAddonCommand { get => _saveItemAddonCommand ??= new(obj => { AddItemAddon(); }); }

        private RelayCommand _updateItemAddonCommand;
        public RelayCommand UpdateItemAddonCommand { get => _updateItemAddonCommand ??= new(obj => { UpdateItemAddon(); }); }

        private RelayCommand _deleteItemAddonCommand;
        public RelayCommand DeleteKillerAddonCommand
        {
            get => _deleteItemAddonCommand ??= new(async obj =>
            {
                if (SelectedItemAddon == null) return;

                if (MessageHelper.MessageDelete(SelectedItemAddon.ItemAddonName) == MessageButtons.Yes)
                {
                    await _dateService.RemoveAsync(SelectedItemAddon);
                    GetItemAddonData();
                } else return;
            });
        }

        #endregion

        #region Методы

        private async void GetItemData()
        {
            var items = await _dateService.GetAllDataAsync<Item>();

            ItemList.Clear();
            foreach (var item in items)
            {
                ItemList.Add(item);
            }

            SelectedItem = ItemList.FirstOrDefault();
        }    

        private async void GetRarityData()
        {
            var items = await _dateService.GetAllDataAsync<Rarity>();

            ItemList.Clear();
            foreach (var item in items)
            {
                RarityList.Add(item);
            }
        }

        private async void AddItem()
        {
            var newItem = new Item() { ItemName = ItemName, ItemImage = ImageItem, ItemDescription = ItemDescription };

            bool exists = await _dateService.ExistsAsync<Item>(x => x.ItemName.ToLower() == newItem.ItemName.ToLower());

            if (exists || string.IsNullOrWhiteSpace(ItemName))
            {
                MessageHelper.MessageExist();
            }
            else
            {
                await _dateService.AddAsync(newItem);
                GetItemData();
                ItemName = string.Empty;
                ItemDescription = string.Empty;
                ImageItem = null;
                SelectedItem = null;
            }
        }

        private async void UpdateItem()
        {

            if (SelectedItem == null) return;

            var entityToUpdate = await _dateService.FindAsync<Item>(SelectedItem.IdItem);

            if (entityToUpdate != null)
            {
                if (MessageHelper.MessageUpdate(SelectedItem.ItemName, ItemName, SelectedItem.ItemDescription, ItemAddonDescription) == MessageButtons.Yes)
                {
                    entityToUpdate.ItemName = ItemName;
                    entityToUpdate.ItemDescription = ItemDescription;
                    entityToUpdate.ItemImage = ImageItem;
                    await _dateService.UpdateAsync(entityToUpdate);

                    ItemList.Clear();
                    GetItemData();

                    ItemName = string.Empty;
                    ItemDescription = string.Empty;
                    ImageItem = null;
                    SelectedItem = null;
                }
            }
            else { MessageBox.Show("Нечего обновлять"); }
        }

        private async void DeleteItem()
        {
            if (MessageHelper.MessageDelete(SelectedItem.ItemName) == MessageButtons.Yes)
            {
                var itemToDelete = await _dateService.FindAsync<Item>(SelectedItem.IdItem);
                if (itemToDelete != null)
                {
                    var itemAddonsToDelete = await _dateService.GetAllDataAsync<ItemAddon>(x => x.Where(itemAddon => itemAddon.IdItem == SelectedItem.IdItem)); /*context.ItemAddons.Where(itemAddon => itemAddon.IdItem == SelectedItem.IdItem);*/
                    if (itemAddonsToDelete != null)
                    {
                        await _dateService.RemoveRangeAsync<ItemAddon>(itemAddonsToDelete);
                        GetItemAddonData();
                    }

                    await _dateService.RemoveAsync<Item>(itemToDelete);
                    GetItemData();
                }
            }
        }

        private async void GetItemAddonData()
        {
            if (SelectedItem == null) ItemAddonList.Clear();
            else
            {
               var addons = await _dateService.GetAllDataAsync<ItemAddon>(x => x.Include(x => x.IdRarityNavigation).Where(x => x.IdItem == SelectedItem.IdItem));
               ItemAddonList.Clear();

               foreach (var item in addons)
               {
                   ItemAddonList.Add(item);
               }
               SelectedItemAddon = ItemAddonList.FirstOrDefault(x => x.IdItem == SelectedItem.IdItem);
            }
        }

        private async void AddItemAddon()
        {
            var newItemAddon = new ItemAddon() { IdItem = SelectedItem.IdItem, ItemAddonName = ItemAddonName, ItemAddonDescription = ItemAddonDescription, ItemAddonImage = ImageItemAddon, IdRarity = SelectedRarity.IdRarity };

            bool exists = ItemAddonList.Any(x => x.ItemAddonName.ToLower() == newItemAddon.ItemAddonName.ToLower());

            if (exists || string.IsNullOrWhiteSpace(ItemName))
            {
               MessageHelper.MessageExist();
            }
            else
            {
                await _dateService.AddAsync(newItemAddon);
                GetItemAddonData();
                ItemAddonName = string.Empty;
                ItemAddonDescription = string.Empty;
                ImageItemAddon = null;
                SelectedItemAddon = null;
            }
        }

        private async void UpdateItemAddon()
        {
            if (SelectedItemAddon == null) return;

            var entityToUpdate = await _dateService.FindAsync<ItemAddon>(SelectedItemAddon.IdItemAddon);

            if (entityToUpdate != null)
            {
                if (MessageHelper.MessageUpdate(SelectedItemAddon.ItemAddonName, ItemAddonName, SelectedItemAddon.ItemAddonDescription, ItemAddonDescription) == MessageButtons.Yes)
                {
                    entityToUpdate.IdItem = ComboBoxSelectedItem.IdItem;
                    entityToUpdate.ItemAddonName = ItemAddonName;
                    entityToUpdate.ItemAddonDescription = ItemAddonDescription;
                    entityToUpdate.ItemAddonImage = ImageItemAddon;
                    entityToUpdate.IdRarity = SelectedRarity.IdRarity;
                    await _dateService.UpdateAsync(entityToUpdate);

                    GetItemAddonData();

                    ItemAddonName = string.Empty;
                    ItemAddonDescription = string.Empty;
                    ImageItemAddon = null;
                    SelectedItemAddon = null;
                }
            }
        }

        private void SelectImageItem()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    ImageItem = ImageHelper.ImageToByteArray(image);
                }
            }
        }

        private void SelectImageItemAddon()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    ImageItemAddon = ImageHelper.ImageToByteArray(image);
                }
            }
        }

        #endregion
    }
}
