CODE	en
AUTHOR	@XAYRGA
VERSION	1.1
*******************************************************


#THIS IS SPECIFIC TO THE LANGUAGE
locale/languageChanged|The language has successfully been changed to English.
locale/currentLang|The current language is 'English'
locale/currentLangName|English
locale/sentence|The quick brown fox jumps over the lazy dog. 

#VERIFY 
verify/userVerified|{0} has successfully completed verification. 
verify/userKicked|{0} was removed from the chat because they failed to verify.
verify/userKickedDoubt| {0} was removed from the chat -- if they're not a bot, they can rejoin in two minutes. 
verify/manualVerifyFail|You must reply to a message to use this function.
#INFO 
info/github|https://github.com/XAYRGA/Xen-Enforce-Bot/tree/Bone-Hurting-Juice

#BASIC
basic/error/noPermission|I don't have permission to perform %s, make sure you've given me permission to do this, or disable the feature %s
basic/welcome|Welcome to Xen Enforce bot! Please take a moment to read the instructions on the bot, or visit the github for more information
basic/xenfbot|Xen Enforce Bot v{0} by https://github.com/XAYRGA/Xen-Enforce-Bot/tree/Bone-Hurting-Juice\n\nRunning translation for `{1}` version [{3}] by {2} \n\nCurrent bot contact: {4}.
basic/words/admin|Administrator
basic/words/manual|Manual
basic/commands/noPermission|Sorry, you don't have permission to perform that action. 
basic/commands/ok|Command completed successfully. 
basic/commands/commandError|I ran into an error running this command, please report this with the following tag: {0}
basic/commands/badArgs|Invalid arguments. Check the readme, commands usually have information that follows them, for example, "/xesetverifytime 60"

#CONFIG
config/success|Successfully changed {0} to {1}
config/dontUnderstandNumber|I couldn't understand the value {0}, it should be a number.
config/somethingWrong|For some reason, I couldn't save the configuration. Contact my developer.
config/featureDisabled|Successfully disabled the feature '{0}'.
config/featureEnabled|Successfully enabled the feature '{0}'.
config/featureNotExist|The feature '{0}' doesn't exist.
config/languageNotSupported|Sorry, I don't support the language '{0}' yet.
config/kicktimeChanged|Successfully changed the verification time to {0} minutes.
config/mediaTimeChanged|Successfully changed the media delay to {0} hours.
config/messageHelp|The attribute %ACTURL must appear in this message. You can also have %NAME and %DURATION.
config/messageSet|The join message has been successfully changed.

#FEATURES 
feature/attackOn|Attack mode ENABLED. No new members will be accepted. 
feature/attackOff|Attack mode DISABLED. New joins will no longer be kicked. 
feature/attackMode/userKicked|{0} was removed from the chat -- attack mode is enabled, no new joins are accepted. 

feature/kickNoIcons/userKicked|{0} was removed from the chat as they don't have any icons and 'kicknoicons' is turned on.
feature/kickNoHandle/userKicked|{0} was removed from the chat as they don't have a handle/username and 'kicknohandle' is turned on.
feature/kickEarlyMedia/userKicked|{0} was removed from the chat for posting media before they verified.
feature/mediaDelay|{0}, you must wait {1} hours to post media after you've verified.
feature/mediaDelayWarn|{0}, you must verify before you can post media. 

captcha/userWelcome|Welcome {0}, to the chat!\n\nPlease take a moment to verify that you're not a bot by completing a quick CAPTCHA\n\nYou have {1} minutes to verify, or else you'll be kicked from the chat.\n\nYou can verify here {2}

#CAPTCHA 


#AUTOREM 
autorem/removedBecauseBot|{0} was removed from the chat because they look like a bot. 
autorem/sorry|Hey %s, sorry for kicking you. Your profile looks a lot like a bot, and I couldn't take any chances. You can try the following:\n1. Add a / additional profile icons\n2. Add an @handle to your profile if you don't have one.\n3. Lengthen your name.



