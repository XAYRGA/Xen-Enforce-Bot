CODE	ru
AUTHOR	@ValkaTR
VERSION	1.0
*******************************************************


#THIS IS SPECIFIC TO THE LANGUAGE
locale/languageChanged|Язык успешно изменён на русский.
locale/currentLang|Текущий язык "русский".
locale/currentLangName|Русский
locale/sentence|Съешь же ещё этих мягких французских булок да выпей чаю.

#VERIFY 
verify/userVerified|{0} успешно завершил проверку. 
verify/userKicked|{0} удалён из чата, так как не прошёл проверку.
verify/userKickedDoubt| {0} удалён из чата -- если Вы не бот, то сможете перезайти через две минуты.

#INFO 
info/github|https://github.com/XAYRGA/Xen-Enforce-Bot/tree/Bone-Hurting-Juice

#BASIC
basic/error/noPermission|У меня нет разрешения на выполнение %s, убедитесь, что вы дали мне на это разрешение, или отключите функцию %s.
basic/welcome|Добро пожаловать в бот Xen Enforce! Пожалуйста, найдите время, чтобы прочитать инструкции по боту, или посетите github для получения дополнительной информации.
basic/xenfbot|Xen Enforce Bot v{0} написан https://github.com/XAYRGA/Xen-Enforce-Bot/tree/Bone-Hurting-Juice\n\nВыполняется перевод `{1}` версии [{3}] авторства {2} \n\nКонтактное лицо бота: {4}.
basic/words/admin|Администратор
basic/words/manual|Руководство
basic/commands/noPermission|К сожалению, у вас нет разрешения на выполнение этого действия.
basic/commands/ok|Команда успешно выполнена.
basic/commands/commandError|Я столкнулся с ошибкой при выполнении этой команды, сообщите об этом с помощью следующего ключевого слова: {0}
basic/commands/badArgs|Недействительные аргументы. Проверьте руководство, команды обычно содержат следующую за ними информацию, например "/xesetverifytime 60".

#CONFIG
config/success|Успешно изменено {0} на {1}
config/dontUnderstandNumber|Я не могу понять значение {0}, это должно быть число.
config/somethingWrong|По какой-то причине мне не удалось сохранить конфигурацию. Свяжитесь с моим разработчиком.
config/featureDisabled|Успешно отключена функция "{0}".
config/featureEnabled|Успешно активирована функция "{0}".
config/featureNotExist|Функция "{0}" не существует.
config/languageNotSupported|Извините, я пока не поддерживаю язык "{0}".
config/kicktimeChanged|Успешно изменено время проверки на {0} мин.
config/mediaTimeChanged|Успешно изменена задержка для медиафайлов на {0} ч.
config/messageHelp|В этом сообщении должен присутствовать атрибут %ACTURL. Вы также можете использовать %NAME и %DURATION.
config/messageSet|Сообщение о присоединения нового участника было успешно изменено.

#FEATURES 
feature/attackOn|Режим атаки ВКЛЮЧЕН. Новые участники не принимаются.
feature/attackOff|Режим атаки ОТКЛЮЧЕН. Новые участники больше не удаляются.
feature/attackMode/userKicked|{0} был удален из чата - режим атаки включен, новые участники не принимаются.

feature/kickNoIcons/userKicked|{0} был удален из чата, так как отсутствует изображение профиля и активирована функция "kicknoicons".
feature/kickNoHandle/userKicked|{0} был удален из чата, так как отсутствует @имя_пользователя и активирована функция "kicknohandle".
feature/kickEarlyMedia/userKicked|{0} был удален из чата за публикацию медиафайлов до успешного прохождения проверки.
feature/mediaDelay|{0}, Вы должны подождать {1} часов, чтобы публиковать медиафайлы после прохождения проверки.
feature/mediaDelayWarn|{0}, Вы должны пройти проверку, прежде чем сможете публиковать медиафайлы.

captcha/userWelcome|{0}, добро пожаловать в чат!\n\nПожалуйста пройдите проверку.\n\nУ вас есть {1} минут для подтверждения, иначе вы будете исключены из чата.\n\nВы можете пройти проверку здесь {2}
#CAPTCHA 


#AUTOREM 
autorem/removedBecauseBot|{0} был удален из чата, так как есть подозрение, что это бот.
autorem/sorry|Привет %s, прошу прощение за то, что исключил Тебя из чата. Ваш профиль подозрительно похож на тот, что есть у ботов. Попробуйте следующее:\n1. установите изображение профиля, если его нет;\n2. добавьте @имя_пользователя к Вашему профилю, если его нет;\n3. выберите имя подлиннее.
