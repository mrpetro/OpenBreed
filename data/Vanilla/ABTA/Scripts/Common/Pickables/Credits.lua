local function Pickup(itemEntity, actorEntity, projection)

	local mapEntity = Entities:GetMapEntity(itemEntity.WorldId)
    local metaData = itemEntity:GetMetadata()

    Logging:Info("ItemEntityId:" .. tostring(itemEntity.Id))
    Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

    local itemId = Items:GetItemId("Credits")


    local creditsValue = 0
	
	if (metaData.Name == "CreditsBig")
	then
		creditsValue = 1000	
    elseif (metaData.Name == "CreditsSmall")
	then
		creditsValue = 100
    end

    actorEntity:GiveItem(itemId, creditsValue)
	
    Logging:Info("Picked up '" .. tostring(creditsValue) .. " Credits'.")

	local stampName = tostring(metaData.Level) .. "/" .. metaData.Name .. "/" .. tostring(metaData.Flavor) .. "/Picked"
	local soundName = "Vanilla/Common/" .. metaData.Name .. "/Picked"
	
	local stampId = Stamps:GetByName(stampName).Id
	local soundId = Sounds:GetByName(soundName)
	
	mapEntity:PutStampAtEntityPosition(itemEntity, stampId, 0)
	itemEntity:EmitSound(soundId)

    Worlds:RequestRemoveEntity(itemEntity)
    Entities:RequestErase(itemEntity)
end

return {
    systemHooks = {
        OnCollision = Pickup
    }
}



