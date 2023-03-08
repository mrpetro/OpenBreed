
local function Hit(missileEntity, targetEntity)

     missileEntity:Emit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\Explosion.xml")
	
     Worlds:RequestRemoveEntity(missileEntity)
     Entities:RequestDestroy(missileEntity)
end

local function Fire(entity, args)

    local emiterEntity = Entities:GetById(args.EmiterEntityId)
    local emiterPos = emiterEntity:GetPosition()
    local emiterDir = emiterEntity:GetDirection()

    entity:SetPosition(emiterPos.X, emiterPos.Y)

    local dx = emiterDir.X * 500.0
    local dy = emiterDir.Y * 500.0

    entity:SetThrust(dx , dy)

    local degree = MovementTools.SnapToCompass16Degree(dx, dy)
    local animName = "Vanilla/Common/Projectile/Missile/High/" .. tostring(degree)

    local animId = Clips:GetByName(animName).Id

    entity:PlayAnimation(0, animId)

end

local function OnInit(entity)
    Triggers:OnEmitEntity(
        entity,
        Fire,
        true)

end

return {
    systemHooks = {
        OnInit = OnInit,
        OnCollision = Hit
    }
}