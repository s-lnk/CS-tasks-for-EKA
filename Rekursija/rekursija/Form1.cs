using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rekursija
{
    public partial class Form1 : Form
    {
        int DRAW_TYPE = 1; //flag of what to draw
        int RECRS_DEEP; //recursion stepping
        Rectangle rectangle;
        int INIT_SIZE; //initial size
        int BASELINE; //gravity for initial drawing
        int SQ_ANGLE; //rotation angle for square
        int SQ_CENTER; //coordinate of center for square

        public Form1()
        {
            InitializeComponent();
            this.Text = "Rekursija by VG";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Refresh(); //Clear panel from previous data and reset global values...
            RECRS_DEEP = 0;
            INIT_SIZE = 200;
            BASELINE = 100;
            SQ_ANGLE = 0;
            SQ_CENTER = 200;
            try
            {
                RECRS_DEEP = Convert.ToInt32(textBox1.Text); //get numerical value
            } catch (Exception)
            {
                RECRS_DEEP = 1;
            }
            switch (DRAW_TYPE) {
                case 1:
                    DrawCircles(panel1.CreateGraphics(), 1, RECRS_DEEP, INIT_SIZE, 0, 0);
                    break;
                case 2:
                    DrawSquares(panel1.CreateGraphics(), 1, RECRS_DEEP, INIT_SIZE, 0, 0);
                    break;
                }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) { DRAW_TYPE = 1; }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) { DRAW_TYPE = 2; }
        }


        private void DrawCircles(Graphics graphics, int from, int n, int size, int x, int y)
        {
            System.Console.WriteLine("Start drawing circle " + from + " " + n + " " + size );
            if ((from - 1) == n)
            {
                System.Console.WriteLine("Recursion complete");
                return;
            } else
            {
                for (int i = 0; i < from; i++ ) {
                    //Draw
                    rectangle.X = x + BASELINE;
                    rectangle.Y = y + BASELINE;
                    rectangle.Size = new System.Drawing.Size(size, size);
                    using (Pen pen = new Pen(Color.Black, 1))
                        graphics.DrawEllipse(pen, rectangle);
                }
                from++;
                size = Convert.ToInt32(size / 2);
                
                DrawCircles(graphics, from, n, size, x + Convert.ToInt32(size / 2), y - Convert.ToInt32(size / 2)); //TOP
                DrawCircles(graphics, from, n, size, x + Convert.ToInt32(size * 1.5), y + Convert.ToInt32(size / 2)); //RIGHT
                DrawCircles(graphics, from, n, size, x + Convert.ToInt32(size / 2), y + Convert.ToInt32(size * 1.5)); //BOTTOM
                DrawCircles(graphics, from, n, size, x - Convert.ToInt32(size / 2), y + Convert.ToInt32(size / 2)); //LEFT
            }


        }

        private void DrawSquares(Graphics graphics, int from, int n, int size, int x, int y)
        {
            if ((from - 1) == n)
            {
                System.Console.WriteLine("Recursion complete");
                return;
            }
            else
            {
                    //Draw
                    System.Console.WriteLine("Draw # " + from + " of " + n + " with angle " + SQ_ANGLE + " and coords " + x + " x " + y + ", size " + size);
                    using (Pen pen = new Pen(Color.FromArgb(from,from,from), 1))
                    {
                    rectangle.X = SQ_CENTER - (size / 2);
                    rectangle.Y = SQ_CENTER - (size / 2);
                    rectangle.Size = new System.Drawing.Size(size, size);
                    using (Matrix m = new Matrix())
                    {
                        m.RotateAt(SQ_ANGLE, new PointF(rectangle.Left + (rectangle.Width / 2),
                                                  rectangle.Top + (rectangle.Height / 2)));
                        graphics.Transform = m;
                        graphics.DrawRectangle(Pens.Black, rectangle);
                        graphics.ResetTransform();
                    }
                }

                from++; //iterate
                size = Convert.ToInt32(size * 0.7); //Decrease size of next rectangle
                SQ_ANGLE = SQ_ANGLE + 45; //Change angle for next square
                DrawSquares(graphics, from, n, size, x , y); 
            }
        }


    }
}
