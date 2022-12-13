using System.Text;
using System.Xml;

using GMap.NET;
using GMap.NET.MapProviders;
using System.Globalization;

using static Calc.Program;
using GeoCoordinatePortable;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Drawing;

namespace Umnik
{
    public partial class MapForm : Form
    {

        private Bitmap _dronePicture;
        public MapForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            // Подгружаем в память картинку дрона
            _dronePicture = new Bitmap(@"Icons/uav-mini-black.png");

            // Создание элементов меню
            ToolStripMenuItem googleMenuItem = new ToolStripMenuItem("Установить Google-карту");
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Сохранить карту");
            ToolStripMenuItem openCycleMapMenuItem = new ToolStripMenuItem("Установить OpenCycleMap-карту");

            ToolStripMenuItem clearMap = new ToolStripMenuItem("Очистить карту");

            // Добавление элементов в меню
            contextMenuStrip1.Items.AddRange(new[] { saveMenuItem, googleMenuItem, openCycleMapMenuItem, clearMap });

            // Ассоциирование контекстного меню
            gmap.ContextMenuStrip = contextMenuStrip1;

            // Установка обработки событий
            saveMenuItem.Click += SaveCurrentMapBitmap!;
            googleMenuItem.Click += GoogleMenuItem_Click!;
            openCycleMapMenuItem.Click += OpenCycleMapMenuItem_Click!;
            clearMap.Click += ClearMap_Click!;

            // Настройки для компонента GMap
            gmap.Bearing = 0;
            // Перетаскивание карты
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

            // Чья карта используется по умолчанию
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;

            // Загрузка этой точки на карте
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.Position = new GMap.NET.PointLatLng(43.9151144529437, 42.7288770675659);

            // Работаем с визуалкой(создаем Overlay для дронов)
            gmap.Overlays.Add(dronesOverlay);

            // Добавляем оверлей для маркеров
            gmap.Overlays.Add(markersOverlay); // Для позиций
            gmap.Overlays.Add(routesOverlay); // Для маршрутов
            gmap.Overlays.Add(polygonOverlay); // Для полигонов
            gmap.Overlays.Add(overlayOfXML); // Для XML(GPX) координат
            // Установка максимального, минимального и текущего значения элемента управления
            trackBarMapZoom.Maximum = 18;
            trackBarMapZoom.Minimum = 2;

            trackBarMapZoom.Value = (int)gmap.Zoom;
            txtZoom.Text = gmap.Zoom.ToString();

            trackBarMarkMode.Minimum = 0;
            trackBarMarkMode.Maximum = 360;

            DronesManager.OnAddDroneEvent += OnAddDrone;
            DronesManager.OnRemoveDroneEvent += OnRemoveDrone;
        }

        private void OnAddDrone(Drone drone)
        {
            dronesOverlay.Markers.Add(drone.DroneMarker);
            gmap.Overlays.Add(drone.MarkersOverlay);
            gmap.Overlays.Add(drone.PolygonsOverlay);
            gmap.Overlays.Add(drone.RoutesOverlay);
        }

        private void OnRemoveDrone(Drone drone)
        {
            dronesOverlay.Markers.Remove(drone.DroneMarker);
            gmap.Overlays.Remove(drone.MarkersOverlay);
            gmap.Overlays.Remove(drone.PolygonsOverlay);
            gmap.Overlays.Remove(drone.RoutesOverlay);
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
                    try
                    {
                        using (StreamReader reader = new StreamReader(OPD.FileName, Encoding.UTF8))
                        {

                            while (!reader.EndOfStream)
                            {
                                string[] coordinates = reader.ReadLine().Split(';');
                                points.Add(new CPoint(double.Parse(coordinates[0]), double.Parse(coordinates[1])));
                            }

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


        GMapRoute routesOfXML = new GMapRoute("XML");
        GMapOverlay overlayOfXML = new GMapOverlay();
        List<CPoint> listWithCPointsFromXML = new List<CPoint>();
        List<PointLatLng> listWithPointLatLngXML = new List<PointLatLng>();
        private void ShowMarksOnMap(object sender, EventArgs e)
        {
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
                var y = GetDouble(xnode.GetAttribute("lat"), 0);
                var x = GetDouble(xnode.GetAttribute("lon"), 0);
                CPoint cPoint = new CPoint(y, x);

                // У каждого узла смотрим его поля
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "ele")
                        cPoint.Ele = GetDouble(childnode.InnerText, 0);
                }

                listWithCPointsFromXML.Add(cPoint);
                listWithPointLatLngXML.Add(cPoint.Point);
            }
            routesOfXML = new GMapRoute(listWithPointLatLngXML, "XMLKislovodskRoute");
            overlayOfXML.Routes.Add(routesOfXML);
        }

