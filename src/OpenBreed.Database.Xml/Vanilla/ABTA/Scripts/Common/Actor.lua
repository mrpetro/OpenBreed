local function ShowMission(actorEntity)
    -- Variable definitions
    local gameCameraEntity = Entities:GetPlayerCamera(actorEntity)
    local missionScreenCameraEntity = Entities:GetMissionScreenCamera()
    local missionScreenTextEntity = Entities:GetMissionScreenText()
    local gameWorld = Worlds:GetWorld(actorEntity)
    local missionEntity = Entities:GetMission(gameWorld.Id)
    local hudCameraEntity = Entities:GetHudCamera()
    local hudViewportEntity = Entities:GetHudViewport()
    local gameViewportEntity = Entities:GetGameViewport()
    local cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id
    local cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id
    local missionMetadata = missionEntity:GetMetadata()
    local textId = gameWorld.Name .. "/" .. missionMetadata.Name
    Logging:Info("Text Id: " .. textId)
    local text = Texts:GetTextString(textId)
    local currentCharacter = 0
    local textLength = string.len(text)

    --functions
    local GameWorldPause
    local GameWorldFadeOut
    local SwitchViewToMissionWorld
    local MissionWorldShowText
    local MissionWorldFadeOut
    local SwitchViewToGameWorld
    local GameWorldUnpause
    local DisplayTextWithNextCharacter
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

        Triggers:OnEntityAnimFinished(
            missionScreenCameraEntity,                      
            MissionWorldShowText,
            true)

        missionScreenCameraEntity:PlayAnimation(0, cameraFadeInClipId)

        Triggers:EveryFrame(
            Commentator,
            DisplayTextWithNextCharacter,
            0,
            true)
    end

    DisplayTextWithNextCharacter = function(entity, args)

        if( currentCharacter > textLength)
        then
            return
        end

        local textPart = string.sub(text,0, currentCharacter)

        missionScreenTextEntity:SetText(0, textPart)

        currentCharacter = currentCharacter + 1

        Triggers:EveryFrame(
            Commentator,
            DisplayTextWithNextCharacter,
            0,
            true)
    end

    MissionWorldShowText = function(entity, args)
        Logging:Info("Mission world show text...")

        Triggers:AnyKeyPressed(
            ShowAllTextIfNeeded,
            true)
    end

    ShowAllTextIfNeeded = function(args)

        --If all text is shown, go straight to fade out
        if( currentCharacter > textLength)
        then
            MissionWorldFadeOut(nil)
            return
        --Else show all text and wait for button to be pressed
        else
            currentCharacter = textLength
            Triggers:AnyKeyPressed(
                MissionWorldFadeOut,
                true)
        end
    end

    MissionWorldFadeOut = function(args)
        Logging:Info("Mission world fade out...")

        Triggers:OnEntityAnimFinished(
            missionScreenCameraEntity,                      
            GameWorldUnpause,
            true)

        missionScreenCameraEntity:PlayAnimation(0, cameraFadeOutClipId)
    end

    GameWorldUnpause = function()
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

        actorEntity.State = nil
    end

    if (tostring(actorEntity.State) == "MissionShowing")
    then
	    return
    end

    actorEntity.State = "MissionShowing"

    if (gameCameraEntity == nil)
    then
	    return
    end

    GameWorldPause()
end

return {
    systemHooks = {
        onEnter = ShowMission
    }
}








