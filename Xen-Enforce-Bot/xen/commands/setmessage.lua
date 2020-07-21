COMMAND.name = "/xesetmessage"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt, args) 
	if (not args[1]) then 
		--msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "basic/commands/badArgs")) 
		msg:replyLocalizedMessage(gc:getString("language"),"basic/commands/badArgs")

		return 
	end 
	local txmsg = table.concat(args," ")
	if (not string.find(txmsg,"ACTURL")) then 
		--msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "config/messageHelp",args[1])) 
		msg:replyLocalizedMessage(gc:getString("language"),"config/messageHelp",args[1])
		return
	end 
	gc:modify("verifyask",txmsg)
	--msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "config/messageSet",args[1])) 
	msg:replyLocalizedMessage(gc:getString("language"),"config/messageSet",args[1])
	gc.invalidated = true
end 