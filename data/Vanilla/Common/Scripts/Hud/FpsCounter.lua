
local updateFpsCounterPos
local fpsCounter

local function OnUpdate(entity, dt)

	local fps = Rendering.Fps
	local text = "FPS: " .. string.format("%.2f", fps)

	entity:SetText(0, text)

end

local function OnInit(entity)

	local hudViewport = Entities:GetHudViewport()

	Triggers:OnEntityViewportResized(hudViewport, updateFpsCounterPos)
	Logging:Info("Initialize")
	fpsCounter = entity;
end

updateFpsCounterPos = function(v, a)
	--Logging:Info("Width: " .. tostring(a.Width))
	--Logging:Info("Height: " .. tostring(a.Height))
	--Logging:Info("Set FPSPosition")
	fpsCounter:SetPosition(-a.Width / 2.0, -a.Height / 2.0)
end

return {
    systemHooks = {
        OnUpdate = OnUpdate,
        OnInit = OnInit
    }
}