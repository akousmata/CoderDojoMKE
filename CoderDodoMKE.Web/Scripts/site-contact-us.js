var ContactUs = function () {

    var wireupFormSubmit = function () {
        $('#contact-us-submit').click(function (evt) {
            evt.preventDefault();
            $('#contact-us-form').submit();
        });
    };

    var _public = {};
    _public.init = function () {
        wireupFormSubmit();
    };
    return _public;

}();
$(document).ready(ContactUs.init);