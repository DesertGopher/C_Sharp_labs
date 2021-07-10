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
    public partial class PenSz : Form
    {
        string ch1;
        public int ch2;
        public PenSz()
        {
            InitializeComponent();
            int[] mass = new int[] { 1, 2, 5, 8, 10, 12, 15 };
            int i = 0;
            while (i < mass.Length)
            { comboBox1.Items.Add(mass[i]); i++; }      //Добавляем элемент в список позиций      
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ch1 = comboBox1.Text;
            ch2 = Convert.ToInt32(ch1);//Преобразовываем текстовое  значение этого поля в целочисленное
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void PenSz_Load(object sender, EventArgs e)
        {

        }
    }

}
