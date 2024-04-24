using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rekursija
{
    public partial class Form1 : Form
    {
        
        delegate void CalcFunction(int a, int b, int c, int start, int end, int step, bool DrawGraphic); //Define delegate
        private CalcFunction FUNCTION_NAME;
        int a, b, c, start, end, step;
        bool DrawGraphic;
        

        public Form1()
        {
            InitializeComponent();
            this.Text = "Delegati by VG";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                a = Convert.ToInt32(textBox1.Text);
                b = Convert.ToInt32(textBox2.Text);
                c = Convert.ToInt32(textBox3.Text);
                start = Convert.ToInt32(textBox4.Text);
                end = Convert.ToInt32(textBox5.Text);
                step = Convert.ToInt32(textBox6.Text);
                textBox7.Text = "     x     y     ";
                
                if (DrawGraphic)
                {
                    panel1.Refresh();
                    using (Graphics g = panel1.CreateGraphics())
                    {
                        g.DrawLine(new Pen(Color.Green, 1), new Point(0, 200), new Point(400, 200)); //Vertical
                        g.DrawLine(new Pen(Color.Green, 1), new Point(200, 0), new Point(200, 400)); //Horizontal
                    }
                }
                Execute_Calculation(FUNCTION_NAME, a, b, c, start, end, step, DrawGraphic);
            } catch (Exception)
            {

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            DrawGraphic = checkBox1.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            FUNCTION_NAME = CalculateF1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            FUNCTION_NAME = CalculateF2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            FUNCTION_NAME = CalculateF3;
        }

        //Add values to text and draw chart if flag was set
        private void textAdd(float x, float y, bool DrawGraphic)
        {
            textBox7.Text = textBox7.Text + Environment.NewLine + "   " + x.ToString("0.00") + "   " + y.ToString("0.00");
            if (DrawGraphic)
            {
                using (Graphics g = panel1.CreateGraphics())
                {
                    Brush br = (Brush)Brushes.Red;
                    g.FillRectangle(br, 200 + x, 200 + (-1) * y, 1, 1);
                }
            }
        }


        private static void Execute_Calculation(CalcFunction _del, int a, int b, int c, int start, int end, int step, bool DrawGraphic)
        {
            System.Console.WriteLine("Invoke " + a + " " + b + " " + c + " " + start + " " + end + " " + step + " " + DrawGraphic);
            _del?.Invoke(a,b,c,start,end,step,DrawGraphic);
        }

        public void CalculateF1(int a, int b, int c, int start, int end, int step, bool DrawGraphic)
        {
         try
            {
                float y;
                for (float x = start; x < end + 1; x = x + step)
                {
                    y = Convert.ToSingle(a * Math.Pow(x, 2) + b * x + c);
                    textAdd(x, y, DrawGraphic);
                }
            } catch (Exception)
            {

            }
        }
        public void CalculateF2(int a, int b, int c, int start, int end, int step, bool DrawGraphic)
        {
            try
            {
                float y;
                for (float x = start; x < end + 1; x = x + step)
                {
                    y = Convert.ToSingle(a / Math.Pow(x, 2) + b / x + c);
                    textAdd(x, y, DrawGraphic);
                }
            }
            catch (Exception)
            {

            }
        }
        public void CalculateF3(int a, int b, int c, int start, int end, int step, bool DrawGraphic)
        {
            try
            {
                float y;
                for (float x = start; x < end + 1; x = x + step)
                {
                    y = Convert.ToSingle((a * x + b) + b / x + c);
                    textAdd(x, y, DrawGraphic);
                }
            }
            catch (Exception)
            {

            }
        }

    }
}
