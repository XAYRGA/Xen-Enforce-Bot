COMMAND.name = "/xetest"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt) 
	msg:replySendMessage("Test ok!");
end 