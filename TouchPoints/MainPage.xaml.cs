using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace TouchPoints
{
    public sealed partial class MainPage : Page
    {
        private CoreWindow _window;
        private ObservableCollection<TouchPoint> _points;
        private Storyboard _storyboard;

        public MainPage()
        {
            _points = new ObservableCollection<TouchPoint>();

            _storyboard = new Storyboard();
            _storyboard.Completed += OnStoryboardCompleted;

            _window = Window.Current.CoreWindow;
            _window.PointerPressed += OnPointerPressed;
            _window.PointerMoved += OnPointerMoved;
            _window.PointerReleased += OnPointerReleased;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            this.InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            _storyboard.Begin();
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            _storyboard.Stop();
        }

        private void OnStoryboardCompleted(object sender, object args)
        {
            for (int i = _points.Count - 1; i >= 0; i--)
            {
                _points[i].Update();

                if (_points[i].Decomposed)
                {
                    _points.RemoveAt(i);
                }
            }

            _storyboard.Begin();
        }

        public IList<TouchPoint> Points
        {
            get { return _points; }
        }

        private void OnPointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            foreach (TouchPoint point in _points)
            {
                if (point.Point.PointerId == args.CurrentPoint.PointerId)
                {
                    point.Dead = true;
                    break;
                }
            }
        }

        private void OnPointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            foreach (TouchPoint point in _points)
            {
                if (point.Point.PointerId == args.CurrentPoint.PointerId)
                {
                    point.Point = args.CurrentPoint;
                }
            }
        }

        private void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.PointerDevice.PointerDeviceType == PointerDeviceType.Touch)
            {
                _points.Add(new TouchPoint(args.CurrentPoint));
            }
        }
    }
}
