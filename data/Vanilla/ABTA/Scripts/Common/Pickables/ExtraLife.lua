local function Pickup(itemEntity, actorEntity, projection)

    local metaData = itemEntity:GetMetadata()

    Logging:Info("ItemEntityId:" .. tostring(itemEntity.Id))
    Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))
	
	actorEntity:AddLives(1)
	
    Logging:Info("Picked up 'ExtraLife'.")

	local stampName = tostring(metaData.Level) .. "/ExtraLife/" .. tostring(metaData.Flavor) .. "/Picked"
	local soundName = "Vanilla/Common/ExtraLife/Picked"
	
	local stampId = Stamps:GetByName(stampName).Id
	local soundId = Sounds:GetByName(soundName)
	
	itemEntity:PutStamp(stampId, 0)
	itemEntity:EmitSound(soundId)

    Worlds:RequestRemoveEntity(itemEntity)
    Entities:RequestDestroy(itemEntity)
end

return {
    systemHooks = {
        OnCollision = Pickup
    }
}



