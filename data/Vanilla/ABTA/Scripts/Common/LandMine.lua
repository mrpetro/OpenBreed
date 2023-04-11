local function Explode(mineEntity, actorEntity, projection)

	local stampId = Stamps:GetByName("Vanilla/L1/MineCrater").Id
    local pos = mineEntity:GetPosition()
	
	mineEntity:PutStamp(stampId, 0)

	local soundId = Sounds:GetByName("Vanilla/Common/LandMine/Explosion")
	local duration = Sounds:GetDuration(soundId)
	mineEntity:EmitSound(soundId)

	mineEntity:StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
		:SetOption("flavor", "Big")
        :SetOption("startX", pos.X + 8)
        :SetOption("startY", pos.Y + 8)
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

    if(actorEntity:HasHealth())
    then
        mineEntity:InflictDamage(10, actorEntity.Id)
    end

	Logging:Info("Boom!")
end

local function CheckExplode(mineEntity, actorEntity)

	if(not(actorEntity:HasHealth())) then
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



