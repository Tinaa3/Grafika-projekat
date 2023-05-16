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
    /// Interaction logic for EllipseWindow.xaml
    /// </summary>
    public partial class EllipseWindow : Window
    {
        public Point point { get; set; }
        public MainWindow mainWindow { get; set; }
        StackPanel stackPanel = new StackPanel();
        VisualBrush visualBrush = new VisualBrush();

        Ellipse ellipse = new Ellipse();
        public Shape LastClickedObject { get; set; }

        public EllipseWindow(Point point, MainWindow mainWindow)
        {
            InitializeComponent();
            this.point = point;
            this.mainWindow = mainWindow;


        }
        Brush ellipseColor;

        Brush textColor;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ellipseColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));

            }
        }
        private void Button_ClickText(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
        }
        Brush borderColor = Brushes.Black;

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                borderColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(RadiusX.Text) || string.IsNullOrWhiteSpace(RadiusY.Text))
            {
                System.Windows.MessageBox.Show("Please enter a value for both X and Y radius.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double radiusXValue, radiusYValue;
            if (!double.TryParse(RadiusX.Text, out radiusXValue) || !double.TryParse(RadiusY.Text, out radiusYValue))
            {
                System.Windows.MessageBox.Show("Invalid value for X or Y radius. Please enter valid numbers.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (radiusXValue < 0 || radiusYValue < 0)
            {
                System.Windows.MessageBox.Show("Invalid value for X or Y radius. Please enter valid numbers.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (string.IsNullOrWhiteSpace(Thickness.Text))
            {
                System.Windows.MessageBox.Show("Please enter a value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            double thicknessValue;
            if (!double.TryParse(Thickness.Text, out thicknessValue))
            {
                System.Windows.MessageBox.Show("Invalid value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (thicknessValue < 0)
            {
                System.Windows.MessageBox.Show("Invalid value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ellipse.Width = double.Parse(RadiusX.Text) * 2;
            ellipse.Height = double.Parse(RadiusY.Text) * 2;
            ellipse.Fill = ellipseColor;
            ellipse.Stroke = borderColor;

            ellipse.StrokeThickness = double.Parse(Thickness.Text);
            Canvas.SetLeft(ellipse, point.X);
            Canvas.SetTop(ellipse, point.Y);
            mainWindow.canvas.Children.Add(ellipse);
            mainWindow.History.Add(ellipse);
            mainWindow.UndoRedoPosition++;

            TextBlock myTextBlock = new TextBlock();

            if (Transparency.Text != "")
                ellipse.Opacity = 1 - (double.Parse(Transparency.Text) / 100);

            myTextBlock.Text = AddText.Text;
            myTextBlock.FontSize = (double)10;
            myTextBlock.Margin = new Thickness(10);

            myTextBlock.Foreground = textColor;

            stackPanel.Background = ellipseColor;
            stackPanel.Children.Add(myTextBlock);
            visualBrush.Visual = stackPanel;
            ellipse.Fill = visualBrush;

            ellipse.MouseLeftButtonDown += new MouseButtonEventHandler(ChangeObject);

            Canvas.SetLeft(mainWindow, point.X);
            Canvas.SetTop(mainWindow, point.Y);
           
            this.Close();
        }

        private void ChangeObject(object sender, RoutedEventArgs e)
        {
            mainWindow.LastClickedObject = ellipse;
            ChangeObject changeObject = new ChangeObject(mainWindow, stackPanel);
            changeObject.ShowDialog();
            return;
        }
    }


}

