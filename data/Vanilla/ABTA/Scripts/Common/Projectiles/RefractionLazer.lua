
local function Hit(projectileEntity, targetEntity, projection)


    local pos = projectileEntity:GetPosition() + projection
    projectileEntity:SetPosition(pos.X, pos.Y)

    local v = projectileEntity:GetThrust()
    local n = - projection:Normalized()

    local vp = - 2 * (Vector2.Dot(n, v)) * n + v

    projectileEntity:SetThrust(vp.X, vp.Y)
    local dir = vp:Normalized()
    local degree = MovementTools.SnapToCompass8Degree(dir.X, dir.Y)

    local animName = "Vanilla/Common/Projectile/RefractionLazer/High/" .. tostring(degree)

    local animId = Clips:GetByName(animName).Id
    projectileEntity:PlayAnimation(0, animId)

     if(targetEntity:HasHealth())
     then
        projectileEntity:InflictDamage(10, targetEntity.Id)
     end
end

local function Explode(entity, args)

    local pos = entity:GetPosition()

    entity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)
        :SetOption("flavor", "Small")
        :Finish()
end

local function Destroy(entity)

     Entities:RequestDestroy(projectileEntity)

end

local function OnInit(entity)

    local dir = entity:GetThrust():Normalized()
    local degree = MovementTools.SnapToCompass8Degree(dir.X, dir.Y)
    local animName = "Vanilla/Common/Projectile/RefractionLazer/High/" .. tostring(degree)
    local animId = Clips:GetByName(animName).Id
    entity:PlayAnimation(0, animId)

    Triggers:OnLifetimeEnd(
        entity,
        Explode,
        true)

    Triggers:OnExpunge(
        entity,
        Destroy,
        true)

end

return {
    systemHooks = {
        OnInit = OnInit,
        OnCollision = Hit
    }
}