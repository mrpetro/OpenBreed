local function DisplayMissionScreen(missionEntity, playerCharacterEntity)

    -- Variable definitions
    local gameCameraEntity = Entities:GetPlayerCamera(playerCharacterEntity)
    local missionScreenCameraEntity = Entities:GetMissionScreenCamera()
    local missionScreenTextEntity = Entities:GetMissionScreenText()
    local missionScreenBackgroundEntity =  Entities:GetMissionScreenBackground()
    local gameWorld = Worlds:GetWorld(playerCharacterEntity)
    local directorEntity = Entities:GetDirector(gameWorld.Id)
    local hudCameraEntity = Entities:GetHudCamera()
    local hudViewportEntity = Entities:GetHudViewport()
    local gameViewportEntity = Entities:GetGameViewport()
    local backgroundDarkenClipId = Clips:GetByName("Vanilla/Common/Picture/Effects/Darken").Id
    local textFadeInClipId = Clips:GetByName("Vanilla/Common/Text/Effects/FadeIn").Id
    local textFadeOutClipId = Clips:GetByName("Vanilla/Common/Text/Effects/FadeOut").Id
    local cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id
    local cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id
    local missionMetadata = missionEntity:GetMetadata()
    local textId = gameWorld.Name .. "/" .. missionMetadata.Name
    Logging:Info("Text Id: " .. textId)
    local text = Texts:GetTextString(textId)
    local currentCharacter = 0
    local textLength = string.len(text)
    local delayTimer = 0

    --functions
    local GameWorldPause
    local GameWorldFadeOut
    local SwitchViewToMissionWorld

    local ScreenFadeIn
    local Wait3Seconds
    local DarkenBackground
    local TextFadeIn
    local WaitForKey
    local TextFadeOut
    local ScreenFadeOut

    local SwitchViewToGameWorld
    local GameWorldUnpause
    local WaitForAction
    local ShowAllTextIfNeeded

    -- Functions
    GameWorldPause = function()
        Logging:Info("Game world pause...")

	    Triggers:OnPausedWorld(
            gameCameraEntity,
            SwitchViewToMissionWorld,
            true)

	    --Triggers:OnPausedWorld(
        --    gameCameraEntity,
        --    GameWorldFadeOut,
        --    true)

        gameCameraEntity:PauseWorld()
    end

    GameWorldFadeOut = function(entity, args)
        Logging:Info("Game world fade out...")

        Triggers:OnEntityAnimFinished(
            gameCameraEntity,                      
            SwitchViewToMissionWorld,
            true)

        gameCameraEntity:PlayAnimation(0, cameraFadeOutClipId)

        Logging:Info("gameCameraEntity: " .. tostring(gameCameraEntity.Id))
    end

    SwitchViewToMissionWorld = function(entity, args)
        Logging:Info("Switch view to mission world camera...")

        missionScreenCameraEntity:SetBrightness(0)
        hudViewportEntity:SetViewportCamera(missionScreenCameraEntity.Id)

        ScreenFadeIn()
    end

    ScreenFadeIn = function()
        Logging:Info("Screen Fade In...")

        Triggers:OnEntityAnimFinished(
            missionScreenCameraEntity,                      
            Wait3Seconds,
            true)

        missionScreenCameraEntity:PlayAnimation(0, cameraFadeInClipId)
    end

    Wait3Seconds = function(entity, args)
        Logging:Info("Wait 2 seconds...")

	    Triggers:AfterDelay(
            Commentator,
            delayTimer,
            TimeSpan.FromMilliseconds(2000),
            DarkenBackground,
            true)
    end

    DarkenBackground = function()
        Logging:Info("Darken background...")

        Triggers:OnEntityAnimFinished(
            missionScreenBackgroundEntity,                      
            TextFadeIn,
            true)

        missionScreenBackgroundEntity:PlayAnimation(0, backgroundDarkenClipId)
    end

    TextFadeIn = function(entity, args)
        Logging:Info("Text fade in...")

        Triggers:OnEntityAnimFinished(
            missionScreenTextEntity,                      
            WaitForKey,
            true)

        missionScreenTextEntity:PlayAnimation(0, textFadeInClipId)
    end

    WaitForKey = function(entity, args)
        Logging:Info("Wait for key...")

        Triggers:AnyKeyPressed(
            TextFadeOut,
            true)

    end

    TextFadeOut = function()
        Logging:Info("Text fade out...")

        Triggers:OnEntityAnimFinished(
            missionScreenTextEntity,                      
            ScreenFadeOut,
            true)

        missionScreenTextEntity:PlayAnimation(0, textFadeOutClipId)
    end

    ScreenFadeOut = function(entity, args)
        Logging:Info("Screen Fade Out...")

        Triggers:OnEntityAnimFinished(
            missionScreenCameraEntity,                      
            GameWorldUnpause,
            true)

        missionScreenCameraEntity:PlayAnimation(0, cameraFadeOutClipId)
    end


    GameWorldUnpause = function(entity, args)
        Logging:Info("Game world unpause...")

	    Triggers:OnUnpausedWorld(
            gameCameraEntity,
            SwitchViewToGameWorld,
            true)

        gameCameraEntity:UnpauseWorld()
    end

    SwitchViewToGameWorld = function(entity, args)
        Logging:Info("Switch view to game world camera...")

        gameCameraEntity:SetBrightness(0)
        hudViewportEntity:SetViewportCamera(hudCameraEntity.Id)

        gameCameraEntity:PlayAnimation(0, cameraFadeInClipId)

        playerCharacterEntity.State = nil

        directorEntity:TryInvoke(Scripting, Logging, "OnStartMission", actorEntity)
    end

    if (tostring(playerCharacterEntity.State) == "MissionShowing")
    then
	    return
    end

    playerCharacterEntity.State = "MissionShowing"

    if (gameCameraEntity == nil)
    then
	    return
    end

    missionScreenBackgroundEntity:SetPictureColor(1.0,1.0,1.0,1.0)
    missionScreenTextEntity:SetTextColor(0,1.0,1.0,1.0,0.0)
    missionScreenTextEntity:SetText(0, text)
    GameWorldPause()
end

return {
    systemHooks = {
        OnShow = DisplayMissionScreen
    }
}








