rh._.exports({"0":[[" ","SFX Properties"]],"1":[[" ","SFX Properties"," ","v1.00 Updated: 07/01/23"],[" ","This panel allows the user to see the parameters for selected SFX. The data is presented as a property grid."],[" ","Name"," ","Description"," ","Ducker"," ","In ducking, the level of one audio signal is reduced by the presence of another signal, this field specifies the transition percentage."," ","Ducker Length"," ","Duration to apply the duck for, in hundreds of a second, seemingly, internally gets turned into milliseconds."," ","Min Delay"," ","Minimum delay to apply before this sample starts playing."," ","Max Delay"," ","Maximum delay to apply before this sample starts playing."," ","Inner Radius"," ","The start point where getting close enough to the source will max out the \"distance\" \"volume\" and won't be attenuated."," ","Outer Radius"," ","The point where getting far enough from the center of the source will mute it, between the inner and the outer radius there will be a proportional falloff, fading in and out depending on the distance."," ","Reverb Send"," ","Specifies the reverberation that this sound will have when playing"," ","Priority"," ","The higher the priority, the higher the probability that the SFX instance won't be killed when there are ","too"," many sounds playing at the same time. There's a hardcoded number of maximum playable sounds, and it will start killing the lower-priority ones."," ","Master Volume"," ","Shared volume of all the samples contained in this SFX."," ","Max Voices"," ","How many simultaneous instances can be played back during gameplay."," ","Tracking Type"," ","Type"," ","Description"," ","2D"," ","The sound won't be panned in the left-right channels. Inherently stereo, uses: musical effects, menu sounds, narration speech..."," ","2D PL2"," ","Dolby Pro Logic 2",". A proprietary surround encoding. U","ses: musical effects, menu sounds, narration speech..."," ","AMB"," ","Ambient sound, uses: Environment sounds which surround player eg. rain"," ","3D"," ","Three-dimensional sound. Inherently mono. Uses: most sounds, sound acts as point source and is tracked."," ","3D Rnd Pos"," ","Three-dimensional sound ","with a random positional offset",". Inherently mono. ","Uses: seagulls, monkeys, thunders etc.."," ","Group Hashcode"," ","Number of the group in which this SFX is setted."," ","Group Max Channels"," ","Limit of voices that all SFXs stored in this group could have playing at the same time."],[" ","The selected flags are presented as a checked listbox, depending on the SFX version there are some slights differences"],[" ","Name V4 and upcoming"," ","Names V201 and 1"," ","Description"," ","Max Reject"," ","Max Reject"," ","There's a hard limit of voices playing at the same time, this flag specifies that when the limit has been reached, and this SFX is being setup, we can choose to kill either the oldest (playing) one of the same type, or this one that it was about to play."," ","UnPausable"," ","Doppler"," ","Ignore Master Volume"," ","Ignore Age"," ","If is set the samples of the list will play in different volumes."," ","Multi Sample"," ","Multi Sample"," ","If false it will pick and play randomly one of the samples in the list."," ","Random Pick"," ","Random Pick"," ","When multisample is off and there are more than one sample, if is set on, will pick and play randomly one of the samples in the list"," ","Shuffled"," ","Shuffled"," ","When multiSample is true and polyphonic is false, and this is enabled, it will interpret the list of samples as a (randomly) shuffled sequence."," ","Loop"," ","Loop"," ","If the SFX has ended it will either loop and restart it back anew or kill it."," ","Polyphonic"," ","Polyphonic"," ","If multiSample is true, and this is enabled, it plays back all the samples of the list at the same time, give or take. If there's a random delay that will affect the start time."," ","\n            Otherwise, if this is disabled, it will interpret the list as a sequence of multiple samples. If shuffled is false, in order. "," ","Underwater"," ","Underwater"," ","If set to false, the volume will be halved when the camera goes underwater."," ","Pause Instant"," ","PauseInNis"," ","Platform-specific stuff for the original Nintendo console"," ","Has Sub-SFX"," ","Hash Sub-SFX"," ","If true, the file reference of the first \"sample\" entry gets interpreted as a SFX hashcode and played as a 3D sound at the current position."," ","Steal On Louder"," ","Steal On Louder"," ","Replace similar sounds of the same type when this one is louder. Works as an alternative to maxReject in similar circumstances. Instead of killing the oldest one we kill and combine both volumes to get a similar sound envelope without losing too much sonority."," ","Treat Like Music"," ","Treat Like Music"," ","This sound uses the master music volume, think of the nomad or trading outpost jingles. If you disable music you can't hear them, even if they are ambient sounds."," ","Kill Me Own Group"," ","-"," ","If set on, when the sound instance is killed, will kill the other instances from the same hashcode group"," ","Group Steal Reject"," ","-"," ","Works like max reject, and steal on louder but in a group level."," ","One Instance Per Frame"," ","-"," ","If set on, will play/create an instance of this sound in each frame."," ","Eurotools Community."]],"2":[[" ","SFX Properties"],[" ","SFX Flags"]],"3":[[" ","Properties Information"],[" ","Flags Information"]],"id":"4"})