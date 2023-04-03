
local cooldownTimerId
local currentWeaponNo = 1
local flamethrowerOffsetIndex = 0
local fireReady = true
local fireCooldownTime = 1000

local speedFactor = 30

local flamethrowerOffsets =
{
    0,
    1,
    2,
    1,
    0,
    -1,
    -2,
    -1
}


local weapons =
{
  --[1] = "AssaultGun",
  [1] = {
      Name = "AssaultGun",
      Projectile = "AssaultGun",
      FireRate = 40,
      MuzzleFlash = "AssaultGun",
      Speed = 12 * speedFactor
  },
  [2] = {
      Name = "MissileLauncher",
      Projectile = "Missile",
      FireRate = 1,
      MuzzleFlash = "",
      Speed = 10 * speedFactor
  },
  [3] = {
      Name = "TrilazerGun",
      Projectile = "TrilazerGun",
      FireRate = 7,
      MuzzleFlash = "",
      Speed = 12 * speedFactor
  },
  [4] = {
      Name = "Flamethrower",
      Projectile = "Firewall",
      FireRate = 25,
      MuzzleFlash = "",
      Speed = 10 * speedFactor
  },
  [5] = {
      Name = "RefractionGun",
      Projectile = "RefractionLazer",
      FireRate = 10,
      MuzzleFlash = "",
      Speed = 8 * speedFactor
  }
}

local function CooldownFinish()
    fireReady = true
end

local function FireBullet(entity)

    if(not(fireReady))
    then
        return
    end

    local currentWeapon = weapons[currentWeaponNo]

    if(not(currentWeapon))
    then
        return
    end
     
    local pos = entity:GetPosition()
    local dir = MovementTools.SnapToCompass8Way(entity:GetDirection())

    pos = pos + dir * 16
    local thrust = dir * currentWeapon.Speed

    local emitter = entity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\" ..  currentWeapon.Projectile)
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)

    if(currentWeapon.Name == "TrilazerGun")
    then
        emitter:SetOption("thrustX", thrust.X)
            :SetOption("thrustY", thrust.Y)
            :Finish()

        local perp = Vector2(dir.Y, -dir.X)

        local p1Thrust = thrust + perp * 2 * speedFactor

        emitter:SetOption("thrustX", p1Thrust.X)
            :SetOption("thrustY", p1Thrust.Y)
            :Finish()

        local p2Thrust = thrust - perp * 2 * speedFactor

        emitter:SetOption("thrustX", p2Thrust.X)
            :SetOption("thrustY", p2Thrust.Y)
            :Finish()
    elseif(currentWeapon.Name == "Flamethrower")
    then

        flamethrowerOffsetIndex = flamethrowerOffsetIndex + 1

        if(flamethrowerOffsetIndex > 8)
        then
            flamethrowerOffsetIndex = 1
        end

        local flamethrowerOffset = flamethrowerOffsets[flamethrowerOffsetIndex]

        local perp = Vector2(thrust.Y, -thrust.X)
        perp:Normalize()

        thrust = thrust + perp * flamethrowerOffset * speedFactor

        emitter:SetOption("thrustX", thrust.X)
            :SetOption("thrustY", thrust.Y)
            :Finish()
    else

        emitter:SetOption("thrustX", thrust.X)
            :SetOption("thrustY", thrust.Y)
            :Finish()
    end

    fireReady = false

	Triggers:AfterDelay(entity, cooldownTimerId, TimeSpan.FromMilliseconds(1000 / currentWeapon.FireRate), CooldownFinish)
end

local function SwitchToPreviousWeapon(entity)
    currentWeaponNo = currentWeaponNo - 1

    if(currentWeaponNo < 1)
    then
        currentWeaponNo = 5
    end

    local currentWeapon = weapons[currentWeaponNo]
    Logging:Info("Switching weapon to: " .. currentWeapon.Name)
end

local function SwitchToNextWeapon(entity)
    currentWeaponNo = currentWeaponNo + 1

    if(currentWeaponNo > 5)
    then
        currentWeaponNo = 1
    end

    local currentWeapon = weapons[currentWeaponNo]
    Logging:Info("Switching weapon to: " .. currentWeapon.Name)
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

local function OnInit(entity)

    cooldownTimerId = entity:GetTimerId("CooldownDelay")

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
        OnInit = OnInit
    }
}








