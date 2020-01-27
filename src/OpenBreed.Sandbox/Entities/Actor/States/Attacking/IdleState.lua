AtackingFsm = {}

AtackingFsm.IdleState = {}

AtackingFsm.IdleState.Enter = function(entity)
	entity.Core.Logging:Verbose("Idle ENTER")
end

AtackingFsm.IdleState.Leave = function(entity)
	entity.Core.Logging:Verbose("Idle LEAVE")
end
