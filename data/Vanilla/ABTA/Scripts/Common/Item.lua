﻿local pickups =
{
  [ItemTypes.Ammo] = {
      Name = "Ammo"
  },
  [ItemTypes.CreditsSmall] = {
      Name = "CreditsSmall"
  },
  [ItemTypes.CreditsBig] = {
      Name = "CreditsBig"
  },
  [ItemTypes.MedkitSmall] = {
      Name = "MedkitSmall"
  },
  [ItemTypes.MedkitBig] = {
      Name = "MedkitBig"
  },
  [ItemTypes.KeycardStandard] = {
      Name = "KeycardStandard"
  }
}

local function GiveItem(itemId, actorEntry)

    local item = pickups[itemId]

    if(not(item))
    then
        return
    end

    Logging:Info("Picked up '" .. item.Name .. "'")

end

local function GiveItem(itemId, actorEntry)

    local item = pickups[itemId]

    if(not(item))
    then
        return
    end

    Logging:Info("Picked up '" .. item.Name .. "'")

end

local function Pickup(itemEntity, actorEntity, projection)

	local mapEntity = Entities:GetMapEntity(itemEntity.WorldId)
    local metaData = itemEntity:GetMetadata()

    Logging:Info("ItemEntityId:" .. tostring(itemEntity.Id))
    Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

    local itemName


    if(metaData.Option == nil)
    then
        itemName = tostring(metaData.Name)
    else
        itemName = tostring(metaData.Name) .. tostring(metaData.Option)
    end

    local itemId = Items:GetItemId(itemName)

    --Unknown item
    if (itemId == -1)
    then
        Logging:Warning("Unknown item:" .. itemName)
        return
    end


    GiveItem(itemId, actorEntity)
    --actorEntity:GiveItem(itemId, 1)
	

    --Logging:Info("Picked up '" .. itemName .. "'")

    if (metaData.Flavor ~= "Trigger")
    then
        local stampName = nil

        if (metaData.Flavor == nil)
        then
            stampName = tostring(metaData.Level) .. "/" .. tostring(metaData.Name) .. "/Picked"
        else
            stampName = tostring(metaData.Level) .. "/" .. tostring(metaData.Name) .. "/" .. tostring(metaData.Flavor) .. "/Picked"
        end

        Logging:Info("Stamp name: " .. stampName)

        local stampId = Stamps:GetByName(stampName).Id

        mapEntity:PutStampAtEntityPosition(itemEntity, stampId, 0)

        local soundName = "Vanilla/Common/" .. metaData.Name .. "/Picked"
        local soundId = Sounds:GetByName(soundName)
        itemEntity:EmitSound(soundId)

    end

    Worlds:RequestRemoveEntity(itemEntity)
    Entities:RequestErase(itemEntity)
end

return {
    systemHooks = {
        OnCollision = Pickup
    }
}