        // Метод очистки маркеров из XML
        private void ClearMarksFromMap(object sender, EventArgs e)
        {
            overlayOfXML.Clear();
            listWithCPointsFromXML.Clear();
            listWithPointLatLngXML.Clear();
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

                    if (image is not null)
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

            //markersOverlay.Clear();
            //ListOfXML.Clear();
            listWithCPointsFromXML.Clear();

            routeOverlayListClick.Clear();
            //routesOverlay.Clear();
            //routesOverlay.Routes.Clear();

            polygonOverlayListClick.Clear();
            //polygonOverlay.Markers.Clear();
            //polygonOverlay.Polygons.Clear();

            //dronesOverlay.Clear();

            //gmap.Overlays.Remove(ListOfXML); ;
            //gmap.Overlays.Remove(markersOverlay);
            //gmap.Overlays.Remove(routesOverlay);
            //gmap.Overlays.Remove(polygonOverlay);
            //gmap.Overlays.Remove(dronesOverlay);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            gmap.Bearing = trackBarMarkMode.Value;
        }
        List<CPoint> markerOverlayListClick = new List<CPoint>();
        List<CPoint> routeOverlayListClick = new List<CPoint>();
        List<CPoint> polygonOverlayListClick = new List<CPoint>();
        List<PointLatLng> markerPoints = new List<PointLatLng>();
        List<PointLatLng> routePoints = new List<PointLatLng>();
        List<PointLatLng> polygonPoints = new List<PointLatLng>();
        GMapOverlay polygonOverlay = new GMapOverlay("polygonOverlay");
        GMapOverlay routesOverlay = new GMapOverlay("routesOverlay");
        GMapOverlay markersOverlay = new GMapOverlay("markersOverlay");
        GMapOverlay dronesOverlay = new GMapOverlay("dronesOverlay");
        GMarkerGoogleType сolor;
        private void gmap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Добавляем метку на слой
                if (rbRoute.Checked)
                {
                    if (selectedDrone?.RoutesOverlay is not null)
                    {
                        OverlayMouseDoubleClick(e, selectedDrone?.RoutesOverlay!, selectedDrone?.RoutesList!, selectedDrone?.RoutesPointList!);
                    }
                    else
                    {
                        OverlayMouseDoubleClick(e, routesOverlay, routeOverlayListClick, routePoints);
                    }

                }
                else if (rbPolygon.Checked)
                {
                    if (selectedDrone?.PolygonsOverlay is not null)
                    {
                        OverlayMouseDoubleClick(e, selectedDrone?.PolygonsOverlay!, selectedDrone?.PolygonsList!, selectedDrone?.PolygonsPointList!);
                    }
                    else
                    {
                        OverlayMouseDoubleClick(e, polygonOverlay, polygonOverlayListClick, polygonPoints);
                    }
                }
                else if (rbMark.Checked)
                {
                    if (selectedDrone?.MarkersOverlay is not null)
                    {
                        OverlayMouseDoubleClick(e, selectedDrone?.MarkersOverlay!, selectedDrone?.MarkersList!, selectedDrone?.MarkersPointList!);
                    }
                    else
                    {
                        OverlayMouseDoubleClick(e, markersOverlay, markerOverlayListClick, markerPoints);
                    }
                }

