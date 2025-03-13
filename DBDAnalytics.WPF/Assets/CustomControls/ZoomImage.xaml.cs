using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DBDAnalytics.WPF.Assets.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для ZoomImage.xaml
    /// </summary>
    public partial class ZoomImage : UserControl
    {
        public ZoomImage()
        {
            InitializeComponent();
        }

        private void ResetZoom()
        {
            _zoomFactor = 1.0;
            _scaleTransform.ScaleX = 1.0;
            _scaleTransform.ScaleY = 1.0;
            _translateTransform.X = 0;
            _translateTransform.Y = 0;
        }

        private Image _image;

        private ScaleTransform _scaleTransform = new ScaleTransform();

        private TranslateTransform _translateTransform = new TranslateTransform();

        private TransformGroup _transformGroup = new TransformGroup();

        private Point _lastMousePosition;

        private bool _isDragging = false;

        private double _zoomFactor = 1.0;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = GetTemplateChild("PART_Image") as Image;

            if (_image != null)
            {
                _transformGroup.Children.Add(_scaleTransform);
                _transformGroup.Children.Add(_translateTransform);

                _image.RenderTransform = _transformGroup;
                _image.RenderTransformOrigin = new Point(0.5, 0.5);

                _image.MouseWheel += Image_MouseWheel;
                _image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                _image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
                _image.MouseMove += Image_MouseMove;
                _image.MouseLeave += Image_MouseLeave;
            }
        }

        /*--События---------------------------------------------------------------------------------------*/

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scaleStep = 0.1;
            double prevZoom = _zoomFactor;

            if (e.Delta > 0)
                _zoomFactor += scaleStep;
            else if (_zoomFactor > 1.0 + scaleStep)
                _zoomFactor -= scaleStep;
            else
                _zoomFactor = 1.0;

            _scaleTransform.ScaleX = _zoomFactor;
            _scaleTransform.ScaleY = _zoomFactor;

            if (_zoomFactor == 1.0)
            {
                _translateTransform.X = 0;
                _translateTransform.Y = 0;
            }
            else
            {
                double scaleRatio = _zoomFactor / prevZoom;
                _translateTransform.X *= scaleRatio;
                _translateTransform.Y *= scaleRatio;
                ClampTranslation();
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_zoomFactor > 1.0)
            {
                _isDragging = true;
                _lastMousePosition = e.GetPosition(this);
                _image.CaptureMouse();
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point currentPosition = e.GetPosition(this);
                Vector delta = currentPosition - _lastMousePosition;

                _translateTransform.X += delta.X;
                _translateTransform.Y += delta.Y;

                ClampTranslation();

                _lastMousePosition = currentPosition;
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            _image.ReleaseMouseCapture();
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            _isDragging = false;
            _image.ReleaseMouseCapture();
        }

        private void ClampTranslation()
        {
            if (_image == null || _image.ActualWidth == 0 || _image.ActualHeight == 0)
                return;

            double maxOffsetX = (_image.ActualWidth * (_zoomFactor - 1)) / 2;
            double maxOffsetY = (_image.ActualHeight * (_zoomFactor - 1)) / 2;

            _translateTransform.X = Math.Max(-maxOffsetX, Math.Min(maxOffsetX, _translateTransform.X));
            _translateTransform.Y = Math.Max(-maxOffsetY, Math.Min(maxOffsetY, _translateTransform.Y));
        }

        /*--Свойства--------------------------------------------------------------------------------------*/

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                nameof(Source), 
                typeof(ImageSource), 
                typeof(ZoomImage), 
                new FrameworkPropertyMetadata(null, OnDescriptionRecordChanged));

        private static void OnDescriptionRecordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZoomImage control)
            {
                control.ResetZoom();
            }
        }
    }
}