-- In 2 player game, turret locks on first player spotted and stays locked utill loosing sight.

local previousDegree = 0

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
    end
	
	previousDegree = degree
end

local function OnInit(entity)

    Triggers:OnEntityDirectionChanged(
        entity,
        OnDirectionChanged,
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








