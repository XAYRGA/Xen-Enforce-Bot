FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "Captcha"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000


function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	
	local apiEndpoint = Config.getValue("APIEndpoint") -- NOTE: Capital config, gets the member from the C# state for config. 
	local challengeData = Helpers.Base64Encode(user.id .. chat.id)
	--print(challengeData)
	local actURL = string.format(apiEndpoint,challengeData)
	local delay = config:getInt("verifydelay") 
	local CustomText = config:getString("verifymessage")
	local UserName = Helpers.getMentionName(user)
	local text = Localization.getStringLocalized(config:getString("language"), "captcha/userWelcome",UserName,delay,actURL)
	if (CustomText~=nil) then 
		if #CustomText > 10 then 
			CustomText = Helpers.quickFormat(CustomText,"%NAME",UserName)
			CustomText = Helpers.quickFormat(CustomText,"%ACTURL",actURL)
			CustomText = Helpers.quickFormat(CustomText,"%DURATION",delay)
			text = CustomText
		end 
	end
	local actMessage = Telegram.sendMessage(chat,text)
	Verify.addInstance(user, chat, actMessage,config,challengeData, delay ,message)
	return true
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)


end 

