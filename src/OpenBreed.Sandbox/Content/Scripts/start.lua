

EngineInitialized = function()
	myass = Core.Rendering.Viewports:Create(10,10,400,400)
end

WorldLoaded = function(world)
	player = Core.Players.GetPlayer("P1")

	--Trigger.AfterDelay(
	--	DateTime.Seconds(5), 
	--	function()
	--		Actor.Create("camera", true, { Owner = player, Location = BaseCameraPoint.Location }) 
	--	end
	--	)

	--Camera.Position = InsertionLZ.CenterPosition
end