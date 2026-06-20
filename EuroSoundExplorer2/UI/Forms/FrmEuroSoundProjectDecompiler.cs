using sb_explorer.Services;
using System;
using System.Text;
using System.Windows.Forms;

namespace sb_explorer
{
    public partial class FrmEuroSoundProjectDecompiler : Form
    {
        public FrmEuroSoundProjectDecompiler()
        {
            InitializeComponent();
            LoadSettings();
            FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            if (parentForm != null && string.IsNullOrWhiteSpace(txtProjectFolder.Text))
            {
                txtProjectFolder.Text = parentForm.Configuration.ProjectFolder;
            }
            if (cbxMode.SelectedIndex < 0)
            {
                cbxMode.SelectedIndex = 0;
            }
            UpdateModeState();
        }

        private void BtnBrowseCompiledFolder_Click(object sender, EventArgs e)
        {
            txtCompiledFolder.Text = BrowseFolder(txtCompiledFolder.Text);
        }

        private void BtnBrowseProjectFolder_Click(object sender, EventArgs e)
        {
            txtProjectFolder.Text = BrowseFolder(txtProjectFolder.Text);
        }

        private void BtnBrowseOutputFolder_Click(object sender, EventArgs e)
        {
            txtOutputFolder.Text = BrowseFolder(txtOutputFolder.Text);
        }

        private void CbxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateModeState();
        }

