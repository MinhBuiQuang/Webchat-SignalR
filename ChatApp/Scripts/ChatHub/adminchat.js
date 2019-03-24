// Declare a proxy to reference the hub. 
var chat = $.connection.chatHub;
$.connection.hub.url = "http://localhost:51585/signalr";
$(function () {
    // Create a function that the hub can call to broadcast messages.
    chat.client.receivingMessage = function (sender, message, name, time) {
        if (sender == $.connection.hub.id)
            document.getElementById("chatbody").innerHTML += makeOutGoingMessage(message, time);
        else
            document.getElementById("chatbody").innerHTML += makeIncomingMessage(name, message, time);
        scrollBottom();
    };
    loadYeuCau();

    $.connection.hub.start().done(function () {

    });
});



$("#btn-send").click(function () {
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
});

$('#txt_message').keypress(function (e) {

    if (e.keyCode == 13) {
        if ($(this).val() != "") {
            $("#btn-send").click();
        }
    }
});
function loadYeuCau() {
    $.ajax({
        url: "/api/YeuCaus",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var k in data) {
                if (data[k].MessageLogs.length == 0) 
                    var item = makeYeuCau(data[k].TieuDe, "", "", "");
                else
                    var item = makeYeuCau(data[k].TieuDe, data[k].MessageLogs[0].User.Username, data[k].MessageLogs[0].Message, data[k].MessageLogs[0].Timestamp, data[k].YeuCauID);
                document.getElementById("listYeuCau").innerHTML += item;
            }
            $(".chat_list").click(function () {
                document.getElementById("chatbody").innerHTML = "";
                var id = $(this).data('yeucauid');
                $("#group-name").val(id);
                chat.server.addToGroupAdmin(id);
                loadTinNhan(id);
            });
        }
    });
}

function loadTinNhan(id) {
    if (id != null)
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
                        document.getElementById("chatbody").innerHTML += makeOutGoingMessage(data[k].Message, data[k].Timestamp);
                    else
                        document.getElementById("chatbody").innerHTML += makeIncomingMessage(data[k].User.Username, data[k].Message, data[k].Timestamp);
                }
                scrollBottom();
            }
        });
}

function formatTime(time) {
    if (time != "") {
        var d = new Date(time);
        var formattedDate = (d.getDate() < 10 ? ("0" + d.getDate()) : d.getDate()) + "/" + ((d.getMonth() + 1) < 10 ? ("0" + (d.getMonth() + 1)) : (d.getMonth() + 1)) + "/" + d.getFullYear();
        var hours = (d.getHours() < 10) ? "0" + d.getHours() : d.getHours();
        var minutes = (d.getMinutes() < 10) ? "0" + d.getMinutes() : d.getMinutes();
        var formattedTime = hours + ":" + minutes;
        time = formattedDate + " " + formattedTime;
    }
    return time;
}

function makeYeuCau(tieuDe, username, tinNhan, time, yeuCauID) {
    if (username != "")
        username += ': ';
    
    
    return `
        <div class="button-effect chat_list" data-yeucauid = \"` + yeuCauID + `\">
            <div class="chat_people">
                <div class="chat_img"> <img src="https://ptetutorials.com/images/user-profile.png" alt="sunil"> </div>
                <div class="chat_ib">
                    <h5>` + tieuDe + ` <span class="chat_date">` + formatTime(time) + `</span></h5>
                    <p>
                        ` + username + tinNhan + `
                    </p>
                </div>
            </div>
        </div>
    `;
}

function makeIncomingMessage(username, message, time) {

    return ` 
   <div class="incoming_msg">
        <div class="incoming_msg_img"> <img src="https://ptetutorials.com/images/user-profile.png" alt="sunil"> </div>
        <div class="received_msg">
            <div class="received_withd_msg">
                <span class="time_date"> ` + username + ` - ` + formatTime(time) + `</span>
                <p>
                    ` + message + `
                </p>
                
            </div>
        </div>
    </div>
    `;
}

function makeOutGoingMessage(message, time) {

    return ` 
  <div class="outgoing_msg">
        <div class="sent_msg">
        <span class="time_date"> ` + formatTime(time) + `</span>
            <p>
                ` + message + `
            </p>
            
        </div>
    </div>
    `;
}

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

function scrollBottom() {
    $("#chatbody").animate({ scrollTop: document.getElementById("chatbody").scrollHeight }, "slow");
}   