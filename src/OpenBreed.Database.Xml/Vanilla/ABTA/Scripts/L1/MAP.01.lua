local function Welcome(director, playerCharacterEntity)

	PlaySound = function(sampleName)
		local soundId = Sounds:GetByName(sampleName)
		local duration = Sounds:GetDuration(soundId)
		Commentator:EmitSound(soundId)	
		return duration
	end

	SayNash = function()
		local delay = PlaySound("Vanilla/Common/Speech/Nash")
		local soundId = Sounds:GetByName("Vanilla/Common/Speech/Nash")
		Triggers:AfterDelay(Commentator, TimeSpan.FromMilliseconds(delay), SayIsDead)
	end

	SayIsDead = function()
		local delay = PlaySound("Vanilla/Common/Speech/IsDead")
		local soundId = Sounds:GetByName("Vanilla/Common/Speech/IsDead")
		Triggers:AfterDelay(Commentator, TimeSpan.FromMilliseconds(delay), SayYoureOnYourOwn)
	end

	SayYoureOnYourOwn = function()
		local delay = PlaySound("Vanilla/Common/Speech/YoureOnYourOwn")
		local soundId = Sounds:GetByName("Vanilla/Common/Speech/YoureOnYourOwn")
	end

	SayNash()
end

return {
    systemHooks = {
        OnStartMission = Welcome
    }
}

