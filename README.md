# OpenBreed
Open source reimplementation of classic Alien Breed 2D games:
 - *Alien Breed (Special Edition 92)* - (ABSE)
 - *Alien Breed II: Horror continues* - (ABHC)
 - *Alien Breed: Tower Assault* - (ABTA)
 
## Obtaining game resources
 
Best way to obtain resources for ABSE is to get them from Amiga port. This would require some decrunching of ATN! files.
As for ABHC, luckly the game was added as a bonus to ABTA CD32 release and this verion has standard file system on CD.  This would also require some ATN! decrunching.
ABTA PC port uses exactly same resources as Amiga version but the container is EPF archive, which can be handled.

## What is ready

MAP reader for ABTA (it will probably for for all three titles since this format didn't changed that much)
SPR(spriteset) reader for ABTA
BLK(tileset) reader for ABTA
ACBM tileSet reader for ABHC and ABSE
ACBM image reader for some images from ABHC and ABSE

## What is in current development

* Readers for various resources from all three games
* Developing of OpenBreed.Editor - at least to state of reading & viewing most of games resources.
* Design & creation of game databases - these will be useful when developing OpenBreed engine. 
* Some refactoring in OpenBreed.Common & Editor

## What's to do

* TBD

## Associated repositories
* **OpenBreed.Common** - which should contain information and implementation of various data formats existing in all three games.

* **OpenBreed.Editor** - map editor for games and for OpenBreed implementation.

* **EPFArchive** - contains EPF files manager useful when handling data from PC port of ABTA.

## Development stuff

Source code language: C#

Platform: .NET 4.6.1 for now

DevEnv: MSVS 2017 (Community or compatible)

Libraries: CS-SLD for graphics probably


