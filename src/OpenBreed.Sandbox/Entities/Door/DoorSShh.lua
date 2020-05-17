Templates.Entities.DoorHorizontal = {
    WorldComponent = {},
    ClassComponent = { "DoorHorizontal" },
    TimerComponent = {},
    FsmComponent = { { "Door.Functioning" , "Closed"} },
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
		{T = "This "},
		{T = "Door ", },
		}
}