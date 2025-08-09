using System.Windows;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Assets.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl
    {
        public CustomTextBox()
        {
            InitializeComponent();
        }

        //public int MaxNameLength { get; set; } = 50;
        //public int MaxDescriptionLength { get; set; } = 2000;

        /*--Свойства--------------------------------------------------------------------------------------*/

        #region MaxLength

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(
                nameof(MaxLength),
                typeof(int), // Тип должен быть int
                typeof(CustomTextBox),
                new FrameworkPropertyMetadata(50)); // Убрал BindsTwoWayByDefault, здесь оно не нужно

        // Тип свойства должен быть int, как и у DependencyProperty
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        #endregion

        #region NameRecord (Текст)

        public static readonly DependencyProperty NameRecordProperty =
            DependencyProperty.Register(
                nameof(NameRecord),
                typeof(string),
                typeof(CustomTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string NameRecord
        {
            get => (string)GetValue(NameRecordProperty);
            set => SetValue(NameRecordProperty, value);
        }

        #endregion

        #region DescriptionRecord

        //public static readonly DependencyProperty DescriptionRecordProperty =
        //    DependencyProperty.Register(
        //        nameof(DescriptionRecord),
        //        typeof(string),
        //        typeof(CustomTextBox),
        //        new FrameworkPropertyMetadata(string.Empty, OnDescriptionRecordChanged));

        //public string DescriptionRecord
        //{
        //    get => (string)GetValue(DescriptionRecordProperty);
        //    set => SetValue(DescriptionRecordProperty, value);
        //}

        //private static void OnDescriptionRecordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is CustomTextBox control)
        //    {
        //        control.DisplayErrorMessage();
        //    }
        //}

        //private void DisplayErrorMessage()
        //{

        //}

        #endregion

    }
}