                // Сохранение наших координат (текстовик, цсв, бд, текстбокс, строки, лист)
                //FileStream fileStream = new FileStream(@"Date\Координаты_ВыбранныеПользователем.txt", FileMode.Append, FileAccess.Write);
                //StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251));
                //streamWriter.WriteLine(y + ";" + x);
                //streamWriter.Close();
            }
        }

        private void OverlayMouseDoubleClick(MouseEventArgs e, GMapOverlay overlayClick, List<CPoint> overlayListClick, List<PointLatLng> pointList)
        {
            MarkerWithPosition(e, overlayClick, overlayListClick, pointList);

            if (rbRoute.Checked || rbPolygon.Checked)
            {
                // Получаем System.сolor для кисти и карандаша
                Color colorPenAndBrush = GetPenAndBrushColor();

                if (rbRoute.Checked)
                {
                    GMapRoute routeClick = new GMapRoute(pointList, "routesClick");
                    routeClick.Stroke = new Pen(colorPenAndBrush);

                    if (overlayClick.Routes.Count != 0)
                        overlayClick.Routes.Clear();

                    overlayClick.Routes.Add(routeClick);
                }

                if (rbPolygon.Checked)
                {
                    GMapPolygon polygon = new GMapPolygon(pointList, "polygonClick");
                    polygon.Fill = new SolidBrush(System.Drawing.Color.FromArgb(50, colorPenAndBrush));
                    polygon.Stroke = new Pen(colorPenAndBrush);

                    if (overlayClick.Polygons.Count != 0)
                        overlayClick.Polygons.Clear();

                    overlayClick.Polygons.Add(polygon);
                }
            }
        }
        private Color GetPenAndBrushColor()
        {
            if (numericUpDownDrones.Enabled)
            {
                return selectedDrone.SystemColor;
            }
            else return System.Drawing.Color.White;
        }
        private void MarkerWithPosition(MouseEventArgs e, GMapOverlay overlayClick, List<CPoint> overlayListClick, List<PointLatLng> pointList)
        {
            // Долгота - longitude - lng - с запада на восток
            double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
            // Широта - latitude - lat - с севера на юг
            double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;

            MarkerClick = new GeoCoordinate(y, x);

            //pointList.Clear();
            var newPoint = new PointLatLng(y, x);
            // Устанавливаем цвет при выбранном активном дроне / цвет маркера по умолчанию
            GMarkerGoogle MyMarker = new GMarkerGoogle(newPoint, selectedDrone?.MarkerGoogleTypeColour ?? GMarkerGoogleType.white_small);
            MyMarker.ToolTip = new GMapRoundedToolTip(MyMarker);
            MyMarker.ToolTipText = string.Format("Coordinate: \n Lng: {0} \n Lat: {1}", gmap.FromLocalToLatLng(e.X, e.Y).Lng, gmap.FromLocalToLatLng(e.X, e.Y).Lat);
            overlayClick.Markers.Add(MyMarker);

            if (rbRoute.Checked || rbPolygon.Checked)
            {
                overlayListClick.Add(new CPoint(y, x));

                //for (int i = 0; i < overlayListClick.Count; i++)
                //    pointList.Add(overlayListClick[i].Point);
                pointList.Add(newPoint);
            }

            markerPlaced = true;
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
            txtDistanceInKm.Text = km.ToString();
        }

        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Получаем локальные координаты куда нажали
                double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;
                double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
                MarkerClick = new GeoCoordinate(y, x);

                markerPlaced = true;
                if (textBoxesIsNotNull == false)
                {
                    txtLatY1.Text = item.Position.Lat.ToString();
                    txtLngX1.Text = item.Position.Lng.ToString();
                    First = new GeoCoordinate(item.Position.Lat, item.Position.Lng);
                    textBoxesIsNotNull = true;
                    if (txtLatY2.Text != "" && txtLngX2.Text != "")
                    {
                        CalculateDistanceBetweenMarkers(First, Second);
                    }
                }
                if (textBoxesIsNotNull == true &&
                    item.Position.Lat.ToString() != txtLatY1.Text &&
                    item.Position.Lng.ToString() != txtLngX1.Text)
                {
                    txtLatY2.Text = item.Position.Lat.ToString();
                    txtLngX2.Text = item.Position.Lng.ToString();

                    Second = new GeoCoordinate(item.Position.Lat, item.Position.Lng);
                    if (txtLatY1.Text != "" && txtLngX1.Text != "")
                    {
                        CalculateDistanceBetweenMarkers(First, Second);
                    }
                }
            }

            if (e.Button == MouseButtons.Middle)
            {
                if (txtLatY1.Text == item.Position.Lat.ToString() &&
                    txtLngX1.Text == item.Position.Lng.ToString())
                {
                    txtLatY1.Text = "";
                    txtLngX1.Text = "";

                    txtDistanceInMeters.Text = "";
                    txtDistanceInKm.Text = "";

                    textBoxesIsNotNull = false;
                }
                if (txtLatY2.Text == item.Position.Lat.ToString() &&
                    txtLngX2.Text == item.Position.Lng.ToString())
                {
                    txtLatY2.Text = "";
                    txtLngX2.Text = "";

                    txtDistanceInMeters.Text = "";
                    txtDistanceInKm.Text = "";
                }

                // Узнаем слой удаляемого маркера
                GMapOverlay? currentOverlay = item.Overlay;
              
                for (int i = 0; i < currentOverlay.Markers.Count; i++)
                {
                    if (currentOverlay.Markers[i].Equals(item))
                    {
                        markerPlaced = false; // Для текстбоксов
                    }
                }

                // Для удлаения метки принадлежащей дрону
                Drone? drone = null;
                foreach (var uav in DronesManager.Drones)
                {
                    if (uav.Name == currentOverlay.Id)
                    {
                        drone = uav;
                    }
                }
                if (drone is not null)
                {
                    drone?.RemoveMarkerFromOverlay(item, currentOverlay);
                    return;
                }

                // Удаление обычных белых меток, без выбранных дронов
                if (currentOverlay == markersOverlay)
                {
                    DeleteMarkFromLists(item, currentOverlay, markerOverlayListClick, markerPoints);
                }
                else if (currentOverlay == routesOverlay)
                {
                    DeleteMarkFromLists(item, currentOverlay, routeOverlayListClick, routePoints);
                    currentOverlay.Routes.Clear();
                    AddRoutesMark(currentOverlay, routePoints);
                }
                else if (currentOverlay == polygonOverlay)
                {
                    DeleteMarkFromLists(item, currentOverlay, polygonOverlayListClick, polygonPoints);
                    currentOverlay.Polygons.Clear();
                    AddPolygonsMark(currentOverlay, polygonPoints);
                }

                // Удаляем в этом слое маркер
                currentOverlay.Markers.Remove(item);
            }
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
            routeClick.Stroke = new Pen(System.Drawing.Color.White);

            if (overlayClick.Routes.Count != 0)
                overlayClick.Routes.Clear();

            overlayClick.Routes.Add(routeClick);
        }

        private void AddPolygonsMark(GMapOverlay overlayClick, List<PointLatLng> points)
        {
            var polygon = new GMapPolygon(points, "polygonClick");
            polygon.Fill = new SolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.White));
            polygon.Stroke = new Pen(System.Drawing.Color.White);

            if (overlayClick.Polygons.Count != 0)
                overlayClick.Polygons.Clear();

            overlayClick.Polygons.Add(polygon);
        }

        // Очищаем текстбоксы
        private void ClearTextBoxes()
        {
            if (txtLatY1.Text.Length != 0 &&
                txtLngX1.Text.Length != 0 &&
                First.Latitude.ToString() == txtLatY1.Text &&
                First.Longitude.ToString() == txtLngX1.Text)
            {
                txtLatY1.Text = "";
                txtLngX1.Text = "";

                txtDistanceInMeters.Text = "";
                txtDistanceInKm.Text = "";

                textBoxesIsNotNull = false;
            }

            if (txtLatY2.Text.Length != 0 &&
                txtLngX2.Text.Length != 0 &&
                Second.Latitude.ToString() == txtLatY2.Text &&
                Second.Longitude.ToString() == txtLngX2.Text)
            {
                txtLatY2.Text = "";
                txtLngX2.Text = "";

                txtDistanceInMeters.Text = "";
                txtDistanceInKm.Text = "";

                textBoxesIsNotNull = false;
            }
        }

        // Очистка слоя маршрутов
        private void CleanRouteLayer(object sender, EventArgs e)
        {
            routeOverlayListClick.Clear();
            routesOverlay.Clear();
            routesOverlay.Routes.Clear();

            ClearTextBoxes();
        }

        // Очистка слоя полигонов
        private void CleanPolygonLayer(object sender, EventArgs e)
        {
            polygonOverlayListClick.Clear();
            polygonOverlay.Markers.Clear();
            polygonOverlay.Polygons.Clear();

            ClearTextBoxes();
        }

        // Очистка меток 
        private void CleanMarks(object sender, EventArgs e)
        {
            markersOverlay.Clear();

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
                lng1 = Convert.ToDouble(txtLngX1.Text);
                // end point
                lat2 = Convert.ToDouble(txtLatY2.Text);
                lng2 = Convert.ToDouble(txtLngX2.Text);

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
            GMarkerGoogle lastMarker = new GMarkerGoogle(new PointLatLng(), сolor);
            for (int i = 0; i < coords.Count; i++)
            {
                if (i != 0) dronesOverlay.Markers.Remove(lastMarker);
                // Добавляем метку на слой
                GMarkerGoogle myMarker = new GMarkerGoogle(new PointLatLng(coords[i].lat, coords[i].lng), _dronePicture);
                lastMarker = myMarker;

                dronesOverlay.Markers.Add(myMarker);
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


        private void списокДроновToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DronesForm dronesForm = new DronesForm();
            dronesForm.ShowDialog();
        }

        private void MapForm_Activated(object sender, EventArgs e)
        {
            // Если дроны есть то включаем
            if (DronesManager.Drones.Count > 0)
            {
                numericUpDownDrones.Enabled = true;
                numericUpDownDrones.Maximum = DronesManager.Drones.Count - 1;
            }
            //  Выключаем если дронов в списке не осталось
            else if (DronesManager.Drones.Count == 0)
            {
                numericUpDownDrones.Enabled = false;
                numericUpDownDrones.Maximum = 0;

            }

            foreach (var drone in DronesManager.Drones)
            {
                if (drone.Name == $"Drone {numericUpDownDrones.Value}")
                {
                    selectedDrone = drone;
                    return;
                }
            }

            gmap.Refresh();
        }


        private void подключитьсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionForm connectionForm = new ConnectionForm();
            connectionForm.ShowDialog();
        }

        Drone selectedDrone = null;
        private void numericUpDownDrones_ValueChanged(object sender, EventArgs e)
        {
            foreach (var drone in DronesManager.Drones)
            {
                if (drone.Name == $"Drone {numericUpDownDrones.Value}")
                {
                    selectedDrone = drone;
                    return;
                }
            }

        }
    }
}

