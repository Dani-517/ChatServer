using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatServer
{
    public static class userManager
    {
        public static List<chatUser> userList = new List<chatUser>();


        // load all the users from data/users
        public static void loadAllUsers() 
        {
            foreach(string file in Directory.GetFiles("data/users/"))
            {
                string filetrim = Path.GetFileNameWithoutExtension(file);
                //Console.WriteLine($"Loading user {filetrim}");
                chatUser? user = userManager.getUser(uint.Parse(filetrim));
                if(user == null)
                {
                    Console.WriteLine($"Failed to load user: {file}");
                    continue;
                }
                userList.Add(user);
            }
        }

        public static chatUser? getUser(uint userID)
        {
            chatUser? user = userList.FirstOrDefault(user => user.userID == userID);
            if(user != null) // check if user was already loaded
            {
               return user;  // user is already loaded, return it
            }

            user = loadUser(userID); // user is not loaded, load it
            if(user != null) //check if user is loaded now
            {
               return user;  // user succesfully loaded, return it
            }

            return null; // user was not loaded and trying to load it failed, returning null
        }

        private static chatUser? loadUser(uint userID)
        {
            chatUser? user = new chatUser();
            try
            {
                user = JsonSerializer.Deserialize<chatUser>(File.ReadAllText($"data/users/{userID}"));
            }
            catch(Exception ex)
            {
                Program.log($"[{userID}]{ex.Message}");
                return null;
            }
            return user;
        }

        public static bool deleteUser(uint userID)
        {
            chatUser? user = getUser(userID);
            if(user != null)
            {
                userList.Remove(user);
                File.Delete($"data/users/{userID}");
                return true; 
            }
            return false;
        }

        public static bool createUser(string userName, string password)
        {
            chatUser user = new chatUser();
            Console.WriteLine($"Creating user with username: {userName}");
            user.userID = (userList.Max(user => user.userID) + 1); // get highest userid and add 1 to it to get the next userid
            user.userName = userName;
            user.userPassword = password;
            userList.Add(user);
            File.Create($"data/users/{user.userID}").Close();
            File.WriteAllText($"data/users/{user.userID}", JsonSerializer.Serialize(user));
            return true; 
        }

        public static bool saveUser(chatUser user)
        {
            File.Create($"data/users/{user.userID}").Close(); // create file if doesnt exist already for some reason
            File.WriteAllText($"data/users/{user.userID}", JsonSerializer.Serialize(user));
            return true; 
        }
    }
    public class chatUser
    {
        [JsonPropertyName("userID")]
        public uint? userID { get; set; }
    
        [JsonPropertyName("userName")]
        public string? userName { get; set; }

        // stuff below is not implemented yet, but is here for future.
        [JsonPropertyName("userPassword")]
        public string? userPassword { get; set; } //NOTE: add password hashing

        [JsonPropertyName("userPermissions")]
        public static List<PermissionManager.chatUserPermission> userPermissions = new List<PermissionManager.chatUserPermission>();
    }
}