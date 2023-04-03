local function Teleport(teleportEntity, actorEntity, projection)

    -- Variable definitions
    local cameraEntity = Entities:GetPlayerCamera(actorEntity)

    local hudCameraEntity = Entities:GetHudCamera()

    local cameraFadeOutClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeOut").Id
    local cameraFadeInClipId = Clips:GetByName("Vanilla/Common/Camera/Effects/FadeIn").Id

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
        --hudCameraEntity:PlayAnimation(0, cameraFadeOutClipId)
    end

    SetPosition = function(entity, args)
        Logging:Info("SetPosition to exit...")

        actorEntity:SetPositionToExit(Entities, Shapes, teleportEntity)

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
        hudCameraEntity:PlayAnimation(0, cameraFadeInClipId)
    end

    -- Execution

    --Logging:Info("TeleportEntityId:" .. tostring(teleportEntity.Id) .. "(" .. teleportEntity.Tag .. ")")
    --Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id) .. "(" .. actorEntity.Tag .. ")")

    if (tostring(actorEntity.State) == "Teleporting")
    then
	    return
    end

    actorEntity.State = "Teleporting"

    PauseWorld()
end

return {
    systemHooks = {
        OnCollision = Teleport
    }
}







