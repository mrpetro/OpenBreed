
local function Destroy(entity)
    Worlds:RequestRemoveEntity(entity)
	Entities:RequestDestroy(entity)
end

local function Animate(entity, args)

    local emiterEntity = Entities:GetById(args.EmiterEntityId)
    local emiterPos = emiterEntity:GetPosition()

    entity:SetPosition(emiterPos.X, emiterPos.Y)

    local clipName = "Vanilla/Common/Explosion/Small"

    local clipId = Clips:GetByName(clipName).Id

    entity:SetSpriteOn()

    Triggers:OnEntityAnimFinished(
        entity,
        Destroy,
        true)

    entity:PlayAnimation(0, clipId)
end



local function OnInit(entity)
    Triggers:OnEmitEntity(
        entity,
        Animate,
        true)

end

return {
    systemHooks = {
        OnInit = OnInit
    }
}










