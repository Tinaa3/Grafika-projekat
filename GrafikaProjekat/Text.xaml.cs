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
    /// Interaction logic for Text.xaml
    /// </summary>
    public partial class Text : Window
    {
        public Point point { get; set; }
        public MainWindow mainWindow { get; set; }
        TextBlock myTextBlock = new TextBlock();
        Brush textColor = Brushes.Black;

        public Text(Point point, MainWindow mainWindow)
        {
            InitializeComponent();
            this.point = point;
            this.mainWindow = mainWindow;

        }

        private void ColorTextButton_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
        }

        private void AddTextButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddTextBox.Text))
            {
                System.Windows.MessageBox.Show("Please enter some text.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (AddTextBox.Text != "" && FontSize.Text != "")
            {
                myTextBlock.Text = AddTextBox.Text;
                myTextBlock.FontSize = double.Parse(FontSize.Text);
                myTextBlock.FontWeight = FontWeights.Bold;
                myTextBlock.Foreground = textColor;
                myTextBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                myTextBlock.VerticalAlignment = VerticalAlignment.Center;
                mainWindow.canvas.Children.Add(myTextBlock);
                myTextBlock.MouseLeftButtonDown += new MouseButtonEventHandler(ChangeObject);

                Canvas.SetLeft(myTextBlock, point.X);
                Canvas.SetTop(myTextBlock, point.Y);
               
                mainWindow.History.Add(myTextBlock);
                mainWindow.UndoRedoPosition++;
                this.Close();
            }
        }

        private void ChangeObject(object sender, RoutedEventArgs e)
        {
            mainWindow.LastClickedText = myTextBlock;
            ChangeText changeText = new ChangeText(mainWindow);
            changeText.ShowDialog();
            return;
        }
    }
}
