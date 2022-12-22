using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class AppConfig
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public enum Platform
        {
            None,
            GameCube,
            PC,
            PS2,
            Wii,
            Xbox,
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public enum Title
        {
            Buffy,
            Sphinx,
            Athens,
            Spyro,
            Robots,
            Predator,
            BatmanBegins,
            IceAge2
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static int[] SfxFileVersion = new int[]
        {
            1,
            4,
            5,
            6,
            201
        };

        //-------------------------------------------------------------------------------------------------------------------------------
        private Platform _PlatformSelected;
        private Title _TitleSelected;
        private string _SoundhFile, _ProjectFolder;
        private int _StreamsFrequency = 22050;
        private int _FileVersion = 201;

        //-------------------------------------------------------------------------------------------------------------------------------
        [DisplayName("Streams Freq.")]
        [Category("EngineX")]
        public int StreamsFrequency
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
            set { _ProjectFolder = value; }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    internal class CustomFileBrowser : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.Filter = "Hashcode File (*.h)|*.h";
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    internal class CustomFolderBrowser : FolderNameEditor
    {
        protected override void InitializeDialog(FolderBrowser openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    internal class FileVersions : TypeConverter
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true; // display drop
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true; // drop-down vs combo
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string @string)
            {
                // conversion logic goes here
                return int.Parse(@string);
            }
            return base.ConvertFrom(context, culture, value);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new int[] { 201, 4, 5, 7 });
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
