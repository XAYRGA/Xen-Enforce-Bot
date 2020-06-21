CODE	en
AUTHOR	@XAYRGA
VERSION	1.0
*******************************************************
#the *'s after the banner are mandatory.

#THIS IS SPECIFIC TO THE LANGUAGE
locale/languageChanged|The language has successfully been changed to English.
locale/currentLang|The current language is 'English'
locale/currentLangName|English
locale/sentence|The quick brown fox jumps over the lazy dog. 

#INFO 
info/github|http://github.com/XAYRGA/xenfbotdn

#BASIC
basic/error/noPermission|I don't have permission to perform %s, make sure you've given me permission to do this, or disable the feature %s
basic/error/somethingWrong|Something went wrong performing %s, if this is troublesome, contact the developer.
basic/welcome|Welcome to Xen Enforce bot! Please take a moment to read the instructions on the bot, or visit the github for more information
basic/xenfbot|Xen Enforce Bot v{0} by http://github.com/XAYRGA/xenfbotdn\n\nRunning translation for `{1}` version [{3}] by {2} \n\nCurrent bot contact: {4}.
basic/words/admin|Administrator
basic/words/manual|Manual
basic/commands/noPermission|Sorry, you don't have permission to perform that action. 
basic/commands/notFound|The command '%s' wasn't found. 
basic/commands/ok|Command completed successfully. 
basic/commands/commandError|I ran into an error running this command, please report this with the following tag: {0}
basic/config/success|Successfully changed {0} to {1}
basic/config/dontUnderstandNumber|I couldn't understand the value {0}, it should be a number.
basic/config/somethingWrong|For some reason, I couldn't save the configuration. Contact my developer.

#CONFIGURATION ATTRIBUTE DESCRIPTIONS
basic/commands/config/availableCommands|The available configuration attributes are:
basic/commands/config/kickTime|<number minutes> # Configures the amount of time someone has to finish the CAPTCHA. 
basic/commands/config/useNameFilter|<true/false enable> # Enables or disable Xen Enforce Bot's built-in name filter. This will filter out bots with common names. 
basic/commands/config/useBotScreen|<true/false enable> # Enables or disables filtering based on profile completeness 
basic/commands/config/kickMediaBeforeCaptcha|<true/false enable> # Enables or disables the media kick filter. When enabled users will be kicked for posting pictures or links before verifying the captcha. 
basic/commands/config/muteUntilVerified|<true/false enable> # Enables or disables the automute function. This will mute users when they join the chat, then unmute them when they complete the captcha. 
basic/commands/config/announceKicks|<true/false enable> # Enables or disables the announcement of kicks and other administrative removal messages.
basic/commands/config/message|<text message> # Changes the welcome message. Use 'NO' to clear custom message. The message __MUST__ contain %WHO and %LINK ! (Who is replaced by the user, and Link is replaced by the URL for the captcha. %DUR will also be replaced with the amount of time they have to verify)  
basic/commands/config/activationMessage|<text message> # Changes the verification message, optionally, you can use %WHO for the person it's talking to. 

#CAPTCHA 
captcha/captcha|CAPTCHA
captcha/userRemovedMessage|%s was removed from the chat because they didn't complete the captcha in %s minutes. 
captcha/userVerified|%s was verified to not be a spambot.
captcha/userWelcome|Welcome %s to the chat!\nPlease take a moment to verify that you're not a bot by completing a quick CAPTCHA\nYou have %s minutes to verify, or else you'll be kicked from the chat. 
captcha/userUnverified|%s has been unverified and must complete the CAPTCHA again. 

#AUTOREM 
autorem/removedBecauseBot|%s was removed from the chat because they look like a bot. 
autorem/sorry|Hey %s, sorry for kicking you. Your profile looks a lot like a bot, and I couldn't take any chances. You can try the following:\n1. Add a / additional profile icons\n2. Add an @handle to your profile if you don't have one.\n3. Lengthen your name.

#MUTEUNTILVERIFIED 
muvfilt/warning|You will not be able to send messages until you've verified. 
movfilt/manual|%s has been manually unmuted by Xen Enforce Bot.

#FURRYMODE / COMMONNAME
furrymode/rem|%s was removed because their name is that of a common bot. 

