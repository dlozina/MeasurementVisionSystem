using System;
using System.Collections.Generic;
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
using Microsoft.Expression.Shapes;

namespace MjerniStolLimovi
{
	/// <summary>
	/// Interaction logic for A.xaml
	/// </summary>
	public partial class A : UserControl , ILimKontrola
	{
        List<Arc> colorPointList = new List<Arc>();
        List<object> pointList = new List<object>();
        List<Point> pointListTranslatedAndRotated = new List<Point>();
        
        public Grid Circles
        {
            get { return circles; }
            set { circles = value; }
        }

        public Grid MainLines
        {
            get { return mainLines; }
            set { mainLines = value; }
        }

        public Grid PointArcs
        {
            get { return pointArcs; }
            set { pointArcs = value; }
        }

        public Grid CircleArcs
        {
            get { return circleArcs; }
            set { circleArcs = value; }
        }

        public Grid Centerlines
        {
            get { return centerlines; }
            set { centerlines = value; }
        }

        public List<string> MeasuresList
        {
            get 
            {
                List<string> measTemp = new List<string>();
                foreach (string s in measuresArray.Items)
                {
                    measTemp.Add(s);
                }
                return measTemp; 
            }
        }
        
        int purpose = 0;
        public int Purpose
        {
            get { return purpose; }
            set 
            { 
                purpose = value;
                LimoviCommon.purposeLookRefresh(purpose, pointArcs, lines, lineArrows, dimensionNames, lineEnumeration, circleEnumeration, angleEnumeration, circleArcs, centerlines);
            }
        }

        public List<object> PointList
        {
            get { return pointList; }
            set
            {
                pointList = value;
                LimoviCommon.pointColoring(PointList, colorPointList, Purpose);
                if (PointList.Count >= colorPointList.Count)
                {
                    pointListTranslatedAndRotated = LimoviCommon.translateAndRotate(PointList, (Point)PointList[10], (Point)PointList[0]);
                    var tempList = GetAll();
                }
            }
        }

        public String SheetName
        {
            get { return TextBlockA.Text; }
        }

        public A()
        {
            this.InitializeComponent();
            colorPointList.Add(Point1);
            colorPointList.Add(Point2);
            colorPointList.Add(Point3);
            colorPointList.Add(Point4);
            colorPointList.Add(Point5);
            colorPointList.Add(Point6);
            colorPointList.Add(Point7);
            colorPointList.Add(Point8);
            colorPointList.Add(Point9);
            colorPointList.Add(Point10);
            colorPointList.Add(Point11);
            LimoviCommon.pointColoring(PointList, colorPointList, Purpose);
        }

        //double GetOne(int measureNumber)
        //{
        //    double centerline2X, centerline1X;
        //    switch (measureNumber)
        //    {
        //        case 0: // B
        //            return (Math.Abs(pointListTranslatedAndRotated[5].Y) + Math.Abs(pointListTranslatedAndRotated[4].Y) + Math.Abs(pointListTranslatedAndRotated[2].Y) + Math.Abs(pointListTranslatedAndRotated[1].Y)) / 4;
        //        case 1: // L4 BOTTOM
        //            centerline2X = Math.Abs(pointListTranslatedAndRotated[6].X + pointListTranslatedAndRotated[5].X) / 2;
        //            return Math.Abs(pointListTranslatedAndRotated[10].X - centerline2X);
        //        case 2: // L5
        //            return Math.Abs(pointListTranslatedAndRotated[9].X - pointListTranslatedAndRotated[10].X);
        //        case 3: // L6
        //            return Math.Abs(pointListTranslatedAndRotated[3].X - pointListTranslatedAndRotated[9].X);
        //        case 4: // L3
        //            return Math.Abs(pointListTranslatedAndRotated[8].X - pointListTranslatedAndRotated[3].X);
        //        case 5: // L2
        //            return Math.Abs(pointListTranslatedAndRotated[7].X - pointListTranslatedAndRotated[8].X);
        //        case 6: // L1
        //            centerline1X = Math.Abs(pointListTranslatedAndRotated[1].X + pointListTranslatedAndRotated[0].X) / 2;
        //            return Math.Abs(centerline1X - pointListTranslatedAndRotated[7].X);
        //        default:
        //            return 0;
        //    }
        //}

