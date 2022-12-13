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
        public double X { get; set; }
        public double Y { get; set; }
        public double Ele { get; set; }
        public CPoint() { }
        public CPoint(double _x, double _y) { X = _x; Y = _y; }
        public CPoint(double _x, double _y, double _ele) { X = _x; Y = _y; Ele = _ele; }

    }
    #endregion
}
