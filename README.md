# OpenBreed
Open source reimplementation of classic Alien Breed 2D games:
 - *Alien Breed (Special Edition 92)* - (ABSE)
 - *Alien Breed II: Horror continues* - (ABHC)
 - *Alien Breed: Tower Assault* - (ABTA)
 
## Project wiki
[Over here](https://github.com/mrpetro/OpenBreed/wiki)
 
## Obtaining game resources
 
Best way to obtain resources for ABSE is to get them from Amiga port. This would require some decrunching of ATN! files.
As for ABHC, luckly the game was added as a bonus to ABTA CD32 release and this verion has standard file system on CD.  This would also require some ATN! decrunching.
ABTA PC port uses exactly same resources as Amiga version but the container is EPF archive, which can be handled.

## What is ready

* MAP reader for ABTA (it will probably for for all three titles since this format didn't changed that much)
* SPR(spriteset) reader for ABTA
* BLK(tileset) reader for ABTA
* LBM (IFF images) reader for ABTA
* ACBM tileSet reader for ABHC and ABSE
* ACBM image reader for some images from ABHC and ABSE

## What is in current development
* Game engine in entity component system (ECS) architecture
* Readers for various resources from all three games
* Developing of OpenBreed.Editor - at least to state of reading & viewing most of games resources.
* Design & creation of game databases - these will be useful when developing OpenBreed engine. 
* Some refactoring in OpenBreed.Common & Editor

## What's to do

Those three games have common problem related with resources. Most of graphic/sound/sprite/tiles are stored as asset files delivered with game executables. But... there are still alot of data which is "hardcoded" in executables.
Examples:
* There is sound file with all game sounds available for ABHC game. But the sound file does not seem to have start and length of each sound sample in that file. It means that the information must be somewhere in the executable
* There are files with sprites for both ABHC and ABSE games. But those file are just plain bitmap pictures with "gallery" of sprites. There are no positions and sizes of sprites in same file. It also means that this information must be hardcoded in executables.
* Each map file has number of cells representing map layout. Each cell consists from two numbers. First one is graphical tile number which is used for displaying map graphics. Second number is code of "action" that is assigned to that cell. Examples are spawn position of monster or player, level exit, obstacle, door, etc... Each game executable know exactly what action needs to be executed for particular cell code (and those actions are also hardcoded, doh...). Some codes are obvious to deduct, other not.

So the goal is to trace or rebuild data hardcoded in game executables and store it in dedicated databases, so future game engine could pickup this information along with usual game assets. And that is why OpenBreed.Editor is being developed first.

## Associated repositories
* **OpenBreed.Common** - which should contain information and implementation of various data formats existing in all three games.

* **OpenBreed.Editor** - map editor for games and for OpenBreed implementation.

* **EPFArchive** - contains EPF files manager useful when handling data from PC port of ABTA.

## Development stuff

Source code language: **C#**

Platform: **.NET 4.7.2** for now

DevEnv: **MSVS 2017** (Community or compatible)

Design architecture: **Entity–Component–System(ECS)** for now

Libraries:
**OpenTK** - handling graphics, inputs and sounds. Visit https://opentk.net for more information.
