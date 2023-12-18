using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatServer
{
    public static class messageManager
    {
        public static List<chatMessage> messageList = new List<chatMessage>();


        // load all the messages from data/messages
        public static void loadAllMessages() 
        {
            foreach(string file in Directory.GetFiles("data/messages/"))
            {
                string filetrim = Path.GetFileNameWithoutExtension(file);
                chatMessage? message = getMessage(uint.Parse(filetrim));
                if(message == null)
                {
                    Console.WriteLine($"Failed to load message: {file}");
                    continue;
                }
                messageList.Add(message);
            }
        }

        public static chatMessage? getMessage(uint messageID)
        {
            chatMessage? message = messageList.FirstOrDefault(message => message.messageID == messageID);
            if(message != null) // check if message was already loaded
            {
               return message;  // message is already loaded, return it
            }

            message = loadMessage(messageID); // message is not loaded, load it
            if(message != null) //check if message is loaded now
            {
               return message;  // message succesfully loaded, return it
            }

            return null; // message was not loaded and trying to load it failed, returning null
        }
        
        private static chatMessage? loadMessage(uint messageID)
        {
            chatMessage? user = new chatMessage();
            try
            {
                user = JsonSerializer.Deserialize<chatMessage>(File.ReadAllText($"data/messages/{messageID}"));
            }
            catch(Exception ex)
            {
                Program.log($"[{messageID}]{ex.Message}");
                return null;
            }
            return user;
        }

        public static bool deleteMessage(uint messageID)
        {
            chatMessage? message = getMessage(messageID);
            if(message != null)
            {
                messageList.Remove(message);
                File.Delete($"data/messages/{messageID}");
                return true; 
            }
            return false;
        }


        public static bool createMessage(uint authorID, string messageContent)
        {
            chatMessage message = new chatMessage();
            message.authorID = authorID;
            message.messageID = (messageList.Max(message => message.messageID) + 1); // get highest messageid and add 1 to it to get the next messageid
            message.messageContent = messageContent;
            message.messageDate = DateTime.Now;
            messageList.Add(message);
            File.Create($"data/messages/{message.messageID}").Close();
            File.WriteAllText($"data/messages/{message.messageID}", JsonSerializer.Serialize(message));
            return true; 
        }

        public static bool saveMessage(chatMessage message)
        {
            File.Create($"data/messages/{message.messageID}").Close(); // create file if doesnt exist already for some reason
            File.WriteAllText($"data/messages/{message.messageID}", JsonSerializer.Serialize(message));
            return true; 
        }
    }
    public class chatMessage
    {
        [JsonPropertyName("messageID")]
        public uint? messageID { get; set; }

        [JsonPropertyName("authorID")]
        public uint? authorID { get; set; }

        [JsonPropertyName("messageContent")]
        public string? messageContent { get; set; }

        [JsonPropertyName("messageDate")]
        public DateTime messageDate { get; set; }
    }
}