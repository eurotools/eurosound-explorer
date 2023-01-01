using MusX;
using MusX.Objects;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmDataViewer
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        private void ShowSoundBank(string filePath)
        {
            SortedDictionary<uint, Sample> SfxSamples = new SortedDictionary<uint, Sample>();
            List<SampleData> waveData = new List<SampleData>();

            //Read File Data 
            SfxHeaderData headerData = sbReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            sbReader.ReadSoundBank(filePath, headerData, SfxSamples, waveData);

            //Update Toolbar
            labelPlatformValue.Text = string.Format("{0}", headerData.Platform);
            labelMemoryValue.Text = string.Format("{0} kb", headerData.FileSize / 1024);
            labelErrorsValue.Text = string.Format("{0}", m_ErrorCount);

            //Add data
            TreeNode soundbankInfo = new TreeNode("SoundbankInfo");
            treeView1.Nodes.Add(soundbankInfo);

            //Print Soundbank Header Dat
            TreeAdd(soundbankInfo, nameof(headerData.FileHashCode), headerData.FileHashCode);
            TreeAdd(soundbankInfo, nameof(headerData.FileVersion), headerData.FileVersion);
            TreeAdd(soundbankInfo, nameof(headerData.FileSize), headerData.FileSize);

            if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
            {
                TreeAdd(soundbankInfo, nameof(headerData.Timespan), headerData.Timespan);
                TreeAdd(soundbankInfo, nameof(headerData.EurocomIma), headerData.EurocomIma);
            }

            TreeAdd(soundbankInfo, "SFXParametersStart", headerData.SFXStart);
            TreeAdd(soundbankInfo, "SFXParametersLength", headerData.SFXLenght);

            TreeAdd(soundbankInfo, nameof(headerData.SampleInfoStart), headerData.SampleInfoStart);
            TreeAdd(soundbankInfo, nameof(headerData.SampleInfoLenght), headerData.SampleInfoLenght);

            TreeAdd(soundbankInfo, nameof(headerData.SpecialSampleInfoStart), headerData.SpecialSampleInfoStart);
            TreeAdd(soundbankInfo, nameof(headerData.SpecialSampleInfoLength), headerData.SpecialSampleInfoLength);

            TreeAdd(soundbankInfo, nameof(headerData.SampleDataStart), headerData.SampleDataStart);
            TreeAdd(soundbankInfo, nameof(headerData.SampleDataLength), headerData.SampleDataLength);

            //Print SFX Parameters List
            TreeNode sfxParameters = new TreeNode("SFXParameters " + SfxSamples.Count);
            treeView1.Nodes.Add(sfxParameters);

            foreach (KeyValuePair<uint, Sample> item in SfxSamples)
            {
                TreeNode hashCode = new TreeNode(string.Format("u32 {0} = {1} (0x{1:X8})", "HashCode", item.Key));
                sfxParameters.Nodes.Add(hashCode);

                TreeAdd(hashCode, nameof(item.Value.DuckerLenght), item.Value.DuckerLenght);
                TreeAdd(hashCode, nameof(item.Value.MinDelay), item.Value.MinDelay);
                TreeAdd(hashCode, nameof(item.Value.MaxDelay), item.Value.MaxDelay);
                TreeAdd(hashCode, nameof(item.Value.InnerRadius), item.Value.InnerRadius);
                TreeAdd(hashCode, nameof(item.Value.OuterRadius), item.Value.OuterRadius);
                TreeAdd(hashCode, nameof(item.Value.ReverbSend), item.Value.ReverbSend);
                TreeAdd(hashCode, nameof(item.Value.TrackingType), item.Value.TrackingType);
                TreeAdd(hashCode, nameof(item.Value.MaxVoices), item.Value.MaxVoices);
                TreeAdd(hashCode, nameof(item.Value.Priority), item.Value.Priority);
                TreeAdd(hashCode, nameof(item.Value.Ducker), item.Value.Ducker);
                TreeAdd(hashCode, nameof(item.Value.MasterVolume), item.Value.MasterVolume);

                //Add Samples
                TreeNode sfxPoolElements = new TreeNode("SFXPoolElements " + item.Value.samplesList.Count);
                hashCode.Nodes.Add(sfxPoolElements);
                foreach (SampleInfo sampleToPrint in item.Value.samplesList)
                {
                    TreeNode fileRef = new TreeNode(string.Format("u32 {0} = {1} (0x{1:X8})", nameof(sampleToPrint.FileRef), sampleToPrint.FileRef));
                    sfxPoolElements.Nodes.Add(fileRef);

                    TreeAdd(fileRef, nameof(sampleToPrint.FileRef), sampleToPrint.FileRef);
                    if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
                    {
                        TreeAdd(fileRef, nameof(sampleToPrint.Pitch), (short)(sampleToPrint.Pitch / 0.2f));
                        TreeAdd(fileRef, nameof(sampleToPrint.PitchOffset), (short)(sampleToPrint.PitchOffset / 0.1f));
                    }
                    else
                    {
                        TreeAdd(fileRef, nameof(sampleToPrint.Pitch), (short)(sampleToPrint.Pitch * 1024));
                        TreeAdd(fileRef, nameof(sampleToPrint.PitchOffset), (short)(sampleToPrint.PitchOffset * 1024));
                    }
                    TreeAdd(fileRef, nameof(sampleToPrint.Volume), (sbyte)sampleToPrint.Volume);
                    TreeAdd(fileRef, nameof(sampleToPrint.VolumeOffset), (sbyte)sampleToPrint.VolumeOffset);
                    TreeAdd(fileRef, nameof(sampleToPrint.Pan), (sbyte)sampleToPrint.Pan);
                    TreeAdd(fileRef, nameof(sampleToPrint.PanOffset), (sbyte)sampleToPrint.PanOffset);
                }
            }

            //Print SFX Waves List
            TreeNode sampleInfo = new TreeNode("SampleInfo " + waveData.Count);
            treeView1.Nodes.Add(sampleInfo);

            for (int i = 0; i < waveData.Count; i++)
            {
                TreeNode waveNode = new TreeNode(i.ToString());
                sampleInfo.Nodes.Add(waveNode);
                TreeAdd(waveNode, "Flags", waveData[i].Flags);
                TreeAdd(waveNode, "Address", waveData[i].Address);
                TreeAdd(waveNode, "MemorySize", waveData[i].MemorySize);
                TreeAdd(waveNode, "Frequency", waveData[i].Frequency);
                TreeAdd(waveNode, "SampleSize", waveData[i].SampleSize);
                if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                {
                    TreeAdd(waveNode, "Channels", waveData[i].Channels);
                    TreeAdd(waveNode, "Bits", waveData[i].Bits);
                }
                TreeAdd(waveNode, "PsiSampleHeader", waveData[i].PsiSampleHeader);
                TreeAdd(waveNode, "SampleLoopOffset", waveData[i].LoopStartOffset);
                TreeAdd(waveNode, "Duration", waveData[i].Duration);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ShowStreamBank(string filePath)
        {
            List<StreamSample> waveData = new List<StreamSample>();

            //Read File Data 
            SfxHeaderData headerData = strReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            strReader.ReadStreamBank(filePath, headerData, waveData);

            //Update Toolbar
            labelPlatformValue.Text = string.Format("{0}", headerData.Platform);
            labelMemoryValue.Text = string.Format("{0} kb", headerData.FileSize / 1024);
            labelErrorsValue.Text = string.Format("{0}", m_ErrorCount);

            //Add data
            TreeNode soundbankInfo = new TreeNode("SoundbankInfo");
            treeView1.Nodes.Add(soundbankInfo);

            //Print Soundbank Header Dat
            TreeAdd(soundbankInfo, nameof(headerData.FileHashCode), headerData.FileHashCode);
            TreeAdd(soundbankInfo, nameof(headerData.FileVersion), headerData.FileVersion);
            TreeAdd(soundbankInfo, nameof(headerData.FileSize), headerData.FileSize);

            if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
            {
                TreeAdd(soundbankInfo, nameof(headerData.Timespan), headerData.Timespan);
                TreeAdd(soundbankInfo, nameof(headerData.EurocomIma), headerData.EurocomIma);
            }

            TreeAdd(soundbankInfo, nameof(headerData.FileStart1), headerData.FileStart1);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength1), headerData.FileLength1);

            TreeAdd(soundbankInfo, nameof(headerData.FileStart2), headerData.FileStart2);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength2), headerData.FileLength2);

            TreeAdd(soundbankInfo, nameof(headerData.FileStart3), headerData.FileStart3);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength3), headerData.FileLength3);

            //Print SFX Parameters List
            TreeNode streamSamples = new TreeNode("StreamSamples " + waveData.Count);
            treeView1.Nodes.Add(streamSamples);

            int index = 0;
            foreach (StreamSample streamSample in waveData)
            {
                TreeNode strSample = new TreeNode((index++).ToString());
                streamSamples.Nodes.Add(strSample);
                TreeAdd(strSample, nameof(streamSample.MarkerSize), streamSample.MarkerSize);
                TreeAdd(strSample, nameof(streamSample.AudioOffset), streamSample.AudioOffset);
                TreeAdd(strSample, nameof(streamSample.AudioSize), streamSample.AudioSize);

                TreeNode streamMarkerData = new TreeNode("Stream Header Data");
                strSample.Nodes.Add(streamMarkerData);
                TreeAdd(streamMarkerData, nameof(streamSample.StartMarkersCount), streamSample.StartMarkersCount);
                TreeAdd(streamMarkerData, nameof(streamSample.MarkersCount), streamSample.MarkersCount);
                TreeAdd(streamMarkerData, nameof(streamSample.StartMarkerOffset), streamSample.StartMarkerOffset);
                TreeAdd(streamMarkerData, nameof(streamSample.MarkerOffset), streamSample.MarkerOffset);
                TreeAdd(streamMarkerData, nameof(streamSample.BaseVolume), streamSample.BaseVolume);

                TreeNode streamStartMarker = new TreeNode("Stream Start Markers " + streamSample.StartMarkers.Length);
                strSample.Nodes.Add(streamStartMarker);

                for (int i = 0; i < streamSample.StartMarkers.Length; i++)
                {
                    StartMarker currentSample = streamSample.StartMarkers[i];

                    TreeNode startMarkerData = new TreeNode(i.ToString());
                    streamStartMarker.Nodes.Add(startMarkerData);
                    TreeAdd(startMarkerData, nameof(currentSample.Index), currentSample.Index);
                    TreeAdd(startMarkerData, nameof(currentSample.Position), currentSample.Position);
                    TreeAdd(startMarkerData, nameof(currentSample.Type), currentSample.Type);
                    if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                    {
                        TreeAdd(startMarkerData, nameof(currentSample.Flags), currentSample.Flags);
                        TreeAdd(startMarkerData, nameof(currentSample.Extra), currentSample.Extra);
                    }
                    TreeAdd(startMarkerData, nameof(currentSample.LoopStart), currentSample.LoopStart);
                    if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                    {
                        TreeAdd(startMarkerData, nameof(currentSample.MarkerCount), currentSample.MarkerCount);
                    }
                    TreeAdd(startMarkerData, nameof(currentSample.LoopMarkerCount), currentSample.LoopMarkerCount);
                    TreeAdd(startMarkerData, nameof(currentSample.MarkerPos), currentSample.MarkerPos);
                    if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                    {
                        TreeAdd(startMarkerData, nameof(currentSample.IsInstant), currentSample.IsInstant);
                        TreeAdd(startMarkerData, nameof(currentSample.InstantBuffer), currentSample.InstantBuffer);
                        TreeAdd(startMarkerData, nameof(currentSample.StateA), currentSample.StateA);
                        TreeAdd(startMarkerData, nameof(currentSample.StateB), currentSample.StateB);
                    }
                }

                TreeNode streamMarker = new TreeNode("Stream Markers " + streamSample.StartMarkers.Length);
                strSample.Nodes.Add(streamMarker);

                for (int i = 0; i < streamSample.StartMarkers.Length; i++)
                {
                    Marker currentSample = streamSample.Markers[i];

                    TreeNode markerData = new TreeNode(i.ToString());
                    streamMarker.Nodes.Add(markerData);
                    TreeAdd(markerData, nameof(currentSample.Index), currentSample.Index);
                    TreeAdd(markerData, nameof(currentSample.Position), currentSample.Position);
                    TreeAdd(markerData, nameof(currentSample.Type), currentSample.Type);
                    if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                    {
                        TreeAdd(markerData, nameof(currentSample.Flags), currentSample.Flags);
                        TreeAdd(markerData, nameof(currentSample.Extra), currentSample.Extra);
                    }
                    TreeAdd(markerData, nameof(currentSample.LoopStart), currentSample.LoopStart);
                    if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                    {
                        TreeAdd(markerData, nameof(currentSample.MarkerCount), currentSample.MarkerCount);
                    }
                    TreeAdd(markerData, nameof(currentSample.LoopMarkerCount), currentSample.LoopMarkerCount);
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}