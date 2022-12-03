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
using System.Threading.Tasks;

namespace Umnik
{
    public partial class MapForm : Form
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

        private Bitmap _dronePicture;
        public MapForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            // Подгружаем в память картинку дрона
            _dronePicture = new Bitmap(@"Icons/uav-mini.png");

            // Создание элементов меню
            ToolStripMenuItem YandexMenuItem = new ToolStripMenuItem("Установить Яндекс-карту");
            ToolStripMenuItem GoogleMenuItem = new ToolStripMenuItem("Установить Google-карту");
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Сохранить карту");
            ToolStripMenuItem OpenCycleMapMenuItem = new ToolStripMenuItem("Установить OpenCycleMap-карту");

            ToolStripMenuItem ClearMap = new ToolStripMenuItem("Очистить карту");

            // Добавление элементов в меню
            contextMenuStrip1.Items.AddRange(new[] { saveMenuItem, YandexMenuItem, GoogleMenuItem, OpenCycleMapMenuItem, ClearMap });

            // Ассоциирование контекстного меню
            gmap.ContextMenuStrip = contextMenuStrip1;

            // Установка обработки событий
            saveMenuItem.Click += SaveCurrentMapBitmap;
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

            // Работаем с визуалкой(создаем Overlay для дронов)
            gmap.Overlays.Add(DronesOverlay);

            // Установка максимального, минимального и текущего значения элемента управления
            trackBarMapZoom.Maximum = 18;
            trackBarMapZoom.Minimum = 2;
            trackBarMapZoom.Value = (int)gmap.Zoom;
            txtZoom.Text = gmap.Zoom.ToString();

