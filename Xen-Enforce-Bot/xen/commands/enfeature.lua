COMMAND.name = "/xenablefilter"
COMMAND.RequireAdmin = true 
COMMAND.AllowedFeatures = {
	["attackmode"] = true, 
	["smartdetect"] = true,
	["phraseban"] = true,
	["kicknohandle"] = true,
	["kicknoicons"] = true,
	["kickblacklisted"] = true,
	["kickunverifiedmedia"] = true,
	["verifyannounce"] = true,
	["verifymute"] = true,
	["mediadelay"] = true,
	["dontdeletejoinmessage"] = true
}

function COMMAND:Execute(gc,msg,VFD,doubt, args) 
	if (not args[1]) then 
		--msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "basic/commands/badArgs")) 
		msg:replyLocalizedMessage(gc:getString("language"),"basic/commands/badArgs")
		return 
	end 
	args[1] = string.lower(args[1])
	if (not self.AllowedFeatures[args[1]]) then 
		--msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "config/featureNotExist",args[1])) 
		msg:replyLocalizedMessage(gc:getString("language"),"config/featureNotExist",args[1])
		return
	end 
	gc:modify(args[1],true)
	msg:replyLocalizedMessage(gc:getString("language"),"config/featureEnabled",args[1])
	gc.invalidated = true
end 