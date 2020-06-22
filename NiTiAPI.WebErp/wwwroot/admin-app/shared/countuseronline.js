

var corporationId = $("#hidUserCorporationId").val();
var fullName = $("#hidUserFullName").val();
var userNameId = $("#hidUserName").val();
var UserImage = "/admin-side/images/img.jpg";
var UserImage2 = $("#hidLoginUserImgae2").val();
var avatarUser = $("#hidAvatarUserId").val();
var dateNow = new Date();
//var localdate = dateFormat(dateNow, "dddd, mmmm dS, yyyy, h:MM:ss TT");
var arr = []; // List of users	
var fileUploadHinh = [];


var countUserOnline = 0;

const connectionUserOnlineAdmin = new signalR.HubConnectionBuilder().withUrl("/useronlinehub").build();

//connectionUserOnlineAdmin.on('ClientGetChatRoom1Members', (data, name, chatRoom, fullname, avatarUser, corporationId1) => {
connectionUserOnlineAdmin.on('ClientGetChatRoom1Members', (data) => {
    if (data.length === 0) {
        $("#totalUserOnline").html('');
        $("#onlineUsers").html('');

        loadTotalUserOnline(corporationId);        

        $("#hidUserUserOnlineAdmin").val(countUserOnline);

        var codeOnlineUser = '';
        codeOnlineUser = '<a>' + 0 + '</>';
        $("#onlineUsers").append(codeOnlineUser);
    }
    else {
        loadCorporationNameUserDis(corporationId, data);        
    }
});

connectionUserOnlineAdmin.start()
    .then(function () {
        var chatroom = "chatRoom1";        
        //connectionUserOnlineAdmin.invoke("RegisterMemberParaLogin", userNameId, chatroom, fullName, avatarUser, corporationId);
        connectionUserOnlineAdmin.invoke("GetChatRoom1Members", userNameId, chatroom, fullName, avatarUser, corporationId);
    })
    .catch(function (error) {
        console.error(error);
    });

function loadTotalUserOnline(corporationid) {
    var totalUser = 0;
    $.ajax({
        type: "GET",
        url: "/Admin/home/GetTotalUserOnline",
        data: {
            coporationId: corporationid
        },
        dataType: "json",
        success: function (response) {
            $("#totalUserOnline").html("");
            var codeTotalUserOnline;
            totalUser = response;
            codeTotalUserOnline = '<a>' + totalUser + '</>';
            $("#totalUserOnline").append(codeTotalUserOnline);            
        },
        error: function (status) {
            niti.notify(status, 'error');
            niti.stopLoading();
        }
    });    
}

function loadCorporationNameUserDis(corporationid, data) {
    $.ajax({
        type: "GET",
        url: "/Admin/Corporation/GetById",
        data: {
            id: corporationid
        },
        dataType: "json",
        success: function (response) {
            //niti.notify(response.Name.toUpperCase(), "success");
            countUserOnline = 0;
            var countUser = 0;
            $.each(data, function (i, item) {
                var itemName = item.corporationId.toString().toUpperCase();
                // trong roomchat1 theo coporationId    userNameId  item.userName
                if (itemName === response.id.toString().toUpperCase()) {                    

                    $("#totalUserOnline").html('');
                    $("#onlineUsers").html('');                          

                    countUser = countUser + 1;

                    var codeOnlineUser;
                    codeOnlineUser = '<a>' + countUser + '</>';
                    $("#onlineUsers").append(codeOnlineUser);                   

                    countUserOnline = countUserOnline + countUser;  

                    $("#hidUserUserOnlineAdmin").val(countUserOnline);

                    $("#users").append(countUserOnline);
                }
            });
            loadTotalUserOnline(corporationid);  
        },
        error: function (status) {
            niti.notify(status, 'error');
            niti.stopLoading();
        }
    });
}

function loadCorporationName(corporationid, data) {
    $.ajax({
        type: "GET",
        url: "/Admin/Corporation/GetById",
        data: {
            id: corporationid           
        },
        dataType: "json",        
        success: function (response) {
            //niti.notify(response.Name.toUpperCase(), "success");
            countUserOnline = 0;
            var countUser = 0;
            $.each(data, function (i, item) {
                var itemName = item.corporationName.toUpperCase();
                if (itemName === response.Name.toUpperCase()) {
                    //alert(data.CorporationName);

                    $("#totalUserOnline").html('');
                    $("#onlineUsers").html('');

                    //var codeTotalUserOnline;
                    //codeTotalUserOnline = '<a>' + countUserOnline + '</>';
                    //$("#totalUserOnline").append(codeTotalUserOnline); 

                    countUser = countUser + 1;                    
                    //countUserOnline = data.length;
                    $("#hidUserUserOnlineAdmin").val(countUserOnline);

                    var codeOnlineUser;
                    codeOnlineUser = '<a>' + countUser + '</>';
                    $("#onlineUsers").append(codeOnlineUser);        
                    countUserOnline = countUserOnline + countUser;
                }                
            });     
            addUserOnline(corporationId, countUser, "AddUserOnline");
            loadTotalUserOnline(corporationid);  
        },
        error: function (status) {
            niti.notify(status, 'error');
            niti.stopLoading();
        }
    });
}

function addUserOnline(corporationid, codeOnlineUser, notes) {
    $.ajax({
        type: "POST",
        url: "/Admin/home/AddUserOnline",
        data: {
            corporationId: corporationid,
            CountUser: codeOnlineUser,
            notes: notes
        },
        dataType: "json",       
        success: function () {
            niti.appUserLoginLogger("Guid", "Add User online.");    
        },
        error: function () {           
            niti.stopLoading();
        }
    });  
}
   