using GrafikaProjekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = GrafikaProjekat.Model.Point;

namespace GrafikaProjekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, int> a = new Dictionary<int, int>();

        private Importer importer;
        private Dictionary<long, SubstationEntity> substations;
        private Dictionary<long, NodeEntity> nodes;
        private Dictionary<long, SwitchEntity> switches;
        private Dictionary<long, LineEntity> lines;

        private List<System.Windows.Point> pointsList;

        public static Dictionary<string, bool> points = new Dictionary<string, bool>();
        public static Dictionary<long, System.Windows.Point> entityIds = new Dictionary<long, System.Windows.Point>();


        Dictionary<string, Tuple<double, double>> nodesPositions;

        public double X;
        public double Y;

        public double xMax { get; set; }
        public double yMin { get; set; }
        public double xMin { get; set; }

        public double yMax { get; set; }

        public List<System.Windows.Point> PointsList { get => pointsList ; set => pointsList = value; }

        public Shape LastClickedObject;
        public TextBlock LastClickedText;
         
        int undoRedoPosition;

        List<object> history;
        private List<object> ClearHistory = new List<object>();
        public List<object> History { get => history; set => history = value; }
        public int UndoRedoPosition { get => undoRedoPosition; set => undoRedoPosition = value; }
        


        public MainWindow()
        {
            InitializeComponent();
            
            

            substations = new Dictionary<long, SubstationEntity>();
            nodes = new Dictionary<long, NodeEntity>();
            switches = new Dictionary<long, SwitchEntity>();
            lines = new Dictionary<long, LineEntity>();
            nodesPositions = new Dictionary<string, Tuple<double, double>>();
            PointsList = new List<System.Windows.Point>();
            History = new List<object>();
            UndoRedoPosition = -1;


        }
        private void LoadEntities()
        {
            substations = importer.GetSubstations();
            nodes = importer.GetNodes();
            switches = importer.GetSwitches();
            lines = importer.GetLines();
        }
        private Double zoomMax = 12;
        private Double zoomMin = 0.2;
        private Double zoomSpeed = 0.001;
        private Double zoom = 1;

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            zoom += zoomSpeed * e.Delta; // Adjust zooming speed (e.Delta = Mouse spin value )
            if (zoom < zoomMin) { zoom = zoomMin; } // Limit Min Scale
            if (zoom > zoomMax) { zoom = zoomMax; } // Limit Max Scale

            System.Windows.Point mousePos = e.GetPosition(canvas);

            if (zoom > 1)
            {
                canvas.RenderTransform = new ScaleTransform(zoom, zoom, mousePos.X, mousePos.Y); // transform Canvas size from mouse position
            }
            else
            {
                canvas.RenderTransform = new ScaleTransform(zoom, zoom); // transform Canvas size
            }
        }
        private void CalculateCoordination()
        {
            xMin = Math.Min(Math.Min(substations.Values.Min((item) => item.X), nodes.Values.Min((item) => item.X)), switches.Values.Min((item) => item.X)) - 0.01;
            xMax = Math.Max(Math.Max(substations.Values.Max((item) => item.X), nodes.Values.Max((item) => item.X)), switches.Values.Max((item) => item.X)) + 0.01;
            X = (xMax - xMin) / 300;

            yMin = Math.Min(Math.Min(substations.Values.Min((item) => item.Y), nodes.Values.Min((item) => item.Y)), switches.Values.Min((item) => item.Y)) - 0.01;
            yMax = Math.Max(Math.Max(substations.Values.Max((item) => item.Y), nodes.Values.Max((item) => item.Y)), switches.Values.Max((item) => item.Y)) + 0.01;
            Y = (yMax - yMin) / 300;
        }

        private void FindLines(Dictionary<long, LineEntity> lines, Dictionary<long, System.Windows.Point> ids)
        {
            List<GeoLines> foundedLines = new List<GeoLines>();
            List<LineEntity> foundedLines2 = new List<LineEntity>();

            foreach (var line in lines)
            {
                if (ids.ContainsKey(line.Value.FirstEnd) && ids.ContainsKey(line.Value.SecondEnd))
                {
                    foundedLines2.Add(line.Value);
                }
            }

            DrawLines(foundedLines2);
        }
        private void Draw()
        {
            if (substations.Count > 0)
            {
                foreach (var substation in substations)
                {
                    DrawEllipse(substation.Value.X, substation.Value.Y, substation.Value.Id, substation.Value.Name, "no status", new SolidColorBrush(Colors.Red));
                }
            }

            if (nodes.Count > 0)
            {
                foreach (var node in nodes)
                {

                    DrawEllipse(node.Value.X, node.Value.Y, node.Value.Id, node.Value.Name, "no status", new SolidColorBrush(Colors.ForestGreen));
                }
            }

            if (switches.Count > 0)
            {
                foreach (var sw in switches)
                {

                    DrawEllipse(sw.Value.X, sw.Value.Y, sw.Value.Id, sw.Value.Name, sw.Value.Status, new SolidColorBrush(Colors.DeepSkyBlue));
                }
            }

            FindLines(lines, entityIds);
        }


        private void DrawEllipse(double xValue, double yValue, long id, string name, string status, SolidColorBrush color)
        {
            double tempX = xMax - xValue;
            int pointX = (int)(tempX / X);

            double tempY = yValue - yMin;
            int pointY = (int)(tempY / Y);

            if (!points.ContainsKey(xValue + "," + yValue))
            {
                Ellipse el = new Ellipse();
                el.Stroke = color;
                el.Fill = color;
                el.Width = 5;
                el.Height = 5;
                el.ToolTip = "ID: " + id.ToString() + "\n" + "NAME: " + name + "\n" + "STATUS: " + status;
                el.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;
                Canvas.SetLeft(el, (pointY * 6) - 2);
                Canvas.SetTop(el, (pointX * 6) - 2);
                points[xValue + "," + yValue] = true;

                entityIds[id] = new System.Windows.Point(xValue, yValue);
                 
                canvas.Children.Add(el);
                nodesPositions.Add(id.ToString(), new Tuple<double, double>((pointX * 6) - 2, (pointY * 6) - 2));
            }
            else
            {
                FindNewCoordinates(xValue, yValue, out double xValueNew, out double yValueNew);
                DrawEllipse(xValueNew, yValueNew, id, name, status, color);
            }
        }
        private void FindNewCoordinates(double xValue, double yValue, out double newX, out double newY)
        {
            if (!points.ContainsKey(xValue + "," + (yValue + 1)))
            {
                newX = xValue;
                newY = yValue + 1;
            }
            else if (!points.ContainsKey(xValue + "," + (yValue - 1)))
            {
                newX = xValue;
                newY = yValue - 1;
            }
            else if (!points.ContainsKey((xValue + 1) + "," + yValue))
            {
                newX = xValue + 1;
                newY = yValue;
            }
            else if (!points.ContainsKey((xValue - 1) + "," + yValue))
            {
                newX = xValue - 1;
                newY = yValue;
            }
            else
            {
                newX = xValue + 1;
                newY = yValue + 1;
            }

        }

        void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.AutoReverse = true;
            myDoubleAnimation.From = 5;
            myDoubleAnimation.To = 10;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1.0));
            (sender as Ellipse).BeginAnimation(Ellipse.WidthProperty, myDoubleAnimation);
            (sender as Ellipse).BeginAnimation(Ellipse.HeightProperty, myDoubleAnimation);
        }


        private void DrawLines(List<LineEntity> Lines)
        {
            List<System.Windows.Shapes.Line> miniLines = new List<System.Windows.Shapes.Line>();

            foreach (var ln in Lines)
            {
                System.Windows.Shapes.Line newLine = new System.Windows.Shapes.Line();

                newLine.Y1 = nodesPositions[ln.FirstEnd.ToString()].Item1 + 2;
                newLine.X1 = nodesPositions[ln.FirstEnd.ToString()].Item2 + 2;
                newLine.Y2 = nodesPositions[ln.SecondEnd.ToString()].Item1 + 2;
                newLine.X2 = nodesPositions[ln.FirstEnd.ToString()].Item2 + 2;

                if (!miniLines.Contains(newLine))
                {
                    newLine.Stroke = System.Windows.Media.Brushes.DimGray;
                    newLine.StrokeThickness = 1;
                    

                    canvas.Children.Add(newLine);
                    miniLines.Add(newLine);
                }

                System.Windows.Shapes.Line newLine2 = new System.Windows.Shapes.Line();
                newLine2.Y1 = nodesPositions[ln.SecondEnd.ToString()].Item1 + 2;
                newLine2.X1 = nodesPositions[ln.FirstEnd.ToString()].Item2 + 2;
                newLine2.Y2 = nodesPositions[ln.SecondEnd.ToString()].Item1 + 2;
                newLine2.X2 = nodesPositions[ln.SecondEnd.ToString()].Item2 + 2;

                if (!miniLines.Contains(newLine2))
                {
                    newLine2.Stroke = System.Windows.Media.Brushes.DimGray;
                    newLine2.StrokeThickness = 1;

                    canvas.Children.Add(newLine2);
                    miniLines.Add(newLine2);
                }
            }
        }

        //List<LineEntity> lineEntities = new List<LineEntity>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            importer = new Importer();
            LoadEntities();
            CalculateCoordination();
            Draw();
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point point = e.GetPosition((IInputElement)sender);
            
            if ((bool)RBEllipse.IsChecked)
            {
                EllipseWindow drawElipseWindow = new EllipseWindow(point, this);
                
                drawElipseWindow.Show();
                
            }
            if ((bool)RBText.IsChecked)
            {
                Text addText = new Text(point, this);
                addText.Show();
            }
            else if ((bool)RBPolygon.IsChecked)
            {
                PointsList.Add(point);
            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((bool)RBPolygon.IsChecked)
            {
                if (this.pointsList.Count >= 3)
                {
                    PolygonWindow drawPolygonWindow = new PolygonWindow(this, PointsList);
                    //pointsList.Clear();
                    drawPolygonWindow.Show();
                }
                else
                {
                    System.Windows.MessageBox.Show("You have to choose at least 3 points for polygon!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (UndoRedoPosition > -1)
            {
                if (History[UndoRedoPosition] is Ellipse)
                {
                    Ellipse ell = (Ellipse)History[UndoRedoPosition];
                    this.canvas.Children.Remove(ell);
                    UndoRedoPosition--;
                }
                else if (History[UndoRedoPosition] is Polygon)
                {
                    Polygon pol = (Polygon)History[UndoRedoPosition];
                    this.canvas.Children.Remove(pol);
                    UndoRedoPosition--;
                }
                else if (History[UndoRedoPosition] is TextBlock)
                {
                    TextBlock tb = (TextBlock)History[UndoRedoPosition];
                    this.canvas.Children.Remove(tb);
                    UndoRedoPosition--;
                }
            }
            else 
            {
                foreach (var item in ClearHistory)
                {
                    if (item is Ellipse)
                    {
                        Ellipse ell = (Ellipse)item;
                        this.canvas.Children.Add(ell);
                        UndoRedoPosition++;
                    }
                    else if (item is Polygon)
                    {
                        Polygon pol = (Polygon)item;
                        this.canvas.Children.Add(pol);
                        UndoRedoPosition++;
                    }
                    else if (item is TextBlock)
                    {
                        TextBlock tb = (TextBlock)item;
                        this.canvas.Children.Add(tb);
                        UndoRedoPosition++;
                    }
                }

            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (History.Count > UndoRedoPosition + 1)
            {
                if (History[UndoRedoPosition + 1] is Ellipse)
                {
                    Ellipse ell = (Ellipse)History[UndoRedoPosition + 1];
                    this.canvas.Children.Add(ell);
                    UndoRedoPosition++;
                }
                else if (History[UndoRedoPosition + 1] is Polygon)
                {
                    Polygon pol = (Polygon)History[UndoRedoPosition + 1];
                    this.canvas.Children.Add(pol);
                    UndoRedoPosition++;
                }
                else if (History[UndoRedoPosition + 1] is TextBlock)
                {
                    TextBlock tb = (TextBlock)History[UndoRedoPosition + 1];
                    this.canvas.Children.Add(tb);
                    UndoRedoPosition++;
                }
            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in History)
            {
                if (item is Ellipse)
                {
                    Ellipse ell = (Ellipse)item;
                    ClearHistory.Add(ell);
                    this.canvas.Children.Remove(ell);

                }
                else if (item is Polygon)
                {
                    Polygon pol = (Polygon)item;
                    ClearHistory.Add(pol);
                    this.canvas.Children.Remove(pol);
                }
                else if (item is TextBlock)
                {
                    TextBlock tb = (TextBlock)item;
                    ClearHistory.Add(tb);
                    this.canvas.Children.Remove(tb);
                }
            }
            History.Clear();
            UndoRedoPosition = -1;
        }
    }
}
