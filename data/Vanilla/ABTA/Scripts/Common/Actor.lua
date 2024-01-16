
local cooldownTimerId
local delayTimerId
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
      FireRate = 25,
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

local function CooldownFinish(entity, args)
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
  [PlayerActions.Fire] = FireBullet,
  [PlayerActions.SwitchWeapon] = SwitchToNextWeapon,
}

local function CheckAction(entity, args)
    local func = actions[args.ActionCode]
    if(func) then
        func(entity)
    else
        Logging:Error("Missing implementation for action: " .. args.ActionCode)
    end
end

local function OnDirectionChanged(entity, args)
    local targetDirection = entity:GetTargetDirection()
    local direction = entity:GetDirection()
    local animDirName = AnimHelper.ToDirectionName(direction)

    local isMoving = entity:IsMoving()
    local movementStateName = nil

    if(isMoving)
    then
        movementStateName = "Walking"
    else
        movementStateName = "Standing"
    end
        
    local clip = Clips:GetByName("Vanilla/Common/Actor/" .. movementStateName .. "/" .. animDirName)

    entity:PlayAnimation(0, clip.Id)
end

local function OnVelocityChanged(entity, args)

    local targetDirection = entity:GetTargetDirection()
    local direction = entity:GetDirection()
    local animDirName = AnimHelper.ToDirectionName(direction)

    local isMoving = entity:IsMoving()
    local movementStateName = nil

    if(isMoving)
    then
        movementStateName = "Walking"
        local clip = Clips:GetByName("Vanilla/Common/Actor/" .. movementStateName .. "/" .. animDirName)
        entity:PlayAnimation(0, clip.Id)
    else
        movementStateName = "Standing"

        local clip = Clips:GetByName("Vanilla/Common/Actor/" .. movementStateName .. "/" .. animDirName)

        entity:StopAnimation(0)
    end

end

local function ShowMission(actorEntity)

    local gameWorld = Worlds:GetWorld(actorEntity)
    local missionEntity = Entities:GetMission(gameWorld.Id)
    missionEntity:TryInvoke(Scripting, Logging, "OnShow", actorEntity)
end

local function Resurrect(entity, args)
	Logging:Info("Resurrect...")
	entity:RestoreFillHealth()
	entity:Resurrect()
	
end

local function Wait3Seconds(entity, args)
	Logging:Info("Wait 3 seconds...")

	Triggers:AfterDelay(
		entity,
		delayTimerId,
		TimeSpan.FromMilliseconds(3000),
		Resurrect,
		true)
end

local function PostEnter(entity, args)
    
	local w = Worlds:GetById(args.WorldId)

    Logging:Info("Entering " .. w.Name)
	
	if(w.Name == "Limbo")
    then
		Wait3Seconds(entity, args)
    else
	
    end
	
end


local function SendToLimbo(entity)
    
    local pos = entity:GetPosition()
    entity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
        :SetOption("flavor", "Small")
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)
        :Finish()
     
    local soundId = Sounds:GetByName("Vanilla/Common/Hero/Dying")
    entity:EmitSound(soundId)

	entity:SetResurrectable(entity.WorldId)

	-- Wait 3 seconds
	
	-- Appear
	-- Enable grace period for 3 seconds


    Logging:Info("Player Died!")
	
	local limboWorld = Worlds:GetByName("Limbo")
	
	Worlds:RequestAddEntity(entity, limboWorld.Id)
	
    Triggers:OnEntityEnteredWorld(
        entity,
        PostEnter,
        true)
	
	
end

local function OnInit(entity)

    Logging:Info("Initializing " .. entity.Id)

    cooldownTimerId = entity:GetTimerId("CooldownDelay")
    delayTimerId = entity:GetTimerId("ActionDeley")


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
		
    Triggers:OnDestroyed(
        entity,
        SendToLimbo,
        false)
			
end

return {
    systemHooks = {
        OnEnter = ShowMission,
        OnInit = OnInit
    }
}








