using Microsoft.AspNetCore.SignalR;
using NiTiAPI.Dapper.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NiTiAPI.WebErp.Hubs
{
    public class UserOnlineHub : Hub
    {

        public async Task GetChatRoom1Members(string username, string chatRoom, string fullname,
             string avatarUser, string corporationid)
        {
            var client = new UserViewModel();
            client.ConnectionId = Context.ConnectionId;
            client.UserName = username;
            client.FullName = fullname;
            client.Avatar = avatarUser;

            client.ChatRoom = ChatRoom.chatroom1;
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);

            client.CorporationId = int.Parse(corporationid);

            ConnectionHelper.Connections.Add(client);

            await Clients.All.SendAsync("ClientGetChatRoom1Members",
                ConnectionHelper.Connections.Where(c => c.ChatRoom == ChatRoom.chatroom1));
        }

        public void RegisterMemberParaLogin(string name, string chatRoom, string fullname,
             string avatarUser, int corporationid)
        {

            var client = new UserViewModel();
            client.ConnectionId = Context.ConnectionId;
            client.UserName = name;
            client.FullName = fullname;
            client.Avatar = avatarUser;

            client.ChatRoom = ChatRoom.chatroom1;
            Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);

            client.CorporationId = corporationid;
            //Clients.All.SendAsync("SendUserOnline", client.UserName, client.Avatar);
            ConnectionHelper.Connections.Add(client);
        }

        public Task ClientGetUser()
        {
            return Clients.All.SendAsync("ClientGetUserOnline",
                ConnectionHelper.Connections.Where(c => c.ChatRoom == ChatRoom.chatroom1));
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var client = ConnectionHelper.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (client != null)
            {
                ConnectionHelper.Connections.Remove(client);
                Clients.All.SendAsync("ClientGetChatRoom1Members", ConnectionHelper.Connections.Where(c => c.ChatRoom == ChatRoom.chatroom1));               
                //Clients.All.SendAsync("ClientGetUserOnline", ConnectionHelper.Connections.Where(c => c.ChatRoom == ChatRoom.chatroom1));
            }
            return base.OnDisconnectedAsync(exception);
        }
        
        public Task SendUserOffline(string username, string userImg)
        {
            return Clients.All.SendAsync("sendUserOffline", username, userImg);
        }

        public Task SendUserOnline(string username, string userImg)
        {
            return Clients.All.SendAsync("SendUserOnline", username, userImg);
        }

        





    }
}
