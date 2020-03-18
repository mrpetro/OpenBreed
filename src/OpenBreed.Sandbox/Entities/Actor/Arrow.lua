﻿Templates.Entities.Arrow = {
    AnimatorComponent = {
        Speed = 10.0,
        Loop = true},
    PositionComponent = { 0, 0 },
    ThrustComponent = { 0, 0 },
    VelocityComponent = { 0, 0 },
    DirectionComponent = { 0, 0 },
    BodyComponent = {
        CofFactor = 1.0,
        CorFactor = 0.0,
        Type = "Dynamic",
		Fixtures = { "Fixtures/Arrow" } },
	MotionComponent = {},
	SpriteComponent = {
		AtlasId = "Atlases/Sprites/Arrow",
        Order = 100.0},
	TextComponent = {
		Text = "Hero",
		Offset = { 0, 32 },
		Color = { 255, 255, 255, 255 },
		FontId = { "Arial", 9 },
        Order = 0.0}
}