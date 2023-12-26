using sb_explorer.Classes.PropertyGridHelpers;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
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
        private string _SoundhFile, _ProjectFolder;
        private uint _StreamsFrequency = 22050;
        private int _FileVersion = 201;

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
        [TypeConverter(typeof(FileVersions))]
        public int FileVersion
        {
            get { return _FileVersion; }
            set { _FileVersion = value; }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Sound.h File")]
        [Category("Project")]
        [Editor(typeof(CustomFileBrowser), typeof(UITypeEditor))]
        public string SoundhFile
        {
            get { return _SoundhFile; }
            set { _SoundhFile = value; ((FrmMain)Application.OpenForms[nameof(FrmMain)]).hashTable.LoadHashTable(); }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Platform")]
        [Category("Project")]
        public Platform PlatformSelected
        {
            get { return _PlatformSelected; }
            set
            {
                _PlatformSelected = value;

                //Check Exceptions
                if (value == Platform.GameCube && TitleSelected == Title.BatmanBegins)
                {
                    StreamsFrequency = 16000;
                }
                else
                {
                    StreamsFrequency = 22050;
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Title")]
        [Category("Project")]
        public Title TitleSelected
        {
            get { return _TitleSelected; }
            set
            {
                _TitleSelected = value;
                switch (value)
                {
                    case Title.Buffy:
                        FileVersion = 201;
                        break;
                    case Title.Sphinx:
                        FileVersion = 201;
                        break;
                    case Title.Athens:
                        FileVersion = 1;
                        break;
                    case Title.Spyro:
                        FileVersion = 4;
                        break;
                    case Title.Robots:
                        FileVersion = 5;
                        break;
                    case Title.Predator:
                        FileVersion = 5;
                        break;
                    case Title.BatmanBegins:
                        FileVersion = 6;
                        break;
                    case Title.IceAge2:
                        FileVersion = 6;
                        break;
                }

                //Check Exceptions
                if (PlatformSelected == Platform.GameCube && value == Title.BatmanBegins)
                {
                    StreamsFrequency = 16000;
                }
                else
                {
                    StreamsFrequency = 22050;
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Files Folder")]
        [Category("Project")]
        [Editor(typeof(CustomFolderBrowser), typeof(UITypeEditor))]
        public string ProjectFolder
        {
            get { return _ProjectFolder; }
            set { _ProjectFolder = value; ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.LoadData(); }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
