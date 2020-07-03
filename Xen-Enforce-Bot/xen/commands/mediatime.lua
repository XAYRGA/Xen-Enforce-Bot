COMMAND.name = "/xesetmediatime"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt, args) 
	if (not args[1]) then 
		msg:replySendMessage("!") 
		return 
	end 
	local num = tonumber(args[1])

	if (num==nil) then 
		msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "config/dontUnderstandNumber",args[1])) 
		return
	end 
	gc:modify("mediadelaytime",num)
	msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "config/kicktimeChanged",num)) 
	gc.invalidated = true
end 