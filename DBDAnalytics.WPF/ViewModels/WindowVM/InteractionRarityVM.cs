using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class InteractionRarityVM : BaseVM, IUpdatable
    {
        private readonly IRarityService _rarityService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionRarityVM(IWindowNavigationService windowNavigationService, IRarityService rarityService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _rarityService = rarityService;

            GetRarities();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<RarityDTO> Rarities { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Редкость";

        #endregion

        #region Свойства : Selected | RarityName

        private RarityDTO _selectedPlayerRarity;
        public RarityDTO SelectedRarity
        {
            get => _selectedPlayerRarity;
            set
            {
                if (_selectedPlayerRarity != value)
                {
                    _selectedPlayerRarity = value;

                    RarityName = value?.RarityName;

                    OnPropertyChanged();
                }
            }
        }

        private string _rarityName;
        public string RarityName
        {
            get => _rarityName;
            set
            {
                _rarityName = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addRarityCommand;
        public RelayCommand AddRarityCommand { get => _addRarityCommand ??= new(obj => { AddRarity(); }); }

        private RelayCommand _deleteRarityCommand;
        public RelayCommand DeleteRarityCommand { get => _deleteRarityCommand ??= new(obj => { DeleteRarity(); }); }

        private RelayCommand _updateRarityCommand;
        public RelayCommand UpdateRarityCommand { get => _updateRarityCommand ??= new(obj => { UpdateRarity(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private async void GetRarities()
        {
            var rarities = await _rarityService.GetAllAsync();

            foreach (var rarity in rarities)
                Rarities.Add(rarity);
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddRarity()
        {
            var newRarityDTO = await _rarityService.CreateAsync(RarityName);

            if (newRarityDTO.Message != string.Empty)
            {
                MessageBox.Show(newRarityDTO.Message);
                return;
            }
            else
            {
                NotificationTransmittingValue(WindowName.InteractionItem, newRarityDTO.RarityDTO, TypeParameter.AddAndNotification);
                NotificationTransmittingValue(WindowName.InteractionOffering, newRarityDTO.RarityDTO, TypeParameter.AddAndNotification);
                NotificationTransmittingValue(WindowName.InteractionKiller, newRarityDTO.RarityDTO, TypeParameter.AddAndNotification);
                Rarities.Add(newRarityDTO.RarityDTO);
            }
                
        }

        private async void UpdateRarity()
        {
            if (SelectedRarity == null)
                return;

            var (RarityDTO, Message) = await _rarityService.UpdateAsync(SelectedRarity.IdRarity, RarityName);

            if (Message == string.Empty)
            {
                NotificationTransmittingValue(WindowName.InteractionItem, (RarityDTO, SelectedRarity.RarityName), TypeParameter.UpdateAndNotification);
                NotificationTransmittingValue(WindowName.InteractionOffering, RarityDTO, TypeParameter.UpdateAndNotification);
                NotificationTransmittingValue(WindowName.InteractionKiller, RarityDTO, TypeParameter.UpdateAndNotification);
                Rarities.ReplaceItem(SelectedRarity, RarityDTO);
            }
            else
                MessageBox.Show(Message);
        }

        private async void DeleteRarity()
        {
            if (SelectedRarity == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _rarityService.DeleteAsync(SelectedRarity.IdRarity);

                if (IsDeleted == true)
                {
                    NotificationTransmittingValue(WindowName.InteractionItem, SelectedRarity.IdRarity, TypeParameter.DeleteAndNotification);
                    NotificationTransmittingValue(WindowName.InteractionOffering, SelectedRarity.IdRarity, TypeParameter.DeleteAndNotification);
                    NotificationTransmittingValue(WindowName.InteractionKiller, SelectedRarity.IdRarity, TypeParameter.DeleteAndNotification);
                    Rarities.Remove(SelectedRarity);
                }
                else
                    MessageBox.Show(Message);
            }
        }

        #endregion

    }
}