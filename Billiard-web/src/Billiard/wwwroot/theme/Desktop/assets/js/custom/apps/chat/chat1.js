"use strict";
var PPCAppChat = function () {
    var Send = function (e) {
        var t = e.querySelector('[data-ppc-element="messages"]'),
            n = e.querySelector('[data-ppc-element="input"]');
        if (0 !== n.value.length) {
            var o,
                a = t.querySelector('[data-ppc-element="template-in"]');
             
            (o = a.cloneNode(!0)).classList.remove("d-none"),
                o.querySelector('[data-ppc-element="message-text"]').innerText = n.value, n.value = "",
                t.appendChild(o), t.scrollTop = t.scrollHeight
 
        }
    };
    var get = function (e) {
        var t = e.querySelector('[data-ppc-element="messages"]');

        
            var o,l = t.querySelector('[data-ppc-element="template-out"]');
               
                
            (o = l.cloneNode(!0)).classList.remove("d-none"),
                        o.querySelector('[data-ppc-element="message-text"]').innerText = "سلام ممنونم - تو خوبی ؟",
                        o.querySelector('[data-ppc-element="datetime"]').innerText = "دیروز",
                        o.querySelector('[data-ppc-element="userdisplayname"]').innerText = "میلاد",
                        o.querySelector('[data-ppc-element="useravatarpic"]'), 
                        t.appendChild(o), t.scrollTop = t.scrollHeight
                
        
    };
    return {
        init: function (t) {
            !function (t) {
                t && (PPCUtil.on(t, '[data-ppc-element="input"]', "keydown",
                    (function (n) {
                        if (13 == n.keyCode)
                            return Send(t),
                                n.preventDefault(), !1
                    })),
                    PPCUtil.on(t, '[data-ppc-element="send"]', "click",
                        (function (n) { get(t) })))
            }(t)
        }
    }
}();
PPCUtil.onDOMContentLoaded((function () {
    PPCAppChat.init(document.querySelector("#ppc_chat_messenger"))
}));