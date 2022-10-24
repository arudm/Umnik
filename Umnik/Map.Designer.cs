
namespace Umnik
{
    partial class Map
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.trackBarMapZoom = new System.Windows.Forms.TrackBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtLatY1 = new System.Windows.Forms.TextBox();
            this.LngX1 = new System.Windows.Forms.TextBox();
            this.msMainFunctions = new System.Windows.Forms.MenuStrip();
            this.miLoadCoordinates = new System.Windows.Forms.ToolStripMenuItem();
            this.miXML_GPX = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowXML_GPX = new System.Windows.Forms.ToolStripMenuItem();
            this.miCleanXML_GPX = new System.Windows.Forms.ToolStripMenuItem();
            this.lblMarkCoord1Desc = new System.Windows.Forms.Label();
            this.lblLngX1Desc = new System.Windows.Forms.Label();
            this.lblLatY1Desc = new System.Windows.Forms.Label();
            this.ssCoordinates = new System.Windows.Forms.StatusStrip();
            this.LatStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.LngStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.mStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.kmStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.trackBarMarkMode = new System.Windows.Forms.TrackBar();
            this.txtDistanceInMeters = new System.Windows.Forms.TextBox();
            this.DistanceInKm = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblMetersDesc = new System.Windows.Forms.Label();
            this.lblKilometersDesc = new System.Windows.Forms.Label();
            this.txtLatY2 = new System.Windows.Forms.TextBox();
            this.LngX2 = new System.Windows.Forms.TextBox();
            this.lblMarkCoord2Desc = new System.Windows.Forms.Label();
            this.lblLngX2Desc = new System.Windows.Forms.Label();
            this.lblLatY2Desc = new System.Windows.Forms.Label();
            this.lblMarkDistanceDesc = new System.Windows.Forms.Label();
            this.btnCleanRoute = new System.Windows.Forms.Button();
            this.btnCleanPoligon = new System.Windows.Forms.Button();
            this.btnCleanMark = new System.Windows.Forms.Button();
            this.rbRoute = new System.Windows.Forms.RadioButton();
            this.rbPolygon = new System.Windows.Forms.RadioButton();
            this.grpMarksMode = new System.Windows.Forms.GroupBox();
            this.rbMark = new System.Windows.Forms.RadioButton();
            this.btnStartFlight = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapZoom)).BeginInit();
            this.msMainFunctions.SuspendLayout();
            this.ssCoordinates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMarkMode)).BeginInit();
            this.grpMarksMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gmap
            // 
            this.gmap.Bearing = 0F;
            this.gmap.CanDragMap = true;
            this.gmap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap.GrayScaleMode = false;
            this.gmap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap.LevelsKeepInMemmory = 5;
            this.gmap.Location = new System.Drawing.Point(12, 27);
            this.gmap.MarkersEnabled = true;
            this.gmap.MaxZoom = 2;
            this.gmap.MinZoom = 2;
            this.gmap.MouseWheelZoomEnabled = true;
            this.gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmap.Name = "gmap";
            this.gmap.NegativeMode = false;
            this.gmap.PolygonsEnabled = true;
            this.gmap.RetryLoadTile = 0;
            this.gmap.RoutesEnabled = true;
            this.gmap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap.ShowTileGridLines = false;
            this.gmap.Size = new System.Drawing.Size(1061, 467);
            this.gmap.TabIndex = 0;
            this.gmap.Zoom = 0D;
            this.gmap.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.gmap_OnMarkerClick);
            this.gmap.Load += new System.EventHandler(this.gMapControl1_Load);
            this.gmap.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gmap_MouseDoubleClick);
            this.gmap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gmap_MouseMove);
            // 
            // trackBarMapZoom
            // 
            this.trackBarMapZoom.Location = new System.Drawing.Point(1096, 53);
            this.trackBarMapZoom.Name = "trackBarMapZoom";
            this.trackBarMapZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMapZoom.Size = new System.Drawing.Size(45, 441);
            this.trackBarMapZoom.TabIndex = 3;
            this.trackBarMapZoom.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1096, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(33, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "Zoom";
            // 
            // txtLatY1
            // 
            this.txtLatY1.Location = new System.Drawing.Point(49, 519);
            this.txtLatY1.Name = "txtLatY1";
            this.txtLatY1.ReadOnly = true;
            this.txtLatY1.Size = new System.Drawing.Size(100, 20);
            this.txtLatY1.TabIndex = 5;
            // 
            // LngX1
            // 
            this.LngX1.Location = new System.Drawing.Point(49, 545);
            this.LngX1.Name = "LngX1";
            this.LngX1.ReadOnly = true;
            this.LngX1.Size = new System.Drawing.Size(100, 20);
            this.LngX1.TabIndex = 6;
            // 
            // msMainFunctions
            // 
            this.msMainFunctions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLoadCoordinates,
            this.miXML_GPX});
            this.msMainFunctions.Location = new System.Drawing.Point(0, 0);
            this.msMainFunctions.Name = "msMainFunctions";
            this.msMainFunctions.Size = new System.Drawing.Size(1161, 24);
            this.msMainFunctions.TabIndex = 7;
            this.msMainFunctions.Text = "msMainFunctions";
            // 
            // miLoadCoordinates
            // 
            this.miLoadCoordinates.Name = "miLoadCoordinates";
            this.miLoadCoordinates.Size = new System.Drawing.Size(143, 20);
            this.miLoadCoordinates.Text = "Загрузить координаты";
            this.miLoadCoordinates.Click += new System.EventHandler(this.загрузитьКоординатыToolStripMenuItem_Click_1);
            // 
            // miXML_GPX
            // 
            this.miXML_GPX.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miShowXML_GPX,
            this.miCleanXML_GPX});
            this.miXML_GPX.Name = "miXML_GPX";
            this.miXML_GPX.Size = new System.Drawing.Size(76, 20);
            this.miXML_GPX.Text = "XML (GPX)";
            // 
            // miShowXML_GPX
            // 
            this.miShowXML_GPX.Name = "miShowXML_GPX";
            this.miShowXML_GPX.Size = new System.Drawing.Size(138, 22);
            this.miShowXML_GPX.Text = "Отобразить";
            this.miShowXML_GPX.Click += new System.EventHandler(this.отобразитьToolStripMenuItem_Click);
            // 
            // miCleanXML_GPX
            // 
            this.miCleanXML_GPX.Name = "miCleanXML_GPX";
            this.miCleanXML_GPX.Size = new System.Drawing.Size(138, 22);
            this.miCleanXML_GPX.Text = "Очистить";
            this.miCleanXML_GPX.Click += new System.EventHandler(this.очиститьToolStripMenuItem_Click);
            // 
            // lblMarkCoord1Desc
            // 
            this.lblMarkCoord1Desc.AutoSize = true;
            this.lblMarkCoord1Desc.Location = new System.Drawing.Point(10, 503);
            this.lblMarkCoord1Desc.Name = "lblMarkCoord1Desc";
            this.lblMarkCoord1Desc.Size = new System.Drawing.Size(112, 13);
            this.lblMarkCoord1Desc.TabIndex = 8;
            this.lblMarkCoord1Desc.Text = "Координаты метки 1";
            // 
            // lblLngX1Desc
            // 
            this.lblLngX1Desc.AutoSize = true;
            this.lblLngX1Desc.Location = new System.Drawing.Point(7, 545);
            this.lblLngX1Desc.Name = "lblLngX1Desc";
            this.lblLngX1Desc.Size = new System.Drawing.Size(36, 13);
            this.lblLngX1Desc.TabIndex = 9;
            this.lblLngX1Desc.Text = "Lng x:";
            // 
            // lblLatY1Desc
            // 
            this.lblLatY1Desc.AutoSize = true;
            this.lblLatY1Desc.Location = new System.Drawing.Point(7, 522);
            this.lblLatY1Desc.Name = "lblLatY1Desc";
            this.lblLatY1Desc.Size = new System.Drawing.Size(33, 13);
            this.lblLatY1Desc.TabIndex = 10;
            this.lblLatY1Desc.Text = "Lat y:";
            // 
            // ssCoordinates
            // 
            this.ssCoordinates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LatStrip,
            this.LngStrip,
            this.mStrip,
            this.kmStrip});
            this.ssCoordinates.Location = new System.Drawing.Point(0, 596);
            this.ssCoordinates.Name = "ssCoordinates";
            this.ssCoordinates.Size = new System.Drawing.Size(1161, 22);
            this.ssCoordinates.TabIndex = 12;
            // 
            // LatStrip
            // 
            this.LatStrip.Name = "LatStrip";
            this.LatStrip.Size = new System.Drawing.Size(44, 17);
            this.LatStrip.Text = "latStrip";
            // 
            // LngStrip
            // 
            this.LngStrip.Name = "LngStrip";
            this.LngStrip.Size = new System.Drawing.Size(48, 17);
            this.LngStrip.Text = "lngStrip";
            // 
            // mStrip
            // 
            this.mStrip.Name = "mStrip";
            this.mStrip.Size = new System.Drawing.Size(42, 17);
            this.mStrip.Text = "mStrip";
            // 
            // kmStrip
            // 
            this.kmStrip.Name = "kmStrip";
            this.kmStrip.Size = new System.Drawing.Size(48, 17);
            this.kmStrip.Text = "kmStrip";
            // 
            // trackBarMarkMode
            // 
            this.trackBarMarkMode.Location = new System.Drawing.Point(533, 492);
            this.trackBarMarkMode.Name = "trackBarMarkMode";
            this.trackBarMarkMode.Size = new System.Drawing.Size(482, 45);
            this.trackBarMarkMode.TabIndex = 13;
            this.trackBarMarkMode.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // txtDistanceInMeters
            // 
            this.txtDistanceInMeters.Location = new System.Drawing.Point(345, 519);
            this.txtDistanceInMeters.Name = "txtDistanceInMeters";
            this.txtDistanceInMeters.ReadOnly = true;
            this.txtDistanceInMeters.Size = new System.Drawing.Size(100, 20);
            this.txtDistanceInMeters.TabIndex = 14;
            // 
            // DistanceInKm
            // 
            this.DistanceInKm.Location = new System.Drawing.Point(345, 545);
            this.DistanceInKm.Name = "DistanceInKm";
            this.DistanceInKm.ReadOnly = true;
            this.DistanceInKm.Size = new System.Drawing.Size(100, 20);
            this.DistanceInKm.TabIndex = 15;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // lblMetersDesc
            // 
            this.lblMetersDesc.AutoSize = true;
            this.lblMetersDesc.Location = new System.Drawing.Point(451, 522);
            this.lblMetersDesc.Name = "lblMetersDesc";
            this.lblMetersDesc.Size = new System.Drawing.Size(15, 13);
            this.lblMetersDesc.TabIndex = 16;
            this.lblMetersDesc.Text = "м";
            // 
            // lblKilometersDesc
            // 
            this.lblKilometersDesc.AutoSize = true;
            this.lblKilometersDesc.Location = new System.Drawing.Point(451, 548);
            this.lblKilometersDesc.Name = "lblKilometersDesc";
            this.lblKilometersDesc.Size = new System.Drawing.Size(21, 13);
            this.lblKilometersDesc.TabIndex = 17;
            this.lblKilometersDesc.Text = "км";
            // 
            // txtLatY2
            // 
            this.txtLatY2.Location = new System.Drawing.Point(208, 519);
            this.txtLatY2.Name = "txtLatY2";
            this.txtLatY2.ReadOnly = true;
            this.txtLatY2.Size = new System.Drawing.Size(100, 20);
            this.txtLatY2.TabIndex = 5;
            // 
            // LngX2
            // 
            this.LngX2.Location = new System.Drawing.Point(208, 545);
            this.LngX2.Name = "LngX2";
            this.LngX2.ReadOnly = true;
            this.LngX2.Size = new System.Drawing.Size(100, 20);
            this.LngX2.TabIndex = 6;
            // 
            // lblMarkCoord2Desc
            // 
            this.lblMarkCoord2Desc.AutoSize = true;
            this.lblMarkCoord2Desc.Location = new System.Drawing.Point(169, 503);
            this.lblMarkCoord2Desc.Name = "lblMarkCoord2Desc";
            this.lblMarkCoord2Desc.Size = new System.Drawing.Size(112, 13);
            this.lblMarkCoord2Desc.TabIndex = 8;
            this.lblMarkCoord2Desc.Text = "Координаты метки 2";
            // 
            // lblLngX2Desc
            // 
            this.lblLngX2Desc.AutoSize = true;
            this.lblLngX2Desc.Location = new System.Drawing.Point(169, 545);
            this.lblLngX2Desc.Name = "lblLngX2Desc";
            this.lblLngX2Desc.Size = new System.Drawing.Size(36, 13);
            this.lblLngX2Desc.TabIndex = 9;
            this.lblLngX2Desc.Text = "Lng x:";
            // 
            // lblLatY2Desc
            // 
            this.lblLatY2Desc.AutoSize = true;
            this.lblLatY2Desc.Location = new System.Drawing.Point(169, 522);
            this.lblLatY2Desc.Name = "lblLatY2Desc";
            this.lblLatY2Desc.Size = new System.Drawing.Size(33, 13);
            this.lblLatY2Desc.TabIndex = 10;
            this.lblLatY2Desc.Text = "Lat y:";
            // 
            // lblMarkDistanceDesc
            // 
            this.lblMarkDistanceDesc.AutoSize = true;
            this.lblMarkDistanceDesc.Location = new System.Drawing.Point(345, 503);
            this.lblMarkDistanceDesc.Name = "lblMarkDistanceDesc";
            this.lblMarkDistanceDesc.Size = new System.Drawing.Size(151, 13);
            this.lblMarkDistanceDesc.TabIndex = 18;
            this.lblMarkDistanceDesc.Text = "Расстояние между метками";
            // 
            // btnCleanRoute
            // 
            this.btnCleanRoute.Location = new System.Drawing.Point(0, 42);
            this.btnCleanRoute.Name = "btnCleanRoute";
            this.btnCleanRoute.Size = new System.Drawing.Size(75, 23);
            this.btnCleanRoute.TabIndex = 20;
            this.btnCleanRoute.Text = "Очистка";
            this.btnCleanRoute.UseVisualStyleBackColor = true;
            this.btnCleanRoute.Click += new System.EventHandler(this.CleanRouteLayer);
            // 
            // btnCleanPoligon
            // 
            this.btnCleanPoligon.Location = new System.Drawing.Point(81, 42);
            this.btnCleanPoligon.Name = "btnCleanPoligon";
            this.btnCleanPoligon.Size = new System.Drawing.Size(75, 23);
            this.btnCleanPoligon.TabIndex = 22;
            this.btnCleanPoligon.Text = "Очистка";
            this.btnCleanPoligon.UseVisualStyleBackColor = true;
            this.btnCleanPoligon.Click += new System.EventHandler(this.CleanPolygonLayer);
            // 
            // btnCleanMark
            // 
            this.btnCleanMark.Location = new System.Drawing.Point(162, 42);
            this.btnCleanMark.Name = "btnCleanMark";
            this.btnCleanMark.Size = new System.Drawing.Size(75, 23);
            this.btnCleanMark.TabIndex = 24;
            this.btnCleanMark.Text = "Очистка";
            this.btnCleanMark.UseVisualStyleBackColor = true;
            this.btnCleanMark.Click += new System.EventHandler(this.CleanMarks);
            // 
            // rbRoute
            // 
            this.rbRoute.AutoSize = true;
            this.rbRoute.Location = new System.Drawing.Point(6, 19);
            this.rbRoute.Name = "rbRoute";
            this.rbRoute.Size = new System.Drawing.Size(70, 17);
            this.rbRoute.TabIndex = 29;
            this.rbRoute.TabStop = true;
            this.rbRoute.Text = "Маршрут";
            this.rbRoute.UseVisualStyleBackColor = true;
            // 
            // rbPolygon
            // 
            this.rbPolygon.AutoSize = true;
            this.rbPolygon.Location = new System.Drawing.Point(82, 19);
            this.rbPolygon.Name = "rbPolygon";
            this.rbPolygon.Size = new System.Drawing.Size(68, 17);
            this.rbPolygon.TabIndex = 30;
            this.rbPolygon.TabStop = true;
            this.rbPolygon.Text = "Полигон";
            this.rbPolygon.UseVisualStyleBackColor = true;
            // 
            // grpMarksMode
            // 
            this.grpMarksMode.Controls.Add(this.rbMark);
            this.grpMarksMode.Controls.Add(this.rbRoute);
            this.grpMarksMode.Controls.Add(this.rbPolygon);
            this.grpMarksMode.Controls.Add(this.btnCleanMark);
            this.grpMarksMode.Controls.Add(this.btnCleanRoute);
            this.grpMarksMode.Controls.Add(this.btnCleanPoligon);
            this.grpMarksMode.Location = new System.Drawing.Point(533, 522);
            this.grpMarksMode.Name = "grpMarksMode";
            this.grpMarksMode.Size = new System.Drawing.Size(238, 71);
            this.grpMarksMode.TabIndex = 31;
            this.grpMarksMode.TabStop = false;
            this.grpMarksMode.Text = "Режимы меток";
            // 
            // rbMark
            // 
            this.rbMark.AutoSize = true;
            this.rbMark.Location = new System.Drawing.Point(162, 19);
            this.rbMark.Name = "rbMark";
            this.rbMark.Size = new System.Drawing.Size(57, 17);
            this.rbMark.TabIndex = 31;
            this.rbMark.TabStop = true;
            this.rbMark.Text = "Метка";
            this.rbMark.UseVisualStyleBackColor = true;
            // 
            // btnStartFlight
            // 
            this.btnStartFlight.Location = new System.Drawing.Point(799, 564);
            this.btnStartFlight.Name = "btnStartFlight";
            this.btnStartFlight.Size = new System.Drawing.Size(75, 23);
            this.btnStartFlight.TabIndex = 32;
            this.btnStartFlight.Text = "Полет";
            this.btnStartFlight.UseVisualStyleBackColor = true;
            this.btnStartFlight.Click += new System.EventHandler(this.StartFlight);
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 618);
            this.Controls.Add(this.btnStartFlight);
            this.Controls.Add(this.grpMarksMode);
            this.Controls.Add(this.lblMarkDistanceDesc);
            this.Controls.Add(this.lblKilometersDesc);
            this.Controls.Add(this.lblMetersDesc);
            this.Controls.Add(this.DistanceInKm);
            this.Controls.Add(this.txtDistanceInMeters);
            this.Controls.Add(this.trackBarMarkMode);
            this.Controls.Add(this.ssCoordinates);
            this.Controls.Add(this.lblLatY2Desc);
            this.Controls.Add(this.lblLatY1Desc);
            this.Controls.Add(this.lblLngX2Desc);
            this.Controls.Add(this.lblLngX1Desc);
            this.Controls.Add(this.lblMarkCoord2Desc);
            this.Controls.Add(this.lblMarkCoord1Desc);
            this.Controls.Add(this.LngX2);
            this.Controls.Add(this.LngX1);
            this.Controls.Add(this.txtLatY2);
            this.Controls.Add(this.txtLatY1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.trackBarMapZoom);
            this.Controls.Add(this.gmap);
            this.Controls.Add(this.msMainFunctions);
            this.MainMenuStrip = this.msMainFunctions;
            this.Name = "Map";
            this.Text = "Map";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Map_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMapZoom)).EndInit();
            this.msMainFunctions.ResumeLayout(false);
            this.msMainFunctions.PerformLayout();
            this.ssCoordinates.ResumeLayout(false);
            this.ssCoordinates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMarkMode)).EndInit();
            this.grpMarksMode.ResumeLayout(false);
            this.grpMarksMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.TrackBar trackBarMapZoom;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtLatY1;
        private System.Windows.Forms.TextBox LngX1;
        private System.Windows.Forms.MenuStrip msMainFunctions;
        private System.Windows.Forms.ToolStripMenuItem miLoadCoordinates;
        private System.Windows.Forms.Label lblMarkCoord1Desc;
        private System.Windows.Forms.Label lblLngX1Desc;
        private System.Windows.Forms.Label lblLatY1Desc;
        private System.Windows.Forms.ToolStripMenuItem miXML_GPX;
        private System.Windows.Forms.ToolStripMenuItem miShowXML_GPX;
        private System.Windows.Forms.ToolStripMenuItem miCleanXML_GPX;
        private System.Windows.Forms.StatusStrip ssCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel LatStrip;
        private System.Windows.Forms.ToolStripStatusLabel LngStrip;
        private System.Windows.Forms.TrackBar trackBarMarkMode;
        private System.Windows.Forms.TextBox txtDistanceInMeters;
        private System.Windows.Forms.TextBox DistanceInKm;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lblMetersDesc;
        private System.Windows.Forms.Label lblKilometersDesc;
        private System.Windows.Forms.TextBox txtLatY2;
        private System.Windows.Forms.TextBox LngX2;
        private System.Windows.Forms.Label lblMarkCoord2Desc;
        private System.Windows.Forms.Label lblLngX2Desc;
        private System.Windows.Forms.Label lblLatY2Desc;
        private System.Windows.Forms.ToolStripStatusLabel mStrip;
        private System.Windows.Forms.ToolStripStatusLabel kmStrip;
        private System.Windows.Forms.Label lblMarkDistanceDesc;
        private System.Windows.Forms.Button btnCleanRoute;
        private System.Windows.Forms.Button btnCleanPoligon;
        private System.Windows.Forms.Button btnCleanMark;
        private System.Windows.Forms.RadioButton rbRoute;
        private System.Windows.Forms.RadioButton rbPolygon;
        private System.Windows.Forms.GroupBox grpMarksMode;
        private System.Windows.Forms.RadioButton rbMark;
        private System.Windows.Forms.Button btnStartFlight;
    }
}

