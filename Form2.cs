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
    public partial class Form2 : Form
    {
        ToolTip tt = new ToolTip();
        public Form2()
        {
            InitializeComponent();
            //Установка подсказки пользователю
            tt.SetToolTip(textBox1, "Не больше 5000");
            tt.SetToolTip(textBox2, "Не больше 5000");
        }

        //Передача в главную форму размеров нового изображения
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 frm1 = this.Owner as Form1;
            try
            {
                int x = Convert.ToInt16(textBox1.Text);
                int y = Convert.ToInt16(textBox2.Text);
                if ((x > 0 && y > 0)&&(x<5001&&y<5001))
                {
                    frm1.p[0].X = x;
                    frm1.p[0].Y = y;
                    Form.ActiveForm.Close();
                }
                else
                {
                    MessageBox.Show("Введены неправильные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Введены неправильные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
