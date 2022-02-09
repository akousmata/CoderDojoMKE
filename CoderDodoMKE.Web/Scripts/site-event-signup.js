var EventSignup = function () {

    var emptyGuid = '00000000-0000-0000-0000-000000000000';
    var newNinjaDDL;
    var localSelected = [];
    var localSelectable = [];

    var initPreselectedDropdowns = function () {
        // Check that the array of JSON objects was created properly before init'ing. 
        // This is being done on the base Signup.cshtml page
        if (typeof selectableEnrollees != "undefined" && selectableEnrollees.length) {
            
            // Build out our local lists that will keep track of what is selected
            // and what is not selected
            var dropdowns = $('.ninja-enrollee-list');
            var selectedNinjas = 0;
            $.each(selectableEnrollees, function(index, element){
                if (element.Selected == 'true') {
                    selectedNinjas++;
                    localSelected.push(element);
                } else {
                    localSelectable.push(element);
                }
            });

            // If the number of dropdowns is equal to the number of selected enrollees, 
            // then the view/models was created correctly, it is safe to proceed.
            if (dropdowns.length == selectedNinjas) {
                $.each(dropdowns, function (index, element) {
                    // Rebuild each of the dropdowns to contain a unique selection
                    // and any additional selectable items.
                    $(element).empty();
                    $(element).append($('<option />', { value: localSelected[index].ID, text: localSelected[index].Name, selected: 'selected' }));
                    $(element).data("prev", localSelected[index]);
                    $.each(localSelectable, function (index, el) {
                        $(element).append($('<option />', { value: localSelectable[index].ID, text: localSelectable[index].Name }));
                    })
                });
            }
        }
    }

    var initNinjaDropdownOnChange = function() {
        $('.ninja-enrollee-list').each(function (index, element) {
            $(element).on('change', function (evt) {
                // Check for the empty Guid and trigger the creation dialog if it's an empty guid
                var selectedOption = $('option:selected', element);                
                if ($(selectedOption).val() == emptyGuid) {
                    $('#createNewModal').modal();
                    $('.ninja-seat-overflow').remove();
                    $('#NumberOfEnrollees option[value="' + $('.ninja-enrollee-list').length + '"]').prop('selected', true)
                    newNinjaDDL = element;
                    return;

                }

                swapDropdowns(selectedOption, element);                
            });            
        });
    }

    var initCreateNinjaCancelButton = function () {
        $('#createNinjaCancel').on('click', function (evt) {
            // Check if there was an original DDL and reset it
            if (newNinjaDDL) {
                refreshDropdowns(true);
            }
        });
    }

    var removeArrayItem = function (arr, id) {
        for (var i = arr.length - 1; i >= 0; i--) {
            if (arr[i].ID == id) {
                arr.splice(i, 1);
            }
        }
    }

    var swapDropdowns = function (selectedOption, originalElement) {
        // Prep all the data we are about to swap around               
        var originalItem = $(originalElement).data('prev');
        var selectedID = $(selectedOption).val();
        var selectedText = $(selectedOption).text();

        // Remove the item from the selectable list
        removeArrayItem(localSelectable, selectedID);

        // Remove the previously selected item from the selected list
        removeArrayItem(localSelected, originalItem.ID);

        // Add the newly selected item to the selected list and update elements data store
        var selectedItem = {
            ID: selectedID,
            Name: selectedText
        };
        localSelected.push(selectedItem);
        $(originalElement).data('prev', selectedItem);

        // Add the original item back into the selectable list
        var selectableItem = {
            ID: originalItem.ID,
            Name: originalItem.Name
        };
        localSelectable.push(selectableItem);

        // notify other dropdowns to clear their own lists and reset
        refreshDropdowns();
    }
    
    var refreshDropdowns = function (usePrevious) {
        $('.ninja-enrollee-list').each(function (index, element) {
            // Rebuild each of the dropdowns using its original selection
            // plus any additional selectable items.
            var selectedOption = $('option:selected', element);
            $(element).empty();

            if (usePrevious || $(selectedOption).val() == emptyGuid) {
                var previousData = $(element).data('prev');
                $(element).append($('<option />', {value: previousData.ID, text: previousData.Name,  selected: 'selected' }));
            } else {
                $(element).append($(selectedOption, { selected: 'selected' }));
            }
                      
            $.each(localSelectable, function (index, el) {
                if (localSelectable[index].ID != emptyGuid) {
                    $(element).append($('<option />', { value: localSelectable[index].ID, text: localSelectable[index].Name }));
                }
            })
            $(element).append($('<option />', { value: emptyGuid, text: "Create new..." }));
        });
    }

    var _public = {};
    _public.init = function () {
        initPreselectedDropdowns();
        initNinjaDropdownOnChange();
        initCreateNinjaCancelButton();
    };
    
    _public.handleNinjaCreation = function (data, status, jqXHR) {        
        $('#createNewModal').modal('hide');
        if (newNinjaDDL) {
            var selectedOption = $('<option />', { value: data.ID, text: data.Name, selected: 'selected' })
            swapDropdowns(selectedOption, newNinjaDDL);
        }
    };

    return _public;
}();
$(document).ready(EventSignup.init);