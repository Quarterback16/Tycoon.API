namespace HSDC
{
    partial class FormMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMenu));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxRemoveCard = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxAddCard = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxConfirmOnReset = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.checkBoxOnTop = new System.Windows.Forms.CheckBox();
            this.checkBoxCardAutoSort = new System.Windows.Forms.CheckBox();
            this.checkBoxCardShowTD = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numericUpDownCardDarken = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.checkBoxSwapMouse = new System.Windows.Forms.CheckBox();
            this.checkBoxCardColoredName = new System.Windows.Forms.CheckBox();
            this.checkBoxCardColorByType = new System.Windows.Forms.CheckBox();
            this.checkBoxCardShowImage = new System.Windows.Forms.CheckBox();
            this.buttonLoadFromUrl = new System.Windows.Forms.Button();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxSortThird = new System.Windows.Forms.ComboBox();
            this.comboBoxSortSecond = new System.Windows.Forms.ComboBox();
            this.comboBoxSortFirst = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCardDarken)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.buttonClear);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxRemoveCard);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxAddCard);
            this.groupBox1.Controls.Add(this.buttonSave);
            this.groupBox1.Controls.Add(this.buttonLoad);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(335, 159);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manage cards in the current deck";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(312, 44);
            this.label3.TabIndex = 9;
            this.label3.Text = "Add cards to the deck by inputting their name; the textbox will auto-complete. To" +
    " remove a card form the deck, select its entry in the \"Remove\"-combobox.";
            // 
            // buttonClear
            // 
            this.buttonClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClear.Location = new System.Drawing.Point(225, 121);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(100, 25);
            this.buttonClear.TabIndex = 8;
            this.buttonClear.Text = "Clear deck";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Remove:";
            // 
            // comboBoxRemoveCard
            // 
            this.comboBoxRemoveCard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRemoveCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxRemoveCard.FormattingEnabled = true;
            this.comboBoxRemoveCard.Location = new System.Drawing.Point(66, 94);
            this.comboBoxRemoveCard.Name = "comboBoxRemoveCard";
            this.comboBoxRemoveCard.Size = new System.Drawing.Size(257, 21);
            this.comboBoxRemoveCard.TabIndex = 6;
            this.comboBoxRemoveCard.SelectedIndexChanged += new System.EventHandler(this.comboBoxRemoveCard_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Add:";
            // 
            // textBoxAddCard
            // 
            this.textBoxAddCard.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxAddCard.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxAddCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAddCard.Location = new System.Drawing.Point(66, 65);
            this.textBoxAddCard.Name = "textBoxAddCard";
            this.textBoxAddCard.Size = new System.Drawing.Size(257, 20);
            this.textBoxAddCard.TabIndex = 4;
            this.textBoxAddCard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxAddCard_KeyDown);
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(119, 121);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 25);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save deck...";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoad.Location = new System.Drawing.Point(10, 121);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(100, 25);
            this.buttonLoad.TabIndex = 6;
            this.buttonLoad.Text = "Load deck...";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 374);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(619, 89);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "About";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(10, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 19);
            this.label12.TabIndex = 13;
            this.label12.Text = "BitCoin:";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(347, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 19);
            this.label5.TabIndex = 12;
            this.label5.Text = "PayPal:";
            // 
            // textBox2
            // 
            this.textBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(397, 59);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(216, 20);
            this.textBox2.TabIndex = 11;
            this.textBox2.Text = "weazel77@gmail.com";
            // 
            // textBox1
            // 
            this.textBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(63, 58);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(216, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "1EHaAiW7JFo2JN1hhFAbaLiu9nK6f5tSXX";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(607, 33);
            this.label6.TabIndex = 9;
            this.label6.Text = "For support and feature requests, write an email to weazel77@gmail.com. If you wo" +
    "uld like to support the development of tool, feel free to donate to:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Location = new System.Drawing.Point(170, 48);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(155, 20);
            this.numericUpDown1.TabIndex = 14;
            this.numericUpDown1.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Card size in px:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxConfirmOnReset);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.comboBoxLanguage);
            this.groupBox4.Controls.Add(this.checkBoxOnTop);
            this.groupBox4.Controls.Add(this.numericUpDown1);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 241);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(335, 127);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tool settings";
            // 
            // checkBoxConfirmOnReset
            // 
            this.checkBoxConfirmOnReset.AutoSize = true;
            this.checkBoxConfirmOnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxConfirmOnReset.Location = new System.Drawing.Point(9, 97);
            this.checkBoxConfirmOnReset.Name = "checkBoxConfirmOnReset";
            this.checkBoxConfirmOnReset.Size = new System.Drawing.Size(205, 17);
            this.checkBoxConfirmOnReset.TabIndex = 21;
            this.checkBoxConfirmOnReset.Text = "Ask for confirmation before deck reset";
            this.checkBoxConfirmOnReset.UseVisualStyleBackColor = true;
            this.checkBoxConfirmOnReset.CheckedChanged += new System.EventHandler(this.checkBoxConfirmOnReset_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Card language:";
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Items.AddRange(new object[] {
            "de",
            "en",
            "es",
            "fr",
            "pt",
            "ru"});
            this.comboBoxLanguage.Location = new System.Drawing.Point(170, 19);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(155, 21);
            this.comboBoxLanguage.TabIndex = 18;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguage_SelectedIndexChanged);
            // 
            // checkBoxOnTop
            // 
            this.checkBoxOnTop.AutoSize = true;
            this.checkBoxOnTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOnTop.Location = new System.Drawing.Point(9, 74);
            this.checkBoxOnTop.Name = "checkBoxOnTop";
            this.checkBoxOnTop.Size = new System.Drawing.Size(259, 17);
            this.checkBoxOnTop.TabIndex = 16;
            this.checkBoxOnTop.Text = "Deck counter windows should always stay on top";
            this.checkBoxOnTop.UseVisualStyleBackColor = true;
            this.checkBoxOnTop.CheckedChanged += new System.EventHandler(this.checkBoxOnTop_CheckedChanged);
            // 
            // checkBoxCardAutoSort
            // 
            this.checkBoxCardAutoSort.AutoSize = true;
            this.checkBoxCardAutoSort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCardAutoSort.Location = new System.Drawing.Point(6, 111);
            this.checkBoxCardAutoSort.Name = "checkBoxCardAutoSort";
            this.checkBoxCardAutoSort.Size = new System.Drawing.Size(207, 17);
            this.checkBoxCardAutoSort.TabIndex = 25;
            this.checkBoxCardAutoSort.Text = "Automatically sort unavail. cards to top";
            this.checkBoxCardAutoSort.UseVisualStyleBackColor = true;
            this.checkBoxCardAutoSort.CheckedChanged += new System.EventHandler(this.checkBoxSort_CheckedChanged);
            // 
            // checkBoxCardShowTD
            // 
            this.checkBoxCardShowTD.AutoSize = true;
            this.checkBoxCardShowTD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCardShowTD.Location = new System.Drawing.Point(6, 88);
            this.checkBoxCardShowTD.Name = "checkBoxCardShowTD";
            this.checkBoxCardShowTD.Size = new System.Drawing.Size(178, 17);
            this.checkBoxCardShowTD.TabIndex = 23;
            this.checkBoxCardShowTD.Text = "Show TD chance on every card";
            this.checkBoxCardShowTD.UseVisualStyleBackColor = true;
            this.checkBoxCardShowTD.CheckedChanged += new System.EventHandler(this.checkBoxTDColumn_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.numericUpDownCardDarken);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.checkBoxSwapMouse);
            this.groupBox5.Controls.Add(this.checkBoxCardColoredName);
            this.groupBox5.Controls.Add(this.checkBoxCardAutoSort);
            this.groupBox5.Controls.Add(this.checkBoxCardColorByType);
            this.groupBox5.Controls.Add(this.checkBoxCardShowImage);
            this.groupBox5.Controls.Add(this.checkBoxCardShowTD);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(353, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(278, 211);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Card display options";
            // 
            // numericUpDownCardDarken
            // 
            this.numericUpDownCardDarken.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownCardDarken.Location = new System.Drawing.Point(170, 157);
            this.numericUpDownCardDarken.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownCardDarken.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCardDarken.Name = "numericUpDownCardDarken";
            this.numericUpDownCardDarken.Size = new System.Drawing.Size(102, 20);
            this.numericUpDownCardDarken.TabIndex = 35;
            this.numericUpDownCardDarken.Value = new decimal(new int[] {
            224,
            0,
            0,
            0});
            this.numericUpDownCardDarken.ValueChanged += new System.EventHandler(this.numericUpDownCardDarken_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(6, 159);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(158, 13);
            this.label13.TabIndex = 34;
            this.label13.Text = "Unvailable card darken amount:";
            // 
            // checkBoxSwapMouse
            // 
            this.checkBoxSwapMouse.AutoSize = true;
            this.checkBoxSwapMouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSwapMouse.Location = new System.Drawing.Point(6, 134);
            this.checkBoxSwapMouse.Name = "checkBoxSwapMouse";
            this.checkBoxSwapMouse.Size = new System.Drawing.Size(183, 17);
            this.checkBoxSwapMouse.TabIndex = 33;
            this.checkBoxSwapMouse.Text = "Swap left and right click on cards";
            this.checkBoxSwapMouse.UseVisualStyleBackColor = true;
            this.checkBoxSwapMouse.CheckedChanged += new System.EventHandler(this.checkBoxSwapMouse_CheckedChanged);
            // 
            // checkBoxCardColoredName
            // 
            this.checkBoxCardColoredName.AutoSize = true;
            this.checkBoxCardColoredName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCardColoredName.Location = new System.Drawing.Point(6, 65);
            this.checkBoxCardColoredName.Name = "checkBoxCardColoredName";
            this.checkBoxCardColoredName.Size = new System.Drawing.Size(165, 17);
            this.checkBoxCardColoredName.TabIndex = 31;
            this.checkBoxCardColoredName.Text = "Colored card names (by rarity)";
            this.checkBoxCardColoredName.UseVisualStyleBackColor = true;
            this.checkBoxCardColoredName.CheckedChanged += new System.EventHandler(this.checkBoxCardColoredName_CheckedChanged);
            // 
            // checkBoxCardColorByType
            // 
            this.checkBoxCardColorByType.AutoSize = true;
            this.checkBoxCardColorByType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCardColorByType.Location = new System.Drawing.Point(6, 42);
            this.checkBoxCardColorByType.Name = "checkBoxCardColorByType";
            this.checkBoxCardColorByType.Size = new System.Drawing.Size(213, 17);
            this.checkBoxCardColorByType.TabIndex = 30;
            this.checkBoxCardColorByType.Text = "Colored card background (by card type)";
            this.checkBoxCardColorByType.UseVisualStyleBackColor = true;
            this.checkBoxCardColorByType.CheckedChanged += new System.EventHandler(this.checkBoxCardColorByType_CheckedChanged);
            // 
            // checkBoxCardShowImage
            // 
            this.checkBoxCardShowImage.AutoSize = true;
            this.checkBoxCardShowImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCardShowImage.Location = new System.Drawing.Point(6, 19);
            this.checkBoxCardShowImage.Name = "checkBoxCardShowImage";
            this.checkBoxCardShowImage.Size = new System.Drawing.Size(211, 17);
            this.checkBoxCardShowImage.TabIndex = 28;
            this.checkBoxCardShowImage.Text = "Show card images (instead of plain BG)";
            this.checkBoxCardShowImage.UseVisualStyleBackColor = true;
            this.checkBoxCardShowImage.CheckedChanged += new System.EventHandler(this.checkBoxCardShowImage_CheckedChanged);
            // 
            // buttonLoadFromUrl
            // 
            this.buttonLoadFromUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoadFromUrl.Location = new System.Drawing.Point(229, 19);
            this.buttonLoadFromUrl.Name = "buttonLoadFromUrl";
            this.buttonLoadFromUrl.Size = new System.Drawing.Size(100, 25);
            this.buttonLoadFromUrl.TabIndex = 32;
            this.buttonLoadFromUrl.Text = "Load from URL";
            this.buttonLoadFromUrl.UseVisualStyleBackColor = true;
            this.buttonLoadFromUrl.Click += new System.EventHandler(this.buttonLoadFromUrl_Click);
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUrl.Location = new System.Drawing.Point(6, 22);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(217, 20);
            this.textBoxUrl.TabIndex = 18;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxUrl);
            this.groupBox2.Controls.Add(this.buttonLoadFromUrl);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 177);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 58);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load a decklist from Hearthhead or HearthPwn";
            // 
            // comboBoxSortThird
            // 
            this.comboBoxSortThird.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSortThird.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSortThird.FormattingEnabled = true;
            this.comboBoxSortThird.Items.AddRange(new object[] {
            "mana [<]",
            "mana [>]",
            "name [<]",
            "name [>]",
            "type [<]",
            "type [>]",
            "count [<]",
            "count [>]"});
            this.comboBoxSortThird.Location = new System.Drawing.Point(73, 111);
            this.comboBoxSortThird.Name = "comboBoxSortThird";
            this.comboBoxSortThird.Size = new System.Drawing.Size(199, 21);
            this.comboBoxSortThird.TabIndex = 36;
            this.comboBoxSortThird.SelectedIndexChanged += new System.EventHandler(this.comboBoxSortThird_SelectedIndexChanged);
            // 
            // comboBoxSortSecond
            // 
            this.comboBoxSortSecond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSortSecond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSortSecond.FormattingEnabled = true;
            this.comboBoxSortSecond.Items.AddRange(new object[] {
            "mana [<]",
            "mana [>]",
            "name [<]",
            "name [>]",
            "type [<]",
            "type [>]",
            "count [<]",
            "count [>]"});
            this.comboBoxSortSecond.Location = new System.Drawing.Point(73, 84);
            this.comboBoxSortSecond.Name = "comboBoxSortSecond";
            this.comboBoxSortSecond.Size = new System.Drawing.Size(199, 21);
            this.comboBoxSortSecond.TabIndex = 35;
            this.comboBoxSortSecond.SelectedIndexChanged += new System.EventHandler(this.comboBoxSortSecond_SelectedIndexChanged);
            // 
            // comboBoxSortFirst
            // 
            this.comboBoxSortFirst.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSortFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSortFirst.FormattingEnabled = true;
            this.comboBoxSortFirst.Items.AddRange(new object[] {
            "mana [<]",
            "mana [>]",
            "name [<]",
            "name [>]",
            "type [<]",
            "type [>]",
            "count [<]",
            "count [>]"});
            this.comboBoxSortFirst.Location = new System.Drawing.Point(73, 57);
            this.comboBoxSortFirst.Name = "comboBoxSortFirst";
            this.comboBoxSortFirst.Size = new System.Drawing.Size(199, 21);
            this.comboBoxSortFirst.TabIndex = 34;
            this.comboBoxSortFirst.SelectedIndexChanged += new System.EventHandler(this.comboBoxSortFirst_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "Primary:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.comboBoxSortSecond);
            this.groupBox6.Controls.Add(this.comboBoxSortThird);
            this.groupBox6.Controls.Add(this.comboBoxSortFirst);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(353, 229);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(278, 139);
            this.groupBox6.TabIndex = 38;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Card sorting";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 39;
            this.label11.Text = "Tertiary:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Secondary:";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(266, 31);
            this.label10.TabIndex = 9;
            this.label10.Text = "With these three combo boxes, you can change the sorting order that the cards wil" +
    "l be put into.";
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 473);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMenu";
            this.Text = "Hearthstone Deck Counter - Menu";
            this.Load += new System.EventHandler(this.FormMenu_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCardDarken)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxRemoveCard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxAddCard;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxOnTop;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.CheckBox checkBoxConfirmOnReset;
        private System.Windows.Forms.CheckBox checkBoxCardShowTD;
        private System.Windows.Forms.CheckBox checkBoxCardAutoSort;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBoxCardColorByType;
        private System.Windows.Forms.CheckBox checkBoxCardShowImage;
        private System.Windows.Forms.CheckBox checkBoxCardColoredName;
        private System.Windows.Forms.Button buttonLoadFromUrl;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox checkBoxSwapMouse;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBoxSortThird;
        private System.Windows.Forms.ComboBox comboBoxSortSecond;
        private System.Windows.Forms.ComboBox comboBoxSortFirst;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownCardDarken;
        private System.Windows.Forms.Label label13;
    }
}