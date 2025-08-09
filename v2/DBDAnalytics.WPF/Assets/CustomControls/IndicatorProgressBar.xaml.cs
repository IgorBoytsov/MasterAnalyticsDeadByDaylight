using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DBDAnalytics.WPF.Assets.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для IndicatorProgressBar.xaml
    /// </summary>
    public partial class IndicatorProgressBar : UserControl
    {
        public IndicatorProgressBar()
        {
            InitializeComponent();
        }

        #region CurrentValue

        [Category("Common Properties")] // Группировка свойства в дизайнере
        [Description("Hook Rate Value")] // Описание свойства в дизайнере
        [Bindable(true)] // Указывает, что свойство может быть связано данными
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)] // Сериализация для дизайнера
        [EditorBrowsable(EditorBrowsableState.Always)] // Отображать в Intellisense
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
                nameof(CurrentValue),
                typeof(double),
                typeof(IndicatorProgressBar),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        // Дополнительное свойство для дизайнерского значения.  Важно, чтобы имя отличалось от CurrentValue
        [Category("Common Properties")] // Группировка свойства в дизайнере
        [Description("Hook Rate Design Time Value")] // Описание свойства в дизайнере
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // Не сериализовать это значение в XAML
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double CurrentValueDesignTime
        {
            get
            {
#if DEBUG
                if (DesignerProperties.GetIsInDesignMode(this))
                    return 42.0;
#endif
                return CurrentValue;
            }
            set
            {
                CurrentValue = value; 
            }
        }


        #endregion

        #region MinimumValue

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(
                nameof(MinimumValue),
                typeof(double),
                typeof(IndicatorProgressBar),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MinimumValue
        {
            get => (double)GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }

        #endregion

        #region MaximumValue

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(
                nameof(MaximumValue),
                typeof(double),
                typeof(IndicatorProgressBar),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MaximumValue
        {
            get => (double)GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }

        #endregion

        #region DisplayedValueType

        public static readonly DependencyProperty DisplayedValueTypeProperty =
            DependencyProperty.Register(
                nameof(DisplayedValueType),
                typeof(string),
                typeof(IndicatorProgressBar),
                new FrameworkPropertyMetadata("%", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string DisplayedValueType
        {
            get => (string)GetValue(DisplayedValueTypeProperty);
            set => SetValue(DisplayedValueTypeProperty, value);
        }

        #endregion

        #region TextValue

        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register(
                nameof(TextValue),
                typeof(string),
                typeof(IndicatorProgressBar),
                new FrameworkPropertyMetadata("Пример текста: ", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string TextValue
        {
            get => (string)GetValue(TextValueProperty);
            set => SetValue(TextValueProperty, value);
        }

        #endregion

        #region ForegroundText

        public static readonly DependencyProperty ForegroundProgressProperty =
            DependencyProperty.Register(
                nameof(ForegroundProgress),
                typeof(Brush),
                typeof(IndicatorProgressBar),
                new FrameworkPropertyMetadata(Brushes.Green, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush ForegroundProgress
        {
            get => (Brush)GetValue(ForegroundProgressProperty);
            set => SetValue(ForegroundProgressProperty, value);
        }

        #endregion

        #region BackgroundProgressBar

        [Category("Common Properties")]
        [Description("Hook Rate Value")]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public static readonly DependencyProperty BackgroundProgressBarProperty =
            DependencyProperty.Register(
                nameof(BackgroundProgressBar),
                typeof(Brush),
                typeof(IndicatorProgressBar),
                new FrameworkPropertyMetadata(Brushes.Green, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush BackgroundProgressBar
        {
            get => (Brush)GetValue(BackgroundProgressBarProperty);
            set => SetValue(BackgroundProgressBarProperty, value);
        }

        [Category("Common Properties")] 
        [Description("Hook Rate Design Time Value")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Brush BackgroundProgressBarDesignTime
        {
            get
            {
#if DEBUG
                if (DesignerProperties.GetIsInDesignMode(this))
                    return (Brush)Brushes.Transparent;
#endif
                return BackgroundProgressBar;
            }
            set
            {
                BackgroundProgressBar = value;
            }
        }

        #endregion
    }
}