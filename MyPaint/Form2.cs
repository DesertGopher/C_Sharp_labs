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
using System.Drawing.Imaging;

namespace MyPaint
{
    public partial class Form2 : Form
    {
        internal List<Figure> objects;
        Figure Obj;
        public bool isChange = true;
        public bool fileExists = false;
        public Size sizeForm;
        public int FigNum = 0;
        public BufferedGraphics buff;
        public bool isStart = false;
        public bool transfer = false; // флаг перемещения
        public int x_cur = 0, y_cur = 0; // переменные для хранения координат перемещения

        Point global_first_point;

        public Form2()
        {
            objects = new List<Figure>();
            InitializeComponent();
        }

        public Form2(Size s)
        {
            if (s.Width <= 0) s.Width = 320;
            if (s.Height <= 0) s.Height = 240;

            sizeForm = s;

            objects = new List<Figure>();

            InitializeComponent();

            AutoScrollMinSize = s;

            Size tmp = new Size(
                s.Width + PreferredSize.Width,
                s.Height + PreferredSize.Height);

            Size = tmp;
        }


        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            if (e.X - AutoScrollPosition.X < 0 || e.Y - AutoScrollPosition.Y < 0 || e.X - AutoScrollPosition.X >= sizeForm.Width || e.Y - AutoScrollPosition.Y >= sizeForm.Height)
                return;

            isStart = true;

            int xx = e.X - AutoScrollPosition.X;
            int yy = e.Y - AutoScrollPosition.Y;

            FigNum = ((Form1)MdiParent).FigNum;

            if (FigNum != 5) // в случае, если не выбран режим выделения
            {
                foreach (Figure f in objects)
                {
                    f.selected = false; // устанавливаем флаг выделения фигуры в false
                }
                transfer = false; // устанавливаем флаг режима перемещения в false
                Refresh();
            }


            switch (FigNum)
            {
                case 0:
                    Obj = new Rect(new Point(xx, yy), new Point(xx, yy), ((Form1)MdiParent).colorPen, ((Form1)MdiParent).colorBackground, ((Form1)MdiParent).widthPen);
                    Obj.fillFigure = ((Form1)MdiParent).fillFigure;
                    break;
                case 1:
                    Obj = new Ellipse(new Point(xx, yy), new Point(xx, yy), ((Form1)MdiParent).colorPen, ((Form1)MdiParent).colorBackground, ((Form1)MdiParent).widthPen);
                    Obj.fillFigure = ((Form1)MdiParent).fillFigure;
                    break;
                case 2:
                    Obj = new StraightLine(new Point(xx, yy), new Point(xx, yy), ((Form1)MdiParent).colorPen, ((Form1)MdiParent).colorBackground, ((Form1)MdiParent).widthPen);
                    break;
                case 3:
                    Obj = new CurveLine(new Point(xx, yy), new Point(xx, yy), ((Form1)MdiParent).colorPen, ((Form1)MdiParent).colorBackground, ((Form1)MdiParent).widthPen);
                    break;
                case 4:
                    global_first_point = Cursor.Position;
                    Obj = new Text(new Point(xx, yy), new Point(xx, yy), ((Form1)MdiParent).colorPen, Color.White, 1);
                    ((Text)Obj).font = ((Form1)MdiParent).font;
                    break;
                case 5: // в случае, если выбран режим выделения
                    Obj = new Rect(new Point(xx, yy), new Point(xx, yy), Color.Black, Color.White, 1); // создание объекта прямоугольника выделения
                    bool crossed = false; // флаг нажатия левой кнопкой мыши в области прямоугольника, в который вписана фигура
                    foreach (Figure f in objects)
                    {
                        if (f.GetRectangle().IntersectsWith(Obj.GetRectangle())) // в случае, если прямоугольник выделения пересекается с прямоугольником, в который вписана фигура
                        {
                            if (f.selected) // если фигура выделена
                                transfer = true; // устанавливаем флаг режима перемещения в true
                            f.selected = true;
                            crossed = true;
                        }
                    }
                    if (!crossed) // если левая кнопка мыши нажата не в области прямоугольника, в который вписана фигура, то флаг перемещения и выделения всех фигур снимается
                    {
                        foreach (Figure f in objects)
                        {
                            f.selected = false;
                        }
                        transfer = false;
                    }
                    break;
            }
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X - AutoScrollPosition.X < 0 || e.Y - AutoScrollPosition.Y < 0 || e.X - AutoScrollPosition.X >= sizeForm.Width || e.Y - AutoScrollPosition.Y >= sizeForm.Height)
                    return;

            ((Form1)MdiParent).SetCursor(e.X - AutoScrollPosition.X, e.Y - AutoScrollPosition.Y); // передача текущих координат курсора мыши

            if (!isStart) return;

