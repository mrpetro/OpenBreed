
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
    Entities:RequestErase(projectileEntity)
end

local function Explode(entity, args)

    local pos = entity:GetPosition()

    entity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)
        :SetOption("flavor", "Big")
        :Finish()
end

local function OnInit(entity)

    local dir = entity:GetThrust():Normalized()
    local degree = MovementTools.SnapToCompass16Degree(dir.X, dir.Y)
    local animName = "Vanilla/Common/Projectile/Missile/High/" .. tostring(degree)
    local animId = Clips:GetByName(animName).Id
    entity:PlayAnimation(0, animId)

    Triggers:OnLifetimeEnd(
        entity,
        Explode,
        true)

end

return {
    systemHooks = {
        OnInit = OnInit,
        OnCollision = Hit
    }
}