using sb_explorer.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace sb_explorer
{
    public partial class FrmEuroSoundSfxExportSection : Form
    {
        public List<EuroSoundSfxTextSection> SelectedSections { get; private set; }

        public FrmEuroSoundSfxExportSection()
        {
            InitializeComponent();
            SelectedSections = new List<EuroSoundSfxTextSection>();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            SelectedSections.Clear();
            foreach (int index in checkedListBoxSections.CheckedIndices)
            {
                switch (index)
                {
                    case 0:
                        SelectedSections.Add(EuroSoundSfxTextSection.Parameters);
                        break;
                    case 1:
                        SelectedSections.Add(EuroSoundSfxTextSection.SamplePoolModes);
                        break;
                    case 2:
                        SelectedSections.Add(EuroSoundSfxTextSection.SamplePoolControl);
                        break;
                }
            }

            if (SelectedSections.Count == 0)
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Select at least one section.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
