
local function Hit(projectileEntity, targetEntity, projection)

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
    Entities:RequestDestroy(projectileEntity)

end

local function OnInit(entity)

    local clipName = "Vanilla/Common/Explosion/Small"
    local animId = Clips:GetByName(clipName).Id
    entity:PlayAnimation(0, animId)

end

return {
    systemHooks = {
        OnInit = OnInit,
        OnCollision = Hit
    }
}