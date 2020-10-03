FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "KickNoHandle"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000


function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	if (config:getBool("kicknohandle")==false) then -- don't continue if the filter is disabled for this chat
		return false 
	end
	if (user.username==nil) then 
		--local msg = message:replySendMessage(Localization.getStringLocalized(config:getString("language"), "feature/kickNoHandle/userKicked",user.first_name)) 
		local msg = message:replyLocalizedMessage(config:getString("language"),"feature/kickNoHandle/userKicked",user.first_name)
		Cleanup.addMessage(message)
		Cleanup.addMessage(msg)
		Removals.addIncident(user,chat,"KICKNOHANDLE")
		Telegram.kickChatMember(chat,user,120)
		return true
	end
	return false
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)

end 

