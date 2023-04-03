local function Explode(mineEntity, actorEntity, projection)

	local stampId = Stamps:GetByName("Vanilla/L1/MineCrater").Id

	mineEntity:PutStamp(stampId, 0)

	local soundId = Sounds:GetByName("Vanilla/Common/LandMine/Explosion")
	local duration = Sounds:GetDuration(soundId)
	mineEntity:EmitSound(soundId)

	mineEntity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
		:SetOption("flavor", "Big")
		:Finish()

	for i = -1,1,1 
	do
		for j = -1,1,1 
		do
			local nextDoorCell = mineEntity:GetEntityByDataGrid(Worlds, i, j)

			Factory:CreateSlowdown(Worlds, mineEntity, mineEntity.WorldId, i, j)

			if(nextDoorCell ~= nil)
			then
				Worlds:RequestRemoveEntity(nextDoorCell)
				Entities:RequestDestroy(nextDoorCell)
			end

		end
	end

	Logging:Info("Boom!")
end

local function CheckExplode(mineEntity, actorEntity)

    local metadata = actorEntity:GetMetadata()

	if(metadata.Name ~= "Actor") then
		return
	end

	Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

	Explode(mineEntity, actorEntity)
end

return {
    systemHooks = {
        OnCollision = CheckExplode
    }
}



