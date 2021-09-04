﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Globalization;
using System.Device.Location;

namespace Umnik
{
    //комент для гита
    public partial class Map : Form
    {
        //Класс точка - координаты
        #region

        public class CPoint
        {
            public double x { get; set; }
            public double y { get; set; }
            public double ele { get; set; }
            public CPoint() { }
            public CPoint(double _x, double _y) { x = _x; y = _y; }
            public CPoint(double _x, double _y, double _ele) { x = _x; y = _y; ele = _ele; }

        }
        #endregion

        public Map()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            // Создание элементов меню
            ToolStripMenuItem YandexMenuItem = new ToolStripMenuItem("Установить Яндекс-карту");
            ToolStripMenuItem GoogleMenuItem = new ToolStripMenuItem("Установить Google-карту");
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Сохранить карту");
            ToolStripMenuItem OpenCycleMapMenuItem = new ToolStripMenuItem("Установить OpenCycleMap-карту");

            ToolStripMenuItem ClearMap = new ToolStripMenuItem("Очистить карту");

            // Слой меток для двойного клика
            gmap.Overlays.Add(PositionsForUser);
            gmap.Overlays.Add(PolygonClick);
            gmap.Overlays.Add(RouteClick);

            // Добавление элементов в меню
            contextMenuStrip1.Items.AddRange(new[] { saveMenuItem, YandexMenuItem, GoogleMenuItem, OpenCycleMapMenuItem, ClearMap });

            // Ассоциирование контекстного меню
            gmap.ContextMenuStrip = contextMenuStrip1;

            // Установка обработки событий
            saveMenuItem.Click += saveMenuItem_Click;
            YandexMenuItem.Click += YandexMenuItem_Click;
            GoogleMenuItem.Click += GoogleMenuItem_Click;
            OpenCycleMapMenuItem.Click += OpenCycleMapMenuItem_Click;
            ClearMap.Click += ClearMap_Click;

            // Настройки для компонента GMap
            gmap.Bearing = 0;
            // Перетаскивание правой кнопки мыши
            gmap.CanDragMap = true;
            // Перетаскивание карты левой кнопкой мыши
            gmap.DragButton = MouseButtons.Left;

            gmap.GrayScaleMode = true;

            // Все маркеры будут показаны
            gmap.MarkersEnabled = true;
            // Максимальное приближение
            gmap.MaxZoom = 18;
            // Минимальное приближение
            gmap.MinZoom = 2;
            // Курсор мыши в центр карты
            gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;

            // Отключение негативного режима
            gmap.NegativeMode = false;
            // Разрешение полигонов
            gmap.PolygonsEnabled = true;
            // Разрешение маршрутов
            gmap.RoutesEnabled = true;
            // Скрытие внешней сетки карты
            gmap.ShowTileGridLines = false;
            // При загрузке 10-кратное увеличение
            gmap.Zoom = 10;
            // Убрать красный крестик по центру
            gmap.ShowCenter = false;

            // Чья карта используется
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;

            // Загрузка этой точки на карте
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.Position = new GMap.NET.PointLatLng(43.9151144529437, 42.7288770675659);

            // Создаём новый список маркеров
            GMapOverlay markersOverlay = new GMapOverlay("markers");

            // Установка максимального, минимального и текущего значения элемента управления
            trackBar1.Maximum = 18;
            trackBar1.Minimum = 2;
            trackBar1.Value = (int)gmap.Zoom;
            textBox1.Text = gmap.Zoom.ToString();

            trackBar2.Minimum = 0;
            trackBar2.Maximum = 360;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            gmap.Zoom = trackBar1.Value;
            textBox1.Text = gmap.Zoom.ToString();
        }

        private void загрузитьКоординатыToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            List<CPoint> points = new List<CPoint>();
            // Имя выбранного файла
            string FileName = "";

