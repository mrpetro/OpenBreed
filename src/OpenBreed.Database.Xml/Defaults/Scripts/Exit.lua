
WorldLoaded = function(World world)
	camera = Entities.GetByName("MainCamera")

	logo = Entities.CreateFromTemplate("Logo1");
	world.Add(logo);

	Trigger.Add(Camera.Events.FadeOut, camera, ScreenFadedOut)
	camera.FadeIn(DateTime.Seconds(1))
end

ScreenFadedOut = function()
	Trigger.Remove(ScreenFadedOut)
	world.Remove(logo);

	Trigger.OnEvent(Camera.Events.FadeOut, camera, Logo1FadedOut)
	camera.FadeOut(DateTime.Seconds(1))
end

