using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyPaint
{
    public partial class Form2 : Form
    {
        public List<Figure> objects =new List<Figure>();
        Figure Obj;
        Graphics g;
        bool isMouseDown = false;
        public string fileName = null; //имя файла
        public bool Change = false;
        Form1 f1;
        BufferedGraphics BG;
        public int FigNum = 3;

        public Form2()
        {
            InitializeComponent();
           
            AutoScrollMinSize = (this.Size);
        }
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            f1 = (Form1)ParentForm;
           
            if (e.Button == MouseButtons.Left && isMouseDown == false)
            {
                isMouseDown = true;
                FigNum = ((Form1)MdiParent).FigNum;
                switch (FigNum) // выбор фигуры для рисования

                {
                    case 0:
                        Obj = new Rect(e.Location, e.Location, f1.pensize, f1.linecolor, f1.fillcolor);

                        break;
                    case 1:
                        Obj = new Ellipse(e.Location, e.Location, f1.pensize, f1.linecolor, f1.fillcolor);

                        break;
                    case 2:
                        Obj = new StraightLine(e.Location, e.Location, f1.pensize, f1.linecolor, f1.fillcolor);
                        break;
                    case 3:
                        Obj = new CurveLine(e.Location, e.Location, f1.pensize, f1.linecolor, f1.fillcolor);
                        break;
                }
                Obj.IsFilling = f1.IsFigureFilling;//Определяем состояние флага заливки
            }
           
        }


        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            f1 = (Form1)ParentForm;
            f1.GetStatusBar().Panels[0].Text = e.Location.X - AutoScrollPosition.X + ", " + (e.Location.Y - AutoScrollPosition.Y);
            g = CreateGraphics();
            if (isMouseDown)
            {
                BG.Render(g);
                Obj.MouseMove(e.Location);
                Obj.DrawDash(g, AutoScrollPosition);               
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            f1 = (Form1)ParentForm;

            if (isMouseDown)
            {
                //Проверка на то, помещается ли фигура в область рисования
                if (Obj.endPoint.X < Size.Width && Obj.endPoint.Y < Size.Height
                && Obj.startPoint.X > 0 && Obj.startPoint.Y > 0)
                {
                    Change = true;
                    Obj.Draw(BG.Graphics, AutoScrollPosition);
                    objects.Add(Obj);
                }
                else
                {
                    Obj.Hide(g);
                }
                BG.Render();
                Invalidate();
                isMouseDown = false;
            }
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            BG.Graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, this.Width, this.Height);
            foreach (Figure i in objects)
            {
                i.Draw(BG.Graphics, AutoScrollPosition);
            }
            BG.Render(e.Graphics);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
            //Устанавливаем максимальный размер буферной зоны
            BufferedGraphicsManager.Current.MaximumBuffer = SystemInformation.PrimaryMonitorMaximizedWindowSize;

            BG = BufferedGraphicsManager.Current.Allocate(g, new Rectangle(0, 0,
                SystemInformation.PrimaryMonitorMaximizedWindowSize.Width,
                SystemInformation.PrimaryMonitorMaximizedWindowSize.Height));

            BG.Graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, Size.Width, Size.Height);
            BG.Render();

        }
        //Возвращение размеры рисунка
        public Size GetSize()
        {
            return Size;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Change) //если текущий файл изменен
            {
                DialogResult result;
                result = MessageBox.Show("Сохранить ли изменения в \"" + this.Text + "\"?", "Вопрос извечный", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            f1 = new Form1();
                            f1.SaveFile(this); //сохранение файла (см. форму 1)
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            e.Cancel = true; //возвращение к файлу
                            return;
                        }
                }
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            BG.Dispose();
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            f1 = (Form1)ParentForm;  
            f1.GetStatusBar().Panels[4].Text = Width.ToString() + ";" + Height.ToString();
        }
    }
}
