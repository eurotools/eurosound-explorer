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

            //Add data
            TreeNode soundbankInfo = ShowHeaderData(headerData, "SoundbankInfo");

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
                if (headerData.FileVersion == 201 || headerData.FileVersion == 01)
                {
                    TreeAdd(hashCode, nameof(item.Value.InnerRadius), item.Value.InnerRadius);
                    TreeAdd(hashCode, nameof(item.Value.OuterRadius), item.Value.OuterRadius);
                }
                TreeAdd(hashCode, nameof(item.Value.ReverbSend), item.Value.ReverbSend);
                TreeAdd(hashCode, nameof(item.Value.TrackingType), item.Value.TrackingType);
                TreeAdd(hashCode, nameof(item.Value.MaxVoices), item.Value.MaxVoices);
                TreeAdd(hashCode, nameof(item.Value.Priority), item.Value.Priority);
                TreeAdd(hashCode, nameof(item.Value.Ducker), item.Value.Ducker);
                TreeAdd(hashCode, nameof(item.Value.MasterVolume), item.Value.MasterVolume);
                if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
                {
                    TreeAdd(hashCode, nameof(item.Value.GroupHashCode), item.Value.GroupHashCode);
                    TreeAdd(hashCode, nameof(item.Value.GroupMaxChannels), item.Value.GroupMaxChannels);
                }
                TreeAdd(hashCode, nameof(item.Value.Flags), item.Value.Flags);
                if (headerData.FileVersion > 5 && headerData.FileVersion < 10)
                {
                    TreeAdd(hashCode, nameof(item.Value.SFXDucker), item.Value.SFXDucker);
                    TreeAdd(hashCode, nameof(item.Value.Spare), item.Value.Spare);
                }
                //Add Samples
                TreeNode sfxPoolElements = new TreeNode("SFXPoolElements " + item.Value.samplesList.Count);
                hashCode.Nodes.Add(sfxPoolElements);
                foreach (SampleInfo sampleToPrint in item.Value.samplesList)
                {
                    TreeNode fileRef = new TreeNode(string.Format("s16 {0} = {1} (0x{1:X4})", nameof(sampleToPrint.FileRef), sampleToPrint.FileRef));
                    sfxPoolElements.Nodes.Add(fileRef);
                    TreeAdd(fileRef, nameof(sampleToPrint.Pitch), (short)sampleToPrint.Pitch);
                    TreeAdd(fileRef, nameof(sampleToPrint.PitchOffset), (short)sampleToPrint.PitchOffset);
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
                SampleData currentSample = waveData[i];

                TreeNode waveNode = new TreeNode(i.ToString());
                sampleInfo.Nodes.Add(waveNode);
                TreeAdd(waveNode, nameof(currentSample.Flags), currentSample.Flags);
                TreeAdd(waveNode, nameof(currentSample.Address), currentSample.Address);
                TreeAdd(waveNode, nameof(currentSample.MemorySize), currentSample.MemorySize);
                TreeAdd(waveNode, nameof(currentSample.Frequency), currentSample.Frequency);
                TreeAdd(waveNode, nameof(currentSample.SampleSize), currentSample.SampleSize);
                if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                {
                    TreeAdd(waveNode, nameof(currentSample.Channels), currentSample.Channels);
                    TreeAdd(waveNode, nameof(currentSample.Bits), currentSample.Bits);
                }
                TreeAdd(waveNode, nameof(currentSample.PsiSampleHeader), currentSample.PsiSampleHeader);
                TreeAdd(waveNode, nameof(currentSample.LoopStartOffset), currentSample.LoopStartOffset);
                TreeAdd(waveNode, nameof(currentSample.Duration), currentSample.Duration);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ShowStreamBank(string filePath)
        {
            List<StreamSample> waveData = new List<StreamSample>();

            //Read File Data 
            SfxHeaderData headerData = strReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            strReader.ReadStreamBank(filePath, headerData, waveData);

            //Add data
            TreeNode soundbankInfo = ShowHeaderData(headerData, "StreambankInfo");

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

                //Print Markers
                PrintMarkers(strSample, headerData, streamSample.StartMarkers, streamSample.Markers);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ShowMusicBank(string filePath)
        {
            List<StreamSample> waveData = new List<StreamSample>();

            //Read File Data 
            SfxHeaderData headerData = musReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            MusicSample fileData = musReader.ReadMusicBank(filePath, headerData);

            //Add data
            TreeNode soundbankInfo = ShowHeaderData(headerData, "MusicbankInfo");

            TreeAdd(soundbankInfo, nameof(headerData.FileStart1), headerData.FileStart1);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength1), headerData.FileLength1);

            TreeAdd(soundbankInfo, nameof(headerData.FileStart2), headerData.FileStart2);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength2), headerData.FileLength2);

            TreeAdd(soundbankInfo, nameof(headerData.FileStart3), headerData.FileStart3);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength3), headerData.FileLength3);

            TreeNode streamMarkerData = new TreeNode("Stream Header Data");
            soundbankInfo.Nodes.Add(streamMarkerData);
            TreeAdd(streamMarkerData, nameof(fileData.StartMarkersCount), fileData.StartMarkersCount);
            TreeAdd(streamMarkerData, nameof(fileData.MarkersCount), fileData.MarkersCount);
            TreeAdd(streamMarkerData, nameof(fileData.StartMarkerOffset), fileData.StartMarkerOffset);
            TreeAdd(streamMarkerData, nameof(fileData.MarkerOffset), fileData.MarkerOffset);
            TreeAdd(streamMarkerData, nameof(fileData.BaseVolume), fileData.BaseVolume);

            //Print Markers
            PrintMarkers(soundbankInfo, headerData, fileData.StartMarkers, fileData.Markers);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ShowSbiBank(string filePath)
        {
            //Read File Data 
            SfxHeaderData headerData = sbiReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            SbiFile fileData = sbiReader.ReadStreamFile(filePath, headerData);

            //Add data
            TreeNode soundbankInfo = ShowHeaderData(headerData, "SoundbankInfo");

            TreeAdd(soundbankInfo, nameof(headerData.FileStart1), headerData.FileStart1);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength1), headerData.FileLength1);

            TreeAdd(soundbankInfo, nameof(headerData.FileStart2), headerData.FileStart2);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength2), headerData.FileLength2);

            TreeAdd(soundbankInfo, nameof(headerData.FileStart3), headerData.FileStart3);
            TreeAdd(soundbankInfo, nameof(headerData.FileLength3), headerData.FileLength3);

            //Print SoundBanks
            TreeNode soundBanksNode = new TreeNode("SoundBanks " + fileData.projectSoundBanks.Length);
            soundbankInfo.Nodes.Add(soundBanksNode);
            for (int i = 0; i < fileData.projectSoundBanks.Length; i++)
            {
                TreeAdd(soundBanksNode, "HashCode", fileData.projectSoundBanks[i]);
            }

            //Print MusicBanks
            TreeNode musicBanksNdoe = new TreeNode("SoundBanks " + fileData.projectMusicBanks.Length);
            soundbankInfo.Nodes.Add(musicBanksNdoe);
            for (int i = 0; i < fileData.projectMusicBanks.Length; i++)
            {
                TreeAdd(musicBanksNdoe, "HashCode", fileData.projectMusicBanks[i]);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ShowProjectDetails(string filePath)
        {
            //Read File Data 
            SfxHeaderData headerData = projDetReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            ProjectDetails fileData = projDetReader.ReadProjectFile(filePath, headerData);

            //Add data
            TreeNode soundbankInfo = ShowHeaderData(headerData, "ProjectDetails Info");

            TreeAdd(soundbankInfo, nameof(headerData.MemoryStart), headerData.MemoryStart);
            TreeAdd(soundbankInfo, nameof(headerData.MemoryLength), headerData.MemoryLength);

            //Print Start Offsets
            TreeNode fileSections = new TreeNode("File Sections Info");
            treeView1.Nodes.Add(fileSections);

            TreeAdd(fileSections, nameof(fileData.MemmorySlotsCount), fileData.MemmorySlotsCount);
            TreeAdd(fileSections, nameof(fileData.MemorySlotsOffset), fileData.MemorySlotsOffset);
            TreeAdd(fileSections, nameof(fileData.SoundBanksCount), fileData.SoundBanksCount);
            TreeAdd(fileSections, nameof(fileData.SoundBanksOffset), fileData.SoundBanksOffset);


            //Print Flags
            TreeNode flagsSection = new TreeNode("Flags Section");
            treeView1.Nodes.Add(flagsSection);
            TreeAdd(flagsSection, nameof(fileData.StereoStreamCount), fileData.StereoStreamCount);
            TreeAdd(flagsSection, nameof(fileData.MonoStreamCount), fileData.MonoStreamCount);
            TreeAdd(flagsSection, nameof(fileData.ProjectCode), fileData.ProjectCode);
            for (int i = 0; i < fileData.flagsValues.Length; i++)
            {
                TreeAdd(flagsSection, i.ToString(), fileData.flagsValues[i]);
            }

            //Print Project Slots
            TreeNode memSlots = new TreeNode("Memory Slots " + fileData.memorySlotsData.Count);
            treeView1.Nodes.Add(memSlots);
            foreach (ProjectSlots item in fileData.memorySlotsData)
            {
                TreeNode memSlot = new TreeNode(string.Format("u32 {0} = {1} (0x{1:X8})", nameof(item.SlotNumber), item.SlotNumber));
                memSlots.Nodes.Add(memSlot);
                TreeAdd(memSlot, nameof(item.MemorySize), item.MemorySize);
                TreeAdd(memSlot, nameof(item.Quantity), item.Quantity);
            }

            //Print SoundBanks Slots
            TreeNode sbMemSlots = new TreeNode("SoundBanks Memory Slots " + fileData.soundBanksData.Count);
            treeView1.Nodes.Add(sbMemSlots);
            foreach (ProjectSoundBank item in fileData.soundBanksData)
            {
                TreeNode sbNode = new TreeNode(string.Format("u32 {0} = {1} (0x{1:X8})", nameof(item.HashCode), item.HashCode));
                sbMemSlots.Nodes.Add(sbNode);
                TreeAdd(sbNode, nameof(item.SlotNumber), item.SlotNumber);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private TreeNode ShowHeaderData(SfxHeaderData headerData, string nodeName)
        {
            //Update Toolbar
            labelPlatformValue.Text = string.Format("{0}", headerData.Platform);
            labelMemoryValue.Text = string.Format("{0} kb", headerData.FileSize / 1024);
            labelErrorsValue.Text = string.Format("{0}", m_ErrorCount);

            //Add data
            TreeNode soundbankInfo = new TreeNode(nodeName);
            treeView1.Nodes.Add(soundbankInfo);

            //Print Soundbank Header Dat
            TreeAdd(soundbankInfo, nameof(headerData.FileHashCode), headerData.FileHashCode);
            TreeAdd(soundbankInfo, nameof(headerData.FileVersion), headerData.FileVersion);
            TreeAdd(soundbankInfo, nameof(headerData.FileSize), headerData.FileSize);

            if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
            {
                TreeAdd(soundbankInfo, nameof(headerData.Platform), headerData.Platform);
                TreeAdd(soundbankInfo, nameof(headerData.Timespan), headerData.Timespan);
                TreeAdd(soundbankInfo, nameof(headerData.UsesAdpcm), headerData.UsesAdpcm);
            }

            return soundbankInfo;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void PrintMarkers(TreeNode strSample, SfxHeaderData headerData, StartMarker[] StartMarkers, Marker[] Markers)
        {
            //Print Start Markers
            TreeNode streamStartMarker = new TreeNode("Stream Start Markers " + StartMarkers.Length);
            strSample.Nodes.Add(streamStartMarker);
            for (int i = 0; i < StartMarkers.Length; i++)
            {
                StartMarker currentSample = StartMarkers[i];

                TreeNode startMarkerData = new TreeNode(string.Format("s32 {0} = {1} (0x{1:X8})", nameof(currentSample.Index), currentSample.Index));
                streamStartMarker.Nodes.Add(startMarkerData);
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

            //Print Markers 
            TreeNode streamMarker = new TreeNode("Stream Markers " + Markers.Length);
            strSample.Nodes.Add(streamMarker);
            for (int i = 0; i < Markers.Length; i++)
            {
                Marker currentSample = Markers[i];

                TreeNode markerData = new TreeNode(string.Format("s32 {0} = {1} (0x{1:X8})", nameof(currentSample.Index), currentSample.Index));
                streamMarker.Nodes.Add(markerData);
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

    //-------------------------------------------------------------------------------------------------------------------------------
}