using GeoCoordinatePortable;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Umnik.MapForm;

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
    internal class Drone
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

        public GMapOverlay MarkersOverlay { get; set; }
        public GMapOverlay RoutesOverlay { get; set; }
        public GMapOverlay PolygonsOverlay { get; set; }

        public List<CPoint> MarkersList { get; set; }
        public List<CPoint> RoutesList { get; set; }
        public List<CPoint> PolygonsList { get; set; }

        public List<PointLatLng> MarkersPointList { get; set; }
        public List<PointLatLng> RoutesPointList { get; set; }
        public List<PointLatLng> PolygonsPointList { get; set; }

        public GMarkerGoogle DroneMarker { get; set; }
        public Drone(DroneColour colour = DroneColour.Black, PointLatLng coordinates = new PointLatLng())
        {
            Name = ((int)colour).ToString();
            DroneColor = colour;
            Path = $@"Icons/uav-mini-{DroneColor.ToString().ToLower()}.png";
            Icon = new Bitmap(Path);
            Coordinates = coordinates;
            DroneMarker = new GMarkerGoogle(new PointLatLng(Coordinates.Lat, Coordinates.Lng), Icon);
            GeoCoordinate = new GeoCoordinate(coordinates.Lat, coordinates.Lng);
            MarkerGoogleTypeColour = CheckDroneColourForMarkerGoogleType(DroneColor);
            SystemColor = CheckDroneSystemColour(DroneColor);
            MarkersOverlay = new GMapOverlay(Name);
            RoutesOverlay = new GMapOverlay(Name);
            PolygonsOverlay = new GMapOverlay(Name);
            MarkersList = new List<CPoint>();
            RoutesList = new List<CPoint>();
            PolygonsList = new List<CPoint>();
            MarkersPointList = new List<PointLatLng>();
            RoutesPointList = new List<PointLatLng>();
            PolygonsPointList = new List<PointLatLng>();
        }

        //public delegate void OnRemoveMarkerFromOverlayEventHandler(GMapMarker marker, GMapOverlay overlay);
        //public static event OnRemoveMarkerFromOverlayEventHandler? OnRemoveMarkerFromOverlayEvent;

        public void RemoveMarkerFromOverlay(GMapMarker marker, GMapOverlay overlay)
        {
            //OnRemoveMarkerFromOverlayEvent?.Invoke(marker, overlay);
            if (overlay == MarkersOverlay)
            {
                var currentMarkersOverlayListClick = MarkersList;
                var currentMarkersPointsList = MarkersPointList;
                DeleteMarkFromLists(marker, overlay, currentMarkersOverlayListClick!, currentMarkersPointsList!);
                //overlay.Clear();
            }
            else if (overlay == RoutesOverlay)
            {
                var currentRoutesOverlayListClick = RoutesList;
                var currentRoutesPointsList = RoutesPointList;
                DeleteMarkFromLists(marker, overlay, currentRoutesOverlayListClick!, currentRoutesPointsList!);
                overlay.Routes.Clear();
                AddRoutesMark(overlay, currentRoutesPointsList);
            }
            else if (overlay == PolygonsOverlay)
            {
                var currentPolygonsOverlayListClick = PolygonsList;
                var currentPolygonPointsList = PolygonsPointList;
                DeleteMarkFromLists(marker, overlay, currentPolygonsOverlayListClick!, currentPolygonPointsList!);
                overlay.Polygons.Clear();
                AddPolygonsMark(overlay, currentPolygonPointsList);
            }

            // Удаляем в этом слое маркер
            overlay.Markers.Remove(marker);
        }
        private void DeleteMarkFromLists(GMapMarker marker, GMapOverlay overlay, List<CPoint> overlayListClick, List<PointLatLng> points)
        {
            for (int i = 0; i < overlayListClick.Count; i++)
            {
                if (overlay.Markers[i].Equals(marker))
                {
                    overlayListClick.RemoveAt(i);
                }
            }
            for (int j = 0; j < points.Count; j++)
            {
                if (points[j] == marker.Position)
                {
                    PointLatLng currentPoint = points[j];
                    points.Remove(currentPoint);
                }
            }
        }

        private void AddRoutesMark(GMapOverlay overlayClick, List<PointLatLng> points)
        {
            // Получаем System.сolor для кисти и карандаша
            GMapRoute routeClick = new GMapRoute(points, "routeClick");
            routeClick.Stroke = new Pen(SystemColor);

            if (overlayClick.Routes.Count != 0)
                overlayClick.Routes.Clear();

            overlayClick.Routes.Add(routeClick);
        }

        private void AddPolygonsMark(GMapOverlay overlayClick, List<PointLatLng> points)
        {
            var polygon = new GMapPolygon(points, "polygonClick");
            polygon.Fill = new SolidBrush(System.Drawing.Color.FromArgb(50, SystemColor));
            polygon.Stroke = new Pen(SystemColor);

            if (overlayClick.Polygons.Count != 0)
                overlayClick.Polygons.Clear();

            overlayClick.Polygons.Add(polygon);
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
