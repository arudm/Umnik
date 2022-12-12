﻿using GeoCoordinatePortable;
using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Umnik
{
    enum DroneColour
    {
        Black,
        Red,
        Green,
        Yellow,
        Blue,
        Orange,
        MaxDroneColour
    }
    internal class UAV
    {
        private string name;
        public string Name
        {
            get
            {
                return name;             // возвращаем значение свойства
            }
            set
            {
                name = $"Drone {value}"; // устанавливаем новое значение свойства
            }
        }

        private Bitmap icon;
        public Bitmap Icon
        {
            get
            {
                return icon;    // возвращаем значение свойства
            }
            set
            {
                icon = value;   // устанавливаем новое значение свойства
            }
        }

        public PointLatLng Coordinates { get; set; }

        public string Path { get; set; }

        public DroneColour DroneColor { get; set; }
        public GeoCoordinate GeoCoordinate { get; set; }
        public GMarkerGoogleType MarkerGoogleTypeColour { get; set; }
        public Color SystemColor { get; set; }

        public UAV(int number = 0, PointLatLng coordinates = new PointLatLng())
        {
            int colour = number;
            Name = colour.ToString();
            DroneColor = (DroneColour)colour;
            Path = $@"Icons/uav-mini-{DroneColor.ToString().ToLower()}.png";
            Icon = new Bitmap(Path);
            Coordinates = coordinates;
            GeoCoordinate = new GeoCoordinate(coordinates.Lat, coordinates.Lng);
            MarkerGoogleTypeColour = CheckDroneColourForMarkerGoogleType(DroneColor);
            SystemColor = CheckDroneSystemColour(DroneColor);
        }
        private GMarkerGoogleType CheckDroneColourForMarkerGoogleType(DroneColour colour)
        {
            switch (colour)
            {
                case DroneColour.Black:
                    return GMarkerGoogleType.black_small;
                case DroneColour.Red:
                    return GMarkerGoogleType.red;
                case DroneColour.Green:
                    return GMarkerGoogleType.green;
                case DroneColour.Yellow:
                    return GMarkerGoogleType.yellow;
                case DroneColour.Blue:
                    return GMarkerGoogleType.blue;
                case DroneColour.Orange:
                    return GMarkerGoogleType.orange;
            }
            return GMarkerGoogleType.white_small;
        }

        private Color CheckDroneSystemColour(DroneColour colour)
        {
            switch (colour)
            {
                case DroneColour.Black:
                    return Color.Black;
                case DroneColour.Red:
                    return Color.Red;
                case DroneColour.Green:
                    return Color.Green;
                case DroneColour.Yellow:
                    return Color.Yellow;
                case DroneColour.Blue:
                    return Color.Blue;
                case DroneColour.Orange:
                    return Color.Orange;
            }
            return Color.White;
        }
    }


}