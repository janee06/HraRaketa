namespace HraRaketa
{
    partial class FormSkore
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_nactiVysledky = new System.Windows.Forms.Button();
            this.btn_vymazVysledky = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(31, 29);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(746, 326);
            this.dataGridView1.TabIndex = 0;
            // 
            // btn_nactiVysledky
            // 
            this.btn_nactiVysledky.Location = new System.Drawing.Point(31, 379);
            this.btn_nactiVysledky.Name = "btn_nactiVysledky";
            this.btn_nactiVysledky.Size = new System.Drawing.Size(139, 37);
            this.btn_nactiVysledky.TabIndex = 1;
            this.btn_nactiVysledky.Text = "Načti výsledky";
            this.btn_nactiVysledky.UseVisualStyleBackColor = true;
            this.btn_nactiVysledky.Click += new System.EventHandler(this.btn_nactiVysledky_Click);
            // 
            // btn_vymazVysledky
            // 
            this.btn_vymazVysledky.Location = new System.Drawing.Point(638, 379);
            this.btn_vymazVysledky.Name = "btn_vymazVysledky";
            this.btn_vymazVysledky.Size = new System.Drawing.Size(139, 37);
            this.btn_vymazVysledky.TabIndex = 2;
            this.btn_vymazVysledky.Text = "Vymaž výsledky";
            this.btn_vymazVysledky.UseVisualStyleBackColor = true;
            // 
            // FormSkore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_vymazVysledky);
            this.Controls.Add(this.btn_nactiVysledky);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormSkore";
            this.Text = "FormSkore";
            this.Load += new System.EventHandler(this.FormSkore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_nactiVysledky;
        private System.Windows.Forms.Button btn_vymazVysledky;
    }
}