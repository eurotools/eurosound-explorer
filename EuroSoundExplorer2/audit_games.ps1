$ErrorActionPreference = 'Stop'
$bin = Join-Path $PSScriptRoot 'bin\Debug'
$asm = [Reflection.Assembly]::LoadFrom((Join-Path $bin 'sb_explorer.exe'))
$common = [MusX.Readers.SfxFunctions]::new()
$method = $asm.GetType('sb_explorer.GenericMethods').GetMethod('GetFileType', [Reflection.BindingFlags]'NonPublic,Static')
$root = 'Y:\GamesSoundbanks'

function Get-Title($name) {
    if ($name -match 'Buffy') { return [sb_explorer.Enumerations+Title]::Buffy }
    if ($name -match 'Sphinx') { return [sb_explorer.Enumerations+Title]::Sphinx }
    if ($name -match 'Spyro') { return [sb_explorer.Enumerations+Title]::Spyro }
    if ($name -match 'Athens') { return [sb_explorer.Enumerations+Title]::Athens }
    if ($name -match 'Batman') { return [sb_explorer.Enumerations+Title]::BatmanBegins }
    if ($name -match 'Predator') { return [sb_explorer.Enumerations+Title]::Predator }
    if ($name -match 'Robots') { return [sb_explorer.Enumerations+Title]::Robots }
    if ($name -match 'Ice Age') { return [sb_explorer.Enumerations+Title]::IceAge2 }
    if ($name -match 'GForce') { return [sb_explorer.Enumerations+Title]::GForce }
    return [sb_explorer.Enumerations+Title]::None
}

function Get-PathPlatform($path, $embedded) {
    if (-not [string]::IsNullOrWhiteSpace($embedded) -and $embedded -ne 'None') { return $embedded }
    if ($path -match '(?i)_bin_gc|\\gc\\') { return 'GameCube' }
    if ($path -match '(?i)_bin_ps2|\\ps2\\') { return 'PS2' }
    if ($path -match '(?i)_bin_ps3|\\ps3\\') { return 'PS3' }
    if ($path -match '(?i)_bin_xe|_bin_xb2|\\xbox360\\') { return 'Xbox360' }
    if ($path -match '(?i)_bin_xb|\\xbox\\') { return 'Xbox' }
    if ($path -match '(?i)_bin_wii|\\wii\\') { return 'Wii' }
    if ($path -match '(?i)_bin_pc|\\pc\\') { return 'PC' }
    return $embedded
}

$catalog = [Collections.Generic.List[object]]::new()
foreach ($game in Get-ChildItem -LiteralPath $root | Where-Object PSIsContainer | Where-Object { $_.Name -notmatch 'Pirates' }) {
    $title = Get-Title $game.Name
    foreach ($file in Get-ChildItem -LiteralPath $game.FullName -Recurse -File | Where-Object { $_.Extension -in '.sfx','.musx' }) {
        try {
            $header = $common.ReadCommonHeader($file.FullName, 'None')
            $signedHash = [BitConverter]::ToInt32([BitConverter]::GetBytes([uint32]$header.FileHashCode), 0)
            $type = $method.Invoke($null, [object[]]@($signedHash, [int]$header.FileVersion, $file.FullName, $title))
            $platform = Get-PathPlatform $file.FullName $header.Platform
            $catalog.Add([pscustomobject]@{ Game=$game.Name; Version=$header.FileVersion; Platform=$platform; Type=$type.ToString(); File=$file.FullName })
        } catch {
            Write-Output "HEADER_ERROR|$($game.Name)|$($file.FullName)|$($_.Exception.GetBaseException().Message)"
        }
    }
}

