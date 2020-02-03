Templates.Entities.TeleportEntry = {
    AnimatorComponent = {
        Speed = 10.0,
        Loop = true,
		AnimationAlias = "Animations/Teleport/Entry"},
    PositionComponent = { 0, 0 },
    BodyComponent = {
        CofFactor = 1.0,
        CorFactor = 1.0,
        Type = "Trigger"},
	SpriteComponent = {
		AtlasAlias = "Atlases/Sprites/Teleport/Entry",
        Order = 0.0}
}