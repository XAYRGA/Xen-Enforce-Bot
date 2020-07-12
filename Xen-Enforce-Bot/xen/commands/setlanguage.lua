COMMAND.name = "/xesetlang"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt, args) 
	if (not args[1]) then 
		msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "basic/commands/badArgs")) 
		return 
	end 
	args[1] = string.lower(args[1])
	if not Localization.supports(args[1])  then 
		msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "config/languageNotSupported"),args[1]) 
		return 
	end 

	gc:modify("language",args[1])
	msg:replySendMessage(Localization.getStringLocalized(args[1], "locale/languageChanged")) 
	gc.invalidated = true
end 