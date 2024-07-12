
# Introduction

First *Alien Breed* game was released on Amiga and PC DOS platforms in 1991 by *Team17*. In next year, special edition of this game appeared only for Amiga.
In 1993 also only for Amiga, a sequel *Horror continues* was created. And 1994 was the year when both Amiga and PC DOS got the last classic Alien Breed game called *Tower Assault*.

Tower Assault for PC DOS uses EPF archive (due to porting being done by different company - *East Point Software*) as container for resources. Writing an EPF archive manager opened a possibility of extracting or modifying game resources like sounds, maps, tiles and sprites to run the game with changed content.
Data format of some resource types appear to be similar between both Amiga/PC platforms and all three games (excluding 1991 version of AB) probably due to usage of same game engine with slight modifications.

# About this project

It consists from two subprojects closely related to each other.

- Game - An open source reimplementation of classic Alien Breed 2D games:
  - *Alien Breed (Special Edition 92)* - (ABSE)
  - *Alien Breed II: Horror continues* - (ABHC)
  - *Alien Breed: Tower Assault* - (ABTA)

- Editor - Used to help out with game development as well as a tool to preview original game assets and perform basic game map editing.

## Is this playable? 
 **Not yet** - Specific game logic is not implemented yet.
 
Development progress can be followed here:

[Open Breed Sandbox Dev Demos](https://www.youtube.com/playlist?list=PLJkKyk7ZrnWJGoFTAFFN0nItfKI7cyHDO)

## Project status

Development is slooooooowly moving forward.
Constantly refactoring things.
Currently expanding Open Breed Editor.

## Project wiki
[Over here](https://github.com/mrpetro/OpenBreed/wiki) - Not updating it that much... Sorry...

Highlights:
* [Tech demos](https://github.com/mrpetro/OpenBreed/wiki/Tech-demos)
* [Engine architecture](https://github.com/mrpetro/OpenBreed/wiki/Engine-architecture)
* [Asset Analysis](https://github.com/mrpetro/OpenBreed/wiki/Assets-analysis)
* [Road Map](https://github.com/mrpetro/OpenBreed/wiki/Road-map)

## Obtaining original game resources
 
Best way to obtain resources for ABSE is to get them from Amiga version. This would require some decrunching of ATN! files.
As for ABHC, luckly the game was added as a bonus to ABTA CD32 release and this verion has standard file system on CD.  This would also require some ATN! decrunching.
ABTA PC port uses exactly same resources as Amiga version but the container is EPF archive, which can be handled.

## Game status

### What is ready? 

* MAP reader for ABTA (it will probably in all three titles since this format didn't changed that much)
* SPR(spriteset) reader for ABTA
* BLK(tileset) reader for ABTA
* LBM (IFF images) reader for ABTA
* ACBM tileSet reader for ABHC and ABSE
* ACBM image reader for some images from ABHC and ABSE

### What is in current development? 
* Game engine in entity component system (ECS) architecture
* Scripting system
* (On hiatus) Developing of OpenBreed.Editor - at least to state of reading & viewing most of games resources.
* (On hiatus) Design & creation of game databases - these will be useful when developing OpenBreed engine. 

### What is to do? 

Those three games have common problem related with resources. Most of graphic/sound/sprite/tiles are stored as asset files delivered with game executables. But... there are still alot of data which is "hardcoded" in executables.
Examples:
* There is sound file with all game sounds available for ABHC game. But the sound file does not seem to have start and length of each sound sample in that file. It means that the information must be somewhere in the executable
* There are files with sprites for both ABHC and ABSE games. But those file are just plain bitmap pictures with "gallery" of sprites. There are no positions and sizes of sprites in same file. It also means that this information must be hardcoded in executables.
* Each map file has number of cells representing map layout. Each cell consists from two numbers. First one is graphical tile number which is used for displaying map graphics. Second number is code of "action" that is assigned to that cell. Examples are spawn position of monster or player, level exit, obstacle, door, etc... Each game executable know exactly what action needs to be executed for particular cell code (and those actions are also hardcoded, doh...). Some codes are obvious to deduct, other not.

So the goal is to trace or rebuild data hardcoded in game executables and store it in dedicated databases, so future game engine could pickup this information along with usual game assets. And that is why OpenBreed.Editor is being developed first.

## Editor status

### What is ready?

**ABTA:**
 - Viewing maps with their original color palettes, tiles and properties
 - Maps edition and storage back to EPF archive (unstable)
 - Viewing tile sets (\*.BLK entries) associated with maps
 - Viewing sprite sets (\*.SPR entries) associated with maps (reverted)
 - Viewing images (\*.LBM entries) from EPF archive 
 - Playing game sounds
 - Level password generator (works for PC and Amiga platforms)

**ABHC:**
 - Viewing maps with their original color palettes, tiles and properties
 - Edition and sorage of maps
 - Viewing tile sets associated with maps
 - Viewing some images used in game with original palette
 
**ABSE**
 - Viewing maps with their color palettes tiles and properties
 - Edition and sorage of maps
 - Viewing tile sets associated with maps
 - Viewing some images used in game with original palette

### What is in current development? 

* Adding functionality that will help out with creating game entites.
* Expanding existing functionality 

### What is to do? 

* Always something...

## Associated repositories
* **EPFArchive** - contains EPF files manager useful when handling data from PC port of ABTA.

## Development stuff

### Source code languages
* **C#**
* **Lua** (ingame scripting)

### Platform
**.NET 6.0** for now

### DevEnv
**MSVS 2022** (Community or compatible)

### Design architecture
**Entity–Component–System(ECS)** for now

### Libraries
* **WPF** - User interface for the editor 
* **NLua** - handling entity creations and their logic trough scripting. Visit http://nlua.org/ for more information.
* **OpenTK** - handling graphics, inputs and sounds. Visit https://opentk.net for more information.
