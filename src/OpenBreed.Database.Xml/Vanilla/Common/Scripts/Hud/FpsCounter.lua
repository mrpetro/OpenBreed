
EntityTypes.FpsCounter = {}

EntityTypes.FpsCounter.Initialize = function(entity)

    local UpdateFpsCounterPos = function(v, a)

		Logging:Info(tostring(a))

	    Logging:Info("Set FPSPosition")
		entity:SetPosition(-a.Width / 2.0, -a.Height / 2.0)
    end

	local hudViewport = Entities:GetHudViewport()

	Triggers:OnEntityViewportResized(hudViewport, UpdateFpsCounterPos);
	Logging:Info("Initialize")

end

EntityTypes.FpsCounter.UpdateText = function(entity)

	local fps = Rendering.Fps
	local text = "FPS: " .. string.format("%.2f", fps)

	entity:SetText(0, text)
end