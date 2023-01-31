local function onUpdate(entity, dt)

	local pos = entity:GetPosition()
	local text = "(" .. string.format("%.0f", pos.X) .. ", " .. string.format("%.0f", pos.Y) .. ")"
	entity:SetText(0, text)

end

local function onInit(entity)

    Triggers:OnCursorKeyPressed(
        entity,
        Fire,
        false)
end

Fire = function(entity, args)
    entity:Emit("Vanilla\\ABTA\\Templates\\Common\\Bullet.xml")
end

SetupNew = function(entity, args)
    Logging:Info("Setting up new...")
end

return {
    systemHooks = {
        onUpdate = onUpdate,
        onInit = onInit
    }
}