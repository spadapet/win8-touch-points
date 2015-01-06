using System.ComponentModel;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml.Media;

namespace TouchPoints
{
    public class TouchPoint : INotifyPropertyChanged
    {
        private PointerPoint _point;
        private bool _dead;
        private int _countdown;

        public event PropertyChangedEventHandler PropertyChanged;

        public TouchPoint(PointerPoint point)
        {
            _point = point;
        }

        public PointerPoint Point
        {
            get { return _point; }

            set
            {
                if (_point != value && value != null)
                {
                    _point = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public bool Dead
        {
            get { return _dead; }

            set
            {
                if (_dead != value)
                {
                    _dead = value;
                    OnPropertyChanged("Dead");
                    OnPropertyChanged("ColorBrush");

                    if (_dead)
                    {
                        _countdown = 30;
                    }
                }
            }
        }

        public bool Decomposed
        {
            get { return _dead && _countdown == 0; }
        }

        public string Description
        {
            get
            {
                return string.Format("ID:{0}, X:{1}, Y:{2}",
                    _point.PointerId,
                    _point.Position.X.ToString("F2"),
                    _point.Position.Y.ToString("F2"));
            }
        }

        public Brush ColorBrush
        {
            get
            {
                return _dead
                    ? new SolidColorBrush(Colors.Red)
                    : new SolidColorBrush(Colors.White);
            }
        }

        public void Update()
        {
            if (_countdown > 0)
            {
                _countdown--;
            }
        }

        public override string ToString()
        {
            return Description;
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
