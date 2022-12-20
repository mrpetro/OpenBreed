local function Open(doorEntity, actorEntity)

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

        local clipName = metaData.Level .. "/" .. metaData.Name .. "/Opening/" .. flavor
        local stampName = metaData.Level .. "/" .. metaData.Name .. "/" .. flavor .. "/Opened"
        local soundName = "Vanilla/Common/" .. metaData.Name .. "/Opening"

        local clipId = Clips:GetByName(clipName).Id
        local stampId = Stamps:GetByName(stampName).Id
        local soundId = Sounds:GetByName(soundName)

        doorCell:SetSpriteOn()
        doorCell:PlayAnimation(0, clipId)
        doorCell:PutStamp(stampId, 0)
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

    RemoveHorizontalDoorObstacle = function(doorCell)
        Logging:Info("Removing Horizontal Door Obstacle...")

        doorCell:SetSpriteOff()
        doorCell:SetBodyOffEx()

        local doorCellMeta = doorCell:GetMetadata()
        doorCellMeta.State = "Opened"

        local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 1, 0)
   
        nextDoorCell:SetBodyOffEx()

        local nextDoorCellMeta = nextDoorCell:GetMetadata()
        nextDoorCellMeta.State = "Opened"

    end

    RemoveVerticalDoorObstacle = function(doorCell)
        Logging:Info("Removing Vertical Door Obstacle...")

        doorCell:SetSpriteOff()
        doorCell:SetBodyOffEx()

        local doorCellMeta = doorCell:GetMetadata()
        doorCellMeta.State = "Opened"

        local nextDoorCell = doorCell:GetEntityByDataGrid(Worlds, 0, 1) 
        nextDoorCell:SetBodyOffEx()

        local nextDoorCellMeta = nextDoorCell:GetMetadata()
        nextDoorCellMeta.State = "Opened"

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

    --local doorState = doorEntity.GetState("Door");

    --doorEntity.AddStateImpulse("Door", "Open");


    --Triggers:OnStateChanged(
    --    doorEntity,
    --    FadeOut,
    --    false)




    --local stampId = Stamps:GetByName("Vanilla/L1/MineCrater").Id;

    --mineEntity:PutStamp(stampId, 0)

    --local soundId = Sounds:GetByName("Vanilla/Common/LandMine/Explosion")
    --local duration = Sounds:GetDuration(soundId)
    --mineEntity:EmitSound(soundId)	

	--Worlds:RequestRemoveEntity(mineEntity)
    --Entities:RequestDestroy(mineEntity)
    Logging:Info("Door Open!")

end

return {
    systemHooks = {
        ScriptRunTrigger = Open
    }
}










