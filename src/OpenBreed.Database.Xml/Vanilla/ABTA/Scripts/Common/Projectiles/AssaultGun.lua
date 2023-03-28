
local function Hit(projectileEntity, targetEntity, projection)

     projectileEntity:StartEmit("Vanilla\\ABTA\\Templates\\Common\\Projectiles\\Explosion.xml")
        :SetOption("flavor", "Small")
        :Finish()
	
     Worlds:RequestRemoveEntity(projectileEntity)
     Entities:RequestDestroy(projectileEntity)
end

local function OnInit(entity)

end

return {
    systemHooks = {
        OnInit = OnInit,
        OnCollision = Hit
    }
}