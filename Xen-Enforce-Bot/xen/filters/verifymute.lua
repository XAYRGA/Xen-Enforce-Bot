FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "VerifyMute"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000



local function FILTERCUSTOM_onUserVerified(thc, thm, groupID, GCO) 
	if (GCO:getBool("verifymute")==false) then  -- skip if filter is not enabled
		return false
	end 
	Telegram.restrictChatMember(thc,thm, 0, true, true, true, true);
end 
modhook.Add("verUserVerifiedNotify","FilterVerifyMute",FILTERCUSTOM_onUserVerified)


function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	if (config:getBool("verifymute")==false) then -- don't continue if the filter is disabled for this chat
		--print("Filter not enabled???")
		return false 
	end
	print(chat,user,config,message)
	Telegram.restrictChatMember(chat,user, 0, false, false, false, false);
	return false
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)

	if (config:getBool("verifymute")==false) then -- don't continue if the filter is disabled for this chat
		return false 
	end

	if (verifyData==nil) then -- user has no verifydata, don't delete their stuff.
		return false  
	end 

	if (message.new_chat_members~=nil) then -- don't delete the join message if the filter is enabled
		return false  
	end 

	if (verifyData:getBool("verified")==false) then  
		message:delete() -- Delete their message if they're not verified
	end 
end 

