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
		local res,data = flt:NewUser(msg.from,msg.chat,msg,gc,VFD,doubt)
		if (res==true) then 
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