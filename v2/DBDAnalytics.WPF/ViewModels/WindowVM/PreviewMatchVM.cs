using DBDAnalytics.Application.DTOs.DetailsViewDTOs;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.ValueConverters;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class PreviewMatchVM : BaseVM, IUpdatable
    {
        private readonly IGetDetailsMatchViewUseCase _getDetailsMatchViewUseCase;

        public PreviewMatchVM(IWindowNavigationService windowNavigationService,
                              IGetDetailsMatchViewUseCase getDetailsMatchViewUseCase) : base(windowNavigationService)
        {
            _getDetailsMatchViewUseCase = getDetailsMatchViewUseCase;
        }

        public async void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            if (parameter is int idMatch)
            {
                Match = await _getDetailsMatchViewUseCase.GetDetailsViewMatch(idMatch);
 
                SetTitle();
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

        private DetailsMatchViewDTO _match;
        public DetailsMatchViewDTO Match
        {
            get => _match;
            set
            {
                _match = value;
                OnPropertyChanged();
            }
        }

        private void SetTitle()
        {
            var dateConverter = new CapitalizeDayAndDateConverter();
            Title = $"Дата - {dateConverter.Convert(Match.DateTimeMatch, null, null, new System.Globalization.CultureInfo("ru-RU"))} " +
                    $"Режим - {Match.GameMode} " +
                    $"Событие - {Match.GameEvent}";
        }
    }
}