using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practice
{
    public partial class Form1 : Form
    {
        static Graphics g;
        static int width = 10;
        static int height = 150;



        class Cannon
        {
            private double ang = 0;
            public double cosA, sinA;
            public float x=400, y=470;
            public float circleX=140, circleY=140;
            
            public float spd;
            public void setAngle(double _ang)
            {
                ang = _ang;
                cosA = Math.Cos(ang);
                sinA = Math.Sin(ang);
            }
            public void drawStick()
            {    //畫砲管
                
                Pen blackPen = new Pen(Brushes.Black);    // 宣告 + new 深藍色  畫筆
                blackPen.Width = 50.0F;   // 改變 畫筆 寬度
                g.DrawLine(blackPen, (float)(x -1*cosA), (float)(y -1*sinA), (float)(x +120*cosA), (float)(y +120*sinA));
            }
            public void drawCircle()
            {
                circleX = 140;
                circleY = 140;
                spd = 0;
                Pen redPen = new Pen(Brushes.Red);
                redPen.Width = 10F;
                g.DrawEllipse(redPen,(float)(x + circleX * cosA), (float)(y + circleY * sinA), 10, 10);
            }
            public void circleMove()
            {
                spd = 4;
                circleX += spd;
                circleY += spd;
                Pen redPen = new Pen(Brushes.Red);
                redPen.Width = 10F;
                g.DrawEllipse(redPen, (float)(x + circleX * cosA), (float)(y + circleY * sinA), 10, 10);
                if(x + circleX * cosA > width)
                {
                    spd = 0;
                    circleX = 140;
                    circleX += spd;
                    circleY = 140;
                    circleY += spd;
                }

            }
            public void rebound()
            {

                if (x + circleX * cosA < width && x + circleX * cosA > 0 && y + circleY * sinA < height && y + circleY * sinA > 0)
                {

                    this.spd = 5;
                    circleX += spd;
                    circleY += spd;

                }
                else
                {
                    spd = 0;
                    circleX = 140;
                    circleY = 140;
                }
            }
        }
        Cannon cannon = new Cannon();
        class Cube
        {
            public float x = 400, y = 50;
            private float size = 30;
            public float spd = 5;
            public void drawCube()
            {
                Pen orangePen = new Pen(Brushes.Orange);
                orangePen.Width = 30F;
                g.DrawRectangle(orangePen, x, y, size, size);
            }
            public void move()
            {
                
            }
            public void rebound()
            {
                if (x < width)
                {
                    x += spd;
                    while (x >= width)
                    {
                        spd *= -1;
                        x += spd;
                    }
                    while (x < 0)
                    {
                        spd *= -1;
                        x += spd;
                    }
                }
            }
        }
        Cube cube = new Cube();
        
        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            cannon.setAngle(Math.PI / 2);
            width = panel1.Width;
            height = panel1.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = (30); // 10 secs
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            cannon.drawStick();
            cube.drawCube();
            cannon.circleMove();
        }

        private void panel1_MouseDown_1(object sender, MouseEventArgs e)
        {
            cannon.circleMove();
            double a = Math.Atan2(e.Y - cannon.y, e.X - cannon.x); // e:滑鼠 點擊處坐標
            cannon.setAngle(a); // 存入母球 行進角度
            panel1.Refresh(); // 重新繪畫轉動過的球桿
            g.DrawRectangle(Pens.Black, e.X - 2, e.Y - 2, 4, 4); // 點擊點 畫小方塊
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double speed = 20;
            panel1.Refresh();
            if (speed != 0)
            {
                cube.move();
                cube.rebound();
            }
            else
            {
                timer1.Stop();		//  停止 計時器
                panel1.Refresh();
            }
        }
    }
 }

