using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    class AddItemWindowViewModel : BaseViewModel
    {
        #region Свойства

        public ObservableCollection<Item> ItemList { get; set; }

        private Item _selectedItem;
        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (value == null) { return; }

                TextBoxItemName = value.ItemName;
                ImageItem = value.ItemImage;
                ComboBoxSelectedItem = value;

                GetItemAddonData();

                TextBoxItemAddonName = string.Empty;
                TextBoxItemAddonDescription = string.Empty;
                ImageItemAddon = null;

                OnPropertyChanged();
            }
        }

        private string _textBoxItemName;
        public string TextBoxItemName
        {
            get => _textBoxItemName;
            set
            {
                _textBoxItemName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxItemDescription;
        public string TextBoxItemDescription
        {
            get => _textBoxItemDescription;
            set
            {
                _textBoxItemDescription = value;
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

        public ObservableCollection<ItemAddon> ItemAddonList { get; set; }

        private ItemAddon _selectedItemAddon;
        public ItemAddon SelectedItemAddon
        {
            get => _selectedItemAddon;
            set
            {
                _selectedItemAddon = value;
                if (value == null) { return; }

                TextBoxItemAddonName = value.ItemAddonName;
                TextBoxItemAddonDescription = value.ItemAddonDescription;
                ImageItemAddon = value.ItemAddonImage;
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
                GetItemAddonData();
                OnPropertyChanged();
            }
        }

        private string _textBoxItemAddonName;
        public string TextBoxItemAddonName
        {
            get => _textBoxItemAddonName;
            set
            {
                _textBoxItemAddonName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxItemAddonDescription;
        public string TextBoxItemAddonDescription
        {
            get => _textBoxItemAddonDescription;
            set
            {
                _textBoxItemAddonDescription = value;
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

        public ObservableCollection<Rarity> RarityList {  get; set; }

        private Rarity _comboBoxSelectedRarity;
        public Rarity ComboBoxSelectedRarity
        {
            get => _comboBoxSelectedRarity;
            set
            {
                _comboBoxSelectedRarity = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public AddItemWindowViewModel() 
        { 
            ItemList = new ObservableCollection<Item>();
            ItemAddonList = new ObservableCollection<ItemAddon>();
            RarityList = new ObservableCollection<Rarity>();

            GetItemData();
            GetItemAddonData();
            GetRarityData();
        }

        #region Команды

        private RelayCommand _selectImageItemCommand;
        public RelayCommand SelectImageItemCommand { get => _selectImageItemCommand ??= new(obj => { SelectImageItem(); }); }

        private RelayCommand _selectImageItemAddonCommand;
        public RelayCommand SelectImageItemAddonCommand { get => _selectImageItemAddonCommand ??= new(obj => { SelectImageItemAddon(); }); }


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
        public RelayCommand DeleteItemAddonCommand { get => _deleteItemAddonCommand ??= new(obj => { DeleteItemAddon(); }); }

        #endregion

        #region Методы

        private async void GetItemData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var items = await context.Items.ToListAsync();
                ItemList.Clear();

                foreach (var item in items)
                {
                    ItemList.Add(item);
                }
            }
        }

        private async void GetRarityData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var items = await context.Rarities.ToListAsync();
                ItemList.Clear();

                foreach (var item in items)
                {
                    RarityList.Add(item);
                }
            }
        }

        private void AddItem()
        {
            var newItem = new Item() { ItemName = TextBoxItemName, ItemImage = ImageItem, ItemDescription = TextBoxItemDescription};

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Items.Any(item => item.ItemName.ToLower() == newItem.ItemName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(TextBoxItemName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.Items.Add(newItem);
                    context.SaveChanges();
                    GetItemData();
                    TextBoxItemName = string.Empty;
                    TextBoxItemDescription = string.Empty;
                    ImageItem = null;
                    SelectedItem = null;
                }
            }
        }

        private void UpdateItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedItem == null)
                {
                    return;
                }

                var entityToUpdate = context.Items.Find(SelectedItem.IdItem);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedItem.ItemName} на {TextBoxItemName} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.ItemName = TextBoxItemName;
                        entityToUpdate.ItemName = TextBoxItemDescription;
                        entityToUpdate.ItemImage = ImageItem;
                        context.SaveChanges();

                        GetItemData();

                        TextBoxItemName = string.Empty;
                        TextBoxItemDescription = string.Empty;
                        ImageItem = null;
                        SelectedItem = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
            }
        }

        private void DeleteItem()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var itemToDelete = context.Items.Find(SelectedItem.IdItem);
                if (itemToDelete != null)
                {
                    var itemAddonToDelete = context.ItemAddons.Where(itemAddon => itemAddon.IdItem == SelectedItem.IdItem);
                    if (itemAddonToDelete != null)
                    {
                        context.ItemAddons.RemoveRange(itemAddonToDelete);
                        context.SaveChanges();
                        GetItemAddonData();
                    }

                    context.Items.Remove(itemToDelete);
                    context.SaveChanges();
                    GetItemData();
                }
            }
        }

        private async void GetItemAddonData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<ItemAddon> Addons = new();

                if (SelectedItem == null)
                {
                    ItemAddonList.Clear();
                }
                else
                {
                    Addons = await context.ItemAddons.Include(rarity => rarity.IdRarityNavigation).Where(ia => ia.IdItem == SelectedItem.IdItem).ToListAsync();
                }

                ItemAddonList.Clear();

                foreach (var item in Addons)
                {
                    ItemAddonList.Add(item);
                }
            }
        }

        private void AddItemAddon()
        {
            var newItemAddon = new ItemAddon() { IdItem = ComboBoxSelectedItem.IdItem, ItemAddonName = TextBoxItemAddonName, ItemAddonDescription = TextBoxItemAddonDescription, ItemAddonImage = ImageItemAddon, IdRarity = ComboBoxSelectedRarity.IdRarity};

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.ItemAddons.Where(ia => ia.IdItem == ComboBoxSelectedItem.IdItem).Any(itemAddon => itemAddon.ItemAddonName.ToLower() == newItemAddon.ItemAddonName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(TextBoxItemName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.ItemAddons.Add(newItemAddon);
                    context.SaveChanges();
                    GetItemAddonData();
                    TextBoxItemAddonName = string.Empty;
                    TextBoxItemAddonDescription = string.Empty;
                    ImageItemAddon = null;
                    SelectedItemAddon = null;
                }
            }
        }

        private void UpdateItemAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedItemAddon == null)
                {
                    return;
                }

                var entityToUpdate = context.ItemAddons.Find(SelectedItemAddon.IdItemAddon);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedItemAddon.ItemAddonName} на {TextBoxItemAddonName} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.IdItem = ComboBoxSelectedItem.IdItem;
                        entityToUpdate.ItemAddonName = TextBoxItemAddonName;
                        entityToUpdate.ItemAddonDescription = TextBoxItemAddonDescription;
                        entityToUpdate.ItemAddonImage = ImageItemAddon;
                        entityToUpdate.IdRarity = ComboBoxSelectedRarity.IdRarity;
                        context.SaveChanges();

                        GetItemAddonData();

                        TextBoxItemAddonName = string.Empty;
                        TextBoxItemAddonDescription = string.Empty;
                        ImageItemAddon = null;
                        SelectedItemAddon = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
            }
        }

        private void DeleteItemAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var itemToDelete = context.ItemAddons.Find(SelectedItemAddon.IdItemAddon);
                if (itemToDelete != null)
                {
                    context.ItemAddons.Remove(itemToDelete);
                    GetItemAddonData();
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
