
local function Hit(projectileEntity, targetEntity, projection)

	local sourceEntityId = projectileEntity:GetSourceEntityId()
	
	-- Ignore hitting source entity 
    if (sourceEntityId == targetEntity.Id)
    then
	    return
    end

    local pos = projectileEntity:GetPosition()
    projectileEntity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
        :SetOption("flavor", "Small")
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)
        :Finish()
	
    if(targetEntity:HasHealth())
    then
        projectileEntity:InflictDamage(10, targetEntity.Id)
    end

	Worlds:RequestRemoveEntity(projectileEntity)
    Entities:RequestErase(projectileEntity)
end

local function OnInit(entity)

end

return {
    systemHooks = {
        OnInit = OnInit,
        OnCollision = Hit
    }
}