            if (e.Button == MouseButtons.Left)
            {
                if (FigNum == 3)
                {
                    Obj.pp.Add(new Point(e.X - AutoScrollPosition.X, e.Y - AutoScrollPosition.Y));
                }
                if (FigNum == 5) // если выбран режим выделения
                {
                    if (transfer) // если включен режим перемещения
                    {
                        x_cur = e.X - AutoScrollPosition.X; // изменение координат перемещения
                        y_cur = e.Y - AutoScrollPosition.Y;
                    }
                    else 
                    {
                        foreach (Figure f in objects)
                        {
                            f.selected = f.GetRectangle().IntersectsWith(Obj.GetRectangle()); // изменение флага выделения у фигуры
                        }
                    }
                    Refresh();
                }
                buff.Render();

                if (!(FigNum == 5 && transfer)) // в случае, если не включен режим выделения и перемещения
                {
                    Graphics g = CreateGraphics();
                    Obj.MouseMove(g, new Point(e.X - AutoScrollPosition.X, e.Y - AutoScrollPosition.Y), AutoScrollPosition.X, AutoScrollPosition.Y);
                    g.Dispose();
                }
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            if (isStart && e.Button == MouseButtons.Left)
            {
                isStart = false;
                int xx = e.X - AutoScrollPosition.X;
                int yy = e.Y - AutoScrollPosition.Y;
                    if (transfer && FigNum == 5) // в случае, если включен режим выделения и перемещения
                    {
                        bool is_in_area = true; // флаг попадания в область рисования
                        foreach (Figure f in objects)
                        {
                            if (f.selected && !f.IsInArea(x_cur - this.Obj.p1.X, y_cur - this.Obj.p1.Y,
                                sizeForm.Width, sizeForm.Height)) // если фигура выделена и она выходит за пределы области рисования
                            {
                                is_in_area = false; // установка флага попадания в область рисования в false
                                break;
                            }
                        }
                        if (is_in_area) // если все выделенные фигуры оказались в области рисованм
                        {
                            foreach (Figure f in objects)
                            {
                                if (f.selected)
                                {
                                    f.MoveTo(x_cur - this.Obj.p1.X, y_cur - this.Obj.p1.Y); // вызов метода для окончательного перемещения выделенных фигур 
                            }
                            }
                        }
                        transfer = false; // снятие флага перемещения
                    }
                    else if (xx >= 0 && yy >= 0 && xx < sizeForm.Width && yy < sizeForm.Height)
                    {
                        Obj.p2.X = xx;
                        Obj.p2.Y = yy;
                        if (FigNum == 4)
                        {
                            Form form = new Form();
                            form.FormBorderStyle = FormBorderStyle.None;
                            form.StartPosition = FormStartPosition.Manual;

                            form.SetDesktopLocation(
                                Math.Min(global_first_point.X, Cursor.Position.X),
                                Math.Min(global_first_point.Y, Cursor.Position.Y));
                            form.BackColor = Color.Magenta;
                            form.TransparencyKey = Color.Magenta;
                            TextBox tmpTextBox = new TextBox();
                            tmpTextBox.Font = ((Form1)this.MdiParent).font;
                            tmpTextBox.ForeColor = ((Form1)this.MdiParent).colorPen;
                            tmpTextBox.Multiline = true;
                            tmpTextBox.Height = Math.Abs(Obj.p2.Y - Obj.p1.Y + 1);
                            tmpTextBox.Width = Math.Abs(Obj.p2.X - Obj.p1.X + 1);
                            tmpTextBox.Parent = form;
                            tmpTextBox.KeyPress += TmpTextBox_KeyPress;
                            form.Size = tmpTextBox.Size;
                            tmpTextBox.Visible = true;
                            form.ShowDialog();
                        }
                        if (FigNum != 5) // условие для того, чтобы не запоминать прямоугольник выделения
                        {
                            objects.Add(Obj);
                            isChange = true;
                        }
                    }
                Invalidate();
            }
        }

