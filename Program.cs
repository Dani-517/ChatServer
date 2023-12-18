using System;
using System.Net;
using System.Text.Json;

namespace ChatServer
{
    internal class Program
    {
        private static bool Shutdown = false; // if this is set to true the app will shut down asap
        static void Main(string[] args)
        {
            ensureFoldersExist();
            userManager.loadAllUsers();
            messageManager.loadAllMessages();
            Initializechat();
            Console.WriteLine("Started.");


            //DEBUG CODE, will show users as json to show which have been loaded succesfully
            Console.WriteLine("--Users--");
            foreach(chatUser user in userManager.userList) 
            {
                Console.WriteLine(JsonSerializer.Serialize(user));
            }


            //DEBUG CODE, will show messages as json to show which have been loaded succesfully
            Console.WriteLine("--Messages--");
            foreach(chatMessage message in messageManager.messageList) 
            {
                Console.WriteLine(JsonSerializer.Serialize(message));
            }

            while(!Shutdown)
            {
                //under construction
            }
        }

        private static void ensureFoldersExist() // the app has some folders built in that it will store data in them, this will automatically create those folders and a default root user, if necessary.
        {
            //if data directory does not exist, create it
            if(!Directory.Exists("data/"))
            {
                Directory.CreateDirectory("data/");
            }
            //if users directory does not exist, create it, and create root user, also clear the messages since they are either non existent or invalid since there are no users.
            if(!Directory.Exists("data/users/"))
            {

                Directory.CreateDirectory("data/users/");
                if(Directory.Exists("data/messages/"))
                {
                    Directory.Delete("data/messages/");
                }
                Directory.CreateDirectory("data/messages/");


                chatMessage defaultMessage = new chatMessage();
                defaultMessage.messageID = 0;
                defaultMessage.authorID = 0;
                defaultMessage.messageContent = "This account has been generated automatically, please change its password.";
                defaultMessage.messageDate = DateTime.Now;
                messageManager.saveMessage(defaultMessage); //message by user 0 (Root)
                
                
                chatUser rootUser = new chatUser();
                rootUser.userID = 0;
                rootUser.userName = "Root";
                rootUser.userPassword = "Root";
                userManager.saveUser(rootUser);
                Console.WriteLine($"Created user [{rootUser.userID}]{rootUser.userName} with password \"{rootUser.userPassword}\", its recommended to change the password");
            }
            else if(!Directory.Exists("data/messages/")) //if messages directory does not exist but users does, do not create new messages.
            {
                Directory.CreateDirectory("data/messages/");
            }
        }

        private static void Initializechat()
        {
            Console.WriteLine("Initializing...");
            //add necessary things later
        }

        internal static void log(string message)
        {
            Console.WriteLine($"LOG: {message}");
            //will be implemented properly later
        }
    }
}