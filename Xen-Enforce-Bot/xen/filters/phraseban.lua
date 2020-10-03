FILTER.Author = "@xayrga"
FILTER.Version = 1.0
FILTER.Name = "PhraseBan"
FILTER.DefaultEnabled = false 
FILTER.Requires = {}
FILTER.Priority = 1000

local badPhrases = { 
	----01 
	[[This making it even harder for a lgbt relationship to succeed as gay standards are also higher than straights]],
	[[Most lgbt people are incels for this reason]],
	----02
	[[Did you know that Johawk lied about coming to see me]],
	[[I opened up so much for you you little faggot]], 
	--02.1
	[[When I spent a night at an inpatient facility this ass didn’t even care his bf was there when I saw]],
	[[at a time he had feelings for me too back the guy would hide my ass]], 
	[[after retardbird I was interested in used me for sex and dumped]],
	--02.2
	[[A man who you think the world of saying he wants to take you to the finest restaurant in the state is something else]],
	[[Then you know that doesn’t happen and you end up getting told by another furry you randomly found on ferzu]],
	[[the reason I dumped contact with him when we were just friends was this little fucker would ignore my YouTube links]],
	--02.3 
	[[His name is Dylan Phillips from Michigan]],
	[[a crazy psycho brother and lives with his mom]]
	----03

}


function FILTER:NewUser(user, chat, message, config, verifyData, doubt)
	return false
end 

function FILTER:OnChatMessage(user, chat, message, config, verifyData, doubt)
	if (config:getBool("phraseban")==false) then -- don't continue if the filter is disabled for this chat
		return false 
	end
	for k,v in pairs(badPhrases) do 
		if string.find(string.lower(message.text),string.lower(v)) then  
			local msg = message:replySendMessage("Applied permanent ban for " .. user.id .. ", tripped filter 'PhraseBan' subindex " .. k .. " -- pushing into global blacklist.\n\nThis filter is experimental, which is why this filter is not localized. If this is a false-positive, please let the developer know. This message will not be removed.")
			--Cleanup.addMessage(msg)
			Removals.addIncident(user,chat,"KICKSPAMPHRASE")
			Telegram.kickChatMember(chat,user,0)
			message:delete()
			return true 
		end 
	end 
end 

