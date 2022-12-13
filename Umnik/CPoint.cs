using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umnik
{
    //Класс точка - координаты
    #region

    internal class CPoint
    {
        public PointLatLng Point { get; set; }
        public double Ele { get; set; }
        public CPoint() { }
        public CPoint(double _y, double _x) { Point = new PointLatLng(_y, _x); }
        public CPoint(double _y, double _x, double _ele) { Point = new PointLatLng(_y, _x); Ele = _ele; }

    }
    #endregion
}
