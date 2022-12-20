local function Show(smartCardEntity, actorEntity)

    -- Variable definitions
    --local smartCardEntity, actorEntity = ...
    local gameCameraEntity = Entities:GetPlayerCamera(actorEntity)
    local smartCardScreenCameraEntity = Entities:GetSmartCardScreenCamera()
    local smartCardScreenTextEntity = Entities:GetSmartCardScreenText()
    local gameWorld = Worlds:GetWorld(smartCardEntity)
    local hudCameraEntity = Entities:GetHudCamera()
    local hudViewportEntity = Entities:GetHudViewport()
    local gameViewportEntity = Entities:GetGameViewport()
    local cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id
    local cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id
    local smartCardMetadata = smartCardEntity:GetMetadata()
    local textId = gameWorld.Name .. "/" .. smartCardMetadata.Name .. "/" .. tostring(smartCardMetadata.Option)
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
    local DisplayTextWithNextCharacter
    local ShowAllTextIfNeeded
    local RemoveSmartcard


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

        smartCardScreenCameraEntity:SetBrightness(0)
        hudViewportEntity:SetViewportCamera(smartCardScreenCameraEntity.Id)

        Triggers:OnEntityAnimFinished(
            smartCardScreenCameraEntity,                      
            SmartCardWorldShowText,
            true)

        smartCardScreenCameraEntity:PlayAnimation(0, cameraFadeInClipId)

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

        smartCardScreenTextEntity:SetText(0, textPart)

        currentCharacter = currentCharacter + 1

        Triggers:EveryFrame(
            Commentator,
            DisplayTextWithNextCharacter,
            0,
            true)
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
            smartCardScreenCameraEntity,                      
            GameWorldUnpause,
            true)

        smartCardScreenCameraEntity:PlayAnimation(0, cameraFadeOutClipId)
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

    RemoveSmartcard = function(entity)

        local metaData = entity:GetMetadata()

        if (metaData.Flavor ~= "Trigger")
        then
            local stampName = tostring(metaData.Level) .. "/" .. tostring(metaData.Name) .. "/" .. tostring(metaData.Flavor) .. "/Picked"
            Logging:Info("StampName: " .. stampName)
            local stampId = Stamps:GetByName(stampName).Id
            Logging:Info("StampId: " .. tostring(stampId))
            entity:PutStamp(stampId, 0)
        end

	    Worlds:RequestRemoveEntity(entity)
	    Entities:RequestDestroy(entity)
    end

    if (tostring(actorEntity.State) == "SmartCardReading")
    then
	    return
    end

    actorEntity.State = "SmartCardReading"

    if (gameCameraEntity == nil)
    then
	    return
    end

    Entities:ForEachEntity(
        gameWorld.Id,
        "SmartCard",
        smartCardMetadata.Option,
        RemoveSmartcard)

    local soundName = "Vanilla/Common/Speech/SmartCardMessageFollows"
    local soundId = Sounds:GetByName(soundName)
    Commentator:EmitSound(soundId)

    GameWorldPause()

end



return {
    systemHooks = {
        ScriptRunTrigger = Show
    }
}








