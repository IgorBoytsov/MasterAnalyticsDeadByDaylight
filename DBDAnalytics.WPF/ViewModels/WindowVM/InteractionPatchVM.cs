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
    internal class InteractionPatchVM : BaseVM, IUpdatable
    {
        private readonly IPatchService _patchService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionPatchVM(IWindowNavigationService windowNavigationService, IPatchService patchService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _patchService = patchService;

            GetPatches();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<PatchDTO> Patches { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Список патчей";

        #endregion

        #region Свойства : Selected | PatchNumber | PatchDateRelease| PatchDateDescription

        private PatchDTO _selectedPatch;
        public PatchDTO SelectedPatch
        {
            get => _selectedPatch;
            set
            {
                if (_selectedPatch != value)
                {
                    _selectedPatch = value;

                    PatchNumber = value.PatchNumber;
                    PatchDateRelease =  value.PatchDateRelease.ToDateTime(TimeOnly.MinValue);

                    OnPropertyChanged();
                }
            }
        }

        private string _patchNumber;
        public string PatchNumber
        {
            get => _patchNumber;
            set
            {
                _patchNumber = value;
                OnPropertyChanged();
            }
        }        
        
        private string _patchDateDescription;
        public string PatchDateDescription
        {
            get => _patchDateDescription;
            set
            {
                _patchDateDescription = value;
                OnPropertyChanged();
            }
        }

        private DateTime _patchDateRelease;
        public DateTime PatchDateRelease
        {
            get => _patchDateRelease;
            set
            {
                _patchDateRelease = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addPatchCommand;
        public RelayCommand AddPatchCommand { get => _addPatchCommand ??= new(obj => { AddPatch(); }); }

        private RelayCommand _deletePatchCommand;
        public RelayCommand DeletePatchCommand { get => _deletePatchCommand ??= new(obj => { DeletePatch(); }); }

        private RelayCommand _updatePatchCommand;
        public RelayCommand UpdatePatchCommand { get => _updatePatchCommand ??= new(obj => { UpdatePatch(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private async void GetPatches()
        {
            var patches = await _patchService.GetAllAsync();

            foreach (var patch in patches)
                Patches.Add(patch);
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddPatch()
        {
            var newPatchDTO = await _patchService.CreateAsync(PatchNumber, DateOnly.FromDateTime(PatchDateRelease), PatchDateDescription);

            if (newPatchDTO.Message != string.Empty)
            {
                MessageBox.Show(newPatchDTO.Message);
                return;
            }
            else
                Patches.Add(newPatchDTO.PatchDTO);
        }

        private async void DeletePatch()
        {
            if (SelectedPatch == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _patchService.DeleteAsync(SelectedPatch.IdPatch);

                if (IsDeleted == true)
                    Patches.Remove(SelectedPatch);
                else
                    MessageBox.Show(Message);
            }
        }

        private async void UpdatePatch()
        {
            if (SelectedPatch == null)
                return;

            var (PatchDTO, Message) = await _patchService.UpdateAsync(SelectedPatch.IdPatch, PatchNumber, DateOnly.FromDateTime(PatchDateRelease), PatchDateDescription);

            if (Message == string.Empty)
                Patches.ReplaceItem(SelectedPatch, PatchDTO);
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите произвести обновление?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedPatchDTO = await _patchService.ForcedUpdateAsync(SelectedPatch.IdPatch, PatchNumber, DateOnly.FromDateTime(PatchDateRelease), PatchDateDescription);
                    Patches.ReplaceItem(SelectedPatch, forcedPatchDTO);
                }
            }
        }

        #endregion
    }
}