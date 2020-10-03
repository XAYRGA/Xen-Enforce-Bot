﻿COMMAND.name = "/xesetverifytime"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt, args) 
	if (not args[1]) then 
		--msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "basic/commands/badArgs")) 
		msg:replyLocalizedMessage(gc:getString("language"),"basic/commands/badArgs")
		return 
	end 
	local num = tonumber(args[1])

	if (num==nil) then 
		--msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "config/dontUnderstandNumber",args[1])) 
		msg:replyLocalizedMessage(gc:getString("language"),"config/dontUnderstandNumber",args[1])
		return
	end 
	gc:modify("verifydelay",num)
	--msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "config/kicktimeChanged",num)) 
	msg:replyLocalizedMessage(gc:getString("language"),"config/kicktimeChanged",args[1])
	gc.invalidated = true
end 
