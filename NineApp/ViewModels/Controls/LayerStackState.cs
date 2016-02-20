using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Nine.MVVM;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Keep the state (mode and "tool" used) inside the LayerStack
    /// </summary>
    public class LayerStackState : BaseViewModel
    {
        public enum InkingModes
        {
            None,
            Pencil,
            Highlighter,
            Pointer,
            Selection,
            StrokeEraser,
            PointEraser
        };

        public enum States
        {
            None,
            DrawingAndManipulating,
            OnlyDrawing,
            OnlyManipulating
        };

        public readonly Dictionary<States, List<States>> StateSequencing = new Dictionary<States, List<States>>
        {
            {States.None, new List<States> {States.DrawingAndManipulating, States.OnlyDrawing, States.OnlyManipulating}},
            {States.OnlyDrawing, new List<States> {States.DrawingAndManipulating, States.OnlyManipulating}},
            {States.OnlyManipulating, new List<States> {States.DrawingAndManipulating, States.OnlyDrawing}},
            {States.DrawingAndManipulating, new List<States> {States.OnlyDrawing, States.OnlyManipulating}}
        };

        private InkCanvasEditingMode _editingMode = InkCanvasEditingMode.None;
        private bool _fingerInkingEnabled;
        private bool _fingerManipulationEnabled;
        private bool _inkingEnabled;
        private InkingModes _inkingMode = InkingModes.None;
        private bool _manipulationEnabled;
        private bool _mouseInkingEnabled;
        private InkCanvasEditingMode _previousEditingMode = InkCanvasEditingMode.None;
        private InkingModes _previousInkingMode = InkingModes.None;
        private States _state = States.None;
        private bool _stylusInkingEnabled;

        public LayerStackState(States state, InkingModes mode)
        {
            SetState(state);
            SetInkingMode(mode);
        }

        public States State
        {
            get { return _state; }
            private set
            {
                _state = value;
                RaisePropertyChanged();
                RaisePropertyChanged("StateAsString");
            }
        }

        public InkCanvasEditingMode EditingMode
        {
            get { return _editingMode; }
            private set
            {
                _editingMode = value;
                RaisePropertyChanged("IsThicknessable");
                RaisePropertyChanged("IsColorable");
                RaisePropertyChanged("IsDrawing");
                RaisePropertyChanged("IsErasing");
                RaisePropertyChanged("IsSelect");
                RaisePropertyChanged("IsGesture");
            }
        }

        public InkCanvasEditingMode PreviousEditingMode
        {
            get { return _previousEditingMode; }
            private set
            {
                _previousEditingMode = value;
                RaisePropertyChanged();
            }
        }

        public InkingModes InkingMode
        {
            get { return _inkingMode; }
            private set
            {
                _inkingMode = value;
                RaisePropertyChanged();
                RaisePropertyChanged("InkingModeAsString");
            }
        }

        public InkingModes PreviousInkingMode
        {
            get { return _previousInkingMode; }
            private set
            {
                _previousInkingMode = value;
                RaisePropertyChanged();
            }
        }

        public string StateAsString
        {
            get { return Enum.GetName(typeof (States), _state); }
        }

        public string InkingModeAsString
        {
            get { return Enum.GetName(typeof (InkingModes), UserInkingMode); }
        }

        public InkingModes UserInkingMode
        {
            get { return _inkingMode == InkingModes.None ? _previousInkingMode : _inkingMode; }
        }

        public InkCanvasEditingMode UserEditingMode
        {
            get { return _editingMode == InkCanvasEditingMode.None ? _previousEditingMode : _editingMode; }
        }

        public bool IsThicknessable
        {
            get { return IsDrawing || UserEditingMode == InkCanvasEditingMode.EraseByPoint; }
        }

        public bool IsColorable
        {
            get { return IsDrawing; }
        }

        public bool IsDrawing
        {
            get
            {
                return UserEditingMode == InkCanvasEditingMode.Ink ||
                       UserEditingMode == InkCanvasEditingMode.InkAndGesture;
            }
        }

        public bool IsErasing
        {
            get
            {
                return UserEditingMode == InkCanvasEditingMode.EraseByStroke ||
                       UserEditingMode == InkCanvasEditingMode.EraseByPoint;
            }
        }

        public bool IsSelect
        {
            get { return UserEditingMode == InkCanvasEditingMode.Select; }
        }

        public bool IsGesture
        {
            get
            {
                return UserEditingMode == InkCanvasEditingMode.GestureOnly ||
                       UserEditingMode == InkCanvasEditingMode.InkAndGesture;
            }
        }

        public bool InkingEnabled
        {
            get { return _inkingEnabled; }
            private set
            {
                _inkingEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool FingerInkingEnabled
        {
            get { return _fingerInkingEnabled; }
            private set
            {
                _fingerInkingEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool MouseInkingEnabled
        {
            get { return _mouseInkingEnabled; }
            private set
            {
                _mouseInkingEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool StylusInkingEnabled
        {
            get { return _stylusInkingEnabled; }
            private set
            {
                _stylusInkingEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool ManipulationEnabled
        {
            get { return _manipulationEnabled; }
            private set
            {
                _manipulationEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool FingerManipulationEnabled
        {
            get { return _fingerManipulationEnabled; }
            private set
            {
                _fingerManipulationEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void SetState(States newState)
        {
            if (State == newState)
            {
                return;
            }

            if (!StateSequencing[State].Contains(newState))
            {
                throw new InvalidOperationException("You cannot set CurrentState from \"" + State + "\" to \"" +
                                                    newState + "\" !");
            }

            // When passing from a "Drawing" to a "Non-Drawing" state, reverse Current and Previous state
            if (
                (State != States.OnlyManipulating && newState == States.OnlyManipulating)
                ||
                (State == States.OnlyManipulating && newState != States.OnlyManipulating)
                )
            {
                var tmpCanvasMode = EditingMode;
                EditingMode = PreviousEditingMode;
                PreviousEditingMode = tmpCanvasMode;

                var tmpInkingMode = InkingMode;
                InkingMode = PreviousInkingMode;
                PreviousInkingMode = tmpInkingMode;
            }

            switch (newState)
            {
                case States.None:
                    EditingMode = InkCanvasEditingMode.None;
                    PreviousEditingMode = InkCanvasEditingMode.None;
                    InkingEnabled = false;
                    FingerInkingEnabled = false;
                    MouseInkingEnabled = false;
                    StylusInkingEnabled = false;
                    ManipulationEnabled = false;
                    FingerManipulationEnabled = false;
                    break;
                case States.DrawingAndManipulating:
                    InkingEnabled = true;
                    FingerInkingEnabled = false;
                    MouseInkingEnabled = true;
                    StylusInkingEnabled = true;
                    ManipulationEnabled = true;
                    FingerManipulationEnabled = true;
                    break;
                case States.OnlyDrawing:
                    InkingEnabled = true;
                    FingerInkingEnabled = true;
                    MouseInkingEnabled = true;
                    StylusInkingEnabled = true;
                    ManipulationEnabled = false;
                    FingerManipulationEnabled = false;
                    break;
                case States.OnlyManipulating:
                    InkingEnabled = false;
                    FingerInkingEnabled = false;
                    MouseInkingEnabled = false;
                    StylusInkingEnabled = false;
                    ManipulationEnabled = true;
                    FingerManipulationEnabled = true;
                    break;
            }

            State = newState;
        }

        public void SetInkingMode(InkingModes newInkingMode)
        {
            if (newInkingMode == InkingModes.None)
            {
                throw new InvalidOperationException(
                    "You can only set CurrentInkingMode to None by setting CurrentState to None or OnlyManipulating");
            }

            if (State != States.DrawingAndManipulating && State != States.OnlyDrawing)
            {
                throw new InvalidOperationException(
                    "You cannot change the CanvasMode in a non-drawing state (CurrentState : " + StateAsString + ") !");
            }

            switch (newInkingMode)
            {
                case InkingModes.Highlighter:
                    EditingMode = InkCanvasEditingMode.InkAndGesture;
                    break;
                case InkingModes.Pencil:
                    EditingMode = InkCanvasEditingMode.InkAndGesture;
                    break;
                case InkingModes.Pointer:
                    EditingMode = InkCanvasEditingMode.InkAndGesture;
                    break;
                case InkingModes.PointEraser:
                    EditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;
                case InkingModes.StrokeEraser:
                    EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
                case InkingModes.Selection:
                    EditingMode = InkCanvasEditingMode.Select;
                    break;
            }

            InkingMode = newInkingMode;
        }
    }
}