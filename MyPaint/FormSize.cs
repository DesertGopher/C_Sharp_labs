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
    public partial class FormSize : Form
    {
        public int height, width;
        public FormSize()
        {
            InitializeComponent();
            checkBox1.Checked = false;//по умолчанию не установлен флажок выбора ввода вручную           
            TextEnFalse();//по умолчанию  выбираем из предложенного
        }
        private void TextEnFalse() //если выбираем из предложенного
        {
            label1.Enabled = false; label2.Enabled = false; //"ширина" и "высота" не доступны для пользователя
            textBox1.Enabled = false; textBox2.Enabled = false; //поля для ручного ввода ширины и высоты не доступны для пользователя
            radioButton1.Enabled = true; radioButton2.Enabled = true; radioButton3.Enabled = true; //флажки выбора размера доступны для пользователя
            button1.Enabled = true; //кнопка "ОК" доступна для нажатия
        }
        private void TextEnTrue() //если вводим вручную
        {
            label1.Enabled = true; label2.Enabled = true;
            textBox1.Enabled = true; textBox2.Enabled = true; //поля для ручного ввода ширины и высоты доступны для пользователя
            radioButton1.Enabled = false; radioButton2.Enabled = false; radioButton3.Enabled = false; //флажки выбора размера не доступны для пользователя
            if (textBox1.Text != "" && textBox2.Text != "") button1.Enabled = true; //если что-то введено, то доступна кнопка "ОК"
            else button1.Enabled = false; //если ничего не введено, то не доступна кнопка "ОК"
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) TextEnTrue();//если установлен флажок выбора ввода вручную   
            else TextEnFalse();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)//если установлен флажок выбора ввода вручную  
            {
                try
                {
                    width = Convert.ToInt32(textBox1.Text);//устанавливаем ширину, равной введённому значению
                    height = Convert.ToInt32(textBox2.Text);//устанавливаем высоту, равной введённому значению
                    this.Close();
                }
                catch (Exception ex) //при возникновении исключений
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            else //если не установлен флажок выбора ввода вручную 
            {
                if (radioButton1.Checked) { width = 320; height = 240; }
                if (radioButton2.Checked) { width = 640; height = 480; }
                if (radioButton3.Checked) { width = 800; height = 600; }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextEnTrue();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextEnTrue();
        }

        private static void ProverkaText(KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != '\b')//проверяем, является ли введённое значение цифрой или нет
            {
                e.Handled = true;// обход обработки элемента по умолчанию
                return;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ProverkaText(e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            ProverkaText(e);
        }

        private void FormSize_Load(object sender, EventArgs e)
        {

        }
    }
}
