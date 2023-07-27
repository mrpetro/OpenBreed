local function Pickup(itemEntity, actorEntity, projection)

    local metaData = itemEntity:GetMetadata()

    Logging:Info("ItemEntityId:" .. tostring(itemEntity.Id))
    Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

    local itemId = Items:GetItemId("KeycardStandard")

    actorEntity:GiveItem(itemId, 1)
	
    Logging:Info("Picked up 'Keycard'.")

	local stampName = tostring(metaData.Level) .. "/KeycardStandard/" .. tostring(metaData.Flavor) .. "/Picked"
	local soundName = "Vanilla/Common/Keycard/Picked"
	
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



