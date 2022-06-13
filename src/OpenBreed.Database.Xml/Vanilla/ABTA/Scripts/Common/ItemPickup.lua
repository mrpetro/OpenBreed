local itemEntity, actorEntity = ...

local metaData = itemEntity:GetMetadata()

Logging:Info("ItemEntityId:" .. tostring(itemEntity.Id))
Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

local stampName = metaData.Level .. "/" .. metaData.Name .. "/" .. metaData.Flavor .. "/Picked"
local soundName = "Vanilla/Common/" .. metaData.Name .. "/Picked"

Logging:Info("StampName:" .. tostring(stampName))
Logging:Info("SoundName:" .. tostring(soundName))

local stampId = Stamps:GetByName(stampName).Id;
itemEntity:PutStamp(stampId, 0)

local soundId = Sounds:GetByName(soundName)
itemEntity:EmitSound(soundId)	

--actorEntity:GiveItem(itemId, 1);

itemEntity:Destroy()



