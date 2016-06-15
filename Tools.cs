using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace risovanie
{
    //Класс в котором хранятся все методы рисования
    public class Tools
    {
        Pen pen, pen1,pen3;
        Color col;
        public Font[] fon;
        public Rectangle rec;
        public string[] text;
        public Bitmap bm1;
        

        //Установка начальных значений полям класса
        public Tools()
        {
            pen = new Pen(Color.Black);
            pen3 = new Pen(Color.Black);
            pen1 = new Pen(Color.White, 10);
            col = Color.Black;
            rec = new Rectangle();
            this.text = new string[1];
            this.text[0] = "";
            this.fon = new Font[1];


        }


        #region  Свойства для доступа к нужным полям класса
        public Pen Pens
        {
            get { return this.pen; }
            set { this.pen = value; }
        }

        public Color Col
        {
            get { return this.col; }
            set { this.col = value; }
        }



        public Font setfont
        {
            set { this.fon[0] = value; }
        }
        #endregion

        #region Пустые методы, для блокировки делегатов deleg и deleg2
        public void empty1(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {
            return;
        }

        public void empty2(ref Graphics g, Point pos, ref Bitmap img, ref PictureBox pc, ref Point[] mas, MouseEventArgs e)
        {
            return;
        }
        #endregion

        #region Методы рисования на pictureBox
        /// <summary>
        /// Метод для рисования кривой
        /// </summary>
        public void Curve(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {
            pc.Image = osn;
            pc.Refresh();
            if (e.Button == MouseButtons.Left)
            {
                gosn.FillEllipse(this.pen.Brush, mas[0].X - this.pen.Width / 2, mas[0].Y - this.pen.Width / 2, this.pen.Width, this.pen.Width);
                gosn.DrawLine(this.pen, mas[0].X, mas[0].Y, e.X, e.Y);
                mas[0].X = e.X; mas[0].Y = e.Y;
                pc.Refresh();
            }
        }

        /// <summary>
        /// Метод для рисования контура прямоугольника
        /// </summary>

        public void Rectangle(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (mas[1].X > e.X && mas[1].Y > e.Y)
                {
                    rec = new Rectangle(e.X, e.Y, mas[1].X - e.X, mas[1].Y - e.Y);
                }
                if (e.X > mas[1].X && e.Y > mas[1].Y)
                {
                    rec = new Rectangle(mas[1].X, mas[1].Y, e.X - mas[1].X, e.Y - mas[1].Y);
                }

                if (mas[1].X > e.X && e.Y > mas[1].Y)
                {
                    rec = new Rectangle(e.X, mas[1].Y, mas[1].X - e.X, e.Y - mas[1].Y);
                }
                if (e.X > mas[1].X && mas[1].Y > e.Y)
                {
                    rec = new Rectangle(mas[1].X, e.Y, e.X - mas[1].X, mas[1].Y - e.Y);
                }
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.DrawRectangle(this.pen, rec);

                pc.Refresh();
            }
            else
            {
                pc.Image = osn;
                gosn.DrawRectangle(this.pen, rec);
                pc.Refresh();
                gtemp.Clear(Color.White);
                mas = new Point[10];
                rec = new Rectangle();
            }
        }

        /// <summary>
        /// Метод для рисования прямой
        /// </summary>

        public void Line(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {

                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.DrawLine(this.pen, mas[1].X, mas[1].Y, e.X, e.Y);

                mas[2].X = e.X; mas[2].Y = e.Y;
                pc.Refresh();

            }
            else
            {

                pc.Image = osn;
                if (mas[2].X != 0 || mas[2].Y != 0) gosn.DrawLine(this.pen, mas[1].X, mas[1].Y, mas[2].X, mas[2].Y);

                pc.Refresh();
                gtemp.Clear(Color.White);
                mas = new Point[10];


            }
        }

        /// <summary>
        /// Метод для рисования контура эллипса
        /// </summary>

        public void Ellipse(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.DrawEllipse(this.pen, mas[1].X, mas[1].Y, e.X - mas[1].X, e.Y - mas[1].Y);
                mas[2].X = e.X; mas[2].Y = e.Y;
                pc.Refresh();
            }
            else
            {
                pc.Image = osn;
                if (mas[2].X != 0 && mas[2].Y != 0) gosn.DrawEllipse(this.pen, mas[1].X, mas[1].Y, mas[2].X - mas[1].X, mas[2].Y - mas[1].Y);
                pc.Refresh();
                gtemp.Clear(Color.White);
                mas = new Point[10];
            }
        }

        /// <summary>
        /// Метод для рисования окружности
        /// </summary>

        public void Okrugn(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.DrawEllipse(this.pen, mas[1].X, mas[1].Y, e.X - mas[1].X, e.X - mas[1].X);
                mas[2].X = e.X; mas[2].Y = e.Y;
                pc.Refresh();
            }
            else
            {
                pc.Image = osn;
                if (mas[2].X != 0 && mas[2].Y != 0) gosn.DrawEllipse(this.pen, mas[1].X, mas[1].Y, mas[2].X - mas[1].X, mas[2].X - mas[1].X);
                pc.Refresh();
                gtemp.Clear(Color.White);
                mas = new Point[10];
            }
        }

        /// <summary>
        /// Метод для рисования контура квадрата
        /// </summary>

        public void square(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {


            if (e.Button == MouseButtons.Left)
            {

                rec = new Rectangle(mas[1].X, mas[1].Y, e.X - mas[1].X, e.X - mas[1].X);
                //if (mas[1].X > e.X && mas[1].Y > e.Y)
                //{
                //    rec = new Rectangle(e.X, e.Y, mas[1].X - e.X, mas[1].Y - e.Y);
                //}      
                //if(e.X > mas[1].X && e.Y > mas[1].Y)
                //{
                //    rec = new Rectangle(mas[1].X, mas[1].Y, e.X - mas[1].X, e.Y - mas[1].Y);
                //}

                //if (mas[1].X > e.X && e.Y > mas[1].Y)
                //{
                //    rec = new Rectangle(e.X, mas[1].Y, mas[1].X - e.X, e.Y - mas[1].Y);
                //}
                //if (e.X > mas[1].X && mas[1].Y > e.Y)
                //{
                //    rec = new Rectangle(mas[1].X, e.Y, e.X - mas[1].X, mas[1].Y - e.Y);
                //}
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.DrawRectangle(this.pen, rec);

                pc.Refresh();
            }
            else
            {
                pc.Image = osn;
                gosn.DrawRectangle(this.pen, rec);
                pc.Refresh();
                gtemp.Clear(Color.White);
                rec = new Rectangle();
                mas = new Point[10];
            }
        }

        /// <summary>
        /// Метод для рисования контура многоугольника
        /// </summary>

        public void multiangle(ref Graphics g, Point pos, ref Bitmap img, ref PictureBox pc, ref Point[] mas, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (mas[4].X == 0)
                {
                    mas[4].X++;
                    mas[2].X = e.X;
                    mas[2].Y = e.Y;
                    mas[3].X = e.X;
                    mas[3].Y = e.Y;
                    return;
                }
                else
                {
                    g.DrawLine(this.pen, mas[3].X, mas[3].Y, e.X, e.Y);
                    mas[3].X = e.X;
                    mas[3].Y = e.Y;
                    pc.Refresh();
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                mas[4].X = 0;
                g.DrawLine(this.pen, mas[2].X, mas[2].Y, mas[3].X, mas[3].Y);
                pc.Refresh();
                mas = new Point[10];

            }
        }

        /// <summary>
        /// Метод для заливки указанной области указанным цветом
        /// </summary>

        public void FloodFill(ref Graphics g, Point pos, ref Bitmap img, ref PictureBox pc, ref Point[] mas, MouseEventArgs e)
        {
            MapFill mf = new MapFill();
            mf.Fill(g, pos, this.col, ref img);
            pc.Image = img;
            g = Graphics.FromImage(img);
            pc.Refresh();
        }

        /// <summary>
        /// Метод для рисования закрашенного прямоугольника
        /// </summary>

        public void FillRect(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mas[1].X > e.X && mas[1].Y > e.Y)
                {
                    rec = new Rectangle(e.X, e.Y, mas[1].X - e.X, mas[1].Y - e.Y);
                }
                if (e.X > mas[1].X && e.Y > mas[1].Y)
                {
                    rec = new Rectangle(mas[1].X, mas[1].Y, e.X - mas[1].X, e.Y - mas[1].Y);
                }

                if (mas[1].X > e.X && e.Y > mas[1].Y)
                {
                    rec = new Rectangle(e.X, mas[1].Y, mas[1].X - e.X, e.Y - mas[1].Y);
                }
                if (e.X > mas[1].X && mas[1].Y > e.Y)
                {
                    rec = new Rectangle(mas[1].X, e.Y, e.X - mas[1].X, mas[1].Y - e.Y);
                }
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.FillRectangle(this.pen.Brush, rec);

                pc.Refresh();
            }
            else
            {
                pc.Image = osn;
                gosn.FillRectangle(this.pen.Brush, rec);
                pc.Refresh();
                gtemp.Clear(Color.White);
                mas = new Point[10];
                rec = new Rectangle();
            }
        }

        /// <summary>
        /// Метод для рисования закрашенного квадрата
        /// </summary>

        public void FillSquare(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (mas[1].X > e.X || mas[1].Y > e.Y)
                {
                    rec = new Rectangle(e.X, e.X, mas[1].X - e.X, mas[1].X - e.X);
                }
                else
                {
                    rec = new Rectangle(mas[1].X, mas[1].Y, e.X - mas[1].X, e.X - mas[1].X);
                }
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.FillRectangle(this.pen.Brush, rec);

                pc.Refresh();
            }
            else
            {
                pc.Image = osn;
                gosn.FillRectangle(this.pen.Brush, rec);
                pc.Refresh();
                gtemp.Clear(Color.White);
                mas = new Point[10];
                rec = new Rectangle();
            }
        }

        /// <summary>
        /// Метод для рисования круга
        /// </summary>

        public void FillOkr(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.FillEllipse(this.pen.Brush, mas[1].X, mas[1].Y, e.X - mas[1].X, e.X - mas[1].X);
                mas[2].X = e.X; mas[2].Y = e.Y;
                pc.Refresh();
            }
            else
            {
                pc.Image = osn;
                if (mas[2].X != 0 && mas[2].Y != 0) gosn.FillEllipse(this.pen.Brush, mas[1].X, mas[1].Y, mas[2].X - mas[1].X, mas[2].X - mas[1].X);
                pc.Refresh();
                gtemp.Clear(Color.White);
                mas = new Point[10];
            }
        }

        /// <summary>
        /// Метод для рисования закрашенного эллипса
        /// </summary>

        public void FillEllipse(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.FillEllipse(this.pen.Brush, mas[1].X, mas[1].Y, e.X - mas[1].X, e.Y - mas[1].Y);
                mas[2].X = e.X; mas[2].Y = e.Y;
                pc.Refresh();
            }
            else
            {
                pc.Image = osn;
                if (mas[2].X != 0 && mas[2].Y != 0) gosn.FillEllipse(this.pen.Brush, mas[1].X, mas[1].Y, mas[2].X - mas[1].X, mas[2].Y - mas[1].Y);
                pc.Refresh();
                gtemp.Clear(Color.White);
                mas = new Point[10];
            }
        }

        /// <summary>
        /// Метод для рисования закрашенного многоугольника
        /// </summary>

        public void FillMultiangle(ref Graphics g, Point pos, ref Bitmap img, ref PictureBox pc, ref Point[] mas, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mas[5].X++;
                if (mas[5].X == 3)
                {
                    if (e.X > mas[2].X || e.Y > mas[2].Y)
                    {
                        mas[6].X = mas[2].X + (e.X - mas[2].X) / 2;
                        mas[6].Y = mas[2].Y + (e.Y - mas[2].Y) / 2;
                    }
                    else
                    {
                        mas[6].X = e.X + (mas[2].X - e.X) / 2;
                        mas[6].Y = e.Y + (mas[2].Y - e.Y) / 2;
                    }
                }
                if (mas[4].X == 0)
                {
                    mas[4].X++;
                    mas[2].X = e.X;
                    mas[2].Y = e.Y;
                    mas[3].X = e.X;
                    mas[3].Y = e.Y;
                    return;
                }
                else
                {
                    g.DrawLine(this.pen, mas[3].X, mas[3].Y, e.X, e.Y);
                    mas[3].X = e.X;
                    mas[3].Y = e.Y;
                    pc.Refresh();
                }
                mas[7].X++;
            }
            if (e.Button == MouseButtons.Right)
            {

                if (mas[7].X < 3)
                {
                    MessageBox.Show("Необходимо указать минимум четыре точки", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                g.DrawLine(this.pen, mas[2].X, mas[2].Y, mas[3].X, mas[3].Y);

                this.FloodFill(ref g, mas[6], ref img, ref pc, ref mas, e);
                pc.Refresh();
                mas = new Point[10];

            }
        }

        /// <summary>
        /// Метод для рисования текста
        /// </summary>

        public void DrawText(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (mas[1].X > e.X || mas[1].Y > e.Y)
                {
                    rec = new Rectangle(e.X, e.Y, mas[1].X - e.X, mas[1].Y - e.Y);
                }
                else
                {
                    rec = new Rectangle(mas[1].X, mas[1].Y, e.X - mas[1].X, e.Y - mas[1].Y);
                }
                temp = new Bitmap(osn);
                gtemp = Graphics.FromImage(temp);
                pc.Image = temp;
                gtemp.DrawRectangle(this.pen, rec);


                mas[5].X = 1;
                pc.Refresh();

            }
            else
            {

                if (mas[5].X == 1)
                {
                    mas[5].X = 0;
                    Form3 frm3 = new Form3(ref this.rec, ref this.text, ref this.fon);
                    frm3.ShowDialog();
                    pc.Image = osn;
                    gosn.DrawString(this.text[0], this.fon[0], this.pen.Brush, this.rec);
                    pc.Refresh();
                    gtemp.Clear(Color.White);
                    mas = new Point[10];
                    this.rec = new Rectangle();
                }

            }
        }

        public void Draw(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {
            if (mas[1].X - e.X != 0 && mas[1].Y - e.Y != 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (mas[1].X > e.X && mas[1].Y > e.Y)
                    {
                        rec = new Rectangle(e.X, e.Y, mas[1].X - e.X, mas[1].Y - e.Y);
                    }
                    if (e.X > mas[1].X && e.Y > mas[1].Y)
                    {
                        rec = new Rectangle(mas[1].X, mas[1].Y, e.X - mas[1].X, e.Y - mas[1].Y);
                    }

                    if (mas[1].X > e.X && e.Y > mas[1].Y)
                    {
                        rec = new Rectangle(e.X, mas[1].Y, mas[1].X - e.X, e.Y - mas[1].Y);
                    }
                    if (e.X > mas[1].X && mas[1].Y > e.Y)
                    {
                        rec = new Rectangle(mas[1].X, e.Y, e.X - mas[1].X, mas[1].Y - e.Y);
                    }

                    temp = new Bitmap(osn);
                    gtemp = Graphics.FromImage(temp);
                    pc.Image = temp;
                    bm1 = new Bitmap(osn.Clone(rec, osn.PixelFormat));
                    Clipboard.SetImage(bm1);
                    gtemp.DrawRectangle(this.pen3, rec);
                    
                    mas[5].X = 1;
                    pc.Refresh();

                }
                else
                {
                    if (mas[5].X == 1)
                    {
                        mas[5].X = 0;
                       // bm1 = new Bitmap(osn.Clone(rec, osn.PixelFormat));
                        pc.Refresh();
                        gtemp.Clear(Color.White);
                        mas = new Point[10];
                        rec = new Rectangle();
                       // Clipboard.SetImage(bm1);
                    }
                }
            }

        }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Файл справки поврежден либо отсутствует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        //{

        //    if (mas[5].X == 1)
        //    {
        //        mas[5].X = 0;
        //        //Form3 frm3 = new Form3(ref this.rec, ref this.text, ref this.fon);
        //        //frm3.ShowDialog();
        //        pc.Image = osn;
        //        //gosn.DrawString(this.text[0], this.fon[0], this.pen.Brush, this.rec);
        //        pc.Refresh();
        //        gtemp.Clear(Color.White);
        //        mas = new Point[10];
        //        this.rec = new Rectangle();
        //    }

        // }



        /// <summary>
        /// Метод для зарисовывания указанной области белым цветом
        /// </summary>

        public void Rezinka(ref Graphics gtemp, ref Graphics gosn, MouseEventArgs e, ref Point[] mas, ref Bitmap osn, ref Bitmap temp, ref PictureBox pc)
        {

            if (e.Button == MouseButtons.Left)
            {
                gosn.FillEllipse(this.pen1.Brush, mas[0].X - this.pen1.Width / 2, mas[0].Y - this.pen1.Width / 2, this.pen1.Width, this.pen1.Width);
                gosn.DrawLine(this.pen1, mas[0].X, mas[0].Y, e.X, e.Y);
                mas[0].X = e.X; mas[0].Y = e.Y;

                pc.Refresh();
            }
        }



        #endregion
    }
}


