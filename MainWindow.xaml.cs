using PZ2.Helpers;
using PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace PZ2
{
    public partial class MainWindow : Window
    {
        public double newX, newY, minX, minY, maxX, maxY, lengthX, lengthY, realX, realY;
        public List<Point> gridPoints = new List<Point>();
        public List<PowerEntity> powerEntities = new List<PowerEntity>();
        public List<LineEntity> lineEntities = new List<LineEntity>();
        public List<double> xPoints = new List<double>();
        public List<double> yPoints = new List<double>();
        public Point firstEnd, secondEnd;

        public Storyboard story = new Storyboard();
        public DoubleAnimation doubleAnimation1 = new DoubleAnimation();
        public DoubleAnimation doubleAnimation2 = new DoubleAnimation();

        public Dictionary<Point, PowerEntity> keyValuePairs = new Dictionary<Point, PowerEntity>();

        public MainWindow()
        {
            InitializeComponent();

            LoadAnimations();
        }

        private void LoadAnimations()
        {
            doubleAnimation1.From = 2;
            doubleAnimation1.To = 20;
            doubleAnimation1.AutoReverse = true;
            doubleAnimation1.Duration = new Duration(TimeSpan.FromSeconds(2));
            Storyboard.SetTargetProperty(doubleAnimation1, new PropertyPath(WidthProperty));

            doubleAnimation2.From = 2;
            doubleAnimation2.To = 20;
            doubleAnimation2.AutoReverse = true;
            doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(2));
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(HeightProperty));

            story.Children.Add(doubleAnimation1);
            story.Children.Add(doubleAnimation2);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            MakeGrid();

            GeographicXmlParser.LoadSubstations(powerEntities, newX, newY, xPoints, yPoints);
            GeographicXmlParser.LoadSwitches(powerEntities, newX, newY, xPoints, yPoints);
            GeographicXmlParser.LoadNodes(powerEntities, newX, newY, xPoints, yPoints);
              
            DrawElements();

            GeographicXmlParser.LoadLineEntities(lineEntities);

            foreach (LineEntity l in lineEntities)
            {
                Calculations.CalculatePoints(l, out firstEnd, out secondEnd, powerEntities, keyValuePairs);
                DrawLineEntities(firstEnd, secondEnd, l);
            }

        }

        private void DrawLineEntities(Point p1, Point p2, LineEntity l)
        {
            GeometryGroup group = new GeometryGroup();
            LineGeometry horizontalLine = new LineGeometry();
            LineGeometry verticalLine = new LineGeometry();

            horizontalLine.StartPoint = p1;
            horizontalLine.EndPoint = new Point(p2.X, p1.Y);

            verticalLine.StartPoint = new Point(p2.X, p1.Y);
            verticalLine.EndPoint = p2;

            group.Children.Add(horizontalLine);
            group.Children.Add(verticalLine);

            Path line = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 0.5,
                Fill = Brushes.Black,
                Visibility = Visibility.Visible,
                Data = group,
                ToolTip = "Id: " + l.Id + " \nName: " + l.Name
            };

            line.MouseRightButtonDown += line_MouseRightButtonDown;
            line.MouseRightButtonUp += line_MouseRightButtonUp;

            mycanvas.Children.Add(line);
        }

        public void MakeGrid()
        {
            Point p;
            List<Point> points = new List<Point>();

            for (int i = 0; i < 900; i = i + 3)
            {
                for (int j = 0; j < 630; j = j + 2)
                {
                    p = new Point(i, j);
                    points.Add(p);
                }
            }

            gridPoints = points;
        }

        private void DrawElements()
        {
            minX = xPoints.Min();
            minY = yPoints.Min();
            maxX = xPoints.Max();
            maxY = yPoints.Max();
            lengthX = (maxX - minX);
            lengthY = (maxY - minY);

            foreach (var element in powerEntities)
            {
                Converter.ToLatLon(element.X, element.Y, 34, out newX, out newY);
                Calculations.CalculateXY(newX, newY, out realX, out realY, lengthX, lengthY, minX, minY);

                Ellipse ellipse = new Ellipse();
                ellipse.Height = 2;
                ellipse.Width = 2;
                ellipse.Fill = element.Color;
                ellipse.ToolTip = element.ToolTip;

                ellipse.Name = "e" + element.Id.ToString();
                this.RegisterName(ellipse.Name, ellipse);
                ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;

                Point tacka = gridPoints.Find(t => t.X == realX && t.Y == realY);

                if (!keyValuePairs.ContainsKey(tacka))
                {
                    keyValuePairs.Add(tacka, element);
                }
                else
                {
                    tacka = Calculations.FindNearestFree(element, realX, realY, gridPoints, keyValuePairs);
                }

                Canvas.SetTop(ellipse, tacka.Y - 1);
                Canvas.SetLeft(ellipse, tacka.X - 1);
                mycanvas.Children.Add(ellipse);
            }
        }
   
        private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard.SetTargetName(doubleAnimation1, ((FrameworkElement)sender).Name);
            Storyboard.SetTargetName(doubleAnimation2, ((FrameworkElement)sender).Name);

            story.Begin((FrameworkElement)sender);
        }

        private void line_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string[] parts = ((Path)sender).ToolTip.ToString().Split(' ');
            string id = parts[1];
            LineEntity current = lineEntities.Find(v => v.Id == long.Parse(id));
            ((Ellipse)mycanvas.FindName("e" + current.FirstEnd.ToString())).Fill = powerEntities.Find(p => p.Id == current.FirstEnd).Color;
            ((Ellipse)mycanvas.FindName("e" + current.SecondEnd.ToString())).Fill = powerEntities.Find(p => p.Id == current.SecondEnd).Color;
        }

        private void line_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string[] parts = ((Path)sender).ToolTip.ToString().Split(' ');
            string id = parts[1];
            LineEntity current = lineEntities.Find(v => v.Id == long.Parse(id));
            ((Ellipse)mycanvas.FindName("e" + current.FirstEnd.ToString())).Fill = Brushes.DarkBlue;
            ((Ellipse)mycanvas.FindName("e" + current.SecondEnd.ToString())).Fill = Brushes.DarkBlue;
        }

    }
}
