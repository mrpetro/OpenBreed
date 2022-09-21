-- Variable definitions
local smartCardEntity, actorEntity = ...
local gameCameraEntity = Entities:GetPlayerCamera(actorEntity)
local smartCardReaderCameraEntity = Entities:GetSmartcardReaderCamera()
local hudCameraEntity = Entities:GetHudCamera()
local hudViewportEntity = Entities:GetHudViewport()
local gameViewportEntity = Entities:GetGameViewport()
local cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id
local cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id

--functions
local GameWorldPause
local GameWorldFadeOut
local SwitchViewToSmartCardWorld
local SmartCardWorldShowText
local SmartCardWorldFadeOut
local SwitchViewToGameWorld
local GameWorldUnpause

-- Functions

GameWorldPause = function()
    Logging:Info("Game world pause...")

	Triggers:OnPausedWorld(
        gameCameraEntity,
        GameWorldFadeOut,
        true)

    gameCameraEntity:PauseWorld()
end

GameWorldFadeOut = function(entity, args)
    Logging:Info("Game world fade out...")

    Triggers:OnEntityAnimFinished(
        gameCameraEntity,                      
        SwitchViewToSmartCardWorld,
        true)

    gameCameraEntity:PlayAnimation(0, cameraFadeOutClipId)

    Logging:Info("gameCameraEntity: " .. tostring(gameCameraEntity.Id))
end

SwitchViewToSmartCardWorld = function(entity, args)
    Logging:Info("Switch view to smart card world camera...")

    smartCardReaderCameraEntity:SetBrightness(0)
    hudViewportEntity:SetViewportCamera(smartCardReaderCameraEntity.Id)
    --actorEntity:SetPosition(Entities, Shapes, smartCardEntity)

    Triggers:OnEntityAnimFinished(
        smartCardReaderCameraEntity,                      
        SmartCardWorldShowText,
        true)

    smartCardReaderCameraEntity:PlayAnimation(0, cameraFadeInClipId)
end

SmartCardWorldShowText = function(entity, args)
    Logging:Info("Smart card world show text...")

    --TODO: Show text here and wait for button
    Triggers:AfterDelay(Commentator, TimeSpan.FromMilliseconds(2000), SmartCardWorldFadeOut)
end


SmartCardWorldFadeOut = function(entity, args)
    Logging:Info("Smart card world fade out...")

    Triggers:OnEntityAnimFinished(
        smartCardReaderCameraEntity,                      
        SwitchViewToGameWorld,
        true)

    smartCardReaderCameraEntity:PlayAnimation(0, cameraFadeOutClipId)
end

SwitchViewToGameWorld = function(entity, args)
    Logging:Info("Switch view to game world camera...")

    gameCameraEntity:SetBrightness(0)
    hudViewportEntity:SetViewportCamera(hudCameraEntity.Id)
    --actorEntity:SetPosition(Entities, Shapes, smartCardEntity)

    Triggers:OnEntityAnimFinished(
        gameCameraEntity,                      
        GameWorldUnpause,
        true)

    gameCameraEntity:PlayAnimation(0, cameraFadeInClipId)
end


GameWorldUnpause = function()
    Logging:Info("GameWorldUnpause...")

    actorEntity.State = nil

    gameCameraEntity:UnpauseWorld()
end

-- Execution

--Logging:Info("TeleportEntityId:" .. tostring(smartCardEntity.Id) .. "(" .. smartCardEntity.Tag .. ")")
--Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id) .. "(" .. actorEntity.Tag .. ")")

if (tostring(actorEntity.State) == "SmartCardReading")
then
	return
end

actorEntity.State = "SmartCardReading"

if (gameCameraEntity == nil)
then
	return
end

smartCardEntity:Destroy()
GameWorldPause()








