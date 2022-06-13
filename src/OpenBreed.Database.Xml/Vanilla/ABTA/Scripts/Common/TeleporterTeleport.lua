local teleportEntity, actorEntity = ...

Logging:Info("TeleportEntityId:" .. tostring(teleportEntity.Id) .. "(" .. teleportEntity.Tag .. ")")
Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id) .. "(" .. actorEntity.Tag .. ")")

if (tostring(actorEntity.State) == "Teleporting")
then
	return
end

actorEntity.State = "Teleporting"

local cameraEntity = actorEntity:GetPlayerCamera(Entities)

if (cameraEntity == nil)
then
	return
end

local cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id;
local cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id;
local targetWorldId = actorEntity.WorldId;



Logging:Info("Teleport!")



