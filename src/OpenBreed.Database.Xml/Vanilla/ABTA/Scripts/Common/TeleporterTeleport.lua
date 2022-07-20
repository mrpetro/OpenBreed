﻿-- Variable definitions
local teleportEntity, actorEntity = ...
local cameraEntity
local cameraFadeOutClipId;
local cameraFadeInClipId;

--functions
local PauseWorld
local FadeOut
local SetPosition
local WorldUnpause
local FadeIn

-- Functions

PauseWorld = function()
    Logging:Info("PauseWorld...")

	Triggers:OnPausedWorld(
        cameraEntity,
        FadeOut,
        true)

    cameraEntity:PauseWorld()
end

FadeOut = function(entity, args)
    Logging:Info("FadeOut...")

    Triggers:OnEntityAnimFinished(
        cameraEntity,                      
        SetPosition,
        true)

    cameraEntity:PlayAnimation(0, cameraFadeOutClipId)
end

SetPosition = function(entity, args)
    Logging:Info("SetPosition...")

    actorEntity:SetPosition(Entities, Shapes, teleportEntity)

    WorldUnpause()
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

--Logging:Info("TeleportEntityId:" .. tostring(teleportEntity.Id) .. "(" .. teleportEntity.Tag .. ")")
--Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id) .. "(" .. actorEntity.Tag .. ")")

if (tostring(actorEntity.State) == "Teleporting")
then
	return
end

actorEntity.State = "Teleporting"

cameraEntity = actorEntity:GetPlayerCamera(Entities)

if (cameraEntity == nil)
then
	return
end


cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id;
cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id;

PauseWorld(); 








