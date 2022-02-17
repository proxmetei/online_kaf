"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

if (document.getElementById("sendButton"))
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.text.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var user = user.replace('/', '')
    var encodedMsg = user + ": " + msg
    var li = document.createElement("div");
    li.classList.add("message")
    li.textContent = encodedMsg;
    if (message.doc != null) {
        var br = document.createElement("br");
        var a = document.createElement("a");
       a.setAttribute('href', "/Chat/GetBytes?GUID=" + message.doc.guid);
        a.textContent = message.doc.name;
        li.appendChild(br);
        li.appendChild(a);
    }
    let chat = document.getElementById("chat")
    chat.appendChild(li);
    chat.scrollTop = chat.scrollHeight;
});

connection.start().then(function () {
    connection.invoke('JoinRoom',document.getElementById('chatName1').value);
    if (document.getElementById("sendButton"))
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});