        double GetOne(int measureNumber)
        {
            double centerline2X, centerline1X;
            switch (measureNumber)
            {
                case 0: // B
                    return (Math.Abs(pointListTranslatedAndRotated[9].Y) + Math.Abs(pointListTranslatedAndRotated[6].Y) + Math.Abs(pointListTranslatedAndRotated[4].Y) + Math.Abs(pointListTranslatedAndRotated[1].Y)) / 4;
                case 3: // L4 BOTTOM ok
                    centerline1X = Math.Abs(pointListTranslatedAndRotated[1].X + pointListTranslatedAndRotated[0].X) / 2;
                    return Math.Abs(centerline1X - pointListTranslatedAndRotated[2].X);
                case 2: // L5 ok
                    return Math.Abs(pointListTranslatedAndRotated[2].X - pointListTranslatedAndRotated[3].X);
                case 1: // L6 ok
                    return Math.Abs(pointListTranslatedAndRotated[3].X - pointListTranslatedAndRotated[5].X);
                case 6: // L3 ok
                    return Math.Abs(pointListTranslatedAndRotated[5].X - pointListTranslatedAndRotated[7].X);
                case 5: // L2 ok
                    return Math.Abs(pointListTranslatedAndRotated[7].X - pointListTranslatedAndRotated[8].X);
                case 4: // L1 ok
                    centerline2X = Math.Abs(pointListTranslatedAndRotated[10].X + pointListTranslatedAndRotated[9].X) / 2;
                    return Math.Abs(pointListTranslatedAndRotated[8].X - centerline2X);
                case 7:
                    return (GetOne(1) + GetOne(2) + GetOne(3) + GetOne(4) + GetOne(5) + GetOne(6));
                default:
                    return 0;
            }
        }

        public List<double> GetAll()
        {
            var tempList = new List<double>();
            for (int i = 0; i < measuresArray.Items.Count; i++)
            {
                tempList.Add(GetOne(i));
            }
            return tempList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PointList.Add(new Point(11.2, 1.6));
            PointList = PointList;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PointList.Add(null);
            PointList = PointList;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PointList = new List<object>();
            PointList = PointList;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            PointList = PointList;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (PointList.Count < 14)
            {
                PointList.Add(new Point(99,36));
                PointList.Add(new Point(84,15.5));
                PointList.Add(new Point(61.2,11.5));
                PointList.Add(new Point(51.3,18.5));
                PointList.Add(new Point(44,8.5));
                PointList.Add(new Point(21,5));
                PointList.Add(new Point(0.5,19.5));
                PointList.Add(new Point(79,24));
                PointList.Add(new Point(71.2,23));
                PointList.Add(new Point(63.5,21.3));
                PointList.Add(new Point(39,17.2));
                PointList.Add(new Point(30.8,16));
                PointList.Add(new Point(23,14.5));
                PointList = PointList;
            }
        }

        /*
        if (PointList.Count < 14)
            {
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList.Add(new Point());
                PointList = PointList;
            }
         */


        #region userControl_MouseDown
        /*
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Path tempLine;
            Ellipse tempCircle;
            
            if (Mouse.DirectlyOver is Path)
            {
                tempLine = ((Path)Mouse.DirectlyOver);
                if (tempLine.Name.Substring(0,4).Contains("Line"))
                {
                    for (int i = 0; i < mainLines.Children.Count; i++)
                    {
                        ((Path)mainLines.Children[i]).Stroke = Brushes.Black;
                    }
                    for (int i = 0; i < Circles.Children.Count; i++)
                    {
                        ((Ellipse)Circles.Children[i]).Stroke = Brushes.Black;
                    }
                    tempLine.Stroke = Brushes.OrangeRed;
                }
            }

            if (Mouse.DirectlyOver is Ellipse)
            {
                tempCircle = ((Ellipse)Mouse.DirectlyOver);
                if (tempCircle.Name.Substring(0, 6).Contains("Circle"))
                {
                    for (int i = 0; i < mainLines.Children.Count; i++)
                    {
                        ((Path)mainLines.Children[i]).Stroke = Brushes.Black;
                    }
                    for (int i = 0; i < Circles.Children.Count; i++)
                    {
                        ((Ellipse)Circles.Children[i]).Stroke = Brushes.Black;
                    }
                    tempCircle.Stroke = Brushes.OrangeRed;
                }
            }
        }
        */
        #endregion

    }
}