using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoodProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PathFigure happyMouth;
        private PathFigure neutralMouth;
        private PathFigure sadMouth;
        public MainWindow()
        {
            InitializeComponent();
            InitializeFaces();
        }

        private void InitializeFaces()
        {

            happyMouth = new PathFigure
            {
                StartPoint = new Point(80, 140),
                Segments = new PathSegmentCollection
                {
                     new QuadraticBezierSegment(new Point(135, 170), new Point(190, 135), true)
                }
            };

            neutralMouth = new PathFigure
            {
                StartPoint = new Point(60, 140),
                Segments = new PathSegmentCollection
                {
                    new QuadraticBezierSegment(new Point(135, 140), new Point(190, 140), true)
                }
            };

            sadMouth = new PathFigure
            {
                StartPoint = new Point(60, 190),
                Segments = new PathSegmentCollection
                {
                    new QuadraticBezierSegment(new Point(135, 140), new Point(190, 190), true)
                }
            };

            Mouth.Data = new PathGeometry(new[] {neutralMouth});
        }

        private void ExpressionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = ExpressionSlider.Value;
            PathFigure currentMouth = InterpolateMouth(value);
            Mouth.Data = new PathGeometry(new[] { currentMouth });
        }

        private PathFigure InterpolateMouth(double t)
        {
            if(t <= 0.5)
            {
                double progress = t / 0.5;
                return InterpolateFigures(neutralMouth, happyMouth, progress);
            }
            else
            {
                double progress = (t - 0.5) / 0.5;
                return InterpolateFigures(neutralMouth, sadMouth, progress);
            }
        }

        private PathFigure InterpolateFigures(PathFigure startFigure, PathFigure endFigure, double progress)
        {
            var interpolatedFigure = new PathFigure
            {
                StartPoint = InterpolatePoint(startFigure.StartPoint, endFigure.StartPoint, progress),
                Segments = new PathSegmentCollection()
            };

            for (int i = 0; i < startFigure.Segments.Count; i++)
            {
                var startSegment = startFigure.Segments[i] as QuadraticBezierSegment;
                var endSegment = endFigure.Segments[i] as QuadraticBezierSegment;

                var interpolatedSegments = new QuadraticBezierSegment
                {
                    Point1 = InterpolatePoint(startSegment.Point1, endSegment.Point1, progress),
                    Point2 = InterpolatePoint(startSegment.Point2, endSegment.Point2, progress)
                };

                interpolatedFigure.Segments.Add(interpolatedSegments);
            }
            return interpolatedFigure;
        }
        private Point InterpolatePoint(Point start, Point end, double progress)
        {
            double x = start.X + (end.X - start.X) * progress;
            double y = start.Y + (end.Y - start.Y) * progress;
            return new Point(x, y);
        }
    }
    
}