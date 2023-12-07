
local function RemoveHorizontalDoorObstacle(doorCell)
    Logging:Info("Removing Horizontal Door Obstacle...")

    doorCell:SetSpriteOff()
    doorCell:SetBodyOffEx()

    local doorCellMeta = doorCell:GetMetadata()
    doorCellMeta.State = "Opened"

    local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 1, 0)
   
    nextDoorCell:SetBodyOffEx()

    local nextDoorCellMeta = nextDoorCell:GetMetadata()
    nextDoorCellMeta.State = "Opened"

    Entities:RequestErase(doorCell)
    Entities:RequestErase(nextDoorCell)

end

local function RemoveVerticalDoorObstacle(doorCell)
    Logging:Info("Removing Vertical Door Obstacle...")

    doorCell:SetSpriteOff()
    doorCell:SetBodyOffEx()

    local doorCellMeta = doorCell:GetMetadata()
    doorCellMeta.State = "Opened"

    local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 0, 1) 
    nextDoorCell:SetBodyOffEx()

    local nextDoorCellMeta = nextDoorCell:GetMetadata()
    nextDoorCellMeta.State = "Opened"

    Entities:RequestErase(doorCell)
    Entities:RequestErase(nextDoorCell)

end



local function Open(doorEntity, actorEntity, projection)

    local metaData = doorEntity:GetMetadata()

    if (metaData.State ~= nil )
    then
        return
    end

    --functions
    local OpenDoor
    local RemoveDoorObstacle

    -- Functions

    OpenDoor = function(doorCell, nextDoorCell, flavor)
        Logging:Info("Opening " .. flavor .. " Door...")

		local mapEntity = Entities:GetMapEntity(doorCell.WorldId)

        local clipName = metaData.Level .. "/" .. metaData.Name .. "/Opening/" .. flavor
        local stampName = metaData.Level .. "/" .. metaData.Name .. "/" .. flavor .. "/Opened"
        local soundName = "Vanilla/Common/" .. metaData.Name .. "/Opening"

        local clipId = Clips:GetByName(clipName).Id
        local stampId = Stamps:GetByName(stampName).Id
        local soundId = Sounds:GetByName(soundName)

        doorCell:SetSpriteOn()
        doorCell:PlayAnimation(0, clipId)
        mapEntity:PutStampAtEntityPosition(doorCell, stampId, 0)
        doorCell:EmitSound(soundId)

        if(flavor == "Horizontal")
        then
            Triggers:OnEntityAnimFinished(
                doorCell,
                RemoveHorizontalDoorObstacle,
                true)
        else
            Triggers:OnEntityAnimFinished(
                doorCell,
                RemoveVerticalDoorObstacle,
                true)
        end

        local doorCellMeta = doorCell:GetMetadata()
        doorCellMeta.State = "Opening"

        local nextDoorCellMeta = nextDoorCell:GetMetadata()
        nextDoorCellMeta.State = "Opening"
    end

    Logging:Info("DoorEntityId:" .. tostring(doorEntity.Id))
    Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

    if (metaData.Option ~= nil)
    then
        local keycardItemId = Items:GetItemId(metaData.Option)

        if (keycardItemId ~= -1)
        then
            Logging:Info("KeyCard " .. tostring(metaData.Option) .. " required.")

            local actorInventory = actorEntity:GetInventory()

            local itemSlot = actorInventory:GetItemSlot(keycardItemId)

            --No keycard item then door can't be opened
            if (itemSlot == nil)
            then
                Logging:Info("No KeyCard!")
                return
            end
        end
    end

    local doorCell = doorEntity:FindHorizontalDoorCell(Worlds)

    if(doorCell ~= doorEntity)
    then
        --Logging:Info("Open from right!")
        local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 1, 0)
        OpenDoor(doorCell, nextDoorCell, "Horizontal")
    else
        if(doorCell:IsSameCellType(Worlds, 1, 0))
        then
            --Logging:Info("Open from left!")
            local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 1, 0)
            OpenDoor(doorCell, nextDoorCell, "Horizontal")
        else
            -- Single cell horizontally, might be vertical Door
            doorCell = doorEntity:FindVerticalDoorCell(Worlds)
            local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 0, 1)
            OpenDoor(doorCell, nextDoorCell, "Vertical")
        end
    end

    Logging:Info("Door Open!")
end

local function DestroyDoor(doorCell, nextDoorCell, flavor)
    Logging:Info("Destroying " .. flavor .. " Door...")

	local mapEntity = Entities:GetMapEntity(doorCell.WorldId)
    local metaData = doorCell:GetMetadata()
    local stampName = metaData.Level .. "/" .. metaData.Name .. "/" .. flavor .. "/Opened"
    local stampId = Stamps:GetByName(stampName).Id

    mapEntity:PutStampAtEntityPosition(doorCell, stampId, 0)

    if(flavor == "Horizontal")
    then
        RemoveHorizontalDoorObstacle(doorCell)
    else
        RemoveVerticalDoorObstacle(doorCell)
    end

    local doorCellMeta = doorCell:GetMetadata()
    doorCellMeta.State = "Destroying"

    local nextDoorCellMeta = nextDoorCell:GetMetadata()
    nextDoorCellMeta.State = "Destroying"


    local c1 = doorCell:GetAabb().Center
    local c2 = nextDoorCell:GetAabb().Center

    local midPos = (c1 + c2) / 2.0
    Logging:Info(tostring(c1))
    Logging:Info(tostring(c2))
    Logging:Info(tostring(midPos))

    doorCell:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
        :SetOption("flavor", "Big")
        :SetOption("startX", midPos.X)
        :SetOption("startY", midPos.Y)
        :Finish()
end


local function Erase(doorEntity)
   
    local doorCell = doorEntity:FindHorizontalDoorCell(Worlds)

    if(doorCell ~= doorEntity)
    then
        --Logging:Info("Open from right!")
        local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 1, 0)
        DestroyDoor(doorCell, nextDoorCell, "Horizontal")
    else
        if(doorCell:IsSameCellType(Worlds, 1, 0))
        then
            --Logging:Info("Open from left!")
            local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 1, 0)
            DestroyDoor(doorCell, nextDoorCell, "Horizontal")
        else
            -- Single cell horizontally, might be vertical Door
            doorCell = doorEntity:FindVerticalDoorCell(Worlds)
            local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 0, 1)
            DestroyDoor(doorCell, nextDoorCell, "Vertical")
        end
    end

    Logging:Info("Door Destroyed!")
end

local function OnInit(entity)

    Triggers:OnEntityLeavingWorld(
        entity,
        Erase,
        true)

end

return {
    systemHooks = {
        OnInit = OnInit,
        OnCollision = Open
    }
}










