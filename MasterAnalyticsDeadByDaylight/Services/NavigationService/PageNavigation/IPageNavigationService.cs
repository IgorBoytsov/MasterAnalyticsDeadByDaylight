﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation
{
    public interface IPageNavigationService
    {
        void NavigateTo(string pageName, object parameter = null);
    }
}
