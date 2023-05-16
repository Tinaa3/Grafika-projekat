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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GrafikaProjekat
{
    /// <summary>
    /// Interaction logic for Polygon.xaml
    /// </summary>
    public partial class PolygonWindow : System.Windows.Window
    {
        Brush polygonColor;
        Brush contourColor;
        Brush polygonTextColor;
        StackPanel stackPanel = new StackPanel();
        Polygon polygon = new Polygon();

        public MainWindow mainWindow;

        List<Point> pointList;

        public PolygonWindow(MainWindow main, List<Point>points)
        {
            InitializeComponent();
            this.mainWindow = main;
            this.pointList = points;
        }

        private void ContourColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                contourColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));

            }
        }

        private void PolygonColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                polygonColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));

            }
        }

        private void PolygonTextColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                polygonTextColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));

            }
        }

        private void DrawPolygon_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ContourThickness.Text))
            {
                System.Windows.MessageBox.Show("Please enter a value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double thicknessValue;
            if (!double.TryParse(ContourThickness.Text, out thicknessValue))
            {
                System.Windows.MessageBox.Show("Invalid value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (thicknessValue < 0)
            {
                System.Windows.MessageBox.Show("Invalid value for Thickness.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            polygon.Stroke = contourColor;
            polygon.StrokeThickness = double.Parse(ContourThickness.Text);
            foreach (var item in pointList)
            {

                polygon.Points.Add(item);

            }
            if (PolygonTransparency.Text != "")
            {
                polygon.Opacity = 1 - (double.Parse(PolygonTransparency.Text) / 100);

            }
            polygon.Fill = polygonColor;
            mainWindow.canvas.Children.Add(polygon);
            mainWindow.PointsList.Clear();
            mainWindow.History.Add(polygon);
            mainWindow.UndoRedoPosition++;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = AddTextP.Text;
            textBlock.Foreground = polygonTextColor;
            Canvas.SetLeft(textBlock, 100);
            Canvas.SetTop(textBlock, 100);
            mainWindow.canvas.Children.Add(textBlock);
            polygon.MouseLeftButtonDown += new MouseButtonEventHandler(ChangeObject);

            this.Close();
        }

        private void ChangeObject(object sender, RoutedEventArgs e)
        {
            mainWindow.LastClickedObject = polygon;
            ChangeObject changeObject = new ChangeObject(mainWindow, stackPanel);
            changeObject.ShowDialog();
            return;
        }
    }
}
