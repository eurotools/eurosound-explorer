# EuroSound Explorer

EuroSound Explorer is a Windows tool for inspecting and playing Eurocom `.sfx` audio files across multiple games, platforms, and MusX file versions.

The tool can browse project folders, identify supported MusX files, display their internal data, resolve hashcodes through `Sound.h`, and inspect raw file structures through the Data Viewer.

## Supported MusX Data

- Soundbanks
- Streambanks
- Music banks
- Soundbank info files
- Sound details files
- Music details files
- Project details files
- Music markers files

Support varies by game, platform, and MusX version because Eurocom changed several file layouts between titles.

## Supported Games

| Game | MusX version | Platforms |
| --- | --- | --- |
| Buffy the Vampire Slayer: Chaos Bleeds | 201 | GameCube, PlayStation 2, Xbox |
| Sphinx and the Cursed Mummy | 201 | GameCube, PlayStation 2, PC, Xbox |
| Athens 2004 | 1 | PlayStation 2 |
| Spyro: A Hero's Tail | 4 | GameCube, PlayStation 2, Xbox |
| Robots | 5 | GameCube, PlayStation 2, PC, Xbox |
| Predator: Concrete Jungle | 5 | PlayStation 2, Xbox |
| Batman Begins | 6 | GameCube, PlayStation 2, Xbox |
| Ice Age 2: The Meltdown | 6 | PlayStation 2, PC, Xbox, Wii |

## Basic Usage

1. Select the target game, platform, project folder, and `Sound.h` file in Settings.
2. Use the Files panel to browse the `.sfx` files in the selected project folder.
3. Load a supported file to inspect its decoded data in the matching panel.
4. Use Data Viewer for a lower-level view of headers, sections, counts, markers, and details data.

## Settings

EuroSound Explorer stores its settings beside the executable in the local `ESEx` folder:

- `General Settings.ini`
- `Dock Settings.xml`
- one `.ini` file per form for saved list-view layout

## Notes

- Hashcode labels are resolved from the configured `Sound.h` file.
- Some MusX version 6 details files use layouts that differ from earlier versions.
- Music details in MusX version 6 display hashcodes using the `Sound.h` music prefix so labels can be resolved correctly.
