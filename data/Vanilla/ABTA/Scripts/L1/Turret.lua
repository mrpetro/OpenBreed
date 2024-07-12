-- In 2 player game, turret locks on first player spotted and stays locked utill loosing sight.

local cooldownTimerId
local delayTimerId
local previousDegree = 0
local speedFactor = 150
local fireRate = 0.3
local fireReady = true

local function Explode(entity)
    
    local pos = entity:GetPosition()
    entity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
        :SetOption("flavor", "Big")
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)
        :Finish()
     
    Worlds:RequestRemoveEntity(entity)
	Entities:RequestErase(entity)
		
end

local function CooldownFinish(entity, args)
    fireReady = true
end

local function FireBullet(entity)

    if(not(fireReady))
    then
        return
    end

    
    local pos = entity:GetPosition()
    local dir = MovementTools.SnapToCompass16Way(entity:GetDirection())

    pos = pos + dir * 16
    local thrust = dir * speedFactor

    local emitter = entity:StartEmit("ABTA\\Templates\\L1\\TurretLazer")
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)
        :SetOption("thrustX", thrust.X)
        :SetOption("thrustY", thrust.Y)
        :Finish()

	Logging:Info("TURRET.FIRE" )

    fireReady = false

	Triggers:AfterDelay(entity, cooldownTimerId, TimeSpan.FromMilliseconds(1000 / fireRate), CooldownFinish)
end

local function OnDirectionChanged(entity, args)

    local pos = entity:GetPosition()
    local targetDirection = entity:GetTargetDirection()
    local direction = entity:GetDirection()
	
	--Logging:Info("TURRET.POS: (" .. tostring(pos.X) .. ", " .. tostring(pos.Y) .. ")" )
	--Logging:Info("TURRET.DIR : (" .. tostring(direction.X) .. ", " .. tostring(direction.Y) .. ")" )

    local degree = MovementTools.SnapToCompass16Degree(direction.X, direction.Y)

    if (degree ~= previousDegree)
    then
		local soundId = Sounds:GetByName("Vanilla/Common/Turret/Turn")
		entity:EmitSound(soundId)
		
	    local animName = "Vanilla/L1/Turret/Tracking/" .. tostring(degree)	
		local clip = Clips:GetByName(animName)
		entity:PlayAnimation(0, clip.Id)
		
		previousDegree = degree	
    end
end

local function OnDirectionSet(entity, args)

    if (entity:HasTrackedEntity())
    then
		FireBullet(entity)
    end
	
end

local function OnInit(entity)

    cooldownTimerId = entity:GetTimerId("CooldownDelay")
    delayTimerId = entity:GetTimerId("ActionDeley")


    Triggers:OnEntityDirectionChanged(
        entity,
        OnDirectionChanged,
        false)

    Triggers:OnEntityDirectionSet(
        entity,
        OnDirectionSet,
        false)

    Triggers:OnDestroyed(
        entity,
        Explode,
        false)
		
end

return {
    systemHooks = {
        OnInit = OnInit
    }
}