            trackBarMarkMode.Minimum = 0;
            trackBarMarkMode.Maximum = 360;
        }

        private void ChangeMapZoom(object sender, EventArgs e)
        {
            gmap.Zoom = trackBarMapZoom.Value;
            txtZoom.Text = gmap.Zoom.ToString();
        }

        private void LoadCoordinatesFromFile(object sender, EventArgs e)
        {
            List<CPoint> points = new List<CPoint>();
            // Пользователь выбирает файл на компьютере

            using (OpenFileDialog OPD = new OpenFileDialog())
            {
                OPD.Title = "Выберите файл для подгрузки данных";
                // Форматы, которые может выбрать пользователь
                OPD.Filter = "Text Files (*.TXT; *.CSV;|*.TXT; *.CSV;|All files(*.*)|*.*";

                // Путь, который перед ним откроется
                OPD.InitialDirectory = @"C:\Users\PC\Desktop";

                if (OPD.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader reader = new StreamReader(OPD.FileName, Encoding.GetEncoding(1251)))
                    {
                        try
                        {
                            while (!reader.EndOfStream)
                            {
                                string[] coordinates = reader.ReadLine().Split(';');
                                points.Add(new CPoint(double.Parse(coordinates[0]), double.Parse(coordinates[1])));
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка чтения текстового файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        MessageBox.Show("Данные успешно прочитаны");
                    }
                }
            }

            // Пока пусть закоментированно будет
            //// Проверка самого себя - что всё работает
            //FileStream fileStream = new FileStream(@"Routes\проверочный.txt", FileMode.Open, FileAccess.Write);
            //StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251));

            //for (int i = 0; i < points.Count; i++)
            //    streamWriter.WriteLine(points[i].X + ";" + points[i].Y);
            //streamWriter.Close();
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
        private void ShowMarksOnMap(object sender, EventArgs e)
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

        private void ClearMarksFromMap(object sender, EventArgs e)
        {
            ListOfXML.Clear();
            ListWithPointsFromXML.Clear();
            gmap.Overlays.Remove(ListOfXML); ;
        }

        private void Map_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show("Вы действительно хотите выйти?",
                                       "Выход",
                                       MessageBoxButtons.YesNo) != DialogResult.Yes;
        }

        // Сохранение изображения
        void SaveCurrentMapBitmap(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog SFD = new SaveFileDialog())
                {
                    // Формат картинки
                    SFD.Filter = "PNG (*.png)|*.png";
                    SFD.DefaultExt = "png";
                    // Название картинки
                    SFD.FileName = "Текущее положение карты";

                    Image image = gmap.ToImage();

                    if (image != null)
                    {
                        using (image)
                        {
                            if (SFD.ShowDialog() == DialogResult.OK)
                            {
                                image.Save(SFD.FileName);
                                MessageBox.Show("Карта успешно сохранена в директории: " + Environment.NewLine + SFD.FileName, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

            //PositionsClick.Clear();
            //ListOfXML.Clear();
            ListWithPointsFromXML.Clear();

            RouteListClick.Clear();
            //RouteClick.Clear();
            //RouteClick.Routes.Clear();

            PolygonListClick.Clear();
            //PolygonClick.Markers.Clear();
            //PolygonClick.Polygons.Clear();

            //DronesOverlay.Clear();

            //gmap.Overlays.Remove(ListOfXML); ;
            //gmap.Overlays.Remove(PositionsClick);
            //gmap.Overlays.Remove(RouteClick);
            //gmap.Overlays.Remove(PolygonClick);
            //gmap.Overlays.Remove(DronesOverlay);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            gmap.Bearing = trackBarMarkMode.Value;
        }

        List<CPoint> RouteListClick = new List<CPoint>();
        List<CPoint> PolygonListClick = new List<CPoint>();
        List<PointLatLng> Points = new List<PointLatLng>();
        GMapOverlay PolygonClick = new GMapOverlay("PolygonClick");
        GMapOverlay RouteClick = new GMapOverlay("RouteClick");
        GMapOverlay PositionsClick = new GMapOverlay("PositionsClick");
        GMapOverlay DronesOverlay = new GMapOverlay("DronesOverlay");
        GMapOverlay Overlay = new GMapOverlay("DronesOverlay");
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

            if (rbRoute.Checked)
                Color = GMarkerGoogleType.red;

            if (rbPolygon.Checked)
                Color = GMarkerGoogleType.blue;

            if (rbMark.Checked)
                Color = GMarkerGoogleType.green;

            // Добавляем метку на слой
            GMarkerGoogle MyMarker = new GMarkerGoogle(new PointLatLng(y, x), Color);
            MyMarker.ToolTip = new GMapRoundedToolTip(MyMarker);
            MyMarker.ToolTipText = string.Format("Coordinate: \n Lng: {0} \n Lat: {1}", gmap.FromLocalToLatLng(e.X, e.Y).Lng, gmap.FromLocalToLatLng(e.X, e.Y).Lat);
            overlayClick.Markers.Add(MyMarker);

            if (rbRoute.Checked || rbPolygon.Checked)
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

            if (rbRoute.Checked)
            {
                GMapRoute routeClick = new GMapRoute(Points, "routeClick");
                routeClick.Stroke = new Pen(System.Drawing.Color.Red);

                if (overlayClick.Routes.Count != 0)
                    overlayClick.Routes.Clear();

                overlayClick.Routes.Add(routeClick);
            }

            if (rbPolygon.Checked)
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
                if (rbRoute.Checked)
                {
                    gmap.Overlays.Add(RouteClick);
                    OverlayMouseDoubleClick(e, RouteClick, RouteListClick);
                }
                else if (rbPolygon.Checked)
                {
                    gmap.Overlays.Add(PolygonClick);
                    OverlayMouseDoubleClick(e, PolygonClick, PolygonListClick);
                }
                else if (rbMark.Checked)
                {
                    gmap.Overlays.Add(PositionsClick);
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
        public void CalculateDistanceBetweenMarkers(GeoCoordinate firstMarker, GeoCoordinate secondMarker)
        {
            double distance = firstMarker.GetDistanceTo(secondMarker);

            distance = Math.Ceiling(distance);
            double km = distance / 1000;

            txtDistanceInMeters.Text = distance.ToString();
            DistanceInKm.Text = km.ToString();
        }

        private void DeleteMark(GMapOverlay overlayClick, List<CPoint> overlayListClick)
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
                    txtLatY1.Text = item.Position.Lat.ToString();
                    LngX1.Text = item.Position.Lng.ToString();
                    First = new GeoCoordinate(item.Position.Lat, item.Position.Lng);
                    textBoxesIsNotNull = true;
                    if (txtLatY2.Text != "" && LngX2.Text != "")
                    {
                        CalculateDistanceBetweenMarkers(First, Second);
                    }
                }
                if (textBoxesIsNotNull == true &&
                    item.Position.Lat.ToString() != txtLatY1.Text &&
                    item.Position.Lng.ToString() != LngX1.Text)
                {
                    txtLatY2.Text = item.Position.Lat.ToString();
                    LngX2.Text = item.Position.Lng.ToString();

                    Second = new GeoCoordinate(y, x);
                    if (txtLatY1.Text != "" && LngX1.Text != "")
                    {
                        CalculateDistanceBetweenMarkers(First, Second);
                    }
                }
            }

            if (e.Button == MouseButtons.Middle)
            {
                if (txtLatY1.Text == item.Position.Lat.ToString() &&
                    LngX1.Text == item.Position.Lng.ToString())
                {
                    txtLatY1.Text = "";
                    LngX1.Text = "";

                    txtDistanceInMeters.Text = "";
                    DistanceInKm.Text = "";

                    textBoxesIsNotNull = false;
                }
                if (txtLatY2.Text == item.Position.Lat.ToString() &&
                    LngX2.Text == item.Position.Lng.ToString())
                {
                    txtLatY2.Text = "";
                    LngX2.Text = "";

                    txtDistanceInMeters.Text = "";
                    DistanceInKm.Text = "";
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

                            DeleteMark(RouteClick, RouteListClick);
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

                            DeleteMark(PolygonClick, PolygonListClick);
                        }
                    }
                }
                // Удаляем в этом слое этот маркер
                overlay.Markers.Remove(item);
            }
        }

        private void ClearTextBoxes()
        {
            if (txtLatY1.Text.Length != 0 &&
                LngX1.Text.Length != 0 &&
                First.Latitude.ToString() == txtLatY1.Text &&
                First.Longitude.ToString() == LngX1.Text)
            {
                txtLatY1.Text = "";
                LngX1.Text = "";

                txtDistanceInMeters.Text = "";
                DistanceInKm.Text = "";

                textBoxesIsNotNull = false;
            }

            if (txtLatY2.Text.Length != 0 &&
                LngX2.Text.Length != 0 &&
                Second.Latitude.ToString() == txtLatY2.Text &&
                Second.Longitude.ToString() == LngX2.Text)
            {
                txtLatY2.Text = "";
                LngX2.Text = "";

                txtDistanceInMeters.Text = "";
                DistanceInKm.Text = "";

                textBoxesIsNotNull = false;
            }
        }
        // Очистка слоя маршрутов
        private void CleanRouteLayer(object sender, EventArgs e)
        {
            RouteListClick.Clear();
            RouteClick.Clear();
            RouteClick.Routes.Clear();

            gmap.Overlays.Remove(RouteClick);

            ClearTextBoxes();
        }

        // Очистка слоя полигонов
        private void CleanPolygonLayer(object sender, EventArgs e)
        {
            PolygonListClick.Clear();
            PolygonClick.Markers.Clear();
            PolygonClick.Polygons.Clear();

            gmap.Overlays.Remove(PolygonClick);

            ClearTextBoxes();
        }

        // Очистка меток 
        private void CleanMarks(object sender, EventArgs e)
        {
            PositionsClick.Clear();

            gmap.Overlays.Remove(PositionsClick);

            ClearTextBoxes();
        }

        private async void StartFlight(object sender, EventArgs e)
        {
            btnStartFlight.Enabled = false;
            grpMarksMode.Enabled = false;
            double lat1,
                   lng1,
                   lat2,
                   lng2;

            List<MockLocation> coords = new List<MockLocation>();

            try
            {
                // Подготавливаем координаты
                double interval = 1;

                // direction of line in degrees
                //start point
                lat1 = Convert.ToDouble(txtLatY1.Text);
                lng1 = Convert.ToDouble(LngX1.Text);
                // end point
                lat2 = Convert.ToDouble(txtLatY2.Text);
                lng2 = Convert.ToDouble(LngX2.Text);

                MockLocation start = new MockLocation(lat1, lng1);
                MockLocation end = new MockLocation(lat2, lng2);
                double azimuth = calculateBearing(start, end);
                coords = getLocations(interval, azimuth, start, end);
            }
            catch
            {
                MessageBox.Show("Ошибка преобразования координат! Проверьте заполнение полей с координатами и повторите попытку!",
                                 "Ошибка!",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                btnStartFlight.Enabled = true;
                grpMarksMode.Enabled = true;
            }
            GMarkerGoogle lastMarker = new GMarkerGoogle(new PointLatLng(), Color);
            for (int i = 0; i < coords.Count; i++)
            {
                if (i != 0) DronesOverlay.Markers.Remove(lastMarker);
                // Добавляем метку на слой
                GMarkerGoogle myMarker = new GMarkerGoogle(new PointLatLng(coords[i].lat, coords[i].lng), _dronePicture);
                lastMarker = myMarker;

                DronesOverlay.Markers.Add(myMarker);
                gmap.Refresh();
                await Task.Delay(1);
            }
            btnStartFlight.Enabled = true;
            grpMarksMode.Enabled = true;
        }

        private void gmap_OnMapZoomChanged()
        {
            trackBarMapZoom.Value = (int)gmap.Zoom;
            txtZoom.Text = Convert.ToString((int)gmap.Zoom);
        }

        List<UAV> listOfUAVs = new List<UAV>();
        private void списокДроновToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DronesForm dronesForm = new DronesForm(ref listOfUAVs);
            dronesForm.ShowDialog();
        }

        private void MapForm_Activated(object sender, EventArgs e)
        {
            DronesOverlay.Clear();
            foreach (UAV item in listOfUAVs)
            {
                // Добавляем метку на слой
                GMarkerGoogle MyMarker = new GMarkerGoogle(new PointLatLng(item.Coordinates.Lat, item.Coordinates.Lng), item.Icon);
                //DronesOverlay.Markers.Clear();
                DronesOverlay.Markers.Add(MyMarker);
            }
            gmap.Refresh();
        }
    }
}

