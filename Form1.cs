using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OTTER
{
    public partial class Form1 : Form 
    {
        bool goUp, goDown, shot, gameOver,quit;
        int speed = 8;


        int score = 0;
        int enemySpeed = 9;

        Random rand = new Random(); // za nasumicno postavljanje neprijatelja na scenu

        int autoSpeed = 7;
        int index = 0; // za brojanje neparijatelja


        #region events

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;

            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if (e.KeyCode == Keys.Space && shot == false)
            {
                MakeBullet();
                shot = true;
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            if (shot == true)
            {
                shot = false;
            }
            if (e.KeyCode == Keys.Enter && quit== true)
            {
                // RestartGame(); // ovdje mozda bolje Application.Exit() ! 
                Application.Exit();
                Close();
            }
        }


        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score : " + score;
            if (goUp == true && auto.Top>0)
            {

                auto.Top -= autoSpeed;

            }
            if (goDown == true && auto.Top+auto.Height<this.ClientSize.Height)
            {
                auto.Top += autoSpeed;

            }
            enemy.Left -= enemySpeed;

            if(enemy.Left+enemy.Width<0)
            {
                ChangeEnemy();
            }

            
            
            foreach (Control x in this.Controls)
            {
               
                if (x is PictureBox && (string)x.Tag=="prepreka")
                {
                    x.Left -= speed;
                    #region Fora s pomicanjem scene 
                    /* Izgledat ce kao da se igracev auto pomice, ali se zapravo pomicu prepreke i neprijatelji !  */
                    if (x.Left<-200)
                    {
                        x.Left = 1000;
                    }
                    #endregion

                    #region Interakcija igraca i prepreke - ako igrac udari o prepreku - umire
                    if (auto.Bounds.IntersectsWith(x.Bounds))
                    {

                        quit = true;
                        GameOver();
                    }

                    #endregion
                }




                if (x is PictureBox && (string)x.Tag=="bullet")
                {
                    #region Bullets (metci, cvjetici) - kretanje, PUCANJE
                    x.Left += 25;
                    if(x.Left>900)
                    {
                        RemoveBullet(((PictureBox)x));
                    }
                    #endregion

                    #region Interakcija metka i neprijatelja 

                    if (enemy.Bounds.IntersectsWith(x.Bounds))
                    {
                        RemoveBullet(((PictureBox)x));
                        score++;
                        ChangeEnemy();
                    }

                     #endregion

                }

            }

            #region Interakcija neprijatelja i igraca - ako neprijatelj dotakne igraca, igrac umire

            if (auto.Bounds.IntersectsWith(enemy.Bounds))
            {
                quit = true;
                GameOver();
            }

            #endregion

            if(score>=10)
            {
                txtScore.Text = "CONGRATS ! YOU WON!!!\n PRESS ENTER ";
                quit = true;
                
            }

        }



        #endregion



        #region funkcije

        private void RestartGame()
        {
            goUp = false;
            goDown = false;
            shot = false;
            gameOver = false;
            quit = false;
            score = 0;
            speed = 8;
            enemySpeed = 9;

            txtScore.Text = "Score :" + score;

            ChangeEnemy();

            auto.Top = 372;
            prepreka1.Left =375;
            prepreka2.Left = 514;
            lvlTimer.Start();
        }


        private void GameOver()
        {
            lvlTimer.Stop();
            txtScore.Text = "Score : " + score + "\n GAME OVER!  ";
            gameOver = true;
        }

        private void RemoveBullet(PictureBox bullet)
        {
            this.Controls.Remove(bullet);
            bullet.Dispose();  // skroz micemo metak (cvjetic)
        }
        #region zanemari !
        // ovo sam slucajno - treba zanmaerit
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        #endregion


        private void MakeBullet()
        {
            PictureBox bullet = new PictureBox();
            
            Image image = Properties.Resources.bullet;
            bullet.BackgroundImage = image;
            bullet.Height = 20;
            bullet.Width = 20;
            bullet.Left = auto.Left + auto.Width; // moze i auto.Right;
            bullet.Top = auto.Top + auto.Height / 2;
            bullet.Tag = "bullet";
            this.Controls.Add(bullet);

               
        }

        private void ChangeEnemy()
        {
            if (index>3)
            {
                index = 1;
            }
            else
            {
                index++;
            }

            switch(index)
            {
                case 1:
                    enemy.Image = Properties.Resources.neprijatelj2;
                    break;
                case 2:
                    enemy.Image = Properties.Resources.neprijatelj2;
                    break;
                case 3:
                    enemy.Image = Properties.Resources.neprijatelj2;
                    break;
            }
            enemy.Left = 1000; //micemo ga sa scene
            if(score%2==0)
            {
                enemy.Top = rand.Next(20, 300);
            }
            else
            {
                enemy.Top = rand.Next(450, this.ClientSize.Height - enemy.Height);
            }
              

        }



        #endregion



        public Form1()
        {
            InitializeComponent();

        }




    }
}
