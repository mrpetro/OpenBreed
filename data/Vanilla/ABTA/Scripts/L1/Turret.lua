-- In 2 player game, turret locks on first player spotted and stays locked utill loosing sight.

local function Explode(entity)
    
    local pos = entity:GetPosition()
    entity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
        :SetOption("flavor", "Big")
        :SetOption("startX", pos.X + 16 + 8)
        :SetOption("startY", pos.Y + 16 + 8)
        :Finish()
     
    Worlds:RequestRemoveEntity(entity)
	Entities:RequestErase(entity)
		
end

local function OnInit(entity)

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








