using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyPaint
{
    [Serializable()]
    abstract class Figure
    {
        public Point p1, p2;
        protected  Color colorPen, colorBackground;
        protected int widthPen;
        public List<Point> pp; 
        public bool fillFigure;
        [NonSerialized()] public bool selected; // флаг выделения
        Point[] points;
        int sW, sH;
        [NonSerialized()] public bool selected_d = false;
        [NonSerialized()] int size_marker = 15;
        [NonSerialized()] int[] dx = new int[8] { 0, 2, 2, 0, 1, 2, 1, 0 };
        [NonSerialized()] int[] dy = new int[8] { 0, 0, 2, 2, 0, 1, 2, 1 };
        public Figure(Point p1, Point p2, Color cP, Color cB, int w)
        {
            this.p1 = p1;
            this.p2 = p2;
            colorPen = cP;
            colorBackground = cB;
            widthPen = w;
            pp = new List<Point>(); // инициализация динамического массива точек произвольной линии
        }

        public abstract void Draw(Graphics g, int x, int y);
        public abstract void DrawDash(Graphics g, int x, int y);
        public abstract Rectangle GetRectangle(); // метод, для получения прямоугольника, в который вписана фигура
        public void Move(Graphics g, int x, int y, int dx, int dy) // метод для промежуточного перемещения выделенных фигур
        {
            DrawDash(g, x + dx, y + dy);
        }
        public void MouseMove(Graphics g, Point p, int x, int y)
        {
            p2 = p;

            DrawDash(g, x, y);
        }
        public void norm(ref int x1, ref int y1, ref int x2, ref int y2)
        {
            int x_min = Math.Min(x1, x2);
            int x_max = Math.Max(x1, x2);
            int y_min = Math.Min(y1, y2);
            int y_max = Math.Max(y1, y2);
            x1 = x_min;
            x2 = x_max;
            y1 = y_min;
            y2 = y_max;
        }
        public void MoveTo(int dx, int dy) // метод для окончательного перемещения выделенных фигур
        {
            p1.X += dx; // изменение координат точек фигур (прямоугольник, эллипс, текс, прямая линия)
            p1.Y += dy;
            p2.X += dx;
            p2.Y += dy;
            // в случае если фигура - произвольная линия
            Point[] points = new Point[pp.Count]; // создание обычного массива точек произвольной линии
            for (int i = 0; i < pp.Count; ++i) // перенос информации из динамического массива в обычный 
            {
                points[i].X = pp[i].X + dx;
                points[i].Y = pp[i].Y + dy;
            }
            pp.Clear();
            pp.AddRange(points);
        }
        public bool IsInArea(int dx, int dy, int w, int h) // метод для проверки попадания блока выделенных фигур в поле рисования
        {
            // для прямоугольника, эллипса, текста, прямой линии
            if (p1.X + dx < 0 || p1.Y + dy < 0 || p2.X + dx >= w || p2.Y + dy >= h)
                return false;
            // для произвольной линии
            for(int i = 0; i < pp.Count; ++i)
            {
                if (pp[i].X + dx < 0 || pp[i].Y + dy < 0 || pp[i].X + dx >= w || pp[i].Y + dy >= h)
                    return false;
            }
            return true;
        }

        public void Align(int step, Size s)
        {
            if (pp.Count != 0)
            {
                int X1 = (int)(Math.Round(pp[0].X / (float)step) * step);
                int Y1 = (int)(Math.Round(pp[0].Y / (float)step) * step);
                int X2 = (int)(Math.Round(pp[pp.Count - 1].X / (float)step) * step);
                int Y2 = (int)(Math.Round(pp[pp.Count - 1].Y / (float)step) * step);

                int dX = X2 - X1;
                int dx = pp[pp.Count - 1].X - pp[0].X;
                double kX = 1;
                if (dX != 0)
                    kX = dX / (float)dx;

                int dY = Y2 - Y1;
                int dy = pp[pp.Count - 1].Y - pp[0].Y;
                double kY = 1;
                if (dY != 0)
                    kY = dY / (float)dy;

                int x1 = pp[0].X;
                int y1 = pp[0].Y;

                Point[] points = new Point[pp.Count]; // создание обычного массива точек произвольной линии
                for (int i = 0; i < pp.Count; ++i) // перенос информации из динамического массива в обычный 
                {
                    points[i].X = Math.Min(s.Width, X1 + (int)Math.Round(kX * (pp[i].X - x1)));
                    points[i].Y = Math.Min(s.Height, Y1 + (int)Math.Round(kY * (pp[i].Y - y1)));
                }
                pp.Clear();
                pp.AddRange(points);
            }
            else
            {
                int X1 = (int)(Math.Round(p1.X / (float)step) * step);
                int Y1 = (int)(Math.Round(p1.Y / (float)step) * step);
                int X2 = (int)(Math.Round(p2.X / (float)step) * step);
                int Y2 = (int)(Math.Round(p2.Y / (float)step) * step);

                if (X2 == X1)
                    p2.X -= p1.X - X1;
                else
                    p2.X = X2;
                p1.X = Math.Min(s.Width, X1);

                if (Y2 == Y1)
                    p2.Y -= p1.Y - Y1;
                else
                    p2.Y = Y2;
                p1.Y = Math.Min(s.Height, Y1);

                p2.X = Math.Min(s.Width, p2.X);
                p2.Y = Math.Min(s.Height, p2.Y);
            }
        }

        virtual public bool IsText()
        {
            return false;
        }

        public void SetColorPen(Color col)
        {
            colorPen = col;
        }

        public Color GetColorPen()
        {
            return colorPen;
        }

        public int GetWidthPen()
        {
            return widthPen;
        }

        public Color GetColorBackground()
        {
            return colorBackground;
        }

        public void SetColorBackground(Color col)
        {
            colorBackground = col;
            fillFigure = true;
        }

        public void SetWidthPen(int w)
        {
            widthPen = w;
        }

        public virtual void SetFont(Font f)
        {
        }

        public void Save()
        {
            Rectangle rr = GetRectangle();
            points = new Point[pp.Count];
            for (int i = 0; i < pp.Count; ++i)
            {
                points[i] = new Point(pp[i].X - rr.X, pp[i].Y - rr.Y);
            }
            sW = rr.Width;
            sH = rr.Height;
        }

        public void FixZoom(int marker, Rectangle rr, int dX, int dY)
        {
            switch (marker)
            {
                case 1:
                    p1 = new Point(rr.X + dX, rr.Y + dY);
                    p2 = new Point(rr.X + rr.Width, rr.Y + rr.Height);
                    break;
                case 2:
                    p1 = new Point(rr.X, rr.Y + dY);
                    p2 = new Point(rr.X + rr.Width + dX, rr.Y + rr.Height);
                    break;
                case 3:
                    p1 = new Point(rr.X, rr.Y);
                    p2 = new Point(rr.X + rr.Width + dX, rr.Y + rr.Height + dY);
                    break;
                case 4:
                    p1 = new Point(rr.X + dX, rr.Y);
                    p2 = new Point(rr.X + rr.Width, rr.Y + rr.Height + dY);
                    break;
                case 5:
                    p1 = new Point(rr.X, rr.Y + dY);
                    break;
                case 6:
                    p2 = new Point(rr.X + rr.Width + dX, rr.Y + rr.Height);
                    break;
                case 7:
                    p2 = new Point(rr.X + rr.Width, rr.Y + rr.Height + dY);
                    break;
                case 8:
                    p1 = new Point(rr.X + dX, rr.Y);
                    break;
                default:
                    break;
            }
        }

        public void Zoom(Graphics g, int x, int y, int marker, Figure cur)
        {
            Rectangle rr = GetRectangle();

            if (pp.Count != 0)
            {
                int X1 = rr.X;
                int Y1 = rr.Y;
                int X2 = rr.X + rr.Width;
                int Y2 = rr.Y + rr.Height;

                switch (marker)
                {
                    case 1:
                        X1 += cur.p2.X - X1;
                        Y1 += cur.p2.Y - Y1;
                        break;
                    case 2:
                        Y1 += cur.p2.Y - Y1;
                        X2 += cur.p2.X - X2;
                        break;
                    case 3:
                        X2 += cur.p2.X - X2;
                        Y2 += cur.p2.Y - Y2;
                        break;
                    case 4:
                        X1 += cur.p2.X - X1;
                        Y2 += cur.p2.Y - Y2;
                        break;
                    case 5:
                        Y1 += cur.p2.Y - Y1;
                        break;
                    case 6:
                        X2 += cur.p2.X - X2;
                        break;
                    case 7:
                        Y2 += cur.p2.Y - Y2;
                        break;
                    case 8:
                        X1 += cur.p2.X - X1;
                        break;
                }

                double kX = 1;
                if (sW != 0)
                    kX = (X2 - X1) / (double)sW;

                double kY = 1;
                if (sH != 0)
                    kY = (Y2 - Y1) / (double)sH;

                pp.Clear();
                for (int i = 0; i < points.Length; ++i)
                {
                    pp.Add(new Point(X1 + (int)Math.Round(kX * points[i].X), Y1 + (int)Math.Round(kY * points[i].Y)));
                }
                Draw(g, x, y);
                PaintRectangle(g, new Pen(Color.Black, 1));
            }
            else
            {
                int dX = cur.p2.X - cur.p1.X;
                int dY = cur.p2.Y - cur.p1.Y;

                Point tp1 = p1;
                Point tp2 = p2;

                FixZoom(marker, rr, dX, dY);
                Draw(g, x, y);
                PaintRectangle(g, new Pen(Color.Black, 1));

                p1 = tp1;
                p2 = tp2;
            }
        }

        public int GetMarker(int x, int y)
        {
            Rectangle r = GetRectangle();
            Rectangle rr = new Rectangle(x, y, 0, 0);

            for (int i = 0; i < 8; ++i)
                if (rr.IntersectsWith(
                    new Rectangle(
                        r.X - size_marker / 2 + dx[i] * r.Width / 2,
                        r.Y - size_marker / 2 + dy[i] * r.Height / 2,
                        size_marker, size_marker)))
                    return i + 1;
            return 0;
        }

        public void PaintRectangle(Graphics g, Pen p)
        {
            if (selected)
            {
                p.Color = Color.Black;
                p.Width = 1;
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawRectangle(p, GetRectangle());
            }
            if (selected_d)
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                Rectangle r = GetRectangle();

                for (int i = 0; i < 8; ++i)
                    g.DrawRectangle(p, 
                        new Rectangle(
                            r.X - size_marker / 2 + dx[i] * r.Width / 2,
                            r.Y - size_marker / 2 + dy[i] * r.Height / 2,
                            size_marker, size_marker));
            }
        }
    }

    [Serializable()]
    class Rect : Figure
    {
        public Rect(Point p1, Point p2, Color cP, Color cB, int w) : base(p1, p2, cP, cB, w) { }
        public override void Draw(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            if (selected) // если прямоугольник выделен, изменяем стиль пера на пунктирный
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            else
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            int x1, x2, y1, y2;
            x1 = p1.X + x;
            y1 = p1.Y + y;
            x2 = p2.X + x;
            y2 = p2.Y + y;
            norm(ref x1, ref y1, ref x2, ref y2);
            Rectangle r = Rectangle.FromLTRB(x1, y1, x2, y2);
            SolidBrush br = new SolidBrush(colorBackground);
            if (fillFigure) // проверка флага заливки
                g.FillRectangle(br, r); // заливка фона прямоугольника
            g.DrawRectangle(p, r);
        }
        public override void DrawDash(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            int x1, x2, y1, y2;
            x1 = p1.X + x;
            y1 = p1.Y + y;
            x2 = p2.X + x;
            y2 = p2.Y + y;
            norm(ref x1, ref y1, ref x2, ref y2);
            Rectangle r = Rectangle.FromLTRB(x1, y1, x2, y2);
            g.DrawRectangle(p, r);
        }
        public override Rectangle GetRectangle()
        {
            return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }
    }

    [Serializable()]
    class Ellipse : Figure
    {
        public Ellipse(Point p1, Point p2, Color cP, Color cB, int w) : base(p1, p2, cP, cB, w) { }
        public override void Draw(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            int x1, y1, x2, y2;
            x1 = p1.X + x;
            y1 = p1.Y + y;
            x2 = p2.X + x;
            y2 = p2.Y + y;
            norm(ref x1, ref y1, ref x2, ref y2);
            Rectangle r = Rectangle.FromLTRB(x1, y1, x2, y2);
            SolidBrush br = new SolidBrush(colorBackground);
            if (fillFigure) // проверка флага заливки
                g.FillEllipse(br, r); // заливка фона эллипса
            g.DrawEllipse(p, r);
        }
        public override void DrawDash(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            int x1, y1, x2, y2;
            x1 = p1.X + x;
            y1 = p1.Y + y;
            x2 = p2.X + x;
            y2 = p2.Y + y;
            norm(ref x1, ref y1, ref x2, ref y2);
            Rectangle r = Rectangle.FromLTRB(x1, y1, x2, y2);
            g.DrawEllipse(p, r);
        }
        public override Rectangle GetRectangle()
        {
            return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }
    }

    [Serializable()]
    class StraightLine : Figure
    {
        public StraightLine(Point p1, Point p2, Color cP, Color cB, int w) : base(p1, p2, cP, cB, w)
        {
        }
        public override void Draw(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            int x1, y1, x2, y2;
            x1 = p1.X + x;
            y1 = p1.Y + y;
            x2 = p2.X + x;
            y2 = p2.Y + y;
            g.DrawLine(p, x1, y1, x2, y2);
        }
        public override void DrawDash(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            int x1, y1, x2, y2;
            x1 = p1.X + x;
            y1 = p1.Y + y;
            x2 = p2.X + x;
            y2 = p2.Y + y;
            g.DrawLine(p, x1, y1, x2, y2);
        }
        public override Rectangle GetRectangle()
        {
            return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }
    }

    [Serializable()]
    class CurveLine : Figure
    {
        public CurveLine(Point p1, Point p2, Color cP, Color cB, int w) : base(p1, p2, cP, cB, w)
        {
            pp.Add(p1);
            pp.Add(p2);
        }
        public override void Draw(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            Point[] points = new Point[pp.Count]; // создание обычного массива точек произвольной линии

            for (int i = 0; i < pp.Count; ++i) // перенос информации из динамического массива в обычный 
            {
                points[i].X = pp[i].X + x;
                points[i].Y = pp[i].Y + y;
            }
            g.DrawCurve(p, points); // рисование произвольной линии
        }
        public override void DrawDash(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Point[] points = new Point[pp.Count]; // создание обычного массива точек произвольной линии

            for (int i = 0; i < pp.Count; ++i) // перенос информации из динамического массива в обычный 
            {
                points[i].X = pp[i].X + x;
                points[i].Y = pp[i].Y + y;
            }
            g.DrawCurve(p, points); // рисование произвольной линии
        }
        public override Rectangle GetRectangle()
        {
            int x_min = pp[0].X;
            int x_max = pp[0].X;
            int y_min = pp[0].Y;
            int y_max = pp[0].Y;
            for (int i = 0; i < pp.Count; ++i)
            {
                x_min = Math.Min(x_min, pp[i].X);
                x_max = Math.Max(x_max, pp[i].X);
                y_min = Math.Min(y_min, pp[i].Y);
                y_max = Math.Max(y_max, pp[i].Y);
            }
            return new Rectangle(x_min, y_min, x_max - x_min, y_max - y_min);
        }
    }

    [Serializable()]
    class Text : Figure
    {
        public Font font; // Размер и тип шрифта
        public String text; // Текст для отображения
        public Text(Point p1, Point p2, Color cP, Color cB, int w) : base(p1, p2, cP, cB, w) { }
        public override void Draw(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            int x1, x2, y1, y2;
            x1 = p1.X + x;
            y1 = p1.Y + y;
            x2 = p2.X + x;
            y2 = p2.Y + y;
            norm(ref x1, ref y1, ref x2, ref y2);
            Rectangle r = Rectangle.FromLTRB(x1, y1, x2, y2);
            SolidBrush br = new SolidBrush(colorPen);
            g.DrawString(text, font, br, r);
        }
        public override void DrawDash(Graphics g, int x, int y)
        {
            Pen p = new Pen(colorPen, widthPen);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            int x1, x2, y1, y2;
            x1 = p1.X + x;
            y1 = p1.Y + y;
            x2 = p2.X + x;
            y2 = p2.Y + y;
            norm(ref x1, ref y1, ref x2, ref y2);
            Rectangle r = Rectangle.FromLTRB(x1, y1, x2, y2);
            g.DrawRectangle(p, r);
        }
        public override Rectangle GetRectangle()
        {
            return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }
  
        public override bool IsText()
        {
            return true;
        }

        public override void SetFont(Font f)
        {
            font = f;
        }

    }
}
