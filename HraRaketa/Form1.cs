using System;
using System.Drawing;
using System.Windows.Forms;

namespace HraRaketa
{
    public partial class Form1 : Form
    {
        int raketaRychlost = 10;
        int meteorrychlost = 10;
        int skore = 0;
        int zivoty = 3;
        int palivo = 100;
        Random rnd = new Random();

        bool pohybVlevo = false;
        bool pohybVpravo = false;

        PictureBox[] meteority;
        PictureBox[] srdce;
        Timer t = new Timer();

        
        Button buttonStart;
        Button buttonStop;
        Button buttonReset;
        TrackBar trackBarRychlost;
        ProgressBar progressBarPalivo;
        Label labelPalivo;
        Label labelInfo;
        Label labelRychlost;
        GroupBox groupOvl;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += Okno_KeyDown;
            this.KeyUp += Okno_KeyUp;

            
            pictureBox_raketa.Left = pozadi.Width / 2 - pictureBox_raketa.Width / 2;
            pictureBox_raketa.Top = pozadi.Height - pictureBox_raketa.Height - 10;

            
            PictureBox meteorit2 = new PictureBox();
            meteorit2.Size = meteorit.Size;
            meteorit2.SizeMode = PictureBoxSizeMode.Zoom;
            meteorit2.Image = meteorit.Image;
            pozadi.Controls.Add(meteorit2);

            meteority = new PictureBox[] { meteorit, meteorit2 };
            foreach (var m in meteority)
                ResetMeteorit(m);

            
            srdce = new PictureBox[] { pictureBoxzivoty1, pictureBoxzivoty2, pictureBoxzivoty3 };

            
            t.Interval = 50;
            t.Tick += GameLoop;

            
            groupOvl = new GroupBox();
            groupOvl.Text = "🕹️ Ovládání";
            groupOvl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            groupOvl.Width = 230;
            groupOvl.Height = 320;
            groupOvl.Left = pozadi.Right + 10;
            groupOvl.Top = 20;
            groupOvl.BackColor = Color.FromArgb(245, 245, 255);
            this.Controls.Add(groupOvl);

            int left = 25;
            int width = 170;

            
            buttonStart = new Button();
            buttonStart.Text = "▶️ Start";
            buttonStart.Top = 30;
            buttonStart.Left = left;
            buttonStart.Width = width;
            buttonStart.BackColor = Color.LightGreen;
            buttonStart.Click += ButtonStart_Click;
            groupOvl.Controls.Add(buttonStart);

            
            buttonStop = new Button();
            buttonStop.Text = "⏸️ Stop";
            buttonStop.Top = 70;
            buttonStop.Left = left;
            buttonStop.Width = width;
            buttonStop.BackColor = Color.LightCoral;
            buttonStop.Click += ButtonStop_Click;
            groupOvl.Controls.Add(buttonStop);

            
            buttonReset = new Button();
            buttonReset.Text = "🔄 Reset";
            buttonReset.Top = 110;
            buttonReset.Left = left;
            buttonReset.Width = width;
            buttonReset.BackColor = Color.LightSteelBlue;
            buttonReset.Click += ButtonReset_Click;
            groupOvl.Controls.Add(buttonReset);

            
            labelRychlost = new Label();
            labelRychlost.Text = "🚀 Rychlost rakety:";
            labelRychlost.Top = 150;
            labelRychlost.Left = left;
            labelRychlost.Width = width;
            groupOvl.Controls.Add(labelRychlost);

            
            trackBarRychlost = new TrackBar();
            trackBarRychlost.Minimum = 5;
            trackBarRychlost.Maximum = 20;
            trackBarRychlost.Value = raketaRychlost;
            trackBarRychlost.Top = 170;
            trackBarRychlost.Left = left;
            trackBarRychlost.Width = width;
            trackBarRychlost.Scroll += TrackBarRychlost_Scroll;
            groupOvl.Controls.Add(trackBarRychlost);

            
            labelPalivo = new Label();
            labelPalivo.Text = "⛽ Palivo:";
            labelPalivo.Top = 210;
            labelPalivo.Left = left;
            labelPalivo.Width = width;
            groupOvl.Controls.Add(labelPalivo);

            
            progressBarPalivo = new ProgressBar();
            progressBarPalivo.Top = 230;
            progressBarPalivo.Left = left;
            progressBarPalivo.Width = width;
            progressBarPalivo.Maximum = 100;
            progressBarPalivo.Value = palivo;
            progressBarPalivo.ForeColor = Color.LimeGreen;
            groupOvl.Controls.Add(progressBarPalivo);

      
            labelInfo = new Label();
            labelInfo.Font = new Font("Consolas", 10, FontStyle.Bold);
            labelInfo.ForeColor = Color.Navy;
            labelInfo.Top = 265;
            labelInfo.Left = left;
            labelInfo.Width = width + 20;
            labelInfo.Text = "Skóre: 0 | ❤️ 3 | Palivo: 100%";
            groupOvl.Controls.Add(labelInfo);
        }

        
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            t.Start();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            t.Stop();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            skore = 0;
            zivoty = 3;
            palivo = 100;
            foreach (var m in meteority)
                ResetMeteorit(m);
            pictureBox_raketa.Left = pozadi.Width / 2 - pictureBox_raketa.Width / 2;
            progressBarPalivo.Value = palivo;
            labelInfo.Text = $"Skóre: {skore} | ❤️ {zivoty} | Palivo: {palivo}%";
            t.Start();
        }

        private void TrackBarRychlost_Scroll(object sender, EventArgs e)
        {
            raketaRychlost = trackBarRychlost.Value;
        }

        private void Okno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) pohybVlevo = true;
            if (e.KeyCode == Keys.Right) pohybVpravo = true;
        }

        private void Okno_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) pohybVlevo = false;
            if (e.KeyCode == Keys.Right) pohybVpravo = false;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (pohybVlevo && pictureBox_raketa.Left > 0 && palivo > 0)
            {
                pictureBox_raketa.Left -= raketaRychlost;
                palivo--;
            }
            if (pohybVpravo && pictureBox_raketa.Right < pozadi.Width && palivo > 0)
            {
                pictureBox_raketa.Left += raketaRychlost;
                palivo--;
            }

            palivo = Math.Max(0, Math.Min(palivo, 100));
            progressBarPalivo.Value = palivo;

            for (int i = 0; i < meteority.Length; i++)
                MoveMeteorit(meteority[i]);

            for (int i = 0; i < srdce.Length; i++)
                srdce[i].Visible = (i < zivoty);

            labelInfo.Text = $"Skóre: {skore} | ❤️ {zivoty} | Palivo: {palivo}%";
        }

        private void MoveMeteorit(PictureBox meteorit)
        {
            meteorit.Top += meteorrychlost;

            if (meteorit.Bounds.IntersectsWith(pictureBox_raketa.Bounds))
            {
                zivoty--;
                palivo = 100;
                ResetMeteorit(meteorit);

                if (zivoty <= 0)
                {
                    t.Stop();
                    MessageBox.Show($"💥 Konec hry!\nSkóre: {skore}", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (meteorit.Top > pozadi.Height)
            {
                ResetMeteorit(meteorit);
                skore++;
            }
        }

        private void ResetMeteorit(PictureBox meteorit)
        {
            meteorit.Top = -meteorit.Height;
            meteorit.Left = rnd.Next(0, pozadi.Width - meteorit.Width);
        }

        private void txtscore_Click(object sender, EventArgs e)
        {
            
        }
    }
}
