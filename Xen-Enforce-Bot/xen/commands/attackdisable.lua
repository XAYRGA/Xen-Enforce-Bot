COMMAND.name = "/xeattackdisable"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt) 
	gc:modify("attackmode",false)
	msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "feature/attackOff")) 
	gc.invalidated = true
end 