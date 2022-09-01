-- Variable definitions
local smartCardEntity, actorEntity = ...
local cameraEntity
local cameraFadeOutClipId;
local cameraFadeInClipId;

--functions
local GameWorldPause
local GameWorldFadeOut
local SwitchViewToSmartCardWorld
local SmartCardWorldUnpause
local SmartCardWorldFadeIn
local SmartCardWorldShowText

-- Functions

GameWorldPause = function()
    Logging:Info("Game world pause...")

	Triggers:OnPausedWorld(
        cameraEntity,
        GameWorldFadeOut,
        true)

    cameraEntity:PauseWorld()
end

GameWorldFadeOut = function(entity, args)
    Logging:Info("Game world fade out...")

    Triggers:OnEntityAnimFinished(
        cameraEntity,                      
        SwitchViewToSmartCardWorld,
        true)

    cameraEntity:PlayAnimation(0, cameraFadeOutClipId)
end

SwitchViewToSmartCardWorld = function(entity, args)
    Logging:Info("Switch view to smart card world camera...")

    --actorEntity:SetPosition(Entities, Shapes, smartCardEntity)

    SmartCardWorldUnpause()
end

SmartCardWorldUnpause = function(entity, args)
    Logging:Info("Smart card world unpause...")

    --actorEntity:SetPosition(Entities, Shapes, smartCardEntity)

    SmartCardWorldFadeIn()
end

SmartCardWorldFadeIn = function(entity, args)
    Logging:Info("Smart card world fade in...")

    --actorEntity:SetPosition(Entities, Shapes, smartCardEntity)

    SmartCardWorldShowText()
end

SmartCardWorldShowText = function(entity, args)
    Logging:Info("Smart card world show text...")

    --actorEntity:SetPosition(Entities, Shapes, smartCardEntity)

end

WorldUnpause = function()
    Logging:Info("WorldUnpause...")

	Triggers:OnUnpausedWorld(
        cameraEntity,
        FadeIn,
        true)

    cameraEntity:UnpauseWorld()
end

FadeIn = function(entity, args)
    Logging:Info("FadeIn...")

    cameraEntity:PlayAnimation(0, cameraFadeInClipId)
end

-- Execution

--Logging:Info("TeleportEntityId:" .. tostring(smartCardEntity.Id) .. "(" .. smartCardEntity.Tag .. ")")
--Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id) .. "(" .. actorEntity.Tag .. ")")

if (tostring(actorEntity.State) == "Teleporting")
then
	return
end

actorEntity.State = "Teleporting"

cameraEntity = Entities:GetPlayerCamera(actorEntity)

if (cameraEntity == nil)
then
	return
end


cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id;
cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id;

GameWorldPause(); 








