using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Base;
using System.Windows.Controls;

namespace Shared.WPF.Navigations.Pages
{
    public sealed class PageNavigation : IPageNavigation
    {
        private Dictionary<FramesName, Frame> _frames = [];
        private Dictionary<PagesName, Page> _pages = [];
        private Dictionary<FramesName, PagesName> _currenDisplayedPage = [];
        private readonly Dictionary<string, IPageFactory> _pagesFactories = [];

        public PageNavigation(IEnumerable<IPageFactory> pageFactories)
        {
            _pagesFactories = pageFactories.ToDictionary(f => f.GetType().Name.Replace("PageFactory", ""), f => f);
        }

        public void RegisterFrame(FramesName frameName, Frame frame)
        {
            if (!_frames.TryAdd(frameName, frame))
                throw new Exception($"Frame с именем '{frameName}' уже зарегистрирована.");
        }

        public void Navigate(PagesName pageName, FramesName frameName)
        {
            if (_pages.TryGetValue(pageName, out var pageExist))
            {
                if (_frames.TryGetValue(frameName, out var frame))
                {
                    frame.Navigate(pageExist);
                    SetCurrentDisplayedPage(frameName, pageName);
                }
                else throw new Exception($"Frame с именем '{frameName}' не зарегистрировано.");
            }
            else
            {
                if (_pagesFactories.TryGetValue(pageName.ToString(), out var factory))
                {
                    if (_frames.TryGetValue(frameName, out var frame))
                    {
                        var page = factory.CreatePage();
                        _pages.TryAdd(pageName, page);
                        frame.Navigate(page);
                        SetCurrentDisplayedPage(frameName, pageName);
                    }
                    else throw new Exception($"Frame с именем '{frameName}' не зарегистрирован.");
                }
                else throw new Exception($"Factory у страницы с именем '{pageName}' не существует.");
            }
        }

        public void TransmittingValue<TData>(PagesName pageName, FramesName frameName, TData value, TransmittingParameter typeParameter = TransmittingParameter.None, bool isNavigateAfterTransmitting = true, bool forceOpenPage = true)
        {
            if (_pages.TryGetValue(pageName, out var pageExist))
            {
                if (pageExist.DataContext is IUpdatable viewModel)
                {
                    if (_frames.TryGetValue(frameName, out var frame))
                    {
                        viewModel.Update(value, typeParameter);

                        if (isNavigateAfterTransmitting)
                            frame.Navigate(pageExist);
                    }
                    else throw new Exception($"Frame с именем '{frameName}' не зарегистрировано.");
                }
                else throw new Exception($"У ViewModel страницы '{pageName}' не реализован интерфейс IUpdatable.");
            }
            else
            {
                if (forceOpenPage)
                    Navigate(pageName, frameName);
            }
        }

        public void Close(PagesName pageName, FramesName frameName)
        {
            if (_pages.ContainsKey(pageName) && _frames.TryGetValue(frameName, out var frameExist))
            {
                frameExist.Navigate(null);
                _pages.Remove(pageName);
                SetDefaultCurrentDisplayedPage(frameName);
            }
            else throw new Exception("Такой страницы либо фрейма не существует.");
        }

        public PagesName GetCurrentDisplayedPage(FramesName frameName) => _currenDisplayedPage.GetValueOrDefault(frameName);

        #region Вспомогательные методы

        private void SetCurrentDisplayedPage(FramesName frame, PagesName page) => _currenDisplayedPage[frame] = page;

        private void SetDefaultCurrentDisplayedPage(FramesName frame) => _currenDisplayedPage[frame] = PagesName.None;

        #endregion
    }
}