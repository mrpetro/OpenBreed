
local function Destroy(entity)
    Worlds:RequestRemoveEntity(entity)
	Entities:RequestDestroy(entity)
end

local function OnInit(entity)

    local entityMetadata = entity:GetMetadata()
    local clipName = "Vanilla/Common/Explosion/" .. entityMetadata.Flavor
    local clipId = Clips:GetByName(clipName).Id

    entity:SetSpriteOn()

    Triggers:OnEntityAnimFinished(
        entity,
        Destroy,
        true)

    entity:PlayAnimation(0, clipId)

end

return {
    systemHooks = {
        OnInit = OnInit
    }
}










