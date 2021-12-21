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
    public partial class GridStepDialog : Form
    {
        Font CustomFont;

        public int gridStep = 10;
        public GridStepDialog()
        {
            InitializeComponent();
            MyFont();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gridStep = (int)numericUpDown1.Value;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void MyFont()
        {
            PrivateFontCollection my_font = new PrivateFontCollection();
            my_font.AddFontFile("MyFont.ttf");
            CustomFont = new Font(my_font.Families[0], 12);
        }

        private void GridStepDialog_Load(object sender, EventArgs e)
        { 
            button1.Font = CustomFont;
            button2.Font = CustomFont;
            numericUpDown1.Font = CustomFont;
        }
    }
}
