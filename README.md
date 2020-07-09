Hey-o!

I'm testing a new core for Xen Enforce Bot, with hopefully, a lot of new features. 

__THIS IS NOT AN UPDATE TO THE OFFICIAL @XENFBOT__  

The official @xenfbot will remain the same until this version is deemed stable. 

This is an update to the beta of xenfbot, which can be used at @xenfbetabot

**Please note, that the beta of this bot may experience crashes, has the possibility to ban users from your chat with false-positives, and is out there for chats who want to help test new features for bot-stopping goodness. **

By default, all features (except the CAPTCHA) come disabled. 

Features can be enabled with the following command:

`/xenablefilter <filter name>`  | enables filter
`/xdisablefilter <filter name>`  | disables filter

so an example would be `/xenablefilter kickunverifiedmedia` 

Here's a list of available filter names: 

* `kickunverifiedmedia` | Will kick somebody if they post URL or media before the verification was completed 
* `kickblacklised` | Will kick somebody automatically if they are on Xenfbot's global ban list (Currently, there are no entries on the global ban list. This list is reserved for spambots, and dangerous individuals only. STRONG consideration will be taken before adding anybody to this list. This feature is DISABLED by default. I will __NEVER__ kick people from your chat for something you did not explicitly enable.)
* `kicknohandle` | Will kick somebody who joins without a handle 
* `kicknoicons` | Will kick somebody who joins with no icons in their profile. 
* `phraseban` | Xenfbot contains a list of common spam phrases bots will say. These phrases are very specific, and if it bans for it, it's likely that you copy-pasted botspam. Again, i'm not here to dictate your chats for you, just to stop bots. This feature is disabled by default, you must explicitly enable it. 
* `verifymute` | Mutes a user until they complete verification. 

Some other configuration commands:

`/xesetmessage <message>` | Will set the message displayed upon join, must contain `%ACTURL`. 

`/xesetverifytime <time>` | sets the time a user has to verify in minutes 

`/xeattackenable` | Enables attack mode, kicks all new joins with no prompt, will delete / cleanup all join messages from new joins. 

`/xeattackdisable` | Disables attack mode, accepts new joins. 

`/xesetlang <language code>` | A language code is for example `en`, `de`, `es`, `ru`. Currently, only supporting english, but there's room for other translations. 

