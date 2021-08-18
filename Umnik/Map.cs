using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Device.Location;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Globalization;

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
            public CPoint () { }

            public CPoint(double _x, double _y) { x = _x; y = _y; }
            public CPoint(double _x, double _y, double _ele)
            {
                x = _x;
                y = _y;
                ele = _ele;
            }
        }
        #endregion

        public Map()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        GMapOverlay PositionsForUser = new GMapOverlay("PositionsForUser");

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            // Создание элементов меню
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Сохранить карту");
            ToolStripMenuItem YandexMenuItem = new ToolStripMenuItem("Установить Яндекс-карту");
            ToolStripMenuItem GoogleMenuItem = new ToolStripMenuItem("Установить Google-карту");
            ToolStripMenuItem OpenCycleMapMenuItem = new ToolStripMenuItem("Установить OpenCycleMap-карту");

            ToolStripMenuItem ClearMap = new ToolStripMenuItem("Очистить карту");

            // Добавление элементов в меню
            contextMenuStrip1.Items.AddRange(new[] { saveMenuItem,  YandexMenuItem, GoogleMenuItem, OpenCycleMapMenuItem, ClearMap });
            
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

                OpenDialogorChoiceAFile.Title = "Выберите файл для подгрузки данных";

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
            FileStream fileStream = new FileStream(@"Date\w.txt", FileMode.Open, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251));

            for (int i = 0; i < points.Count; i++)
                streamWriter.WriteLine(points[i].x + ";" + points[i].y);
            streamWriter.Close();
        }

        private void gmap_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                gmap.Overlays.Add(PositionsForUser);

                // Долгота - longitude - lng - с запада на восток
                double x = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
                // Широта - latitude - lat - с севера на юг
                double y = gmap.FromLocalToLatLng(e.X, e.Y).Lat;

                textBox2.Text = x.ToString();
                textBox3.Text = y.ToString();

                // Добавляем метку на слой
                GMarkerGoogle MarkerWithMyPosition = new GMarkerGoogle(new PointLatLng(y, x), GMarkerGoogleType.blue_pushpin);
                MarkerWithMyPosition.ToolTip = new GMapRoundedToolTip(MarkerWithMyPosition);
                MarkerWithMyPosition.ToolTipText = string.Format("Coordinate: \n Lng: {0} \n Lat: {1}", gmap.FromLocalToLatLng(e.X, e.Y).Lng, gmap.FromLocalToLatLng(e.X, e.Y).Lat);
                PositionsForUser.Markers.Add(MarkerWithMyPosition);
                
               

                // Сохранение наших координат (текстовик, цсв, бд, текстбокс, строки, лист)
                //FileStream fileStream = new FileStream(@"Date\Координаты_ВыбранныеПользователем.txt", FileMode.Append, FileAccess.Write);
                //StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251));
                //streamWriter.WriteLine(y + ";" + x);
                //streamWriter.Close();
            }
        }

        // Вывод координат маркера нажатием ЛКМ на него в текстбоксы
        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var lat = gmap.FromLocalToLatLng(e.X, e.Y).Lat;
                var lng = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
                textBox2.Text = lat.ToString();
                textBox3.Text = lng.ToString();
            }

            if (e.Button == MouseButtons.Middle)
            {
                // Узнаем слой удаляемого маркера
                GMapOverlay overlay = item.Overlay;
                // Удаляем в этом слое этот маркер
                overlay.Markers.Remove(item);
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
        }
    }
}
