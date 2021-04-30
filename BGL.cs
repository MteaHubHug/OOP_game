using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OTTER
{
    /// <summary>
    /// -
    /// </summary>
    public partial class BGL : Form
    {
        /* ------------------- */
        #region Environment Variables

        List<Func<int>> GreenFlagScripts = new List<Func<int>>();

        /// <summary>
        /// Uvjet izvršavanja igre. Ako je <c>START == true</c> igra će se izvršavati.
        /// </summary>
        /// <example><c>START</c> se često koristi za beskonačnu petlju. Primjer metode/skripte:
        /// <code>
        /// private int MojaMetoda()
        /// {
        ///     while(START)
        ///     {
        ///       //ovdje ide kod
        ///     }
        ///     return 0;
        /// }</code>
        /// </example>
        public static bool START = true;

        //sprites
        /// <summary>
        /// Broj likova.
        /// </summary>
        public static int spriteCount = 0, soundCount = 0;

        /// <summary>
        /// Lista svih likova.
        /// </summary>
        //public static List<Sprite> allSprites = new List<Sprite>();
        public static SpriteList<Sprite> allSprites = new SpriteList<Sprite>();

        //sensing
        int mouseX, mouseY;
        Sensing sensing = new Sensing();

        //background
        List<string> backgroundImages = new List<string>();
        int backgroundImageIndex = 0;
        string ISPIS = "";

        SoundPlayer[] sounds = new SoundPlayer[1000];
        TextReader[] readFiles = new StreamReader[1000];
        TextWriter[] writeFiles = new StreamWriter[1000];
        bool showSync = false;
        int loopcount;
        DateTime dt = new DateTime();
        String time;
        double lastTime, thisTime, diff;

        #endregion
        public bool levelup = false;
        Form1 lvl2 = new Form1();
        bool goLeft, goRight, jumping, isGameOver; //svi su inic. false 

        // varijable za igraca
        int jumpSpeed;
        int force;
        int myscore = 0;

        int playerSpeed = 7;
        // varijable (brzine) za platforme koje se pomicu
        int horizontalSpeed = 5;
        int verticalSpeed = 3;

        // varijable brzine neprijatelja
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;

        /* ------------------- */
        #region Events

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            try
            {                
                foreach (Sprite sprite in allSprites)
                {                    
                    if (sprite != null)
                        if (sprite.Show == true)
                        {
                            g.DrawImage(sprite.CurrentCostume, new Rectangle(sprite.X, sprite.Y, sprite.Width, sprite.Heigth));
                        }
                    if (allSprites.Change)
                        break;
                }
                if (allSprites.Change)
                    allSprites.Change = false;
            }
            catch
            {
                //ako se doda sprite dok crta onda se mijenja allSprites
                MessageBox.Show("Greška!");
            }
        }

        private void startTimer(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            Init();
        }

        private void updateFrameRate(object sender, EventArgs e)
        {
            updateSyncRate();
        }

        /// <summary>
        /// Crta tekst po pozornici.
        /// </summary>
        /// <param name="sender">-</param>
        /// <param name="e">-</param>
        public void DrawTextOnScreen(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var brush = new SolidBrush(Color.WhiteSmoke);
            string text = ISPIS;

            SizeF stringSize = new SizeF();
            Font stringFont = new Font("Arial", 14);
            stringSize = e.Graphics.MeasureString(text, stringFont);

            using (Font font1 = stringFont)
            {
                RectangleF rectF1 = new RectangleF(0, 0, stringSize.Width, stringSize.Height);
                e.Graphics.FillRectangle(brush, Rectangle.Round(rectF1));
                e.Graphics.DrawString(text, font1, Brushes.Black, rectF1);
            }
        }

        private void mouseClicked(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = true;
            sensing.MouseDown = true;
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = true;
            sensing.MouseDown = true;            
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = false;
            sensing.MouseDown = false;
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

            //sensing.MouseX = e.X;
            //sensing.MouseY = e.Y;
            //Sensing.Mouse.x = e.X;
            //Sensing.Mouse.y = e.Y;
            sensing.Mouse.X = e.X;
            sensing.Mouse.Y = e.Y;

        }
       

        private void MainGameTimerEvent(object sender, EventArgs e) //glavni dogadjaj - timer - igra traje
        {
            score.Text = "Score: " + myscore;
            player.Top += jumpSpeed;

            if(goLeft== true)
            {
                player.Left -= playerSpeed;
            }
            if (goRight==true)
            {
                player.Left += playerSpeed;
            }

            if (jumping ==true && force<0)
            {
                jumping = false;
            }

            if(jumping==true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10; 
            }

            foreach (Control x in this.Controls)
            {
                #region Igracev odnos s platformama,bocama i neprijateljima
                if (x is PictureBox)
                {
                    // fora s castanjem 
                    if((string)x.Tag== "platform")
                    {

                        if(player.Bounds.IntersectsWith(x.Bounds))//ovo je da igrac moze skocit s platforme
                        {
                            force = 8;
                            player.Top = x.Top - player.Height; //ovo je da bude tocno iznad platforme kad skoci na nju

                            #region Gravitacija Pomicne Horizontalne Platforme
                            if((string)x.Name=="horizontalPlatform" && goLeft==false || (string)x.Name == "horizontalPlatform" && goRight == false)
                            {
                                player.Left -= horizontalSpeed; //micemo igraca zajedno s platformom brzinom platforme
                            }
                            #endregion

                        }

                        x.BringToFront(); // platforme ispred sveg ostalog

                    }


                    if ((string)x.Tag=="bottle")
                    {
                        if(player.Bounds.IntersectsWith(x.Bounds) && x.Visible==true)
                        {
                            x.Visible = false;
                            myscore++;
                        }
                    }

                    if (myscore >= 10) portal.Visible = true; 
                    else portal.Visible = false;

                    if((string)x.Tag=="enemy")
                    {
                        if(player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gameTimer.Stop(); // ako nas neprijatelj dodirne - igra je gotova
                            isGameOver = true;
                            score.Text = "RIP Duxy !" + Environment.NewLine + "RIP planet Earth :'( ";
                        }
                    }

                }
                #endregion
            }
            #region Kretanje horizontalne i vertikalne pomicne platforme
            horizontalPlatform.Left -= horizontalSpeed;
            //dosla je do kraja, nek se mice nalijevo... .Left je pocetak s lijeve strane + sirina > sirina forme -> rikverc
            if (horizontalPlatform.Left<0 || (horizontalPlatform.Left + horizontalPlatform.Width)>this.ClientSize.Width)     
            {
                horizontalSpeed = -horizontalSpeed; //obrnem brzinu
            }
           
            verticalPlatform.Top += verticalSpeed;
            if(verticalPlatform.Top<233 || verticalPlatform.Top>498)
            {
                verticalSpeed = -verticalSpeed;
            }
            #endregion

            #region Kretanje neprijatelja 

            enemyOne.Left -= enemyOneSpeed;
            // da nam neprijatelji ne upadnu sa svoje platforme !
            if (enemyOne.Left<pictureBox6.Left || (enemyOne.Left+enemyOne.Width)>pictureBox6.Right)  
            {
                enemyOneSpeed = -enemyOneSpeed;  
            }

            enemyTwo.Left += enemyTwoSpeed;

            if(enemyTwo.Left<pictureBox2.Left || (enemyTwo.Right)>pictureBox2.Right)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }
            #endregion

            #region Padanje u ponor - gubitak zivota
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                gameTimer.Stop();
                isGameOver = true;
                score.Text = "RIP Duxy!" + Environment.NewLine + "Try stay on platform next time !";
            }
            #endregion

            #region Portal - iduci LEVEL!

            if (player.Bounds.IntersectsWith(portal.Bounds) && myscore>=10) //stavi ovdje 10 kasnije!
            {
                gameTimer.Stop();
                isGameOver = true; 
                score.Text = "NEXT LEVEL !";
                ////prelazak na iduci lvl : 
                levelup = true;
                //Close();
   
            }
            #endregion

            if(levelup==true)
            {
               // Application.Exit();
                this.Hide();
                lvl2.Show();
                
            }


        }

        private void keyDown(object sender, KeyEventArgs e) //dogadjaj - kada stisnem tipke
        {
            //sensing.Key = e.KeyCode.ToString();
            //sensing.KeyPressedTest = true;
            if(e.KeyCode==Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if(e.KeyCode==Keys.Space && jumping==false) //nema visestrukog skakanja!
            {
                jumping = true;
            }
        }

        private void keyUp(object sender, KeyEventArgs e) //dogadjaj - kada pustim tipke
        {
            //sensing.Key = "";
            //sensing.KeyPressedTest = false;
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if(jumping==true )
            {
                jumping = false;
            }

            if(e.KeyCode == Keys.Enter && isGameOver==true) //ovako mogu restartirati usred igre
            {
                RestartGame();
            }

        }

        private void Update(object sender, EventArgs e)
        {
            if (sensing.KeyPressed(Keys.Escape))
            {
                START = false;
            }

            if (START)
            {
                this.Refresh();
            }
        }

        #endregion
        /* ------------------- */
        #region Start of Game Methods

        //my
        #region my

        //private void StartScriptAndWait(Func<int> scriptName)
        //{
        //    Task t = Task.Factory.StartNew(scriptName);
        //    t.Wait();
        //}

        //private void StartScript(Func<int> scriptName)
        //{
        //    Task t;
        //    t = Task.Factory.StartNew(scriptName);
        //}

        private int AnimateBackground(int intervalMS)
        {
            while (START)
            {
                setBackgroundPicture(backgroundImages[backgroundImageIndex]);
                Game.WaitMS(intervalMS);
                backgroundImageIndex++;
                if (backgroundImageIndex == 3)
                    backgroundImageIndex = 0;
            }
            return 0;
        }

        private void KlikNaZastavicu()
        {
            foreach (Func<int> f in GreenFlagScripts)
            {
                Task.Factory.StartNew(f);
            }
        }

        #endregion

        /// <summary>
        /// BGL
        /// </summary>
        public BGL()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pričekaj (pauza) u sekundama.
        /// </summary>
        /// <example>Pričekaj pola sekunde: <code>Wait(0.5);</code></example>
        /// <param name="sekunde">Realan broj.</param>
        public void Wait(double sekunde)
        {
            int ms = (int)(sekunde * 1000);
            Thread.Sleep(ms);
        }

        //private int SlucajanBroj(int min, int max)
        //{
        //    Random r = new Random();
        //    int br = r.Next(min, max + 1);
        //    return br;
        //}

        /// <summary>
        /// -
        /// </summary>
        public void Init()
        {
            if (dt == null) time = dt.TimeOfDay.ToString();
            loopcount++;
            //Load resources and level here
            this.Paint += new PaintEventHandler(DrawTextOnScreen);
            SetupGame();
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="val">-</param>
        public void showSyncRate(bool val)
        {
            showSync = val;
            if (val == true) syncRate.Show();
            if (val == false) syncRate.Hide();
        }

        /// <summary>
        /// -
        /// </summary>
        public void updateSyncRate()
        {
            if (showSync == true)
            {
                thisTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
                diff = thisTime - lastTime;
                lastTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

                double fr = (1000 / diff) / 1000;

                int fr2 = Convert.ToInt32(fr);

                syncRate.Text = fr2.ToString();
            }

        }

        //stage
        #region Stage

        /// <summary>
        /// Postavi naslov pozornice.
        /// </summary>
        /// <param name="title">tekst koji će se ispisati na vrhu (naslovnoj traci).</param>
        public void SetStageTitle(string title)
        {
            this.Text = title;
        }

        /// <summary>
        /// Postavi boju pozadine.
        /// </summary>
        /// <param name="r">r</param>
        /// <param name="g">g</param>
        /// <param name="b">b</param>
        public void setBackgroundColor(int r, int g, int b)
        {
            this.BackColor = Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Postavi boju pozornice. <c>Color</c> je ugrađeni tip.
        /// </summary>
        /// <param name="color"></param>
        public void setBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        /// <summary>
        /// Postavi sliku pozornice.
        /// </summary>
        /// <param name="backgroundImage">Naziv (putanja) slike.</param>
        public void setBackgroundPicture(string backgroundImage)
        {
            this.BackgroundImage = new Bitmap(backgroundImage);
        }

        /// <summary>
        /// Izgled slike.
        /// </summary>
        /// <param name="layout">none, tile, stretch, center, zoom</param>
        public void setPictureLayout(string layout)
        {
            if (layout.ToLower() == "none") this.BackgroundImageLayout = ImageLayout.None;
            if (layout.ToLower() == "tile") this.BackgroundImageLayout = ImageLayout.Tile;
            if (layout.ToLower() == "stretch") this.BackgroundImageLayout = ImageLayout.Stretch;
            if (layout.ToLower() == "center") this.BackgroundImageLayout = ImageLayout.Center;
            if (layout.ToLower() == "zoom") this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        #endregion

        //sound
        #region sound methods

        /// <summary>
        /// Učitaj zvuk.
        /// </summary>
        /// <param name="soundNum">-</param>
        /// <param name="file">-</param>
        public void loadSound(int soundNum, string file)
        {
            soundCount++;
            sounds[soundNum] = new SoundPlayer(file);
        }

        /// <summary>
        /// Sviraj zvuk.
        /// </summary>
        /// <param name="soundNum">-</param>
        public void playSound(int soundNum)
        {
            sounds[soundNum].Play();
        }

        /// <summary>
        /// loopSound
        /// </summary>
        /// <param name="soundNum">-</param>
        public void loopSound(int soundNum)
        {
            sounds[soundNum].PlayLooping();
        }

        /// <summary>
        /// Zaustavi zvuk.
        /// </summary>
        /// <param name="soundNum">broj</param>
        public void stopSound(int soundNum)
        {
            sounds[soundNum].Stop();
        }

        #endregion

        //file
        #region file methods

        /// <summary>
        /// Otvori datoteku za čitanje.
        /// </summary>
        /// <param name="fileName">naziv datoteke</param>
        /// <param name="fileNum">broj</param>
        public void openFileToRead(string fileName, int fileNum)
        {
            readFiles[fileNum] = new StreamReader(fileName);
        }

        /// <summary>
        /// Zatvori datoteku.
        /// </summary>
        /// <param name="fileNum">broj</param>
        public void closeFileToRead(int fileNum)
        {
            readFiles[fileNum].Close();
        }

        /// <summary>
        /// Otvori datoteku za pisanje.
        /// </summary>
        /// <param name="fileName">naziv datoteke</param>
        /// <param name="fileNum">broj</param>
        public void openFileToWrite(string fileName, int fileNum)
        {
            writeFiles[fileNum] = new StreamWriter(fileName);
        }

        /// <summary>
        /// Zatvori datoteku.
        /// </summary>
        /// <param name="fileNum">broj</param>
        public void closeFileToWrite(int fileNum)
        {
            writeFiles[fileNum].Close();
        }

        /// <summary>
        /// Zapiši liniju u datoteku.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <param name="line">linija</param>
        public void writeLine(int fileNum, string line)
        {
            writeFiles[fileNum].WriteLine(line);
        }

        /// <summary>
        /// Pročitaj liniju iz datoteke.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <returns>vraća pročitanu liniju</returns>
        public string readLine(int fileNum)
        {
            return readFiles[fileNum].ReadLine();
        }

        /// <summary>
        /// Čita sadržaj datoteke.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <returns>vraća sadržaj</returns>
        public string readFile(int fileNum)
        {
            return readFiles[fileNum].ReadToEnd();
        }

        #endregion

        //mouse & keys
        #region mouse methods

        /// <summary>
        /// Sakrij strelicu miša.
        /// </summary>
        public void hideMouse()
        {
            Cursor.Hide();
        }

        /// <summary>
        /// Pokaži strelicu miša.
        /// </summary>
        public void showMouse()
        {
            Cursor.Show();
        }

        /// <summary>
        /// Provjerava je li miš pritisnut.
        /// </summary>
        /// <returns>true/false</returns>
        public bool isMousePressed()
        {
            //return sensing.MouseDown;
            return sensing.MouseDown;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void score_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

      

        /// <summary>
        /// Provjerava je li tipka pritisnuta.
        /// </summary>
        /// <param name="key">naziv tipke</param>
        /// <returns></returns>
        public bool isKeyPressed(string key)
        {
            if (sensing.Key == key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Provjerava je li tipka pritisnuta.
        /// </summary>
        /// <param name="key">tipka</param>
        /// <returns>true/false</returns>
        public bool isKeyPressed(Keys key)
        {
            if (sensing.Key == key.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion
        /* ------------------- */

        /* ------------ GAME CODE START ------------ */

        /* Game variables */

        
        /* Initialization */

        
        private void SetupGame()
        {
            //1. setup stage
            SetStageTitle("PMF");
            //setBackgroundColor(Color.WhiteSmoke);            
            setBackgroundPicture("backgrounds\\back.jpg");
            //none, tile, stretch, center, zoom
            setPictureLayout("stretch");

            //2. add sprites
        
        
            //3. scripts that start
        }

        /* Scripts */


        
        private void RestartGame()
        {
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            myscore = 0;
           
            score.Text = "Score : " + myscore;
            
            foreach (Control x in this.Controls) 
            {
                if (x is PictureBox && x.Visible==false) //kada pokupimo boce, one se vise ne vide - trebaju se o5 sve vidit
                {
                    x.Visible = true;
                }
            }

            // resetiram poziciju igraca, platformi i neprijatelj
            player.Left = 32; //ovo su X i Y koordinate igraca, vracam ih tamo gdje sam inicijalno postavila igraca
            player.Top = 575;

            enemyOne.Left = 325;
            enemyTwo.Left = 367;

            horizontalPlatform.Left = 357;
            verticalPlatform.Top = 492 ;

            gameTimer.Start();  // takodjer ugradjena metoda! 

        }



        private int Metoda()
        {
            while (START) //ili neki drugi uvjet
            {

                Wait(0.1);
            }
            return 0;
        }



        /* ------------ GAME CODE END ------------ */


    }
}
