COMMAND.name = "/xeattackenable"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt) 
	gc:modify("attackmode",true)
	msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "feature/attackOn")) 
	gc.invalidated = true
end 