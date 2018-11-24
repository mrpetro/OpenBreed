# OpenBreed
Open source reimplementation of classic Alien Breed 2D games:
 - *Alien Breed (Special Edition 92)* - (ABSE)
 - *Alien Breed II: Horror continues* - (ABHC)
 - *Alien Breed: Tower Assault* - (ABTA)
 
 **Obtaining game resources**
 
Here is a table of *Alien Breed* games with their ports and information about resources extraction possibility:

+--------+-----------------------------------------------------------------------------+
|        |                                    PORTS                                    |
|  Game  +-----------------------------------------------------------------------------+
|        |        PC DOS           |   Amiga(500/600/1200)   |       Amiga CD32        |
+--------+-------------------------+-------------------------+-------------------------+
|  ABSE  |     Doesn't exist       |      Amiga DOS files    |      Doesn't exist      |
|        |                         |(Some crunched with ATN!)|                         |
+--------+-------------------------+-------------------------+-------------------------+
|  ABHC  |     Doesn't exist       |       No AmigaDOS       |      Amiga DOS files    |
|        |                         |  (Difficult to extract) |(Some crunched with ATN!)|
+--------+-------------------------+-------------------------+-------------------------+
|  ABTA  |     EPF archive         |       No AmigaDOS       |       No AmigaDOS       |
|        |  (Easy to extract)      |  (Difficult to extract) |  (Difficult to extract) |
+--------+-------------------------+-------------------------+-------------------------+


So the table states that to obtain resources for ABSE is to get them from Amiga port. This would require some decrunching of ATN! files.
As for ABHC, luckly the game was added as a bonus to ABTA CD32 release and this verion has standard file system on CD.  This would also require some ATN! decrunching.
ABTA PC port uses exactly same resources as Amiga version but the container is EPF archive, which can be handled.

**What is ready**

MAP reader for ABTA (it will probably for for all three titles since this format didn't changed that much)
SPR(spriteset) reader for ABTA
BLK(tileset) reader for ABTA

**What's to do**

For starters, write some missing readers for the resources from first two titles (ABSE and ABHC) and put them in OpenBreed.Common repository.
