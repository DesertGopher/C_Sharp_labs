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
    public partial class FormSize : Form
    {
        public Size sizeForm;
        Font CustomFont;
        public FormSize()
        {
            if (sizeForm.Width == 320 && sizeForm.Height == 240)
            {
                radioButton1.Checked = true;
            }
            if (sizeForm.Width == 600 && sizeForm.Height == 480)
            {
                radioButton2.Checked = true;
            }
            if (sizeForm.Width == 800 && sizeForm.Height == 600)
            {
                radioButton3.Checked = true;
            }
            InitializeComponent();
            MyFont();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                int w, h;
                if (!int.TryParse(textBox1.Text, out w))
                    w = 320;
                if (!int.TryParse(textBox2.Text, out h))
                    h = 240;
                sizeForm.Width = w;
                sizeForm.Height = h;
            }
            else
            {
                if (radioButton1.Checked)
                {
                    sizeForm.Width = 320;
                    sizeForm.Height = 240;
                }
                if (radioButton2.Checked)
                {
                    sizeForm.Width = 640;
                    sizeForm.Height = 480;
                }
                if (radioButton3.Checked)
                {
                    sizeForm.Width = 800;
                    sizeForm.Height = 600;
                }
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b') return;
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                if (textBox2.Text.Length > 3)
                {
                    textBox2.Text = textBox2.Text.Substring(0, 4);
                    e.KeyChar = '\0';
                    return;
                }
            }
            else e.KeyChar = '\0';
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b') return;
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                if (textBox2.Text.Length > 3)
                {
                    textBox2.Text = textBox2.Text.Substring(0, 4);
                    e.KeyChar = '\0';
                    return;
                }
            }
            else e.KeyChar = '\0';
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox1.Checked;
            textBox2.Enabled = checkBox1.Checked;
            groupBox1.Enabled = !checkBox1.Checked;
        }

        private void PictureSize_Load(object sender, EventArgs e)
        {

        }

        private void MyFont()
        {
            PrivateFontCollection my_font = new PrivateFontCollection();
            my_font.AddFontFile("MyFont.ttf");
            CustomFont = new Font(my_font.Families[0], 12);
        }

        private void FormSize_Load(object sender, EventArgs e)
        {
            label1.Font = CustomFont;
            label2.Font = CustomFont;
            checkBox1.Font = CustomFont;
            radioButton1.Font = CustomFont;
            radioButton2.Font = CustomFont;
            radioButton3.Font = CustomFont;
            //groupBox1.Font = CustomFont;
            button1.Font = CustomFont;
            button2.Font = CustomFont;
            textBox1.Font = CustomFont;
            textBox2.Font = CustomFont;
        }
    }
}
