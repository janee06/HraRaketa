using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace HraRaketa
{
    public partial class FormSkore : Form
    {
        public FormSkore()
        {
            InitializeComponent();
            SQLitePCL.Batteries.Init();
        }

        private void FormSkore_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_nactiVysledky_Click(object sender, EventArgs e)
        {
            NactiVysledky();
        }

        private void btn_vymazVysledky_Click(object sender, EventArgs e)
        {
            string cesta = "Data Source=database.db";
            using (var conn = new SqliteConnection(cesta))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM ScoreLog";
                cmd.ExecuteNonQuery();
            }
        }

        private void NactiVysledky()
        {
            string cesta = "Data Source=database.db";

            using (var conn = new SqliteConnection(cesta))
            {
                conn.Open();

                string sql = "SELECT Jmeno, Skore, Zivoty, Palivo, Datum FROM Skore ORDER BY Skore DESC";

                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dataGridView1.DataSource = dt;

                  
                    if (dataGridView1.Columns.Count > 0)
                    {
                        dataGridView1.Columns["Jmeno"].HeaderText = "Jméno";
                        dataGridView1.Columns["Skore"].HeaderText = "Skóre";
                        dataGridView1.Columns["Zivoty"].HeaderText = "Životy";
                        dataGridView1.Columns["Palivo"].HeaderText = "Palivo";
                        dataGridView1.Columns["Datum"].HeaderText = "Datum";
                    }
                }
            }
        }

        private void VymazVysledky()
        {
            string cesta = "Data Source=database.db";

            var dotaz = MessageBox.Show(
                "Opravdu chceš vymazat všechny výsledky?",
                "Potvrzení",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dotaz != DialogResult.Yes)
                return;

            using (var conn = new SqliteConnection(cesta))
            {
                conn.Open();

                string sql = "DELETE FROM Skore";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            NactiVysledky();
            MessageBox.Show("Výsledky byly vymazány!","info" , MessageBoxButtons.OK,MessageBoxIcon.Information );
            
        }
    }
}
