using System.Windows.Media.Imaging;
using MuPdf.Helper;
using Nine.Layers;
using Nine.Lessons.Contents;
using Nine.MVVM;
using SaliencyMap;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Describe how to create and draw a saliency map at the view level from the right content.
    /// </summary>
    public class SaliencyMap : BaseViewModel
    {
        private bool _isAnswered;
        private BitmapSource _source;

        public SaliencyMap(GraphicalAnswerLayer answerLayer, double width, double height, int clusterSize = 50)
        {
            ISaliencyMap map = new MySaliencyMap((int) width, (int) height, clusterSize);
            foreach (var point in answerLayer.SaliencyMap.Points)
            {
                map.AddPoint((int) point.X, (int) point.Y);
            }
            map.Build();

            Source = map.GetGraphic(32).ToBitmapSourceBis(); // Fix High-Dpi screen issue
            IsAnswered = ((ExerciseContent) answerLayer.Content).HasBeenCollected;
        }

        public BitmapSource Source
        {
            get { return _source; }
            set
            {
                _source = value;
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
    }
}