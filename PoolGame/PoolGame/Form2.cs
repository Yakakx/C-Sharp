using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoolGame
{
    public partial class Form2 : Form
    {
        static Graphics g;
        static int r = 10, r2 = 20;
        static double fr = 0;
        BufferedGraphicsContext currentContext;
        BufferedGraphics gBuffer;
        static int width = 0, height = 0;

        class ball
        {
            int id;
            public double x = 0, y = 0;
            Color c;
            SolidBrush br;
            private double ang = 0;
            public double cosA, sinA;
            public double spd = 0;
            public ball(int bx, int by, Color cc, int i)
            {
                x = bx;
                y = by;
                c = cc;
                br = new SolidBrush(cc);
                id = i;
            }

            public void draw()
            {
                g.FillEllipse(br, (int)(x - r), (int)(y - r2), r2, r2);
            }
            public void setAng(double _ang) {    //ex4：角度 改變
                ang = _ang;                       // 存 新角度
                cosA = Math.Cos(ang);     //  重算 coSine
                sinA = Math.Sin(ang);       //  重算 Sine
            }
            public void drawStick() {    //ex4：畫球桿
                double r20 = 20 * r;
                Pen blackPen = new Pen(Brushes.Black);    // 宣告 + new 深藍色  畫筆
                blackPen.Width = 3.0F;   // 改變 畫筆 寬度
                g.DrawLine(blackPen,      // 深藍色  畫筆
                     (float)(x - r20 * cosA), (float)(y - r20 * sinA),    //  12 倍大的 同心圓周上的點
                     (float)(x - r * cosA), (float)(y - r * sinA)        //  球 圓周上的點
                );  // - r12   -r , 使球杆 畫在滑鼠點的另一邊
            }
            public void move()
            {  //  移動 球
                if (spd > 0)
                {  //  速度 >0 才移動
                    x += spd * cosA;   //  x 方向分量
                    y += spd * sinA;    //  y 方向分量
                    spd -= fr;              //  速度 依摩擦力大小 遞減
                }
                else spd = 0;  //  避免 <0 而反向移動
            }
            public void rebound()
            {
                if (x < r || x > width - r) {  //出左右邊
                    setAng(Math.PI - ang);
                    if (x < r)
                        x = r;    // 拉回桌 內
                    else
                        x = width - r;
                }
                else if (y < r || y > height - r) { //出上下邊
                    setAng(-ang);
                    if (y < r)
                        y = r;    //拉回桌 內
                    else
                        y = height - r;
                }
            }
        }

        ball[] balls = new ball[10];
        public Form2()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            for (int i = 1; i < 10; i++)
            {
                balls[i] = new ball(200, i * (r2 + 10) + 90, Color.FromArgb(255, (i * 100) % 256, (i * 50) % 256, (i * 25) % 256), i);
            }
            balls[0] = new ball(600, 230, Color.FromArgb(255, 255, 255, 255), 0);
            balls[0].setAng(Math.PI / 2);
            width = panel1.Width;
            height = panel1.Height;
            currentContext = BufferedGraphicsManager.Current;
            gBuffer = currentContext.Allocate(
                this.panel1.CreateGraphics(),
                new Rectangle(0, 0, width, height));
            g = gBuffer.Graphics;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Owner.Show();
        }


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Owner.Show();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            double a = Math.Atan2(e.Y - balls[0].y, e.X - balls[0].x); // e:滑鼠 點擊處坐標
            balls[0].setAng(a); // 存入母球 行進角度
            panel1.Refresh(); // 重新繪畫轉動過的球桿
            g.DrawRectangle(Pens.HotPink, e.X - 2, e.Y - 2, 4, 4); // 點擊點 畫小方塊
            gBuffer.Render();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            balls[0].spd = (vScrollBar1.Maximum - vScrollBar1.Value) * 1.1; // 母球 加 速度
            fr = (vScrollBar2.Maximum - vScrollBar2.Value) / 120.0;  // 摩擦力
            timer1.Enabled = true;
        }
        private void hit(ball b0, ball b1)
        {

            if (b0.spd < b1.spd && b1.spd>0)
            {   // b1 hit  b0  速度快的 撞 慢的
                ball t = b0;     //  交換球，讓速度快的球 成為 b0
                b0 = b1;
                b1 = t;
                
            }
            double dx = b1.x - b0.x, dy = b1.y - b0.y;
            if (Math.Abs(dx) <= r2 && Math.Abs(dy) <= r2)
            { //  x坐標間差距 < 球直徑
              // 而且　　y坐標間差距 < 球直徑
                double ang = Math.Atan2(dy, dx);   //  球b0 中心 到 球b1 中心 連線方向
                b1.setAng(ang);     //  球b1 被撞后方向
                b0.setAng(ang + Math.PI / 2.0);   //  球b0  碰撞 b1 后 和 b1 的夾角 90° 

                double spd_average = (b0.spd + b1.spd) / 2.0;
                b0.spd = b1.spd = spd_average;    //  碰撞 後 先大略平均分配 兩球的速度
                                                  // 白球速度 == 紅球速度 == 兩球的速度 和 /2
                checkBox1.Checked = true;
            }
            if (checkBox1.Checked)
            {
                timer1.Stop();
                panel1.Refresh();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            double sum_spd = 0;  // 球速度 加總
            panel1.Refresh();  //呼叫panel1_Paint 事件處理副程式
            for (int i = 0; i < 10; i++) {
                if (balls[i].spd > 0)
                {
                    balls[i].move();
                    balls[i].rebound();
                    sum_spd += balls[i].spd;
                    
                }
                for (int j = i + 1; j < 10; j++)
                {   // j > i 兩球間不重複 碰撞偵測
                    hit(balls[i], balls[j]);
                    
                }
            }
            if (sum_spd <= 0.001)
            {  //  所有球 都停了
                timer1.Stop();		//  停止 計時器
                panel1.Refresh();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                timer1.Start();
                panel1.Enabled = true;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g.Clear(panel1.BackColor);
            for (int i = 0; i < 10; i++)
                balls[i].draw();
            if (balls[0].spd < 0.0001) balls[0].drawStick();
            gBuffer.Render(e.Graphics);
        }

        

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Interval = (20); // 10 secs
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
            label1.Text = "歡迎 ━ " + ((Form1)Owner).textBox1.Text;
        }
    }
}
