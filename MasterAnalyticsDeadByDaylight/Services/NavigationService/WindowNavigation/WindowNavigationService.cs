using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation
{
    public class WindowNavigationService
    {
        private readonly Dictionary<string, Type> _registeredViews = new Dictionary<string, Type>();
        private readonly Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        private Window _mainWindow;

        public WindowNavigationService(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        // Регистрация окна для навигации
        public void RegisterView(string viewName, Type viewType)
        {
            _registeredViews.Add(viewName, viewType);
        }

        // Переход к окну
        public void NavigateTo(string viewName, object parameters = null)
        {
            if (_registeredViews.ContainsKey(viewName))
            {
                Type viewType = _registeredViews[viewName];
                Window window = (Window)Activator.CreateInstance(viewType);

                if (parameters != null)
                {
                    _viewParameters[viewName] = parameters;
                    window.DataContext = parameters;
                }

                // Закрываем текущее окно (если оно есть)
                //if (_mainWindow.Content is Window currentWindow)
                //{
                //    currentWindow.Close();
                //}

                // Отображаем новое окно
                _mainWindow.Content = window;
                window.Show();
            }
            else
            {
                // Обработка ошибки: окно не найдено
                MessageBox.Show($"View '{viewName}' not registered.");
            }
        }

        // Получение параметров для окна
        public object GetParameters(string viewName)
        {
            if (_viewParameters.ContainsKey(viewName))
            {
                return _viewParameters[viewName];
            }
            return null;
        }

    }
}
