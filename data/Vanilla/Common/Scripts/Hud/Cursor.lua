﻿local function OnUpdate(entity, dt)

	local pos = entity:GetPosition()
	local text = "(" .. string.format("%.0f", pos.X) .. ", " .. string.format("%.0f", pos.Y) .. ")"
	entity:SetText(0, text)

end

local function OnInit(entity)

    Triggers:OnCursorKeyPressed(
        entity,
        Fire,
        false)
end

Fire = function(entity, args)
    --entity:Emit("ABTA\\Templates\\Common\\Bullet")
end

SetupNew = function(entity, args)
    Logging:Info("Setting up new...")
end

return {
    systemHooks = {
        OnUpdate = OnUpdate,
        OnInit = OnInit
    }
}