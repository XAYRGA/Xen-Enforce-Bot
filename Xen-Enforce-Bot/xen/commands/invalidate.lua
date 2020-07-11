COMMAND.name = "/xeinvalidate"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt) 
	msg:replySendMessage("DEVEL01: Cached configuration marked as invalidated :: Configuration will be re-queried from the database on next update.");
	gc.invalidated = true
end 