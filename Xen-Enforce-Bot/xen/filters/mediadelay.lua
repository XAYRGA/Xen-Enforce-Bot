FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "MediaDelay"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000

function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	return false
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)

end 

function FILTER:OnRawMessage(user, chat, message, config, verifyData, doubt)
	if (config:getBool("mediadelay")==false) then -- don't continue if the filter is disabled for this chat
		return false 
	end
	if verifyData==nil then 
		return false
	end
	if verifyData:getBool("trusted")==true then 
		return false
	end

	local pName = Helpers.getMentionName(user)

	if (verifyData:getBool("verified")==false) then 
			local msg = message:replySendMessage(Localization.getStringLocalized(config:getString("language"), "feature/mediaDelayWarn",pName)) 
			Cleanup.addMessage(msg)
			message:delete() 
		return false 
	end 

	local vTime = verifyData:getLong("tverified"); 
	local iTime =  config:getInt("mediadelaytime")
	if ( (vTime + (60^2) * iTime ) < Helpers.getUnixTime() ) then
		return false
	end

	if (message.photo~=nil or message.video~=nil or message.video_note~=nil or message.document~=nil) then 
		local msg = message:replySendMessage(Localization.getStringLocalized(config:getString("language"), "feature/mediaDelay",pName,iTime)) 
		Cleanup.addMessage(msg)
		message:delete() 		
	end
end

