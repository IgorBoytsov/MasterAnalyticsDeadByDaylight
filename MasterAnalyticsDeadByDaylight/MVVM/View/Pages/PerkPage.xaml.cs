﻿using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.PerkService;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для PerkPage.xaml
    /// </summary>
    public partial class PerkPage : Page
    {
        public PerkPage()
        {
            InitializeComponent();

            Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();

            IDataService dataService = new DataService(_contextFactory);
            IPerkCalculationService perkCalculationService = new PerkCalculationService(_contextFactory);

            DataContext = new PerkPageViewModel(dataService, perkCalculationService);
        }
    }
}