        private void BtnDecompile_Click(object sender, EventArgs e)
        {
            try
            {
                EuroSoundProjectDecompilerOptions options = BuildOptions();
                SaveSettings();
                using (FrmProgress progressForm = new FrmProgress(
                    "Decompile EuroSound Project",
                    "Preparing project decompile...",
                    worker => EuroSoundProjectDecompiler.Decompile(options, worker)))
                {
                    progressForm.ShowDialog(this);

                    if (progressForm.Error != null)
                    {
                        MessageBox.Show(progressForm.Error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    EuroSoundProjectDecompilerResult result = (EuroSoundProjectDecompilerResult)progressForm.Result;
                    if (result == null)
                    {
                        return;
                    }

                    ShowResult(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private EuroSoundProjectDecompilerOptions BuildOptions()
        {
            FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            EuroSoundProjectDecompilerMode mode = cbxMode.SelectedIndex == 0
                ? EuroSoundProjectDecompilerMode.Create
                : EuroSoundProjectDecompilerMode.ReplaceSections;

            return new EuroSoundProjectDecompilerOptions
            {
                CompiledFolder = txtCompiledFolder.Text,
                EuroSoundProjectFolder = txtProjectFolder.Text,
                OutputFolder = txtOutputFolder.Text,
                Mode = mode,
                SelectedTitle = parentForm.Configuration.TitleSelected,
                Hashcodes = parentForm.HashTable,
                ExportSoundBanks = chkExportSoundBanks.Checked,
                ExportGroups = chkExportGroups.Checked,
                ExportDuckerGroups = chkExportDuckerGroups.Checked,
                ExportSfx = chkExportSfx.Checked,
                ExportMemoryMaps = chkExportMemoryMaps.Checked,
                RewriteSamplesOnly = chkRewriteSamplesOnly.Checked,
                ExportPlatformSpecificSamplePoolModes = chkExportPlatformSamplePools.Checked,
                ReplaceSfxParameters = chkSfxParameters.Checked,
                ReplaceSfxSamplePoolFiles = chkSfxSamplePoolFiles.Checked,
                ReplaceSfxSamplePoolModes = chkSfxSamplePoolModes.Checked,
                ReplaceSfxSamplePoolControl = chkSfxSamplePoolControl.Checked,
                ReplaceGroupDependencies = chkGroupDependencies.Checked,
                ReplaceGroupParameters = chkGroupParameters.Checked,
                ReplaceDuckerDependencies = chkDuckerDependencies.Checked,
                ReplaceDuckerParameters = chkDuckerParameters.Checked,
                ReplaceSoundBankDependencies = chkSoundBankDependencies.Checked
            };
        }

        private string BrowseFolder(string selectedPath)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(selectedPath))
                {
                    folderBrowserDialog.SelectedPath = selectedPath;
                }

                return folderBrowserDialog.ShowDialog(this) == DialogResult.OK ? folderBrowserDialog.SelectedPath : selectedPath;
            }
        }

        private void UpdateModeState()
        {
            bool replaceMode = cbxMode.SelectedIndex == 1;

            txtProjectFolder.Enabled = replaceMode;
            btnBrowseProjectFolder.Enabled = replaceMode;
            txtOutputFolder.Enabled = !replaceMode;
            btnBrowseOutputFolder.Enabled = !replaceMode;
            grpReplaceSections.Enabled = replaceMode;
        }

        private void LoadSettings()
        {
            Properties.Settings settings = Properties.Settings.Default;
            txtCompiledFolder.Text = settings.DecompilerCompiledFolder;
            txtProjectFolder.Text = settings.DecompilerProjectFolder;
            txtOutputFolder.Text = settings.DecompilerOutputFolder;
            cbxMode.SelectedIndex = Math.Max(0, Math.Min(cbxMode.Items.Count - 1, settings.DecompilerMode));
            chkExportSoundBanks.Checked = settings.DecompilerExportSoundBanks;
            chkExportGroups.Checked = settings.DecompilerExportGroups;
            chkExportDuckerGroups.Checked = settings.DecompilerExportDuckerGroups;
            chkExportSfx.Checked = settings.DecompilerExportSfx;
            chkExportPlatformSamplePools.Checked = settings.DecompilerExportPlatformSamplePools;
            chkSfxParameters.Checked = settings.DecompilerReplaceSfxParameters;
            chkSfxSamplePoolFiles.Checked = settings.DecompilerReplaceSfxSamplePoolFiles;
            chkSfxSamplePoolModes.Checked = settings.DecompilerReplaceSfxSamplePoolModes;
            chkSfxSamplePoolControl.Checked = settings.DecompilerReplaceSfxSamplePoolControl;
            chkGroupDependencies.Checked = settings.DecompilerReplaceGroupDependencies;
            chkGroupParameters.Checked = settings.DecompilerReplaceGroupParameters;
            chkDuckerDependencies.Checked = settings.DecompilerReplaceDuckerDependencies;
            chkDuckerParameters.Checked = settings.DecompilerReplaceDuckerParameters;
            chkSoundBankDependencies.Checked = settings.DecompilerReplaceSoundBankDependencies;
        }

        private void SaveSettings()
        {
            Properties.Settings settings = Properties.Settings.Default;
            settings.DecompilerCompiledFolder = txtCompiledFolder.Text;
            settings.DecompilerProjectFolder = txtProjectFolder.Text;
            settings.DecompilerOutputFolder = txtOutputFolder.Text;
            settings.DecompilerMode = cbxMode.SelectedIndex;
            settings.DecompilerExportSoundBanks = chkExportSoundBanks.Checked;
            settings.DecompilerExportGroups = chkExportGroups.Checked;
            settings.DecompilerExportDuckerGroups = chkExportDuckerGroups.Checked;
            settings.DecompilerExportSfx = chkExportSfx.Checked;
            settings.DecompilerExportPlatformSamplePools = chkExportPlatformSamplePools.Checked;
            settings.DecompilerReplaceSfxParameters = chkSfxParameters.Checked;
            settings.DecompilerReplaceSfxSamplePoolFiles = chkSfxSamplePoolFiles.Checked;
            settings.DecompilerReplaceSfxSamplePoolModes = chkSfxSamplePoolModes.Checked;
            settings.DecompilerReplaceSfxSamplePoolControl = chkSfxSamplePoolControl.Checked;
            settings.DecompilerReplaceGroupDependencies = chkGroupDependencies.Checked;
            settings.DecompilerReplaceGroupParameters = chkGroupParameters.Checked;
            settings.DecompilerReplaceDuckerDependencies = chkDuckerDependencies.Checked;
            settings.DecompilerReplaceDuckerParameters = chkDuckerParameters.Checked;
            settings.DecompilerReplaceSoundBankDependencies = chkSoundBankDependencies.Checked;
            settings.Save();
        }

        private void ShowResult(EuroSoundProjectDecompilerResult result)
        {
            if (result.Cancelled)
            {
                MessageBox.Show("Project decompile was cancelled.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            StringBuilder message = new StringBuilder();
            message.AppendLine("EuroSound project decompile finished.");
            message.AppendLine("SoundBanks read: " + result.SoundbankFilesRead);
            message.AppendLine("StreamBanks read: " + result.StreambanksRead);
            message.AppendLine("MusicBanks read: " + result.MusicbanksRead);
            message.AppendLine("SoundBanks written: " + result.SoundbanksWritten);
            message.AppendLine("Groups written: " + result.GroupsWritten);
            message.AppendLine("Ducker Groups written: " + result.DuckerGroupsWritten);
            message.AppendLine("SFX written: " + result.SfxWritten);
            message.AppendLine("Samples.txt entries: " + result.SamplesWritten);
            message.AppendLine("Music WAVs written: " + result.MusicsWritten);
            message.AppendLine("Memory Maps written: " + result.MemoryMapsWritten);
            if (!string.IsNullOrEmpty(result.ReportPath))
            {
                message.AppendLine("Report: " + result.ReportPath);
            }

            if (result.FailedFiles > 0)
            {
                message.AppendLine("Failed files: " + result.FailedFiles);
                AppendLines(message, result.FailedFileMessages, 5);
            }

            if (result.Warnings.Count > 0)
            {
                message.AppendLine("Warnings: " + result.Warnings.Count);
                AppendLines(message, result.Warnings, 5);
            }

            bool hasWarnings = result.FailedFiles > 0 || result.Warnings.Count > 0;
            MessageBox.Show(message.ToString(), Application.ProductName, MessageBoxButtons.OK, hasWarnings ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private void AppendLines(StringBuilder message, System.Collections.Generic.List<string> values, int maxLines)
        {
            int count = Math.Min(values.Count, maxLines);
            for (int i = 0; i < count; i++)
            {
                message.AppendLine(values[i]);
            }
        }
    }
}
