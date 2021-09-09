using System;
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

using static Calc.Program;

namespace Umnik
{
    public partial class Map : Form
    {
        //Класс точка - координаты
        #region

        public class CPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Ele { get; set; }
            public CPoint() { }
            public CPoint(double _x, double _y) { X = _x; Y = _y; }
            public CPoint(double _x, double _y, double _ele) { X = _x; Y = _y; Ele = _ele; }

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
            gmap.Overlays.Add(BetweenClick);
            gmap.Overlays.Add(PositionsClick);
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
                streamWriter.WriteLine(points[i].X + ";" + points[i].Y);
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

                cPoint.X = GetDouble(xnode.GetAttribute("lat"), 0);
                cPoint.Y = GetDouble(xnode.GetAttribute("lon"), 0);

                // У каждого узла смотрим его поля
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "ele")
                        cPoint.Ele = GetDouble(childnode.InnerText, 0);
                }

                ListWithPointsFromXML.Add(cPoint);
            }

            for (int i = 0; i < ListWithPointsFromXML.Count; i++)
            {
                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng
                    (ListWithPointsFromXML[i].X, ListWithPointsFromXML[i].Y), GMarkerGoogleType.blue_dot);
                marker.ToolTip = new GMapRoundedToolTip(marker);
                marker.ToolTipText = ListWithPointsFromXML[i].Ele.ToString();
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

            PositionsClick.Clear();
            ListOfXML.Clear();
            ListWithPointsFromXML.Clear();

            RouteListClick.Clear();
            RouteClick.Clear();
            RouteClick.Routes.Clear();

            PolygonListClick.Clear();
            PolygonClick.Markers.Clear();
            PolygonClick.Polygons.Clear();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            gmap.Bearing = trackBar2.Value;
        }

        List<CPoint> RouteListClick = new List<CPoint>();
        List<CPoint> PolygonListClick = new List<CPoint>();
        List<PointLatLng> Points = new List<PointLatLng>();
        GMapOverlay PolygonClick = new GMapOverlay("PolygonClick");
        GMapOverlay RouteClick = new GMapOverlay("RouteClick");
        GMapOverlay PositionsClick = new GMapOverlay("PositionsClick");
        GMapOverlay BetweenClick = new GMapOverlay("BetweenClick");
        GMarkerGoogleType Color;
        private void MarkerWithPosition(MouseEventArgs e, GMapOverlay overlayClick, List<CPoint> overlayListClick = null)
        {
            // Долгота - longitude - lng - с запада на восток
            double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
            // Широта - latitude - lat - с севера на юг
            double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;

            MarkerClick = new GeoCoordinate(y, x);

            //textBox2.Text = x.ToString();
            //textBox3.Text = y.ToString();

            Points.Clear();

            if (radioButton1.Checked)
                Color = GMarkerGoogleType.red;

            if (radioButton2.Checked)
                Color = GMarkerGoogleType.blue;

            if (radioButton3.Checked)
                Color = GMarkerGoogleType.green;

            // Добавляем метку на слой
            GMarkerGoogle MyMarker = new GMarkerGoogle(new PointLatLng(y, x), Color);
            MyMarker.ToolTip = new GMapRoundedToolTip(MyMarker);
            MyMarker.ToolTipText = string.Format("Coordinate: \n Lng: {0} \n Lat: {1}", gmap.FromLocalToLatLng(e.X, e.Y).Lng, gmap.FromLocalToLatLng(e.X, e.Y).Lat);
            overlayClick.Markers.Add(MyMarker);

            if (radioButton1.Checked || radioButton2.Checked)
            {
                overlayListClick.Add(new CPoint(y, x));

                for (int i = 0; i < overlayListClick.Count; i++)
                    Points.Add(new PointLatLng(overlayListClick[i].X, overlayListClick[i].Y));
            }

            markerPlaced = true;
        }
        private void OverlayMouseDoubleClick(MouseEventArgs e, GMapOverlay overlayClick, List<CPoint> overlayListClick = null)
        {
            MarkerWithPosition(e, overlayClick, overlayListClick);

            if (radioButton1.Checked)
            {
                GMapRoute routeClick = new GMapRoute(Points, "routeClick");
                routeClick.Stroke = new Pen(System.Drawing.Color.Red);

                if (overlayClick.Routes.Count != 0)
                    overlayClick.Routes.Clear();

                overlayClick.Routes.Add(routeClick);
            }

            if (radioButton2.Checked)
            {
                var polygon = new GMapPolygon(Points, "Click");
                polygon.Fill = new SolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.Blue));
                polygon.Stroke = new Pen(System.Drawing.Color.Blue);

                if (overlayClick.Polygons.Count != 0)
                    overlayClick.Polygons.Clear();

                overlayClick.Polygons.Add(polygon);
            }
        }
        private void gmap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Добавляем метку на слой
                if (radioButton1.Checked)
                {
                    OverlayMouseDoubleClick(e, RouteClick, RouteListClick);
                }
                else if (radioButton2.Checked)
                {
                    OverlayMouseDoubleClick(e, PolygonClick, PolygonListClick);
                }
                else if (radioButton3.Checked)
                {
                    OverlayMouseDoubleClick(e, PositionsClick);
                }

                // Сохранение наших координат (текстовик, цсв, бд, текстбокс, строки, лист)
                //FileStream fileStream = new FileStream(@"Date\Координаты_ВыбранныеПользователем.txt", FileMode.Append, FileAccess.Write);
                //StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251));
                //streamWriter.WriteLine(y + ";" + x);
                //streamWriter.Close();
            }
        }

        GeoCoordinate MarkerClick;
        GeoCoordinate MoveCursor;
        bool markerPlaced = false;
        private void DistanceMarkerCursor(double y, double x)
        {
            if (markerPlaced == true)
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
        }
        private void gmap_MouseMove(object sender, MouseEventArgs e)
        {
            double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;
            double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;

            DistanceMarkerCursor(y, x);

            LatStrip.Text = "lat = " + Convert.ToString(y) + ";";
            LngStrip.Text = "   lng = " + Convert.ToString(x) + ";";

        }

        bool textBoxesIsNotNull = false;
        GeoCoordinate First;
        GeoCoordinate Second;
        public void DistanceBetweenMarkers(GeoCoordinate firstMarker, GeoCoordinate secondMarker)
        {
            double distance = firstMarker.GetDistanceTo(secondMarker);

            distance = Math.Ceiling(distance);
            double km = distance / 1000;

            textBox4.Text = distance.ToString();
            textBox5.Text = km.ToString();
        }

        private void MiddleMouseDelete(GMapOverlay overlayClick, List<CPoint> overlayListClick)
        {
            for (int i = 0; i < overlayListClick.Count; i++)
                Points.Add(new PointLatLng(overlayListClick[i].X, overlayListClick[i].Y));

            if (overlayClick == RouteClick)
            {
                //Points.Clear();
                GMapRoute routeClick = new GMapRoute(Points, "routeClick");
                routeClick.Stroke = new Pen(System.Drawing.Color.Red);

                if (overlayClick.Routes.Count != 0)
                    overlayClick.Routes.Clear();

                overlayClick.Routes.Add(routeClick);
                Points.Clear();
            }
            if (overlayClick == PolygonClick)
            {
                //Points.Clear();
                var polygon = new GMapPolygon(Points, "Click");
                polygon.Fill = new SolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.Blue));
                polygon.Stroke = new Pen(System.Drawing.Color.Blue);

                if (overlayClick.Polygons.Count != 0)
                    overlayClick.Polygons.Clear();

                overlayClick.Polygons.Add(polygon);
                Points.Clear();
            }
        }
        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;
                double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;

                MarkerClick = new GeoCoordinate(y, x);


                markerPlaced = true;
                if (textBoxesIsNotNull == false)
                {
                    textBox2.Text = item.Position.Lat.ToString();
                    textBox3.Text = item.Position.Lng.ToString();
                    First = new GeoCoordinate(item.Position.Lat, item.Position.Lng);
                    textBoxesIsNotNull = true;
                    if (textBox6.Text != "" && textBox7.Text != "")
                    {
                        DistanceBetweenMarkers(First, Second);
                    }
                }
                if (textBoxesIsNotNull == true &&
                    item.Position.Lat.ToString() != textBox2.Text &&
                    item.Position.Lng.ToString() != textBox3.Text)
                {
                    textBox6.Text = item.Position.Lat.ToString();
                    textBox7.Text = item.Position.Lng.ToString();

                    Second = new GeoCoordinate(y, x);
                    if (textBox2.Text != "" && textBox3.Text != "")
                    {
                        DistanceBetweenMarkers(First, Second);
                    }
                }

            }

            if (e.Button == MouseButtons.Middle)
            {

                if (textBox2.Text == item.Position.Lat.ToString() &&
                    textBox3.Text == item.Position.Lng.ToString())
                {
                    textBox2.Text = "";
                    textBox3.Text = "";

                    textBox4.Text = "";
                    textBox5.Text = "";

                    textBoxesIsNotNull = false;
                }
                if (textBox6.Text == item.Position.Lat.ToString() &&
                    textBox7.Text == item.Position.Lng.ToString())
                {
                    textBox6.Text = "";
                    textBox7.Text = "";

                    textBox4.Text = "";
                    textBox5.Text = "";
                }

                // Узнаем слой удаляемого маркера
                GMapOverlay overlay = item.Overlay;
                for (int i = 0; i < overlay.Markers.Count; i++)
                {
                    if (overlay.Markers[i].Equals(item))
                    {
                        markerPlaced = false;
                    }
                }
                if (overlay == RouteClick && overlay.Markers.Count == 0)
                {
                    RouteListClick.Clear();
                    RouteClick.Clear();
                    RouteClick.Routes.Clear();
                }
                else if (overlay == RouteClick && overlay.Markers.Count != 0)
                {
                    for (int i = 0; i < RouteListClick.Count; i++)
                    {
                        if (RouteClick.Markers[i].Equals(item))
                        {
                            RouteListClick.RemoveAt(i);
                            RouteClick.Markers.RemoveAt(i);
                            RouteClick.Routes.Clear();
                            Points.Clear();

                            MiddleMouseDelete(RouteClick, RouteListClick);
                        }
                    }
                }

                if (overlay == PolygonClick && overlay.Markers.Count == 0)
                {
                    PolygonListClick.Clear();
                    PolygonClick.Clear();
                    PolygonClick.Polygons.Clear();
                }
                else if (overlay == PolygonClick && overlay.Markers.Count != 0)
                {
                    for (int i = 0; i < PolygonListClick.Count; i++)
                    {
                        if (PolygonClick.Markers[i].Equals(item))
                        {
                            PolygonListClick.RemoveAt(i);
                            PolygonClick.Markers.RemoveAt(i);
                            PolygonClick.Polygons.Clear();
                            Points.Clear();

                            MiddleMouseDelete(PolygonClick, PolygonListClick);
                        }
                    }
                }
                // Удаляем в этом слое этот маркер
                overlay.Markers.Remove(item);
            }
        }

        private void ClearTextBoxes()
        {
            if (textBox2.Text.Length != 0 &&
              textBox3.Text.Length != 0 &&
             First.Latitude.ToString() == textBox2.Text &&
             First.Longitude.ToString() == textBox3.Text)
            {
                textBox2.Text = "";
                textBox3.Text = "";

                textBox4.Text = "";
                textBox5.Text = "";

                textBoxesIsNotNull = false;
            }
            if (textBox6.Text.Length != 0 &&
              textBox7.Text.Length != 0 &&
             Second.Latitude.ToString() == textBox6.Text &&
             Second.Longitude.ToString() == textBox7.Text)
            {
                textBox6.Text = "";
                textBox7.Text = "";

                textBox4.Text = "";
                textBox5.Text = "";

                textBoxesIsNotNull = false;
            }



            //if (textBox2.Text.Length != 0 &&
            //  textBox3.Text.Length != 0 &&
            //  textBox4.Text.Length != 0 &&
            //  textBox5.Text.Length != 0 &&
            //  textBox6.Text.Length != 0 &&
            //  textBox7.Text.Length != 0)
            //{
            //    textBox2.Text = "";
            //    textBox3.Text = "";

            //    textBox4.Text = "";
            //    textBox5.Text = "";

            //    textBox6.Text = "";
            //    textBox7.Text = "";

            //    textBoxesIsNotNull = false;

        }
        // Очистка слоя маршрутов
        private void button2_Click(object sender, EventArgs e)
        {
            RouteListClick.Clear();
            RouteClick.Clear();
            RouteClick.Routes.Clear();

            ClearTextBoxes();
        }

        // Очистка слоя полигонов
        private void button4_Click(object sender, EventArgs e)
        {
            PolygonListClick.Clear();
            PolygonClick.Markers.Clear();
            PolygonClick.Polygons.Clear();

            ClearTextBoxes();
        }

        // Пользовательская очистка
        private void button6_Click(object sender, EventArgs e)
        {
            PositionsClick.Clear();

            ClearTextBoxes();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // point interval in meters
            double interval = 1;
            // direction of line in degrees
            //start point
            double lat1 = Convert.ToDouble(textBox2.Text);
            double lng1 = Convert.ToDouble(textBox3.Text);
            // end point
            double lat2 = Convert.ToDouble(textBox6.Text);
            double lng2 = Convert.ToDouble(textBox7.Text);

            MockLocation start = new MockLocation(lat1, lng1);
            MockLocation end = new MockLocation(lat2, lng2);
            double azimuth = calculateBearing(start, end);
            List<MockLocation> coords = getLocations(interval, azimuth, start, end);

            for (int i = 0; i < coords.Count; i++)
            {
                // Добавляем метку на слой
                GMarkerGoogle MyMarker = new GMarkerGoogle(new PointLatLng(coords[i].lat, coords[i].lng), new Bitmap(@"Icons/uav-mini.png"));
                BetweenClick.Markers.Clear();
                BetweenClick.Markers.Add(MyMarker);
                gmap.Refresh();
            }
        }
    }
}
