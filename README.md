# OpenBreed.Common
Common code and resources for open source implementation of classic Alien Breed 2D games and map editor:
 - *Alien Breed (Special Edition 92)* - (ABSE)
 - *Alien Breed II: Horror continues* - (ABHC)
 - *Alien Breed: Tower Assault* - (ABTA)
 
 ## File types

### Map files
 
For ABSE & ABHC - LXMA files (like L1MA, LAMA, L3MA)

For ABTA - \*.MAP files ()

These files contains tile map layout, each one with property(like spawn point of monster, walls, keys, etc...). It also contains two palette sets for usage with right input tile set. There are some other data that purpose is questionable. maps file format is shared between all three AB titles. But... ABTA contains one additional MISS(Mission?) data block which describes message texts, entrances/exists from map, monster speeds, strengths and some other unknown data.


### Tile set files

For ABSE & ABHC - LXBM files (like L1BM, L2BM, L3BM)

For ABTA - \*.BLK files

These files consist from tile graphics. Each tile is 16x16 pixels. ABSE & ABHC tiles are stored as bit planes (Amiga ACBM). ABTA \*.BLK files are using some different format.
 
 ### Sprite set files

For ABSE & ABHC - LXBO files (like L1BO, L2BO, LEBO)

For ABTA - \*.SPR files

Each sprite set file contains collection of sprites representing various character and object animations. Sprites can have different sizes. ABSE & ABHC sprite sets are stored as picture (in form of bit planes) and there is not information about each sprite size and number.
ABTA sprite sets have more complex structure, where each sprite is separate.
 
 
 
 ### Sound files

For ABSE & ABHC - ???
For ABTA - 

<TODO>