            // Пользователь выбирает файл на компьютере
            try
            {
                OpenFileDialog OpenDialogorChoiceAFile = new OpenFileDialog();
                // Форматы, которые может выбрать пользователь
                OpenDialogorChoiceAFile.Filter = "Text Files (*.TXT; *.CSV;|*.TXT; *.CSV;|All files(*.*)|*.*";

                // Путь, который перед ним откроется
                OpenDialogorChoiceAFile.InitialDirectory = @"C:\Users\PC\Desktop";
                //ofd.InitialDirectory = @"C:\Users\PC\Desktop";

                OpenDialogorChoiceAFile.Title = "Выбирите файл для подгрузки данных";

                if (OpenDialogorChoiceAFile.ShowDialog() == DialogResult.OK)
                    // Сохранили имя и формат выбранного файла
                    FileName = OpenDialogorChoiceAFile.FileName.ToString();
            }
            catch
            {
                MessageBox.Show("Error");
            }

            // Отсекаем имя и формат от друг друга
            string[] FileNameAndFormat = FileName.Split(new char[] { '.' });

            // Если пользователь выбран текстовый файл
            if (FileNameAndFormat[1] == "txt")
            {
                // Читаем в массив все строки файла
                string[] ArrayOfStringsWithCoor = File.ReadAllLines(FileName, Encoding.GetEncoding(1251));

                // От 0 до количества строк в файле
                for (int i = 0; i < ArrayOfStringsWithCoor.Length; i++)
                {
                    // Парсим строку на поля структуры
                    string[] OneStringWithCoor = ArrayOfStringsWithCoor[i].Split(new char[] { ';' });
                    // Добавляем эту структуру 
                    points.Add(new CPoint(Convert.ToDouble(OneStringWithCoor[0]), Convert.ToDouble(OneStringWithCoor[1])));
                }
                MessageBox.Show("Данные успешно прочитаны");
            }

            // Если пользователь выбрал ксв файл - другого выбора не может быть
            else
            {
                // Читаем все строки
                using (StreamReader ReaderCSCFile = new StreamReader(FileName, Encoding.GetEncoding(1251)))
                {
                    while (!ReaderCSCFile.EndOfStream)
                    {
                        string ArrayOfStringsWithCoor = ReaderCSCFile.ReadLine();
                        string[] OneStringWithCoor = ArrayOfStringsWithCoor.Split(new char[] { ';' });
                        points.Add(new CPoint(Convert.ToDouble(OneStringWithCoor[0]), Convert.ToDouble(OneStringWithCoor[1])));
                    }
                }
                MessageBox.Show("Данные успешно прочитаны");
            }

