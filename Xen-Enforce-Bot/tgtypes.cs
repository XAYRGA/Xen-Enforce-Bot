using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XenfbotDN
{
    public class TGUser
    {
        public long id;
        public bool is_bot;
        public string first_name;
        public string last_name;
        public string username;
        public string language_code;
    }

    public class TGResponse
    {
        public bool ok;
        public JToken result;

    }
    public class TGUpdate
    {
        public long update_id;
        public TGMessage message;
        public TGMessage edited_message;
    }

    public class TGChat
    {
        public long id;
        public string type;
        public string title;
        public string username;
        public string firstname;
        public string lastname;
        public bool all_members_are_administrators;
        public object photo; //!!
        public string description;
        public string invite_link;
        public object pinned_message; //!!
        public string sticker_set_name;
        public string can_set_sticker_set;

        public TGMessage sendMessage(string text)
        {
            return Telegram.sendMessage(this, text);
        }
        public TGMessage sendMessage(string text, string markdown)
        {
            return Telegram.sendMessage(this, text, markdown);
        }
    }

    public class TGChatMember
    {
        public TGUser user;
        public string status;
        public int until_date;
        public bool can_be_edited;
        public bool can_change_info;
        public bool can_post_messages;
        public bool can_edit_messages;
        public bool can_delete_messages;
        public bool can_invite_users;
        public bool can_restrict_members;
        public bool can_pin_messages;
        public bool can_promote_members;
        public bool is_member;
        public bool can_send_messages;
        public bool can_send_media_messages;
        public bool can_send_other_messages;
        public bool can_add_web_page_previews;
    }

    public class TGProfilePhotos
    {
        public int total_count;
    }

    public class TGPhotoSize
    {
        public string file_id;
        public int width;
        public int height;
        public int file_size;
    }
    public class TGVideo
    {
        public string file_id;
        public string file_unique_id;
    }
    public class TGVideoNote
    {
        public string file_id;
        public string file_unique_id;
    }
    public class TGDocument
    {
        public string file_id;
        public string file_unique_id;
    }

    public class TGMessage
    {
        public long message_id;
        public TGUser from;
        public int date;
        public TGChat chat;
        public TGUser forward_from;
        public TGChat forward_from_chat;
        public int forward_from_message_id;
        public string forward_signature;
        public string forward_sender_name;
        public int forward_date;
        public int edit_date;
        public TGPhotoSize[] photo;
        public string text;
        public TGUser[] new_chat_members;
        public TGVideo video;
        public TGDocument document;
        public TGVideoNote video_note;

        public TGMessage replySendMessage(string text)
        {
            if (this.chat != null)
            {
                return  Telegram.sendMessage(this.chat, text);
            }
            else
            {
                Helpers.warn("[!] Tried to reply to a message that has no chat.");
                return null;
            }
        }

        public TGMessage replyLocalizedMessage(string code, string locstring,params object[] data)
        {
            var message = Localization.getStringLocalized(code, locstring, data);
            if (this.chat != null)
            {
                return Telegram.sendMessage(this.chat, message);
            }
            else
            {
                Helpers.warn("[!] Tried to reply to a message that has no chat.");
                return null;
            }
        }

        public bool delete()
        {
            if (this.chat != null)
            {
                return Telegram.deleteMessage(chat, this);
            }
            else
            {
                Helpers.warn("[!] Tried to delete a message from a null chat.");
                return false;
            }
        }

        public bool isSenderAdmin()
        {
            var ChatMem = Telegram.getChatMember(chat, from);
            if (ChatMem.status == "creator" || ChatMem.status == "admin" || ChatMem.status == "administrator")
                return true;

            return false;
        }
    }

}