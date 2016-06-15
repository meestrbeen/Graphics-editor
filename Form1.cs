using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;



namespace risovanie
{
    //Делегат для работы инструментами, которые вызываются по pictureBox1MouseMove
    delegate void oper(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc);
    //Делегат для работы инструментов, которые вызываються по pictureBox1MouseClick
    delegate void oper1(ref Graphics g, Point pos, ref Bitmap img, ref PictureBox pc, ref Point[] mas, MouseEventArgs e);
    public partial class Form1 : Form
    {
        public int CheckButton = 0;
        public Point[] p = new Point[10];//массив точек,используется во всех инструментах рисования
        public Graphics osn, temp;//поверхности рисования
        public Bitmap bm, bmtmp, bmv,bm1;//рисунки, для хранения текущего изображения
        
        public Tools t = new Tools();//екземпляр класса инструментов
        public int Kursor = 0;
        oper deleg;
        oper1 deleg2;
        Pen pen,pen1= new Pen(Brushes.White);
        Brush br;
        
        ToolTip tp = new ToolTip();
        int x =760, y = 420;


        static class NativeMethods
        {
            public static Cursor LoadCustomCursor(string path)
            {
                IntPtr hCurs = LoadCursorFromFile(path);
                if (hCurs == IntPtr.Zero) throw new Win32Exception();
                var curs = new Cursor(hCurs);
                // Note: force the cursor to own the handle so it gets released properly
                var fi = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
                fi.SetValue(curs, true);
                return curs;
            }
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            private static extern IntPtr LoadCursorFromFile(string path);
        }



        //Первичная инициализация компонентов программы
        public Form1()
        {

            InitializeComponent();
            osn = pictureBox1.CreateGraphics();
            deleg = new oper(t.Curve);
            deleg2 = new oper1(t.empty2);
            bm = new Bitmap(x, y);
            bmtmp = new Bitmap(bm);
            bmv = new Bitmap(x, y);//рисунок для хранения выделения
            p = new Point[10];

            pictureBox1.Image = bm;
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
            osn.Clear(Color.White);
            temp.Clear(Color.White);
            pictureBox1.Refresh();
            temp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            comboBox1.BeginUpdate();
            for (int i = 1; i < 11; i++)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.EndUpdate();
            t.setfont = SystemFonts.DefaultFont;
            //Установка всплывающих подсказок елементам формы
            tp.SetToolTip(button1, "Инструмент для рисования произвольных линий");
            tp.SetToolTip(button2, "Инструмент для рисования прямоугольников");
            tp.SetToolTip(button3, "Инструмент для рисования прямых линий");
            tp.SetToolTip(button5, "Инструмент для рисования элипсов");
            tp.SetToolTip(button6, "Инструмент для рисования окружностей");
            tp.SetToolTip(button7, "Инструмент для рисования квадратов");
            tp.SetToolTip(button8, "Инструмент для рисования многоугольников");
            tp.SetToolTip(button9, "Инструмнет для заливки области выбранным цветом");
            tp.SetToolTip(button4, "Выбор цвета");
            tp.SetToolTip(button10, "Инструмент для очистки указанной области");
            tp.SetToolTip(button11, "Инструмент для вставки текста в изображение");


        }

