local function Pickup(itemEntity, actorEntity, projection)

    local metaData = itemEntity:GetMetadata()

    Logging:Info("ItemEntityId:" .. tostring(itemEntity.Id))
    Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

    local itemId = Items:GetItemId("Ammo")

    actorEntity:GiveItem(itemId, 1)
	
    Logging:Info("Picked up 'Ammo'.")

	local stampName = tostring(metaData.Level) .. "/Ammo/" .. tostring(metaData.Flavor) .. "/Picked"
	local soundName = "Vanilla/Common/Ammo/Picked"
	
	local stampId = Stamps:GetByName(stampName).Id
	local soundId = Sounds:GetByName(soundName)
	
	itemEntity:PutStamp(stampId, 0)
	itemEntity:EmitSound(soundId)

    Worlds:RequestRemoveEntity(itemEntity)
    Entities:RequestErase(itemEntity)
end

return {
    systemHooks = {
        OnCollision = Pickup
    }
}



