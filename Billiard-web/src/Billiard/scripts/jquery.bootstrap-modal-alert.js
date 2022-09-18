// <![CDATA[
(function ($) {
    $.bootstrapModalAlert = function (options) {
        var defaults = {
            caption: 'تائید عملیات',
            body: 'آیا عملیات درخواستی اجرا شود؟'
        };
        options = $.extend(defaults, options);

        var alertContainer = "#alertContainer";
        var html = '<div class="modal fade" id="alertContainer"  tabindex="-1" role="dialog" aria-labelledby="ModalLabel" data-bs-backdrop="static" data-bs-keyboard="false" aria-hidden="true">'+
            '<div class="modal-dialog"><div class="modal-content"><div class="modal-header">'
            + '<h5 id="ModalLabel">' + options.caption + '</h5>' + '<a class="btn-close" data-bs-dismiss="modal" aria-label="Close"></a></div>' +
            '<div class="modal-body">'
            + options.body + '</div></div></div></div></div>';

        $(alertContainer).modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();

        $(alertContainer).remove();
        $(html).appendTo('body');
        $(alertContainer).modal('show');
    };
})(jQuery);
// ]]>