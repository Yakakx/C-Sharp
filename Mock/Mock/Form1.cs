using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mock
{
    public partial class Form1 : Form
    {
        static Graphics g;
        static int height;
        static int width;
        static int mouse_clcick;
        static int count = 0;
        static Boolean hit1 = false;
        static Boolean hit2 = false;
        static int count1 = 0;
        static int count2 = 0;
        static int counter = 0;
        class Bullet
        {
            private double ang = 0;
            public double cosA, sinA;
            public double x = width/2, y = height;
            public double circleX = 140, circleY = 140;
            public double spd;
            int id;
            public int c_X; public int c_Y;
            Color c;
            SolidBrush br;
            int r=10;
            public Bullet(double bx, double by, Color bc, int bi)
            {
                x = bx;
                y = by;
                c = bc;
                id = bi;
                br = new SolidBrush(bc);
            }
            public void setAngle(double _ang)
            {
                ang = _ang;
                cosA = Math.Cos(ang);
                sinA = Math.Sin(ang);
            }
            public void drawStick()
            {    //畫砲管
                c_X = width / 2;
                c_Y = height+10;
                Pen blackPen = new Pen(Brushes.Black);    // 宣告 + new 深藍色  畫筆
                blackPen.Width = 40.0F;   // 改變 畫筆 寬度
                g.DrawLine(blackPen, (float)(c_X - 1 * cosA), (float)(c_Y - 1 * sinA), (float)(c_X + 120 * cosA), (float)(c_Y + 120 * sinA));
            }
            public void draw()
            {
                g.FillEllipse(br, (float)(x+110*cosA), (float)(y+110*sinA), r, r);
            }
            public void move()
            {
                if (spd > 0)
                {
                    x += spd * cosA;
                    y += spd * sinA;
                }
                else
                {
                    spd = 0;
                }
            }
            public void rebound()
            {
                if (x > width||x<0||y> height|| y < 0){
                    spd = 0;
                    x = (width/2 + 100 * cosA);
                    y = (height+20) + 100* sinA +70;
                }
            }
        }
        Bullet[] bullets = new Bullet[3];
        class Cube
        {
            public int count = 0;
            public float x = 700, y = 80;
            private float size = 20;
            private float spd = -6;
            private float ySpd = 0;
            public void drawCube()
            {
                Pen bluePen = new Pen(Brushes.Blue);
                bluePen.Width = 20F;
                g.DrawRectangle(bluePen, x, y, size, size);
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
            public void yDrop()
            {
                ySpd = 1;
                y += ySpd;
                if (y > height / 2)
                {
                    hit1 = false;
                    y = 80;
                    spd = -10;
                }
            }
            public void drop()
            {
                ySpd = 1;
                y += ySpd;
                if (y > height)
                {
                    hit1 = false;
                    y = 80;
                    spd = -6;
                    counter++;
                }
            }
        }
        Cube cube = new Cube();

        class Cube2
        {
            public int count = 0;
            public float x = 400, y = 20;
            private float size = 30;
            private float spd = 4.5f;
            private float ySpd = 0;
            public void drawCube()
            {
                Pen orangePen = new Pen(Brushes.Orange);
                orangePen.Width = 30F;
                g.DrawRectangle(orangePen, x, y, size, size);
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
            public void yDrop()
            {
                ySpd = 1;
                y += ySpd;
                if (y > height / 2)
                {
                    hit2 = false;
                    y = 20;
                    spd = 8;
                }
            }
            public void drop()
            {
                ySpd = 1;
                y += ySpd;
                if (y > height)
                {
                    hit2 = false;
                    y = 20;
                    spd = 4.5f;
                    counter++;
                }
            }
        }
        Cube2 cube2 = new Cube2();
        
            public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            width = panel1.Width;
            height = panel1.Height;
            for(int i = 0; i <3; i++)
            {
                bullets[i] = new Bullet((width / 2 )* (1 + i * 0.03), height, Color.Red, 1);
                bullets[i].setAngle(Math.PI / 2);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = (30); // 
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }
        

        private void panel1_MouseDown_1(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                double a = Math.Atan2(e.Y - bullets[i].y, e.X - bullets[i].x); // e:滑鼠 點擊處坐標
                bullets[i].setAngle(a);
                
            }
                // 存入母球 行進角度
                panel1.Refresh(); // 重新繪畫轉動過的球桿
                g.DrawRectangle(Pens.HotPink, e.X - 2, e.Y - 2, 4, 4); // 點擊點 畫小方塊
                
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            bullets[0].drawStick();
            for(int i = 0; i<3; i++)
            {
                bullets[i].draw();
            }
            cube.drawCube();
            cube2.drawCube();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double speed = 20;  
            panel1.Refresh();
            if (speed != 0)
            {
                cube.rebound();
                
                cube2.rebound();
                for (int i = 0; i < 3; i++)
                {
                    bullets[i].move();
                    bullets[i].rebound();
                    if (bullets[i].x + bullets[i].spd * bullets[i].cosA > cube.x &&
                        bullets[i].x + bullets[i].spd * bullets[i].cosA < cube.x+20 &&
                        bullets[i].y + bullets[i].spd * bullets[i].sinA < cube.y + 100&&
                        bullets[i].y + bullets[i].spd * bullets[i].sinA > cube.y+30)//cube.y+100
                    {
                        hit1=true;
                        count1++;
                    }
                    else if(bullets[i].x + bullets[i].spd * bullets[i].cosA > cube2.x &&
                        bullets[i].x + bullets[i].spd * bullets[i].cosA < cube2.x + 30 &&
                        bullets[i].y + bullets[i].spd * bullets[i].sinA < cube2.y + 100 &&
                        bullets[i].y + bullets[i].spd * bullets[i].sinA > cube2.y + 30)
                    {
                        hit2 = true;
                        count2++;
                    }
                    if (hit1 == true && count1 % 2 == 1) 
                    {
                        cube.yDrop();
                    }
                    else if(hit2 == true && count2%2==1)
                    {
                        cube2.yDrop();
                    }
                    else if (hit1 == true && count1%2==0)
                    {
                        cube.drop();
                    }
                    else if (hit2 == true && count2 %2== 0)
                    {
                        cube2.drop();
                    }
                }
                label1.Text = "Count: " + counter.ToString();
            }
            else
            {
                timer1.Stop();		//  停止 計時器
                panel1.Refresh();
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            mouse_clcick++;
            bullets[mouse_clcick % 3].spd = 4;
        }
    }
}
