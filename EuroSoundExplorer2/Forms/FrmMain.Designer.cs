
namespace sb_explorer
{
    partial class FrmMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.mainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.MainMenu = new System.Windows.Forms.ToolStrip();
            this.MenuItem_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.MenuItem_File_Settings = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_File_DecodeAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_File_DataViewer = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_File_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_File_ResetSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_File_Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_File_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Menu1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.MenuItem_SfxFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_MediaPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SB = new System.Windows.Forms.ToolStripDropDownButton();
            this.MenuItem_SB_HashCodes = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SB_SamplePool = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SB_SampleProps = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SB_WavHeaderData = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Stream = new System.Windows.Forms.ToolStripDropDownButton();
            this.MenuItem_Streams_StreamData = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Streams_StartMarkers = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Streams_Markers = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Music = new System.Windows.Forms.ToolStripDropDownButton();
            this.MenuItem_Music_MusicData = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Music_StartMarkers = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Music_Markers = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Project = new System.Windows.Forms.ToolStripDropDownButton();
            this.MenuItem_ProjectDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Project_ProjectData = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Project_MemorySlots = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Project_Soundbank = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SoundDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_MusicDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SBI = new System.Windows.Forms.ToolStripDropDownButton();
            this.MenuItem_SBI_StoredSoundBanks = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SBI_StoredMusicBanks = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainDockPanel
            // 
            this.mainDockPanel.ActiveAutoHideContent = null;
            this.mainDockPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.mainDockPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.mainDockPanel.Location = new System.Drawing.Point(0, 25);
            this.mainDockPanel.Name = "mainDockPanel";
            this.mainDockPanel.Size = new System.Drawing.Size(1026, 702);
            this.mainDockPanel.TabIndex = 0;
            // 
            // MainMenu
            // 
            this.MainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_File,
            this.MenuItem_Menu1,
            this.MenuItem_SB,
            this.MenuItem_Stream,
            this.MenuItem_Music,
            this.MenuItem_Project,
            this.MenuItem_SBI});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainMenu.ShowItemToolTips = false;
            this.MainMenu.Size = new System.Drawing.Size(1026, 25);
            this.MainMenu.Stretch = true;
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "toolStrip1";
            // 
            // MenuItem_File
            // 
            this.MenuItem_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_File_Settings,
            this.MenuItem_File_DecodeAudio,
            this.MenuItem_File_DataViewer,
            this.MenuItem_File_Separator1,
            this.MenuItem_File_ResetSettings,
            this.MenuItem_File_Separator2,
            this.MenuItem_File_Exit});
            this.MenuItem_File.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_File.Image")));
            this.MenuItem_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_File.Name = "MenuItem_File";
            this.MenuItem_File.Size = new System.Drawing.Size(38, 22);
            this.MenuItem_File.Text = "File";
            this.MenuItem_File.DropDownOpening += new System.EventHandler(this.MenuItem_File_DropDownOpening);
            // 
            // MenuItem_File_Settings
            // 
            this.MenuItem_File_Settings.Name = "MenuItem_File_Settings";
            this.MenuItem_File_Settings.Size = new System.Drawing.Size(170, 22);
            this.MenuItem_File_Settings.Text = "Settings";
            this.MenuItem_File_Settings.Click += new System.EventHandler(this.MenuItem_File_Settings_Click);
            // 
            // MenuItem_File_DecodeAudio
            // 
            this.MenuItem_File_DecodeAudio.Name = "MenuItem_File_DecodeAudio";
            this.MenuItem_File_DecodeAudio.Size = new System.Drawing.Size(170, 22);
            this.MenuItem_File_DecodeAudio.Text = "Decode Audio File";
            this.MenuItem_File_DecodeAudio.Click += new System.EventHandler(this.MenuItem_File_DecodeAudio_Click);
            // 
            // MenuItem_File_DataViewer
            // 
            this.MenuItem_File_DataViewer.Name = "MenuItem_File_DataViewer";
            this.MenuItem_File_DataViewer.Size = new System.Drawing.Size(170, 22);
            this.MenuItem_File_DataViewer.Text = "Data Viewer";
            this.MenuItem_File_DataViewer.Click += new System.EventHandler(this.MenuItem_File_DataViewer_Click);
            // 
            // MenuItem_File_Separator1
            // 
            this.MenuItem_File_Separator1.Name = "MenuItem_File_Separator1";
            this.MenuItem_File_Separator1.Size = new System.Drawing.Size(167, 6);
            // 
            // MenuItem_File_ResetSettings
            // 
            this.MenuItem_File_ResetSettings.Name = "MenuItem_File_ResetSettings";
            this.MenuItem_File_ResetSettings.Size = new System.Drawing.Size(170, 22);
            this.MenuItem_File_ResetSettings.Text = "Reset Settings";
            // 
            // MenuItem_File_Separator2
            // 
            this.MenuItem_File_Separator2.Name = "MenuItem_File_Separator2";
            this.MenuItem_File_Separator2.Size = new System.Drawing.Size(167, 6);
            // 
            // MenuItem_File_Exit
            // 
            this.MenuItem_File_Exit.Name = "MenuItem_File_Exit";
            this.MenuItem_File_Exit.Size = new System.Drawing.Size(170, 22);
            this.MenuItem_File_Exit.Text = "Exit";
            this.MenuItem_File_Exit.Click += new System.EventHandler(this.MenuItem_File_Exit_Click);
            // 
            // MenuItem_Menu1
            // 
            this.MenuItem_Menu1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItem_Menu1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_SfxFiles,
            this.MenuItem_MediaPlayer});
            this.MenuItem_Menu1.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Menu1.Image")));
            this.MenuItem_Menu1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Menu1.Name = "MenuItem_Menu1";
            this.MenuItem_Menu1.Size = new System.Drawing.Size(60, 22);
            this.MenuItem_Menu1.Text = "Menu 1";
            this.MenuItem_Menu1.DropDownOpening += new System.EventHandler(this.MenuItem_Menu1_DropDownOpening);
            // 
            // MenuItem_SfxFiles
            // 
            this.MenuItem_SfxFiles.Name = "MenuItem_SfxFiles";
            this.MenuItem_SfxFiles.Size = new System.Drawing.Size(142, 22);
            this.MenuItem_SfxFiles.Text = "Files";
            this.MenuItem_SfxFiles.Click += new System.EventHandler(this.MenuItem_Files_Click);
            // 
            // MenuItem_MediaPlayer
            // 
            this.MenuItem_MediaPlayer.Name = "MenuItem_MediaPlayer";
            this.MenuItem_MediaPlayer.Size = new System.Drawing.Size(142, 22);
            this.MenuItem_MediaPlayer.Text = "Media Player";
            this.MenuItem_MediaPlayer.Click += new System.EventHandler(this.MenuItem_MediaPlayer_Click);
            // 
            // MenuItem_SB
            // 
            this.MenuItem_SB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItem_SB.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_SB_HashCodes,
            this.MenuItem_SB_SamplePool,
            this.MenuItem_SB_SampleProps,
            this.MenuItem_SB_WavHeaderData});
            this.MenuItem_SB.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_SB.Image")));
            this.MenuItem_SB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_SB.Name = "MenuItem_SB";
            this.MenuItem_SB.Size = new System.Drawing.Size(85, 22);
            this.MenuItem_SB.Text = "SoundBanks";
            this.MenuItem_SB.DropDownOpening += new System.EventHandler(this.MenuItem_SB_DropDownOpening);
            // 
            // MenuItem_SB_HashCodes
            // 
            this.MenuItem_SB_HashCodes.Name = "MenuItem_SB_HashCodes";
            this.MenuItem_SB_HashCodes.Size = new System.Drawing.Size(169, 22);
            this.MenuItem_SB_HashCodes.Text = "HashCodes";
            this.MenuItem_SB_HashCodes.Click += new System.EventHandler(this.MenuItem_SB_HashCodes_Click);
            // 
            // MenuItem_SB_SamplePool
            // 
            this.MenuItem_SB_SamplePool.Name = "MenuItem_SB_SamplePool";
            this.MenuItem_SB_SamplePool.Size = new System.Drawing.Size(169, 22);
            this.MenuItem_SB_SamplePool.Text = "Sample Pool";
            this.MenuItem_SB_SamplePool.Click += new System.EventHandler(this.MenuItem_SB_SamplePool_Click);
            // 
            // MenuItem_SB_SampleProps
            // 
            this.MenuItem_SB_SampleProps.Name = "MenuItem_SB_SampleProps";
            this.MenuItem_SB_SampleProps.Size = new System.Drawing.Size(169, 22);
            this.MenuItem_SB_SampleProps.Text = "Sample Properties";
            this.MenuItem_SB_SampleProps.Click += new System.EventHandler(this.MenuItem_SB_SampleProps_Click);
            // 
            // MenuItem_SB_WavHeaderData
            // 
            this.MenuItem_SB_WavHeaderData.Name = "MenuItem_SB_WavHeaderData";
            this.MenuItem_SB_WavHeaderData.Size = new System.Drawing.Size(169, 22);
            this.MenuItem_SB_WavHeaderData.Text = "Wav Header Data";
            this.MenuItem_SB_WavHeaderData.Click += new System.EventHandler(this.MenuItem_SB_WavHeaderData_Click);
            // 
            // MenuItem_Stream
            // 
            this.MenuItem_Stream.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItem_Stream.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Streams_StreamData,
            this.MenuItem_Streams_StartMarkers,
            this.MenuItem_Streams_Markers});
            this.MenuItem_Stream.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Stream.Image")));
            this.MenuItem_Stream.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Stream.Name = "MenuItem_Stream";
            this.MenuItem_Stream.Size = new System.Drawing.Size(62, 22);
            this.MenuItem_Stream.Text = "Streams";
            this.MenuItem_Stream.DropDownOpening += new System.EventHandler(this.MenuItem_Stream_DropDownOpening);
            // 
            // MenuItem_Streams_StreamData
            // 
            this.MenuItem_Streams_StreamData.Name = "MenuItem_Streams_StreamData";
            this.MenuItem_Streams_StreamData.Size = new System.Drawing.Size(143, 22);
            this.MenuItem_Streams_StreamData.Text = "Stream Data";
            this.MenuItem_Streams_StreamData.Click += new System.EventHandler(this.MenuItem_Streams_StreamData_Click);
            // 
            // MenuItem_Streams_StartMarkers
            // 
            this.MenuItem_Streams_StartMarkers.Name = "MenuItem_Streams_StartMarkers";
            this.MenuItem_Streams_StartMarkers.Size = new System.Drawing.Size(143, 22);
            this.MenuItem_Streams_StartMarkers.Text = "Start Markers";
            this.MenuItem_Streams_StartMarkers.Click += new System.EventHandler(this.MenuItem_Streams_StartMarkers_Click);
            // 
            // MenuItem_Streams_Markers
            // 
            this.MenuItem_Streams_Markers.Name = "MenuItem_Streams_Markers";
            this.MenuItem_Streams_Markers.Size = new System.Drawing.Size(143, 22);
            this.MenuItem_Streams_Markers.Text = "Markers";
            this.MenuItem_Streams_Markers.Click += new System.EventHandler(this.MenuItem_Streams_Markers_Click);
            // 
            // MenuItem_Music
            // 
            this.MenuItem_Music.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItem_Music.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Music_MusicData,
            this.MenuItem_Music_StartMarkers,
            this.MenuItem_Music_Markers});
            this.MenuItem_Music.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Music.Image")));
            this.MenuItem_Music.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Music.Name = "MenuItem_Music";
            this.MenuItem_Music.Size = new System.Drawing.Size(52, 22);
            this.MenuItem_Music.Text = "Music";
            this.MenuItem_Music.DropDownOpening += new System.EventHandler(this.MenuItem_Music_DropDownOpening);
            // 
            // MenuItem_Music_MusicData
            // 
            this.MenuItem_Music_MusicData.Name = "MenuItem_Music_MusicData";
            this.MenuItem_Music_MusicData.Size = new System.Drawing.Size(180, 22);
            this.MenuItem_Music_MusicData.Text = "Music Data";
            this.MenuItem_Music_MusicData.Click += new System.EventHandler(this.MenuItem_Music_MusicData_Click);
            // 
            // MenuItem_Music_StartMarkers
            // 
            this.MenuItem_Music_StartMarkers.Name = "MenuItem_Music_StartMarkers";
            this.MenuItem_Music_StartMarkers.Size = new System.Drawing.Size(180, 22);
            this.MenuItem_Music_StartMarkers.Text = "Start Markers";
            this.MenuItem_Music_StartMarkers.Click += new System.EventHandler(this.MenuItem_Music_StartMarkers_Click);
            // 
            // MenuItem_Music_Markers
            // 
            this.MenuItem_Music_Markers.Name = "MenuItem_Music_Markers";
            this.MenuItem_Music_Markers.Size = new System.Drawing.Size(180, 22);
            this.MenuItem_Music_Markers.Text = "Markers";
            this.MenuItem_Music_Markers.Click += new System.EventHandler(this.MenuItem_Music_Markers_Click);
            // 
            // MenuItem_Project
            // 
            this.MenuItem_Project.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItem_Project.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_ProjectDetails,
            this.MenuItem_SoundDetails,
            this.MenuItem_MusicDetails});
            this.MenuItem_Project.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Project.Image")));
            this.MenuItem_Project.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Project.Name = "MenuItem_Project";
            this.MenuItem_Project.Size = new System.Drawing.Size(76, 22);
            this.MenuItem_Project.Text = "File Details";
            this.MenuItem_Project.DropDownOpening += new System.EventHandler(this.MenuItem_Project_DropDownOpening);
            // 
            // MenuItem_ProjectDetails
            // 
            this.MenuItem_ProjectDetails.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Project_ProjectData,
            this.MenuItem_Project_MemorySlots,
            this.MenuItem_Project_Soundbank});
            this.MenuItem_ProjectDetails.Name = "MenuItem_ProjectDetails";
            this.MenuItem_ProjectDetails.Size = new System.Drawing.Size(180, 22);
            this.MenuItem_ProjectDetails.Text = "Project Details";
            // 
            // MenuItem_Project_ProjectData
            // 
            this.MenuItem_Project_ProjectData.Name = "MenuItem_Project_ProjectData";
            this.MenuItem_Project_ProjectData.Size = new System.Drawing.Size(147, 22);
            this.MenuItem_Project_ProjectData.Text = "Project Data";
            this.MenuItem_Project_ProjectData.Click += new System.EventHandler(this.MenuItem_Project_ProjectData_Click);
            // 
            // MenuItem_Project_MemorySlots
            // 
            this.MenuItem_Project_MemorySlots.Name = "MenuItem_Project_MemorySlots";
            this.MenuItem_Project_MemorySlots.Size = new System.Drawing.Size(147, 22);
            this.MenuItem_Project_MemorySlots.Text = "Memory Slots";
            this.MenuItem_Project_MemorySlots.Click += new System.EventHandler(this.MenuItem_Project_MemorySlots_Click);
            // 
            // MenuItem_Project_Soundbank
            // 
            this.MenuItem_Project_Soundbank.Name = "MenuItem_Project_Soundbank";
            this.MenuItem_Project_Soundbank.Size = new System.Drawing.Size(147, 22);
            this.MenuItem_Project_Soundbank.Text = "SoundBank";
            this.MenuItem_Project_Soundbank.Click += new System.EventHandler(this.MenuItem_Project_Soundbank_Click);
            // 
            // MenuItem_SoundDetails
            // 
            this.MenuItem_SoundDetails.Name = "MenuItem_SoundDetails";
            this.MenuItem_SoundDetails.Size = new System.Drawing.Size(180, 22);
            this.MenuItem_SoundDetails.Text = "Sound Details";
            this.MenuItem_SoundDetails.Click += new System.EventHandler(this.MenuItem_SoundDetails_Click);
            // 
            // MenuItem_MusicDetails
            // 
            this.MenuItem_MusicDetails.Name = "MenuItem_MusicDetails";
            this.MenuItem_MusicDetails.Size = new System.Drawing.Size(180, 22);
            this.MenuItem_MusicDetails.Text = "Music Details";
            this.MenuItem_MusicDetails.Click += new System.EventHandler(this.MenuItem_MusicDetails_Click);
            // 
            // MenuItem_SBI
            // 
            this.MenuItem_SBI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItem_SBI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_SBI_StoredSoundBanks,
            this.MenuItem_SBI_StoredMusicBanks});
            this.MenuItem_SBI.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_SBI.Image")));
            this.MenuItem_SBI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_SBI.Name = "MenuItem_SBI";
            this.MenuItem_SBI.Size = new System.Drawing.Size(36, 22);
            this.MenuItem_SBI.Text = "SBI";
            this.MenuItem_SBI.DropDownOpening += new System.EventHandler(this.MenuItem_SBI_DropDownOpening);
            // 
            // MenuItem_SBI_StoredSoundBanks
            // 
            this.MenuItem_SBI_StoredSoundBanks.Name = "MenuItem_SBI_StoredSoundBanks";
            this.MenuItem_SBI_StoredSoundBanks.Size = new System.Drawing.Size(176, 22);
            this.MenuItem_SBI_StoredSoundBanks.Text = "Stored Soundbanks";
            this.MenuItem_SBI_StoredSoundBanks.Click += new System.EventHandler(this.MenuItem_SBI_StoredSoundBanks_Click);
            // 
            // MenuItem_SBI_StoredMusicBanks
            // 
            this.MenuItem_SBI_StoredMusicBanks.Name = "MenuItem_SBI_StoredMusicBanks";
            this.MenuItem_SBI_StoredMusicBanks.Size = new System.Drawing.Size(176, 22);
            this.MenuItem_SBI_StoredMusicBanks.Text = "Stored Musicbanks";
            this.MenuItem_SBI_StoredMusicBanks.Click += new System.EventHandler(this.MenuItem_SBI_StoredMusicBanks_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 727);
            this.Controls.Add(this.mainDockPanel);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "EuroSound Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel mainDockPanel;
        private System.Windows.Forms.ToolStrip MainMenu;
        private System.Windows.Forms.ToolStripDropDownButton MenuItem_File;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_File_Exit;
        private System.Windows.Forms.ToolStripDropDownButton MenuItem_SB;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SB_HashCodes;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SB_SamplePool;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SB_SampleProps;
        private System.Windows.Forms.ToolStripDropDownButton MenuItem_Stream;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Streams_StreamData;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Streams_StartMarkers;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Streams_Markers;
        private System.Windows.Forms.ToolStripDropDownButton MenuItem_Music;
        private System.Windows.Forms.ToolStripDropDownButton MenuItem_Project;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Music_MusicData;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Music_StartMarkers;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Music_Markers;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_File_Settings;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_File_DecodeAudio;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_File_DataViewer;
        private System.Windows.Forms.ToolStripSeparator MenuItem_File_Separator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_File_ResetSettings;
        private System.Windows.Forms.ToolStripSeparator MenuItem_File_Separator2;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SB_WavHeaderData;
        private System.Windows.Forms.ToolStripDropDownButton MenuItem_Menu1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SfxFiles;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_MediaPlayer;
        private System.Windows.Forms.ToolStripDropDownButton MenuItem_SBI;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SBI_StoredSoundBanks;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SBI_StoredMusicBanks;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_ProjectDetails;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Project_ProjectData;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Project_MemorySlots;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Project_Soundbank;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SoundDetails;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_MusicDetails;
    }
}

