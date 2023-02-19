
local currentWeaponNo = 1
local weapons =
{
  --[1] = "AssaultGun",
  [1] = "Missile",
  [2] = "TrilazerGun",
  [3] = "RefractionLazer",
  --[5] = "Firewall",
}


local function FireBullet(entity)

    local currentWeaponName = weapons[currentWeaponNo]

    if(currentWeaponName)
    then
        entity:Emit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\" ..  currentWeaponName .. ".xml")
    end
end

local function SwitchToPreviousWeapon(entity)
    currentWeaponNo = currentWeaponNo - 1

    if(currentWeaponNo < 1)
    then
        currentWeaponNo = 3
    end

    local currentWeaponName = weapons[currentWeaponNo]
    Logging:Info("Switching weapon to: " .. currentWeaponName)
end

local function SwitchToNextWeapon(entity)
    currentWeaponNo = currentWeaponNo + 1

    if(currentWeaponNo > 3)
    then
        currentWeaponNo = 1
    end

    local currentWeaponName = weapons[currentWeaponNo]
    Logging:Info("Switching weapon to: " .. currentWeaponName)
end

local actions =
{
  [GameActions.Fire] = FireBullet,
  [GameActions.PreviousWeapon] = SwitchToPreviousWeapon,
  [GameActions.NextWeapon] = SwitchToNextWeapon,
}

local function CheckAction(entity, args)  
    local func = actions[args.ActionType]
    if(func) then
        func(entity)
    else
        Logging:Error("Missing implementation for action: " .. args.ActionType)
    end
end

local function OnDirectionChanged(entity, args)
    local targetDirection = entity:GetTargetDirection()
    local direction = entity:GetDirection()
    local metadata = entity:GetMetadata()
    local className = metadata.Name
    local animDirName = AnimHelper.ToDirectionName(direction)

    local isMoving = entity:IsMoving()
    local movementStateName = nil

    if(isMoving)
    then
        movementStateName = "Walking"
    else
        movementStateName = "Standing"
    end
        
    local clip = Clips:GetByName("Vanilla/Common/" .. className .. "/" .. movementStateName .. "/" .. animDirName)

    entity:PlayAnimation(0, clip.Id)
end

local function OnVelocityChanged(entity, args)
    local targetDirection = entity:GetTargetDirection()
    local direction = entity:GetDirection()
    local metadata = entity:GetMetadata()
    local className = metadata.Name
    local animDirName = AnimHelper.ToDirectionName(direction)

    local isMoving = entity:IsMoving()
    local movementStateName = nil

    if(isMoving)
    then
        movementStateName = "Walking"
        local clip = Clips:GetByName("Vanilla/Common/" .. className .. "/" .. movementStateName .. "/" .. animDirName)
        entity:PlayAnimation(0, clip.Id)
    else
        movementStateName = "Standing"

        local clip = Clips:GetByName("Vanilla/Common/" .. className .. "/" .. movementStateName .. "/" .. animDirName)

        entity:StopAnimation(0)
    end

end

local function ShowMission(actorEntity)

    local gameWorld = Worlds:GetWorld(actorEntity)
    local missionEntity = Entities:GetMission(gameWorld.Id)
    missionEntity:TryInvoke(Scripting, Logging, "OnShow", actorEntity)
end

local function onInit(entity)
    Triggers:OnEntityAction(
        entity,
        CheckAction,
        false)

    Triggers:OnEntityDirectionChanged(
        entity,
        OnDirectionChanged,
        false)

    Triggers:OnEntityVelocityChanged(
        entity,
        OnVelocityChanged,
        false)
end

return {
    systemHooks = {
        OnEnter = ShowMission,
        onInit = onInit
    }
}








