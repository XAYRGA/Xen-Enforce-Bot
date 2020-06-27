FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "KickNoIcons"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000


function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	if (config:getBool("kicknoicons")==false) then -- don't continue if the filter is disabled for this chat
		--print("Filter not enabled???")
		return false 
	end
	local icons = getNumProfilePhotos(user)
	if (icons==nil) then 
		return -- possibly bad
	end
	if (icons < 1) then 
		local msg = message:replySendMessage(Localization.getStringLocalized(config:getString("language"), "feature/kickNoIcons/userKicked",user.first_name)) 
		Cleanup.addMessage(message)
		Cleanup.addMessage(msg)
		Telegram.kickChatMember(chat,user,120)
		return true
	end
	return false
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)

end 

