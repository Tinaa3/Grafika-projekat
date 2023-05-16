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
    /// Interaction logic for ChangeText.xaml
    /// </summary>
    public partial class ChangeText : Window
    {
        Brush textColor = null;
        MainWindow mainWindow { get; set; }
        public ChangeText(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void ChangeText_Click(object sender, RoutedEventArgs e)
        {
            if (FontSizetb.Text != "")
            {
                mainWindow.LastClickedText.FontSize = double.Parse(FontSizetb.Text);
            }
            if (ChangeColorText != null)
            {
                mainWindow.LastClickedText.Foreground = textColor;
            }

            this.Close();
        }

        private void ChangeColorText_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
           
        }
    }
}
