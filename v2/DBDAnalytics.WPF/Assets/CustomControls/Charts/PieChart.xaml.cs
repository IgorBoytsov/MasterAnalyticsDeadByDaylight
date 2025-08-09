using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DBDAnalytics.WPF.Assets.CustomControls.Charts
{
    /// <summary>
    /// Логика взаимодействия для PieChart.xaml
    /// </summary>
    public partial class PieChart : UserControl
    {

        public PieChart()
        {
            InitializeComponent();
            this.Loaded += PieChart_Loaded;
        }

        #region Свойства 

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", 
            typeof(ObservableCollection<PieChartItem>), 
            typeof(PieChart), 
            new PropertyMetadata(new ObservableCollection<PieChartItem>(), OnItemsChanged));

        public ObservableCollection<PieChartItem> Items
        {
            get { return (ObservableCollection<PieChartItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pieChart = (PieChart)d;
            pieChart.UpdatePie();
        }

        #endregion

        #region События

        private void PieChart_Loaded(object sender, RoutedEventArgs e)
        {
            if (Items != null)
            {
                Items.CollectionChanged += Items_CollectionChanged;
            }
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdatePie();
        }

        private void PieChart_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Items != null)
            {
                Items.CollectionChanged -= Items_CollectionChanged;
            }
        }

        private void PieCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePie();
        }

        #endregion

        #region Отрисовка диограмы

        private void UpdatePie()
        {
            PieCanvas.Children.Clear();

            if (Items == null || Items.Count == 0)
                return;

            double total = 0;
            foreach (var item in Items)
            {
                total += item.Value;
            }

            double canvasWidth = PieCanvas.ActualWidth;
            double canvasHeight = PieCanvas.ActualHeight;
            double radius = Math.Min(canvasWidth, canvasHeight) / 2;
            Point center = new Point(canvasWidth / 2, canvasHeight / 2);

            double startAngle = 0;

            foreach (var item in Items)
            {
                if (item.Value <= 0)
                    continue;

                double percentage = item.Value / total;
                double sweepAngle = 360 * percentage;

                // Создание сектора геометрии 
                PathGeometry sectorGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = center;

                // Вычислите конечную точку дуги
                Point arcEndPoint = new Point(
                    center.X + radius * Math.Cos(startAngle * Math.PI / 180),
                    center.Y + radius * Math.Sin(startAngle * Math.PI / 180));

                pathFigure.Segments.Add(new LineSegment(arcEndPoint, false));

                Point arcEndPoint2 = new Point(
                    center.X + radius * Math.Cos((startAngle + sweepAngle) * Math.PI / 180),
                    center.Y + radius * Math.Sin((startAngle + sweepAngle) * Math.PI / 180));

                ArcSegment arcSegment = new ArcSegment
                {
                    Point = arcEndPoint2,
                    Size = new Size(radius, radius),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = sweepAngle > 180,
                    RotationAngle = 0
                };

                pathFigure.Segments.Add(arcSegment);
                pathFigure.Segments.Add(new LineSegment(center, false));

                sectorGeometry.Figures.Add(pathFigure);

                // Создаём путь
                Path sectorPath = new Path
                {
                    Data = sectorGeometry,
                    Fill = item.Color,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    ToolTip = $"{item.Name}: {item.Value}",
                };

                PieCanvas.Children.Add(sectorPath);

                // Вычисляем позицию для текста сбоку от сектора
                double middleAngle = startAngle + (sweepAngle / 2);
                Point labelPosition = CalculateOuterLabelPosition(center, radius, middleAngle);

                TextBlock label = new TextBlock
                {
                    Text = $"{item.Value}%",
                    Foreground = Brushes.White,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold
                };

                // Выравнивание текста в зависимости от квадранта
                if (middleAngle > 90 && middleAngle <= 270)
                {
                    // Левая половина круга - выравнивание по правому краю
                    label.TextAlignment = TextAlignment.Right;
                    labelPosition.X -= 5;
                }
                else
                {
                    // Правая половина круга - выравнивание по левому краю
                    label.TextAlignment = TextAlignment.Left;
                    labelPosition.X += 5;
                }

                Canvas.SetLeft(label, labelPosition.X);
                Canvas.SetTop(label, labelPosition.Y - (label.FontSize / 2));
                PieCanvas.Children.Add(label);

                startAngle += sweepAngle;
            }
        }

        private void UpdatePieGoogleAI()
        {
            PieCanvas.Children.Clear();

            if (Items == null || Items.Count == 0)
                return;

            double total = 0;
            foreach (var item in Items)
            {
                total += item.Value;
            }

            double canvasWidth = PieCanvas.ActualWidth;
            double canvasHeight = PieCanvas.ActualHeight;
            double radius = Math.Min(canvasWidth, canvasHeight) / 2;
            Point center = new Point(canvasWidth / 2, canvasHeight / 2);

            double startAngle = 0;

            foreach (var item in Items)
            {
                if (item.Value <= 0)
                    continue;

                double percentage = item.Value / total;
                double sweepAngle = 360 * percentage;

                // Создание сектора геометрии 
                PathGeometry sectorGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = center;

                // Вычислите конечную точку дуги
                Point arcEndPoint = new Point(
                    center.X + radius * Math.Cos(startAngle * Math.PI / 180),
                    center.Y + radius * Math.Sin(startAngle * Math.PI / 180));

                pathFigure.Segments.Add(new LineSegment(arcEndPoint, false));

                Point arcEndPoint2 = new Point(
                    center.X + radius * Math.Cos((startAngle + sweepAngle) * Math.PI / 180),
                    center.Y + radius * Math.Sin((startAngle + sweepAngle) * Math.PI / 180));

                ArcSegment arcSegment = new ArcSegment
                {
                    Point = arcEndPoint2,
                    Size = new Size(radius, radius),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = sweepAngle > 180,
                    RotationAngle = 0
                };

                pathFigure.Segments.Add(arcSegment);
                pathFigure.Segments.Add(new LineSegment(center, false));

                sectorGeometry.Figures.Add(pathFigure);

                // Создаём путь
                Path sectorPath = new Path
                {
                    Data = sectorGeometry,
                    Fill = item.Color,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    ToolTip = $"{item.Name}: {item.Value}",
                };

                PieCanvas.Children.Add(sectorPath);

                // Добавляем текст для легенды
                Point labelPosition = CalculateLabelPosition(center, radius, startAngle + (sweepAngle / 2));
                TextBlock label = new TextBlock
                {
                    Text = item.Value.ToString(),
                    Foreground = Brushes.White,
                    FontSize = 16
                };

                Canvas.SetLeft(label, labelPosition.X - (label.DesiredSize.Width / 2));
                Canvas.SetTop(label, labelPosition.Y - (label.DesiredSize.Height / 2));
                PieCanvas.Children.Add(label);

                startAngle += sweepAngle;
            }
        }

        #endregion

        private Point CalculateOuterLabelPosition(Point center, double radius, double angle)
        {
            double labelRadius = radius * 1.1; // Увеличиваем радиус, чтобы текст был снаружи
            double x = center.X + labelRadius * Math.Cos(Math.PI * angle / 180);
            double y = center.Y + labelRadius * Math.Sin(Math.PI * angle / 180);
            return new Point(x, y);
        }

        private Point CalculateLabelPosition(Point center, double radius, double angle)
        {
            double labelRadius = radius * 0.7;
            double x = center.X + labelRadius * Math.Cos(Math.PI * angle / 180);
            double y = center.Y + labelRadius * Math.Sin(Math.PI * angle / 180);
            return new Point(x, y);
        }

    }

    public class PieChartItem : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }

            }
        }

        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                }

            }
        }

        private string _displayValue;
        public string DisplayValue
        {
            get => _displayValue;
            set
            {
                if (_displayValue != value)
                {
                    _displayValue = value;
                    OnPropertyChanged();
                }

            }
        }

        private Brush _color;
        public Brush Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}