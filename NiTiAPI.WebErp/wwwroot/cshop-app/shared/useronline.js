
var corporationName = $("#hidCorporationName").val();

var fullName = $("#hidUserFullName").val();
var userNameId = $("#hidUserNameId").val();
var UserImage = $("#hidUserImage").val();
var avatarUser = $("#hidAvatarUserId").val();

const connectionUserOnline = new signalR.HubConnectionBuilder().withUrl("/useronlinehub").build();

connectionUserOnline.start()
    .then(function () {
        var chatroom = "UserOnline";
        
        connectionUserOnline.invoke("RegisterMemberParaLogin", userNameId, chatroom, fullName,
            avatarUser, corporationName.toUpperCase());
        connectionUserOnline.invoke("GetChatRoom1Members"); 
        

    })
    .catch(function (error) {
        console.error(error.message);
    });

