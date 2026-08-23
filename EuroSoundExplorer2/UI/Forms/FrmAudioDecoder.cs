using AudioDecoders;
using NAudio.Wave;
using sb_explorer.Classes;
using System;
using System.IO;
using System.Windows.Forms;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmAudioDecoder : Form
    {
        private readonly AudioFunctions audioClass = new AudioFunctions();

        //------------------------------------------------------------------------------------------------------------------------------
        public FrmAudioDecoder()
        {
            InitializeComponent();
            lbxFormats.SelectedIndex = 0;
            lbxFormats.SelectedIndexChanged += LbxFormats_SelectedIndexChanged;
        }

        //------------------------------------------------------------------------------------------------------------------------------
        private void BtnBrowseFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btnConvert.Enabled = true;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BtnConvert_Click(object sender, EventArgs e)
        {
            foreach (string fileName in openFileDialog1.FileNames)
            {
                //Read all data that we have to decode
                byte[] rawAdpcmFile = File.ReadAllBytes(fileName);

                if (rawAdpcmFile.Length > nudHeaderBytes.Value)
                {
                    //Get an array with the encoded data that we have to decode excluding the header bytes
                    byte[] adpcmDataToDecode = new byte[rawAdpcmFile.Length - (int)nudHeaderBytes.Value];
                    Array.Copy(rawAdpcmFile, (int)nudHeaderBytes.Value, adpcmDataToDecode, 0, adpcmDataToDecode.Length);

                    //Decode data
                    byte[] pcmConvertedData = null;
                    try
                    {
                        switch (lbxFormats.SelectedIndex)
                        {
                            case 0: // Sony ADPCM
                                uint loopOffset = 0;
                                SonyAdpcm sonyVag = new SonyAdpcm();
                                pcmConvertedData = sonyVag.Decode(adpcmDataToDecode, ref loopOffset);
                                break;
                            case 1: // IMA Adpcm
                                ImaAdpcm imaCodec = new ImaAdpcm();
                                pcmConvertedData = audioClass.ShortArrayToByteArray(imaCodec.Decode(adpcmDataToDecode, adpcmDataToDecode.Length * 2));
                                break;
                            case 2: // EngineXT v15/v18 legacy DSP container
                                pcmConvertedData = audioClass.ShortArrayToByteArray(new LegacyDspAdpcm().Decode(rawAdpcmFile));
                                break;
                            case 3: // EngineXT v21 NGCA DSP container
                                pcmConvertedData = audioClass.ShortArrayToByteArray(new NgcaDspAdpcm().Decode(rawAdpcmFile));
                                break;
                            case 4: // Eurocom ADPCM
                                Eurocom_ImaAdpcm eurocomCodec = new Eurocom_ImaAdpcm();
                                pcmConvertedData = audioClass.ShortArrayToByteArray(eurocomCodec.Decode(adpcmDataToDecode));
                                break;
                            case 5: // Xbox ADPCM
                                XboxAdpcm xboxCodec = new XboxAdpcm();
                                pcmConvertedData = audioClass.ShortArrayToByteArray(xboxCodec.Decode(adpcmDataToDecode));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //Write decoded data
                    if (pcmConvertedData != null)
                    {
                        SoundFile soundToPlay = new SoundFile();
                        soundToPlay.PcmData[0] = pcmConvertedData;
                        soundToPlay.sampleRate = (uint)nudFrequency.Value;
                        soundToPlay.channels = (uint)nudChannels.Value;

                        //Create Wav file
                        RawSourceWaveStream rawLeftChannel = null;
                        IWaveProvider wavFile = audioClass.CreateMonoWav(ref rawLeftChannel, soundToPlay.PcmData[0], soundToPlay);
                        WaveFileWriter.CreateWaveFile16(fileName + "_Decode.wav", wavFile.ToSampleProvider());

                        //Write raw data
                        File.WriteAllBytes(fileName + "_Decode.raw", pcmConvertedData);
                    }
                }
                else
                {
                    MessageBox.Show("Header file size exceeds the file length.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //Close form at the end
            Close();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LbxFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFormats.SelectedIndex == 2)
                nudHeaderBytes.Value = LegacyDspAdpcm.HeaderSize;
            else if (lbxFormats.SelectedIndex == 3)
                nudHeaderBytes.Value = NgcaDspAdpcm.HeaderSize;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
