using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickBuster
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            
            playBGM(1,0);//載入一開始就撥放第一首,0表示不關掉
        }

        


        private void Form2_FormClosed(object sender, FormClosedEventArgs e) //關閉form2的功能
        {
            this.Close();
           
        }

        private void Startbtn_Click(object sender, EventArgs e)//按下開始鍵
        {

            PlayForm a = new PlayForm();//產生Form2的物件，才可以使用它所提供的Method
      
            a.Show(); 
            //playBGM(1,1);//再次呼叫,把音樂關掉.原本是想這樣設計的,但是這樣bgm2就無法撥放,反正之後新bgm會覆蓋之上.(應該也可以在playform使用bgm.play再次開啟)
            this.Hide();
            a.FormClosed += new FormClosedEventHandler(Form2_FormClosed); //只有加入這個,form2關閉後才會結束程式,不然會一直跑
        }

        public void playBGM(int on,int off)//撥放背景音樂程式
        {
            SoundPlayer BGM1 = new SoundPlayer();
            string path1 = "";//儲存音樂路徑,此為封面音樂
            string path2 = "";//此為遊戲時的音樂
            string path3 = "";//此為勝利的bgm
            string path4 = "";//此為失敗的bgm
            path1 = Application.StartupPath + @"\resource1\Sacret.wav";//先取得此應用程式的位置,使用這個的目的是為了讓程式使用相對路徑,而非絕對路徑,這樣其他電腦才能使用
            path2 = Application.StartupPath + @"\resource1\eternalMiko.wav";
            path3 = Application.StartupPath + @"\resource1\win.wav";
            path4 = Application.StartupPath + @"\resource1\lost.wav";

            if (on == 1) BGM1.SoundLocation = path1; //依照on選擇哪首bgm
            else if (on == 2) BGM1.SoundLocation = path2;
            else if (on == 3) BGM1.SoundLocation = path3;
            else if (on == 4) BGM1.SoundLocation = path4;


            BGM1.Play();//撥放

            if (off == 1)BGM1.Stop();//當第二參數為2就會關掉音樂
            
        }

        private void Quitbtn_Click(object sender, EventArgs e)//關掉鍵,關閉遊戲
        {
            this.Close();
        }
    }
}
