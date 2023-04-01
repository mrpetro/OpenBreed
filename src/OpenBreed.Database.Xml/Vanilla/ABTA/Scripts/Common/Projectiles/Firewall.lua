
local function Hit(projectileEntity, targetEntity, projection)

     projectileEntity:StartEmit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\Explosion.xml")
        :SetOption("flavor", "Small")
        :Finish()

     projectileEntity:Expunge()
end

local function Destroy(entity)

     Entities:RequestDestroy(projectileEntity)

end

local function OnInit(entity)

    local clipName = "Vanilla/Common/Explosion/Small"
    local animId = Clips:GetByName(clipName).Id
    entity:PlayAnimation(0, animId)

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