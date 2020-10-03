modhook = {}
local hooktab = {}
modhook.Tb = hooktab

function modhook.Add(hk,uname,func)
	if not hk then return false,"NO EVENT NAME" end
	if not uname then return false,"NO UNIQUE NAME" end
	if not func then return false, "NO CALLBACK FUNC!" end
	if not hooktab[hk] then hooktab[hk] = {} end
	hooktab[hk][uname] = func
end

function modhook.Call(hk,...)
	if not hk then return false,"NO EVENT NAME" end
	local mk = hooktab[hk]
	if mk then 
		for k,v in pairs(mk) do 
			local s,e = pcall(v,...) 
			if not s then 
				print(e or "")
			end 
		end
	end
end

function modhook.Remove(hk,uname)
	if not hk then return false,"NO EVENT NAME" end
	if not uname then return false,"NO UNIQUE NAME" end
	
	if not hooktab[hk] then hooktab[hk] = {} end
	hooktab[hk][uname] = nil 
end

function string.explode(inputstr, sep)
        if sep == nil then --// check for separator 
                sep = "%s" --// if we don't have the separator assume we're looking for %s 
        end 
        local t={} ; i=1 --// storage table and index 
        for str in string.gmatch(inputstr, "([^"..sep.."]+)") do --// match regex pattern 
                t[i] = str --// store in set index on find 
                i = i + 1 --// Increment
        end
        return t
end
