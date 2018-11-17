using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HSDC
{
    public partial class FormMenu : Form
    {
        private DeckCounter dc;
        public FormMenu(DeckCounter dc)
        {
            InitializeComponent();
            this.dc = dc;
            
            // Load option values
            checkBoxCardShowImage.Checked = dc.Options.GetAsBool("CardShowImage");
            checkBoxCardColorByType.Checked = dc.Options.GetAsBool("CardColorByType");
            checkBoxCardColoredName.Checked = dc.Options.GetAsBool("CardColoredName");
            checkBoxCardShowTD.Checked = dc.Options.GetAsBool("CardShowTD");
            numericUpDown1.Value = dc.Options.GetAsInteger("CardHeight");
            checkBoxCardAutoSort.Checked = dc.Options.GetAsBool("CardAutoSort");
            checkBoxSwapMouse.Checked = dc.Options.GetAsBool("SwapMouse");

            checkBoxOnTop.Checked = dc.Options.GetAsBool("StayOnTop");
            comboBoxLanguage.Text = dc.Options.GetAsString("Language");
            checkBoxConfirmOnReset.Checked = dc.Options.GetAsBool("ConfirmOnDeckReset");

            comboBoxSortFirst.Text = dc.Options.GetAsString("SortFirst");
            comboBoxSortSecond.Text = dc.Options.GetAsString("SortSecond");
            comboBoxSortThird.Text = dc.Options.GetAsString("SortThird");
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            textBoxAddCard.AutoCompleteCustomSource = dc.ListAvailableCards();
            dc.ComboBoxRemoveCard = comboBoxRemoveCard;
            comboBoxRemoveCard.Items.AddRange(dc.ListCardsInDeck());
        }

        private void textBoxAddCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dc.AddCard(textBoxAddCard.Text);
                textBoxAddCard.Text = "";
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            dc.SaveDeck();
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            dc.LoadDeck();
        }

        private void comboBoxRemoveCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            dc.RemoveCard(comboBoxRemoveCard.SelectedItem.ToString());
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dc.EmptyDeck();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            dc.SetCardHeight(Convert.ToInt32(numericUpDown1.Value));
        }

        private void checkBoxOnTop_CheckedChanged(object sender, EventArgs e)
        {
            dc.SetStayOnTop(checkBoxOnTop.Checked);
            this.TopMost = checkBoxOnTop.Checked;
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            dc.SetLanguage(comboBoxLanguage.Text);
            textBoxAddCard.AutoCompleteCustomSource = dc.ListAvailableCards();
        }

        private void checkBoxConfirmOnReset_CheckedChanged(object sender, EventArgs e)
        {
            dc.Options.Set("ConfirmOnDeckReset", checkBoxConfirmOnReset.Checked.ToString());
        }

        private void checkBoxTDColumn_CheckedChanged(object sender, EventArgs e)
        {
            dc.Options.Set("CardShowTD", checkBoxCardShowTD.Checked.ToString());
            dc.RefreshAllCards();
        }

        private void checkBoxSort_CheckedChanged(object sender, EventArgs e)
        {
            dc.Options.Set("CardAutoSort", checkBoxCardAutoSort.Checked.ToString());
            dc.Sort();
        }

        private void checkBoxCardColorByType_CheckedChanged(object sender, EventArgs e)
        {
            dc.Options.Set("CardColorByType", checkBoxCardColorByType.Checked.ToString());
            dc.RefreshAllCards();
        }

        private void checkBoxCardShowImage_CheckedChanged(object sender, EventArgs e)
        {
            dc.Options.Set("CardShowImage", checkBoxCardShowImage.Checked.ToString());
            dc.RefreshAllCards();
        }

        private void checkBoxCardColoredName_CheckedChanged(object sender, EventArgs e)
        {
            dc.Options.Set("CardColoredName", checkBoxCardColoredName.Checked.ToString());
            dc.RefreshAllCards();
        }

        private void buttonLoadFromUrl_Click(object sender, EventArgs e)
        {
            if (textBoxUrl.Text == "")
            {
                MessageBox.Show("Please input a website URL!");
            }
            else
            {
                dc.LoadDeckFromWebsite(textBoxUrl.Text);
            }
        }

        private void checkBoxSwapMouse_CheckedChanged(object sender, EventArgs e)
        {
            dc.Options.Set("SwapMouse", checkBoxSwapMouse.Checked.ToString());
        }

        private void comboBoxSortFirst_SelectedIndexChanged(object sender, EventArgs e)
        {
            dc.Options.Set("SortFirst", comboBoxSortFirst.Text);
            dc.Sort();
        }

        private void comboBoxSortSecond_SelectedIndexChanged(object sender, EventArgs e)
        {
            dc.Options.Set("SortSecond", comboBoxSortSecond.Text);
            dc.Sort();
        }

        private void comboBoxSortThird_SelectedIndexChanged(object sender, EventArgs e)
        {
            dc.Options.Set("SortThird", comboBoxSortThird.Text);
            dc.Sort();
        }

        private void numericUpDownCardDarken_ValueChanged(object sender, EventArgs e)
        {
            dc.Options.Set("CardDarkenValue", numericUpDownCardDarken.Value.ToString());
            dc.RefreshAllCards();
        }

       
    }
}
