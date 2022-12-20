local function Explode(mineEntity, actorEntity)

	Logging:Info("MineEntityId:" .. tostring(mineEntity.Id))
	Logging:Info("ActorEntityId:" .. tostring(actorEntity.Id))

	local stampId = Stamps:GetByName("Vanilla/L1/MineCrater").Id

	mineEntity:PutStamp(stampId, 0)

	local soundId = Sounds:GetByName("Vanilla/Common/LandMine/Explosion")
	local duration = Sounds:GetDuration(soundId)
	mineEntity:EmitSound(soundId)	



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

	--Worlds:RequestRemoveEntity(mineEntity)
	--Entities:RequestDestroy(mineEntity)
	Logging:Info("Boom!")
end

return {
    systemHooks = {
        ScriptRunTrigger = Explode
    }
}



