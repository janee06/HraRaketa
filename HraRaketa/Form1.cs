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
            t.Start();
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

            palivo = Math.Max(palivo, 0);

            for (int i = 0; i < meteority.Length; i++)
                MoveMeteorit(meteority[i]);

           
            for (int i = 0; i < srdce.Length; i++)
                srdce[i].Visible = (i < zivoty);

           
            Boxskore.Text = "Skóre: " + skore;
            Boxpalivo.Text = "Palivo: " + palivo + "%";
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
                    MessageBox.Show("Konec hry!\nSkóre: " + skore);
                    this.Close();
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
