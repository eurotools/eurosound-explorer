using System;
using System.ComponentModel;
using System.Globalization;

namespace EuroSoundExplorer2.Classes.PropertyGridHelpers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
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
            return new StandardValuesCollection(Enumerations.SfxFileVersion);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
