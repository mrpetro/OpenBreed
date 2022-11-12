local function Show(smartCardEntity, actorEntity)

    -- Variable definitions
    --local smartCardEntity, actorEntity = ...
    local gameCameraEntity = Entities:GetPlayerCamera(actorEntity)
    local smartCardReaderCameraEntity = Entities:GetSmartcardReaderCamera()
    local smartCardReaderTextEntity = Entities:GetSmartCardReaderText()
    local gameWorld = Worlds:GetWorld(smartCardEntity)
    local hudCameraEntity = Entities:GetHudCamera()
    local hudViewportEntity = Entities:GetHudViewport()
    local gameViewportEntity = Entities:GetGameViewport()
    local cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id
    local cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id
    local smartCardMetadata = smartCardEntity:GetMetadata()
    local textId = gameWorld.Name .. "/" .. smartCardMetadata.Name .. tostring(smartCardMetadata.Option)
    Logging:Info("Text Id: " .. textId)
    local text = Texts:GetTextString(textId)
    local currentCharacter = 0
    local textLength = string.len(text)

    --functions
    local GameWorldPause
    local GameWorldFadeOut
    local SwitchViewToSmartCardWorld
    local SmartCardWorldShowText
    local SmartCardWorldFadeOut
    local SwitchViewToGameWorld
    local GameWorldUnpause
    local PrintNextCharacter
    local ShowAllTextIfNeeded

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

        DisplayTextToNextCharacter(gameWorld, nil)
    end


    DisplayTextToNextCharacter = function(world, args)

       if( currentCharacter > textLength)
       then
            return
       end

       local textPart = string.sub(text,0, currentCharacter)

       smartCardReaderTextEntity:SetText(0, textPart)

       --Triggers:EveryUpdate(
       --     gameWorld,
       --     DisplayTextToNextCharacter,
       --     true)

       Triggers:AfterDelay(
            Commentator,
            TimeSpan.FromMilliseconds(60),
            DisplayTextToNextCharacter,
            true)

       currentCharacter = currentCharacter + 1
    end

    SmartCardWorldShowText = function(entity, args)
        Logging:Info("Smart card world show text...")

        Triggers:AnyKeyPressed(
            ShowAllTextIfNeeded,
            true)
    end

    ShowAllTextIfNeeded = function(args)

        --If all text is shown, go straight to fade out
        if( currentCharacter > textLength)
        then
            SmartCardWorldFadeOut(nil)
            return
        --Else show all text and wait for button to be pressed
        else
            currentCharacter = textLength
            Triggers:AnyKeyPressed(
                SmartCardWorldFadeOut,
                true)
        end
    end

    SmartCardWorldFadeOut = function(args)
        Logging:Info("Smart card world fade out...")

        Triggers:OnEntityAnimFinished(
            smartCardReaderCameraEntity,                      
            GameWorldUnpause,
            true)

        smartCardReaderCameraEntity:PlayAnimation(0, cameraFadeOutClipId)
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
        --actorEntity:SetPosition(Entities, Shapes, smartCardEntity)

        --Triggers:OnEntityAnimFinished(
        --    gameCameraEntity,                      
        --    GameWorldUnpause,
        --    true)

        gameCameraEntity:PlayAnimation(0, cameraFadeInClipId)

        actorEntity.State = nil
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

end

return {
    systemHooks = {
        ScriptRunTrigger = Show
    }
}








