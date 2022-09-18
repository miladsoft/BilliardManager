$(function () {
    //Disabling password auto-complete in Firefox
    $("input[type=password]").val('');
});

//-----------------------------------------------bootstrap
$(function () {
    $(".menu-title-gray-500 .menu-item .menu-link").each(function () {
        var href = $(this).find('a').attr('href');
        if (!href) {
            href = $(this).attr('href');
        }
        if (href === location.pathname) {
            $(this).addClass('active');
        }
    });
});
$(function () {
    $(".aside-menu .menu-item .menu-link").each(function () {
        var href = $(this).find('a').attr('href');
        if (!href) {
            href = $(this).attr('href');
        }
        if (href === location.pathname) {

            $(this).parent().parent().addClass('hover show');
            $(this).parent().parent().parent().addClass('show');
            $(this).addClass('active selectedmenu');


            var box = document.querySelector('.hover-scroll-overlay-y');
            targetElm = document.querySelector('.menu-link.active.selectedmenu'); // <-- Scroll to here within ".box"
            scrollToElm(box, targetElm, 600);


        }
    });
});

function scrollToElm(container, elm, duration) {
    var pos = getRelativePos(elm);
    scrollTo(container, pos.top, 2);  // duration in seconds
}

function getRelativePos(elm) {
    var box = document.querySelector('.hover-scroll-overlay-y');

    var pPos = box.getBoundingClientRect(), // parent pos
        cPos = elm.getBoundingClientRect(), // target pos
        pos = {};

        pos.top = cPos.top - pPos.top + elm.scrollTop,
        pos.right = cPos.right - pPos.right,
        pos.bottom = cPos.bottom - pPos.bottom,
        pos.left = cPos.left - pPos.left;

    return pos;
}

function scrollTo(element, to, duration, onDone) {
    var start = element.scrollTop,
        change = to - start,
        startTime = performance.now(),
        val, now, elapsed, t;

    function animateScroll() {
        now = performance.now();
        elapsed = (now - startTime) / 1000;
        t = (elapsed / duration);

        element.scrollTop = start + change * easeInOutQuad(t);

        if (t < 1)
            window.requestAnimationFrame(animateScroll);
        else
            onDone && onDone();
    };

    animateScroll();
}

function easeInOutQuad(t) { return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t };
$.validator.setDefaults({
    ignore: "", // for hidden tabs and also textarea's
    errorElement: 'span',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback');
        element.closest('.form-group').append(error);
    },
    highlight: function (element, errorClass, validClass) {
        if (element.type === 'radio') {
            this.findByName(element.name).addClass(errorClass).removeClass(validClass);
        } else {
            $(element).addClass(errorClass).removeClass(validClass);
            $(element).addClass('is-invalid').removeClass('is-valid');
            $(element).closest('.form-group').find('.input-group-text, label').removeClass('text-success').addClass('text-danger');
        }
        $(element).trigger('highlited');
    },
    unhighlight: function (element, errorClass, validClass) {
        if (element.type === 'radio') {
            this.findByName(element.name).removeClass(errorClass).addClass(validClass);
        } else {
            $(element).removeClass(errorClass).addClass(validClass);
            $(element).removeClass('is-invalid').addClass('is-valid');
            $(element).closest('.form-group').find('.input-group-text, label').removeClass('text-danger').addClass('text-success');
        }
        $(element).trigger('unhighlited');
    }
});

function removeAllTagsAndTrim(html) {
    return !html ? "" : $.trim(html.replace(/(<([^>]+)>)/ig, ""));
}

$.validator.methods.originalRequired = $.validator.methods.required;
$.validator.addMethod("required", function (value, element, param) {
    value = removeAllTagsAndTrim(value);
    if (!value) {
        return false;
    }
    return $.validator.methods.originalRequired.call(this, value, element, param);
}, $.validator.messages.required);

//-----------------------------------------------bootstrap

//------------------------------ if a webpage is being loaded inside an iframe
function defrm() {
    document.write = '';
    window.top.location = window.self.location;
    setTimeout(function () {
        document.body.innerHTML = '';
    }, 0);
    window.self.onload = function (evt) {
        document.body.innerHTML = '';
    };
}
if (window.top !== window.self) {
    try {
        if (window.top.location.host) { /* will throw */ }
        else {
            defrm(); /* chrome */
        }
    } catch (ex) {
        defrm(); /* everyone else */
    }
}
//-----------------------

//-----------------------Ajax forms
function dataAjaxBegin() {
    $.bootstrapModalAlert({
        caption: 'شروع انجام عملیات',
        body: '<div class="alert alert-info"> <span class="fas fa-thumbs-up" aria-hidden="true"></span> درحال ارسال اطلاعات به سرور. لطفا اندکی تامل نمائید. </div>'
    });
    setTimeout(function () {
        $('.modal').modal('hide');
        return true;
    }, 2000);
}

function dataAjaxSuccess(data, status, xhr) {
    $.bootstrapModalAlert({
        caption: 'تائید انجام عملیات',
        body: '<div class="alert alert-success"> <span class="fas fa-thumbs-up" aria-hidden="true"></span> عملیات درخواستی با موفقیت انجام شد.</div>'
    });
    setTimeout(function () {
        $('.modal').modal('hide');
        window.location.reload();
    }, 2000); // milliseconds

}

function dataAjaxFailure(xhr, status, error) {
    if (xhr.status === 401) {
        window.location.href = xhr.getResponseHeader('Location');
        return;
    }
    $.bootstrapModalAlert({
        caption: 'خطا در انجام عملیات',
        body: '<div class="alert alert-danger"> <span class="fas fa-thumbs-down" aria-hidden="true"></span> ' + xhr.responseText + '</div>'
    });

    setTimeout(function () {
        $('.modal').modal('hide');
    }, 2000); // milliseconds
}
//-----------------------Ajax forms

//--------------------- Set dir rtl
function checkRTL(s) {
    var ltrChars = 'A-Za-z\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02B8\u0300-\u0590\u0800-\u1FFF' + '\u2C00-\uFB1C\uFDFE-\uFE6F\uFEFD-\uFFFF',
        rtlChars = '\u0591-\u07FF\uFB1D-\uFDFD\uFE70-\uFEFC',
        rtlDirCheck = new RegExp('^[^' + ltrChars + ']*[' + rtlChars + ']');
    return rtlDirCheck.test(s);
}
function setDirection(selector) {
    var string = selector.val();
    for (var i = 0; i < string.length; i++) {
        var isRtl = checkRTL(string[i]);
        var dir = isRtl ? 'RTL' : 'LTR';
        if (dir === 'RTL') var finalDirection = 'RTL';
        if (finalDirection == 'RTL') dir = 'RTL';
    }
    if (dir === 'LTR') {
        selector.css("direction", "ltr");
    } else {
        selector.css("direction", "rtl");
    }
}
// Change Input Direction Depend on Language
$(document).ready(function () {
    $('input[type="text"]').keyup(function () {
        setDirection($(this));
    });
});
$(document).ready(function () {
    $('a[data-bs-toggle="tab"]').on('show.bs.tab', function (e) {
        localStorage.setItem('activeTab', $(e.target).attr('href'));
    });
    var activeTab = localStorage.getItem('activeTab');
    if (activeTab) {
        $('#myTab a[href="' + activeTab + '"]').tab('show');
    }
});

$(window).on('load', function () {
    $("#loader-page").delay(500).fadeOut("slow");
})

