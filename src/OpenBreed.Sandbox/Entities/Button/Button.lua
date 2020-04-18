﻿Templates.Entities.DoorHorizontal = {
    AnimationComponent = {
        Speed = 5.0,
        Loop = false},
    PositionComponent = { 0, 0 },
    BodyComponent = {
        CofFactor = 1.0,
        CorFactor = 1.0,
        Type = "Static",
		Fixtures = { "Fixtures/DoorHorizontal" } },
	SpriteComponent = {
		AtlasId = "Atlases/Sprites/Door/Horizontal",
        Order = 0.0},
	TextComponent = {
		Text = "Door",
		Offset = { -10, 10 },
		Color = { 255, 255, 255, 255 },
		FontId = { "Arial", 9 },
        Order = 0.0}
}