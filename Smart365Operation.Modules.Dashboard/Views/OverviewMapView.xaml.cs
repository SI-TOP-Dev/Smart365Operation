﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Feeling.GIS.Map.Core;
using Smart365Operation.Modules.Dashboard.Interfaces;

namespace Smart365Operation.Modules.Dashboard
{
    /// <summary>
    /// OverviewMapView.xaml 的交互逻辑
    /// </summary>
    public partial class OverviewMapView : UserControl
    {
        public OverviewMapView()
        {
            InitializeComponent();
            ConfigMap();
            //this.DataContext = viewModel;
        }

        private void ConfigMap()
        {
            Map.Manager.Mode = AccessMode.ServerAndCache;
            Map.Position = new PointLatLng(34.210170, 108.869360);
            // Map.Zoom = 1;
            Map.MaxZoom = 16;
            Map.Zoom = 10;
            Map.MapTileType = MapType.GoogleMapChina;
            //comboBoxMapType.ItemsSource = Enum.GetValues(typeof(MapType));
            //var currentMarker = new MapMarker(Map.Position);
            //{
            //    //currentMarker.Shape = new PositionMarker(this, currentMarker, "position marker");
            //    currentMarker.Offset = new System.Windows.Point(-15, -15);
            //    currentMarker.ZIndex = int.MaxValue;
            //    Map.Markers.Add(currentMarker);
            //}

        }

        private void repeatbtn_left_Click(object sender, RoutedEventArgs e)
        {
            Map.MoveMap(50, 0);
        }

        private void repeatbtn_right_Click(object sender, RoutedEventArgs e)
        {
            Map.MoveMap(-50, 0);
        }

        private void repeatbtn_top_Click(object sender, RoutedEventArgs e)
        {
            Map.MoveMap(0, 50);
        }

        private void repeatbtn_bottom_Click(object sender, RoutedEventArgs e)
        {
            Map.MoveMap(0, -50);
        }

        private void repeatbtn_hand_Click(object sender, RoutedEventArgs e)
        {
            Map.Cursor = Cursors.Hand;
        }
    }
}
