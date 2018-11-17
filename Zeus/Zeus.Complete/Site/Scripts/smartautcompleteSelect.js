/*
Autocomplete for Rhea

*/



(function ($) {

    var methods = {
        init: function (options) {
            return this.each(function () {
                if ($(this).data('SA'))
                    return resetData();

                // Constants
                var textNotFound = "No match found";
                var textLoadingResults = "Loading results..... ";
                var itemNotFound = {
                    label: textNotFound,
                    value: ''
                };
                var loadRecordsItem = { label: textLoadingResults, value: "" };


                var parent = $(this);
                var autocomplete = null;
                var menu = null;
                var focusedItem = null;
                var pageIndex = -1;
                var lastScrollTop = 0;
                var selectedIndex = 0;
                var lastSelectedIndex = -1;
                var data = new Array();
                var ignoreFocus = false;
                var loading = false;
                var startedLoadingFromScroll = false;
                var selectedText = "";
                var firstTimeLoaded = true;
                var firstItem = "";
                var responseObject = null;
                var requestObject = null;
                var mouseClicked = false;
                var isAutocompleteOpen = false;
                var id = $(this).attr('id');
                var mouseInside = false;
                var parentDiv = parent.parent('div');



                

                


                if (!parent.hasClass('textBoxAutocomplete'))
                    parent.addClass('textBoxAutocomplete');


                // TODO: add logic for read-only.
                /* 
                parent.attr('readonly', true);
                parent.attr('editable', false);
                */


                /*
                // TODO: START combobox adaptation changes
                if (this.tagName.toLowerCase() === "select") {
                var selectAttributes = [];
                var selectOptions = new Array();

                // replace the combo-box with input element so as to call autocomplete method.
                // store all the attributes.
                $(this).each(function () {
                $.each(this.attributes, function () {
                //this.attributes is not a plain object but an array of attribute nodes which contain both name and value.
                if (this.specified) {
                var attribute = this;
                selectAttributes.push(attribute);
                }
                });
                });
                // store all the options
                var selected = null;
                selectOptions = $(this).find('option');
                $(selectOptions).each(function () {
                if ($(this)[0].selected == true)
                selected = this;
                });


                //alert(selectOptions + " " + selected.value + " " + selectAttributes);
                var id = selectAttributes[1].value;
                $(this).replaceWith("<input id='" + id + "' value = '" + selected.text + "'/>");

                parent = $("#" + id);
                }
                //END combobox adaptation changes
                */


                var createDropMask = function () {
                    var dropMask = $("#textBoxAutocomplete-drop-mask");
                    if (dropMask.length == 0) {
                        dropMask = $(document.createElement("div"));
                        dropMask.attr("id", "textBoxAutocomplete-drop-mask")
                        .attr("class", "textBoxAutocomplete-drop-mask")
                        .attr("width", "1024")
                        .attr("height", "768")
                            ;

                        dropMask.hide();
                        dropMask.appendTo($("body"));

                        $('#textBoxAutocomplete-drop-mask').bind("click", function () {
                            //parent.autocomplete('close');
                            alert('closed drop mask');
                        });
                    }
                };

                var startSearching = function (si) {
                    lastScrollTop = autocomplete.scrollTop();
                    if (!si) {
                        var activeElement = autocomplete.find(".ui-state-hover");
                        if (activeElement.length) {
                            activeElement = activeElement.parent();
                            selectedIndex = data.length ? activeElement.parent().children().index(activeElement) : 0;
                        }
                    }
                    else selectedIndex = si;
                    if (!selectedIndex) {
                        lastScrollTop = 0;
                    }
                    parent.autocomplete('search');
                    startedLoadingFromScroll = false;
                };

                //Resets the current search
                var resetData = function () {
                    data = new Array();
                    pageIndex = -1;
                    selectedIndex = 0;
                    lastSelectedIndex = -1;
                    focusedItem = null;
                    loading = false;
                    startedLoadingFromScroll = false;
                    lastScrollTop = 0;
                    autocomplete.scrollTop(0);
                    parent.removeAttr('sa-value').autocomplete('option', 'disabled', false);
                    //parent.data().autocomplete.term = null;

                };

                //JS event cancelling logic
                var stopEvent = function (e) {
                    if (e.stopPropagation)
                        e.stopPropagation();
                    else e.cancelBubble = true;
                    if (e.preventDefault)
                        e.preventDefault();
                    else e.returnValue = false;
                    return false;
                };



                // Overwrite the source logic to call our getDataFunc function
                options.source = function (request, response) {

                    responseObject = response;
                    requestObject = request;
                    // Don't start another search while a search is still in progress
                    if (loading) {

                        return;
                    }
                    // Verify that options.getDataFunc exists and it is a function
                    if (options.getDataFunc && typeof (options.getDataFunc) === "function") {
                        //If the selected item has not changed, we return
                        if (lastSelectedIndex == selectedIndex && mouseClicked == false)
                            return;
                        mouseClicked = false;
                        lastSelectedIndex = selectedIndex;

                        // Get the term we are searching for
                        var term = firstTimeLoaded === true ? parent.attr('sa-value') : parent.attr('sa-value') || "";

                        {
                            loading = true;


                            if (firstTimeLoaded == false) {
                                $(parent).addClass('ui-autocomplete-loading-s');
                                //                                $('.ui-autocomplete').addClass('waitCursor');


                            }

                            // Call the function to get the data
                            options.getDataFunc(term, pageIndex + 1, options.pageSize, function (r) {
                                loading = false;

                                if (r) {

                                    // Allow scrolling
                                    if (!r.length) {
                                        startedLoadingFromScroll = false;
                                    }



                                    // If data already exists, we add our new data to the existing data
                                    if (data.length)
                                        for (var i = 0; i < r.length; i++) {
                                            //check if data contains r
                                            if (data.indexOf(r[i]) == -1)
                                                data.push(r[i]);
                                        }
                                    else {
                                        data = new Array();
                                        data = r;
                                    }

                                    //adding record 'item not found' logic
                                    if (data.length == 0 && firstTimeLoaded == false) {

                                        data.push(itemNotFound);
                                    } else {
                                        data = jQuery.grep(data, function (value) {
                                            return value != itemNotFound;
                                        });

                                    }




                                }
                                setTimeout(function () {
                                    // sets timeout

                                    response(data);

                                    // Scroll to the last position
                                    autocomplete.scrollTop(lastScrollTop);

                                    // Increment the current page index
                                    pageIndex++;

                                    firstTimeLoaded = false; //firstTime set to false 

                                    $('.ui-autocomplete').removeClass('waitCursor');
                                    $(parent).removeClass('ui-autocomplete-loading-s');

                                }, 150);

                            });
                        }
                    }
                };


                $(parent)
                    .bind('hover', function () {
                        mouseInside = true;
                    }, function () {
                        mouseInside = false;
                    })

                    .bind('autocompletecreate', function () {

                        // Get the elements created by JqueryUI Autocomplete
                        autocomplete = $(this).autocomplete('widget');
                        menu = $(this).data().autocomplete.menu.element;

                        firstItem = parent.val();

                        if (autocomplete.attr('sa-scroll') != 'on') {
                            // Create the scrolling functionality to request new data when we arrived at the end of list
                            autocomplete.scroll(function (e) {
                                if (loading)
                                    return stopEvent(e);
                                if (startedLoadingFromScroll) {
                                    if ($.browser.msie || $.browser.mozilla)
                                        autocomplete.scrollTop(lastScrollTop);
                                    return stopEvent(e);
                                }
                                if (autocomplete[0].scrollHeight - autocomplete.scrollTop() <= autocomplete.outerHeight()) {
                                    startedLoadingFromScroll = true;
                                    startSearching(Math.max(autocomplete.find(".ui-menu-item").length - 1, 0));
                                }
                            }).attr('sa-scroll', 'on');

                            // After releasing the mouse just after getting new data, we release the scrolling functionality
                            $(document).mousemove(function (e) {
                                if (!e.buttons)
                                    startedLoadingFromScroll = false;
                            });
                        }
                        startSearching();
                    })



                    .bind('autocompletefocus', function (event, ui) {
                        focusedItem = ui.item;
                        selectedText = focusedItem !== undefined ? focusedItem.value : '';
                        if (ignoreFocus) {
                            ignoreFocus = false;
                            return;
                        }
                        // If reached the last element in the list, get new data
                        if (autocomplete.find(".ui-menu-item:last .ui-state-hover").length)
                            startSearching();
                    })

                    .autocomplete(options)

                    .keyup(function (e) {
                        if (e.keyCode == 27) //escape
                            return resetData();
                        if (e.keyCode == 38 || e.keyCode == 40 || focusedItem && focusedItem.label == parent.val()) {//
                            return;
                        }
                        if (parent.attr('sa-value') != parent.val())
                            resetData();
                        parent.attr('sa-value', parent.val());
                    })

                // Bind event to force user to select something from list
                    .bind('focusout', function (event, ui) {

                        if (parent.val() !== selectedText) {
                            parent.val('');
                            parent.attr('sa-value', '');
                        }
                        if (event.target.id != this.id) {
                            resetData();
                        }

                    })


                    .bind('autocompleteopen', function (event, ui) {
                        createDropMask();

                        if (!selectedIndex) {
                            if (options.autoFocus) {
                                ignoreFocus = true;
                                menu.menu('activate', event, autocomplete.find(".ui-menu-item:first"));
                            }
                        } else {
                            ignoreFocus = true;
                            menu.menu('activate', event, autocomplete.find(".ui-menu-item:eq(" + selectedIndex + ")"));

                        }
                        if (autocomplete.find(".ui-menu-item:first")[0].innerText !== textNotFound) {
                            isAutocompleteOpen = true;
                        } else {
                            isAutocompleteOpen = false;
                        }
                    })
                    .bind('autocompleteclose', function (event, ui) {
                        //When the dropdown closes, reset the current search 
                        resetData();
                        isAutocompleteOpen = false;
                    })
                    .data('SA', this);




                // Logic for closing drop down upon click
                $("html").bind('click', function (e) {
                    //textBoxAutocomplete-drop-mask
                    if (mouseInside === false) {
                        parent.autocomplete('close');
                        isAutocompleteOpen = false;

                        /*
                        // remove open class
                        var buttonAutocomplete = $('#' + id + 'Button');
                        if ( $(buttonAutocomplete).hasClass('open')) {
                        $(this).removeClass('open');
                        }
                        */
                    }
                });



                // Add BUTTON logic
                parent = $("#" + id);
                var button = "<button class='autocompleteButton' tabIndex='-1' type='button' id='" + id + "Button' />";
                $(button).insertAfter(parent);

                //TODO: if button is readonly, do not bind event

                $("#" + id + "Button")
                .click(function (e) {
                    mouseClicked = true;
                    if (isAutocompleteOpen === false) {

                        //Add open class to show change in button arrow
                        // if (!$(this).hasClass('open'))
                        // $(this).addClass('open');

                        pageIndex = -1;
                        options.source(requestObject, responseObject);
                    } else {
                        e.stopPropagation();
                        e.preventDefault();
                    }

                });

                // TODO: add logic for read-only button.
                /* 
                $("#" + id + "Button").attr('disabled', true); 
                */

                
                // Encapsulate this textbox in DIV
                parent.next('button').andSelf().wrapAll('<div class="wrapperDiv" />');

            });
        },
        destroy: function () {
            $(this).removeData('SA').autocomplete('destroy');
        }



    };




    /*The jQuery object gets these methods from the $.fn object. 
    This object contains all of the jQuery object methods, and if we want to write our own methods, 
    it will need to contain those as well.
    */

    // Name this plugin smartautocompleteSelect
    $.fn.smartautocompleteSelect = function (method) {
        /* Notice that to use another method inside, we use this, not $( this ). This is because our function (plugin) is a part of the same object as other methods. */

        //Method calling logic as described in jQuery Plugin Authoring documentation: http://docs.jquery.com/Plugins/Authoring
        if (methods[method])
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        else if (typeof method === 'object' || !method)
            return methods.init.apply(this, arguments);
        else $.error('Method ' + method + ' does not exist on jQuery.smartautocompleteSelect');
    };
})(jQuery);
 