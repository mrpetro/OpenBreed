
local function Erase(entity)
    Worlds:RequestRemoveEntity(entity)
	Entities:RequestErase(entity)
end

local function OnInit(entity)


	Logging:Info("------------------------------------------")

    --local entityMetadata = entity:GetMetadata()
    --local clipName = "Vanilla/Common/Explosion/" .. entityMetadata.Flavor
    --local clipId = Clips:GetByName(clipName).Id

    --entity:SetSpriteOn()

    --Triggers:OnEntityAnimFinished(
    --    entity,
    --    Erase,
    --    true)
	

    --entity:PlayAnimation(0, clipId)

end

return {
    systemHooks = {
        OnInit = OnInit
    }
}










