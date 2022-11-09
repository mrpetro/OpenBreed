local function onUpdate(entity, dt)

	local fps = Rendering.Fps
	local text = "FPS: " .. string.format("%.2f", fps)

	entity:SetText(0, text)

end

local function onInit(entity)

	local hudViewport = Entities:GetHudViewport()

	Triggers:OnEntityViewportResized(hudViewport, updateFpsCounterPos)
	Logging:Info("Initialize")

end

local updateFpsCounterPos = function(v, a)

	Logging:Info(tostring(a))

	Logging:Info("Set FPSPosition")
	entity:SetPosition(-a.Width / 2.0, -a.Height / 2.0)
end

return {
    systemHooks = {
        onUpdate = onUpdate,
        onInit = onInit
    }
}