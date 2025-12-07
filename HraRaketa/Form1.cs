using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

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

        // GUI prvky
        Button buttonStart;
        Button buttonStop;
        Button buttonReset;
        Button buttonVysledky;
        TrackBar trackBarRychlost;
        ProgressBar progressBarPalivo;
        Label labelPalivo;
        Label labelInfo;
        Label labelRychlost;
        GroupBox groupOvl;

        public Form1()
        {
            InitializeComponent();
            SQLitePCL.Batteries.Init();

            this.KeyPreview = true;
            this.KeyDown += Okno_KeyDown;
            this.KeyUp += Okno_KeyUp;

            // RAKETA
            pictureBox_raketa.Left = pozadi.Width / 2 - pictureBox_raketa.Width / 2;
            pictureBox_raketa.Top = pozadi.Height - pictureBox_raketa.Height - 10;

            // METEORITY
            PictureBox meteorit2 = new PictureBox();
            meteorit2.Size = meteorit.Size;
            meteorit2.SizeMode = PictureBoxSizeMode.Zoom;
            meteorit2.Image = meteorit.Image;
            pozadi.Controls.Add(meteorit2);

            meteority = new PictureBox[] { meteorit, meteorit2 };
            foreach (var m in meteority)
                ResetMeteorit(m);

            // SRDCE
            srdce = new PictureBox[] { pictureBoxzivoty1, pictureBoxzivoty2, pictureBoxzivoty3 };

            // TIMER
            t.Interval = 50;
            t.Tick += GameLoop;

            // ====== Vytvoření tabulky v DB ======
            using (var conn = new SqliteConnection("Data Source=database.db"))
            {
                conn.Open();

                string sql = @"CREATE TABLE IF NOT EXISTS ScoreLog(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Jmeno TEXT,
                    Skore INT,
                    Zivoty INT,
                    Palivo INT,
                    Datum TEXT
                )";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // ====== OVLÁDACÍ PANEL ======
            groupOvl = new GroupBox();
            groupOvl.Text = "🕹️ Ovládání";
            groupOvl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            groupOvl.Width = 230;
            groupOvl.Height = 380;
            groupOvl.Left = pozadi.Right + 10;
            groupOvl.Top = 20;
            groupOvl.BackColor = Color.FromArgb(245, 245, 255);
            this.Controls.Add(groupOvl);

            int left = 25;
            int width = 170;

            // START
            buttonStart = new Button();
            buttonStart.Text = "▶️ Start";
            buttonStart.Top = 80;
            buttonStart.Left = left;
            buttonStart.Width = width;
            buttonStart.BackColor = Color.LightGreen;
            buttonStart.Click += ButtonStart_Click;
            groupOvl.Controls.Add(buttonStart);

           
            buttonStop = new Button();
            buttonStop.Text = "⏸️ Stop";
            buttonStop.Top = 120;
            buttonStop.Left = left;
            buttonStop.Width = width;
            buttonStop.BackColor = Color.LightCoral;
            buttonStop.Click += ButtonStop_Click;
            groupOvl.Controls.Add(buttonStop);

           
            buttonReset = new Button();
            buttonReset.Text = "🔄 Reset";
            buttonReset.Top = 160;
            buttonReset.Left = left;
            buttonReset.Width = width;
            buttonReset.BackColor = Color.LightSteelBlue;
            buttonReset.Click += ButtonReset_Click;
            groupOvl.Controls.Add(buttonReset);

           
            labelRychlost = new Label();
            labelRychlost.Text = "🚀 Rychlost rakety:";
            labelRychlost.Top = 200;
            labelRychlost.Left = left;
            groupOvl.Controls.Add(labelRychlost);

            trackBarRychlost = new TrackBar();
            trackBarRychlost.Minimum = 5;
            trackBarRychlost.Maximum = 20;
            trackBarRychlost.Value = raketaRychlost;
            trackBarRychlost.Top = 220;
            trackBarRychlost.Left = left;
            trackBarRychlost.Width = width;
            trackBarRychlost.Scroll += TrackBarRychlost_Scroll;
            groupOvl.Controls.Add(trackBarRychlost);

            
            labelPalivo = new Label();
            labelPalivo.Text = "⛽ Palivo:";
            labelPalivo.Top = 260;
            labelPalivo.Left = left;
            groupOvl.Controls.Add(labelPalivo);

            progressBarPalivo = new ProgressBar();
            progressBarPalivo.Top = 280;
            progressBarPalivo.Left = left;
            progressBarPalivo.Width = width;
            progressBarPalivo.Maximum = 100;
            progressBarPalivo.Value = palivo;
            groupOvl.Controls.Add(progressBarPalivo);

            
            labelInfo = new Label();
            labelInfo.Font = new Font("Consolas", 10, FontStyle.Bold);
            labelInfo.ForeColor = Color.Navy;
            labelInfo.Top = 310;
            labelInfo.Left = left;
            labelInfo.Width = width + 20;
            labelInfo.Text = "Skóre: 0 | ❤️ 3 | Palivo: 100%";
            groupOvl.Controls.Add(labelInfo);

         
            buttonVysledky = new Button();
            buttonVysledky.Text = "📊 Zobrazit výsledky";
            buttonVysledky.Top = 340;
            buttonVysledky.Left = left;
            buttonVysledky.Width = width;
            buttonVysledky.Click += ButtonZobrazVysledky;
            groupOvl.Controls.Add(buttonVysledky);
        }

       
        private void ButtonZobrazVysledky(object sender, EventArgs e)
        {
            FormSkore okno = new FormSkore();
            okno.ShowDialog();
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

        // ===== GAME LOOP =====
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

            palivo = Math.Max(0, palivo);
            progressBarPalivo.Value = palivo;

            foreach (var m in meteority)
                MoveMeteorit(m);

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
                    UlozSkoreDoDatabaze();
                    MessageBox.Show($"💥 Konec hry!\nSkóre: {skore}", "Game Over");
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

        // ===== ULOŽENÍ SKÓRE =====
        private void UlozSkoreDoDatabaze()
        {
            using (var conn = new SqliteConnection("Data Source=database.db"))
            {
                conn.Open();

                string sql = @"
                INSERT INTO ScoreLog (Jmeno, Skore, Zivoty, Palivo, Datum)
                VALUES (@j, @s, @z, @p, @d)";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@j", textBoxJmeno.Text);
                    cmd.Parameters.AddWithValue("@s", skore);
                    cmd.Parameters.AddWithValue("@z", zivoty);
                    cmd.Parameters.AddWithValue("@p", palivo);
                    cmd.Parameters.AddWithValue("@d", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
