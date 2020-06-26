--- TEMPORARY FILTER SYSTEM CODE! ALL IS CURRENTLY HAX. 
FilterSys = {} 
FilterSys.Entries = {}

FILTER = {} 
dofile("./xen/filters/captcha.lua")
FilterSys.Entries[1] = FILTER 

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