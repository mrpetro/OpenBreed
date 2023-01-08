local function onUpdate(entity, dt)

	local pos = entity:GetPosition()
	local text = "(" .. string.format("%.0f", pos.X) .. ", " .. string.format("%.0f", pos.Y) .. ")"
	entity:SetText(0, text)

end

local function onInit(entity)

end

return {
    systemHooks = {
        onUpdate = onUpdate,
        onInit = onInit
    }
}