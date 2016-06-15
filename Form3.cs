using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace risovanie
{
    public partial class Form3 : Form
    {
        Rectangle rec;
        string[] str;
        Font[] f;
        
        //Установка размеров richTextBox1 по данным переданным в качестве параметров, а также получение 
        //ссылок на шрифт и строку
        public Form3(ref Rectangle pr,ref string[] stroka,ref Font[] font)
        {
            
            InitializeComponent();
            rec = pr;
            str = stroka;
            f = font;
            richTextBox1.Width = rec.Width;
            richTextBox1.Height = rec.Height;
            richTextBox1.Font = f[0];
            
        }

        //Выбор шрифта
        private void button2_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                //Передача в метод отрисовки текста выбранного шрифта
                f[0] = fd.Font;

                richTextBox1.Font = fd.Font;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Передача в метод отрисовки текста, текста из richTextBox1
            str[0] = richTextBox1.Text;
            
            Form.ActiveForm.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
