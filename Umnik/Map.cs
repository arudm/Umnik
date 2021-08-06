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
using System.Device.Location;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;



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

            public CPoint(double _x, double _y) { x = _x; y = _y; }
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
            gmap.Position = new GMap.NET.PointLatLng(47.2226466, 38.8391624);

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

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Выход из программы");
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //Инициализируем новую переменную класса SaveFileDialog,
                //открывающий диалоговое окно для сохранения файла. 
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    //Задаем текущую строку фильтра имен файлов,
                    //которая определяет варианты, доступные в поле 
                    //"Сохранить как тип файла" или "Файлы типа"
                    //диалогового окна.                    
                    dialog.Filter = "PNG (*.png)|*.png";

                    //Задаем строку, содержащую имя файла,
                    //выбранное в диалоговом  окне файла.
                    dialog.FileName = "GMap.NET image";

                    //Создаем новое изображение и
                    //передаем компонент с картой.
                    Image image = this.gmap.ToImage();

                    if (image != null)
                    {
                        using (image)
                        {
                            //Запускаем общее диалоговое окно с
                            //заданным по умолчанию владельцем.                          
                            //Данное окно возвращает объект
                            //System.Windows.Forms.DialogResult.OK,
                            //если пользователь нажимает кнопку
                            //ОК в диалоговом окне; в противном случае 
                            //— объект System.Windows.Forms.DialogResult.Cancel.
                            //Если пользователь нажал ОК, то идем дальше.
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                //Заносим в переменную имя файла введенное 
                                //в диалоговом окне.
                                string fileName = dialog.FileName;

                                //Выполняем проверку:
                                //был ли задан формат изображения карты,
                                //если нет, то добавляем после имени
                                //расширение файла.
                                if (!fileName.EndsWith(".png",
                                    StringComparison.OrdinalIgnoreCase))
                                {
                                    fileName += ".png";
                                }
                                //Выполняем сохранение изображения карты.
                                image.Save(fileName);

                                //Выводим сообщение об успешном сохранении 
                                //и пути к данному изображению карты.
                                MessageBox.Show("Карта успешно сохранена в директории: "
                                    + Environment.NewLine
                                    + dialog.FileName, "GMap.NET",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Asterisk);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //Если на одном из этапов сохранения произошла ошибка 
                MessageBox.Show("Ошибка при сохранении карты: "
                    + Environment.NewLine
                    + exception.Message,
                    "GMap.NET",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
            }
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
                MarkerWithMyPosition.ToolTipText = string.Format("Koordinate: \n Lng: {0} \n Lat: {1}", gmap.FromLocalToLatLng(e.X, e.Y).Lng, gmap.FromLocalToLatLng(e.X, e.Y).Lat);
                PositionsForUser.Markers.Add(MarkerWithMyPosition);
                
               

                // Сохранение наших координат (текстовик, цсв, бд, текстбокс, строки, лист)
                //FileStream fileStream = new FileStream(@"Date\Координаты_ВыбранныеПользователем.txt", FileMode.Append, FileAccess.Write);
                //StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251));
                //streamWriter.WriteLine(y + ";" + x);
                //streamWriter.Close();
            }
        }

        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PositionsForUser.Markers.Remove(item);
            }
        }
    }
}
