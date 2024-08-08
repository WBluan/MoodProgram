using MoodProgram.ViewModel;
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
        private MouthViewModel ViewModel => DataContext as MouthViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ExpressionSlider.Value = 0.5;
            ViewModel?.UpdateExpression(ExpressionSlider.Value);
        }

        private void ExpressionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ViewModel.UpdateExpression(ExpressionSlider.Value);
        }
    }
    
}