using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MoodProgram.Models
{
    public class Mouth
    {
        public PathFigure MouthFigure { get; set; }
        public SolidColorBrush ExpressionColor { get; set; }

        public Mouth(PathFigure mouthFigure, SolidColorBrush color)
        {
            MouthFigure = mouthFigure;
            ExpressionColor = color;
        }
    }
}
