FILTER.Author = "@xayrga"
FILTER.Desc = "Xen Enforce Bot -- CAPTCHA Filter"
FILTER.Version = 1.0
FILTER.Name = "Captcha"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000
-- Captcha filter for Xen Enforce Bot 4 


function FILTER:NewUser(user, chat, message, config, verifyData, doubt, from)
	
	Verify.doCaptcha(user, chat, config ,message)
	return true
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)


end 

