using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    [Serializable()]
    public partial class Form1 : Form
    {
        public int FigNum = 3; // номер выбранной фигуры
        public int pensize; //толщина линии
        public Color linecolor, fillcolor; //цвет линии и фона
        int IfOpen;
        Form2 f2;
        public bool IsFigureFilling { get; set; }
        public Form1()
        {
            InitializeComponent();
            linecolor = Color.Black;//по умолчанию цвет линии - чёрный    
            fillcolor = Color.White;//по умолчанию цвет фона - белый    
            pensize = 1;//по умолчанию толщина линии - 1   
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void CheckIfOpen()
        {
            IfOpen = MdiChildren.Length;
            if(IfOpen==0)
            {
                сохранитьToolStripMenuItem.Enabled = false;
                сохранитьКакToolStripMenuItem.Enabled = false;
            }
            else
            {
                сохранитьToolStripMenuItem.Enabled = true;
                сохранитьКакToolStripMenuItem.Enabled = true;
                f2 = (Form2)ActiveMdiChild;
                if(f2.Change==false)
                {
                    сохранитьToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                Form f = new Form2();
                f.MdiParent = this;
                f.Text = "Рисунок " + this.MdiChildren.Length.ToString();
               // f.Height = MySize.height; //
                //f.Width = MySize.width; //
                f.Show();
            
        }

        public void SaveFile(Form2 f2) // функция сохранения файла
        {
            string fileName;
            BinaryFormatter formatter1 = new BinaryFormatter(); //Сохранение объекта obj некоторого класса X в файле с именем fileName 
            if (f2.fileName == null) //Имя файла, выбранное в диалоговом окне файла - если раньше этот файл не был сохранен
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Environment.CurrentDirectory; //При инициализации файловых диалогов указываем в качестве стартового каталога текущий каталог программы
                saveFileDialog1.Filter = "Мюсли(*.rytp)|*.rytp|All files (*.*)|*.*"; //Задаем текущую строку фильтра имен файлов
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) //если нажали OK
                {
                    fileName = saveFileDialog1.FileName;
                    Stream myStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                    formatter1.Serialize(myStream, f2.objects);
                    myStream.Close();
                    f2.Change = false;
                    f2.fileName = fileName;
                    f2.Text = Path.GetFileName(saveFileDialog1.FileName);
                }
            }
            else // если раньше этот файл был сохранен
            {
                fileName = f2.fileName;
                Stream myStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter1.Serialize(myStream, f2.objects);
                myStream.Close();
                f2.Change = false;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            OpenFileDialog OpenPic = new OpenFileDialog();
            OpenPic.InitialDirectory = Environment.CurrentDirectory;
            OpenPic.Filter = "Мюсли(*.rytp)|*.rytp|All files (*.*)|*.*";
            if (OpenPic.ShowDialog() == DialogResult.OK)
            {
                fileName = OpenPic.FileName;
                BinaryFormatter formatter1 = new BinaryFormatter(); // Восстановление сохранённого объекта из файла:                           
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                List<Figure> array = (List<Figure>)formatter1.Deserialize(stream);
                stream.Close();
                f2 = new Form2();
                f2.MdiParent = this;
                f2.fileName = fileName;
                f2.Text = OpenPic.SafeFileName;
                f2.objects = array;
                f2.Show();
                f2.Change = false;
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile((Form2)ActiveMdiChild);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f2 = (Form2)ActiveMdiChild;
            f2.fileName = null;
            SaveFile((Form2)ActiveMdiChild);
        }

        private void цветЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog myDialog = new ColorDialog();  //открытие диалогового окна                              
            DialogResult result = myDialog.ShowDialog();
            if (result == DialogResult.OK) linecolor = myDialog.Color;
            statusBar1.Refresh();
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog myDialog = new ColorDialog(); //открытие диалогового окна 
            DialogResult result = myDialog.ShowDialog();
            if (result == DialogResult.OK) fillcolor = myDialog.Color;
            statusBar1.Refresh();
        }

        private void толщинаЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenSz myDialog = new PenSz(); //открытие диалогового окна                           
            DialogResult result = myDialog.ShowDialog(this);
            if (result == DialogResult.OK) pensize = myDialog.ch2;  //проверка и установление выбранной толщины линии 
            statusBar1.Refresh();
        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            эллипсToolStripMenuItem.Checked = false;
            прямаяToolStripMenuItem.Checked = false;
            криваяToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = true;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = true;
            toolStripButton11.Checked = false;

            FigNum = 0;


        }

        private void эллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            эллипсToolStripMenuItem.Checked = true;
            прямаяToolStripMenuItem.Checked = false;
            криваяToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = true;

            FigNum = 1;

        }

        private void прямаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            эллипсToolStripMenuItem.Checked = false;
            прямаяToolStripMenuItem.Checked = true;
            криваяToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            toolStripButton8.Checked = true;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = false;

            FigNum = 2;

        }

        private void криваяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            эллипсToolStripMenuItem.Checked = false;
            прямаяToolStripMenuItem.Checked = false;
            криваяToolStripMenuItem.Checked = true;
            прямоугольникToolStripMenuItem.Checked = false;
            toolStripButton8.Checked = false;
            toolStripButton9.Checked = true;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = false;

            FigNum = 3;

        }

        private void заливкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (заливкаToolStripMenuItem.Checked|| toolStripButton12.Checked)
                IsFigureFilling = true; //флаг заливки для функции в Form2-> Class Figure
            else IsFigureFilling = false;

        }

        private void размерФормыToolStripMenuItem_Click(object sender, EventArgs e)
        {          
                FormSize MySize = new FormSize();
            DialogResult result = MySize.ShowDialog(this);
            if (result == DialogResult.OK) 
            {
                f2 = new Form2();
                f2.MdiParent = this;
                f2.Text = "Рисунок " + this.MdiChildren.Length.ToString();
                f2.Height = MySize.height; 
                f2.Width = MySize.width; 
                f2.Show();
            }
        }

        private void файлToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            CheckIfOpen();
        }

        private void statusBar1_DrawItem(object sender, StatusBarDrawItemEventArgs sbdevent)
        {
            Graphics g = statusBar1.CreateGraphics();        //Объект для рисования прямоугольников в строке состояния

            //вывод толщины линии
            statusBarPanelPenSize.Text = pensize.ToString();

            //Подписи 
            g.DrawString("Линия", new Font(Font, FontStyle.Regular),
                new SolidBrush(Color.Black), statusBarPanelCoord.Width + 5, 7);
            g.DrawString("Заливка", new Font(Font, FontStyle.Regular),
             new SolidBrush(Color.Black), statusBarPanelCoord.Width + 5 + statusBarPanelLineCol.Width, 7);

            int RectDist = 60; //Перемення для расположения прямоугольников относительно начала панели

            //Рисование индикаторов цвета
            g.FillRectangle(new SolidBrush(linecolor), Rectangle.FromLTRB(statusBar1.Panels[0].Width + RectDist, 2,
                statusBar1.Panels[0].Width + RectDist + 20, 22));
            g.DrawRectangle(new Pen(Color.Black, 1), Rectangle.FromLTRB(statusBar1.Panels[0].Width + RectDist, 2,
                statusBar1.Panels[0].Width + RectDist + 20, 22));

            g.FillRectangle(new SolidBrush(fillcolor),
                Rectangle.FromLTRB(statusBar1.Panels[0].Width + statusBar1.Panels[1].Width + RectDist, 2,
                statusBar1.Panels[0].Width + statusBar1.Panels[1].Width + RectDist + 20, 22));
            g.DrawRectangle(new Pen(Color.Black, 1),
                Rectangle.FromLTRB(statusBar1.Panels[0].Width + statusBar1.Panels[1].Width + RectDist, 2,
                statusBar1.Panels[0].Width + statusBar1.Panels[1].Width + RectDist + 20, 22));
        }

        public StatusBar GetStatusBar()
        {
            return statusBar1;
        }
    }
}
