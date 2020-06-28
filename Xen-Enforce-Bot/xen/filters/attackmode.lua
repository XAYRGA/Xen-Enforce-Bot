FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "AttackMode"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000


function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	if (config:getBool("attackmode")==false) then -- don't continue if the filter is disabled for this chat
		--print("Filter not enabled???")
		return false 
	end
	local msg = message:replySendMessage(Localization.getStringLocalized(config:getString("language"), "feature/attackMode/userKicked",user.first_name)) 
	Removals.addIncident(user,chat,"ATTACKMODEON")
	Cleanup.addMessage(msg)
	Telegram.kickChatMember(chat,user,360)
	return true
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)

end 

