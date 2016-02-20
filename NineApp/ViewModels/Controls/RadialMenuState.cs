using System;
using System.Windows;
using Nine.MVVM;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    ///     The ViewModel associated to RadialMenu control.
    /// </summary>
    public class RadialMenuState : BaseViewModel
    {
        public enum Levels
        {
            None,
            Main,
            DrawingModes,
            Tags,
            Thicknesses,
            Colors
        };

        private readonly double radialMenuRadius = 150;
        private Point _centerPosition = new Point(0, 0);
        private Levels _level = Levels.None;

        public RadialMenuState(Levels level, Point center)
        {
            Open(level);
            SetPosition(center);
        }

        public Levels Level
        {
            get { return _level; }
            set
            {
                _level = value;
                RaisePropertyChanged();
                RaisePropertyChanged("LevelAsString");
                RaisePropertyChanged("IsOpen");
            }
        }

        public string LevelAsString
        {
            get { return Enum.GetName(typeof (Levels), _level); }
        }

        public bool IsOpen
        {
            get { return Level != Levels.None; }
        }

        public Point CenterPosition
        {
            get { return _centerPosition; }
            set
            {
                _centerPosition = value;
                RaisePropertyChanged();
                RaisePropertyChanged("LeftPosition");
                RaisePropertyChanged("TopPosition");
            }
        }

        public double LeftPosition
        {
            get { return CenterPosition.X - radialMenuRadius; }
        }

        public double TopPosition
        {
            get { return CenterPosition.Y - radialMenuRadius; }
        }

        public void Open(Levels level)
        {
            Level = level;
        }

        public void Open(Levels level, Point center)
        {
            Open(level);
            SetPosition(center);
        }

        public void Close()
        {
            Level = Levels.None;
        }

        public void SetPosition(Point center)
        {
            CenterPosition = center;
        }
    }
}