
local function Hit(projectileEntity, targetEntity, projection)

     projectileEntity:StartEmit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\Explosion.xml")
        :SetOption("flavor", "Small")
        :Finish()

     projectileEntity:Expunge()
end

local function Explode(entity, args)

    local pos = entity:GetPosition()

    entity:StartEmit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\Explosion.xml")
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
    local degree = MovementTools.SnapToCompass16Degree(dir.X, dir.Y)
    local animName = "Vanilla/Common/Projectile/Missile/High/" .. tostring(degree)
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