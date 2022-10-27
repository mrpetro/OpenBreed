local fpsEntity = ...

local fps = Rendering.Fps
local text = "FPS: " .. string.format("%.2f", fps)

fpsEntity:SetText(0, text)

