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
		local cmdLookup = Commands.CommandTable[mtext[1]]
		table.remove(mtext,1)
		if (cmdLookup~=nil) then 
			if cmdLookup.RequireAdmin then 
				if (msg:isSenderAdmin()) then 
					cmdLookup:Execute(gc,msg,VFD,doubt,mtext)
				else 
					msg:replySendMessage(Localization.getStringLocalized(gc:getString("language"), "basic/commands/noPermission"))
				end
			end 
		end 
	end 
end 

modhook.Add("OnTextMessage","commands",OnMessage)