
WorldLoaded = function(world)

	Logging:Info("Loaded")
    
	ts = TimeSpan.FromSeconds(2)

	Logging:Info(ts:ToString())

	Triggers:AfterDelay(ts, function()
		Logging:Info("Triggered!!!!!!!!!!!!")
	end)

    soundId = Sounds:GetByName("Vanilla/Common/Speech/Nash")
	Sounds:PlaySample(soundId)
    soundId = Sounds:GetByName("Vanilla/Common/Speech/IsDead")
	Sounds:PlaySample(soundId)
    soundId = Sounds:GetByName("Vanilla/Common/Speech/YoureOnYourOwn")
	Sounds:PlaySample(soundId)
end

