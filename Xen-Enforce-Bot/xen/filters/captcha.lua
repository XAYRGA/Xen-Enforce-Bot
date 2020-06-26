FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "Captcha"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000

function FILTER:NewUser(user, chat, message, config, verifyData, doubt)

	local actMessage = Telegram.sendMessage(chat,"Test! Activation instance blah")
	Verify.addInstance(user, chat, actMessage,config,"FoxyWoxy", config:getInt("verifydelay"))



	return true
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)


end 

