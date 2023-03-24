
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
end

local function Fire(entity, args)

    local dir = entity:GetThrust():Normalized()
    local degree = MovementTools.SnapToCompass8Degree(dir.X, dir.Y)

    local animName = "Vanilla/Common/Projectile/RefractionLazer/High/" .. tostring(degree)

    local animId = Clips:GetByName(animName).Id

    entity:PlayAnimation(0, animId)

end

local function Explode(entity, args)

    local pos = entity:GetPosition()

    entity:StartEmit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\Explosion.xml")
        :SetOption("startX", pos.X)
        :SetOption("startY", pos.Y)
        :SetOption("flavor", "Small")
        :Finish()
end

local function OnInit(entity)
    Triggers:OnEmitEntity(
        entity,
        Fire,
        true)

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