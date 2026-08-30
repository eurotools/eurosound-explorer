$ErrorActionPreference = 'Stop'
$asm = [Reflection.Assembly]::LoadFrom((Join-Path $PSScriptRoot 'bin\Debug\sb_explorer.exe'))
$resolver = $asm.GetType('MusX.EuroSoundStreamLoopResolver')
$method = $resolver.GetMethods([Reflection.BindingFlags]'NonPublic,Static') |
    Where-Object { $_.Name -eq 'TryResolveV18' -and $_.GetParameters()[0].ParameterType.Name -eq 'StreamSample' }
$errors = [Collections.Generic.List[string]]::new()
$counts = @{}
$loops = 0

foreach ($file in Get-ChildItem 'Y:\GamesSoundbanks' -Recurse -File -Filter *.sfx |
    Where-Object { $_.Name -match '(?i)_STR_' -and $_.FullName -notmatch 'Pirates' }) {
    try {
        $reader = [MusX.Readers.StreamBankReader]::new()
        $header = $reader.ReadStreamBankHeader($file.FullName, 'None')
        if ($header.FileVersion -notin 15,18,21) { continue }
        if (($header.StreamFlags -band 1) -eq 0) { continue }
        $codec = switch ($header.FileStart1) {
            1 { [MusX.EuroSoundAudioCodec]::EurocomImaAdpcm }
            2 { [MusX.EuroSoundAudioCodec]::SonyVagAdpcm }
            3 { [MusX.EuroSoundAudioCodec]::DspAdpcmNgca }
            4 { [MusX.EuroSoundAudioCodec]::Pcm16 }
            6 { [MusX.EuroSoundAudioCodec]::Xma }
            default { [MusX.EuroSoundAudioCodec]::Unknown }
        }
        $sample = [MusX.Objects.StreamSample]::new()
        $sample.Flags = $header.StreamFlags
        $sample.SampleCount = $header.SampleCount
        $sample.LoopStartSample = $header.LoopStartSample
        $sample.LoopStartByteOffset = $header.LoopStartByteOffset
        $sample.LoopEndByteOffset = $header.LoopEndByteOffset
        $sample.Channels = [Math]::Max(1, $header.Channels)
        $sample.AudioReference = [MusX.Objects.AudioDataReference]::new()
        $sample.AudioReference.Codec = $codec
        $sample.AudioReference.Channels = [int]$sample.Channels
        $loops++
        $args = [object[]]@($sample, [int]$sample.SampleCount, [uint32]0, [uint32]0)
        $ok = $method.Invoke($null, $args)
        if (!$ok -or $args[2] -ge $args[3] -or $args[3] -gt $sample.SampleCount) {
            $errors.Add("v$($header.FileVersion)|$($sample.AudioReference.Codec)|start=$($args[2]) end=$($args[3]) total=$($sample.SampleCount)|$($file.FullName)")
        }
        $key = "v$($header.FileVersion)-$($sample.AudioReference.Codec)"
        $counts[$key] = 1 + ($counts[$key] -as [int])
    } catch {
        $errors.Add("READ|$($file.FullName)|$($_.Exception.GetBaseException().Message)")
    }
}

$counts.GetEnumerator() | Sort-Object Name | ForEach-Object { "LOOPGROUP|$($_.Name)|$($_.Value)" }
$errors | Select-Object -First 30
"ENGINELOOPS|Loops=$loops|Errors=$($errors.Count)"

$markerResolver = $asm.GetType('sb_explorer.Services.Audio.EuroSoundMarkerLoopResolver')
$markerMethod = $markerResolver.GetMethod('TryResolvePlayback', [Reflection.BindingFlags]'Public,Static')
$markerMode = $asm.GetType('sb_explorer.Services.Audio.MarkerLoopMode')
$loopUnlessEnd = [Enum]::Parse($markerMode, 'LoopUnlessEndMarker')
$legacyErrors = [Collections.Generic.List[string]]::new()
$legacyLoops = 0
foreach ($file in Get-ChildItem 'Y:\GamesSoundbanks' -Recurse -File -Filter *.sfx |
    Where-Object { $_.Name -match '(?i)_STR_' -and $_.FullName -notmatch 'Pirates' }) {
    try {
        $reader = [MusX.Readers.StreamBankReader]::new()
        $header = $reader.ReadStreamBankHeader($file.FullName, 'None')
        if ($header.FileVersion -in 15,18,21) { continue }
        $samples = [Collections.Generic.List[MusX.Objects.StreamSample]]::new()
        $reader.ReadStreamBank($file.FullName, $header, $samples)
        foreach ($sample in $samples) {
            if ($null -eq $sample.AudioReference) { continue }
            $total = [MusX.EuroSoundCodecMatrix]::EncodedByteCountToSamples(
                $sample.AudioReference.Codec, $sample.AudioSize, [Math]::Max(1, $sample.AudioReference.Channels))
            if ($total -eq 0) { continue }
            $args = [object[]]@($sample.Markers, [int][Math]::Min([int]::MaxValue, $total), $loopUnlessEnd, 0, [uint32]0, 0)
            $ok = $markerMethod.Invoke($null, $args)
            if (!$ok) { continue }
            $legacyLoops++
            if ($args[4] -ge $args[5] -or $args[5] -gt $total) {
                $legacyErrors.Add("v$($header.FileVersion)|$($sample.AudioReference.Codec)|start=$($args[4]) end=$($args[5]) total=$total|$($file.FullName)")
            }
        }
    } catch {
        $legacyErrors.Add("READ|$($file.FullName)|$($_.Exception.GetBaseException().Message)")
    }
}
$legacyErrors | Select-Object -First 30
"LEGACYLOOPS|Loops=$legacyLoops|Errors=$($legacyErrors.Count)"
