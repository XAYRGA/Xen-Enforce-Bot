--- TEMPORARY FILTER SYSTEM CODE! ALL IS CURRENTLY HAX. 
FilterSys = {} 
FilterSys.Entries = {}


FILTER = {} 
dofile("./xen/filters/attackmode.lua")
FilterSys.Entries[1] = FILTER 

FILTER = {} 
dofile("./xen/filters/kicknohandle.lua")
FilterSys.Entries[2] = FILTER 

FILTER = {} 
dofile("./xen/filters/kicknoicons.lua")
FilterSys.Entries[3] = FILTER 

FILTER = {} 
dofile("./xen/filters/phraseban.lua")
FilterSys.Entries[4] = FILTER 

FILTER = {} 
dofile("./xen/filters/kickearlymedia.lua")
FilterSys.Entries[5] = FILTER 

FILTER = {} 
dofile("./xen/filters/verifymute.lua")
FilterSys.Entries[6] = FILTER 

FILTER = {} 
dofile("./xen/filters/captcha.lua")
FilterSys.Entries[7] = FILTER 

--gc, msg, VFD, doubt

function FilterSys.NewMember(gc, msg, VFD, doubt)  
	for pos,flt in pairs(FilterSys.Entries) do   
		local ok,res = pcall(flt.NewUser,flt,msg.from,msg.chat,msg,gc,VFD,doubt) 
		print(ok,res)
		if (res==true) then 
			return
		end 
		if (ok==false) then 
			msg:replySendMessage("An exception has occured executing this filter:\n\nPrimary Stack header: " .. tostring(res) .. "\n\n" .. debug.traceback() .. "\n\nSecondary State Stack:\n------XEN-ENFORCE-BOT\n" .. Helpers.getStack() .. "\n\nThis is not supposed to happen, nor are you supposed to see this message if the bot is running in production mode. Please contact the bot's administrator!")
			return
		end
	end 
end 
modhook.Add("NewChatMember","Filter",FilterSys.NewMember)



function FilterSys.OnMessage(gc, msg, VFD, doubt)  
	for pos,flt in pairs(FilterSys.Entries) do   
		local res,data = flt:OnChatMessage(msg.from,msg.chat,msg,gc,VFD,doubt)
		if (res==true) then 
			return
		end 
	end 
end 
modhook.Add("OnTextMessage","Filter",FilterSys.OnMessage)


function FilterSys.OnRawMessage(gc, msg, VFD, doubt)  
	for pos,flt in pairs(FilterSys.Entries) do   
		if flt.OnRawMessage then 
			local res,data = flt:OnRawMessage(msg.from,msg.chat,msg,gc,VFD,doubt)
			if (res==true) then 
				return
			end 
		end
	end 
end 
modhook.Add("OnRawMessage","Filter",FilterSys.OnRawMessage)