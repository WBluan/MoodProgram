using MoodProgram.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Runtime.CompilerServices;

namespace MoodProgram.ViewModel
{
    public class MouthViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Mouth _currentMouth;
        public Mouth CurrentMouth 
        {
            get => _currentMouth;
            set 
            {
                _currentMouth = value;
                OnPropertyChanged();
            }
        }
        private double _value;
        public double Value 
        {
            get => _value;
            set
            {
                if(_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MoodDescription));
                    UpdateExpression(value);
                }
            }
        }
        public Mouth HappyExpression { get; private set; }
        public Mouth NeutralExpression { get; private set; }
        public Mouth SadExpression { get; private set; }
        public string MoodDescription 
        {
            get
            {
                if (Value <= 0.33)
                    return "SAD";
                else if (Value > 0.33 && Value < 0.66)
                    return "NEUTRAL";
                else
                    return "HAPPY";
            }
        }
        public MouthViewModel()
        {
            InitializeExpressions();
            CurrentMouth = NeutralExpression;
        }

        private void InitializeExpressions()
        {
            HappyExpression = new Mouth
            (
                new PathFigure
                {
                    StartPoint = new Point(50, 140),
                    Segments = new PathSegmentCollection
                    {
                        new QuadraticBezierSegment(new Point(105, 170), new Point(160, 135), true)
                    }
                },
                new SolidColorBrush(Colors.LightGreen)
            );

            NeutralExpression = new Mouth
            (
                new PathFigure
                {
                    StartPoint = new Point(30, 140),
                    Segments = new PathSegmentCollection
                    {
                        new QuadraticBezierSegment(new Point(105, 140), new Point(160, 140), true)
                    }
                },
                new SolidColorBrush(Colors.LightYellow)
            );

            SadExpression = new Mouth
            (
                new PathFigure
                {
                    StartPoint = new Point(30, 190),
                    Segments = new PathSegmentCollection
                    {
                        new QuadraticBezierSegment(new Point(105, 140), new Point(160, 190), true)
                    }
                },
                new SolidColorBrush(Colors.IndianRed)
            );
        }

        public void UpdateExpression(double value)
        {
            Value = value;
            if (value <= 0.5)
            {
                double progress = value / 0.5;
                CurrentMouth = InterpolateExpression(SadExpression, NeutralExpression, progress);
            }
            else
            {
                double progress = (value - 0.5) / 0.5;
                CurrentMouth = InterpolateExpression(NeutralExpression, HappyExpression, progress);
            }
        }

        private Mouth InterpolateExpression(Mouth start, Mouth end, double progress)
        {
            var interpolatedFigure = InterpolateFigures(start.MouthFigure, end.MouthFigure, progress);
            var interpolatedColor = InterpolateColors(start.ExpressionColor, end.ExpressionColor, progress);

            return new Mouth(interpolatedFigure, interpolatedColor);
        }

        private SolidColorBrush InterpolateColors(SolidColorBrush startColor, SolidColorBrush endColor, double progress)
        {
            byte a = (byte)(startColor.Color.A + (endColor.Color.A - startColor.Color.A) * progress);
            byte r = (byte)(startColor.Color.R + (endColor.Color.R - startColor.Color.R) * progress);
            byte g = (byte)(startColor.Color.G + (endColor.Color.G - startColor.Color.G) * progress);
            byte b = (byte)(startColor.Color.B + (endColor.Color.B - startColor.Color.B) * progress);

            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
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

                var interpolatedSegment = new QuadraticBezierSegment
                {
                    Point1 = InterpolatePoint(startSegment.Point1, endSegment.Point1, progress),
                    Point2 = InterpolatePoint(startSegment.Point2, endSegment.Point2, progress)
                };

                interpolatedFigure.Segments.Add(interpolatedSegment);
            }
            return interpolatedFigure;
        }

        private Point InterpolatePoint(Point start, Point end, double progress)
        {
            double x = start.X + (end.X - start.X) * progress;
            double y = start.Y + (end.Y - start.Y) * progress;
            return new Point(x, y);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
