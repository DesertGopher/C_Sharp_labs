using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing.Text;


namespace MyPaint
{
    public partial class Form1 : Form
    {
        public Color colorPen, colorBackground;
        public int widthPen;
        public Size sizeForm2;
        public int FigNum = 0;
        public bool fillFigure = false;
        public Font font;
        private bool panelAdded = false;
        public bool gridOn = false;
        Font CustomFont;
        public int gridStep;
        public bool alignToGrid = false;

        public Form1()
        {
            InitializeComponent();
            colorPen = Color.Black;
            colorBackground = Color.White;
            widthPen = 1;
            font = new Font("Times New Roman", 12);
            MyFont();
            menuStrip1.Font = CustomFont;
            gridStep = 10;
    }

        private void окноToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void новоеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(sizeForm2);
            f.MdiParent = this;
            f.Text = "Рисунок " + this.MdiChildren.Length.ToString();
            f.Show();
            сохранитьToolStripMenuItem.Enabled = true;
            сохранитьКакToolStripMenuItem.Enabled = true;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "kek files|*.kek|All files|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                Form2 f2 = new Form2();
                f2.MdiParent = this;
                f2.Text = openFileDialog.FileName;
                f2.objects = (List<Figure>)(formatter.Deserialize(stream));
                f2.sizeForm = (Size)(formatter.Deserialize(stream));
                f2.AutoScrollMinSize = f2.sizeForm;
                Size tmp = new Size(f2.sizeForm.Width + f2.PreferredSize.Width, f2.sizeForm.Height + f2.PreferredSize.Height);
                f2.Size = tmp;
                stream.Close();
                f2.Show();
                f2.isChange = false;
                f2.fileExists = true;
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                ((Form2)(this.ActiveMdiChild)).save();
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Form2)this.ActiveMdiChild).saveAs();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MdiChildren.Length > 0) {
                сохранитьКакToolStripMenuItem.Enabled = true;
                сохранитьToolStripMenuItem.Enabled = ((Form2)(this.ActiveMdiChild)).isChange;
            }
            else
            {
                сохранитьКакToolStripMenuItem.Enabled = false;
                сохранитьToolStripMenuItem.Enabled = false;
            }
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorBackground = colorDialog.Color;
                Graphics g = statusBar1.CreateGraphics();
                SolidBrush br = new SolidBrush(colorBackground);
                Rectangle r = new Rectangle(337, 0, statusBar1.Panels[4].Width, statusBar1.Size.Height);
                g.FillRectangle(br, r); //рисование прямоугольника с текущим цветом фона на панели строки состояния
                br.Dispose();
                g.Dispose();
            }
            colorDialog.Dispose();
        }

        private void цветЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorPen = colorDialog.Color;
                Graphics g = statusBar1.CreateGraphics();
                SolidBrush br = new SolidBrush(colorPen);
                Rectangle r = new Rectangle(237, 0, statusBar1.Panels[2].Width, statusBar1.Size.Height);
                g.FillRectangle(br, r); //рисование прямоугольника с текущим цветом линии на панели строки состояния
                br.Dispose();
                g.Dispose();
            }
            colorDialog.Dispose();
        }

        private void размерФормыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSize dialogSize = new FormSize();

            if (dialogSize.ShowDialog() == DialogResult.OK)
            {
                sizeForm2 = dialogSize.sizeForm;
            }
            dialogSize.Dispose();
        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e) // метод-обработчик нажатия на пункт подменю и кнопку кнопочной панели "Прямоугольник"
        {
            if (panelAdded)
            {
                statusBar1.Panels.RemoveAt(8);
                panelAdded = false;
            }
            эллипсToolStripMenuItem.Checked = false;
            прямаяЛинияToolStripMenuItem.Checked = false;
            произвольнаяЛинияToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = true;
            шрифтToolStripMenuItem.Checked = false;
            выделениеToolStripMenuItem.Checked = false;
            FigNum = 0;
            toolStripButton8.Checked = true;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = false;
            toolStripButton14.Checked = false;
            toolStripButton15.Checked = false;
        }

        private void эллипсToolStripMenuItem_Click(object sender, EventArgs e) // метод-обработчик нажатия на пункт подменю и кнопку кнопочной панели "Эллипс"
        {
            if (panelAdded)
            {
                statusBar1.Panels.RemoveAt(8);
                panelAdded = false;
            }
            эллипсToolStripMenuItem.Checked = true;
            прямаяЛинияToolStripMenuItem.Checked = false;
            произвольнаяЛинияToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            шрифтToolStripMenuItem.Checked = false;
            выделениеToolStripMenuItem.Checked = false;
            FigNum = 1;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = true;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = false;
            toolStripButton14.Checked = false;
            toolStripButton15.Checked = false;
        }

        private void прямаяToolStripMenuItem_Click(object sender, EventArgs e) // метод-обработчик нажатия на пункт подменю и кнопку кнопочной панели "Прямая линия"
        {
            if (panelAdded)
            {
                statusBar1.Panels.RemoveAt(8);
                panelAdded = false;
            }
            эллипсToolStripMenuItem.Checked = false;
            прямаяЛинияToolStripMenuItem.Checked = true;
            произвольнаяЛинияToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            шрифтToolStripMenuItem.Checked = false;
            выделениеToolStripMenuItem.Checked = false;
            FigNum = 2;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = true;
            toolStripButton11.Checked = false;
            toolStripButton14.Checked = false;
            toolStripButton15.Checked = false;
        }

        private void криваяToolStripMenuItem_Click(object sender, EventArgs e) // метод-обработчик нажатия на пункт подменю и кнопку кнопочной панели "Произвольная линия"
        {
            if (panelAdded)
            {
                statusBar1.Panels.RemoveAt(8);
                panelAdded = false;
            }
            эллипсToolStripMenuItem.Checked = false;
            прямаяЛинияToolStripMenuItem.Checked = false;
            произвольнаяЛинияToolStripMenuItem.Checked = true;
            прямоугольникToolStripMenuItem.Checked = false;
            шрифтToolStripMenuItem.Checked = false;
            выделениеToolStripMenuItem.Checked = false;
            FigNum = 3;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = true;
            toolStripButton14.Checked = false;
            toolStripButton15.Checked = false;
        }

        private void заливкаToolStripMenuItem_Click(object sender, EventArgs e) // метод-обработчик нажатия на пункт подменю и кнопку кнопочной панели "Заливка"
        {
            fillFigure = !fillFigure; 
            заливкаToolStripMenuItem.Checked = fillFigure;
            toolStripButton12.Checked = fillFigure; // фиксация кнопки "Заливка" на кнопочной панели в нажатом состоянии
        }

        public void SetSizePicture(Size sizePict) // метод для отображения размера текущего рисунка
        {
            statusBar1.Panels[7].Text = sizePict.Width.ToString() + "x" + sizePict.Height.ToString(); // отображение на панели строки состояния размера текущего рисунка
        }

        public void SetCursor(int x, int y) // метод для отображения на панели строки состояния текущих координат курсора мыши
        {
            statusBar1.Panels[5].Text = x.ToString();
            statusBar1.Panels[6].Text = y.ToString();
        }

        private void statusBar1_DrawItem(object sender, StatusBarDrawItemEventArgs sbdevent) // метод-обработчик отображения информации на панели строки состояния
        {
            Graphics g = statusBar1.CreateGraphics();
            SolidBrush br = new SolidBrush(colorPen);
            Rectangle r = new Rectangle(237, 0, statusBar1.Panels[2].Width, statusBar1.Size.Height); 
            g.FillRectangle(br, r); //рисование прямоугольника с текущим цветом линии на панели строки состояния
            br.Dispose();

            br = new SolidBrush(colorBackground);
            r = new Rectangle(337, 0, statusBar1.Panels[4].Width, statusBar1.Size.Height);
            g.FillRectangle(br, r); //рисование прямоугольника с текущим цветом фона на панели строки состояния
            br.Dispose();
            g.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                font = fontDialog.Font;
                if (toolStripButton14.Checked)
                {
                    statusBar1.Panels[8].Text = font.Name + " " + font.SizeInPoints.ToString();
                }
                //statusBar1.Panels[0].Text = "Толщина линии: " + widthPen.ToString(); // отображение на панели строки состояния текущей толщины линии
            }
            fontDialog.Dispose();
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            эллипсToolStripMenuItem.Checked = false;
            прямаяЛинияToolStripMenuItem.Checked = false;
            произвольнаяЛинияToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = true;
            выделениеToolStripMenuItem.Checked = false;
            FigNum = 4;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = false;
            toolStripButton14.Checked = true;
            toolStripButton15.Checked = false;
            if (panelAdded)
            {
                statusBar1.Panels.RemoveAt(8);
                panelAdded = false;
            }
            statusBar1.Panels.Add(font.Name + " " + font.SizeInPoints.ToString());
            statusBar1.Panels[8].AutoSize = StatusBarPanelAutoSize.Contents;
            panelAdded = true;
        }

        private void выделениеToolStripMenuItem_Click(object sender, EventArgs e) // обработчик события нажатия на пункт режима выделения фигур
        {
            эллипсToolStripMenuItem.Checked = false;
            прямаяЛинияToolStripMenuItem.Checked = false;
            произвольнаяЛинияToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            шрифтToolStripMenuItem.Checked = false;
            выделениеToolStripMenuItem.Checked = true;
            FigNum = 5;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = false;
            toolStripButton14.Checked = false;
            toolStripButton15.Checked = true;
        }

        private void удалитьВыделенныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
                ((Form2)this.ActiveMdiChild).DeleteSelected();
        }


        private void толщинаЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenSz dialogWidth = new PenSz();

            if (dialogWidth.ShowDialog() == DialogResult.OK)
            {
                widthPen = dialogWidth.getWidth();
                statusBar1.Panels[0].Text = "Толщина линии: " + widthPen.ToString(); // отображение на панели строки состояния текущей толщины линии
            }
            dialogWidth.Dispose();
        }

        private void копироватьКакМетафайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Form2)(this.ActiveMdiChild)).CopyAsMetafile();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Form2)(this.ActiveMdiChild)).Copy();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Form2)(this.ActiveMdiChild)).Copy();

            удалитьВыделенныеToolStripMenuItem_Click(sender, e);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Form2)(this.ActiveMdiChild)).Paste();

            выделениеToolStripMenuItem_Click(sender, e);
        }

        private void выделитьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            выделениеToolStripMenuItem_Click(sender, e);
            ((Form2)(this.ActiveMdiChild)).SelectAll();
            ((Form2)(this.ActiveMdiChild)).Refresh();
        }

        private void правкаToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            bool tb = (this.ActiveMdiChild != null && ((Form2)(this.ActiveMdiChild)).IsSelected());
            копироватьКакМетафайлToolStripMenuItem.Enabled = tb;
            копироватьToolStripMenuItem.Enabled = tb;
            вырезатьToolStripMenuItem.Enabled = tb;

            IDataObject ido = Clipboard.GetDataObject();
            вставитьToolStripMenuItem.Enabled = (ido != null && ido.GetDataPresent("Lab11"));
        }

        private void сеткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridOn = !gridOn;
            сеткаToolStripMenuItem.Checked = gridOn;
            if (gridOn == false)
            {
                alignToGrid = false;
                привязатьКСеткеToolStripMenuItem.Checked = alignToGrid;
            }
            foreach (Form2 f2 in MdiChildren)
            {
                f2.Refresh();
            }
        }

        private void шагСеткиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridStepDialog dialog = new GridStepDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                gridStep = dialog.gridStep;

                if (gridOn)
                {
                    foreach (Form2 f2 in MdiChildren)
                    {
                        f2.Refresh();
                    }
                }
            }
            dialog.Dispose();
        }

        private void выровнятьПоСеткеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridOn)
            {
                ((Form2)ActiveMdiChild).alignObjects();
                ((Form2)ActiveMdiChild).Refresh();
            }
        }

        private void привязатьКСеткеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!gridOn)
                сеткаToolStripMenuItem_Click(sender, e);
            alignToGrid = !alignToGrid;
            привязатьКСеткеToolStripMenuItem.Checked = alignToGrid;
        }

        private void MyFont()
        {
            PrivateFontCollection my_font = new PrivateFontCollection();
            my_font.AddFontFile("MyFont.ttf");
            CustomFont = new Font(my_font.Families[0], 12);
        }
    }
}
