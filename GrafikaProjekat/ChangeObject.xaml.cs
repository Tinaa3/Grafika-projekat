using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GrafikaProjekat
{
    /// <summary>
    /// Interaction logic for ChangeObject.xaml
    /// </summary>
    public partial class ChangeObject : Window
    {
        Brush borderColor = null;
        Brush fillColor = null;
        StackPanel stackPanel;
        MainWindow mainWindow { get; set; }
        public ChangeObject(MainWindow mainWindow, StackPanel stackPanel)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.stackPanel = stackPanel;
        }

        private void ChangeObjectColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fillColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
            
        }

        private void ChangeObjectBorderColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                borderColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
        }

        private void ChangeObject_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BorderThickness.Text))
            {
                System.Windows.MessageBox.Show("Please enter a value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double thicknessValue;
            if (!double.TryParse(BorderThickness.Text, out thicknessValue))
            {
                System.Windows.MessageBox.Show("Invalid value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (thicknessValue < 0)
            {
                System.Windows.MessageBox.Show("Invalid value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

                mainWindow.LastClickedObject.StrokeThickness = double.Parse(BorderThickness.Text);
            
                mainWindow.LastClickedObject.Stroke = borderColor;
            
                mainWindow.LastClickedObject.Fill = fillColor;
            
           
            this.Close();
        }
    }
}
