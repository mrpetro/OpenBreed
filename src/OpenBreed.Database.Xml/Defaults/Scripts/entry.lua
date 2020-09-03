
WorldLoaded = function(World world)
	camera = Entities.GetByName("MainCamera")

	logo = Entities.CreateFromTemplate("Logo1");
	world.Add(logo);

	Trigger.Add(Camera.Events.FadeIn, camera, Logo1FadedIn)
	camera.FadeIn(DateTime.Seconds(1))
end

Logo1FadedIn = function()
	Trigger.Remove(Logo1FadedIn)
	world.Remove(logo);

	Trigger.OnEvent(Camera.Events.FadeOut, camera, Logo1FadedOut)
	camera.FadeOut(DateTime.Seconds(1))
end

Logo1FadedOut = function()
	Trigger.Remove(Logo1FadedOut)
	world.Remove(logo);

    logo = Entities.CreateFromTemplate("Logo2");
	world.Add(logo);

	Trigger.OnEvent(Camera.Events.FadeIn, camera, Logo2FadedIn)
	camera.FadeIn(DateTime.Seconds(1))
end

Logo2FadedIn = function()
	Trigger.Remove(Logo2FadedIn)
	world.Remove(logo);

	Trigger.OnEvent(Camera.Events.FadeOut, camera, Logo2FadedOut)
	camera.FadeOut(DateTime.Seconds(1))
end

Logo2FadedOut = function()
	Trigger.Remove(Logo2FadedOut)
	world.Remove(logo);
end