            // Проверка самого себя - что всё работает
            FileStream fileStream = new FileStream(@"Routes\проверочный.txt", FileMode.Open, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251));

            for (int i = 0; i < points.Count; i++)
                streamWriter.WriteLine(points[i].x + ";" + points[i].y);
            streamWriter.Close();
        }

        public static double GetDouble(string value, double defaultValue)
        {
            double result;

            //Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                //Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }

            return result;
        }

        GMapOverlay ListOfXML = new GMapOverlay("XML");
        List<CPoint> ListWithPointsFromXML = new List<CPoint>();
        private void отобразитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gmap.Overlays.Add(ListOfXML);

            // Создали документ
            XmlDocument xml = new XmlDocument();
            // Открыли его по пути
            xml.Load(@"Routes\3858821316.gpx");
            // Создаем область видимости из xml файла
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("x", "http://www.topografix.com/GPX/1/1");
            XmlNodeList nl = xml.SelectNodes("//x:trkpt", nsmgr);

            // Элементы ХМЛ-документа
            foreach (XmlElement xnode in nl)
            {
                CPoint cPoint = new CPoint();

                cPoint.x = GetDouble(xnode.GetAttribute("lat"), 0);
                cPoint.y = GetDouble(xnode.GetAttribute("lon"), 0);

                // У каждого узла смотрим его поля
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "ele")
                        cPoint.ele = GetDouble(childnode.InnerText, 0);
                }

                ListWithPointsFromXML.Add(cPoint);
            }

            for (int i = 0; i < ListWithPointsFromXML.Count; i++)
            {
                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng
                    (ListWithPointsFromXML[i].x, ListWithPointsFromXML[i].y), GMarkerGoogleType.blue_dot);
                marker.ToolTip = new GMapRoundedToolTip(marker);
                marker.ToolTipText = ListWithPointsFromXML[i].ele.ToString();
                ListOfXML.Markers.Add(marker);
            }
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListOfXML.Clear();
            ListWithPointsFromXML.Clear();
        }

        private void Map_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;

        }

        // Сохранение изображения
        void saveMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialogforsavemap = new SaveFileDialog())
                {
                    // Формат картинки
                    dialogforsavemap.Filter = "PNG (*.png)|*.png";

                    // Название картинки
                    dialogforsavemap.FileName = "Текущее положение карты";

                    Image image = gmap.ToImage();

                    if (image != null)
                    {
                        using (image)
                        {
                            if (dialogforsavemap.ShowDialog() == DialogResult.OK)
                            {
                                string fileName = dialogforsavemap.FileName;
                                if (!fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                                    fileName += ".png";

                                image.Save(fileName);
                                MessageBox.Show("Карта успешно сохранена в директории: " + Environment.NewLine + dialogforsavemap.FileName, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                    }
                }
            }

            // Если ошибка
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка при сохранении карты: " + Environment.NewLine + exception.Message, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        // Смена поставщика карт
        void YandexMenuItem_Click(object sender, EventArgs e)
        {
            gmap.MapProvider = GMapProviders.YandexMap;
            gmap.Zoom = 10;
        }

        void GoogleMenuItem_Click(object sender, EventArgs e)
        {
            gmap.MapProvider = GMapProviders.GoogleMap;
            gmap.Zoom = 10;
        }

        void OpenCycleMapMenuItem_Click(object sender, EventArgs e)
        {
            gmap.MapProvider = GMapProviders.OpenCycleMap;
            gmap.Zoom = 10;
        }

        // Очистка карты
        void ClearMap_Click(object sender, EventArgs e)
        {
            gmap.Overlays.Clear();

            PositionsForUser.Clear();
            ListOfXML.Clear();
            ListWithPointsFromXML.Clear();

            RouteListClick.Clear();
            RouteClick.Clear();
            RouteClick.Routes.Clear();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            gmap.Bearing = trackBar2.Value;
        }


        List<CPoint> RouteListClick = new List<CPoint>();
        List<CPoint> PolygonListClick = new List<CPoint>();
        List<PointLatLng> points = new List<PointLatLng>();
        GMapOverlay PolygonClick = new GMapOverlay("PolygonClick");
        GMapOverlay RouteClick = new GMapOverlay("RouteClick");
        GMapOverlay PositionsForUser = new GMapOverlay("PositionsForUser");
        private void gmap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Долгота - longitude - lng - с запада на восток
                double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
                // Широта - latitude - lat - с севера на юг
                double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;

                MarkerClick = new GeoCoordinate(y, x);

                //textBox2.Text = x.ToString();
                //textBox3.Text = y.ToString();

                // Добавляем метку на слой
                if (checkBox1.Checked == true)
                {
                    points.Clear();
                    GMarkerGoogle MarkerWithMyPosition = new GMarkerGoogle(new PointLatLng(y, x), GMarkerGoogleType.red);
                    MarkerWithMyPosition.ToolTip = new GMapRoundedToolTip(MarkerWithMyPosition);
                    MarkerWithMyPosition.ToolTipText = string.Format("Coordinate: \n Lng: {0} \n Lat: {1}", gmap.FromLocalToLatLng(e.X, e.Y).Lng, gmap.FromLocalToLatLng(e.X, e.Y).Lat);
                    RouteClick.Markers.Add(MarkerWithMyPosition);

                    if (RouteClick.Routes.Count != 0)
                    {
                        RouteClick.Routes.Clear();
                    }

                    RouteListClick.Add(new CPoint(y, x));

                    
                    for (int i = 0; i < RouteListClick.Count; i++)
                        points.Add(new PointLatLng(RouteListClick[i].x, RouteListClick[i].y));

                    GMapRoute routeClick = new GMapRoute(points, "routeClick");
                    routeClick.Stroke = new Pen(Color.Red);
                    RouteClick.Routes.Add(routeClick);
                    

                }
                else if (checkBox2.Checked == true)
                {
                    points.Clear();
                    GMarkerGoogle MarkerWithMyPosition = new GMarkerGoogle(new PointLatLng(y, x), GMarkerGoogleType.blue);
                    MarkerWithMyPosition.ToolTip = new GMapRoundedToolTip(MarkerWithMyPosition);
                    MarkerWithMyPosition.ToolTipText = string.Format("Coordinate: \n Lng: {0} \n Lat: {1}", gmap.FromLocalToLatLng(e.X, e.Y).Lng, gmap.FromLocalToLatLng(e.X, e.Y).Lat);
                    PolygonClick.Markers.Add(MarkerWithMyPosition);

                    // Удаляем полигон, если прокладываем повторно, чтобы не было наложения
                    if (PolygonClick.Polygons.Count != 0)
                    {
                        PolygonClick.Polygons.Clear();
                    }

                    PolygonListClick.Add(new CPoint(y, x));

                    for (int i = 0; i < PolygonListClick.Count; i++)
                        points.Add(new PointLatLng(PolygonListClick[i].x, PolygonListClick[i].y));

                    var polygon = new GMapPolygon(points, "Click");
                    polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                    polygon.Stroke = new Pen(Color.Red);
                    PolygonClick.Polygons.Add(polygon);
                    
                    
                }
                else
                {
                    GMarkerGoogle MarkerWithMyPosition = new GMarkerGoogle(new PointLatLng(y, x), GMarkerGoogleType.orange);
                    MarkerWithMyPosition.ToolTip = new GMapRoundedToolTip(MarkerWithMyPosition);
                    MarkerWithMyPosition.ToolTipText = string.Format("Coordinate: \n Lng: {0} \n Lat: {1}", gmap.FromLocalToLatLng(e.X, e.Y).Lng, gmap.FromLocalToLatLng(e.X, e.Y).Lat);
                    PositionsForUser.Markers.Add(MarkerWithMyPosition);


                }

                flag = true;

                // Сохранение наших координат (текстовик, цсв, бд, текстбокс, строки, лист)
                //FileStream fileStream = new FileStream(@"Date\Координаты_ВыбранныеПользователем.txt", FileMode.Append, FileAccess.Write);
                //StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251));
                //streamWriter.WriteLine(y + ";" + x);
                //streamWriter.Close();
            }
        }

        GeoCoordinate MarkerClick;
        GeoCoordinate MoveCursor;
        bool flag = false;
        private void gmap_MouseMove(object sender, MouseEventArgs e)
        {
            double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;
            double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;

            if (flag == true)
            {
                MoveCursor = new GeoCoordinate(y, x);
                double distance = MarkerClick.GetDistanceTo(MoveCursor);

                distance = Math.Ceiling(distance);
                double km = distance / 1000;

                mStrip.Text = " Distance between marker and cursor = " + distance.ToString() + " m;";
                kmStrip.Text = km.ToString() + " km;";
            }
            else
            {
                mStrip.Text = " Distance between marker and cursor = 0 m;";
                kmStrip.Text = "0 km;";
            }

            LatStrip.Text = "lat = " + Convert.ToString(y) + ";";
            LngStrip.Text = "   lng = " + Convert.ToString(x) + ";";

        }

        public void DistanceBetweenMarkers(GeoCoordinate First, GeoCoordinate Second)
        {
            double distance = First.GetDistanceTo(Second);

            distance = Math.Ceiling(distance);
            double km = distance / 1000;

            textBox4.Text = distance.ToString();
            textBox5.Text = km.ToString();
        }

        bool count = false;
        GeoCoordinate First;
        GeoCoordinate Second;
        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (count == false)
                {
                    textBox2.Text = item.Position.Lat.ToString();
                    textBox3.Text = item.Position.Lng.ToString();
                    First = new GeoCoordinate(item.Position.Lat, item.Position.Lng);
                    count = true;
                    if (textBox6.Text != "" & textBox7.Text != "")
                    {
                        DistanceBetweenMarkers(First, Second);
                    }
                }
                if (count == true & item.Position.Lat.ToString() != textBox2.Text & item.Position.Lng.ToString() != textBox3.Text)
                {
                    textBox6.Text = item.Position.Lat.ToString();
                    textBox7.Text = item.Position.Lng.ToString();

                    double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
                    double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;

                    Second = new GeoCoordinate(y, x);
                    if (textBox2.Text != "" & textBox3.Text != "")
                    {
                        DistanceBetweenMarkers(First, Second);
                    }

                }

            }

            if (e.Button == MouseButtons.Middle)
            {
                if (item.Position.Lat == MarkerClick.Latitude & item.Position.Lng == MarkerClick.Longitude)
                {
                    flag = false;
                }
                if (textBox2.Text == item.Position.Lat.ToString() & textBox3.Text == item.Position.Lng.ToString())
                {
                    textBox2.Text = "";
                    textBox3.Text = "";

                    textBox4.Text = "";
                    textBox5.Text = "";

                    count = false;
                }
                if (textBox6.Text == item.Position.Lat.ToString() & textBox7.Text == item.Position.Lng.ToString())
                {
                    textBox6.Text = "";
                    textBox7.Text = "";

                    textBox4.Text = "";
                    textBox5.Text = "";
                }

                // Узнаем слой удаляемого маркера
                GMapOverlay overlay = item.Overlay;

                if (overlay == RouteClick & overlay.Markers.Count == 0)
                {
                    RouteListClick.Clear();
                    RouteClick.Clear();
                    RouteClick.Routes.Clear();
                }
                else if (overlay == RouteClick & overlay.Markers.Count != 0)
                {
                    for (int i = 0; i < RouteListClick.Count; i++)
                    {
                        if (RouteClick.Markers[i].Equals(item))
                        {
                            RouteListClick.RemoveAt(i);
                            RouteClick.Markers.RemoveAt(i);
                            RouteClick.Routes.Clear();
                            points.Clear();

                            for (int j = 0; j < RouteListClick.Count; j++)
                                points.Add(new PointLatLng(RouteListClick[j].x, RouteListClick[j].y));

                            GMapRoute routeClick = new GMapRoute(points, "routeClick");
                            routeClick.Stroke = new Pen(Color.Red);
                            RouteClick.Routes.Add(routeClick);
                            points.Clear();
                        }
                    }
                }

                if (overlay == PolygonClick & overlay.Markers.Count == 0)
                {
                    PolygonListClick.Clear();
                    PolygonClick.Clear();
                }
                else if (overlay == PolygonClick & overlay.Markers.Count != 0)
                {
                    for (int i = 0; i < PolygonListClick.Count; i++)
                    {
                        if (PolygonClick.Markers[i].Equals(item))
                        {
                            PolygonListClick.RemoveAt(i);
                            PolygonClick.Markers.RemoveAt(i);
                            PolygonClick.Polygons.Clear();
                            points.Clear();

                            for (int j = 0; j < PolygonListClick.Count; j++)
                                points.Add(new PointLatLng(PolygonListClick[j].x, PolygonListClick[j].y));

                            var polygon = new GMapPolygon(points, "Click");
                            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                            polygon.Stroke = new Pen(Color.Red);
                            PolygonClick.Polygons.Add(polygon);
                            points.Clear();
                        }
                    }
                }
                // Удаляем в этом слое этот маркер
                overlay.Markers.Remove(item);
            }
        }




        // Очистка слоя маршрутов
        private void button2_Click(object sender, EventArgs e)
        {
            RouteListClick.Clear();
            RouteClick.Clear();
            RouteClick.Routes.Clear();
            
        }

        // Очистка слоя полигонов
        private void button4_Click(object sender, EventArgs e)
        {
            PolygonListClick.Clear();
            PolygonClick.Markers.Clear();
            PolygonClick.Polygons.Clear();
            
        }

        // Пользовательская очистка
        private void button6_Click(object sender, EventArgs e)
        {
            PositionsForUser.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Enabled = false;
            }
            else
            {
                checkBox2.Enabled = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
            }
        }
    }
}
