Templates.Entities.TeleportExit = {
    AnimationComponent = {
        Speed = 10.0,
        Loop = true,
		AnimId = "Animations/Teleport/Entry"},
    PositionComponent = { 0, 0 },
	SpriteComponent = {
		AtlasId = "Atlases/Sprites/Teleport/Exit",
        Order = 0.0,
        Width = 32.0,
        Height = 32.0},
	TextComponent = {
		Text = "TeleportExit",
		Offset = { 0, 32 },
		Color = { 255, 255, 255, 255 },
		FontId = { "Arial", 9 },
        Order = 0.0}
}