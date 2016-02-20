using System;
using System.Collections.ObjectModel;
using Nine.Layers;
using Nine.Lessons.Contents;
using Nine.MVVM;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Allow showing a bar-chart onto the app.
    /// </summary>
    public class BarChart : BaseViewModel
    {
        private readonly Layers.Components.Charts.BarChart barChartModel;
        private ObservableCollection<AnswerData> _answers;
        private bool _isAnswered;
        private int _participantCount;
        private object _selectedItem;
        private string _subTitle = "Aucun participant";
        private string _title = "QCM";

        public BarChart(QuizAnswerLayer answerLayer)
        {
            Title = answerLayer.Content.Name;
            ParticipantCount = answerLayer.NbParticipants;
            Answers = new ObservableCollection<AnswerData>();
            IsAnswered = ((ExerciseContent) answerLayer.Content).HasBeenCollected;
            barChartModel = answerLayer.BarChart;
            X = barChartModel.Position.X;
            Y = barChartModel.Position.Y;

            //Get data & process it
            foreach (var d in barChartModel.Answers)
            {
                Answers.Add(new AnswerData {Category = "Réponse " + d.Key, Number = d.Value});
            }

            //Fixtures
            //Answers = new ObservableCollection<AnswerData>();
            //Answers.Add(new AnswerData() { Category = "Globalization", Number = 75 });
            //Answers.Add(new AnswerData() { Category = "Features", Number = 2 });
            //Answers.Add(new AnswerData() { Category = "ContentTypes", Number = 12 });
            //Answers.Add(new AnswerData() { Category = "Correctness", Number = 83 });
            //Answers.Add(new AnswerData() { Category = "Best Practices", Number = 29 });
        }

        public ObservableCollection<AnswerData> Answers
        {
            get { return _answers; }
            set
            {
                _answers = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Width");
            }
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }

        public double Width
        {
            get
            {
                var minWidth = 300;
                var oneColumWidth = 120;
                return Math.Max((Answers.Count*oneColumWidth), minWidth);
            }
        }

        public double X
        {
            get { return barChartModel.Position.X; }
            set
            {
                barChartModel.Position.X = value;
                RaisePropertyChanged();
            }
        }

        public double Y
        {
            get { return barChartModel.Position.Y; }
            set
            {
                barChartModel.Position.Y = value;
                RaisePropertyChanged();
            }
        }

        public int ParticipantCount
        {
            get { return _participantCount; }
            set
            {
                _participantCount = value;
                if (_participantCount > 1)
                {
                    SubTitle = _participantCount + " participants";
                }
                else if (_participantCount == 1)
                {
                    SubTitle = "1 participant";
                }
                else
                {
                    SubTitle = "Aucun participant";
                }
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAnswered
        {
            get { return _isAnswered; }
            set
            {
                _isAnswered = value;
                RaisePropertyChanged();
            }
        }

        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                _subTitle = value;
                RaisePropertyChanged();
            }
        }

        public class AnswerData
        {
            public string Category { get; set; }
            public int Number { get; set; }
        }
    }
}