
local function Hit(projectileEntity, targetEntity, projection)

     projectileEntity:StartEmit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\Explosion.xml")
        :SetOption("flavor", "Small")
        :Finish()

     Worlds:RequestRemoveEntity(projectileEntity)
     Entities:RequestDestroy(projectileEntity)
end

local function Fire(entity, args)

    local clipName = "Vanilla/Common/Explosion/Small"

    local animId = Clips:GetByName(clipName).Id

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