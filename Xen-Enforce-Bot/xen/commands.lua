Commands = {}
Commands.CommandTable = {}


local a = file.Find("./xen/commands/","*.lua")
for k,v in pairs(a) do
	COMMAND = {} 
	print("Loaded command" .. v)
	dofile(v)
	Commands.CommandTable[COMMAND.name] = COMMAND;
end 


local function OnMessage(gc,msg, VFD, doubt)
	local mtext = string.explode(msg.text)
	local lc 

	if mtext~=nil then 
		mtext[1] = Helpers.quickFormat(mtext[1], "@" .. root.botUsername, "") -- strips bot username from bot commands
		print(mtext[1])
		local cmdLookup = Commands.CommandTable[mtext[1]]
		table.remove(mtext,1)
		if (cmdLookup~=nil) then 
			if cmdLookup.RequireAdmin then 
				if (msg:isSenderAdmin()) then 
					local s,e = pcall(function()  
							cmdLookup:Execute(gc,msg,VFD,doubt,mtext)
					end)
					if s==false then 
						msg:replySendMessage("An exception has occured executing this command:\n\nPrimary Stack header: " .. e .. "\n\n" .. debug.traceback() .. "\n\nSecondary State Stack:\n------XEN-ENFORCE-BOT\n" .. Helpers.getStack() .. "\n\nThis is not supposed to happen, nor are you supposed to see this message if the bot is running in production mode. Please contact the bot's administrator!")
					end

				else 
					msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "basic/commands/noPermission"))
				end
			end 
		end 
	end 
end 
modhook.Add("OnTextMessage","commands",OnMessage)