FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "KickEarlyMedia"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000

function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	return false
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)

end 

local function tripFilter(user, chat, message, config, verifyData, doubt)

	local msg = message:replySendMessage(Localization.getStringLocalized(config:getString("language"), "feature/kickEarlyMedia/userKicked",user.first_name)) 
		Cleanup.addMessage(msg)
	message:delete() 
	--Telegram.kickChatMember(chat,user,500)
end

function FILTER:OnRawMessage(user, chat, message, config, verifyData, doubt)
	if (config:getBool("kickunverifiedmedia")==false) then -- don't continue if the filter is disabled for this chat
		return false 
	end
	if verifyData==nil then 
		return false
	end
	if verifyData:getBool("verified")==true then 
		return true
	end
	local text = message.text
	if (text~=nil) then 
		text = string.lower(text)
		if string.find(text,"http://") or string.find(text,"https://") or string.find(text,".com") then  
			tripFilter(user, chat, message, config, verifyData, doubt)
			return true
		end 
	end
	if (message.photo~=nil) then 
		tripFilter(user, chat, message, config, verifyData, doubt)
		return true
	end
end