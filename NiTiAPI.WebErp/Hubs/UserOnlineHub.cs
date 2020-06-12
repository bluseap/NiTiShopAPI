using Microsoft.AspNetCore.SignalR;
using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NiTiAPI.WebErp.Hubs
{
    public class UserOnlineHub : Hub
    {
       

        public Task GetChatRoom1Members()
        {
            return Clients.All.SendAsync("ClientGetChatRoom1Members", 
                ConnectionHelper.Connections.Where(c => c.ChatRoom == ChatRoom.chatroom1));
        }

        public Task ClientGetUserOnline()
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
                //Clients.All.SendAsync("ClientGetChatRoom1Members", ConnectionHelper.Connections.Where(c => c.ChatRoom == ChatRoom.chatroom1));               
                Clients.All.SendAsync("ClientGetUserOnline", 
                    ConnectionHelper.Connections.Where(c => c.ChatRoom == ChatRoom.chatroom1));
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

        public void RegisterMemberParaLogin(string name, string chatRoom, string fullname, 
            string avatarUser, string corporationame )
        {
            var client = new UserViewModel();
            client.ConnectionId = Context.ConnectionId;
            client.UserName = name;
            client.FullName = fullname;
            client.Avatar = avatarUser;

            client.ChatRoom = ChatRoom.chatroom1;
            Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);

            client.CorporationName = corporationame;
            //Clients.All.SendAsync("SendUserOnline", client.UserName, client.Avatar);
            ConnectionHelper.Connections.Add(client);
        }





    }
}
