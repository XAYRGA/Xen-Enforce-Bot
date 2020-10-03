COMMAND.name = "/xeverify"
COMMAND.RequireAdmin = true 

function COMMAND:Execute(gc,msg,VFD,doubt, args) 
	if (msg.reply_to_message~=nil) then
		Verify.doTrustUser(msg.reply_to_message.from,msg.chat)
		local name = Helpers.getMentionName(msg.reply_to_message.from);
		msg:replyLocalizedMessage(gc:getString("language"),"verify/userVerified",name)
	else 
		msg:replyLocalizedMessage(gc:getString("language"),"verify/manualVerifyFail",name)
		return false
	end
	--msg:replyLocalizedMessage()
	-- verify/userVerified

end 
