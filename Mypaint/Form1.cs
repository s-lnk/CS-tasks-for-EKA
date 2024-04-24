using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mypaint
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap bmp;
        Color SELECTED_COLOR = Color.Red; //By default Red is choosen
        int PEN_SIZE = 1; //Default pen size to draw
        int SELECTED_TOOL;
        bool DRAWING;
        int? initX = null; //Used for realtime drawing of Pen and Eraser
        int? initY = null; //Used for realtime drawing of Pen and Eraser
        int SHAPE_initX; //Used for storing coordinates for shapes
        int SHAPE_initY; //Used for storing coordinates for shapes
        int SHAPE_lastX; //Used for storing coordinates for shapes
        int SHAPE_lastY; //Used for storing coordinates for shapes

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "MyPaint by VG";
            button2.BackColor = SELECTED_COLOR;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height); //Bitmap to store image
            g = Graphics.FromImage(bmp); //graphics area to draw
            g.Clear(Color.White); //Make bitmap background white
            pictureBox1.Image = bmp; //Set picture box to display bitmap
            SELECTED_TOOL = 1;//By default Pen is preselected

        }

        private void playSound()
        {
            System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(mypaint.Properties.Resources.Windows_Startup);
            simpleSound.Play();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Exit App
            playSound();
            System.Windows.Forms.Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //New empty document
            playSound();
            g.Clear(Color.White); //Fill with white
            pictureBox1.Image = bmp; //Rfresh image in picturebox
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open image
            playSound();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Bitmap pic = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb); //Create bitmap with image dimensions
                pic = new Bitmap(open.FileName); //open bitmap
                g.DrawImage(pic, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height)); //Fit opened image in picturebox
                pictureBox1.Image = bmp; //Rfresh image in picturebox
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Save image to BMP
            playSound();
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Image Files(*.bmp)|*.bmp";
            if (save.ShowDialog() == DialogResult.OK)
            {
                g.DrawImage(bmp, new Point(0, 0)); //Draw to bitmap
                bmp.Save(save.FileName, System.Drawing.Imaging.ImageFormat.Bmp);//Save bitmap
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Print image to default printer
            playSound();
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PageToPrint);
            pd.Print();
        }

        private void PageToPrint(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Prepare document to print
            e.Graphics.DrawImage(pictureBox1.Image, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SELECTED_TOOL = 1; //Pen
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SELECTED_TOOL = 2; //Line
        }
        private void button4_Click(object sender, EventArgs e)
        {
            SELECTED_TOOL = 3; //Rectangle
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SELECTED_TOOL = 4; //Ellipse
        }
        private void button6_Click(object sender, EventArgs e)
        {
            SELECTED_TOOL = 5; //Eraser
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                SELECTED_COLOR = c.Color;
                button2.BackColor = SELECTED_COLOR;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            DRAWING = true; //Drawing mode on
            SHAPE_initX = e.X; //Store initial coordinates for shapes
            SHAPE_initY = e.Y; //Store initial coordinates for shapes
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (DRAWING)
            {
                switch (SELECTED_TOOL)
                {
                    case 1: //Pen - draws in real time
                        Pen p = new Pen(SELECTED_COLOR, PEN_SIZE);
                        g.DrawLine(p, new Point(initX ?? e.X, initY ?? e.Y), new Point(e.X, e.Y));
                        initX = e.X;
                        initY = e.Y;
                        break;
                    case 5: //Eraser - Pen with white color and pen size 50
                        Pen eraser = new Pen(Color.White, 50);
                        g.DrawLine(eraser, new Point(initX ?? e.X, initY ?? e.Y), new Point(e.X, e.Y));
                        initX = e.X;
                        initY = e.Y;
                        break;
                    default: //Shapes - do nothing here, just store current coordinates
                        SHAPE_lastX = e.X;
                        SHAPE_lastY = e.Y;
                        break;
                }
                pictureBox1.Image = bmp;
            }

        }
        

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Pen p = new Pen(SELECTED_COLOR, PEN_SIZE);
            int iX, iY, lX, lY;
            //Below find and set top-left coordinate iX, iY and bottom-right lX, lY to correctly draw shapes
            if (SHAPE_initX < SHAPE_lastX)
            {
                iX = SHAPE_initX;
                lX = SHAPE_lastX;
            } else
            {
                iX = SHAPE_lastX;
                lX = SHAPE_initX;
            }
            if (SHAPE_initY < SHAPE_lastY)
            {
                iY = SHAPE_initY;
                lY = SHAPE_lastY;
            }
            else
            {
                iY = SHAPE_lastY;
                lY = SHAPE_initY;
            }
            switch (SELECTED_TOOL)
            {
                case 1: //Pen - drawm realtime, do nothing
                    break;
                case 2: //Line
                    g.DrawLine(p, new Point(SHAPE_initX, SHAPE_initY), new Point(SHAPE_lastX, SHAPE_lastY));
                    break;
                case 3: //Rectangle
                    g.DrawRectangle(p, iX, iY, lX - iX, lY - iY);
                    break;
                case 4: //Ellipse
                    g.DrawEllipse(p, iX, iY, lX - iX, lY - iY);
                    break;
                case 5: //Eraser - drawm realtime, do nothing
                    break;
            }
            DRAWING = false; //Drawing mode off
            initX = null; 
            initY = null;
            pictureBox1.Image = bmp;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g.DrawImage(bmp, new Point(0, 0)); //Draw to bitmap
       }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            PEN_SIZE = 1;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PEN_SIZE = 3;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            PEN_SIZE = 5;
        }
    }
}
