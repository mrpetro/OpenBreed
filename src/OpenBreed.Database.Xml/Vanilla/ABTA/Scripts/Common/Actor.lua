
local cooldownTimerId
local currentWeaponNo = 1
local fireReady = true
local fireCooldownTime = 1000

local weapons =
{
  --[1] = "AssaultGun",
  [1] = {
      Name = "AssaultGun",
      Projectile = "AssaultGun",
      FireRate = 100,
      MuzzleFlash = "",
      Speed = 500
  },
  [2] = {
      Name = "MissileLauncher",
      Projectile = "Missile",
      FireRate = 1,
      MuzzleFlash = "",
      Speed = 500
  },
  [3] = {
      Name = "TrilazerGun",
      Projectile = "TrilazerGun",
      FireRate = 7,
      MuzzleFlash = "",
      Speed = 500
  },
  [4] = {
      Name = "Flamethrower",
      Projectile = "Firewall",
      FireRate = 25,
      MuzzleFlash = "",
      Speed = 500
  },
  [5] = {
      Name = "RefractionGun",
      Projectile = "RefractionLazer",
      FireRate = 10,
      MuzzleFlash = "",
      Speed = 200
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
    local dir = entity:GetDirection() * currentWeapon.Speed


    local emitter = entity:StartEmit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\" ..  currentWeapon.Projectile .. ".xml")
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)

    if(currentWeapon.Name == "TrilazerGun")
    then
        emitter:SetOption("thrustX", dir.X)
            :SetOption("thrustY", dir.Y)
            :Finish()

        local perp = Vector2(dir.Y, -dir.X)
        perp:Normalize()

        local p1Dir = dir + perp * 30

        emitter:SetOption("thrustX", p1Dir.X)
            :SetOption("thrustY", p1Dir.Y)
            :Finish()

        local p2Dir = dir - perp * 30

        emitter:SetOption("thrustX", p2Dir.X)
            :SetOption("thrustY", p2Dir.Y)
            :Finish()
    else
        emitter:SetOption("thrustX", dir.X)
            :SetOption("thrustY", dir.Y)
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