        //Выход из программы через команду главного меню
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Сохранить перед выходом?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".bmp";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(sfd.FileName);
                }
            }
            else Application.Exit();



        }

        //Команда главного меню "Открыть"
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            try
            {
                if (op.ShowDialog() == DialogResult.OK)
                {
                    bm = new Bitmap(op.FileName);
                    pictureBox1.Image = bm;
                    bmtmp = new Bitmap(bm);
                    osn = Graphics.FromImage(bm);
                    temp = Graphics.FromImage(bmtmp);

                }
            }
            catch
            {
                MessageBox.Show("Ошибка открытия", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //Команда главного меню "Новый"
        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.ShowDialog(this);
            if (p[0].X == 0) return;

            bm = new Bitmap(p[0].X, p[0].Y);
            bmtmp = new Bitmap(bm);
            p = new Point[10];

            pictureBox1.Image = bm;
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
            osn.Clear(Color.White);
            temp.Clear(Color.White);
            pictureBox1.Refresh();
            temp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            pictureBox2.Visible = false;

        }

        //Команда главного меню "Сохранить"
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".bmp";
            try
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName) == false) pictureBox1.Image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    else MessageBox.Show(" Файл с таким именем уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка сохранения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //Вызов метода записанного в делегат deleg при движении мишки над pictureBox1
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            deleg(ref temp, ref osn, e, ref p, ref bm, ref bmtmp, ref pictureBox1);
            // this.Cursor = new Cursor("pen2.cur");
            //Cursor = new Cursor(GetType(), "pen.cur");
            //this.pictureBox1.Cursor = new Cursor(GetType(), "pen.cur");
            //this.Cursor = NativeMethods.LoadCustomCursor("pen2.cur");
            //this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
           

        }

        //Запись координат мыши при начале ее движения над pictureBox1
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (CheckButton == 1 && e.Button == MouseButtons.Left && pictureBox2.Visible == true)
            {
                bm = new Bitmap(bm);
                bmtmp = new Bitmap(bm);
                pictureBox1.Image = bm;
                osn = Graphics.FromImage(bm);
                temp = Graphics.FromImage(bmtmp);

                osn.DrawImage(pictureBox2.Image, pictureBox2.Location.X - 6, pictureBox2.Location.Y + 5);
                pictureBox1.Image = bm;
                bmtmp = new Bitmap(bm);
                osn = Graphics.FromImage(bm);
                temp = Graphics.FromImage(bmtmp);
                pictureBox2.Visible = false;
               
            }
            if (e.Button == MouseButtons.Left)
            {
                p[1].X = e.X;
                p[1].Y = e.Y;
            }
            if (CheckButton == 1 && e.Button == MouseButtons.Left)
            {
                pictureBox2.Location = new Point(p[1].X + 6, p[1].Y-5);
            }
            p[0].X = e.X;
            p[0].Y = e.Y;
        }
        
        //Обработчик нажатия кнопки "Кривая"
        private void button1_Click(object sender, EventArgs e)
        {
            f1();
            Kursor = 1;
            deleg2 = new oper1(t.empty2);
            deleg = new oper(t.Curve);
            p = new Point[10];
            toolStripStatusLabel1.Text = "Кривая.Выберите цвет и толщину линии, нажмите на левую кнопку мыши и ведите по рабочей области";
        }
        //
        //Обработчик нажатия кнопки "Прямоугольник"
        private void button2_Click(object sender, EventArgs e)
        {
            f1();
            deleg2 = new oper1(t.empty2);
            if (radioButton1.Checked == true)
            {
                deleg = new oper(t.Rectangle);
                toolStripStatusLabel1.Text = "Контур прямоугольника. Нажмите на нужной части рабочей области и отведите мышь";
            }
            else
            {
                deleg = new oper(t.FillRect);
                toolStripStatusLabel1.Text = "Сплошной прямоугольник. Нажмите на нужной части рабочей области и отведите мышь";
            }
            p = new Point[10];
        }


        //Обработчик нажатия кнопки "Цвет"
        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                br = new SolidBrush(cd.Color);
                pen = new Pen(br, t.Pens.Width);
                t.Pens = pen;
                t.Col = cd.Color;
            }
            t.rec = new Rectangle();
            p = new Point[10];

            button36.BackColor = t.Col;

        }

        //Обработчик нажатия кнопки "Прямая"
        private void button3_Click(object sender, EventArgs e)
        {
            f1();
            deleg2 = new oper1(t.empty2);
            deleg = new oper(t.Line);
            p = new Point[10];
            toolStripStatusLabel1.Text = "Прямая. Выберите цвет и толщину линии, нажмите на левую кнопку мыши и ведите по рабочей области";
        }

        //Обработчик нажатия кнопки "Эллипс"
        private void button5_Click(object sender, EventArgs e)
        {
            f1();
            deleg2 = new oper1(t.empty2);
            if (radioButton1.Checked == true)
            {
                deleg = new oper(t.Ellipse);
                toolStripStatusLabel1.Text = "Контур эллипса. Нажмите на нужной части рабочей области и отведите мышь";
            }
            else
            {
                deleg = new oper(t.FillEllipse);
                toolStripStatusLabel1.Text = "Сплошной эллипс. Нажмите на нужной части рабочей области и отведите мышь";
            }
            p = new Point[10];
        }

        //Обработчик нажатия кнопки "Окружность"
        private void button6_Click(object sender, EventArgs e)
        {
            f1();
            deleg2 = new oper1(t.empty2);
            if (radioButton1.Checked == true)
            {
                deleg = new oper(t.Okrugn);
                toolStripStatusLabel1.Text = "Контур окружности. Нажмите на нужной части рабочей области и отведите мышь";
            }
            else
            {
                deleg = new oper(t.FillOkr);
                toolStripStatusLabel1.Text = "Сплошная окружность. Нажмите на нужной части рабочей области и отведите мышь";
            }
            p = new Point[10];
        }

        //Обработчик нажатия кнопки "Квадрат"
        private void button7_Click(object sender, EventArgs e)
        {
            f1();
            deleg2 = new oper1(t.empty2);
            if (radioButton1.Checked == true)
            {
                deleg = new oper(t.square);
                toolStripStatusLabel1.Text = "Контур квадрата. Нажмите на нужной части рабочей области и отведите мышь";
            }
            else
            {
                deleg = new oper(t.FillSquare);
                toolStripStatusLabel1.Text = "Сплошной квадрат. Нажмите на нужной части рабочей области и отведите мышь";
            }
            p = new Point[10];
        }

        //Обработчик нажатия кнопки "Многоугольник"
        private void button8_Click(object sender, EventArgs e)
        {
            f1();
            deleg = new oper(t.empty1);

            if (radioButton1.Checked == true)
            {
                deleg2 = new oper1(t.multiangle);
                toolStripStatusLabel1.Text = "Контур многоугольника. Нажимайте левой кнопкой мыши в нужных точках. Для завершения нажмите правую кнопку мыши";
            }
            else
            {
                deleg2 = new oper1(t.FillMultiangle);
                toolStripStatusLabel1.Text = "Сплошной многоугольник.  Нажимайте левой кнопкой мыши в нужных точках. Для завершения нажмите правую кнопку мыши. Для коррктной работы многоуголник должен быть выпуклым и иметь минимум 4 угла";
            }
            p = new Point[10];
        }

        //Обработчик нажатия кнопки "Заливка"
        private void button9_Click(object sender, EventArgs e)
        {
            f1();
            deleg = new oper(t.empty1);
            deleg2 = new oper1(t.FloodFill);
            p = new Point[10];
            toolStripStatusLabel1.Text = "Укажите точку в области, которую надо залить";
        }

        //Вызов метода записанного в делегат deleg2 при нажатии кнопок мыши на pictureBox1
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            Point point = new Point(e.X, e.Y);
            deleg2(ref osn, point, ref bm, ref pictureBox1, ref p, e);
        }

        //Обработывает выбор элемента из comboBox1
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            br = new SolidBrush(t.Pens.Color);
            pen = new Pen(br, Convert.ToInt32(comboBox1.SelectedItem.ToString()));
            t.rec = new Rectangle();
            p = new Point[10];
            t.Pens = pen;


        }

        //Команда главного меню "Очистить"
        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1();
            osn.Clear(Color.White);
            temp.Clear(Color.White);
            pictureBox1.Refresh();
            p = new Point[10];
            t.rec = new Rectangle();
            i1++;
            l2[i1] = new Bitmap(pictureBox1.Image);
            for (int i = i1 + 1; i < 10000; i++)
            {
                l2[i] = null;
            }
        }


        //Обработчик нажатия кнопки "Текст"
        private void button11_Click(object sender, EventArgs e)
        {
            f1();
            deleg2 = new oper1(t.empty2);
            deleg = new oper(t.DrawText);
            p = new Point[10];
            toolStripStatusLabel1.Text = "Отрисовка текста на рабочей области. Выделите место под текст мышью, отпустите ее, введите текст и нажмите ОК";
        }

        //Обработчик нажатия кнопки "Ластик"
        private void button10_Click(object sender, EventArgs e)
        {
            f1();
            deleg2 = new oper1(t.empty2);
            deleg = new oper(t.Rezinka);
            toolStripStatusLabel1.Text = "Инструмент для очистки части рабочей области";
            p = new Point[10];
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            f1();
            y = pictureBox1.Size.Height -100;
            x = pictureBox1.Size.Width - 100;

            //bm = new Bitmap(x, y);
            //bmtmp = new Bitmap(bm);


            //pictureBox1.Width = pictureBox1.Width + 100;
            //pictureBox1.Height = pictureBox1.Height + 100;
            bm = new Bitmap(bm, x, y);
            bmtmp = new Bitmap(bm);

            pictureBox1.Image = bm;
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
            //osn.Clear(Color.White);
            //temp.Clear(Color.White);
            i1++;
            l2[i1] = new Bitmap(pictureBox1.Image);
        }

        

        private void button15_Click(object sender, EventArgs e)//свернуть
        {
            if (groupBox1.Visible == true) groupBox1.Visible = false;
            else groupBox1.Visible = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button16.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button16.BackColor;
            button36.BackColor = t.Col;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button17.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button17.BackColor;
            button36.BackColor = t.Col;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button18.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button18.BackColor;
            button36.BackColor = t.Col;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button19.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button19.BackColor;
            button36.BackColor = t.Col;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button20.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button20.BackColor;
            button36.BackColor = t.Col;

        }

        private void button21_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button21.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button21.BackColor;
            button36.BackColor = t.Col;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button22.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button22.BackColor;
            button36.BackColor = t.Col;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button23.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button23.BackColor;
            button36.BackColor = t.Col;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button24.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button24.BackColor;
            button36.BackColor = t.Col;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button25.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button25.BackColor;
            button36.BackColor = t.Col;
        }

        private void button34_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button34.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button34.BackColor;
            button36.BackColor = t.Col;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button33.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button33.BackColor;
            button36.BackColor = t.Col;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button32.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button32.BackColor;
            button36.BackColor = t.Col;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button35.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button35.BackColor;
            button36.BackColor = t.Col;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button31.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button31.BackColor;
            button36.BackColor = t.Col;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button30.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button30.BackColor;
            button36.BackColor = t.Col;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button29.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button29.BackColor;
            button36.BackColor = t.Col;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button28.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button28.BackColor;
            button36.BackColor = t.Col;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button27.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button27.BackColor;
            button36.BackColor = t.Col;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            br = new SolidBrush(button26.BackColor);
            pen = new Pen(br, t.Pens.Width);
            t.Pens = pen;
            t.Col = button26.BackColor;
            button36.BackColor = t.Col;
        }
     
        

        private void вернутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (i1 == 0) i1++;
            //save.Load();
            //Load1();
            bm = new Bitmap(l2[i1-1]);
           i1--;
            pictureBox1.Image = bm;
            bmtmp = new Bitmap(bm);
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);

        }

        private void button12_Click(object sender, EventArgs e)
        {
            f1();

            y =pictureBox1.Size.Height + 100;
            x = pictureBox1.Size.Width + 100;

            //bm = new Bitmap(x, y);;
            //bmtmp = new Bitmap(bm);;


            //pictureBox1.Width = pictureBox1.Width + 100;;
            //pictureBox1.Height = pictureBox1.Height + 100;;
            bm = new Bitmap(bm, x, y);
            bmtmp = new Bitmap(bm);

            pictureBox1.Image = bm;
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
            //osn.Clear(Color.White);;
            //temp.Clear(Color.White);;

            //pictureBox1.Height += 70;
            //    pictureBox1.Width += 70;
            //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;


            i1++;
            l2[i1] = new Bitmap(pictureBox1.Image);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1();
            bool c1 = Clipboard.ContainsImage();
            if (c1 == true)
            {
                var c = Clipboard.GetImage();
                bm = new Bitmap(c);
                pictureBox1.Image = bm;
                bmtmp = new Bitmap(bm);
                osn = Graphics.FromImage(bm);
                temp = Graphics.FromImage(bmtmp);

                x = bm.Width;
                y = bm.Height;
            }
        }



        //Обработчик закрытия программы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Array.Clear(l2, 0, l2.Length);
                //for (int i = 0; i < 100; i++)
                //{
                //    Save();
                //}
                i1 = 0;
                e.Cancel = true;
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
       
        private void button37_Click(object sender, EventArgs e)//кнопка возврата по слоям
        {
            if (i1 == 0) i1++;
            bm = new Bitmap(l2[i1 - 1]);
            i1--;
            pictureBox1.Image = bm;
            bmtmp = new Bitmap(bm);
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            if (l2[i1 + 1] == null) i1--;
            bm = new Bitmap(l2[i1 + 1]);
            i1++;
            pictureBox1.Image = bm;
            bmtmp = new Bitmap(bm);
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
        }

        private void button39_Click(object sender, EventArgs e)
        {
            CheckButton = 1;
            deleg2 = new oper1(t.empty2);
            deleg = new oper(t.Draw);
            p = new Point[10];
            
      }
        String Back;
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            i1++;
            l2[i1] = new Bitmap(pictureBox1.Image);
            for (int i = i1 + 1; i < 10000; i++)
            {
                l2[i] = null;
            }
            if (CheckButton == 1 && e.Button == MouseButtons.Left && pictureBox2.Visible== false && p[1].X != e.X && p[1].Y != e.Y)
            {
                pictureBox1.Image = bm;
                bmtmp = new Bitmap(bm);
                osn = Graphics.FromImage(bm);
                temp = Graphics.FromImage(bmtmp);

                bool c1 = Clipboard.ContainsImage();
                if (c1 == true)
                {
                    var c = Clipboard.GetImage();
                    bm1 = new Bitmap(c);
                    pictureBox2.Image = bm1;
                }
                osn.FillRectangle(this.pen1.Brush, pictureBox2.Location.X - 5, pictureBox2.Location.Y + 5, pictureBox2.Size.Width-3, pictureBox2.Size.Height-2);
                pictureBox1.Image = bm;
                bmtmp = new Bitmap(bm);
                osn = Graphics.FromImage(bm);
                temp = Graphics.FromImage(bmtmp);
                pictureBox2.Visible = true;
            }

            //if (CheckButton == 1 && e.Button == MouseButtons.Left && pictureBox2.Visible ==true )
            //{
            //    osn.DrawImage(pictureBox2.Image, pictureBox2.Location.X - 6, pictureBox2.Location.Y + 5);
            //    pictureBox1.Image = bm;
            //    bmtmp = new Bitmap(bm);
            //    osn = Graphics.FromImage(bm);
            //    temp = Graphics.FromImage(bmtmp);
            //    pictureBox2.Visible = false;

            //}
            try
            { 
                Back = "C:/backup.bmp";
                l2[i1].Save(Back, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            catch
            {  
            }
            try
            {
                Back = "E:/backup.bmp";
                l2[i1].Save(Back, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            catch
            {
            }
            try
            {
                Back = "D:/backup.bmp";
                l2[i1].Save(Back, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            catch
            {
            }
            try
            {
                Back = "backup.bmp";
                l2[i1].Save(Back, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            catch
            {
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        //    if (pictureBox2.Visible == true)
        //    {
        //        osn.DrawImage(pictureBox2.Image, pictureBox2.Location.X - 6, pictureBox2.Location.Y + 5);
        //        pictureBox1.Image = bm;
        //        bmtmp = new Bitmap(bm);
        //        osn = Graphics.FromImage(bm);
        //        temp = Graphics.FromImage(bmtmp);
        //        CheckButton = 0;
        //        pictureBox2.Visible = false;
        //    }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            l2[i1] = new Bitmap(pictureBox1.Image);
        }

        bool status = false;
        Point start,finish;

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
           
            start = e.Location;
            status = true;
            
            //pictureBox2.Location = new Point((Cursor.Position.X - this.Location.X), (Cursor.Position.Y - this.Location.Y));
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            
            finish =pictureBox2.Location;
            status = false;
            bm1 = new Bitmap(pictureBox2.Image);

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }

        private void открытьПоследнееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bm = new Bitmap(Back);       
            pictureBox1.Image = bm;
            bm = new Bitmap(pictureBox1.Image);
            bmtmp = new Bitmap(bm);
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
           
            ///
        }

        Tools t1 = new Tools();

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckButton == 1 && pictureBox2.Visible == true)
            {
                Clipboard.SetImage(pictureBox2.Image);
               
            }
        }
        //
        private void button14_Click(object sender, EventArgs e)
        {
            bm = new Bitmap(bm);
            bmtmp = new Bitmap(bm);
            pictureBox1.Image = bm;
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
            if (CheckButton == 1 && pictureBox2.Visible == true)
            {            
                osn.DrawImage(pictureBox2.Image, pictureBox2.Location.X - 6, pictureBox2.Location.Y + 5);
                pictureBox1.Image = bm;
                bmtmp = new Bitmap(bm);
                osn = Graphics.FromImage(bm);
                temp = Graphics.FromImage(bmtmp);
                
            }
        }
        //
        private void button40_Click(object sender, EventArgs e)
        {
            if (CheckButton == 1 && pictureBox2.Visible == true)
            {
                Clipboard.SetImage(pictureBox2.Image);
                pictureBox1.Image = bm;
                bmtmp = new Bitmap(bm);
                osn = Graphics.FromImage(bm);
                temp = Graphics.FromImage(bmtmp);
                pictureBox2.Visible = false;
                
            }

        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (status == true) //Если "перемещение включено"
            {
                int v1, v2;
                v1 = pictureBox2.Location.X - (start.X-e.X);
                v2 = pictureBox2.Location.Y - (start.Y - e.Y);
                // pictureBox2.Location = new Point((Cursor.Position.X - this.Location.X-200), (Cursor.Position.Y - this.Location.Y-200));
                 pictureBox2.Location = new Point(v1, v2); //Задаем координаты pictureBox1 равные координатам курсора, с поправкой на расположение формы
                //pictureBox2.Location = e.Location;
                //pictureBox2.Location.X =- start.X- e.X;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

   

        //Команда главного меню "Помощь"
        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            try
            {
                MessageBox.Show("Графический редактор\nВерсия 1.0\n© Замт.Пасынков, 2016. Все права защищены.", "О Программе", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("Файл справки поврежден либо отсутствует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public int i1 = 0;
        
        //public int[] l1 = new int[100];
        public Bitmap[] l2 = new Bitmap[100000];
       

        public void f1()
        {
            bm = new Bitmap(bm);
            bmtmp = new Bitmap(bm);
            pictureBox1.Image = bm;
            osn = Graphics.FromImage(bm);
            temp = Graphics.FromImage(bmtmp);
            if (CheckButton == 1  && pictureBox2.Visible == true)
            {
                osn.DrawImage(pictureBox2.Image, pictureBox2.Location.X - 6, pictureBox2.Location.Y + 5);
                pictureBox1.Image = bm;
                bmtmp = new Bitmap(bm);
                osn = Graphics.FromImage(bm);
                temp = Graphics.FromImage(bmtmp);
                pictureBox2.Visible = false;
                CheckButton = 0;
            }
            pictureBox2.Visible = false;
            CheckButton = 0;
            
        }
        //public void Save()
        //{
        //    l2[i1] = bm;
        //    i1++;
        //    BinaryFormatter bf = new BinaryFormatter();
        //    FileStream file = File.Create("/doc.dat");

        //    PlayerData data = new PlayerData();
        //    data.l1 = l1;
        //    data.l2 = l2;

        //    bf.Serialize(file, data);
        //    file.Close();
        //}

        //public void Load1()
        //{
        //    if (File.Exists("/doc.dat"))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        FileStream file = File.Open("/doc.dat", FileMode.Open);
        //        PlayerData data = (PlayerData)bf.Deserialize(file);
        //        file.Close();

        //        l1 = data.l1;
        //        l2 = data.l2;
        //        bm = l2[i1];
        //        i1--;
        //    }
        //}

    }
}

//[Serializable]
//class PlayerData
//{
//    public int[] l1;
//    public Bitmap[] l2 = new Bitmap[100];
//}








