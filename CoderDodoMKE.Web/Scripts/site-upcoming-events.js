var UpcomingEvents = function () {

    var loadEvents = function () {
        $('.dojo-event').each(function (index, element) {
            var eventId = $(this).data('eventbrite');
            var eventUrl = 'https://www.eventbriteapi.com/v3/events/' + eventId + '/';
            var that = this;

            $.ajax({
                method: 'GET',
                url: eventUrl,
                data: {
                    'token': ''
                },
                success: function (data, status, xhr) {
                    
                    var eventDate = data.start.local.split('T');
                    var eventDateParts = eventDate[0].split('-');
                    var formattedDate = eventDateParts[1] + '/' + eventDateParts[2] + '/' + eventDateParts[0];
                    var href = data.url;
                    $(that).children('a.event-date-and-time').prop('href', href).html(data.name.text + ' ' + formattedDate);
                    $.each(data.ticket_classes, function (index, element) {
                        if (element.quantity_total - element.quantity_sold === 0) {
                            $(that).append('<p>Sorry, this ticket type is sold out! Please try selecting a future event.</p>');
                        } else {
                            var available = element.quantity_total - element.quantity_sold;
                            $(that).append('<p class="availability-info"><span class="number-available">' + available + ' ' + element.name + ' Session</span> tickets available</p>');
                        }
                    });
                }
            });
        });        
    };

    var _public = {};
    _public.init = function () {
        loadEvents();
    };

    return _public;
}();
$(document).ready(UpcomingEvents.init);