

EngineInitialized = function()
    Logging:Verbose("Engine initialized.")

	 --Entities:CreateFromTemplate("")
	--myass = Core.Rendering.Viewports:Create(10,10,400,400)
end

WorldLoaded = function(world)
    Logging:Verbose("World '" .. world .. "' loaded.")

	--Trigger.OnEvent(Collision.Events.FadeIn, camera, Logo2FadedIn)

	--player = Players:GetPlayer("P1")

	--Trigger.AfterDelay(
	--	DateTime.Seconds(5), 
	--	function()
	--		Actor.Create("camera", true, { Owner = player, Location = BaseCameraPoint.Location }) 
	--	end
	--	)

	--Camera.Position = InsertionLZ.CenterPosition
end