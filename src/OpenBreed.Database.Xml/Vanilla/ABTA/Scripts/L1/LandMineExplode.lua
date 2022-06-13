local mineEntity, actorEntity = ...

Logging:Info("MineEntityId:" .. tostring(mineEntity.Id))
Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

local stampId = Stamps:GetByName("Vanilla/L1/MineCrater").Id;

mineEntity:PutStamp(stampId, 0)

local soundId = Sounds:GetByName("Vanilla/Common/LandMine/Explosion")
local duration = Sounds:GetDuration(soundId)
mineEntity:EmitSound(soundId)	

mineEntity:Destroy()
Logging:Info("Boom!")



