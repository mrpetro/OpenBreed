local itemEntity, actorEntity = ...

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

actorEntity:GiveItem(itemId, 1)
Logging:Info("Picked up '" .. metaData.Name .. "'");

if (metaData.Flavor ~= "Trigger")
then
    local stampId = -1

    if (metaData.Flavor == nil)
    then
        stampId = Stamps:GetByName(tostring(metaData.Level) .. "/" .. tostring(metaData.Name) .. "/Picked").Id
    else
        stampId = Stamps:GetByName(tostring(metaData.Level) .. "/" .. tostring(metaData.Name) .. "/" .. tostring(metaData.Flavor) .. "/Picked").Id
    end

    itemEntity:PutStamp(stampId, 0)

    local soundName = "Vanilla/Common/" .. metaData.Name .. "/Picked"
    local soundId = Sounds:GetByName(soundName)
    itemEntity:EmitSound(soundId)

end

itemEntity:Destroy()



