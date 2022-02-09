var ManageEnrollment = function () {

    var initCancelButtons = function () {
        $('.enrollment-list-item').each(function (index, element) {
            var enrollmentID = $(this).attr('id');
            $('.cancel-enrollment', this).on('click', function (evt) {
                $.ajax({
                    url: '/Enrollment/Cancel/' + enrollmentID,
                    success: function (data, status, jqXHR) {
                        alert('Cancellation successful');
                        $('.' + enrollmentID).remove();
                    },
                    error: function (jqXHR, status, error) {
                        alert('Could not cancel the enrollment, please contact Coder Dojo MKE to cancel.');
                    }
                })
            });
        });
    }

    var _public = {};
    _public.init = function () {
        initCancelButtons();
    }
    return _public;
}();
$(document).ready(ManageEnrollment.init);