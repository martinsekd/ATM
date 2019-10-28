﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ATM.Interfaces;

namespace ATM.System
{
    public class Program : Application
    {
        private static Program gui;
        private static Window window;
        private static Canvas can;
        public static Shape[] shapes;

        [STAThread]
        static void Main(string[] args)
        {
            
            
            Thread t = new Thread(guimetode);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            
            
            var rec = TransponderReceiver.TransponderReceiverFactory.CreateTransponderDataReceiver();

            IDataFormatter df = new DataFormatter(rec);
            IFlightCollection flightCollection = new FlightCollection(new FlightCalculator(), df);
            ICollisionDetector collisionDetector = new CollisionDetector(flightCollection, new CollisionCollection());
            ILog logger = new Log(collisionDetector);

            //rec.TransponderDataReady += ATM.Receive;
            t.Join();

            //Log.WriteLog("ConsoleLog.txt", String.Format("{0} @ {1}", "Log is Created by Martinique and Magnus the mosquito", DateTime.Now));

            while (true)
            {

            }
        }

        [STAThread]
        static public void guimetode()
        {
            gui = new Program();
            window = new Window();

            var field = new Rectangle();
            field.Stroke = Brushes.Red;
            field.Width = 500;
            field.Height = 500;
            can = new Canvas();
            can.Children.Add(field);
            Canvas.SetTop(field,0);
            Canvas.SetLeft(field, 0);
            window.Content = can;

            shapes = new Ellipse[30];
            for(int i=0;i<30;i++) {
                shapes[i] = new Ellipse();
            shapes[i].Height = 10;
            shapes[i].Width = 10;
            can.Children.Add(shapes[i]);
            can.Background = Brushes.Gray;
            Canvas.SetTop(shapes[i], 100);
            Canvas.SetLeft(shapes[i], 600);
            shapes[i].Fill = Brushes.Green;
            }
            window.Show();
            gui.Run();


        }
        public delegate void koorDelegate();

        public static void setflight(double x, double y,int index)
        {
            koorDelegate metode = delegate
            {
                Canvas.SetTop(shapes[index], y);
                Canvas.SetLeft(shapes[index], x);
            };
            Application.Current.Dispatcher.BeginInvoke(metode);
        }
    }
}
