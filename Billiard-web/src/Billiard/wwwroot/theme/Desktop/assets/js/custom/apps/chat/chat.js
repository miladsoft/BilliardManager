"use strict";
var PPCAppChat = function () {
    var protocol = location.protocol === "https:" ? "wss:" : "ws:";
    var uri = protocol + "//" + window.location.host + "/ws";
    var socket = new WebSocket(uri);
    var userId = document.querySelector("#userId").value;
    var pcm = document.querySelector("#ppc_chat_messenger");
    function connect() {
        socket.onopen = function (event) {
            console.log("opened connection to " + uri);
        };

        socket.onclose = function (event) {
            console.log("closed connection from " + uri);
        };

        socket.onmessage = function (event) {
            var r = JSON.parse(event.data);
                
              
              
            switch (r.Type) {
                case "chat": {
                    if (r.Sender == userId) {
                        Send(pcm, r.Content,r.DateTime,r.UserDisplayName,r.UserAvatar);
                    }
                    else {
                        get(pcm, r.Content,r.DateTime,r.UserDisplayName,r.UserAvatar);
                    }
                    break;
                }

                default:
                    break;
            }
            console.log(event.data);
        };

        socket.onerror = function (event) {
            console.log("error: " + event.data);
        };
    }
    connect();

    function SendMessage(msg) {
        if (msg.length != 0) {
            var message = {};
            message.Type = "chat";
            message.Sender = userId;
            message.Content = msg;
            message.Receiver = "group";
            message.IsPrivate = false;
            console.log(JSON.stringify(message));

            socket.send(JSON.stringify(message));
        }

    }

    var Send = function (e, message, datetime , userdisplayname, useravatarpic)  {
        var t = e.querySelector('[data-ppc-element="messages"]');
        var o, a = t.querySelector('[data-ppc-element="template-in"]');
        (o = a.cloneNode(!0)).classList.remove("d-none"),
        o.querySelector('[data-ppc-element="message-text"]').innerText = message,
        o.querySelector('[data-ppc-element="datetime"]').innerText = datetime,
        o.querySelector('[data-ppc-element="userdisplayname"]').innerText = userdisplayname,
        o.querySelector('[data-ppc-element="userPic"]').src = "/avatars/"+useravatarpic,
            t.appendChild(o), t.scrollTop = t.scrollHeight
    };

    var get = function (e, message, datetime , userdisplayname, useravatarpic) {
        var t = e.querySelector('[data-ppc-element="messages"]');
        var o, l = t.querySelector('[data-ppc-element="template-out"]');
        (o = l.cloneNode(!0)).classList.remove("d-none"),
            o.querySelector('[data-ppc-element="message-text"]').innerText = message,
            o.querySelector('[data-ppc-element="datetime"]').innerText = datetime,
            o.querySelector('[data-ppc-element="userdisplayname"]').innerText = userdisplayname,
            o.querySelector('[data-ppc-element="userPic"]').src = "/avatars/"+useravatarpic,
            t.appendChild(o), t.scrollTop = t.scrollHeight
    };
    return {
        init: function (t) {
            !function (t) {
                t && (PPCUtil.on(t, '[data-ppc-element="input"]', "keydown",
                    (function (n) {

                        var m = t.querySelector('[data-ppc-element="input"]');

                        var _message = m.value;
                        if (13 == n.keyCode) {
                            m.value = "";
                            return SendMessage(_message),
                                n.preventDefault(), !1
                        }

                    })),
                    PPCUtil.on(t, '[data-ppc-element="send"]', "click",
                        (function (n) {
                            var m = t.querySelector('[data-ppc-element="input"]');

                            var _message = m.value;


                            m.value = "";
                            return SendMessage(_message),
                                n.preventDefault(), !1


                        })))
            }(t)
        }
    }
}();
PPCUtil.onDOMContentLoaded((function () {
    PPCAppChat.init(document.querySelector("#ppc_chat_messenger"))
}));

function ToggleMember() {
    var x = document.getElementById("GroupMember");
    if (x.style.display === "none") {
      x.style.display = "block";
    } else {
      x.style.display = "none";
    }
  }