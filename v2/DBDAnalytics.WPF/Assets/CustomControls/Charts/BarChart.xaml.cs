using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Assets.CustomControls.Charts
{
    /// <summary>
    /// Логика взаимодействия для BarChart.xaml
    /// </summary>
    public partial class BarChart : UserControl
    {
        public ObservableCollection<BarChartData> Data
        {
            get => (ObservableCollection<BarChartData>)GetValue(DataProperty);
            set
            {
                SetValue(DataProperty, value);
                UpdateNormalizedData();
            }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(ObservableCollection<BarChartData>),
                typeof(BarChart), new PropertyMetadata(new ObservableCollection<BarChartData>(), OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BarChart chart)
            {
                chart.UpdateNormalizedData();
            }
        }

        private ObservableCollection<BarChartData> _normalizedData = new();
        public ObservableCollection<BarChartData> NormalizedData
        {
            get => _normalizedData;
            private set
            {
                _normalizedData = value;
                OnPropertyChanged(nameof(NormalizedData));
            }
        }

        public BarChart()
        {
            InitializeComponent();
            //DataContext = this;
            SizeChanged += (s, e) => UpdateNormalizedData();
        }

        private void UpdateNormalizedData()
        {
            if (Data == null || Data.Count == 0)
            {
                NormalizedData.Clear();
                return;
            }

            double maxValue = Data.Max(d => d.Value);
            if (maxValue == 0) maxValue = 1;

            double chartHeight = ActualHeight > 0 ? ActualHeight : 200;

            NormalizedData = new ObservableCollection<BarChartData>(
                Data.Select(item => new BarChartData
                {
                    Label = item.Label,
                    Value = item.Value,
                    ScaledValue = (item.Value / maxValue) * (chartHeight * 0.8)
                })
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class BarChartData
    {
        public string Label { get; set; } = string.Empty;
        public double Value { get; set; }
        public double ScaledValue { get; set; }
    }
}