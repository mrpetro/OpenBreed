local function onUpdate(entity, dt)

	local pos = entity:GetPosition()
	local text = "(" .. string.format("%.0f", pos.X) .. ", " .. string.format("%.0f", pos.Y) .. ")"
	entity:SetText(0, text)

end

local function onInit(entity)

    Triggers:OnCursorMoved(
        entity,
        Print1,
        false)

    Triggers:OnCursorKeyPressed(
        entity,
        Print2,
        false)

end

Print1 = function(cursor, args)
    Logging:Info("Moved")
end

Print2 = function(cursor, args)
    Logging:Info("Pressed")
end

return {
    systemHooks = {
        onUpdate = onUpdate,
        onInit = onInit
    }
}