        private void TmpTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ((Text)Obj).text = ((TextBox)sender).Text;
                ((Form)((TextBox)sender).Parent).Close();
            }
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            buff.Graphics.Clear(BackColor);
            Rect cur = new Rect(new Point(0, 0), new Point(sizeForm.Width, sizeForm.Height ), Color.White, Color.White, 0);
            cur.fillFigure = true;
            cur.Draw(buff.Graphics, AutoScrollPosition.X, AutoScrollPosition.Y);
            foreach (Figure f in objects)
            {
                if (FigNum == 5) // если выбран режим выделения
                {
                    if (f.selected) // если фигура выделена
                    {
                        Pen p = new Pen(Color.Purple, 1);
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        if (transfer) // если включен режим перемещения
                        {
                            f.Move(buff.Graphics, AutoScrollPosition.X, AutoScrollPosition.Y,
                                x_cur - this.Obj.p1.X, y_cur - this.Obj.p1.Y); // вызов метода для промежуточного перемещения выделенных фигур
                        }
                        else
                        {
                            Rectangle rr = f.GetRectangle(); // получение прямоугольника, в который вписана фигура
                            rr.X += AutoScrollPosition.X; // учёт скроллинга
                            rr.Y += AutoScrollPosition.Y;
                            f.Draw(buff.Graphics, AutoScrollPosition.X, AutoScrollPosition.Y); // рисование самой фигуры
                            buff.Graphics.DrawRectangle(p, rr); // рисование контура выделения
                        }
                    }
                    else
                    {
                        f.Draw(buff.Graphics, AutoScrollPosition.X, AutoScrollPosition.Y); // рисование фигуры, если она не выделена
                    }
                }
                else
                {
                    f.Draw(buff.Graphics, AutoScrollPosition.X, AutoScrollPosition.Y); // рисование фигуры, если не выбран режим выделения
                }
            }
            buff.Render(e.Graphics);
        }

        public void saveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog.Filter = "kek files|*.kek|All files|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, objects);
                formatter.Serialize(stream, sizeForm);
                Text = saveFileDialog.FileName;
                stream.Close();
                isChange = false;
                fileExists = true;
            }
        }

        public void save()
        {
            if (fileExists)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(Text, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, objects);
                formatter.Serialize(stream, sizeForm);
                stream.Close();
                isChange = false;
            }
            else
            {
                saveAs();
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isChange)
            {
                switch (MessageBox.Show(
                    "Сохранить документ\n" + Text,
                    "Сохранить?",
                    MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        save();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            BufferedGraphicsManager.Current.MaximumBuffer = SystemInformation.PrimaryMonitorMaximizedWindowSize;
            buff = BufferedGraphicsManager.Current.Allocate(this.CreateGraphics(),new Rectangle(0,0,1920,1080));
            buff.Graphics.Clear(BackColor);
            Rect cur = new Rect(new Point(0, 0), new Point(sizeForm.Width - 1, sizeForm.Height - 1), Color.White, Color.White, 0);
            cur.fillFigure = true;
            cur.Draw(buff.Graphics, AutoScrollPosition.X, AutoScrollPosition.Y);
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            buff.Dispose();
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            ((Form1)MdiParent).SetSizePicture(sizeForm); // передача размера текущего рисунка
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e) // метод, который позволяет получить отпущенную клавишу
        {
            if (e.KeyCode == Keys.Delete) // если нажата кнопка Delete
            {
                DeleteSelected(); // вызов метода для удаления выделенных фигур
            }
        }

        public void DeleteSelected() // метод для удаления выделенных фигур
        {
            foreach (Figure figure in objects.ToArray())
            {
                if (figure.selected == true)
                {
                    objects.Remove(figure);
                }
            }
            Refresh();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }

        public void CopyAsMetafile()
        {
            Graphics g = CreateGraphics();
            IntPtr dc = g.GetHdc();
            Metafile mf = new Metafile(dc, EmfType.EmfOnly);
            g.ReleaseHdc(dc);
            g.Dispose();

            Graphics gr = Graphics.FromImage(mf);

            foreach (Figure f in objects)
                if (f.selected)
                {
                    f.selected = false;
                    f.Draw(gr, 0, 0);
                    f.selected = true;
                }

            gr.Dispose();

            ClipboardMetafileHelper.PutEnhMetafileOnClipboard(this.Handle, mf);
        }

        public void Copy()
        {
            List<Figure> buff = new List<Figure>();

            foreach (Figure f in objects)
            {
                if (f.selected)
                {
                    buff.Add(f);
                }
            }

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, buff);
            DataObject dataObject = new DataObject();
            dataObject.SetData("Lab11", ms);
            Clipboard.SetDataObject(dataObject, true);
            ms.Close();
        }

        public void Paste()
        {
            foreach (Figure f in objects)
            {
                f.selected = false;
            }

            MemoryStream ms = (MemoryStream)Clipboard.GetDataObject().GetData("Lab11");
            BinaryFormatter formatter = new BinaryFormatter();
            List<Figure> buff = (List<Figure>)formatter.Deserialize(ms);
            ms.Close();

            int x_min = Int32.MaxValue;
            int y_min = Int32.MaxValue;
            int x_max = Int32.MinValue;
            int y_max = Int32.MinValue;

            foreach (Figure f in buff)
            {
                Rectangle t = f.GetRectangle();

                if (x_min > t.X)
                    x_min = t.X;
                if (y_min > t.Y)
                    y_min = t.Y;

                if (x_max < t.X + t.Width)
                    x_max = t.X + t.Width;
                if (y_max < t.Y + t.Height)
                    y_max = t.Y + t.Height;
            }

            if (y_max - y_min >= sizeForm.Height || x_max - x_min >= sizeForm.Width)
            {
                MessageBox.Show("Блок превышает допустимые размеры!");
                return;
            }

            foreach (Figure f in buff)
            {
                f.MoveTo(-x_min, -y_min);
                f.selected = true;
                objects.Add(f);
            }

            FigNum = 5;
            Refresh();
        }

        public bool IsSelected()
        {
            foreach (Figure f in objects)
            {
                if (f.selected) return true;
            }
            return false;
        }

        public void SelectAll()
        {
            foreach (Figure f in objects)
            {
                f.selected = true;
            }

            FigNum = 5;
        }
    }
}

        