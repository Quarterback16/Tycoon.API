namespace HSDC
{
    partial class FormMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.deck = new System.Windows.Forms.FlowLayoutPanel();
            this.labelNumCards = new System.Windows.Forms.Label();
            this.labelTopdeck = new System.Windows.Forms.Label();
            this.buttonMenu = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonMemoryReader = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deck
            // 
            this.deck.AutoSize = true;
            this.deck.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.deck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deck.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.deck.Location = new System.Drawing.Point(0, 0);
            this.deck.Margin = new System.Windows.Forms.Padding(10);
            this.deck.Name = "deck";
            this.deck.Size = new System.Drawing.Size(174, 65);
            this.deck.TabIndex = 0;
            this.deck.Resize += new System.EventHandler(this.deck_Resize);
            // 
            // labelNumCards
            // 
            this.labelNumCards.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNumCards.Location = new System.Drawing.Point(1, 1);
            this.labelNumCards.Name = "labelNumCards";
            this.labelNumCards.Size = new System.Drawing.Size(50, 25);
            this.labelNumCards.TabIndex = 8;
            this.labelNumCards.Text = "0 / 30";
            this.labelNumCards.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTopdeck
            // 
            this.labelTopdeck.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTopdeck.Location = new System.Drawing.Point(52, 1);
            this.labelTopdeck.Name = "labelTopdeck";
            this.labelTopdeck.Size = new System.Drawing.Size(50, 25);
            this.labelTopdeck.TabIndex = 10;
            this.labelTopdeck.Text = "00 %";
            this.labelTopdeck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonMenu
            // 
            this.buttonMenu.BackColor = System.Drawing.Color.Gray;
            this.buttonMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMenu.BackgroundImage")));
            this.buttonMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMenu.ForeColor = System.Drawing.Color.Black;
            this.buttonMenu.Location = new System.Drawing.Point(129, 1);
            this.buttonMenu.Name = "buttonMenu";
            this.buttonMenu.Size = new System.Drawing.Size(25, 25);
            this.buttonMenu.TabIndex = 11;
            this.buttonMenu.UseVisualStyleBackColor = false;
            this.buttonMenu.Click += new System.EventHandler(this.buttonMenu_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonMemoryReader);
            this.panel1.Controls.Add(this.buttonReset);
            this.panel1.Controls.Add(this.buttonMenu);
            this.panel1.Controls.Add(this.labelTopdeck);
            this.panel1.Controls.Add(this.labelNumCards);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 27);
            this.panel1.TabIndex = 12;
            // 
            // buttonMemoryReader
            // 
            this.buttonMemoryReader.BackColor = System.Drawing.Color.White;
            this.buttonMemoryReader.Enabled = false;
            this.buttonMemoryReader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMemoryReader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMemoryReader.ForeColor = System.Drawing.Color.Black;
            this.buttonMemoryReader.Location = new System.Drawing.Point(418, 9);
            this.buttonMemoryReader.Name = "buttonMemoryReader";
            this.buttonMemoryReader.Size = new System.Drawing.Size(72, 25);
            this.buttonMemoryReader.TabIndex = 14;
            this.buttonMemoryReader.Text = "MR = OFF";
            this.buttonMemoryReader.UseVisualStyleBackColor = false;
            this.buttonMemoryReader.Visible = false;
            this.buttonMemoryReader.Click += new System.EventHandler(this.buttonMemoryReader_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.BackColor = System.Drawing.Color.IndianRed;
            this.buttonReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonReset.BackgroundImage")));
            this.buttonReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReset.ForeColor = System.Drawing.Color.Black;
            this.buttonReset.Location = new System.Drawing.Point(103, 1);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(25, 25);
            this.buttonReset.TabIndex = 13;
            this.buttonReset.UseVisualStyleBackColor = false;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(174, 92);
            this.Controls.Add(this.deck);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(180, 120);
            this.Name = "FormMain";
            this.Text = "Hearthstone Deck Counter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel deck;
        private System.Windows.Forms.Label labelNumCards;
        private System.Windows.Forms.Label labelTopdeck;
        private System.Windows.Forms.Button buttonMenu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonMemoryReader;
    }
}

