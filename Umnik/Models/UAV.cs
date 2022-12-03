using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Orange
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

        public UAV(int number = 0, PointLatLng coordinates = new PointLatLng())
        {
            int colour = number + 1;
            Name = colour.ToString();
            DroneColor = (DroneColour)colour;
            Path = $@"Icons/uav-mini-{DroneColor.ToString().ToLower()}.png";
            Icon = new Bitmap(Path);
            Coordinates = coordinates;
        }
    }


}
