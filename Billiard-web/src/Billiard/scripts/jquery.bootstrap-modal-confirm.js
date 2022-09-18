// <![CDATA[
(function ($) {
    $.bootstrapModalConfirm = function (options) {
        var defaults = {
            caption: 'تائید عملیات',
            body: 'آیا عملیات درخواستی اجرا شود؟',
            onConfirm: null,
            confirmText: 'تائید',
            closeText: 'انصراف'
        };
        options = $.extend(defaults, options);

        var confirmContainer = "#confirmContainer";
        var html = '<div class="modal fade" id="confirmContainer">' +
                   '<div class="modal-dialog"><div class="modal-content"><div class="modal-header">' +
                        
            + '<h5>' + options.caption + '</h5>' + '<a class="btn-close" data-bs-dismiss="modal" aria-label="Close"> </a></div>' +
                   '<div class="modal-body">'
                        + options.body + '</div>' +
                   '<div class="modal-footer">'
                        + '<a href="#" class="btn btn-success" id="confirmBtn">' + options.confirmText + '</a>'
                    + '<a href="#" class="btn btn-default" data-dismiss="modal">' + options.closeText + '</a></div></div></div></div>';

        $(confirmContainer).remove();
        $(html).appendTo('body');
        $(confirmContainer).modal('show');

        $('#confirmBtn', confirmContainer).click(function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();

            if (options.onConfirm)
                options.onConfirm();
            $(confirmContainer).modal('hide');
        });
    };
})(jQuery);
// ]]>