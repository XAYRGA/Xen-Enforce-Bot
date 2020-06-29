FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "Captcha"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000


function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	
	local apiEndpoint = root.Config.getValue("APIEndpoint")
	local challengeData = user.id .. chat.id
	local actURL = string.format(apiEndpoint,challengeData)
	local delay = config:getInt("verifydelay") 

	local text = Localization.getStringLocalized(config:getString("language"), "feature/captcha/askVerify",Helpers.getMentionName(user),


	local actMessage = Telegram.sendMessage(chat,"Test! Activation instance blah")
	Verify.addInstance(user, chat, actMessage,config,"FoxyWoxy", delay ,message)

	return true
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)


end 

