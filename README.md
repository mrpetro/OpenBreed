# OpenBreed
Open source reimplementation of classic Alien Breed 2D games:
 - *Alien Breed (Special Edition 92)* - (ABSE)
 - *Alien Breed II: Horror continues* - (ABHC)
 - *Alien Breed: Tower Assault* - (ABTA)
 
 **Obtaining game resources**
 
Best way to obtain resources for ABSE is to get them from Amiga port. This would require some decrunching of ATN! files.
As for ABHC, luckly the game was added as a bonus to ABTA CD32 release and this verion has standard file system on CD.  This would also require some ATN! decrunching.
ABTA PC port uses exactly same resources as Amiga version but the container is EPF archive, which can be handled.

**What is ready**

MAP reader for ABTA (it will probably for for all three titles since this format didn't changed that much)
SPR(spriteset) reader for ABTA
BLK(tileset) reader for ABTA

**What's to do**

For starters, write some missing readers for the resources from first two titles (ABSE and ABHC) and put them in OpenBreed.Common repository.

**Development platform**
Source code language: C#
Platform: .NET 4.6.1 for now
DevEnv: MSVS 2017 (Community or compatible)
Libraries: CS-SLD for graphics probably

