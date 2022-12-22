using MusX.Objects;
using System.Collections.Generic;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamBankReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------
        //  READ SFXs
        //-------------------------------------------------------------------------------------------
        public void ReadStreamBank(string filePath, SfxHeaderData headerData, List<StreamSample> streamedSamples)
        {
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                StreamBankReaderOld oldReader = new StreamBankReaderOld();
                oldReader.ReadStreamFile(filePath, headerData, streamedSamples);
            }
            else
            {
                StreamBankReaderNew oldReader = new StreamBankReaderNew();
                oldReader.ReadStreamFile(filePath, headerData, streamedSamples);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
