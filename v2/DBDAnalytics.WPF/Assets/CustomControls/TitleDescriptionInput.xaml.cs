using DBDAnalytics.WPF.Command;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DBDAnalytics.WPF.Assets.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для TitleDescriptionInput.xaml
    /// </summary>
    public partial class TitleDescriptionInput : UserControl, INotifyPropertyChanged
    {
        public TitleDescriptionInput()
        {
            InitializeComponent();
        }

        public int MaxNameLength { get; set; } = 50;
        public int MaxDescriptionLength { get; set; } = 2000;

        /*--Свойства--------------------------------------------------------------------------------------*/

        #region NameRecord

        public static readonly DependencyProperty NameRecordProperty =
            DependencyProperty.Register(
                nameof(NameRecord),
                typeof(string),
                typeof(TitleDescriptionInput),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string NameRecord
        {
            get => (string)GetValue(NameRecordProperty);
            set => SetValue(NameRecordProperty, value);
        }

        #endregion

        #region DescriptionRecord

        public static readonly DependencyProperty DescriptionRecordProperty =
            DependencyProperty.Register(
                nameof(DescriptionRecord),
                typeof(string),
                typeof(TitleDescriptionInput),
                new FrameworkPropertyMetadata(string.Empty, OnDescriptionRecordChanged));

        public string DescriptionRecord
        {
            get => (string)GetValue(DescriptionRecordProperty);
            set => SetValue(DescriptionRecordProperty, value);
        }

        private static void OnDescriptionRecordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TitleDescriptionInput control)
            {
                control.DisplayErrorMessage();
            }
        }

        private void DisplayErrorMessage()
        {

        }

        #endregion

        #region ErrorMessage

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(
                nameof(ErrorMessage),
                typeof(string),
                typeof(TitleDescriptionInput),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление записи

        public static readonly DependencyProperty AddRecordCommandProperty =
            DependencyProperty.Register(
                nameof(AddRecordCommand), 
                typeof(ICommand), 
                typeof(TitleDescriptionInput),
                new PropertyMetadata(null));

        public ICommand AddRecordCommand
        {
            get { return (ICommand)GetValue(AddRecordCommandProperty); }
            set { SetValue(AddRecordCommandProperty, value); }
        }

        #endregion

        #region Обновление записи

        public static readonly DependencyProperty UpdateRecordCommandProperty =
           DependencyProperty.Register(
               nameof(UpdateRecordCommand),
               typeof(ICommand),
               typeof(TitleDescriptionInput),
               new PropertyMetadata(null));

        public ICommand UpdateRecordCommand
        {
            get { return (ICommand)GetValue(UpdateRecordCommandProperty); }
            set { SetValue(UpdateRecordCommandProperty, value); }
        }

        #endregion

        #region Удаление записи

        public static readonly DependencyProperty DeleteRecordCommandProperty =
           DependencyProperty.Register(
               nameof(DeleteRecordCommand),
               typeof(ICommand),
               typeof(TitleDescriptionInput),
               new PropertyMetadata(null));

        public ICommand DeleteRecordCommand
        {
            get { return (ICommand)GetValue(DeleteRecordCommandProperty); }
            set { SetValue(DeleteRecordCommandProperty, value); }
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}