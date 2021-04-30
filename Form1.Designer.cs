namespace OTTER
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.auto = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.lvlTimer = new System.Windows.Forms.Timer(this.components);
            this.prepreka1 = new System.Windows.Forms.PictureBox();
            this.prepreka2 = new System.Windows.Forms.PictureBox();
            this.enemy = new System.Windows.Forms.PictureBox();
            this.txtScore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.auto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.prepreka1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.prepreka2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemy)).BeginInit();
            this.SuspendLayout();
            // 
            // auto
            // 
            this.auto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("auto.BackgroundImage")));
            this.auto.Location = new System.Drawing.Point(17, 372);
            this.auto.Name = "auto";
            this.auto.Size = new System.Drawing.Size(79, 41);
            this.auto.TabIndex = 0;
            this.auto.TabStop = false;
            // 
            // lvlTimer
            // 
            this.lvlTimer.Enabled = true;
            this.lvlTimer.Interval = 20;
            this.lvlTimer.Tick += new System.EventHandler(this.MainGameTimerEvent);
            // 
            // prepreka1
            // 
            this.prepreka1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.prepreka1.Location = new System.Drawing.Point(375, 478);
            this.prepreka1.Name = "prepreka1";
            this.prepreka1.Size = new System.Drawing.Size(34, 133);
            this.prepreka1.TabIndex = 1;
            this.prepreka1.TabStop = false;
            this.prepreka1.Tag = "prepreka";
            // 
            // prepreka2
            // 
            this.prepreka2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.prepreka2.Location = new System.Drawing.Point(514, 0);
            this.prepreka2.Name = "prepreka2";
            this.prepreka2.Size = new System.Drawing.Size(34, 151);
            this.prepreka2.TabIndex = 2;
            this.prepreka2.TabStop = false;
            this.prepreka2.Tag = "prepreka";
            // 
            // enemy
            // 
            this.enemy.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("enemy.BackgroundImage")));
            this.enemy.Location = new System.Drawing.Point(847, 275);
            this.enemy.Name = "enemy";
            this.enemy.Size = new System.Drawing.Size(74, 27);
            this.enemy.TabIndex = 3;
            this.enemy.TabStop = false;
            // 
            // txtScore
            // 
            this.txtScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.txtScore.Font = new System.Drawing.Font("Bowlby One SC", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScore.Location = new System.Drawing.Point(12, 9);
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(184, 93);
            this.txtScore.TabIndex = 4;
            this.txtScore.Text = "Score : 0";
            // 
            // Form1
            // 
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(951, 611);
            this.Controls.Add(this.txtScore);
            this.Controls.Add(this.enemy);
            this.Controls.Add(this.prepreka2);
            this.Controls.Add(this.prepreka1);
            this.Controls.Add(this.auto);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyIsDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyIsUp);
            ((System.ComponentModel.ISupportInitialize)(this.auto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.prepreka1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.prepreka2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox auto;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer lvlTimer;
        private System.Windows.Forms.PictureBox prepreka1;
        private System.Windows.Forms.PictureBox prepreka2;
        private System.Windows.Forms.PictureBox enemy;
        private System.Windows.Forms.Label txtScore;
    }
}
