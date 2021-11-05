using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace B09502132周哲瑋Ass005BrickBuster
{
     
    
    public partial class PlayForm : Form
    {
        //變數宣告
        PictureBox[] bricks; //磚塊物件宣告
        int X_Inc = 20, Y_Inc = 20;//x->水平移動,y垂直,正->右下,負相反
        bool stay = true; //確認球是否還在邊界外
        bool stay2 = true;//確認球是否未碰撞玩家
        int hidden = 0; //撞擊的磚塊數,如果撞完會啟動,YOU WIN!
        StartForm form1;//引用StartForm的函數(playBGM)
        int[] x_pos = {152,262,362,612,742,842,42,242,362,482,602,712,802};//磚塊的位置X
        int[] y_pos = { 138, 218 };//磚塊的位置Y
        bool ischange = true;//確認球的gif是否已經更換過,不然每0.1s都一直更換的話,動畫效果出不來
        int time = 120;//時限2min
       
         




        public PlayForm()
        {
            InitializeComponent();

            brickBuild();//磚塊建立
            form1 = new StartForm();//配給記憶體
            form1.playBGM(2, 0);//撥放第二首BGM
            ball.ImageLocation = Application.StartupPath + @"\resource1\ball_cw.gif";//球的初始的設定為順時鐘
            TIME.Text = "Time:" + time;//初始化時間120sec




        }
        
        private void PlayForm_KeyUp(object sender, KeyEventArgs e)//當鬆開右鍵或左鍵時,人物恢復到原本的狀態
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                player.ImageLocation = Application.StartupPath + @"\resource1\reimu.png";
                player.Width = 50;//恢復原本的寬度,因為跑動的人物寬度是80
            }
        }
       


        private void PlayForm_KeyDown(object sender, KeyEventArgs e) //按下按鍵
        {
            if (e.KeyCode == Keys.Left)
            { 
                player.Location = new Point(player.Location.X - 20, player.Location.Y); //location是此物件(玩家)的位置,按左鍵左走20
                if (player.ImageLocation != Application.StartupPath + @"\resource1\reimu_left.gif")
                {
                    player.ImageLocation = Application.StartupPath + @"\resource1\reimu_left.gif";
                    player.Width = 70;
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                player.Location = new Point(player.Location.X + 20, player.Location.Y);//按右鍵右走20
                if (player.ImageLocation != Application.StartupPath + @"\resource1\reimu_right.gif")
                {
                    player.ImageLocation = Application.StartupPath + @"\resource1\reimu_right.gif";
                    player.Width = 70;
                }
            }

            if (e.KeyCode == Keys.Escape) this.Close(); //按Esc離開遊戲,只有在playform使用才能
            
                
        }

        private void timer1_Tick(object sender, EventArgs e)//interval 100 ms,每0.1秒更新,不然球跑太慢
        {
           

            brickCrash(bricks);//檢查是否撞擊磚塊


            //邊界碰撞
            if ((ball.Left < 0 || ball.Left > 980) && stay)//左右邊界,this.Bounds.Width->似乎會有點bug,畫框水平x -20~742,經測試980best.還有只有當
            {
                X_Inc = -X_Inc; //相反方向反彈
                ischange = true;//改變球轉向
                stay = false;//只有第一次進入邊界才會改變方向,不然有可能卡在邊界外部不斷改方向
            }
            else if ((ball.Top > 490 || ball.Top < 25) && stay)
            {
                Y_Inc = -Y_Inc;  //上下邊界,可用this.Bounds.Height 畫框垂直y 0~500 ,經測試500best
                stay = false;
            }
            //if (ball.Left > 0 && ball.Left < 740 && ball.Top < 400 && ball.Top > 0 && !stay) stay = true;// 改用else較精簡
            else stay = true;//回到框中,恢復原本true,之後再撞到邊界外,才能反彈


            //玩家是否擊中球：
            //正擊
            if (ball.Left >= player.Left && ball.Left <= (player.Left + player.Width) && ball.Top >= (player.Top - ball.Height) && stay2)
            { 
                Y_Inc = -Y_Inc;
                stay2 = false;//才不會讓球一直卡在玩家身上
            }
            //側擊->應該還要考慮當下球的運動方向
            else if (((ball.Right >= player.Left && ball.Right <= player.Left + 7 && ball.Bottom >= player.Top - 7) ||
                (ball.Left >= player.Right - 7 && ball.Left <= player.Right && ball.Bottom >= player.Top - 7)) && stay2)
            {
                 if(ball.Left<player.Left) X_Inc = -Math.Abs(X_Inc);//當碰到左邊,向左移動(負值)
                 else X_Inc = Math.Abs(X_Inc);//當碰到右邊(other),向右移動(正值)

                //X_Inc = -X_Inc; //運動方向都反轉
                Y_Inc = -Y_Inc;
                stay2 = false;
                ischange = true;//改變球的轉向
            }
            else stay2 = true;
            
            //玩家的移動
            if (player.Location.X >= 970)//,(之後改用鍵盤)this.Bounds.Width,980是測試過的,因為物件的框太大
                player.Left = 969;//減去 球拍的水平位置，否則的話，球拍會在視窗 ,-1是讓她之後再跑到這if
            else if(player.Location.X <= -30)//-40是測試過的
                player.Left = 1;
            

            //球移動
            ball.Left += X_Inc;//水平,和form左邊的距離
            ball.Top += Y_Inc;//垂直


            checkDirection(X_Inc,ischange);//檢查球要順時鐘轉還是逆時鐘



        }

        public void checkDirection(int x,bool un)//當球向右移時,順時鐘旋轉,左移則逆時針
        {
            if(x>0 && un)ball.ImageLocation = Application.StartupPath + @"\resource1\ball_cw.gif";
            else if(x < 0 && un) ball.ImageLocation = Application.StartupPath + @"\resource1\ball_ccw.gif";
            ischange = false;
        }

        public void brickBuild()//磚塊建立
        {
            bricks = new PictureBox[13];//創建bricks的圖片組
            string pathBK = "";//儲存磚頭圖路徑
            pathBK = Application.StartupPath + @"\resource1\brick2.png";//先取得此應用程式的位置,使用這個的目的是為了讓程式使用相對路徑,而非絕對路徑,這樣其他電腦才能使用
            for (int i =0;i<13;i++)
            {
                bricks[i] = new PictureBox(); //產生每一個磚塊PictureBox物件
                bricks[i].Load(pathBK);
                bricks[i].Left = x_pos[i];
                bricks[i].Top = y_pos[i/7]; //  0~6/7 = 0 , 7~12/7 = 1
                
                bricks[i].SizeMode = PictureBoxSizeMode.StretchImage; //圖案依PictureBox的大小進行縮放設定
                bricks[i].Height = 50; //磚塊的高度
                bricks[i].Width = 50; //磚塊的寬度                    
                this.Controls.Add(bricks[i]); //將磚塊PictureBox物件加入Form1控制項集合
            }
        }

        private void timer2_Tick(object sender, EventArgs e)//專門給遊戲計時的timer
        {
            time--; //interval已設為1000ms,1秒減掉一個時間
            TIME.Text = "Time:" + time; //文字顯示剩餘時間
            if(time==0 && hidden < 13) {
                timer2.Stop();
                timer1.Stop();//timer1也關掉,不然一直跑會耗資源
                form1.playBGM(4,0);
                resultText.Text = "You Lost!";
                resultText.Visible = true;
            }
            else if (time == 0) { timer2.Stop(); timer1.Stop(); }//無論如何當時間歸零時,都把timer2關起來

            if(time<=10) //最後十秒,會逼逼叫提醒,加上計時變紅
            {
                TIME.ForeColor = Color.Red;
                Console.Beep();
            }

        }

        public void brickCrash(PictureBox[] bricks)//檢查磚塊是否被撞到
        {
          
           
            for (int i=0;i<13;i++)//每一個磚塊都檢查,一共13個
            {
               
               
               if(bricks[i].Visible == true)//如果磚塊還能看見的話,就檢查是否有撞到
               {
                    
                    if (ball.Left >= bricks[i].Left && ball.Left <= (bricks[i].Right) 
                                      && ball.Top <= (bricks[i].Bottom) && ball.Top >= (bricks[i].Top ))
                    {
                        //Y_Inc = -Y_Inc; 我這裡不會因為因此而改變動向,因為這樣他就自己在上面彈來彈去,一堆都彈光了
                        bricks[i].Visible = false; //擊中後，將磚塊的Visible屬性設false，隱形
                        hidden++;
                        goto HitBrickExit; //擊中，就不用測試其他的磚，跳離節省時間
                    }
                  
                    
                    
                   
                }

               if (hidden == 13) //如果全都打掉,就會出現you win!
               {

                    timer1.Stop();//一定得暫停,不然timer一直跑,不斷執行啟動playBGM,會有次次次的聲音或是無聲
                    timer2.Stop();//也把timer2(遊戲計時)停止
                    form1.playBGM(3, 0);//撥放勝利音效
                    resultText.Visible = true;//把勝利文字顯示
                    

                    

               }
               
            }
            HitBrickExit: {  };//跳脫

        }

       




    }
}
