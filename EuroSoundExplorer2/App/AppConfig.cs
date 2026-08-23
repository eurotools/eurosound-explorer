using sb_explorer.Classes.PropertyGridHelpers;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using static sb_explorer.Enumerations;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class AppConfig
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        private Platform _PlatformSelected;
        private Title _TitleSelected;
        private string _SoundhFile, _ProjectFolder, _ProjectTitle;
        private uint _StreamsFrequency = 22050;
        private int _FileVersion;

        public event Action SoundhFileChanged;
        public event Action ProjectFolderChanged;

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Streams Freq.")]
        [Category("EngineX")]
        public uint StreamsFrequency
        {
            get { return _StreamsFrequency; }
            set { _StreamsFrequency = value; }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("File Version")]
        [Category("EngineX")]
        [ReadOnly(true)]
        public int FileVersion
        {
            get { return _FileVersion; }
            set { _FileVersion = value; }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Hash Table (Sound.h / AudioFileTable.h / SFX_Defines.h)")]
        [Category("Project")]
        [Editor(typeof(CustomFileBrowser), typeof(UITypeEditor))]
        public string SoundhFile
        {
            get { return _SoundhFile; }
            set
            {
                _SoundhFile = value;
                OnSoundhFileChanged();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public Platform PlatformSelected
        {
            get { return _PlatformSelected; }
            set
            {
                _PlatformSelected = value;

                //Check Exceptions
                StreamsFrequency = GetDefaultStreamsFrequency(value, TitleSelected);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Title")]
        [Category("Project")]
        public string ProjectTitle
        {
            get { return _ProjectTitle; }
            set { _ProjectTitle = value; }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public Title TitleSelected
        {
            get { return _TitleSelected; }
            set
            {
                _TitleSelected = value;
                //Check Exceptions
                StreamsFrequency = GetDefaultStreamsFrequency(PlatformSelected, value);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Files Folder")]
        [Category("Project")]
        [Editor(typeof(CustomFolderBrowser), typeof(UITypeEditor))]
        public string ProjectFolder
        {
            get { return _ProjectFolder; }
            set
            {
                _ProjectFolder = value;
                OnProjectFolderChanged();
            }
        }

        private void OnSoundhFileChanged()
        {
            if (SoundhFileChanged != null)
            {
                SoundhFileChanged();
            }
        }

        private void OnProjectFolderChanged()
        {
            if (ProjectFolderChanged != null)
            {
                ProjectFolderChanged();
            }
        }

        private static uint GetDefaultStreamsFrequency(Platform platform, Title title)
        {
            if (platform == Platform.GameCube && title == Title.BatmanBegins)
            {
                return 16000;
            }

            return 22050;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