$tests = $catalog | Group-Object Game,Version,Platform,Type | ForEach-Object { $_.Group | Select-Object -First 1 }
$passed = 0
$errors = [Collections.Generic.List[object]]::new()
foreach ($test in $tests) {
    try {
        $data = [sb_explorer.LoadedProjectData]::new()
        $options = [sb_explorer.Services.EuroSoundFileLoadOptions]::new()
        $options.FilePath = $test.File
        $options.ProjectFolder = [IO.Path]::GetDirectoryName($test.File)
        $options.Platform = $test.Platform
        switch ($test.Type) {
            'SoundbankFile' { [void][sb_explorer.Services.EuroSoundFileLoader]::LoadSoundBank($options, $data) }
            'StreamFile' { [void][sb_explorer.Services.EuroSoundFileLoader]::LoadStreamBank($options, $data) }
            'MusicFile' { [void][sb_explorer.Services.EuroSoundFileLoader]::LoadMusicBank($options, $data) }
            'ProjectDetails' { [void][sb_explorer.Services.EuroSoundFileLoader]::LoadProjectDetails($options, $data) }
            'SoundDetailsFile' { [void][sb_explorer.Services.EuroSoundFileLoader]::LoadSoundDetails($options, $data) }
            'MusicDetails' { [void][sb_explorer.Services.EuroSoundFileLoader]::LoadMusicDetails($options, $data) }
            'MusicMarkers' { [void][sb_explorer.Services.EuroSoundFileLoader]::LoadMusicMarkers($options, $data) }
            'SBI' { [void][sb_explorer.Services.EuroSoundFileLoader]::LoadSbi($options, $data) }
            default { throw "Unsupported classification $($test.Type)" }
        }
        $passed++
    } catch {
        $errors.Add([pscustomobject]@{ Game=$test.Game; Version=$test.Version; Platform=$test.Platform; Type=$test.Type; File=$test.File; Error=$_.Exception.GetBaseException().Message })
    }
}

Write-Output "SUMMARY|Files=$($catalog.Count)|Groups=$($tests.Count)|Passed=$passed|Errors=$($errors.Count)"
$errors | ForEach-Object { Write-Output "LOAD_ERROR|$($_.Game)|$($_.Version)|$($_.Platform)|$($_.Type)|$($_.File)|$($_.Error)" }

$typeErrors = [Collections.Generic.List[string]]::new()
foreach ($entry in $catalog) {
    $name = [IO.Path]::GetFileName($entry.File)
    $expected = $null
    if ($name -match '(?i)projectdetails') { $expected = 'ProjectDetails' }
    elseif ($name -match '(?i)sounddetails') { $expected = 'SoundDetailsFile' }
    elseif ($name -match '(?i)musicdetails') { $expected = 'MusicDetails' }
    elseif ($name -match '(?i)musicmarkers') { $expected = 'MusicMarkers' }
    elseif ($name -match '(?i)_STR_') { $expected = 'StreamFile' }
    elseif ($name -match '(?i)_SB_') { $expected = 'SoundbankFile' }
    if ($null -ne $expected -and $entry.Type -ne $expected) {
        $typeErrors.Add("$($entry.Game)|$name|expected $expected, got $($entry.Type)")
    }
}
Write-Output "TYPECHECK|Checked=$($catalog.Count)|Errors=$($typeErrors.Count)"
$typeErrors | Select-Object -First 100 | ForEach-Object { Write-Output "TYPE_ERROR|$_" }

$countErrors = [Collections.Generic.List[string]]::new()
foreach ($entry in $tests) {
    try {
        if ($entry.Type -eq 'SoundbankFile') {
            $bankReader = [MusX.Readers.SoundBankReader]::new()
            $header = $bankReader.ReadSfxHeader($entry.File, $entry.Platform)
            $count = $bankReader.GetNumberOfSFXs($entry.File, $header)
            if ($count -lt 0) { $countErrors.Add("$($entry.File)|negative SFX count $count") }
        } elseif ($entry.Type -eq 'StreamFile' -and $entry.Version -notin 15,18,21) {
            $streamReader = [MusX.Readers.StreamBankReader]::new()
            $header = $streamReader.ReadStreamBankHeader($entry.File, $entry.Platform)
            if (($header.FileLength1 % 4) -ne 0) { $countErrors.Add("$($entry.File)|unaligned stream table length $($header.FileLength1)") }
            $streams = [Collections.Generic.List[MusX.Objects.StreamSample]]::new()
            $streamReader.ReadStreamBank($entry.File, $header, $streams)
            if ($streams.Count -ne ($header.FileLength1 / 4)) { $countErrors.Add("$($entry.File)|display $($header.FileLength1/4), reader $($streams.Count)") }
        }
    } catch {
        $countErrors.Add("$($entry.File)|$($_.Exception.GetBaseException().Message)")
    }
}
Write-Output "COUNTCHECK|Groups=$($tests.Count)|Errors=$($countErrors.Count)"
$countErrors | ForEach-Object { Write-Output "COUNT_ERROR|$_" }

