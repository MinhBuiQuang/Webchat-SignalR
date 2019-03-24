// Declare a proxy to reference the hub. 
var chat = $.connection.chatHub;
$.connection.hub.url = "http://localhost:51585/signalr";
$(function () {   
    // Create a function that the hub can call to broadcast messages.
    chat.client.receivingMessage = function (sender, message, name, time) {
        if (sender == $.connection.hub.id)
            document.getElementById("chatbody").innerHTML += makeMessageRight(name, message, time);
        else
            document.getElementById("chatbody").innerHTML += makeMessageLeft(name, message, time);
        scrollBottom();
    };
    loadYeuCau();

    $.connection.hub.start().done(function () {
        
    }); 
});

function scrollBottom() {
    $("#chatPanel").animate({ scrollTop: $("#chatbody").height() }, "slow");
}   

document.getElementById("btn-send").addEventListener("click", async (event) => {
    var groupName = document.getElementById("group-name").value;
    var groupMsg = document.getElementById("txt_message").value;
    try {
        if (groupMsg.trim() != "") {
            chat.server.sendingMessageToGroup(groupName, groupMsg);
            $('#txt_message').val('').focus();
        }
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
});

$('#txt_message').keypress(function (e) {

    if (e.keyCode == 13) {
        if ($(this).val() != "") {
            $("#btn-send").click();
        }
    }


});

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function loadMessage() {
    var id = document.getElementById("group-name").value;
    if(id != null)
        $.ajax({
            url: "/api/MessageLogs",
            type: "GET",
            data: { yeucauid: id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var userid = getCookie("userid");
                for (var k in data) {
                    if (userid == data[k].UserID)
                        document.getElementById("chatbody").innerHTML += makeMessageRight(data[k].User.Username, data[k].Message, data[k].Timestamp);
                    else
                        document.getElementById("chatbody").innerHTML += makeMessageLeft(data[k].User.Username, data[k].Message, data[k].Timestamp);
                }
                scrollBottom();
            }
        });
}

function loadYeuCau() {
    $.ajax({
        url: "/api/YeuCaus",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var k in data) {
                var item = "<a href=\"#\" data-yeucauid =\"" + data[k].YeuCauID + "\" class=\"list-group-item\">" + data[k].TieuDe + "</a>";
                document.getElementById("list-yeu-cau").innerHTML += item;                
            }
            $("a.list-group-item").on("click", function (e) {
                var id = $(this).data('yeucauid');
                if (id != null) {
                    document.getElementById("chatbody").innerHTML = "";
                    $("#group-name").val(id);
                    chat.server.addToGroup(id);
                    loadMessage();
                    if (!$("#collapseOne").is(".in"))
                        $("#btn-collapse").click();                    
                }
            });
        }
    });

    
}

$('#collapseOne').on("shown.bs.collapse", function () {
    scrollBottom();
});

function makeMessageLeft(user, message, time) {
    var d = new Date(time);
    var formattedDate = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
    var hours = (d.getHours() < 10) ? "0" + d.getHours() : d.getHours();
    var minutes = (d.getMinutes() < 10) ? "0" + d.getMinutes() : d.getMinutes();
    var formattedTime = hours + ":" + minutes;
    formattedDate = formattedDate + " " + formattedTime;

    return ` 
    <li class="left clearfix">
        <span class="chat-img pull-left">
            <img src="http://placehold.it/50/55C1E7/fff&amp;text=U" alt="User Avatar" class="img-circle">
        </span>
        <div class="chat-body clearfix">
            <div class="header">
                <strong class="primary-font">`+ user + `</strong> 
                <small class="pull-right text-muted"><span class="glyphicon glyphicon-time"></span>` + time +`</small>
            </div>
            <p>` + message + ` </p>
        </div>
    </li>
    `;
}

function makeMessageRight(user, message, time) {
    var d = new Date(time);
    var formattedDate = (d.getDate() < 10 ? ("0" + d.getDate()) : d.getDate()) + "/" + ((d.getMonth() + 1) < 10 ? ("0" + (d.getMonth() + 1))  : (d.getMonth() + 1)) + "/" + d.getFullYear();
    var hours = (d.getHours() < 10) ? "0" + d.getHours() : d.getHours();
    var minutes = (d.getMinutes() < 10) ? "0" + d.getMinutes() : d.getMinutes();
    var formattedTime = hours + ":" + minutes;
    formattedDate = formattedDate + " " + formattedTime;

    return ` 
    <li class="right clearfix">
        <span class="chat-img pull-right">
            <img src="http://placehold.it/50/FA6F57/fff&amp;text=ME" alt="User Avatar" class="img-circle">
        </span>
        <div class="chat-body clearfix">
            <div class="header">
                <small class=" text-muted"><span class="glyphicon glyphicon-time"></span>` + formattedDate  + `</small>
                <strong class="pull-right primary-font">` + user +`</strong>
            </div>
            <p class="pull-right">
                ` + message +`
            </p>
        </div>
    </li>
    `;
}
