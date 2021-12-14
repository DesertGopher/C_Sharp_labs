using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;

namespace MyPaint
{
    public partial class PenSz : Form
    {
        Font CustomFont;
        public PenSz()
        {
            InitializeComponent();
            MyFont();
            button1.Font = CustomFont;
            button2.Font = CustomFont;
            comboBox1.Font = CustomFont;
        }

        private void button1_Click(object sender, EventArgs e) // метод-обработчик нажатия на кнопку "OK"
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e) // метод-обработчик нажатия на кнопку "Cancel"
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        public int getWidth() // метод для получения выбранной пользователем толщины линии
        {
            int number;
            if (int.TryParse(comboBox1.Text, out number))
                return number;
            else
                return 1;
        }

        private void MyFont()
        {
            PrivateFontCollection my_font = new PrivateFontCollection();
            my_font.AddFontFile("MyFont.ttf");
            CustomFont = new Font(my_font.Families[0], 12);
        }
    }
}