function Convert-Platform($value) {
    if ($value -match '(?i)^GC|GameCube') { return [sb_explorer.Enumerations+Platform]::GameCube }
    if ($value -match '(?i)^WII|Wii') { return [sb_explorer.Enumerations+Platform]::Wii }
    if ($value -match '(?i)^PS2') { return [sb_explorer.Enumerations+Platform]::PS2 }
    if ($value -match '(?i)^PS3') { return [sb_explorer.Enumerations+Platform]::PS3 }
    if ($value -match '(?i)^XE|XB2|Xbox360') { return [sb_explorer.Enumerations+Platform]::Xbox360 }
    if ($value -match '(?i)^XB|Xbox') { return [sb_explorer.Enumerations+Platform]::Xbox }
    if ($value -match '(?i)^PC') { return [sb_explorer.Enumerations+Platform]::PC }
    return [sb_explorer.Enumerations+Platform]::None
}

$store = [sb_explorer.ProjectProfileStore]::new()
$roots = [Collections.Generic.List[object]]::new()
foreach ($gameGroup in $catalog | Group-Object Game) {
    $profile = [sb_explorer.ProjectProfile]::new()
    $profile.Name = $gameGroup.Name
    foreach ($entry in $gameGroup.Group) {
        $match = [regex]::Match($entry.File, '(?i)^(.+?\\_bin_(?:gc|ps2|ps3|xe|xb2|xb1|xb|wii|pc))(?:\\|$)')
        if (-not $match.Success) { continue }
        $platform = Convert-Platform $entry.Platform
        if ($platform -eq [sb_explorer.Enumerations+Platform]::None) { continue }
        $folder = $match.Groups[1].Value
        if ([string]::IsNullOrWhiteSpace($profile.GetFolder($platform))) {
            $profile.SetFolder($platform, $folder)
            $roots.Add([pscustomobject]@{ Game=$profile.Name; Platform=$platform; Folder=$folder })
        }
    }
    $store.Profiles.Add($profile)
}

$detector = $asm.GetType('sb_explorer.ProjectConfigurationDetector').GetMethod('Apply', [Reflection.BindingFlags]'NonPublic,Static')
$autoErrors = [Collections.Generic.List[string]]::new()
foreach ($rootEntry in $roots) {
    try {
        $config = [sb_explorer.AppConfig]::new()
        $config.ProjectFolder = $rootEntry.Folder
        $detector.Invoke($null, [object[]]@($config, $store))
        if ($config.PlatformSelected -ne $rootEntry.Platform) {
            $autoErrors.Add("$($rootEntry.Game)|$($rootEntry.Folder)|expected $($rootEntry.Platform), got $($config.PlatformSelected)")
        }
        if ($config.FileVersion -le 0) {
            $autoErrors.Add("$($rootEntry.Game)|$($rootEntry.Folder)|format version not detected")
        }
    } catch {
        $autoErrors.Add("$($rootEntry.Game)|$($rootEntry.Folder)|$($_.Exception.GetBaseException().Message)")
    }
}
Write-Output "AUTOSELECT|Roots=$($roots.Count)|Passed=$($roots.Count-$autoErrors.Count)|Errors=$($autoErrors.Count)"
$autoErrors | ForEach-Object { Write-Output "AUTO_ERROR|$_" }
