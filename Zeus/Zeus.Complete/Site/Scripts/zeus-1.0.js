/* Colors taken from dashboard.js of the Color admin theme */
var blue = '#348fe2',
    blueLight = '#5da5e8',
    blueDark = '#1993E4',
    aqua = '#49b6d6',
    aquaLight = '#6dc5de',
    aquaDark = '#3a92ab',
    green = '#00acac',
    greenLight = '#33bdbd',
    greenDark = '#008a8a',
    orange = '#f59c1a',
    orangeLight = '#f7b048',
    orangeDark = '#c47d15',
    dark = '#2d353c',
    grey = '#b6c2c9',
    purple = '#727cb6',
    purpleLight = '#8e96c5',
    purpleDark = '#5b6392',
    red = '#ff5b57';
//var graphColourPalette = [blue, aqua, green, purple, orange, red, grey, dark, aquaDark, greenDark, purpleDark, orangeDark, dark];

(function ($) {

    // Custom function to check if an event is already bound on an element
    $.fn.hasEvent = function (event) {

        if (this.data('events') != undefined) {

            var firstSeparator = event.indexOf('.');
            
            if (firstSeparator > 0) {
                var namespace = event.substring(firstSeparator + 1, event.length);
                event = event.substring(0, firstSeparator);

                if (this.data('events')[event] != undefined) {
                    for (var i = 0; i < this.data('events')[event].length; i++) {
                        if (this.data('events')[event][i].namespace == namespace) {
                            return true;
                        }
                    }
                }

                return false;
            }
            
            return this.data('events')[event] != undefined;
        }

        return false;
    };
})(jQuery);

; (function ($, window, document, undefined) {

    $.zeusValidate = function() { };

    $.zeusValidate.extractNumber = function (value, element) {
        if (!$(element).hasClass('rhea-numeric')) {
            return value;
        }

        // Ignore '$', '-', and ',' characters
        value = value.replace(/[(\,|\-|\$)]/g, "");

        // Ignore decimals as we only want to check the whole number for length validation
        var decimals = parseInt($(element).data(dataTypes.Decimal));

        if (decimals > 0) {
            var decimalStart = value.indexOf('.');
            if (decimalStart > 0) {
                value = value.substr(0, decimalStart);
            }
        }

        return value;
    };
    
    $.zeusValidate.handleDateTimeComparison = function(currentProperty, currentPropertyValue, dependentProperty, dependentPropertyValue) {
        
        var currentDateTime = $.zeusValidate.parseDateTimeValue(currentProperty, currentPropertyValue);
        var dependentDateTime = $.zeusValidate.parseDateTimeValue(dependentProperty, dependentPropertyValue);
        
        var result = { };
        
        result.currentProperty = currentProperty;
        result.currentPropertyValue = currentDateTime.value;
        
        result.dependentProperty = dependentProperty;
        result.dependentPropertyValue = dependentDateTime.value;
        
        // Handle comparing Date Time
        if (currentDateTime.datatype != 'other' && currentDateTime.datatype != 'other') {
            // If comparing Date against DateTime (or vice-versa), reset Time component to effectively ignore it
            if ((currentDateTime.datatype == 'date' && dependentDateTime.datatype == 'datetime')
                || (currentDateTime.datatype == 'datetime' && dependentDateTime.datatype == 'date')) {
                var currentDate = new Date(currentDateTime.value);
                
                currentDate.setHours(0);
                currentDate.setMinutes(0);
                currentDate.setSeconds(0);
                currentDate.setMilliseconds(0);
                
                result.currentPropertyValue = Date.parse(currentDate.toString());
                
                var dependentDate = new Date(dependentDateTime.value);
                
                dependentDate.setHours(0);
                dependentDate.setMinutes(0);
                dependentDate.setSeconds(0);
                dependentDate.setMilliseconds(0);

                result.dependentPropertyValue = Date.parse(dependentDate.toString());
                
                // If comparing Time against DateTime (or vice-versa), reset Date component to effectively ignore it
            } else if ((currentDateTime.datatype == 'time' && dependentDateTime.datatype == 'datetime')
                || (currentDateTime.datatype == 'datetime' && dependentDateTime.datatype == 'time')) {
                var currentDate = new Date(currentDateTime.value);

                currentDate.setFullYear(1970);
                currentDate.setMonth(1);
                currentDate.setDate(1);

                result.currentPropertyValue = Date.parse(currentDate.toString());
                
                var dependentDate = new Date(dependentDateTime.value);
                
                dependentDate.setFullYear(1970);
                dependentDate.setMonth(1);
                dependentDate.setDate(1);

                result.dependentPropertyValue = Date.parse(dependentDate.toString());
            }
        }

        return result;
    };
    
    // Convert a DateTime/Date/Time to its parsed date value
    $.zeusValidate.parseDateTimeValue = function(element, value) {
        var datePicker = $(element).data(dataTypes.DatePicker);
        var dateTimePicker = $(element).data(dataTypes.DateTimePicker);
        var timePicker = $(element).data(dataTypes.TimePicker);
        var datetimeticks = $(element).data(dataTypes.DateTimeTicks);
        var datetimetype = $(element).data(dataTypes.DateTimeType);
        
        var result = { };

        result.datatype = 'other';
        result.value = value;
        
        if (datePicker != undefined) {
            result.datatype = 'date';
            result.value = datePicker.selectedValue != null ? datePicker.selectedValue.getTime() / 1000 : value;
        } else if (dateTimePicker != undefined) {
            result.datatype = 'datetime';
            result.value = dateTimePicker.selectedValue != null ? dateTimePicker.selectedValue.getTime() / 1000 : value;
        } else if (timePicker != undefined) {
            result.datatype = 'time';
            result.value = timePicker.selectedValue != null ? timePicker.selectedValue.getTime() / 1000 : value;
        } else if (datetimeticks != undefined && datetimetype != undefined) {
            result.datatype = datetimetype;
            result.value = parseInt(datetimeticks);
        }

        // Revert to original value if not parsed correctly
        if (isNaN(result.value)) {
            result.value = value;
        }
        
        return result;
    };
    
    $.zeusValidate.is = function(value1, operator, value2, passOnNull, failOnNull) {
        if ( /^true$/i .test(passOnNull)) {

            var value1nullish = this.isNullish(value1);
            var value2nullish = this.isNullish(value2);

            if (value1nullish || value2nullish) {
                return true;
            }
        }

        if ( /^true$/i .test(failOnNull)) {

            var value1nullish = this.isNullish(value1);
            var value2nullish = this.isNullish(value2);

            if (value1nullish || value2nullish) {
                return false;
            }
        }

        var values1Array = [];
        if ($.isArray(value1)) {
            values1Array = value1;
        } else {
            values1Array.push(value1);
        }

        var values2Array = [];
        if ($.isArray(value2)) {
            values2Array = value2;
        } else {
            values2Array.push(value2);
        }
        var results = [];

        for (var i = 0; i < values1Array.length; i++) {
            for (var j = 0; j < values2Array.length; j++) {
                results.push(this.comparisonTest(values1Array[i], operator, values2Array[j]));
                //results.push(this.comparisonTest(value1, operator, value2[i]));
                
            }
        }

        if (operator == "NotEqualTo") {
            // Negative AND validation (all must be true)
            return $.inArray(false, results) == -1; // array must not contain false.
        } else {
            // Positive OR validation (at least one must be true)
            return $.inArray(true, results) != -1;  // array must at least contain one true.
        }


       
    };
    
    $.zeusValidate.comparisonTest = function(value1, operator, value2) {
        if (this.isDate(value1)) {
            value1 = isNaN(Date.parse(value1)) ? Date.parse('01/01/0001 ' + value1) : Date.parse(value1);
            value2 = isNaN(Date.parse(value2)) ? Date.parse('01/01/0001 ' + value2) : Date.parse(value2);
        } else if (this.isBool(value1)) {
            if (/^true$/i.test(value1)) {
                value1 = true;
            } else if (/^false$/i.test(value1)) {
                value1 = false;
            }
            
            if (/^true$/i.test(value2)) {
                value2 = true;
            } else if (/^false$/i.test(value2)) {
                value2 = false;
            }
            
            value1 = !!value1;
            value2 = !!value2;
        } else if (this.isNumeric(value1)) {
            value1 = parseFloat(value1);
            value2 = parseFloat(value2);
        }

        switch (operator) {
            case "EqualTo":
                if (value1 == value2) return true;
                break;
            case "NotEqualTo":
                if (value1 != value2) return true;
                break;
            case "GreaterThan":
                if (value1 > value2) return true;
                break;
            case "LessThan":
                if (value1 < value2) return true;
                break;
            case "GreaterThanOrEqualTo":
                if (value1 >= value2) return true;
                break;
            case "LessThanOrEqualTo":
                if (value1 <= value2) return true;
                break;
            case "RegExMatch":
                return (new RegExp(value2)).test(value1);
                break;
            case "NotRegExMatch":
                return !(new RegExp(value2)).test(value1);
                break;
        }

        return false;
    };

    $.zeusValidate.getId = function(element, dependentProperty) {
        var pos = element.id.lastIndexOf("_") + 1;
        return element.id.substr(0, pos) + dependentProperty;
    };

    $.zeusValidate.getName = function(element, dependentProperty) {
        var pos = element.name.lastIndexOf(".") + 1;
        return element.name.substr(0, pos) + dependentProperty;
    };

    $.zeusValidate.isNullish = function(input) {
        return input == null || input == undefined || $.trim(input) == "";
    };

    $.zeusValidate.isNumeric = function(input) {
        return (input - 0) == input && input.length > 0;
    };

    $.zeusValidate.isInteger = function(input) {
        return /^\d+$/ .test(input);
    };

    $.zeusValidate.isFloat = function(input) {
        return /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/ .test(input);
    };

    $.zeusValidate.isDate = function(input) {
        var dateTest = new RegExp( /^(?=\d)(?:(?!(?:(?:0?[5-9]|1[0-4])(?:\.|-|\/)10(?:\.|-|\/)(?:1582))|(?:(?:0?[3-9]|1[0-3])(?:\.|-|\/)0?9(?:\.|-|\/)(?:1752)))(31(?!(?:\.|-|\/)(?:0?[2469]|11))|30(?!(?:\.|-|\/)0?2)|(?:29(?:(?!(?:\.|-|\/)0?2(?:\.|-|\/))|(?=\D0?2\D(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|2[0-8]|1\d|0?[1-9])([-.\/])(1[012]|(?:0?[1-9]))\2((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?:$|(?=\x20\d)\x20)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$/ );
        
        return dateTest.test(input);
    };

    $.zeusValidate.isBool = function(input) {
        return /^true$/i.test(input) || /^false$/i.test(input);
    };

    $.zeusValidate.isJSON = function(input) {
        try {
            var parsedValue = $.parseJSON(input);
            if(typeof parsedValue != 'object')
            {
                return false;
            }
        } catch(e) {
            return false;
        }
        
        return true;
    };
    
    $.zeusValidate.getFieldPrefixFromName = function(property, name) {
        var delimiter = '.';
        var prefix = '';

        if (property.name.lastIndexOf(delimiter) != -1) {
            prefix = property.name.substring(0, property.name.lastIndexOf(delimiter) + 1);
        }

        return prefix + name;
    };

    $.zeusValidate.getFieldPrefixFromId = function(property, id) {

        var propertyId = property.id;
        
        // Strip suffix number for radio button elements
        if (propertyId.indexOf('ContainerFor-') == - 1 && propertyId.lastIndexOf('-') != - 1) {
            propertyId = propertyId.substring(0, propertyId.lastIndexOf('-'));
        }

        var delimiter = '_';
        var prefix = '';

        if (propertyId.lastIndexOf(delimiter) != -1) {
            prefix = propertyId.substring(0, propertyId.lastIndexOf(delimiter) + 1);
        }

        return prefix + id;
    };
    
    $.zeusValidate.replaceAll = function(text, target, replacement) {
        while (text.indexOf(target) != -1) {
            text = text.replace(target, replacement);
        }

        return text;
    };

    $.zeusValidate.extractTitle = function(text) {
        var responseText = text != undefined ? text.toLowerCase() : '';

        var titleOpenTag = '<title>';
        var titleCloseTag = '</title>';
        var titleOpenIndex = responseText.indexOf(titleOpenTag);
        var titleCloseIndex = responseText.indexOf(titleCloseTag);
        var title = false;
        
        if (titleOpenIndex > -1 && titleCloseIndex > -1) {
            var start = titleOpenIndex + titleOpenTag.length;
            var end = titleCloseIndex - start;
                    
            title = responseText.substr(start, end);
        }

        return title;
    };

    $.zeusValidate.addError = function(error) {
        var container = $('#main_form').find("#validation-error-summary"),//[data-valmsg-summary=true]
            list = container.find("ul");
        //alert("add error");
        list.empty();
        $("<li />").append(error).appendTo(list);

        if (container.hasClass('validation-summary-valid')) {
            container.addClass("validation-summary-errors")
                .addClass("alert")
                .addClass("alert-danger")
                .removeClass("validation-summary-valid")
                .removeClass("noErrors");
        }
    };
    
    $.zeusValidate.addPropertyError = function(propertyId, propertyName, error) {
        var container = $('#main_form').find("[data-valmsg-summary=true]"),
            list = container.find("ul");
        //alert('adding property error');
        list.empty();
        //alert('add Property error');
        $("<li />").append('<a href="#' + propertyId + '">' + propertyName + '</a> - ' + error).appendTo(list);

        if (container.hasClass('validation-summary-valid')) {
            container.addClass("validation-summary-errors")
                .addClass("alert")
                .addClass("alert-danger")
                .removeClass("noErrors")
                .removeClass("validation-summary-valid");
        }
    };

    $.zeusValidate.ignoreDirty = false;
    $.zeusValidate.alwaysIgnoreDirty = false;
    $.zeusValidate.initialContentHTML = undefined;
    $.zeusValidate.skipNextFocusErrors = false;
    $.zeusValidate.useHighContrast = (/^true$/i.test($('#accessibility_settings').data('high-contrast'))) ? true : false;
    $.zeusValidate.blockUIoptions = { overlayCSS: { backgroundColor: 'transparent' }, css: { padding:'15px 15px 15px 15px', backgroundColor: ($.zeusValidate.useHighContrast ? '#5E8E3F' : '#ffffff'), border:'solid 1px ' + ($.zeusValidate.useHighContrast ? '#5E8E3F' : '#b3c3ce'), color: ($.zeusValidate.useHighContrast ? '#000000' : '#446C86') } };
    $.zeusValidate.blockUIdefaultMessage = '<div class="msgInfo">Retrieving data please wait</div>';
    
    var pluginName = 'zeus',
        defaultOptions = {};

    // Data types that include the 'data-' prefix
    var fullDataTypes = {
        FieldPrefix: 'data-zfp',
        ActionForDependencyType: 'data-zadt',
        DependentProperty: 'data-zdp',
        ComparisonType: 'data-zct',
        DependentValue: 'data-zdv',
        Type: 'data-zt',
        DependentPropertyVisibleIf: 'data-zdpv',
        ComparisonTypeVisibleIf: 'data-zctv',
        DependentValueVisibleIf: 'data-zdvv',
        PassOnNullVisibleIf: 'data-zpnv',
        FailOnNullVisibleIf: 'data-zfnv',
        DependentPropertyReadOnlyIf: 'data-zdpr',
        ComparisonTypeReadOnlyIf: 'data-zctr',
        DependentValueReadOnlyIf: 'data-zdvr',
        PassOnNullReadOnlyIf: 'data-zpnr',
        FailOnNullReadOnlyIf: 'data-zfnr',
        DependentPropertyEditableIf: 'data-zdpe',
        ComparisonTypeEditableIf: 'data-zcte',
        DependentValueEditableIf: 'data-zdve',
        PassOnNullEditableIf: 'data-zpne',
        FailOnNullEditableIf: 'data-zfne',
        DependentPropertyClearIf: 'data-zdpc',
        ComparisonTypeClearIf: 'data-zctc',
        DependentValueClearIf: 'data-zdvc',
        PassOnNullClearIf: 'data-zpnc',
        FailOnNullClearIf: 'data-zfnc',
        AlwaysClearIf: 'data-zac',
        DependentPropertyGst: 'data-zdpg',
        ExclusiveGst: 'data-zeg',
        DependentPropertyAge: 'data-zdpa',
        DependentPropertiesCopy: 'data-zdpsc',
        ReadOnlyType: 'data-zrot',
        SubmitName: 'data-zsn',
        SubmitType: 'data-zst',
        Url: 'data-zu',
        LatitudeLongitude: 'data-zll',
        IsNullable: 'data-zin',
        Decimal: 'data-zde',
        IsComplexType: 'data-zict',
        Parameters: 'data-zp',
        SkipLink: 'data-zsl',
        SkipLinkTable: 'data-zslt',
        RadioButtonGroup: 'data-zrbg',
        CheckboxList: 'data-zcl',
        PinnedCount: 'data-zpc',
        PropertyIdGrid: 'data-zpig',
        Countdown: 'data-zcd',
        PagedMetadata: 'data-zpm',
        ObjectValues: 'data-zov',
        HistoryType: 'data-zht',
        HistoryDescription: 'data-zhd',
        ButtonClear: 'data-zbc',
        IsDownload: 'data-zid',
        Toggle: 'data-toggle',
        ErrorTipFor: 'data-zet',
        DisplayName: 'data-zdn',
        DependentPropertyId: 'data-zdpi',
        DependentValueAdw: 'data-zdva',
        ExcludeValues: 'data-zev',
        Code: 'data-zc',
        Dominant: 'data-zd',
        OrderType: 'data-zot',
        DisplayType: 'data-zdt',
        MultiSelect: 'data-zms',
        EmptyMessage: 'data-zem',
        Switcher: 'data-zs',
        SwitcherChecked: 'data-zsc',
        SwitcherUnchecked: 'data-zsu',
        PagedMetadataPropertyId: 'data-zpmpi',
        PagedHasMore: 'data-zphm',
        SkipUnsavedChanges: 'data-zsuc',
        SkipValidation: 'data-zsv',
        DatePicker: 'data-zdpic',
        TimePicker: 'data-ztpi',
        DateTimePicker: 'data-zdtpi',
        DateTimeTicks: 'data-zdtti',
        DateTimeType: 'data-zdtty',
        DateTime: 'data-zdate',
        Click: 'data-zck',
        PropertyNameForAjax: 'data-zpnfa',
        GraphTopLevelUrl: 'data-zgtlu',
        GraphDrillDownUrl: 'data-zgddu',
        GraphType: 'data-zgt',
        WidgetContext: 'data-zwc',
        WidgetDataContext: 'data-zwdc',
        CalendarCategory: 'data-zcalc',
        CalendarData: "data-zcald",
        CalendarCategoryEventList: 'data-zcalj',
        CalendarRendered: 'data-zcal',
        CalendarDefaultView: 'data-zcalv',
        CalendarDragResizeAction: "data-zucdr",
        CalendarEventAddBtn: 'data-zcal-add',
        AjaxRoutes: 'data-zr'
    };

    // Same as fullDataTypes but without the 'data-' prefix (populated in constructor)
    var dataTypes = {};

    function Zeus(element, options) {
        
        this.element = element;
        
        // Merge passed options with default options
        this.options = $.extend({}, defaultOptions, options);

        this._defaultOptions = defaultOptions;
        this._name = pluginName;

        // Populate dataTypes from fullDataTypes but without 'data-' prefix
        dataTypes = $.extend({}, fullDataTypes);
        for (var dataType in dataTypes) {
            dataTypes[dataType] = dataTypes[dataType].replace(/^data-/, '');
        }

        // Make data types accessible via jQuery
        $.zeusFullDataTypes = fullDataTypes;
        $.zeusDataTypes = dataTypes;

        this.init();
    }

    Zeus.prototype = {

        init: function () {
            this.globalAjaxHandlers();
            this.prepareReset();
            App.init(); // Color admin theme initialiser. This may need to be removed eventually.
            this.applyBehaviours();
            this.postApplyBehavioursInit();
        },

        // Once only function to register ajax handlers that display messages on blocking ajax calls
        globalAjaxHandlers: function () {
            // Ensure Anti-Forgery token is sent on all Ajax calls
            $.ajaxSetup({ data: { '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val() } });

            // Ajax indicator bound to ajax start/stop document events
            $(document).ajaxStart(function () {
                $.blockUI($.zeusValidate.blockUIoptions);
            });

            $(document).ajaxSend(function () {
                $.blockUI($.zeusValidate.blockUIoptions);
            });

            $(document).ajaxComplete(function (event, request, settings) {
                var title = $.zeusValidate.extractTitle(request.responseText);

                // When the response text contains a <title>
                // and it is not from an application error,
                // and the title does not start with the expected "ESS Web |" title beginning,
                // then we assume the users security token has expired and the response is the STS sign in page.
                // So, reload current page to trigger the STS sign in process in full.
                if (title !== false && request.status != 500 && title.indexOf('ESS Web |') != 0) {
                    $.zeusValidate.addError('Your session has timed out. Please log in again.');
                    $.zeusValidate.alwaysIgnoreDirty = true; // Ignore dirty check
                    window.location.reload();
                }

                $.unblockUI();
                $.zeusValidate.blockUIoptions.message = $.zeusValidate.blockUIdefaultMessage;
            });

            $(document).ajaxStop(function (event, request, settings) {
                $.unblockUI();
                $.zeusValidate.blockUIoptions.message = $.zeusValidate.blockUIdefaultMessage;
            });

            $(document).ajaxSuccess(function (event, request, settings) {
                $.unblockUI();
                $.zeusValidate.blockUIoptions.message = $.zeusValidate.blockUIdefaultMessage;
            });

            $(document).ajaxError(function (event, request, settings) {
                // Handle application error (status 500)
                if (request.status == 500) {
                    $.zeusValidate.addError('The server encountered an internal error and was unable to process your request. Please try again later.');
                }

                $.unblockUI();
                $.zeusValidate.blockUIoptions.message = $.zeusValidate.blockUIdefaultMessage;
            });
        },

        prepareReset: function () {
            // Store current #content HTML for use during reset()
            var content = $('#content');

            if (content.length > 0) {
                $.zeusValidate.initialContentHTML = content.html();
            }
        },

        // Gets new content for the provided contentContainer, contained within the given panel using an ajax request to the provided url.
        // The content container might be the panel-body if you're refreshing the entire panel, or a subcontainer within it.
        // This function also calls prepareNewContent on the returned content to apply behaviours to elements.
        // data should be in JSON form
        getNewContentForPanel: function (panel, contentContainer, url, data) {
            var $rhea = this;
            if (!panel.hasClass('panel-loading')) {
                var spinner = $('<div class="panel-loader"><span class="spinner-small"></span></div>');
                panel.addClass('panel-loading');
                panel.find('.panel-body').prepend(spinner);
                var ajaxOptions = {
                    url: url,
                    global: false, // Prevents the "Please wait" notices set in $.ajaxStart(), $.ajaxSend(), etc. being triggered, as the loading spinner makes them redundant.
                    type: 'GET',
                    success: function (data) {
                        spinner.remove();
                        panel.removeClass('panel-loading');
                        contentContainer.html(data);
                        $rhea.prepareNewContent(contentContainer);
                    },
                    error: function (data) {
                        spinner.remove();
                        panel.removeClass('panel-loading');
                        contentContainer.html('<div>Error loading content</div><div style="display: none">' + /<body.*?>([\s\S]*?)<\/body>/img.exec(data.responseText)[1] + '</div>');
                    }
                };
                if (data !== undefined) {
                    $.extend(ajaxOptions, {
                        type: 'POST',
                        data: data,
                        contentType: 'application/json; charset=utf-8',
                    })
                }
                $.ajax(ajaxOptions);
            }
        },


        // Call this function on new content to apply behaviours to it.
        prepareNewContent: function (newContent) {
            var originalElement = this.element;
            this.element = newContent;
            this.applyBehaviours();
            $.validator.unobtrusive.parseDynamicContent(newContent);
            this.element = originalElement;
        },

        applyBehaviours: function () {
            this.skip();
            this.resize();
            this.maxlength();
            this.primary();
            this.trigger();
            this.visibleif();
            this.readonlyif();
            this.editableif();
            this.clearif();
            this.actionif();
            this.paged();
            this.multiplegridselect();
            //this.sidenavigation();
            this.dropdowns();
            this.senddisabled();
            this.retargetsummary(); // must come after dropdowns()
            this.gst();
            this.age();
            this.copy();
            this.historypin();
            this.numeric();
            this.crn();
            this.reset();
            this.clear();
            this.knockout();
            this.fooTheTable();
            this.gridSortable();
            this.datetimepicker();
            this.datepicker();
            this.timepicker();
            this.focuserrors();
            this.requiredifindicator();
            this.tooltipposition();
            this.smartautocomplete();
            this.switchers();
            this.checkallforcheckboxes();
            this.flotGraphs();
            this.ajaxload();
            this.groupajax();
            this.widgets();
            this.processCalendar();
            this.alerts();
            //this.equalizepanels();
            this.dateBasedContent();
            this.dirtycheck(); // should always be last
        },

        postApplyBehavioursInit: function () {
            // Do initial load of data for widgets
            $('[' + fullDataTypes.Click + '=reload]').trigger("click");
        },

        equalizepanels: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('div.row').each(function () {
                var row = $(this);
                if (row.closest('.zeus-widget-container').length == 0) { // Don't equalise widgets
                    row.equalize({ equalize: 'height', reset: false, children: '.panel' });
                    // change height to min-height
                    row.find('.panel').each(function () {
                        var panel = $(this)
                        panel.css('min-height', panel.css('height'));
                        panel.css('height', '');
                    });
                }
            });
        },
        alerts: function () {
            var alertContainer = $('#zeus-alert');
            
            if (alertContainer.length) {
                var data = alertContainer.data('zeus-alerts');
                
                for (var i = 0; i < data.length; i++) {
                    $.gritter.add({
                        title: '',
                        text: data[i].Text,
                        image: '',
                        sticky: true,
                        time: '',
                        class_name: 'my-sticky-class'
                    });
                }
            }
        },

        primary: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('input[type="text"]').bind('keypress.rhea-primary', function (e) {
                var property = $(this);
                var readonly = property.attr('disabled') || property.attr('readonly');
                
                // On ENTER and not readonly, submit closest primary button, otherwise allow browser to decide
                if (e.which == 13) {
                    
                    if (readonly) {
                        e.preventDefault();
                        return false;
                    }
                    
                    var primary = property.closest('fieldset').find('.primary').not(':hidden')[0];
                    
                    if (primary == undefined) {
                        primary = property.closest('form').find('.primary').not(':hidden')[0];
                    }
                    
                    if (primary != undefined) {
                        primary.click();
                        e.preventDefault();
                        return false;
                    }
                }

                return true;
            });
            

            // manual overriding of Primary Button on Keyboard ENTER.
            /*
             root.find('fieldset').bind('keypress.rhea-primary', function(e) {

                var property = $(this);

                // if e.target.tagName != 'A'

                if (e.target.tagName != 'A') {
                    //on ENTER in fieldset
                    if (e.which == 13) {
                        // find the .primary button within the fieldset
                        var primary = property.find('.primary').not(':hidden')[0];

                        if (primary == undefined) {
                            // find button within the fieldset
                            primary = property.find('button[type="submit"]').not(':hidden')[0];

                            if (primary == undefined) {
                                // find closest button to fieldset
                                primary = property.closest('button[type="submit"]').not(':hidden')[0];

                                if (primary == undefined) {
                                    // find primary button for the form
                                    primary = root.find('.primary').not(':hidden')[0];
                                }
                            }
                        }
                        if (primary != undefined) {
                            primary.click();
                            primary.focus();
                            e.preventDefault();
                            return false;
                        }
                    }
                }
                return true; // prevent browser triggering the button  
            }); 
            */
        },
        
        focuserrors: function () {
            if ($.zeusValidate.skipNextFocusErrors) {
                $.zeusValidate.skipNextFocusErrors = false;
                return;
            }
            
            setTimeout(function () {
                // After page load, if there are any error, warning, information or success messages then focus #content so the messages are read to screen-reader users
                var errors = $('#validation-error-summary ul li');
                var success = $('section.msgGood ul li');
                var warning = $('section.msgWarn ul li');
                var information = $('section.msgInfo ul li');
                
                // Summary exists and is not hidden and contains messages
                if (errors.length > 0 && errors.not(':hidden')) {
                    // Focus main error form header
                    var errorH2 = $('#validation-error-summary h3');
                    
                    if (errorH2.length) {
                        // Temporarily add tabindex to allow focus on non <input>, <a> and <select> element
                        errorH2.attr('tabindex', '-1');
                    
                        // Apply focus
                        errorH2.focus();
                    
                        // Remove tabindex
                        //errorH2.removeAttr('tabindex');
                    }
                } else if (success.length > 0 && success.not(':hidden')) {
                    // Focus success header
                    var successH2 = $('section.msgGood h3');
                
                    if (successH2.length) {
                        // Temporarily add tabindex to allow focus on non <input>, <a> and <select> element
                        successH2.attr('tabindex', '-1');
                    
                        // Apply focus
                        successH2.focus();
                    
                        // Remove tabindex
                        //successH2.removeAttr('tabindex');
                    }
                } else if (warning.length > 0 && warning.not(':hidden')) {
                    // Focus warning header
                    var warningH2 = $('section.msgWarn h3');
                
                    if (warningH2.length) {
                        // Temporarily add tabindex to allow focus on non <input>, <a> and <select> element
                        warningH2.attr('tabindex', '-1');
                    
                        // Apply focus
                        warningH2.focus();
                    
                        // Remove tabindex
                        //warningH2.removeAttr('tabindex');
                    }
                } else if (information.length > 0 && information.not(':hidden')) {
                    // Focus information header
                    var informationH2 = $('section.msgInfo h3');
                
                    if (informationH2.length) {
                        // Temporarily add tabindex to allow focus on non <input>, <a> and <select> element
                        informationH2.attr('tabindex', '-1');
                    
                        // Apply focus
                        informationH2.focus();
                    
                        // Remove tabindex
                        //informationH2.removeAttr('tabindex');
                    }
                }
            }, 1);
        },
        
        resize: function () {
            var scrollHeight = function (e) {
                e.height(0);
                e.height(e[0].scrollHeight);
                e.focus(); // Force focus to fix a problem with the maxlength indicator position when an element resizes
            };
            
            // Resize text area's based on content
            $('textarea').bind('input.rhea-resize', function () {
                scrollHeight($(this));
            }).each(function (index, element) {
                scrollHeight($(element));
            });
        },

        maxlength: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            // Set character counter on elements with maxlength, but not including numeric length elements because bootstrap-maxlength can't handle these.
            root.find('[maxlength]').not('[data-val-range]').each(function () {
                var property = $(this);
                var maxlength = property.attr('maxlength');

                property.maxlength({
                    alwaysShow: true,
                    threshold: maxlength,
                    warningClass: "label label-success",
                    limitReachedClass: "label label-danger",
                    separator: ' of ',
                    preText: 'You have ',
                    postText: ' chars remaining.',
                    validate: true
                });
            });


        },

        skip: function () {
            var content = $('#content');
            
            if (content != undefined) {
                $.zeusValidate.alwaysIgnoreDirty = (/^true$/i.test(content.data('rhea-skipunsavedchanges'))) ? true : false;
                
                // Note: Skip validation is handled in rhea.validate.unobtrusive's ignore function
            }
            
            // [SkipLink] attribute
            //Loop through all the elements with data-rhea-skiplink
            var root = $(this.element) || $(document);
            
            var skipLinks = root.find('[' + fullDataTypes.SkipLink + ']'); //Gets all elements that have data-rhea-skiplink
            if (skipLinks !== undefined) {
                for (var i = 0; i < skipLinks.length; i++) {
                        var skipLinkProp = $(skipLinks[i]);
                        $('#skipLinks').find('ul')[0].innerHTML += '<li><a href=\"#' + skipLinkProp.attr('id') + '\">skip to ' + skipLinkProp.attr(fullDataTypes.DisplayName).toLowerCase() + '</a></li>';
                    } 
            }
             
             //check to see if the page has element with ID = elementName + "Table"
            //this indicates that tables exists which has been assigned skipLink attribute
            var skipLinksTable = root.find('[' + fullDataTypes.SkipLinkTable + ']'); // Gets all elements/grids that have this attribute
            
            if (skipLinksTable !== undefined) {
                for (var j = 0; j < skipLinksTable.length; j++) {

                    var skipLinkPropTable = $(skipLinksTable[j]);
                    var skipLinkPropTableChildTable = $(skipLinkPropTable).find('table')[0];
                    if (skipLinkPropTableChildTable !== undefined) {//check if skipLinkProp has any children in it. That is, if table or results are rendered.
                        var tableIdAssigned = skipLinkPropTableChildTable.id;
                        if (tableIdAssigned !== undefined)  // table with this ID exists
                        { 
                            var skipLinkName = skipLinkPropTable.attr(fullDataTypes.DisplayName);
                            if ($(skipLinkPropTable.prev('legend')).length > 0)
                                skipLinkName = $(skipLinkPropTable.prev('legend'))[0].outerText;
                            $('#skipLinks').find('ul')[0].innerHTML += '<li><a href=\"#' + tableIdAssigned + '\">skip to ' + skipLinkName.toLowerCase() + '</a></li>';
                        }
                    }

                }
            } 



                  
        },
        
        serializeform: function (form) {
            // Find disabled inputs, and remove the "disabled" attribute
            var disabled = form.find(':input:disabled').removeAttr('disabled');

            // Unselect selected items in visible dual select listboxes (the real selected items are in a hidden listbox)
            var available = $('select[multiple][id$="_Available"] option:selected').removeAttr('selected');
            var selected = $('select[multiple][id$="_Selected"] option:selected').removeAttr('selected');
            
            // Serialize the form with the disabled inputs now included
            var serialized = form.serialize();

            // re-select options in each visible dual select listbox that had selected options
            available.attr('selected', 'selected');
            selected.attr('selected', 'selected');
            
            // re-disable the set of inputs that were formerly disabled
            disabled.attr('disabled', 'disabled');

            return serialized;
        },
        
        dirtycheck: function () {
            var $rhea = this;
            
            // Bind links that open in a new window or those that do not navigate so they set the ignoreDirty flag true when triggered
            $('a').each(function () {
                if (this.href == 'javascript:;' || this.href.indexOf('#') == 0 || $(this).hasClass('cancel')) {
                    if (!$(this).hasEvent('click.rhea-ignoredirty')) {
                        $(this).bind('click.rhea-ignoredirty', function () {
                            // Ignore links that open in a new window or those that do not navigate
                            $.zeusValidate.ignoreDirty = true;
                        });
                    }
                }
            });
            
            $('button').each(function () {
                if (!$(this).hasEvent('click.rhea-ignoredirty')) {
                    $(this).bind('click.rhea-ignoredirty', function () {
                        // Ignore links that open in a new window or those that do not navigate
                        $.zeusValidate.ignoreDirty = true;
                    });
                }
            });

            var catcher = function () {
                var quickfindChanged = false;
                var changed = false;

                $('form').each(function () {
                    if ($(this).data('initialForm') != $rhea.serializeform($(this))) {
                        if ($(this).attr('id') == 'quickfind') {
                            quickfindChanged = true;
                        } else {
                            changed = true;
                        }

                        $(this).addClass('changed');
                    } else {
                        $(this).removeClass('changed');
                    }
                });

                // Don't prompt if we are supposed to ignore it
                if ($.zeusValidate.ignoreDirty) {
                    $.zeusValidate.ignoreDirty = false;
                } else if (changed && !$.zeusValidate.alwaysIgnoreDirty) {
                    // Only prompt if a main form has changed (not quickfind) and we're not forcing a reload ourselves
                    return 'You have unsaved changes.';
                }
                
                return undefined;
            };

            $('form').each(function () {
                // Set initial form state before any changes have been made
                $(this).data('initialForm', $rhea.serializeform($(this)));
            }).submit(function () {
                var formElement = this;
                var changed = false;
                $('form').each(function () {
                    // Check if other forms are dirty
                    if (this != formElement && $(this).data('initialForm') != $rhea.serializeform($(this))) {
                        changed = true;
                        $(this).addClass('changed');
                    } else {
                        $(this).removeClass('changed');
                    }
                });
                    
                // Don't prompt the user if they are submitting the main form and the quickfind form is dirty
                if (formElement.id != 'quickfind') {
                    $(window).unbind('beforeunload', catcher);
                }
            });
            
            // Dont bind 'beforeunload' more than once
            if (!$(window).hasEvent('beforeunload')) {
                $(window).bind('beforeunload', catcher);
            }
        },

        senddisabled: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('form').each(function () {
                var form = $(this);

                form.find('button[name="submitType"]').bind('click.rhea-senddisabled', function () {
                    var button = $(this);
                    
                    // Store whether the button submit results in a file download
                    form.data(dataTypes.IsDownload, button.data(dataTypes.IsDownload));
                });
                
                form.submit(function (e) {
                    if (e.isDefaultPrevented()) {
                        // jQuery validation found errors
                        return;
                    }

                    // Change message to indicate data is being submitted
                    $.zeusValidate.blockUIoptions.message = '<div class="msgInfo">Sending data please wait</div>';
                    
                    // Block UI, relying on page to reload or redirect which results in an unblocked page
                    $.blockUI($.zeusValidate.blockUIoptions);

                    // Remove block after a short timeout (2.5 sec) if the button results in a file download ([Button(ResultsInDownload = true)]) as the page will remain as is (no reload/redirect)
                    // Otherwise, remove block after a long timeout (60 sec) so eventually the user will have control again if something went wrong (or they cancelled the submit which there is no event to watch for)
                    var timeout = form.data(dataTypes.IsDownload) ? 1000 * 2.5 : 1000 * 60;
                    
                    setTimeout(function () {
                        $.unblockUI();
                        $.zeusValidate.blockUIoptions.message = $.zeusValidate.blockUIdefaultMessage;
                    }, timeout);
                    
                    // Find disabled inputs, and remove the "disabled" attribute so the value is submitted in the form submit
                    var disabled = $(this).find(':input:disabled').removeAttr('disabled');
                    
                    setTimeout(function () {
                        // Re-disable the set of inputs that were formerly disabled
                        disabled.attr('disabled', 'disabled');
                    }, 1);
                });
            });
        },
        
        retargetsummary: function () {
            // Check if the error targets are actually visible, if not assume target is a select2 so retarget for that
            $('section[data-valmsg-summary=true]').find('a').each(function () {
                var targetID = $(this).attr('href');
                var target = $(targetID);
                
                if (target.length > 0 && target.is(':hidden')) {
                    var selectTarget = $('#s2id_focus_' + target.attr('id'));
                    
                    // Retarget to progressive select2
                    if (selectTarget.length > 0 && selectTarget.is(':visible')) {
                        $(this).attr('href', '#' + selectTarget.attr('id'));
                    } else {
                        selectTarget = $('#s2id_focus_select2_' + target.attr('id'));

                        // Retarget to ajax select2 (adw)
                        if (selectTarget.length > 0 && selectTarget.is(':visible')) {
                            $(this).attr('href', '#' + selectTarget.attr('id'));
                        } else {
                            selectTarget = $('#' + target.attr('id') + '_Selected');
                            
                            // Retarget to dual select '_Selected' listbox
                            if (selectTarget.length > 0 && selectTarget.is(':visible')) {
                                $(this).attr('href', '#' + selectTarget.attr('id'));
                            }
                        }
                    }
                }
            });
        },

        dropdowns: function () {
            this.autocomplete();
            this.hierarchy();
        },

        _autocomplete: function (element) {

            // Ensure drop mask is closed properly on the recreation of select2 elements
            /*var mask = $("#select2-drop-mask");
            if (mask.length > 0) {
                mask.trigger('mousedown');
            }*/
            
            // Send geospatial to correct method
            if ($(element).hasClass('rhea-ajax-geospatial-address')) {
                return this._autocompleteGeospatialAddress(element);
            }
            
            // send job seeker search to correct method
            if ($(element).hasClass('rhea-ajax-jsk-search')) {
                return this._autocompleteJobseekerSearch(element);
            }
            
            if ($(element).data(dataTypes.CheckboxList)) {
                // Ignore checkbox list
                return;
            }
            var $rhea = this;
            var root = $(this.element) || $(document);
            
            // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
            if (element[0].id.length == 0 && element.find('input').length > 0) {
                return;
                // Get nested input property
                element = element.find('input')[0];
                // TODO: On some occasions, the id after this assignment is at element.id which means i should assign just element.find('input') instead of with [0]
            }

            // Always allow clear
            var allowClear = true;
            
            var emptyMessage = element.data(dataTypes.EmptyMessage);
                
            if (emptyMessage != undefined && emptyMessage.length > 0) {
                allowClear = true;
            } else if (element.hasClass('rhea-numeric') && element.data(dataTypes.IsNullable)) {
                allowClear = true;
                emptyMessage = (/^true$/i.test($(this).data(dataTypes.IsNullable))) ? '' : '0';
            }
                
            if (!allowClear) {
                if (element.is('select')) {
                    for (var i = 0; i < element[0].options.length; i++) {
                        if (element[0].options[i].value == '') {
                            emptyMessage = element[0].options[i].text;
                            allowClear = true;
                            element[0].options[i].text = '';
                            break;
                        }
                    }
                }
            }

            var options = {
                width: 'copy',
                allowClear: allowClear,
            };
                
            if (emptyMessage != undefined && emptyMessage.length > 0) {
                options.placeholder = emptyMessage;
            }

            options.placeholder = ' ';

            // Set the Select2 widget to accept multiple values if required.
            // Unfortunately, this has to be done differently for client side selctions (using the select element), and ajax powered selections (which the widget forces to be an 'input', instead)
            if (element.hasClass('rhea-multiple')) {
                if (element.is('input')) options.multiple = true;
            }
            //if (element.is('select') && !element.is('select:not([multiple])')) {
            //        options.multiple = true;
            //}
            
        // TODO: Need to use Parameters in rhea-ajax to setup heirarchy response (on change of Parameter property value, update this one in the same way Adw heirarchy works)
            

            if (element.hasClass('rhea-adw') || element.hasClass('rhea-ajax')) {

                options.id = 'Value';
                var valueFromDependent = undefined;
                
                if (element.hasClass('rhea-adw')) {

                    var dependentValue;
                    if (element.data(dataTypes.DependentPropertyId) != undefined) {
                        dependentValue = $('#' + element.data(dataTypes.DependentPropertyId)).val();

                        if (dependentValue == undefined || dependentValue == null) {
                            dependentValue = $('#select2_' + element.data(dataTypes.DependentPropertyId)).val();
                        }
                        
                        valueFromDependent = dependentValue;
                    } else {
                        dependentValue = element.data(dataTypes.DependentValueAdw);
                    }

                    //valueFromDependent = dependentValue;

                    options.ajax = {
                        cache: true,
                        quietMillis: 1000,
                        type: 'GET',
                        dataType: 'json',
                        url: element.data(dataTypes.Url),
                        traditional: true,
                        data: function (term, page) {
                            return {
                                text: term,
                                page: page,
                                code: element.data(dataTypes.Code),
                                dependentValue: dependentValue,
                                dominant: element.data(dataTypes.Dominant),
                                orderType: element.data(dataTypes.OrderType),
                                displayType: element.data(dataTypes.DisplayType),
                                excludeValues: element.data(dataTypes.ExcludeValues) != undefined ? element.data(dataTypes.ExcludeValues).split(',') : undefined
                            };
                        },
                        results: function (data, page) {
                            return { results: data.result, more: data.more };
                        }
                    };
                    
                    
                    options.formatSelection = function (data) {
                        return data.Text;
                    };


                } else {
                    
                    options.ajax = {
                        cache: true,
                        quietMillis: 1000,
                        type: 'GET',
                        dataType: 'json',
                        url: element.data(dataTypes.Url),
                        data: function (term, page) {
                            var result = {
                                text: term,
                                page: page
                            };

                            var parameters = [];

                            if (element.data(dataTypes.Parameters) != undefined && element.data(dataTypes.Parameters).length > 0) {
                                parameters = element.data(dataTypes.Parameters).split(',');
                            }

                            if (parameters.length > 0) {
                                for (var j = 0; j < parameters.length; j++) {
                                    
                                    var key = parameters[j];

                                    var dependentId = (element.data(dataTypes.FieldPrefix) != undefined && element.data(dataTypes.FieldPrefix).length > 0) ? element.data(dataTypes.FieldPrefix) + '_' + key : key;
                                    
                                    key = key.charAt(0).toLowerCase() + key.slice(1);

                                    result[key] = $('#' + dependentId).val();
                                }
                            }

                            return result;
                        },
                        results: function (data, page) {
                            return { results: data.result, more: data.more };
                        }
                    };

                    var isComplexType = element.data(dataTypes.IsComplexType);

                    options.formatSelection = function (data) {
                         // TODO: for adw-ajax that are string based, this should use data.Value, otherwise data.Text
                        return isComplexType ? data.Text : data.Value;
                    };


                    var select2element = $('#select2_' + element[0].id);
                
                    if (select2element == undefined || select2element.length == 0) {
                       
                        var parameters = [];

                        if (element.data(dataTypes.Parameters) != undefined && element.data(dataTypes.Parameters).length > 0) {
                            parameters = element.data(dataTypes.Parameters).split(',');
                        }

                        if (parameters.length > 0) {
                            // Setup hierarchy
                            for (var i = 0; i < parameters.length; i++) {
                                
                                var dependentId = (element.data(dataTypes.FieldPrefix) != undefined && element.data(dataTypes.FieldPrefix).length > 0) ? element.data(dataTypes.FieldPrefix) + '_' + parameters[i] : parameters[i];
                                
                                var dependentProperty = $('#' + dependentId);
                                
                                dependentProperty.bind('change.rhea-hierarchy', function () {
                                    var dependentValue = dependentProperty.val();

                                    if (dependentValue != '' && dependentValue != null && dependentValue != undefined) {
                                        if (element.attr('disabled')) {
                                            element.removeAttr('disabled');
                                        }
                                        if (element.attr('readonly')) {
                                            element.removeAttr('readonly');
                                        }
                                    } else {
                                        var readOnlyType = element.data(dataTypes.ReadOnlyType);
                                        element.attr(readOnlyType, readOnlyType);
                                    }


                                    var originalVal = element.val();
                                    element.empty();
                                    if (element.is('select')) {
                                        element.append($('<option />').val('').text(''));
                                        element.append($('<option id="' + element[0].id + '_0__Value" name="' + element[0].name + '[0].Value" />').val('').text(''));
                                    }
                                    element.val('');
                                    
                                    if (originalVal != element.val()) {
                                        element.change();
                                    }


                                    //
                                    if (dependentValue == undefined || dependentValue == null || dependentValue == '') {


                                        if (element.is('select')) {

                                            var isComplexType = element.data(dataTypes.IsComplexType);

                                            if (element.hasClass('rhea-ajax') && isComplexType) {
                                                var hiddenValue = $('#' + element[0].id + '_0__Value');

                                                if (hiddenValue != undefined && hiddenValue.length > 0) {
                                                    hiddenValue.remove();
                                                }

                                                var hiddenText = $('#' + element[0].id + '_0__Text');

                                                if (hiddenText != undefined && hiddenText.length > 0) {
                                                    hiddenText.remove();
                                                }
                                            }

                                        }
                                    }

                                    //



                                    $rhea._autocomplete(element);
                                });
                            }
                        }
                    }
                }

                //Formats the text matched with user searched text. Adds class 'select2-match'. Adds underline to matched text in options
                options.formatResult = function (data, container, query) {
                    var markup = [];

                    var text = data.Text;
                    var term = query.term;
                    
                    var match = text.toUpperCase().indexOf(term.toUpperCase()), tl = term.length;

                    if (match < 0) {
                        markup.push(text);
                    } else {
                        markup.push(text.substring(0, match));
                        markup.push("<span class='select2-match'>");
                        markup.push(text.substring(match, match + tl));
                        markup.push("</span>");
                        markup.push(text.substring(match + tl, text.length));
                    }
                    
                    return markup.join("");
                };
                
                // Set preselected item
                options.initSelection = function (el, callback) {
                    var data = undefined;
                    
                    var selectedElement = $('#' + el[0].id.replace('select2_', '') + ' option:selected');

                    if (selectedElement.val() != undefined && selectedElement != '') {
                        data = { "Value": selectedElement.val(), "Text": selectedElement.text() };
                    } else {
                        selectedElement = $('#' + el[0].id.replace('select2_', ''));

                        var val = selectedElement.val();

                        if (val != undefined && val != '') {
                            data = { "Value": val, "Text": val };
                        }
                    }

                    callback(data);
                };

                var select2element = $('#select2_' + element[0].id);
                
                if (select2element == undefined || select2element.length == 0) {
                                            
                    select2element = $('<input type="hidden" id="select2_' + element[0].id + '" name="select2.' + element[0].name + '" style="width:100%" value="' + element.val() + '" />');

                    element.after(select2element);
                    element.addClass('hidden');
                    
                    // Update actual select element on change of select2 element
                    select2element.bind('change.rhea-select2element', function (details) {
                        element.empty();

                        var selected = $(this).select2('data');
                        
                        if (selected == null) {
                            selected = {};
                            selected.Value = '';
                            selected.Text = '';
                        }

                        if (element.is('select')) {

                            var isComplexType = element.data(dataTypes.IsComplexType);
                            
                            if (element.hasClass('rhea-ajax') && isComplexType) {
                                var hiddenValue = $('#' + element[0].id + '_0__Value');
                                
                                if (hiddenValue == undefined || hiddenValue.length == 0) {
                                    if (selected.Value != '') {
                                        hiddenValue = $('<input type="hidden" id="' + element[0].id + '_0__Value" name="' + element[0].name + '[0].Value" />').val(selected.Value);
                                        element.before(hiddenValue);
                                    }
                                } else {
                                    if (selected.Value == '') {
                                        hiddenValue.remove();
                                    } else {
                                        hiddenValue.val(selected.Value);
                                    }
                                }
                                
                                var hiddenText = $('#' + element[0].id + '_0__Text');
                                
                                if (hiddenText == undefined || hiddenText.length == 0) {
                                    if (selected.Value != '') {
                                        hiddenText = $('<input type="hidden" id="' + element[0].id + '_0__Text" name="' + element[0].name + '[0].Text" />').val(selected.Text);
                                        element.before(hiddenText);
                                    }
                                } else {
                                    if (selected.Value == '') {
                                        hiddenText.remove();
                                    } else {
                                        hiddenText.val(selected.Text);
                                    }
                                }
                                
                            }
                            
                            element.append($('<option />').val('').text(''));
                            element.append($('<option id="' + element[0].id + '_0__Value" name="' + element[0].name + '[0].Value" />').val(selected.Value).text(selected.Text));
                        }

                        element.val(selected.Value);
                        element.change();
                    });
                    

                } else {
                    // Already setup, just clear selection
                    if (!element.hasClass('rhea-adw') || valueFromDependent != undefined) {// && element.val().length > 0) {
                        select2element.val('');
                    }
                }
                
                if (emptyMessage != undefined && emptyMessage.length > 0) {
                    element.data(dataTypes.EmptyMessage, emptyMessage);
                }

                select2element.select2(options);

                // Set the original element value to support postbacks
                select2element.bind("change.select2watch", function (event) {
                    element.val(event.val);
                });
                // Set the starting value to be what was sent from the server if it's there
                var existingData = element.data(dataTypes.MultiSelect);
                if (existingData != undefined && existingData.length) {
                    select2element.select2("data", existingData);
                    element.val(select2element.val());
                }

                // Auto create Text/Value fields for server side multi selects that can be used by the custom ModelBinder to recreate the ViewModel on postback
                var containingForm = select2element.closest('form');
                containingForm.bind("submit", function () {
                    if (element.hasClass('rhea-multiple')) {
                        var name = element.attr('name');
                        var choices = element.parent().find('.select2-search-choice>div');
                        var values = element.val().split(',');
                        if (values[0] == "") values = []; // Force an empty array if nothing has been selected.
                        if (choices.length != values.length) alert('the server side multi select value to text mapping has a cardinality mismatch. Please report this.');
                        for (var i = 0; i < values.length; ++i) {
                            $("<input>").attr("type", "hidden").attr("name", name + "[" + i + "].Text").attr("value", $(choices[i]).text()).appendTo(containingForm);
                            $("<input>").attr("type", "hidden").attr("name", name + "[" + i + "].Value").attr("value", values[i]).appendTo(containingForm);
                        }
                    }
                    else if (element.hasClass('rhea-adw')) { // For single ADW select boxes, manually write out the model binding information
                        var name = element.attr('name');
                        var selectedOption = element.parent().find('option#' + name + '_0__Value');
                        if (selectedOption.length == 0) selectedOption = element.parent().find('option[selected="selected"]'); // Unfortunately, Select2 changes it's own data structure depending on whether or not a selected option has gone through a post back, so we need to check two types of element to find the actual selected value.
                        element.closest('form').find('input[name="' + name + '[0].Value"]').attr("value", selectedOption.attr("value"));
                        element.closest('form').find('input[name="' + name + '[0].Text"]').attr("value", selectedOption.text());
                    }
                });
                // Prevent the submission of the containing form when pressing Enter.
                element.parent().find(".select2-input").bind('keypress', function (event) {
                    return event.keyCode != 13;
                });

                if (element.attr('disabled') || element.attr('readonly')) {
                    //select2element.select2('enable' , false);
                    select2element.select2('readonly', true);
                } else {
                    //select2element.select2('enable', true);
                    select2element.select2('readonly', false);
                }
                
                var s2focus = $('#s2id_focus_select2_' + element[0].id);

                if (s2focus.length > 0) {
                    // Setup label "for" to now point to focusable select2element
//                    var label = $('label[for="' + element[0].id + '"]');
//
//                    if (label.length > 0) {
//                        label.attr('for', s2focus[0].id);
//                    }

                    // Setup focus triggers so when select2element is focused the real element is updated for jquery validation
                    s2focus.bind('focusin.rhea', function () {
                        element.trigger("focusin");
                    });

                    s2focus.bind('focusout.rhea', function () {
                        element.trigger("focusout");
                    });
                }
            } else {
                element.css('width', '100%');
                
                // Unhide original element if hidden so select2 element can be rendered correctly
                element.removeClass('hidden');
                
                // Create select2 element based on current element
                element.select2(options);

                element.bind("change.select2watch", function (event) {
                    if (element[0].type == "select-multiple") {
                        for (var i = 0; i < element[0].children.length; i++) {
                            if (element[0].children[i].value == event.val) {
                                element[0].children[i].selected = true;
                            }
                        }
                    }
                });

                            // Prevent the submission of the containing form when pressing Enter.
                element.parent().find('.select2-input').bind('keypress', function (event) {
                    return event.keyCode != 13;
                });

                if (element.attr('disabled') || element.attr('readonly')) {
                    //select2element.select2('enable' , false);
                    element.select2('readonly', true);
                } else {
                    //select2element.select2('enable', true);
                    element.select2('readonly', false);
                }
                
                var s2focus = $('#s2id_focus_' + element[0].id);

                if (s2focus.length > 0) {
                    // Setup label "for" to now point to focusable select2element
//                    var label = $('label[for="' + element[0].id + '"]');
//
//                    if (label.length > 0) {
//                        label.attr('for', s2focus[0].id);
//                    }

                    // Setup focus triggers so when select2element is focused the real element is updated for jquery validation
                    s2focus.bind('focusin.rhea', function () {
                        element.trigger("focusin");
                    });

                    s2focus.bind('focusout.rhea', function () {
                        element.trigger("focusout");
                    });
                }

                // Hide original element as select2 element exists now instead
                // Commented as select2-offscreen style is now applied by select2
                //element.addClass('hidden');
            }
        },

        _autocompleteJobseekerSearch: function (element) {
            if (!element.is('input') || !$(element).hasClass('rhea-ajax-jsk-search')) {
                // this is not job seeker address.
                return;
            }
            var $rhea = this;
            var root = $(this.element) || $(document);
            var emptyMessage = ' ';
            var allowClear = true;

            var options = {
                width: 'copy',
                allowClear: allowClear
            };
            // Prepare custom job seeker
            var prepareCustomJobseeker = function (data) {

            };

            options.placeholder = emptyMessage;
            options.id = 'SingleLineJobseekerSearch';
            options.minimumInputLength = 6;

            options.ajax = {
                quietMillis: 1000,
                type: 'GET', dateType: 'json', url: element.data(dataTypes.Url), traditional: true,
                data: function (term, page) {
                    return { text: term, page: page };
                },
                results: function (data, page) {
                    return { results: data.result, more: data.more };
                }
            };
            options.formatSelection = function (data) {
                return data.SingleLineJobseekerSearch;
            }; 

            options.formatResult = function (data, container, query) {
                var markup = [];
                var text = data.SingleLineJobseekerSearch;
                var term = query.term;
                var match = text.toUpperCase().indexOf(term.toUpperCase()), // starting point of the match
                    termLength = term.length;
                if (match < 0) {
                    markup.push(text);
                } else {

                    markup.push(text.substring(0, match));
                    markup.push("<span class='select2-match'>");
                    markup.push(text.substring(match, match + termLength));
                    markup.push("</span>");
                    markup.push(text.substring(match + termLength, text.length))
                }

                return markup.join("");
            };

            // Set preselected item
            options.initSelection = function (el, callback) {
                var data = undefined;
                selectedElement = $('#' + el[0].id.replace('select2_', ''));
                var val = selectedElement.val();
                if (val != undefined && val != '') {
                    data = { "SingleLineJobseekerSearch": val };
                }

                callback(data);
            };

            if (emptyMessage != undefined && emptyMessage.length > 0) {
                element.data(dataTypes.EmptyMessage, emptyMessage);
            }

            element.css('width', '100%');

            // show the original element
            element.removeClass('hidden');

            // create select2 element based on the current options.
            element.select2(options);

            if (element.attr('disabled') || element.attr('readonly')) {
                element.select2('readonly', true);
            }
            else {
                element.select2('readonly', false);
            }

            var select2element = element;

            // update actual select element upon the change of select2 element.
            select2element.bind('change.rhea-select2element', function () {
                element.empty();

                var data = $(this).select2('data');
                if (data == null) {
                    element.val('');
                    data = { "SingleLineJobseekerSearch": '', 'Id': '', 'Type': '', 'Salutation': '', 'FirstName': '', 'MiddleName': '', 'LastName': '', 'Gender': '', 'DOB': '' };
                }

                // make sure data is correct before preparing
                if (data.SingleLineJobseekerSearch != undefined && data.Id != undefined && data.Type != undefined && data.Salutation != undefined && data.FirstName != undefined && data.MiddleName != undefined && data.LastName != undefined && data.Gender != undefined && data.DOB != undefined) {
                    prepareCustomJobseeker(data);
                }
            });

            var s2focus = $('#s2id_focus_' + element[0].id);
            if (s2focus.length > 0) {
                // setup focus triggers, so when the select2element is focused, then the real element is updated for jquery validation
                s2focus.bind('focusin.rhea', function () {
                    element.trigger("focusin");
                });

                s2focus.bind('focusout.rhea', function () {
                    element.trigger("focusout");
                });
            }

        },

        _autocompleteGeospatialAddress: function (element) {
            
            if (!element.is('input') || !$(element).hasClass('rhea-ajax-geospatial-address')) {
                // Ignore incorrect type
                return;
            }

            var $rhea = this;
            var root = $(this.element) || $(document);

            var emptyMessage = ' ';
            var allowClear = true;
            
                
            var options = {
                width: 'copy',
                allowClear: allowClear
            };


            var prepareCustomAddress = function (data) {
                $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'SingleLineAddress')).val(data.SingleLineAddress);
                
                $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Reliability')).val(data.Reliability != undefined && data.Reliability.length > 0 ? data.Reliability : 'Unknown');
                
                // Get latitude and longitude 
                $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Latitude')).val(data.Latitude != undefined && data.Latitude.length > 0 ? data.Latitude : '');
                $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Longitude')).val(data.Longitude != undefined && data.Longitude.length > 0 ? data.Longitude : '');

                $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Longitude'))
                
                $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Line1')).val(data.Line1);
                $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Line2')).val(data.Line2);
                $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Line3')).val(data.Line3);

                // Rebuild state dropdown
                var state = $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'State'));
                state.val(data.State);
                $('#select2_' + state[0].id).remove();
                $('#s2id_select2_' + state[0].id).remove();
                $rhea._autocomplete(state);

                // Rebuild postcode dropdown
                var postcode = $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Postcode'));

                if (data.State != undefined && data.State.length > 0) {
                    postcode.val(data.Postcode);
                    postcode.removeAttr('disabled');
                    postcode.removeAttr('readonly');
                } else {
                    postcode.val('');
                    postcode.attr('readonly', 'readonly');
                }

                $('#select2_' + postcode[0].id).remove();
                $('#s2id_select2_' + postcode[0].id).remove();
                $rhea._autocomplete(postcode);

                // Rebuild locality dropdown
                var locality = $('#' + $.zeusValidate.getFieldPrefixFromId(element[0], 'Locality'));

                if (data.Postcode != undefined && data.Postcode.length > 0) {
                    locality.val(data.Locality);
                    locality.removeAttr('disabled');
                    locality.removeAttr('readonly');
                } else {
                    locality.val('');
                    locality.attr('readonly', 'readonly');
                }

                $('#select2_' + locality[0].id).remove();
                $('#s2id_select2_' + locality[0].id).remove();
                $rhea._autocomplete(locality);
            };
            
            options.placeholder = emptyMessage;
            options.id = 'SingleLineAddress';
            options.minimumInputLength = 5;
            
                    options.ajax = {
                        quietMillis: 1000,
                        type: 'GET',
                        dataType: 'json',
                        url: element.data(dataTypes.Url),
                        traditional: true,
                data: function (term, page) {
                            return {
                                text: term,
                                page: page,
                                returnLatLong: (/^true$/i.test(element.data(dataTypes.LatitudeLongitude))) ? true : false
                            };
                        },
                results: function (data, page) {
                            return { results: data.result, more: data.more };
                        }
                    };
                    
                    options.formatSelection = function (data) {
                        return data.SingleLineAddress;
                    };

                options.formatResult = function (data, container, query) {
                    var markup = [];

                    var text = data.SingleLineAddress;
                    var term = query.term;
                    
                    var match = text.toUpperCase().indexOf(term.toUpperCase()), tl = term.length;

                    if (match < 0) {
                        markup.push(text);
                    } else {
                        markup.push(text.substring(0, match));
                        markup.push("<span class='select2-match'>");
                        markup.push(text.substring(match, match + tl));
                        markup.push("</span>");
                        markup.push(text.substring(match + tl, text.length));
                    }
                    
                    return markup.join("");
                };
                
                // Set preselected item
                options.initSelection = function (el, callback) {
                    var data = undefined;
                    
                    selectedElement = $('#' + el[0].id.replace('select2_', ''));

                    var val = selectedElement.val();

                    if (val != undefined && val != '') {
                        data = { "SingleLineAddress": val };
                    }

                    callback(data);
                };
                
                if (emptyMessage != undefined && emptyMessage.length > 0) {
                    element.data(dataTypes.EmptyMessage, emptyMessage);
                }
            
            
                element.css('width', '100%');
                
                // Unhide original element if hidden so select2 element can be rendered correctly
                element.removeClass('hidden');
                
                // Create select2 element based on current element
                element.select2(options);

                if (element.attr('disabled') || element.attr('readonly')) {
                    //select2element.select2('enable' , false);
                    element.select2('readonly', true);
                } else {
                    //select2element.select2('enable', true);
                    element.select2('readonly', false);
                }
            
            var select2element = element;//$('#select2_' + element[0].id);
                
                    // Update actual select element on change of select2 element
            select2element.bind('change.rhea-select2element', function () {
                        element.empty();

                        var data = $(this).select2('data');
                        
                        if (data == null) {
                            element.val('');
                    data = { "SingleLineAddress": '', 'Line1': '', 'Line2': '', 'Line3': '', 'State': '', 'Postcode': '', 'Locality': '', 'Reliability': '', 'Latitude': '', 'Longitude': '' };
                        }
                        
                        // Ensure data structure is correct before preparing
                        if (data.SingleLineAddress != undefined && data.Line1 != undefined && data.Line2 != undefined && data.Line3 != undefined && data.State != undefined && data.Postcode != undefined && data.Locality != undefined && data.Reliability != undefined) {
                            prepareCustomAddress(data);
                        }
                    });
                
                var s2focus = $('#s2id_focus_' + element[0].id);

                if (s2focus.length > 0) {
                    // Setup label "for" to now point to focusable select2element
//                    var label = $('label[for="' + element[0].id + '"]');
//
//                    if (label.length > 0) {
//                        label.attr('for', s2focus[0].id);
//                    }

                    // Setup focus triggers so when select2element is focused the real element is updated for jquery validation
                s2focus.bind('focusin.rhea', function () {
                        element.trigger("focusin");
                    });

                s2focus.bind('focusout.rhea', function () {
                        element.trigger("focusout");
                    });
                }

                // Hide original element as select2 element exists now instead
                //element.addClass('hidden');
        },

        
        autocomplete: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            // Find all selects
            root.find('select').each(function () {
                if ($(this).hasClass('rhea-ajax') || $(this).hasClass('rhea-adw')) {
                    return;
                }
                //Render your JQuery autocomplete code here. Autocomplete_defect_fix
                
                $rhea._autocomplete($(this));
            });
            
            root.find('.rhea-adw').each(function () {
                $rhea._autocomplete($(this));
                //$(this).combobox();
                
            });
             

            root.find('.rhea-ajax').each(function () {
                $rhea._autocomplete($(this));
            });

            root.find('.rhea-ajax-geospatial-address').each(function () {
                $rhea._autocompleteGeospatialAddress($(this));
            });

            root.find('.rhea-ajax-jsk-search').each(function () {
                $rhea._autocompleteJobseekerSearch($(this));
            });
        },

        sidenavigation: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);
        
            // Replacement for voodoo interface side navigation (commented out in interface.js)
            root.find('.sideNav li a').bind('click.rhea', function () {
                var a = $(this)[0];
                var ul = $(this).next();

                if (ul.hasClass('hidden')) {
                    ul.removeClass('hidden');
                    $(this).next().slideDown(200, function () {
//                        a.innerHTML = a.innerHTML.replace('?', '?');
                        //a.children[0].src = a.children[0].src.replace('close', 'open');
                        
                        a.lastChild.innerHTML = '(Closes a list below)'; 
                        if (a.hasAttribute('class'))
                            a.setAttribute('class', 'open');
                    });
                } else {
                    $(this).next().slideUp(200, function () {
                        $(this).addClass('hidden');
//                        a.innerHTML = a.innerHTML.replace('?', '?');
                        //a.children[0].src = a.children[0].src.replace('open', 'close');
                        a.lastChild.innerHTML = '(Opens a list below)';
                        if (a.hasAttribute('class'))
                            a.setAttribute('class', 'close');
                        if (ul.hasClass('block')) {
                            ul.removeClass('block');
                        }
                    });
                }
            });  
        },

        actionif: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-actionif').each(function () {
                var property = $(this);
                var fieldprefix = '';
                
                if (property.data(dataTypes.FieldPrefix).length > 0) {
                    fieldprefix = property.data(dataTypes.FieldPrefix) + '_';
                }

                var comparisonType = $(this).data(dataTypes.ComparisonType);
                var valueToTestAgainst = $(this).data(dataTypes.DependentValue);
                var actionForDependencyType = $(this).data(dataTypes.ActionForDependencyType);
                var propertyType = $(this).data(dataTypes.Type);

                var prefixedDependentPropertyId = fieldprefix + property.data(dataTypes.DependentProperty);
                var dependentProperty = $('#' + prefixedDependentPropertyId);
                
                // If dependent property not found by ID, assume it is a radio button and get by name instead (this will return multiple elements)
                if (dependentProperty.length == 0 || dependentProperty.data(dataTypes.RadioButtonGroup)) {
                    dependentProperty = $('input:radio[name="' + $.zeusValidate.replaceAll(prefixedDependentPropertyId, '_', '.') + '"]');
                }

                // Don't bind if self-referencing
                if (property[0] == dependentProperty[0]) {
                    return;
                }

                var actionIfChange = function () {
                    var dependentPropertyValue = null;

                    if ($(this)[0].type == 'checkbox' || $(this)[0].type == 'radio' || $(this).length > 1) {
                        for (var index = 0; index != $(this).length; index++) {
                            if ($(this)[index]['checked']) {
                                dependentPropertyValue = $(this)[index].value;
                                break;
                            }
                        }

                        if (dependentPropertyValue == null) {
                            dependentPropertyValue = false;
                        }
                    } 
                    
                    else if ($(this)[0].type == "select-multiple" && $(this)[0].length != undefined && $(this)[0].length > 0
                        && $(this)[0].value !== undefined && $(this)[0].value != "") {
                        dependentPropertyValue = [];

                        for (var j = 0; j < $(this)[0].children.length; j++) {
                            if ($(this)[0].children[j].selected) {
                                dependentPropertyValue.push($(this)[0].children[j].value);
                            }
                        }
                        
                    }
                    else {
                        dependentPropertyValue = $(this)[0].value;
                    }

                    if ($.zeusValidate.is(dependentPropertyValue, comparisonType, valueToTestAgainst, false, false)) {
                        if (actionForDependencyType.toLowerCase() == 'visible') {
                            property.removeClass('hidden');
                        } else if (actionForDependencyType.toLowerCase() == 'hidden') {
                            property.addClass('hidden');
                        } else if (actionForDependencyType.toLowerCase() == 'enabled') {
                            if (propertyType == 'link') {
                                property.find('span').addClass('hidden');
                                property.find('a').removeClass('hidden');
                            } else {
                                property.removeAttr('disabled');
                            }
                        } else if (actionForDependencyType.toLowerCase() == 'disabled') {
                            if (propertyType == 'link') {
                                property.find('a').addClass('hidden');
                                property.find('span').removeClass('hidden');
                            } else {
                                property.attr('disabled', 'disabled');
                            }
                        }
                    } else {
                        if (actionForDependencyType.toLowerCase() == 'visible') {
                            property.addClass('hidden');
                        } else if (actionForDependencyType.toLowerCase() == 'hidden') {
                            property.removeClass('hidden');
                        } else if (actionForDependencyType.toLowerCase() == 'enabled') {
                            if (propertyType == 'link') {
                                property.find('a').addClass('hidden');
                                property.find('span').removeClass('hidden');
                            } else {
                                property.attr('disabled', 'disabled');
                            }
                        } else if (actionForDependencyType.toLowerCase() == 'disabled') {
                            if (propertyType == 'link') {
                                property.find('span').addClass('hidden');
                                property.find('a').removeClass('hidden');
                            } else {
                                property.removeAttr('disabled');
                            }
                        }
                    }
                };
                
                if (dependentProperty.length > 1) {
                    // Bind to each radio button element
                    for (var i = 0; i < dependentProperty.length; i++) {
                        $(dependentProperty[i]).bind('change.rhea-actionif', actionIfChange);
                    }
                } else {
                    dependentProperty.bind('change.rhea-actionif', actionIfChange);
                }
            });
        },

        visibleif: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-visibleif').each(function () {
                var property = $(this);

                var readOnlyType = $(this).data(dataTypes.ReadOnlyType);
                var dependentPropertyId = $(this).data(dataTypes.DependentPropertyVisibleIf);
                var comparisonType = $(this).data(dataTypes.ComparisonTypeVisibleIf);
                var passOnNull = $(this).data(dataTypes.PassOnNullVisibleIf);
                var failOnNull = $(this).data(dataTypes.FailOnNullVisibleIf);
                var valueToTestAgainst = $(this).data(dataTypes.DependentValueVisibleIf);

                
                // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
                if (property[0].id.length == 0 && property.find('input').length > 0) {
                    // Get nested input property
                    property = property.find('input');
                }

                var containerFor = 'ContainerFor-';
                var prefixedDependentPropertyId = $.zeusValidate.getFieldPrefixFromId(property[0], dependentPropertyId);
                
                if (prefixedDependentPropertyId.indexOf(containerFor) != -1) {
                    prefixedDependentPropertyId = prefixedDependentPropertyId.substring(containerFor.length, prefixedDependentPropertyId.length);
                }
                
                var dependentProperty = $('#' + prefixedDependentPropertyId);
                
                // If dependent property not found by ID, assume it is a radio button and get by name instead (this will return multiple elements)
                if (dependentProperty.length == 0 || dependentProperty.data(dataTypes.RadioButtonGroup)) {
                    dependentProperty = $('input:radio[name="' + $.zeusValidate.replaceAll(prefixedDependentPropertyId, '_', '.') + '"]');
                }

                // Don't bind if self-referencing
                if (property[0] == dependentProperty[0]) {
                    return;
                }
                
                var visibleIfChange = function () {
                    var dependentPropertyValue = null;

                    if ($(this)[0].type == 'checkbox' || $(this)[0].type == 'radio' || $(this).length > 1) {
                        for (var index = 0; index != $(this).length; index++) {
                            if ($(this)[index]['checked']) {
                                dependentPropertyValue = $(this)[index].value;
                                break;
                            }
                        }

                        if (dependentPropertyValue == null) {
                            dependentPropertyValue = false;
                        }
                    } 
                    
                    else if ($(this)[0].type == "select-multiple" && $(this)[0].length != undefined && $(this)[0].length > 0 &&
                        $(this)[0].value !== undefined && $(this)[0].value != "") {

                        dependentPropertyValue = [];
                        for (var j = 0; j < $(this)[0].children.length; j++) {
                            if ($(this)[0].children[j].selected) {
                                dependentPropertyValue.push($(this)[0].children[j].value);
                            }
                        }
                    }
                    
                    
                    else {
                        dependentPropertyValue = $(this)[0].value;
                    }

                    var propertyId = property[0].id;

                    if (propertyId.indexOf('InnerContainerFor-') != -1) {
                        propertyId = propertyId.replace('InnerContainerFor-', '');
                    }

                    if (propertyId.indexOf('ContainerFor-') == -1 && propertyId.lastIndexOf('-') != -1) {
                        propertyId = propertyId.substring(0, propertyId.lastIndexOf('-'));
                    }

                    var containerId = propertyId;
                    containerId = (containerId.indexOf(containerFor) == -1) ? containerFor + containerId : containerId;

                    if ($.zeusValidate.is(dependentPropertyValue, comparisonType, valueToTestAgainst, passOnNull, failOnNull)) {
                        $('#' + containerId).removeClass('hidden');
                    } else {
                        $('#' + containerId).addClass('hidden');
                    }
                };

                if (dependentProperty.length > 1) {
                    // Bind to each radio button element
                    for (var i = 0; i < dependentProperty.length; i++) {
                        $(dependentProperty[i]).bind('change.rhea-visibleif', visibleIfChange);
                    }
                } else {
                    dependentProperty.bind('change.rhea-visibleif', visibleIfChange);
                }
            });
        },

        readonlyif: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-readonlyif').each(function () {
                var property = $(this);
                
                var readOnlyType = $(this).data(dataTypes.ReadOnlyType);
                var dependentPropertyId = $(this).data(dataTypes.DependentPropertyReadOnlyIf);
                var comparisonType = $(this).data(dataTypes.ComparisonTypeReadOnlyIf);
                var passOnNull = $(this).data(dataTypes.PassOnNullReadOnlyIf);
                var failOnNull = $(this).data(dataTypes.FailOnNulleadOnlyIf);
                var valueToTestAgainst = $(this).data(dataTypes.DependentValueReadOnlyIf);

                // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
                if (property[0].id.length == 0 && property.find('input').length > 0) {
                    // Get nested input property
                    property = property.find('input');
                }

                var prefixedDependentPropertyId = $.zeusValidate.getFieldPrefixFromId(property[0], dependentPropertyId);
                var dependentProperty = $('#' + prefixedDependentPropertyId);
                
                // If dependent property not found by ID, assume it is a radio button and get by name instead (this will return multiple elements)
                if (dependentProperty.length == 0 || dependentProperty.data(dataTypes.RadioButtonGroup)) {
                    dependentProperty = $('input:radio[name="' + $.zeusValidate.replaceAll(prefixedDependentPropertyId, '_', '.') + '"]');
                }

                // Don't bind if self-referencing
                if (property[0] == dependentProperty[0]) {
                    return;
                }

                var readOnlyIfChange = function () {
                    var dependentPropertyValue = null;

                    if ($(this)[0].type == 'checkbox' || $(this)[0].type == 'radio' || $(this).length > 1) {
                        for (var index = 0; index != $(this).length; index++) {
                            if ($(this)[index]['checked']) {
                                dependentPropertyValue = $(this)[index].value;
                                break;
                            }
                        }

                        if (dependentPropertyValue == null) {
                            dependentPropertyValue = false;
                        }
                    } 
                    
                    else if ($(this)[0].type == "select-multiple" && $(this)[0].length != undefined && $(this)[0].length > 0 &&
                        $(this)[0].value !== undefined && $(this)[0].value != "") {

                        dependentPropertyValue = [];
                        for (var j = 0; j < $(this)[0].children.length; j++) {
                            if ($(this)[0].children[j].selected) {
                                dependentPropertyValue.push($(this)[0].children[j].value);
                            }
                        }
                        
                    }

                    else {
                        dependentPropertyValue = $(this)[0].value;
                    }

                    if ($.zeusValidate.is(dependentPropertyValue, comparisonType, valueToTestAgainst, passOnNull, failOnNull)) {
                        if (property.is('select') || property.hasClass('rhea-adw') || property.hasClass('rhea-ajax') || property.hasClass('rhea-ajax-geospatial-address') || property.hasClass('rhea-ajax-jsk-search')) {
                            property.attr('disabled', 'disabled');
                            $rhea._autocomplete(property);
                            property.trigger('rhea-dualselect');
                        } else if (property.hasClass('t-input')) {
                            if (property.data('tTextBox') != undefined) {
                                property.data('tTextBox').disable();
                            } else if (property.data('tDateTimePicker') != undefined) {
                                property.data('tDateTimePicker').disable();
                            } else if (property.data('tDatePicker') != undefined) {
                                property.data('tDatePicker').disable();
                            } else if (property.data('tTimePicker') != undefined) {
                                property.data('tTimePicker').disable();
                            } else {
                                property.attr(readOnlyType, readOnlyType);
                            }
                        } else {
                            property.attr(readOnlyType, readOnlyType);
                        }

                        // Update enable state for check all checkbox on a check box list
                        if (property[0].type == 'checkbox' && property.data('checkallid') != undefined) {
                            var checkall = $('#' + property.data('checkallid'));

                            if (checkall) {
                                checkall.attr(readOnlyType, readOnlyType);
                            }
                        }
                    } else {
                        if (property.is('select') || property.hasClass('rhea-adw') || property.hasClass('rhea-ajax') || property.hasClass('rhea-ajax-geospatial-address') || property.hasClass('rhea-ajax-jsk-search')) {
                            property.removeAttr('readonly');
                            property.removeAttr('disabled');
                            $rhea._autocomplete(property);
                            property.trigger('rhea-dualselect');
                        } else if (property.hasClass('t-input')) {
                            if (property.data('tTextBox') != undefined) {
                                property.data('tTextBox').enable();
                            } else if (property.data('tDateTimePicker') != undefined) {
                                property.data('tDateTimePicker').enable();
                            } else if (property.data('tDatePicker') != undefined) {
                                property.data('tDatePicker').enable();
                            } else if (property.data('tTimePicker') != undefined) {
                                property.data('tTimePicker').enable();
                            } else {
                                property.removeAttr('readonly');
                                property.removeAttr('disabled');
                            }
                        } else {
                            property.removeAttr('readonly');
                            property.removeAttr('disabled');
                        }

                        // Update enable state for check all checkbox on a check box list
                        if (property[0].type == 'checkbox' && property.data('checkallid') != undefined) {
                            var checkall = $('#' + property.data('checkallid'));

                            if (checkall) {
                                checkall.removeAttr('readonly');
                                checkall.removeAttr('disabled');
                            }
                        }
                    }
                        
                    // If current property has been made readonly but has focus, refocus so browser properly obeys the readonly state
                    if ($(':focus')[0] == property[0]) {
                        property.blur();
                        property.focus();
                    }
                };

                if (dependentProperty.length > 1) {
                    // Bind to each radio button element
                    for (var i = 0; i < dependentProperty.length; i++) {
                        $(dependentProperty[i]).bind('change.rhea-readonlyif', readOnlyIfChange);
                    }
                } else {
                    dependentProperty.bind('change.rhea-readonlyif', readOnlyIfChange);
                }
            });
        },

        editableif: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-editableif').each(function () {
                var property = $(this);
                
                var readOnlyType = $(this).data(dataTypes.ReadOnlyType);
                var dependentPropertyId = $(this).data(dataTypes.DependentPropertyEditableIf);
                var comparisonType = $(this).data(dataTypes.ComparisonTypeEditableIf);
                var passOnNull = $(this).data(dataTypes.PassOnNullEditableIf);
                var failOnNull = $(this).data(dataTypes.FailOnNullEditableIf);
                var valueToTestAgainst = $(this).data(dataTypes.DependentValueEditableIf);

                // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
                if (property[0].id.length == 0 && property.find('input').length > 0) {
                    // Get nested input property
                    property = property.find('input');
                }

                var prefixedDependentPropertyId = $.zeusValidate.getFieldPrefixFromId(property[0], dependentPropertyId);
                var dependentProperty = $('#' + prefixedDependentPropertyId);
                
                // If dependent property not found by ID, assume it is a radio button and get by name instead (this will return multiple elements)
                if (dependentProperty.length == 0 || dependentProperty.data(dataTypes.RadioButtonGroup)) {
                    dependentProperty = $('input:radio[name="' + $.zeusValidate.replaceAll(prefixedDependentPropertyId, '_', '.') + '"]');
                }
                
                // Don't bind if self-referencing
                if (property[0] == dependentProperty[0]) {
                    return;
                }

                var editableIfChange = function () {
                    var dependentPropertyValue = null;

                    if ($(this)[0].type == 'checkbox' || $(this)[0].type == 'radio' || $(this).length > 1) {
                        for (var index = 0; index != $(this).length; index++) {
                            if ($(this)[index]['checked']) {
                                dependentPropertyValue = $(this)[index].value;
                                break;
                            }
                        }

                        if (dependentPropertyValue == null) {
                            dependentPropertyValue = false;
                        }
                    } 
                    
                    else if ($(this)[0].type == "select-multiple" && $(this)[0].length != undefined && $(this)[0].length > 0 &&
                        $(this)[0].value !== undefined && $(this)[0].value != "") {

                        dependentPropertyValue = [];
                        for (var j = 0; j < $(this)[0].children.length; j++) {
                            if ($(this)[0].children[j].selected) {
                                dependentPropertyValue.push($(this)[0].children[j].value);
                            }
                        }
                        
                    }
                    
                    else {
                        dependentPropertyValue = $(this)[0].value;
                    }

                    if ($.zeusValidate.is(dependentPropertyValue, comparisonType, valueToTestAgainst, passOnNull, failOnNull)) {
                        if (property.is('select') || property.hasClass('rhea-adw') || property.hasClass('rhea-ajax') || property.hasClass('rhea-ajax-geospatial-address') || property.hasClass('rhea-ajax-jsk-search')) {
                            property.removeAttr('readonly');
                            property.removeAttr('disabled');
                            $rhea._autocomplete(property);
                            property.trigger('rhea-dualselect');
                        } else if (property.hasClass('t-input')) {
                            if (property.data('tTextBox') != undefined) {
                                property.data('tTextBox').enable();
                            } else if (property.data('tDateTimePicker') != undefined) {
                                property.data('tDateTimePicker').enable();
                            } else if (property.data('tDatePicker') != undefined) {
                                property.data('tDatePicker').enable();
                            } else if (property.data('tTimePicker') != undefined) {
                                property.data('tTimePicker').enable();
                            } else {
                                property.removeAttr('readonly');
                                property.removeAttr('disabled');
                            }
                        } else {
                            property.removeAttr('readonly');
                            property.removeAttr('disabled');
                        }


                        // Update enable state for check all checkbox on a check box list
                        if (property[0].type == 'checkbox' && property.data('checkallid') != undefined) {
                            var checkall = $('#' + property.data('checkallid'));

                            if (checkall) {
                                checkall.removeAttr('readonly');
                                checkall.removeAttr('disabled');
                            }
                        }
                    } else {
                        if (property.is('select') || property.hasClass('rhea-adw') || property.hasClass('rhea-ajax') || property.hasClass('rhea-ajax-geospatial-address') || property.hasClass('rhea-ajax-jsk-search')) {
                            property.attr('disabled', 'disabled');
                            $rhea._autocomplete(property);
                            property.trigger('rhea-dualselect');
                        } else if (property.hasClass('t-input')) {
                            if (property.data('tTextBox') != undefined) {
                                property.data('tTextBox').disable();
                            } else if (property.data('tDateTimePicker') != undefined) {
                                property.data('tDateTimePicker').disable();
                            } else if (property.data('tDatePicker') != undefined) {
                                property.data('tDatePicker').disable();
                            } else if (property.data('tTimePicker') != undefined) {
                                property.data('tTimePicker').disable();
                            } else {
                                property.attr(readOnlyType, readOnlyType);
                            }
                        } else {
                            property.attr(readOnlyType, readOnlyType);
                        }

                        // Update enable state for check all checkbox on a check box list
                        if (property[0].type == 'checkbox' && property.data('checkallid') != undefined) {
                            var checkall = $('#' + property.data('checkallid'));

                            if (checkall) {
                                checkall.attr(readOnlyType, readOnlyType);
                            }
                        }
                    }
                    
                    // If current property has been made readonly but has focus, refocus so browser properly obeys the readonly state
                    if ($(':focus')[0] == property[0]) {
                        property.blur();
                        property.focus();
                    }
                };
                
                if (dependentProperty.length > 1) {
                    // Bind to each radio button element
                    for (var i = 0; i < dependentProperty.length; i++) {
                        $(dependentProperty[i]).bind('change.rhea-editableif', editableIfChange);
                    }
                } else {
                    dependentProperty.bind('change.rhea-editableif', editableIfChange);
                }
            });
        },
        
        clearif: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-clearif').each(function () {
                var property = $(this);
                
                var readOnlyType = $(this).data(dataTypes.ReadOnlyType);
                var dependentPropertyId = $(this).data(dataTypes.DependentPropertyClearIf);
                var comparisonType = $(this).data(dataTypes.ComparisonTypeClearIf);
                var passOnNull = $(this).data(dataTypes.PassOnNullClearIf);
                var failOnNull = $(this).data(dataTypes.FailOnNullClearIf);
                var valueToTestAgainst = $(this).data(dataTypes.DependentValueClearIf);
                var always = (/^true$/i.test($(this).data(dataTypes.AlwaysClearIf))) ? true : false;

                // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
                if (property[0].id.length == 0 && property.find('input').length > 0) {
                    // Get nested input property
                    property = property.find('input');
                }

                var prefixedDependentPropertyId = $.zeusValidate.getFieldPrefixFromId(property[0], dependentPropertyId);
                var dependentProperty = $('#' + prefixedDependentPropertyId);
                
                // If dependent property not found by ID, assume it is a radio button and get by name instead (this will return multiple elements)
                if (dependentProperty.length == 0 || dependentProperty.data(dataTypes.RadioButtonGroup)) {
                    dependentProperty = $('input:radio[name="' + $.zeusValidate.replaceAll(prefixedDependentPropertyId, '_', '.') + '"]');
                }
                
                // Don't bind if self-referencing
                if (property[0] == dependentProperty[0]) {
                    return;
                }

                var clearIfChange = function () {
                    var dependentPropertyValue = null;

                    if ($(this)[0].type == 'checkbox' || $(this)[0].type == 'radio' || $(this).length > 1) {
                        for (var index = 0; index != $(this).length; index++) {
                            if ($(this)[index]['checked']) {
                                dependentPropertyValue = $(this)[index].value;
                                break;
                            }
                        }

                        if (dependentPropertyValue == null) {
                            dependentPropertyValue = false;
                        }
                    } 
                    
                    else if ($(this)[0].type == "select-multiple" && $(this)[0].length != undefined && $(this)[0].length > 0 &&
                        $(this)[0].value !== undefined && $(this)[0].value != "") {

                        dependentPropertyValue = [];
                        for (var j = 0; j < $(this)[0].children.length; j++) {
                            if ($(this)[0].children[j].selected) {
                                dependentPropertyValue.push($(this)[0].children[j].value);
                            }
                        }
                        
                    }

                    else {
                        dependentPropertyValue = $(this)[0].value;
                    }

                    if (always || $.zeusValidate.is(dependentPropertyValue, comparisonType, valueToTestAgainst, passOnNull, failOnNull)) {

                        var val = '';
                        
                        if (property.hasClass('rhea-numeric') && !/^true$/i.test(property.data(dataTypes.IsNullable))) {
                            val = $.autoNumeric.Format(property, 0);
                        }
                        
                        if (property[0].type == 'checkbox' || property[0].type == 'radio') {
                            property[0].checked = false;
                        } else {
                            property.val(val);
                        }

                        // Clear chaining not support for 'always' clear ([Clear] attribute) to prevent infinite clear loop
                        if (always) {
                            property.trigger('change.rhea-gst');
                            property.trigger('change.rhea-copy');
                        } else {
                            property.change();
                        }
                        
                        if (property.is('select') || property.hasClass('rhea-ajax') || property.hasClass('rhea-adw')) {
                            $rhea._autocomplete(property);
                        }
                    }
                };
                
                if (dependentProperty.length > 1) {
                    // Bind to each radio button element
                    for (var i = 0; i < dependentProperty.length; i++) {
                        $(dependentProperty[i]).bind('change.rhea-clearif', clearIfChange);
                    }
                } else {
                    dependentProperty.bind('change.rhea-clearif', clearIfChange);
                }
            });
        },
        
        gst: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-gst').each(function () {
                var property = $(this);
                
                var readOnlyType = $(this).data(dataTypes.ReadOnlyType);
                var dependentPropertyId = $(this).data(dataTypes.DependentPropertyGst);
                var isExclusive = $(this).data(dataTypes.ExclusiveGst);

                // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
                if (property[0].id.length == 0 && property.find('input').length > 0) {
                    // Get nested input property
                    property = property.find('input');
                }

                var prefixedDependentPropertyId = $.zeusValidate.getFieldPrefixFromId(property[0], dependentPropertyId);
                var dependentProperty = $('#' + prefixedDependentPropertyId);
                
                // If dependent property not found by ID, assume it is a radio button and get by name instead (this will return multiple elements)
                if (dependentProperty.length == 0 || dependentProperty.data(dataTypes.RadioButtonGroup)) {
                    dependentProperty = $('input:radio[name="' + $.zeusValidate.replaceAll(prefixedDependentPropertyId, '_', '.') + '"]');
                }
                
                // Don't bind if self-referencing
                if (property[0] == dependentProperty[0]) {
                    return;
                }

                var gstChange = function () {
                    
                    if (dependentPropertyId == null || dependentPropertyId == undefined) {
                        return;
                    }

                    var dependentPropertyValue = null;

                    if ($(this)[0].type == 'checkbox' || $(this)[0].type == 'radio' || $(this).length > 1) {
                        for (var index = 0; index != $(this).length; index++) {
                            if ($(this)[index]['checked']) {
                                dependentPropertyValue = $(this)[index].value;
                                break;
                            }
                        }

                        if (dependentPropertyValue == null) {
                            dependentPropertyValue = false;
                        }
                    } else {
                        dependentPropertyValue = $(this)[0].value;
                    }

                    dependentPropertyValue = $.zeusValidate.replaceAll(dependentPropertyValue.replace('$', ''), ',', '');

                    if (!isNaN(dependentPropertyValue)) {
                        var amount = new Number(dependentPropertyValue);
                        
                        var gst = isExclusive ? new Number(amount * 0.1) : new Number(amount / 11);

                        if (property.hasClass('rhea-numeric')) {
                            gst = $.autoNumeric.Format(property, gst);
                        }
                        
                        property.val(gst);
                        property.change();
                    }
                };
                
                if (dependentProperty.length > 1) {
                    // Bind to each radio button element
                    for (var i = 0; i < dependentProperty.length; i++) {
                        $(dependentProperty[i]).bind('change.rhea-gst', gstChange);
                    }
                } else {
                    dependentProperty.bind('change.rhea-gst', gstChange);
                }
            });
        },
        
        age: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-age').each(function () {
                var property = $(this);
                
                var dependentPropertyId = $(this).data(dataTypes.DependentPropertyAge);

                // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
                if (property[0].id.length == 0 && property.find('input').length > 0) {
                    // Get nested input property
                    property = property.find('input');
                }

                var prefixedDependentPropertyId = $.zeusValidate.getFieldPrefixFromId(property[0], dependentPropertyId);
                var dependentProperty = $('#' + prefixedDependentPropertyId);
                
                // If dependent property not found by ID, assume it is a radio button and get by name instead (this will return multiple elements)
                if (dependentProperty.length == 0 || dependentProperty.data(dataTypes.RadioButtonGroup)) {
                    dependentProperty = $('input:radio[name="' + $.zeusValidate.replaceAll(prefixedDependentPropertyId, '_', '.') + '"]');
                }
                
                // Don't bind if self-referencing
                if (property[0] == dependentProperty[0]) {
                    return;
                }

                var gstChange = function () {
                    
                    if (dependentPropertyId == null || dependentPropertyId == undefined) {
                        return;
                    }

                    var dependentPropertyValue = null;

                    if ($(this)[0].type == 'checkbox' || $(this)[0].type == 'radio' || $(this).length > 1) {
                        for (var index = 0; index != $(this).length; index++) {
                            if ($(this)[index]['checked']) {
                                dependentPropertyValue = $(this)[index].value;
                                break;
                            }
                        }

                        if (dependentPropertyValue == null) {
                            dependentPropertyValue = false;
                        }
                    } else {
                        dependentPropertyValue = $(this)[0].value;
                    }

                    dependentPropertyValue = $.zeusValidate.replaceAll(dependentPropertyValue.replace('$', ''), ',', '');

                    var systemDate = Date.parseExact($('#system_date').text(), 'd/M/yyyy');
                    var dependentDate = Date.parseExact(dependentPropertyValue, 'd/M/yyyy');
                    
                    if (systemDate != null && dependentDate != null) {
                        var age = systemDate.getFullYear() - dependentDate.getFullYear() - ((systemDate.getMonth() < dependentDate.getMonth() || (systemDate.getMonth() == dependentDate.getMonth() && systemDate.getDate() < dependentDate.getDate())) ? 1 : 0);
                        
                        property.val(age);
                        property.change();
                    }
                };
                
                if (dependentProperty.length > 1) {
                    // Bind to each radio button element
                    for (var i = 0; i < dependentProperty.length; i++) {
                        $(dependentProperty[i]).bind('change.rhea-gst', gstChange);
                    }
                } else {
                    dependentProperty.bind('change.rhea-gst', gstChange);
                }
            });
        },
        
        copy: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-copy').each(function () {
                var property = $(this);
                
                var readOnlyType = $(this).data(dataTypes.ReadOnlyType);
                var dependentPropertyId = $(this).data(dataTypes.DependentPropertiesCopy);

                // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
                if (property[0].id.length == 0 && property.find('input').length > 0) {
                    // Get nested input property
                    property = property.find('input');
                }

                var dependentProperties = [];
                
                if ($.isArray(dependentPropertyId)) {
                    dependentProperties = dependentPropertyId;
                } else if (dependentPropertyId != null && dependentPropertyId != undefined) {
                    dependentProperties.push(dependentPropertyId);
                }

                for (var i = 0; i < dependentProperties.length; i++) {
                    var dependentProperty = $('#' + $.zeusValidate.getFieldPrefixFromId(property[0], dependentProperties[i]));

                    // Don't bind if self-referencing
                    if (property[0] == dependentProperty[0]) {
                        return;
                    }

                    dependentProperty.bind('change.rhea-copy', function () {

                        var doCurrent = false;
                        
                        if (dependentProperties.length > 1) {
                            var overallValue = new Number(0);
                            
                            // Loop through each dependent and add their values if it all dependent properties are numeric
                            // Otherwise revert to copying the value of the current changed dependent property
                            for (var j = 0; j < dependentProperties.length; j++) {
                                var dP = $('#' + $.zeusValidate.getFieldPrefixFromId(property[0], dependentProperties[j]));

                                var dPValue = null;

                                if (dP[0].type == 'checkbox' || dP[0].type == 'radio' || dP.length > 1) {
                                    for (var k = 0; k != dP.length; k++) {
                                        if (dP[k]['checked']) {
                                            dPValue = dP[k].value;
                                            break;
                                        }
                                    }

                                    if (dPValue == null) {
                                        dPValue = false;
                                    }
                                } else {
                                    dPValue = dP[0].value;
                                }

                                dPValue = $.zeusValidate.replaceAll(dPValue.replace('$', ''), ',', '');
                                
                                if (!$.zeusValidate.isNumeric(dPValue)) {
                                    doCurrent = true;
                                } else {
                                    overallValue = overallValue + new Number(dPValue);
                                }
                            }
                            
                            if (!doCurrent) {
                                if (property.hasClass('rhea-numeric')) {
                                    overallValue = $.autoNumeric.Format(property, overallValue);
                                }
                                property.val(overallValue);
                                property.change();
                            }
                        } else {
                            doCurrent = true;
                        }
                        
                        if (doCurrent) {
                            var dependentPropertyValue = null;

                            if ($(this)[0].type == 'checkbox' || $(this)[0].type == 'radio' || $(this).length > 1) {
                                for (var index = 0; index != $(this).length; index++) {
                                    if ($(this)[index]['checked']) {
                                        dependentPropertyValue = $(this)[index].value;
                                        break;
                                    }
                                }

                                if (dependentPropertyValue == null) {
                                    dependentPropertyValue = false;
                                }
                            } else {
                                dependentPropertyValue = $(this)[0].value;
                            }

                            dependentPropertyValue = $.zeusValidate.replaceAll(dependentPropertyValue.replace('$', ''), ',', '');
                            
                            if (property.hasClass('rhea-numeric')) {
                                dependentPropertyValue = $.autoNumeric.Format(property, dependentPropertyValue);
                            }

                            property.val(dependentPropertyValue);
                            property.change();
                        }
                    });
                }
            });
        },

        datetimepicker: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.datetime').each(function () {
                var hiddenInput = $($(this).find('input[type="hidden"]')[0]);
                var dateContainer = $(this);
                var timeContainer = $(this);
                var dateInput = $(dateContainer.find('input')[0]);
                var timeInput = $(timeContainer.find('input')[1]);

                // Check minimum date value on input
                var todayFormatted = new Date().toString('d/MM/yyyy');
                if (dateInput.val() == '1/01/0001') {
                    dateInput.val(todayFormatted);
                }

                // Apply transform to container to make plugin apply to group add on button
                dateInput.datepicker({
                    format: 'd/mm/yyyy',
                    todayBtn: true,
                    todayHighlight: true, 
                    autoclose: true,
                    weekStart: 1,
                });

                dateContainer.find('.fordate').bind('click', function () {
                    if (dateInput.attr('disabled') == undefined && dateInput.attr('readonly') == undefined) {
                        dateInput.focus();
                    }
                });

                // Apply transform to container to make plugin apply to group add on button
                timeInput.timepicker({
                    timeFormat: 'g:i A'
                });

                timeContainer.find('.fortime').bind('click', function () {
                    if (timeInput.attr('disabled') == undefined && timeInput.attr('readonly') == undefined) {
                        timeInput.focus();
                    }
                });

                dateInput.bind('change.date focusout.date', function () {
                    var container = $(this).parent();
                    
                    var hiddenInput = $(container.find('input[type="hidden"]')[0]);
                    var dateInput = $(container.find('input')[0]);

                    var oldValue = hiddenInput.val();
                    var oldValues = oldValue.split(' ');

                    if (oldValues.length != 3) {
                        oldValues = [todayFormatted, '12:00', 'AM'];
                    }

                    if (oldValues[0] == '1/01/0001') {
                        oldValues[0] = todayFormatted;
                    }

                    var newValue = $.trim(dateInput.val());

                    if (newValue != '') {
                        // Check if value is valid. Use if it is, otherwise revert it to last known good value
                        if (Date.parseExact(newValue, 'd/mm/yyyy') != null) {
                            hiddenInput.val(newValue + ' ' + oldValues[1] + ' ' + oldValues[2]);
                        } else {
                            // Revert dateInput value using hiddenInput
                            dateInput.val(oldValues[0]);
                        }
                    }
                });

                timeInput.bind('change.time focusout.date', function () {
                    var container = $(this).parent();

                    var hiddenInput = $(container.find('input[type="hidden"]')[0]);
                    var timeInput = $(container.find('input')[1]);

                    var oldValue = hiddenInput.val();
                    var oldValues = oldValue.split(' ');

                    if (oldValues.length != 3) {
                        oldValues = [todayFormatted, '12:00', 'AM'];
                    }

                    var newValue = $.trim(timeInput.val());

                    if (newValue != '') {
                        // Check if value is valid. Use if it is, otherwise revert it to last known good value
                        if (Date.parseExact(newValue, 'h:mm tt') != null) {
                            hiddenInput.val(oldValues[0] + ' ' + newValue);
                        } else {
                            // Revert timeInput value using hiddenInput
                            timeInput = oldValues[1] + oldValues[2];
                        }
                    }
                });
            });


        },

        datepicker: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);
            
            root.find('.date').each(function () {
                var container = $(this);
                var input = $($(this).find('input')[0]);

                // Check minimum date value on input
                var todayFormatted = new Date().toString('d/MM/yyyy');
                if (input.val() == '1/01/0001') {
                    input.val(todayFormatted);
                }

                // Apply transform to container to make plugin apply to group add on button
                container.datepicker({   
                    format: 'd/mm/yyyy',
                    todayBtn: true,
                    todayHighlight: true, 
                    autoclose: true,
                    weekStart: 1,
                });

                input.bind('change.date focusout.date', function () {
                    var oldValue = input.data('lastval');

                    if (oldValue == '1/01/0001') {
                        oldValue = todayFormatted;
                    }

                    var newValue = $.trim(input.val());

                    if (newValue != '') {
                        // Check if value is valid. Use if it is, otherwise revert it to last known good value
                        if (Date.parseExact(newValue, 'd/mm/yyyy') != null) {
                            input.data('lastval', newValue);
                        } else {
                            // Revert dateInput value using hiddenInput
                            input.val(oldValue);
                        }
                    }
                });
            });
        },

        timepicker: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.time').each(function () {
                var container = $(this);
                var input = $($(this).find('input')[0]);
                var btn = $($(this).find('.input-group-addon')[0]);

                // Apply transform to container to make plugin apply to group add on button
                var tp = input.timepicker({
                    timeFormat: 'g:i A'
                });

                btn.bind('click', function () {
                    if (input.attr('disabled') == undefined && input.attr('readonly') == undefined) {
                        input.focus();
                    }
                });

                input.bind('change.date focusout.date', function () {
                    var oldValue = input.data('lastval');

                    var newValue = $.trim(input.val());

                    if (newValue != '') {
                        // Check if value is valid. Use if it is, otherwise revert it to last known good value
                        if (Date.parseExact(newValue, 'h:mm tt') != null) {
                            input.data('lastval', newValue);
                        } else {
                            // Revert dateInput value using hiddenInput
                            input.val(oldValue);
                        }
                    }
                });
            });
        },

        numeric: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-numeric').each(function () {

                var options = {};

                options.aSep = '';
                
                if ($(this).data('val-currency') != undefined) {
                    options.aSep = ',';
                    options.aSign = '$';
                }
                
                options.wEmpty = (/^true$/i.test($(this).data(dataTypes.IsNullable))) ? 'empty' : 'zero';

                if ($(this).data(dataTypes.Decimal) != undefined) {
                    options.mDec = parseInt($(this).data(dataTypes.Decimal));
                } else {
                    options.mDec = 0;
                }

                var min = $(this).data('val-range-min');
                var max = $(this).data('val-range-max');

                if (min != undefined) {
                    if ($(this).data(dataTypes.Decimal) != undefined) {
                        options.vMin = parseFloat(min);
                    } else {
                        options.vMin = parseInt(min);
                    }
                }

                if (max != undefined) {
                    if ($(this).data(dataTypes.Decimal) != undefined) {
                        options.vMax = parseFloat(max);
                    } else {
                        options.vMax = parseInt(max);
                    }
                }
                
                $(this).autoNumeric(options);
            });
        },

        ajaxload: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-ajaxload').each(function () {
                var property = $(this);

                // Remove so this doesn't trigger again on this page load
                property.removeClass('rhea-ajaxload');

                var url = property.data(dataTypes.Url);
                var prefix = property.data(dataTypes.FieldPrefix);
                var parameters = [];
                var results = {};

                if (property.data(dataTypes.Parameters) != undefined && property.data(dataTypes.Parameters).length > 0) {
                    parameters = property.data(dataTypes.Parameters).split(',');
                }

                if (parameters.length > 0) {
                    for (var i = 0; i < parameters.length; i++) {

                        var key = parameters[i];

                        var targetId = (prefix != undefined && prefix.length > 0) ? prefix + '_' + key : key;

                        key = key.toLowerCase();
                        
                        results[key] = $('#' + targetId).val();
                    }
                }
                
                var groupNameID = property.data(dataTypes.PropertyNameForAjax);

                if (groupNameID == undefined || groupNameID.length == 0) {
                    return;
                }

                var group = $('#ContainerFor-' + groupNameID);
                var innerGroup = $('#InnerContainerFor-' + groupNameID);

                if (group == undefined || group.length == 0) {
                    return;
                }

                var p = group.parent();
                var groupNamePart = group.children()[0].id.replace(/^ContainerFor-/, '');
                var pT = $('#' + groupNameID + '_ParentType');
                var pN = $('#' + groupNameID + '_PropertyNameInParent');


                var panel = $(group).first('.panel');
                var contentContainer = $(property).find('.panel-body');

                if (!panel.hasClass('panel-loading')) {
                    var spinner = $('<div class="panel-loader"><span class="spinner-small"></span></div>');
                    panel.addClass('panel-loading');
                    var panelBody = panel.find('.panel-body');
                    panelBody.prepend(spinner);
                    //panelBody.hide();

                    /*
                    var ajaxOptions = {
                        url: url,
                        global: false, // Prevents the "Please wait" notices set in $.ajaxStart(), $.ajaxSend(), etc. being triggered, as the loading spinner makes them redundant.
                        success: function (data) {
                            spinner.remove();
                            panel.removeClass('panel-loading');
                            contentContainer.html(data);
                            $rhea.prepareNewContent(contentContainer);
                        },
                        error: function (data) {
                            spinner.remove();
                            panel.removeClass('panel-loading');
                            contentContainer.html('<div>Error loading content</div><div style="display: none">' + /<body.*?>([\s\S]*?)<\/body>/img.exec(data.responseText)[1] + '</div>');
                        }
                    };*/
                }


                $.ajax({
                    type: 'GET',
                    dataType: 'html',
                    global: false, // Prevents the "Please wait" notices set in $.ajaxStart(), $.ajaxSend(), etc. being triggered, as the loading spinner makes them redundant.
                    url: url,
                    cache: false,
                    data: results,
                    headers: {
                        'Zeus-Ajax': true,
                        'Zeus-Parent-Type': pT.val(),
                        'Zeus-Property-Name': pN.val()
                    }
                }).done(function (data) {
                    spinner.remove();
                    panel.removeClass('panel-loading');
                    //panelBody.show();
                    //var parent = panel.parent();
                    //panel.remove();

                    //parent.html(data);
                    //$rhea.prepareNewContent(parent);
                    group.html(data);
                    $rhea.prepareNewContent(group);
                    $('#main_form').data('initialForm', $rhea.serializeform($('#main_form'))); // Update initial form data with new form
                });
            });
        },

        groupajax: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('[' + fullDataTypes.PropertyNameForAjax + ']').each(function () {
                var element = $(this);

                var groupNameID = element.data(dataTypes.PropertyNameForAjax);

                if (groupNameID == undefined || groupNameID.length == 0) {
                    return;
                }

                // Handle anchor
                if (element.is('a')) {
                    element.bind('click.zeus-groupajax', function (e) {
                        e.preventDefault();


                        var group = $('#ContainerFor-' + groupNameID);
                        var innerGroup = $('#InnerContainerFor-' + groupNameID);
                        if (group == undefined || group.length == 0) {
                            return;
                        }

                        var p = group.parent();
                        var groupNamePart = group.children()[0].id.replace(/^ContainerFor-/, '');
                        var pT = $('#' + groupNameID + '_ParentType');
                        var pN = $('#' + groupNameID + '_PropertyNameInParent');

                        var panel = group.first('.panel');

                        if (panel.hasClass('panel-loading')) {
                            // Ignore click when already loading
                            return;
                        }
                        
                        var spinner = $('<div class="panel-loader"><span class="spinner-small"></span></div>');
                        panel.addClass('panel-loading');
                        var panelBody = panel.find('.panel-body');
                        panelBody.prepend(spinner);
                        //var h = panelBody.height();
                        //panelBody.attr('height', h);
                        //panelBody.hide();
                        


                        $.ajax({
                            type: 'GET',
                            dataType: 'html',
                            global: false,
                            url: e.target.href,
                            cache: false,
                            headers: {
                                'Zeus-Ajax': true,
                                'Zeus-Parent-Type': pT.val(),
                                'Zeus-Property-Name': pN.val()
                            }
                        }).done(function (data) {
                            spinner.remove();// not necessary? replacement of group gets rid of this anyway?
                            panel.removeClass('panel-loading'); // not necessary? replacement of group gets rid of this anyway?
                            
                            //panelBody.show();
                            // When a title is present it means the user session has expired and they've been presented with the STS login form
                           // if ($.zeusValidate.extractTitle(data) !== false) {
                                // So exit early and allow the global ajaxComplete to handle this
                            //    return;
                           // }

                            group.html(data);
                            $rhea.prepareNewContent(group);
                            $('#main_form').data('initialForm', $rhea.serializeform($('#main_form'))); // Update initial form data with new form
                        });
                    });
                }
            });
        },

        trigger: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-trigger').each(function () {

                var property = $(this);
                
                var submitTypeName = property.data(dataTypes.SubmitName);
                var submitType = property.data(dataTypes.SubmitType);

                // If property has no ID then it is a Telerik control which has the actual input with ID nested inside its own markup
                if (property[0].id.length == 0 && property.find('input').length > 0) {
                    // Get nested input property
                    property = property.find('input');
                }

                property.bind('change.rhea-trigger', function () {

                    var initialData = $('#main_form').data('initialForm');
                    var serializedData = $rhea.serializeform($('#main_form')) + '&' + submitTypeName + '=' + submitType;
                    var propertyId = property.attr('id');
                   
                    $.ajax({
                        type: 'POST',
                        data: serializedData
                    }).done(function (data) {
                        // When a title is present it means the user session has expired and they've been presented with the STS login form
                        if ($.zeusValidate.extractTitle(data) !== false) {
                            // So exit early and allow the global ajaxComplete to handle this
                            return;
                        }

                        // If there is an open select2, close it before updating the page
                        var openSelect = $("#select2-drop");
                        
                        if (openSelect.length > 0) {
                            openSelect.select2('close');
                        }
                        
                        var content = $('#content');
                        content.html(data);
                        $rhea.prepareNewContent(content);
                        $('#main_form').data('initialForm', initialData); // Reset with original initial form data from before trigger

                        var s2element = $('#s2id_focus_' + propertyId);
                        var select2element = $('#s2id_focus_select2_' + propertyId);

                        var refocus;
                        
                        if (s2element.length > 0) {
                            refocus = function () { $('#' + propertyId).select2('focus'); };
                        } else if (select2element.length > 0) {
                            refocus = function () { $('#select2_' + propertyId).select2('focus'); };
                        } else {
                            refocus = function () { $('#' + propertyId).focus(); };
                        }
                        
                        refocus();
                        setTimeout(refocus, 1);
                    });
                });
            });
        },

        hierarchy: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-hierarchy').each(function () {
                var property = $(this);
                var dependentProperty = $('#' + property.data(dataTypes.DependentPropertyId));

                dependentProperty.bind('change.rhea', function () {
                    
                    var dependentValue = dependentProperty.val();

                    if (dependentValue != '' && dependentValue != null && dependentValue != undefined) {
                        if (property.attr('disabled')) {
                            property.removeAttr('disabled');
                        }
                        if (property.attr('readonly')) {
                            property.removeAttr('readonly');
                        }
                    } else {
                        var readOnlyType = property.data(dataTypes.ReadOnlyType);
                        property.attr(readOnlyType, readOnlyType);
                    }

                    var originalVal = property.val();
                    
                    property.empty();
                    property.append($('<option />').val('').text(''));
                    property.val('');
                    
                    if (originalVal != property.val()) {
                        property.change();
                    }

                    $rhea._autocomplete(property);
                });
            });
        },

        paged: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-paged').each(function () {
                // Paged link
                var property = $(this);

                    if (property.attr('data-rhea-scroll-load') == 'true') {
                    $(window).bind('scroll', function () {
                        if ($(window).scrollTop() + $(window).height() >= root.height() - 200) {
                            // user has reached bottom of the page
                            $rhea.loadMoreResults(property);
                        }
                    });
                    }

                // Show paged link (only visible when javascript is enabled for AJAX paging)
                property.removeClass('hidden');
                
                property.bind('click.rhea-paged', function (e) {
                    // Prevent paged link default behaviour
                    e.preventDefault();

                    $rhea.loadMoreResults(property);
                });
            });
        },

        loadMoreResults: function (property) {
            $rhea = this;

            // Grid
            var gridProperty = $('#' + property.data(dataTypes.PropertyIdGrid));

            // Do not execute AJAX if paged link has since been hidden
            if (property.hasClass('hidden')) {
                return;
            }

            // Paged metatadata
            var metadataValue = '';

            if (property.is('[' + fullDataTypes.PagedMetadata + ']')) {
                // History or Bulletins
                metadataValue = property.data(dataTypes.PagedMetadata);
            }
            else {
                // Grid
                metadataValue = $('#' + property.data(dataTypes.PagedMetadataPropertyId)).val();
            }

            // Call for next page using paged metadata
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: property.data(dataTypes.Url),
                data: {
                    metadata: metadataValue
                },
                cache: true
            }).done(function (data) {
                if (data == '' || data == null) {
                    // Hide paged link and return if no data came back
                    property.addClass('hidden');
                    return;
                }

                // Append retrieved page to grid
                var newContent = $(data);
                gridProperty.append(newContent);
                $rhea.prepareNewContent(newContent);


                // *******Sort Table*******

                //                        var table = gridProperty.parent('table');
                //                        var currentColumn = table.find('.sorting-desc');
                //                        currentColumn = currentColumn.length === 0 ? table.find('.sorting-asc') : currentColumn;
                //                        
                //                        //  call SortsTable method here.
                //                        var isReSort = true;
                //                        $.fn.sortsTable(table, currentColumn, null, isReSort);


                //*******End of Sort Table*******


                // Get latest paged metadata
                var metadata = $(data).last().data(dataTypes.PagedMetadata);

                if (metadata) {
                    // Update metadata
                    if (property.is('[' + fullDataTypes.PagedMetadata + ']')) {
                        // History or Bulletins
                        property.data(dataTypes.PagedMetadata, metadata);
                    }
                    else {
                        // Grid
                        $('#' + property.data(dataTypes.PagedMetadataPropertyId)).val(metadata);
                    }
                }

                var hasMore = (/^true$/i.test($(data).last().data(dataTypes.PagedHasMore))) ? true : false;

                if (!hasMore) {
                    // Hide paged link if there are no more pages
                    property.addClass('hidden');
                }

                // No need to resync for history
                if (property.is('[' + fullDataTypes.PagedMetadata + ']')) {
                    return;
                }

                // Resync the 'initialForm' data to include the retrieved page data so the dirty check behaves correctly

                // Get current form by grabbing parent form of property
                var parentForm = property.closest('form');

                if (parentForm == undefined) {
                    parentForm = $('#main_form');
                }

                // Initial form data as array
                var initialFormArray = parentForm.data('initialForm').split('&');

                // Retrieved page data as array
                var pageArray = $(data).find(':input').serialize().split('&');

                // Metadata property id as name
                var name = $.zeusValidate.replaceAll(property.data(dataTypes.PagedMetadataPropertyId), '_', '.');
                var placeholder = $.zeusValidate.replaceAll(gridProperty.attr('id').replace('-grid', ''), '_', '.') + '=';

                var updatedFormArray = new Array();

                for (var i = 0; i < initialFormArray.length; i++) {
                    // If current data is the metadata property, don't add it and instead...
                    if (initialFormArray[i].indexOf(name) == 0) {
                        // Include the retrieved page data
                        for (var j = 0; j < pageArray.length; j++) {
                            updatedFormArray.push(pageArray[j]);
                        }

                        // Include placeholder
                        updatedFormArray.push(placeholder);

                        // And include the updated metadata
                        updatedFormArray.push($('#' + property.data(dataTypes.PagedMetadataPropertyId)).val(metadata).serialize());
                    } else {
                        // Ignore placeholder as we need to add that just before metadata
                        if (initialFormArray[i] != placeholder) {
                            // Include initial form data in updated form data
                            updatedFormArray.push(initialFormArray[i]);
                        }
                    }
                }

                // Update initial form data to the updated form data
                parentForm.data('initialForm', updatedFormArray.join('&'));
            });

        },
        
        historypin: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);
            
            // Space out each individual history panel
            var topOffset = 100;
            var zindexOffset = 1020;
            root.find('.theme-panel').each(function () {
                $(this).css('top', topOffset + 'px');
                topOffset += 50;
                $(this).css('z-index', zindexOffset);
                zindexOffset -= 10;
            })

            root.find('.history-pin').each(function () {
                var pin = $(this);

                var historyType = pin.data(dataTypes.HistoryType);
                var historyDescription = pin.data(dataTypes.HistoryDescription);
                var objectValues = pin.data(dataTypes.ObjectValues);
                var url = pin.data(dataTypes.Url);

                var parentItem = pin.closest('li');
                var parentList = parentItem.closest('ul');

                pin.on('click.zeus-pin', function (e) {
                    e.preventDefault();
                    var pinnedCount = parseInt(parentList.attr(fullDataTypes.PinnedCount));

                    // Check for early exit on exceeding pin count
                    var limit = 10;
                    if (!pin.hasClass('pinned') && pinnedCount >= limit) {
                        $.zeusValidate.addPropertyError('pinLink', 'Pin ' + historyDescription, 'You have reached the maximum limit of ' + limit + ' pinned ' + historyDescription + ' records. To add this record to your pinned list, you must first remove a ' + historyDescription + ' record.');
                        return; // Early exit
                    }

                    // Update server record
                    $.ajax({
                        type: 'POST',
                        global: false,
                        url: url,
                        data: {
                            historyType: historyType,
                            values: objectValues
                        },
                        cache: false
                    });

                    pin.toggleClass('pinned');
                    if (pin.hasClass('pinned')) {
                        // Push to top of list
                        parentItem.prependTo(parentList);
                        // Update icon
                        pin.find('i').removeClass("fa-bookmark-o").addClass("fa-bookmark");
                        // Update url
                        url = url.replace('Pin', 'Unpin');
                        // Update pin count
                        parentList.attr(fullDataTypes.PinnedCount, pinnedCount + 1);
                    }
                    else {
                        // Push to bottom of list
                        parentItem.appendTo(parentList);
                        // Update icon
                        pin.find('i').removeClass("fa-bookmark").addClass("fa-bookmark-o");
                        // Update url
                        url = url.replace('Unpin', 'Pin');
                        // Update pin count
                        parentList.attr(fullDataTypes.PinnedCount, pinnedCount - 1);
                    }

                    // Move focus to link, for keyboard users
                    pin.prev().focus();
                });
            });
        },

        multiplegridselect: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            root.find('.rhea-multiple-select-grid').each(function () {
                var property = $(this);
                
                // Grid
                var gridProperty = $('#' + property.data(dataTypes.PropertyIdGrid));
                
                // See if all are checked
                var allChecked = true;
                gridProperty.find('input:checkbox').each(function () {
                    if ($(this)[0] != property[0] && !$(this).is(':checked')) {
                        allChecked = false;
                    }
                });
                property.attr({ checked: allChecked });

                // Show select/deselect all checkbox (only visible when javascript is enabled)
                property.removeClass('hidden');

                property.bind('click.rhea-mutiple-select-grid', function () {
                    // Set all checkboxes in grid to match the select/deselect all checkbox
                    gridProperty.find('input:checkbox').each(function () {
                        $(this).attr({ checked: property.is(':checked') });
                    });
                });
                
            });
        },
        
        requiredifindicator: function () {
            // Bind to change event of RequiredIf dependent properties to check whether the indicator should be visible or not
            $('[data-val-requiredif-dependentproperty]').each(function () {
                var property = $(this);
                
                var requiredIndicator = $('label[for=' + property.attr('id') + ']').find('abbr.req');
                if (requiredIndicator.length == 0) {
                    requiredIndicator = $('label[for=s2id_focus_' + property.attr('id') + ']').find('abbr.req');

                    if (requiredIndicator.length == 0) {
                        requiredIndicator = $('label[for=s2id_focus_select2_' + property.attr('id') + ']').find('abbr.req');
                    }

                    if (requiredIndicator.length == 0 && property.hasClass('rhea-dualselect')) {
                        requiredIndicator = $('label[for=' + property.attr('id') + '_Selected]').find('abbr.req');
                    }
                }

                var dependentPropertyId = $.zeusValidate.getFieldPrefixFromId(property[0], property.data('val-requiredif-dependentproperty'));
                var dependentProperty = $('#' + dependentPropertyId);
                var comparisonType = property.data('val-requiredif-comparisontype');
                var valueToTestAgainst = property.data('val-requiredif-value');
        
                // Handle case where value to test against is an array
                if ($.zeusValidate.isJSON(property.data('val-requiredif-value'))) {
                    var valuesToTestAgainst = $.parseJSON(property.data('val-requiredif-value'));
        
                    if ($.isArray(valuesToTestAgainst)) {
                        valueToTestAgainst = valuesToTestAgainst;
                    }
                } 
        
                var passOnNull = property.data('val-requiredif-passonnull');
                var failOnNull = property.data('val-requiredif-failonnull');
                var dependentPropertyValue = null;


                var isRequired = function () {
                    dependentPropertyValue = null;
                    if (dependentProperty.length > 1 || dependentProperty[0].type == 'checkbox') {
                        for (var index = 0; index != dependentProperty.length; index++) {
                            if (dependentProperty[index]["checked"]) {
                                dependentPropertyValue = dependentProperty[index].value;
                                break;
                            }
                        }

                        if (dependentPropertyValue == null) {
                            dependentPropertyValue = false;
                        }
                    } else if (dependentProperty[0].type == 'radio') {
                        // Grab all radio buttons for dependent property and use selected value
                        var dependentProperties = $('[name="' + dependentProperty[0].name + '"]');
                        for (var index = 0; index != dependentProperties.length; index++) {
                            if (dependentProperties[index]["checked"]) {
                                dependentPropertyValue = dependentProperties[index].value;
                                break;
                            }
                        }
                    } 
                    
                    else if (dependentProperty[0].type == "select-multiple" && dependentProperty[0].length != undefined && dependentProperty[0].length > 0 &&
                        dependentProperty[0].value !== undefined && dependentProperty[0].value != "") { 
                        dependentPropertyValue = [];
                        for (var j = 0; j < dependentProperty[0].children.length; j++) {
                            if (dependentProperty[0].children[j].selected) {
                                dependentPropertyValue.push(dependentProperty[0].children[j].value);
                            }
                        }
                        
                    }

                    else {
                        dependentPropertyValue = dependentProperty[0].value;
                    }
                    
                    if ($.zeusValidate.is(dependentPropertyValue, comparisonType, valueToTestAgainst, passOnNull, failOnNull)) {
                        // Required so show indicator
                        requiredIndicator.removeClass('hidden');
                    } else {
                        // Hide indicator
                        requiredIndicator.addClass('hidden');
                    }
                };

                if (dependentProperty[0].type == 'checkbox' || dependentProperty[0].type == 'radio' || dependentProperty.length > 1) {

                    var dependentProperties = $('[name="' + dependentProperty[0].name + '"]');
                    
                    for (var index = 0; index != dependentProperties.length; index++) {
                        $(dependentProperties[index]).bind('change.rhea-indicator', function () {
                            isRequired();
                        });
                    }
                } else {
                    dependentProperty.bind('change.rhea-indicator', function () {
                        isRequired();
                    });
                }
            });
        },
        
        crn: function () {
            
            $('[data-val-crn]').bind('change.rhea-crn', function () {
                this.value = this.value.toUpperCase();
            });
            
            $('[data-val-crn]').bind('keyup.rhea-crn', function () {
                var selectionStart = this.selectionStart;
                var selectionEnd = this.selectionEnd;
                
                this.value = this.value.toUpperCase();
                
                this.selectionStart = selectionStart;
                this.selectionEnd = selectionEnd;
            });
        },
        
        tooltipposition: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            var checkPosition = function (property) {
                    var span = property.find('span');

                    // Store current top setting for below position
                if (span.data('top') == undefined) {
                        if (span[0].currentStyle != undefined) {
                            span.data('top', span[0].currentStyle.top);
                        }
                    }

                    var offset = property.offset();
                    var height = property.outerHeight(false);
                    var spanHeight = span.outerHeight(false);

                    var viewportBottom = $(window).scrollTop() + $(window).height();
                    var spanTop = offset.top + height;
                    var enoughRoomBelow = spanTop + spanHeight <= viewportBottom;

                    // Default to use below position
                    var top = span.data('top');

                    // But if there's not enough room, reposition above
                    if (!enoughRoomBelow) {
                        top = -(spanHeight + 3);
                    }

                    span.css({ top: top });
                };
            
            var changeVisibility = function (property, isErrorTip) {
                
                if (isErrorTip === true) {
                    if ($(root.find('.hintTip span')).hasClass('visible')) {
                        $(root.find('.hintTip span')).removeClass('visible');
                    }
                }
                else {
                    if ($(root.find('.errorTip span')).hasClass('visible')) {
                        $(root.find('.errorTip span')).removeClass('visible');
                    }
                }
                var span = property.find('span');
            
                if ($(span).hasClass('visible')) {
                    $(span).removeClass('visible');
                } else {
                    $(span).addClass('visible');
                    checkPosition(property);
                }
            };


            var hideTips = function () {

                if ($(root.find('.hintTip span')).hasClass('visible')) {
                    $(root.find('.hintTip span')).removeClass('visible');
                }

                if ($(root.find('.errorTip span')).hasClass('visible')) {
                    $(root.find('.errorTip span')).removeClass('visible');
                }

            };


            root.find('.errorTip').each(function () {
                var property = $(this);
//                property.bind('focus.rhea-tooltipposition', function() { checkPosition(property); });
//                property.bind('hover.rhea-tooltipposition', function() { checkPosition(property); });
                var isErrorTip = true;
                property.bind('click.rhea-tooltipposition', function () { changeVisibility(property, isErrorTip); });
                property.bind('onenter.rhea-tooltipposition', function () { changeVisibility(property, isErrorTip); });
                property.bind('focusout.rhea-tooltipposition', function () { hideTips(); });
            });
            
            root.find('.hintTip').each(function () {
                var property = $(this);

//                property.bind('focus.rhea-tooltipposition', function() { checkPosition(property); });
//                property.bind('hover.rhea-tooltipposition', function() { checkPosition(property); });
                var isErrorTip = false;
                property.bind('click.rhea-tooltipposition', function () { changeVisibility(property, isErrorTip); });
                property.bind('onenter.rhea-tooltipposition', function () { changeVisibility(property, isErrorTip); });
                property.bind('focusout.rhea-tooltipposition', function () { hideTips(); });
            });
        },
        

        clear: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);
            
                root.find('button[' + fullDataTypes.ButtonClear + '="true"]')
            .each(function () {
                var clearButton = $(this);
                // Bind Click event to this button
                clearButton.bind('click', function (e) {

                    // prevent from leaving the current page.
                    e.preventDefault();

                    var parentPanel = $(clearButton).parents('div.panel');

                    var allElments;
               
                    // check if button is inside a panel
                    if (parentPanel.length == 1) {
                        // get elements within that panel
                        allElments = $(parentPanel[0]).find('*'); //.elements; 
                        
                        // Clear select2 elements
                        $(parentPanel).find('.select2-container').each(function () {
                            var disabled = false;
                            var childInputs = $(this).find('input');
                            if (childInputs.length > 0) disabled = childInputs[0].disabled;
                            if (disabled) {
                                $(this).select2('readonly', true);
                            } else {
                                $(this).select2("val", "", true);
                             }
                         });
                    } 
                    else {
                        // if Button is not contained in panel, then get all elements of form
                        allElments = $('#main_form *');
                        // Clear select2 elements
                         $(allElments).find('.select2-container').each(function () {
                            var disabled = false;
                            var childInputs = $(this).find('input');
                            if (childInputs.length > 0) disabled = childInputs[0].disabled;
                            if (disabled) {
                                $(this).select2('readonly', true);
                            } else {
                                $(this).select2("val", "", true);
                            }
                        });
                    }
                   
                    
                   if (allElments != undefined) {
                       for (var i = 0; i < allElments.length; i++) {
                            if (($(allElments[i]).attr('readOnly') != undefined)
                           ||
                            (allElments[i].hasAttribute('disabled'))) {
                               continue;
                           }
                           var elementType = allElments[i].type;
                           if (elementType) {
                               switch (elementType.toLowerCase()) {
                               case "text":
                               case "password":
                               case "textarea":     // case "hidden":
                                   if ( 
                                       $(allElments[i]).hasClass('select2-focusser')
                                        ||
                                        $(allElments[i]).hasClass('select2-input')
                                        ||
                                        $(allElments[i]).hasClass('select2-offscreen')
                                            ) {
                                       // if input is part of SELECT2 control.
                                   } else {
                                       allElments[i].value = '';
                                   }
                                   break;
                               case "radio":
                               case "checkbox":
                                   if (allElments[i].checked) {
                                       allElments[i].checked = false;
                                   }
                                   break;
                               case "select":
                               case "select-one":
                               case "select-multi":
                                   // setting Select element's index to -1, on their value Select2 is dependent.
                                       allElments[i].selectedIndex = -1;
                                   
                                   break;                                
                               default:
                                   break;
                               }
                           }

                       }
                   }
                });

            });
            
            
        },


        fooTheTable: function () {
            $(".footable").footable();
        },
        
        knockout: function () {
            // Register the dirtyFlag function on Knockout
            ko.dirtyFlag = function (root, isInitiallyDirty) {
                var result = function () { },
                    _initialState = ko.observable(ko.toJSON(root)),
                    _isInitiallyDirty = ko.observable(isInitiallyDirty);

                result.isDirty = ko.computed(function () {
                    return _isInitiallyDirty() || _initialState() !== ko.toJSON(root);
                });

                result.reset = function () {
                    _initialState(ko.toJSON(root));
                    _isInitiallyDirty(false);
                };

                return result;
            };


            // Configure and databind the tables
            $(".editable-table").each(function () {
                //var viewModel = KOViewModel(JSON.parse($(this).find(".rhea-ko-data").attr("value")));
                var theTable = $(this).children("table");
                var vmDataString = theTable.attr("data-rhea-ko-data");
                if (vmDataString.length > 0) {
                    //var viewModel = KOViewModel(JSON.parse(vmDataString));
                    var vm = ko.mapping.fromJSON(vmDataString, koGridRowMapping, {});
                    vm.dirtyItems = ko.computed(function () {
                        return ko.utils.arrayFilter(this.rows(), function (item) {
                            return item.dirtyFlag.isDirty();
                        });
                    }, vm);

                    vm.isDirty = ko.computed(function () {
                        return this.dirtyItems().length > 0;
                    }, vm);

                    ko.applyBindings(vm, $(this)[0]);

                }



            });

            $(".gridSaveButton").click(function () {
                //var fgdg = $(this).siblings("table > tbody");

                var vm = ko.dataFor($(this)[0]);

                if (vm.isDirty()) {
                    var theUrl = $(this).attr("data-saveurl");
                    var theData = ko.toJSON(vm.dirtyItems());

                    $.ajax({
                        url: theUrl,
                        type: "POST",
                        contentType: "application/json",
                        data: theData,
                        success: function (status) {
                            alert(status);
                        },
                        error: function (request, status, theError) {
                            alert(theError);
                        }
                    });
                }


            });

        },

        gridSortable: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);
            
            // Foreach grid on the page:
                // Foreach link header tag of this grid bind click event on them.
                    // get the source of image to determine if the grid column was sorted previously
                            // if so reverse the sort order and update the image
                            // otherwise set the sort order to ascending and update the image.
                    // sort the grid based on sort order selected in previous steps.
            root.find('table').each(function () {
             
                var currentTable = this;
                
              

                $(currentTable).sortTable({
                    
                    // add custom functions
                });
                


                $(currentTable).on('beforetablesort', function (event, data) {
                    // block ui //$.blockUI($.zeusValidate.blockUIoptions);
                    $.blockUI($.zeusValidate.blockUIoptions);
                });

                $(currentTable).on('aftertablesort', function (event, data) {


                    // get the columns
                    var th_all = $(this).find('th');
                    //  remove class 'arrow' from header columns
                    th_all.find('.arrow').remove();
                    var dir = $.fn.sortTable.dir;

                    var arrow = data.direction === dir.ASC ? '&uarr;' : '&darr;';
                    // add relevant arrow according to sort order to sorted column.
                    th_all.eq(data.column).append('<span class="arrow">' + arrow + '</span>');


                    var readerText = "Sorted ";
                    readerText += data.direction === dir.ASC ? "ascending" : "descending";
                    readerText += ". Select to change sort order.";
                    var defaultReaderText = 'No sort order set. Select to change sort order.';

                    // set title text of anchor tag in headers to default and do the same for span tags.
                    th_all.find('a').each(function () {
                        $(this).attr('title', defaultReaderText);
                        $(this).find('.readers')[0].innerText = defaultReaderText;
                    });

                    // set appropriate title to anchor tag and text within span tag.
                    var thSortedAnchor = th_all.eq(data.column).find('a');
                    thSortedAnchor.attr('title', readerText);
                    thSortedAnchor.find('.readers')[0].innerText = readerText;

//                    thSortedAnchor.blur();
//                    thSortedAnchor.focus();

                 

 
                    // START RE-SYNC with initial Form data.
                    // Resync the 'initialForm' data to include the retrieved page data so the dirty check behaves correctly

                    // Get current form by grabbing parent form of property
                    var parentForm = $(this).closest('form');

                    if (parentForm == undefined) {
                        parentForm = $('#main_form');
                    }

                    // Get Initial form data as array
                    var initialFormArray = parentForm.data('initialForm').split('&');

                    // Obtain grid ID.
                    var gridId = $(currentTable).find('tbody')[0].id;

                    // Get a placeholder which Identifies the Result property records, this will later be used to loop through initialFormArray to find the position of records.
                    var placeholder = $.zeusValidate.replaceAll(gridId.replace('-grid', ''), '_', '.') + '%5B';

                    // Get the current serializedData of the form. We can obtain the SORTED records and then they will be used to replace Old records. Form array by splitting it '&' delimiter.
                    var currentFormArray = $rhea.serializeform($('#main_form')).split('&');

                    // Create a new array which will replace initialFormArray.
                    var updatedFormArray = new Array();

                    // Variable to check if the append was successful or not.
                    var currentInserted = false;

                    // loop through the initialFormArray, 
                    //   if (Placeholder not found at current index)
                    //      add that record into updateFormArray
                    //   else if the placeholder is found  (this will be true for each search result record.
                    //      if currentInserted is false
                    //          loop through currentFormArray 
                    //              find the position of placeholder in current metadata
                    //              if found add this record (newly sorted) to updatedFormArray
                    //              set our check to true, currentInserted = true.

                    for (var i = 0; i < initialFormArray.length; i++) {

                        if (initialFormArray[i].indexOf(placeholder) == 0) {
                            if (!currentInserted) {
                                for (var j = 0; j < currentFormArray.length; j++) {
                                    if (currentFormArray[j].indexOf(placeholder) == 0) {
                                        updatedFormArray.push(currentFormArray[j]);
                                    }
                                }
                                currentInserted = true;
                            }
                        } else {
                            updatedFormArray.push(initialFormArray[i]);
                        }
                    }
                    // Update initial form data to the updated form data
                    parentForm.data('initialForm', updatedFormArray.join('&'));
                    // END RE-SYNC with initial Form data.


                    

                    // unblock ui
                    $.unblockUI();

                });

                
            });
            
        },
        
        reset: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            var resetID = 0;
            root.find('input[type="reset"]').each(function () {
                var resetButton = $(this);
                
                // Assign ID to reset button for re-focus after reset
                resetButton.data('resetid', resetID++);
                
                resetButton.bind('click.rhea-reset', function (e) {
                    if ($.zeusValidate.initialContentHTML != undefined) {
                        e.preventDefault();
                    
                        // Get reset ID
                        var id = resetButton.data('resetid');
                    
                        // Get current initial data
                        var initialData = $('#main_form').data('initialForm');
                        
                        // Reload initial content HTML
                        $('#content').html($.zeusValidate.initialContentHTML);
                        
                        $rhea.prepareNewContent($('#content'));

                        $.zeusValidate.skipNextFocusErrors = true;
                        
                        // Reset with original initial form data from before trigger
                        $('#main_form').data('initialForm', initialData);

                        // Hide error messages
                        var container = root.find('[data-valmsg-summary="true"]');
                        var list = container.find('ul');
                        if (list && list.length) {
                            list.empty();
                            container.addClass('validation-summary-valid')
                                .removeClass('validation-summary-errors')
                                .removeClass('alert')
                                .removeClass('alert-danger');
                        }
                        
                        // Hide error indicators
                        $('.parsley-error').removeClass('parsley-error');
                        $('[' + fullDataTypes.ErrorTipFor + ']').hide();
                        
                        // Hide success messages
                        var successList = $('section.msgGood');      //.find('ul');
                        if (successList && successList.length) {
                            successList.empty();
                            successList.remove();
                        }
                        
                        // Hide warning messages
                        var warningList = $('section.msgWarn');      //.find('ul');
                        if (warningList && warningList.length) {
                            warningList.empty();
                            warningList.remove();
                        }
                        
                        // Hide information messages
                        var informationList = $('section.msgInfo');  //.find('ul');
                        if (informationList && informationList.length) {
                            informationList.empty();
                            informationList.remove();
                        }
                         
                        // Re-focus the reset button
                        $('input[type="reset"]').each(function () {
                            if ($(this).data('resetid') == id) {
                                $(this).focus();
                            }
                        });
                    }
                });
            });
        },
        
        smartautocomplete: function () {
            
        var textBoxId = "OrganisationNewAutocomplete";
            

        //Function the SA plugin called when data is needed.
        var getData = function (input, pageIndex, pageSize, callback) {
            if (input === undefined) {
                input = "#$%!"; // type something which is not available in options to avoid autocomplete opening on screen load.
            }

            $.ajax({
                    url: $("#" + textBoxId).data(dataTypes.Url), // urlInp,  //element.data(dataTypes.Url) 
                quietMillis: 1000,
                type: 'GET',
                //cache: true,
                dataType: 'json',
                data: {
                    input: input,
                    pageIndex: pageIndex,
                    pageSize: pageSize

                },

                success: function (response) {
                    
                    if (response) {
                        response = $.map(response, function (item) {
                            //alert(response+ " " + item.value);
                            return { label: item.Text, value: item.Value, selected: item.Selected };
                        });
                        callback(response);
                    } else {
                        callback();
                    }
                },
                error: function (e) {
                    alert('error' + e);
                },
                contentType: 'application/json; charset=utf-8'


            });


        };
            
            // or have foreach

            $('.textBoxAutocomplete').each(function () {
                    $(this).smartautocompleteSelect({
                 getDataFunc: getData,
                 pageSize: 10,
                 autoFocus: true,
                 minLength: 0
             });
            });

            /*
         if($("#"+textBoxId).length > 0) {

                $("." + textBoxId).smartautocompleteSelect({
                 getDataFunc: getData,
                 pageSize: 10,
                 autoFocus: true,
                 minLength: 0
             });
         }*/

        },
        
        // Convert appropriately marked input elements into the set of HTML elements required to render a 'switcher' instead of a checkbox.
        // Also hooks up the event handlers and focus handlers to make the switcher keyboard accessible and generally hide the fact that
        // there is still a hidden checkbox modelling the on-off behaviour.
        switchers: function () {
            var bools = $('input[' + fullDataTypes.Switcher + '="true"]');
            bools.each(function () {
                var jQueryCheckbox = $(this);
                var checkbox = $(this)[0];
                var subcontainer = $('<div></div>')
                    .append($('<span class="left">' + jQueryCheckbox.attr(fullDataTypes.SwitcherChecked) + '</span>'))
                    .append($('<span class="left-addition"></span>'))
                    .append($('<span class="right-addition"></span>'))
                    .append($('<span class="right">' + jQueryCheckbox.attr(fullDataTypes.SwitcherUnchecked) + '</span>'))
                    .append($('<span class="centre"></span>'))
                ;

                if (checkbox.checked) {
                    subcontainer.css('left', '0');
                    subcontainer.find(".right-addition").css("visibility", "hidden");
                }
                else {
                    subcontainer.css('left', '-55px');
                    subcontainer.find(".left-addition").css("visibility", "hidden");
                }

                var container = $('<div class="switcher">').append(subcontainer);
                var slideSwitcher = function () {
                    if (checkbox.checked) {
                        subcontainer.find(".left-addition").css("visibility", "visible");
                        subcontainer.animate({ left: '0' }, 300, 'swing', function () {
                            subcontainer.find(".right-addition").css("visibility", "hidden");
                        });
                    }
                    else {
                        subcontainer.find(".right-addition").css("visibility", "visible");
                        subcontainer.animate({ left: '-55px' }, 300, 'swing', function () {
                            subcontainer.find(".left-addition").css("visibility", "hidden");
                        });
                    }
                };
                jQueryCheckbox.bind("change", function (event) {
                    slideSwitcher();
                });
                jQueryCheckbox.bind("focus", function (event) {
                    container.addClass('switcher-focus');
                });
                jQueryCheckbox.bind("blur", function (event) {
                    container.removeClass('switcher-focus');
                });
                container.bind("click", function (event) {
                    jQueryCheckbox.click() // Toggle checkbox
                    jQueryCheckbox.focus();
                });
                jQueryCheckbox.after(container);
                jQueryCheckbox.addClass("readers"); // Hide from regular viewers
                // light green, dark green, light red, dark red, grey
                //#b0ebca, #3c763d, #f8b2b2, #a94442, #b6c2c9
                // better red, better green
                // #d9534f, #5cb85c
            });
        },

        // Automatically add a "Check all" check box to every check box list displayed in the page.
        // This is a convenience function only and removing it should not impact the overall function of the page.
        checkallforcheckboxes: function () {
            $(".check-box-list", this.element).each(function (index) {
                var listParentElement = $(this);
                var checkboxes = listParentElement.find('input[type="checkbox"]');

                if (checkboxes.length == 0) {
                    return;
                }

                var firstCheckbox = $(checkboxes[0]);
                var id = 'CheckAllFor-' + firstCheckbox.id;
                var input = $('<input type="checkbox" id="' + id + '">');
                var label = $('<label for="' + id + '">').text('Check all');
                var wrapper = $('<div class="checkbox">').append(input).append(label);
                var listItem = $('<li>').append(wrapper).prependTo(listParentElement);

                // Include a pointer back to the 'check all' checkbox from each checkbox
                // (used in editableif and readonlyif for dynamically matching the 'check all' enable state with the checkboxes)
                for (var i = 0; i < checkboxes.length; i++) {
                    $(checkboxes[i]).data('checkallid', id);
                }

                // Make sure initial enable state matches
                if (firstCheckbox.attr('disabled')) {
                    input.attr('disabled', firstCheckbox.attr('disabled'));
                }

                if (firstCheckbox.attr('readonly')) {
                    input.attr('readonly', firstCheckbox.attr('readonly'));
                }

                // Add check/uncheck all event
                input.bind("change.rhea-checkall", function (event) {
                    var checkboxes = listItem.siblings().find('input[type="checkbox"]');
                    $(this).attr('checked') ? checkboxes.attr('checked', $(this).attr('checked')) : checkboxes.removeAttr('checked');
                })
            });
        },

        // Use FLOT javascript libraries to render up appropriately annotated tables
        flotGraphs: function () {
            var $rhea = this;
            $(".zeus-graph-table", $rhea.element).each(function () {
                var table = $(this);

                // Data refresh organising section
                var topLevelButtons = $('[' + fullDataTypes.GraphTopLevelUrl + ']', $rhea.element);
                topLevelButtons.off(".zeus-graph-refresh");
                topLevelButtons.on("click.zeus-graph-refresh", function (event) {
                    event.preventDefault();
                    var contentContainer = $(this).closest('div').parent();
                    var uri = $(this).attr(fullDataTypes.GraphTopLevelUrl);
                    var panel = $(this).closest('.panel');
                    var graphType = panel.find('[' + fullDataTypes.GraphType + ']').attr(fullDataTypes.GraphType);
                    $rhea.getNewContentForPanel(panel, contentContainer, uri, JSON.stringify({ graphType: graphType }));
                });

                // Graph rendering part
                var graphsToRender = table.parent().siblings().find('div[' + fullDataTypes.GraphType + ']');
                graphsToRender.each(function (index) {
                    var graph = $(this);
                    var tipFormatter;
                    var postDataCreator;
                    var dataModel = [];
                    var graphType = graph.attr(fullDataTypes.GraphType);
                    var options = { // Common options for all graphs - they will be added to later by the specific graph type handling sections
                        grid: {
                            clickable: true,
                            hoverable: true,
                        },
                    };

                    // Choice container for graphs that allow toggling of data sets
                    var choiceContainer = $('<div class="zeus-graph-choice-container" style="margin: 0 auto">'); // This will be appended only if necessary
                    // Function to create a checkbox for  data series selection. Called below if needed, once for each data sereis in the dataModel
                    function createCheckboxFor(colNum, labelText) {
                        var choiceId = graph.attr('id') + '-choice-' + colNum;
                        var checkbox = $('<input type="checkbox" checked="checked" id="' + choiceId + '">');
                        checkbox.on("change.zeus-graph-choice", function (event) {
                            var chosenDataSets = [];
                            choiceContainer.find("input").each(function (index) {
                                if (this.checked) {
                                    chosenDataSets.push(dataModel[index]);
                                }
                            });
                            $.plot(graph, chosenDataSets, options);
                        });
                        choiceContainer.append(checkbox);
                        choiceContainer.append($('<label for="' + choiceId + '">').text(labelText));
                    }


                    // PIES -------------------------------------------------------------------------
                    if (graphType == "pie") {
                        // Generate the data
                        table.find('tr').each(function (rowNum) {
                            if (graphsToRender.length > 1 && rowNum == 0) { return; } // Skip the header row for multi pies
                            var key = $(this).find('th').text();
                            var value = parseFloat($(this).find('td').eq(index).text());
                            dataModel.push({
                                label: key,
                                data: value,
                            });
                        })

                        // Define pie options
                        $.extend(options, {
                            series: {
                                pie: {
                                    show: true,
                                    innerRadius: 0.0,
                                    label: {
                                        show: false,
                                        //formatter: function (seriesName, series, a, b) {
                                        //    return '<div data-rhea-series-index="'+(index++)+'"style="display: none; font-size:x-small;text-align:center;padding:2px;color:0;border: thick solid '+series.color+'; border-radius=5px;">'
                                        //        + seriesName + ': ' + series.data[0][1]
                                        //        + '</div>';
                                        //}
                                    }
                                }
                            },
                            legend: {
                                show: true,
                                position: "sw"
                            },
                            //colors: graphColourPalette,
                        });

                        // Initialise the graph - using a kludge to render it offscreen as flot demands that the graph area be visible,
                        // or else it will die claiming it can't render to a graph of height 0.
                        var tabPane = graph.parent();
                        tabPane.css("left", "-100000px").css("display", "block");
                        var plot = $.plot(graph, dataModel, options);
                        tabPane.css("display", '').css("left", ''); // Removes inline styles, so those styles revert to their class based values

                        tipFormatter = function (tip, item) {
                            tip.text(item.series.label + ': ' + item.series.data[item.dataIndex][1].toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
                        };
                        postDataCreator = function (item) {
                            return JSON.stringify({
                                label: item.series.label,
                                dataPoint: item.datapoint[1][0][1],
                                graphType: graphType,
                            })
                        }
                    }
                        // BARS -----------------------------------------------------------------------
                    else if (graphType == "bar") {
                        // Generate the data
                        var tickModel = [];
                        var counter = 0;
                        var numColumns = table.find('tr').last().find('td').length; // This is the length of one row only, but since the tables are all rectangular, this should be okay.
                        if (numColumns > 1) { // Multi bars
                            choiceContainer.appendTo(graph.parent());
                            var columns = table.find('tr').first().find('th');
                            columns.each(function (colNum) {
                                if (colNum == 0) { return; } // Skip the first empty cell
                                createCheckboxFor(colNum, $(this).text());

                                // The actual data model
                                dataModel.push({
                                    label: $(this).text(),
                                    data: [],
                                    bars: {
                                        order: -colNum,
                                    },
                                    color: colNum,
                                })
                            });
                            table.find('tr').each(function (rowNum) {
                                if (rowNum == 0) { return; } // Skip the first row (as this has headers only)
                                var key = $(this).find('th').text();
                                $(this).find('td').each(function (colNum) {
                                    var value = parseFloat($(this).text());
                                    dataModel[colNum].data.push([value, counter]);
                                });
                                tickModel.push([counter, key]);
                                ++counter;
                            });
                        }
                        else { // Single bars
                            table.find('tr').each(function (rowNum) {
                                var key = $(this).find('th').text();
                                var value = parseFloat($(this).find('td').text());
                                dataModel.push({
                                    label: key,
                                    data: [[value, counter]],
                                });
                                tickModel.push([counter, key]);
                                ++counter;
                            })
                        }

                        // Define bar options
                        $.extend(options, {
                            series: {
                                bars: {
                                    show: true,
                                    barWidth: 0.5 / numColumns,
                                    align: "center",
                                    horizontal: true,
                                    fill: true,
                                    fillColor: { colors: [{ opacity: 1 }, { opacity: 1 }, { opacity: 0 }] }
                                }
                            },
                            legend: {
                                show: (numColumns > 1) ? true : false,
                            },
                            yaxis: {
                                ticks: tickModel
                            },
                            //colors: graphColourPalette,
                        });

                        // Initialise the graph
                        $.plot(graph, dataModel, options);

                        tipFormatter = function (tip, item) {
                            tip.text(item.series.label + ': ' + item.series.data[item.dataIndex][0].toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
                        };
                        postDataCreator = function (item) {
                            return JSON.stringify({
                                label: item.series.label,
                                dataPoint: item.datapoint[0],
                                graphType: graphType,
                            })
                        }
                    }
                        // LINES --------------------------------------------------------------------
                    else if (graphType == 'line') {
                        // Generate the data
                        var counter = 0;
                        var numColumns = table.find('tr').last().find('td').length; // This is the length of one row only, but since the tables are all rectangular, this should be okay.
                        if (numColumns == 1) {
                            dataModel.push({
                                label: 'Chart',
                                data: [],
                            });
                        }
                        else {
                            choiceContainer.appendTo(graph.parent());
                            table.find('tr').first().find('th').each(function (colNum) {
                                if (colNum == 0) { return; } // Skip the first empty cell
                                createCheckboxFor(colNum, $(this).text());

                                // The actual data series objects
                                dataModel.push({
                                    label: $(this).text(),
                                    data: [],
                                    color: colNum,
                                });
                            });
                        }

                        // Populate the dataModel with data
                        table.find('tr').each(function (rowNum) {
                            if (rowNum == 0 && numColumns > 1) { return; } // Skip the header line
                            var xval = parseFloat($(this).find('th').text());
                            $(this).find('td').each(function (colNum) {
                                var yval = parseFloat($(this).text());
                                dataModel[colNum].data.push([xval, yval]);
                            });
                        });

                        // Define line options
                        $.extend(options, {
                            series: {
                                lines: {
                                    show: true,
                                    fill: (numColumns > 1) ? false : true,
                                    fillColor: { colors: [{ opacity: 0 }, { opacity: 1 }] }
                                },
                                points: {
                                    show: true,
                                },
                            },
                            legend: {
                                show: (numColumns > 1) ? true : false,
                            },
                            //colors: graphColourPalette,
                        });

                        // Initialise the graph
                        $.plot(graph, dataModel, options);

                        tipFormatter = function (tip, item) {
                            tip.text((numColumns > 1 ? item.series.label + ': ' : '')
                                + item.series.data[item.dataIndex][0].toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                                + '  ,  ' + item.series.data[item.dataIndex][1]).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                        };
                        postDataCreator = function (item) {
                            return JSON.stringify({
                                label: item.series.label,
                                dataPoint: item.datapoint,
                                graphType: graphType,
                            })
                        }
                    }

                    // Initialise the hover text
                    var tip = graph.siblings('.zeus-graph-tooltip');
                    var hovered = false;
                    graph.on("plothover.flotgraphs", function (event, pos, item) {
                        if (item) {
                            tipFormatter(tip, item);
                            tip.css("border-color", item.series.color);
                            tip.css("display", "block");
                            tip.css("top", pos.pageY - window.pageYOffset);
                            if (tip.width() + 20 > $(document).width() - pos.pageX) { // Check to see if the tip will go off the screen
                                tip.css("left", pos.pageX - window.pageXOffset - 20 - tip.width());
                            }
                            else {
                                tip.css("left", pos.pageX - window.pageXOffset + 20);
                            }
                            hovered = true;
                        }
                    });
                    graph.on("mousemove.flotgraphs", function (event) {
                        if (!hovered) {
                            tip.css("display", "none");
                        }
                        hovered = false;
                    });
                    graph.on("mouseout.flotgraphs", function (event) {
                        tip.css("display", "none");
                    });

                    // Initialise "Drill down" capability
                    graph.on("plotclick.flotgraphs", function (event, pos, item) {
                        var contentContainer = graph.closest('.tab-content').parent();
                        var drillDownUri = contentContainer.find('[' + fullDataTypes.GraphDrillDownUrl + ']').attr(fullDataTypes.GraphDrillDownUrl);
                        if (item && drillDownUri.length > 0) {
                            var panel = contentContainer.closest(".panel");
                            $rhea.getNewContentForPanel(panel, contentContainer, drillDownUri, postDataCreator(item));
                        }
                    });
                });
            });
        },

        widgets: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);

            var routeData = $('#zeus-ajax-routes').data(dataTypes.AjaxRoutes);
            
            // Layout updating functions
            function saveLayout(container) {
                // Update user preferences silently in the background
                var widgets = container.find(".panel-title");
                var widgetNames = widgets.map(function (index, element) {
                    return $(element).text();
                });
                var newLayout = widgetNames.toArray().join();
                $.ajax({
                    url: routeData.SetWidgetLayout, //'/Ajax/SetWidgetLayout',
                    global: false,
                    type: 'POST',
                    data: JSON.stringify({
                        widgetLayout: newLayout,
                        widgetContext: container.attr(fullDataTypes.WidgetContext),
                    }),
                    contentType: 'application/json; charset=utf-8',
                });
            }

            // Adding new widgets event handler
            function addNewWidget(event) {
                var button = $(this);

                var ajaxOptions = {
                    url: routeData.AddWidget, //'/Ajax/AddWidget',
                    type: 'POST',
                    data: JSON.stringify({
                        widgetName: button.find('td').last().text(),
                        widgetContext: button.attr(fullDataTypes.WidgetContext),
                    }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        var container = root.find('.zeus-widget-container').first();
                        if (container.length == 0) container = $('.zeus-widget-container').first();
                        var newContent = $(data);
                        container.append(newContent);
                        $rhea.prepareNewContent(newContent);
                        button.parent().remove(); // Removes the button and the surrounding list item element

                        // Load content for the first time
                        newContent.find('[' + fullDataTypes.Click + '=reload]').trigger("click");
                    },
                };
                $.ajax(ajaxOptions);
            }

            root.find('.zeus-widget-selection-list button').on("click.zeus-widgets", addNewWidget);

            // data context changers
            root.find('.zeus-widget-data-context-list button').on("click.zeus-widgets", function () {
                var ajaxOptions = {
                    url: routeData.SetWidgetDataContext, //'/Ajax/SetWidgetDataContext',
                    type: 'POST',
                    data: JSON.stringify({
                        dataContext: $(this).attr(fullDataTypes.WidgetDataContext),
                        widgetContext: $(this).attr(fullDataTypes.WidgetContext),
                    }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        // refresh all widgets
                        $('[' + fullDataTypes.Click + '=reload]').trigger("click");
                    },
                };
                $.ajax(ajaxOptions);
            });

            // Add heading button handlers
            var tooltipDefaults = {
                    placement: 'bottom',
                    trigger: 'hover',
                    container: 'body'
            }
            // remove
            root.find('[' + fullDataTypes.Click + '=remove]').on("hover.zeus-widgets", function () {
                $(this).tooltip($.extend({
                    title: 'Remove',
                }, tooltipDefaults));
                $(this).tooltip('show');
            });
            root.find('[' + fullDataTypes.Click + '=remove]').on("click.zeus-widgets", function (e) {
                e.preventDefault();
                $(this).tooltip('destroy');
                var panel = $(this).closest('.panel');
                var name = panel.find(".panel-title").text();

                // Remove widget from user preferences silently in the background
                var container = panel.closest('.zeus-widget-container');
                var widgetContext = container.attr(fullDataTypes.WidgetContext);
                panel.parent().remove(); // Removes the surrounding div that has the col numbers specification
                saveLayout(container);


                // Add back the button to the selection list if there is one.
                // It'd be nice to have this HTML defined only in one place, rather than here AND in the WidgetSelectorViewModel.
                var button = $('<button ' + fullDataTypes.WidgetContext + '="' + widgetContext + '"><table><tbody><tr><td><i class="fa fa-plus-circle fa-2x"></i></td><td>' + name + '</td></tr></tbody></table></button>');
                var listItem = $('<li>').append(button);
                var selectionList = $('.zeus-widget-selection-list');
                if (selectionList.length == 1) {
                    var notFound = true;
                    var children = selectionList.children();
                    for (var index = 0; index < children.length; ++index) {
                        var testee = $(children[index]);
                        if (testee.find('td').last().text() > name) {
                            testee.before(listItem);
                            notFound = false;
                            break;
                        }
                    }
                    if (notFound) {
                        selectionList.append(listItem);
                    }
                }
                button.on("click.zeus-widgets", addNewWidget);
            });

            // collapse
            root.find('[' + fullDataTypes.Click + '=collapse]').on("hover.zeus-widgets", function () {
                $(this).tooltip($.extend({
                    title: 'Collapse / Expand',
                }, tooltipDefaults));
                $(this).tooltip('show');
            });
            root.find('[' + fullDataTypes.Click + '=collapse]').on("click.zeus-widgets", function (e) {
                e.preventDefault();
                $(this).closest('.panel').find('.panel-body').slideToggle();
            });

            // reload
            root.find('[' + fullDataTypes.Click + '=reload]').on("hover.zeus-widgets", function () {
                $(this).tooltip($.extend({
                    title: 'Reload',
                }, tooltipDefaults));
                $(this).tooltip('show');
            });
            root.find('[' + fullDataTypes.Click + '=reload]').on("click.zeus-widgets", function (e) {
                e.preventDefault();
                var target = $(this).closest('.panel');
                var targetBody = $(target).find('.panel-body');
                $rhea.getNewContentForPanel(target, targetBody, targetBody.attr(fullDataTypes.Url), JSON.stringify({ widgetContext: targetBody.attr(fullDataTypes.WidgetContext) }));
            });

            // expand
            root.find('[' + fullDataTypes.Click + '=expand]').on("hover.zeus-widgets", function () {
                $(this).tooltip($.extend({
                    title: 'Expand / Compress',
                }, tooltipDefaults));
                $(this).tooltip('show');
            });
            root.find('[' + fullDataTypes.Click + '=expand]').on("click.zeus-widgets", function (e) {
                e.preventDefault();
                var target = $(this).closest('.panel');

                if ($('body').hasClass('panel-expand') && $(target).hasClass('panel-expand')) {
                    $('body, .panel').removeClass('panel-expand');
                    $('.panel').removeAttr('style');
                } else {
                    $('body').addClass('panel-expand');
                    $(this).closest('.panel').addClass('panel-expand');
                }
                $(window).trigger('resize');
            });

            // move up
            root.find('[' + fullDataTypes.Click + '=moveup]').on("hover.zeus-widgets", function () {
                $(this).tooltip($.extend({
                    title: 'Move up',
                }, tooltipDefaults));
                $(this).tooltip('show');
            });
            root.find('[' + fullDataTypes.Click + '=moveup]').on("click.zeus-widgets", function (e) {
                e.preventDefault();
                var me = $(this).closest('.panel').parent();
                var them = me.prev();
                if (them.length == 1) {
                    them.before(me);
                    saveLayout(me.closest('.zeus-widget-container'));
                    $(this).focus();
                }
            });

            // move down
            root.find('[' + fullDataTypes.Click + '=movedown]').on("hover.zeus-widgets", function () {
                $(this).tooltip($.extend({
                    title: 'Move down',
                }, tooltipDefaults));
                $(this).tooltip('show');
            });
            root.find('[' + fullDataTypes.Click + '=movedown]').on("click.zeus-widgets", function (e) {
                e.preventDefault();
                var me = $(this).closest('.panel').parent();
                var them = me.next();
                if (them.length == 1) {
                    them.after(me);
                    saveLayout(me.closest('.zeus-widget-container'));
                    $(this).focus();
                }
            });

            // Dragging
            root.find('.zeus-widget-container').sortable({
                update: function (event, ui) {
                    // Update user preferences silently in the background
                    saveLayout(ui.item.closest('.zeus-widget-container'));
                },
                start: function (event, ui) {
                    ui.placeholder.height(ui.item.height()); // Fix placeholder size bug
                }
            });
        },


        processCalendar: function () {

            // Global variables
            var $rhea = this;
            var root = $(this.element) || $(document);

            var topButtonsString = { left: 'today prev, next ', center: 'title', right: 'month,agendaWeek,agendaDay' };
            var date = new Date(); 

            var AgendaDayView = 'agendaDay';
            var AgendaWeekView = 'agendaWeek';
            var MonthView = 'month';
            var DefaultSessionTime = 2; // value in hours i.e. 2 hours.
            var StartTimePropertyName = "'Start.Time'";
            var StartDatePropertyName = "'Start.Date'";
            var EndTimePropertyName = "'End.Time'";
            var EndDatePropertyName = "'End.Date'";
            var StartPropertyName = "'Start'"; // hidden property
            var EndPropertyName = "'End'";      // hidden property
            var ModelStateIsValidPropertyName = "'ModelStateIsValid'";
            var javascriptUrl = "javascript:;";
            var SubmitTypeVariableName = 'submitType';
            var dialogDiv = $("#calendarCategory-dialog-form");
            var startTime, endTime;
            var currentAction;
            var submitTypeChosen = undefined;


            // Bind esc event on modal
            $(document).keyup(function (e) {
                if (e.keyCode == 27) // ESC key
                {
                    closeModal();
                }
            });

            // This function binds to click and enter events on buttons inside of Modal and get their submitType.
            function BindButtonsForSubmitType(mform)
            {

                $(mform).find("button[type='submit']").each(function () {
                    var button = $(this);
                    var buttonValue = $(button).attr('value'); 
                    if(buttonValue != undefined && buttonValue != "")
                    {
                        if(buttonValue.toLowerCase() != "close")
                        {
                            $(button).click(function (e) {

                                submitTypeChosen = buttonValue;

                            });
                        }
                    }
                });

            }


            // This method will be called when user has clicked existing event with the intention of editing that event.
            function editEvent(url) {

                //TODO: This is where you will load a Modal with view-model properties.            

                if (url != undefined) {
                    processAjaxGet(url);
                }
                else {
                    alert('url is undefined in editEvent(). url = ');
                }

            }

                    
            // This method is responsible for processing AJAX GET call to specified url.
            function processAjaxGet(url) {
                if (url != undefined && url != '') {
                    // Make an AJAX call here.                  
                    $.ajax({
                        url: url,
                        quietMillis: 1000,
                        type: 'GET',
                        dataType: 'html',
                        cache: false,
                        error: function (e) {
                            alert('error occurred in ajax call. ' + e);
                        },
                        contentType: 'application/xml; charset=utf-8'

                    }).done(function (response) {

                        // When a title is present it means the user session has expired and they've been presented with the STS login form
                        if ($.zeusValidate.extractTitle(response) !== false) {
                            // So exit early and allow the global ajaxComplete to handle this
                            return;
                        }

                        bindPostSubmit(response, url);

                    });
                }
                else {
                    $.zeusValidate.addError('Encountered error: Url is undefined or empty processAjaxGet().');
                }
            }

            // This function is responsible for updating the post Token and form's Id.
            function updateFormIdGetActionWithNewToken(response, url) {

                var tokenPrefix = "__RequestVerificationToken=";
                var postAction = undefined;
                if (response != undefined) {
                    // Change the id for this form.
                    response = $.zeusValidate.replaceAll(response, 'id="main_form"', 'id="modal_form"');
                    response = $('<div>').html(response); // you'll want to wrap this inside of <form> tag as changes to Object.cshtml will no longer return form.
                    //alert('Data received from post action is  ' + response);
                    // Update request verification token.                      
                    var tokenElement = $(response).find('input[name="__RequestVerificationToken"]');//[0].Value;
                    var tokenForPost = $(tokenElement).attr('value');
                    currentAction = url;
                    postAction = url + '?' + tokenPrefix + tokenForPost;

                }
                return { postAction: postAction, response: response };
            }

            // This method will be used to process Post / submits for all AJAX POST actions.
            function bindPostSubmit(response, url) {

                if (response != undefined && response != '') {

                    // Change form id element.
                    var result = updateFormIdGetActionWithNewToken(response, url);
                    var actionForPost = result.postAction;

                    // Get Modal.
                    var modalBody = $("#modal-message .modal-body");

                    var hideModal = false;
                    var input = $(result.response).find("input[name=" + ModelStateIsValidPropertyName + "]"); //
                    if (input != undefined) {
                        var modelValidity = $(input).attr('value'); //
                        //alert(modelValidity.toLowerCase());

                        if (modelValidity != undefined && modelValidity.toLowerCase().indexOf('true') >= 0) {

                            // TODO: If this is set to true, that means update / or add has been successful, so we can redirect user to Calendar so this newly added event will be populated.
                            hideModal = true;

                            $.zeusValidate.ignoreDirty = true;
                            location.reload(true); // parameter value true: indicates that full page -reload (from server) will be done.

                            // TODO:  ADD event straight to calendar and close the modal here....
                        }
                    }
                    if (!hideModal) {

                        $(modalBody).empty();

                        $(modalBody).append(result.response);

                        var mform = $('#modal_form');

                        BindButtonsForSubmitType(mform);

                        // Update both Start and End Sessions here (including Date and Time as well as Hidden fields),
                        // do this only for ADD and not for EDIT.
                        var st = $(mform.find("input[name=" + StartPropertyName + "]")[0]);
                        if (st != undefined && (st.val() == '' || st.val() == undefined)) {//|| st.val() == undefined )

                            var dateTimeString = startTime.toString('d/MM/yyyy h:mm tt');
                            dateTimeString = $.zeusValidate.replaceAll(dateTimeString, "0:00 AM", "12:00 AM");

                            $(mform.find("input[name=" + StartPropertyName + "]")[0]).val();
                            // Val() doesn't change the value attribute, what a surprise !!!, so have to set value via attr.
                            $(mform.find("input[name=" + StartPropertyName + "]")[0]).attr('value', dateTimeString);
                        }
                        var st1 = $(mform.find("input[name=" + StartDatePropertyName + "]")[0]);
                        if (st1 != undefined && (st1.val() == '' || st1.val() == undefined)) {
                            var dayString = startTime.toString('d/MM/yyyy');
                            $(mform.find("input[name=" + StartDatePropertyName + "]")[0]).val();
                            $(mform.find("input[name=" + StartDatePropertyName + "]")[0]).attr('value', dayString);
                        }
                        var st2 = $(mform.find("input[name=" + StartTimePropertyName + "]")[0]);
                        if (st2 != undefined && (st2.val() == '' || st2.val() == undefined)) {
                            var timeString = startTime.toString('h:mm tt');
                            $(mform.find("input[name=" + StartTimePropertyName + "]")[0]).val(timeString);
                            $(mform.find("input[name=" + StartTimePropertyName + "]")[0]).attr('value', timeString);
                        }
                        var et = $(mform.find("input[name=" + EndPropertyName + "]")[0]);
                        if (et != undefined && (et.val() == '' || et.val() == undefined)) {//|| st.val() == undefined )
                            var dateTimeString = endTime.toString('d/MM/yyyy h:mm tt');
                            dateTimeString = $.zeusValidate.replaceAll(dateTimeString, "0:00 AM", "12:00 AM");
                            $(mform.find("input[name=" + EndPropertyName + "]")[0]).val(dateTimeString);
                            $(mform.find("input[name=" + EndPropertyName + "]")[0]).attr('value', dateTimeString);
                        }
                        var et1 = $(mform.find("input[name=" + EndDatePropertyName + "]")[0]);
                        if (et1 != undefined && (et1.val() == '' || et1.val() == undefined)) {
                            var dayString = endTime.toString('d/MM/yyyy');
                            $(mform.find("input[name=" + EndDatePropertyName + "]")[0]).val(dayString);
                            $(mform.find("input[name=" + EndDatePropertyName + "]")[0]).attr('value', dayString);
                        }
                        var et2 = $(mform.find("input[name=" + EndTimePropertyName + "]")[0]);
                        if (et2 != undefined && (et2.val() == '' || et2.val() == undefined)) {
                            var timeString = endTime.toString('h:mm tt');
                            $(mform.find("input[name=" + EndTimePropertyName + "]")[0]).val(timeString);
                            // Val() doesn't change the value attribute, what a surprise !!!, so have to set value via attr.
                            $(mform.find("input[name=" + EndTimePropertyName + "]")[0]).attr('value', timeString);

                        }

                        var modalOpen = $("#modal-message").modal();

                        // Bind click event of close button so modal is closed.
                        $(modalBody).find('button[class ~= "cancel"]').first().each(function () { // button[class="cancel btn-inverse btn"]

                            $(this).click(function (e) {
                                e.preventDefault();
                                closeModal();
                            });
                            //return false;
                        });



                        $rhea.prepareNewContent(mform);

                        mform.bind('submit', function (e) {
                            e.preventDefault();                            

                            var isValid = $(mform).valid();

                            if (isValid !== undefined && isValid == true) {

                                $.zeusValidate.ignoreDirty = true;

                                var serializedForm = mform.serialize() + '&submitType=' + submitTypeChosen +'&';
                                var formModel = JSON.stringify({ 'model': serializedForm });
                                if (submitTypeChosen != undefined) {
                                    //alert('submit type chosen' + submitTypeChosen);
                                    formModel = JSON.stringify({ 'model': serializedForm});
                                }
                                //.toString('d/MM/yyyy g:i A')
                                $.ajax({
                                    url: actionForPost,
                                    quietMillis: 1000,
                                    type: 'POST',
                                    dataType: 'html',
                                    data: formModel 

                                }).done(function (data) {
                                    if (data == undefined || data == '') { 
                                        // Some error occurred
                                        $.zeusValidate.ignoreDirty = true;
                                        location.reload(true); // parameter value true: indicates that full page -reload (from server) will be done.
                                    }
                                    else {
                                        bindPostSubmit(data, url);
                                    }
                                });

                            }
                            else {
                                //alert("Form is invalid " + isValid);
                            }
                            // After successful form submission, control comes here. But we are doing page reload so ...
                        });
                    }
                }
            }

            // Closes the Modal.
            function closeModal() {
                $("#modal-message").modal('hide');
                $(dialogDiv).modal('hide');
            }

            // This function carries out AJAX call to action that is responsible for responding to Resizing and dragging of event.
            function processEventDragResize(url, start, end,  revertFunc)
            {
                 
                if (url != undefined) {                    
                    var startDateChange = start != null ? start.toString('d/MM/yyyy h:mm tt') : null;  
                    var endDateChange = end != null ? end.toString('d/MM/yyyy h:mm tt') : start.toString('d/MM/yyyy'); // if end is null i.e. it's an all day event.
                    //alert(startDateChange + "" + endDateChange  + " ");
                    //var dataToSend = JSON.stringify({ start: startDateChange , end: endDateChange });//!= null ? startDateChange.toString() : null      != null ? endDateChange.toString(): null
                    $.ajax({

                        url: url,
                        quietMillis: 1000,
                        type: 'POST',
                        dataType: 'html',
                        data: { start: startDateChange, end: endDateChange },
                        error: function (error) {                            
                            $.zeusValidate.addError('The server encountered an internal error and was unable to process your request. Please try again later.');                            
                        }
                        
                    }).done(function (data) {                        
                        // When a title is present it means the user session has expired and they've been presented with the STS login form
                        if ($.zeusValidate.extractTitle(data) !== false) {
                            // So exit early and allow the global ajaxComplete to handle this
                            return;
                        }
                        else if (data == undefined || data == '') {
                            $.zeusValidate.addError('The server encountered an internal error and was unable to process your request. Please try again later. ' + 'No data received.');
                        }
                        else if (data.toLowerCase() == "true") {   
                            return;
                        }   

                        revertFunc(); // Revert the move if anything goes wrong or if the response is false.
                    });

                } 
            }

                   
                    
            $(".vertical-box").each(function () {

                var dialog, selectCategoryform;
                // store this as parentCalendar
                var currentCalendar = $(this);
                        
                // check if the current element has not been rendered as calendar already.
                var isCalendarRenderedBefore = $(currentCalendar).attr(fullDataTypes.CalendarRendered);
                if (isCalendarRenderedBefore != undefined && isCalendarRenderedBefore == "true") {
                    // if it has been rendred then return
                    return;
                }
                $(currentCalendar).attr(fullDataTypes.CalendarRendered, "true");
                        

                //var removeDropCheckbox = $(currentCalendar).children('#drop-remove'); // no longer required.
                        
                // get list of categories for this calendar
                var categoryList = $(currentCalendar).find(".external-event");
                var categoriesArray = [];
                var categoriesArrayTrimmed = [];
                if (categoryList != undefined) {
                    $(categoryList).each(function () {

                        var category = $(this).attr('data-title');
                        categoriesArray.push(category);
                        categoriesArrayTrimmed.push(category.replace(/\s+/g, ''))
                    });
                }

                // Get Default View value.
                var defaultView = $(currentCalendar).attr(fullDataTypes.CalendarDefaultView);

                switch (defaultView) {
                    case 'day':
                        defaultView = AgendaDayView;
                        break;
                    case 'month':
                        defaultView = MonthView;
                        break;
                    case 'week':
                        defaultView = AgendaWeekView;
                        break;
                    default:
                        defaultView = MonthView;
                        break;
                }


                // Populates the array of categories which will be used in a Dialog box for adding a new event.
                function populateCategoriesList(dialogContainer) {

                    // populate categories.  
                    var selectMenu = $(dialogContainer).find('.radio-button-list');

                    $(selectMenu).empty();
                    $(categoriesArray).each(function (index) {

                        var option = categoriesArray[index];
                               
                        var trimmedOption = option.replace(/\s+/g, ''); // all whitespaces and /g indicates global flag.
                        var selectedTag = '';
                        if (index == 0) {
                            selectedTag = 'checked';
                        }
                        var radio = '<input id=\'rd-' + trimmedOption + '\' type=\'radio\' name=\'group1\' ' + selectedTag + '  value=\'' + trimmedOption + '\'> <label for=\'rd-' + trimmedOption + '\'>' + option + '</label> <br>';
                        
                        $(selectMenu).append(radio);

                        
                    });
                    //$(selectMenu).selectmenu();
                }

                // This method is responsible for handling click on '+' (plus) button or when user clicks anywhere on the calendar with intention of adding new event. 
                //List of categories will be shown in Modal for user to add event for that category.
                function HandleEventAddClick()
                {
                    populateCategoriesList(dialogDiv);
                    dialog = $(dialogDiv).modal();
                    // handle submit
                    selectCategoryform = dialog.find('form').on('submit', function (event) {
                        event.preventDefault();
                        //alert('Form submitting ' + selectCategoryform[0]);
                        addNewEvent();
                    });
                }


                // This method will be called when user has clicked anywhere on calendar with the intention of adding new event.
                function addNewEvent() {
                            
                    // Opens a dialog box that allows user to select the category for the new event.

                    var categorySelected = $(selectCategoryform).find('input[type]:checked').val();

                    if ($.inArray(categorySelected, categoriesArrayTrimmed) != -1) {


                        //TODO: This is where you will load a Modal with view-model properties.
                        $(dialogDiv).modal('hide');

                        var attribute = 'div[' + fullDataTypes.CalendarCategory + '="' + categorySelected + '"]';

                        var url = undefined;
                        $(categoryList).each(function () {
                            var attributeValue = $(this).attr(fullDataTypes.CalendarCategory);
                            if (attributeValue == categorySelected) {
                                url = $(this).attr(fullDataTypes.Url);
                                return false;
                            }
                        });

                        processAjaxGet(url);

                    }
                    else {
                        (dialog).dialog("close");

                    }
                }               


                var calendarDiv = $(currentCalendar).find("#calendar"); // TODO: change this from ID to className.

                var calendar = $(calendarDiv).fullCalendar({

                    header: topButtonsString,
                    selectable: true,
                    selectHelper: true,
                    droppable: true,
                    defaultView: defaultView,
                    dayClick: function (date, jsEvent, view) {
                        var allDay = false;
                        allDay = date.toString().indexOf('00:00:00') > -1;
                        var dateCopy = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), 0, 0);
                        startTime = date;
                        endTime = dateCopy;
                        if (!allDay) {
                            endTime.setHours(endTime.getHours() + DefaultSessionTime); // the default end time will be 2 hours.
                        }

                        HandleEventAddClick();
                            },

                    drop: function (date, jsEvent, ui)
                    {
                        var categoryDiv = $(this);
                        var categoryUrl = $(categoryDiv).attr(fullDataTypes.Url);
                        var allDay = false;                        
                        allDay = date.toString().indexOf('00:00:00') > -1;
                        var dateCopy = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), 0, 0);
                        startTime = date;
                        endTime = dateCopy;                                               
                        if (!allDay)
                        { 
                            endTime.setHours(endTime.getHours() + DefaultSessionTime); // the default end time will be 2 hours. 
                            alert(startTime.toString() + ',' + endTime.toString());
                        }

                        processAjaxGet(categoryUrl);
                    },

                    eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc, jsEvent, ui, view)
                    {
                        //alert(event.start + "" + event.end + " " + view); 
                        var eventInContext = $(this);
                        var url = eventInContext.attr(fullDataTypes.CalendarDragResizeAction);

                        if (confirm("Are you sure about this change?")) {                            

                            processEventDragResize(url, event.start, event.end, revertFunc); // response from AJAX call. 
                        }
                        else {
                            revertFunc();
                        }

                    },
                        
                    eventResize: function (event, dayDelta, minuteDelta, revertFunc, jsEvent, ui, view) {
                        var eventInContext = $(this);

                        // Disable re-sizing in 'Month' view
                        if (view.name.indexOf(MonthView) == 0) {
                            revertFunc();
                        }
                        else if (confirm("Are you sure about this change?")) {
                            
                            var url = eventInContext.attr(fullDataTypes.CalendarDragResizeAction);
                            processEventDragResize(url, event.start, event.end, revertFunc); // response from AJAX call. 
                        }
                        else {
                            revertFunc();
                        }

                    },

                    eventClick: function (event, jsEvent, view) {

                        jsEvent.preventDefault();// prevent navigating to different page.

                        var eventAnchor = $(this);
                        var eventUrl = $(eventAnchor).attr(fullDataTypes.Url);
                        
                        editEvent(eventUrl);
                    },

                    select: function(start, end, allDay)
                    {
                        // at this stage, get the view model properties and feed them into the Modal.

                       
                        // check if the event does not exist at the location,
                        //      if it exists then 
                        //          Make ajax call to view
                        //          Populate form fields to display
                        //          open the model to edit them.
                        //      else
                        //          display list of categories
                        //          allow user to pick one category
                        //          Make ajax call to view
                        //          Get form fields to display
                        //          open the model to edit them.
                        //       

                        
                        /*
                        $.ajax(function () {



                        });
                       
                         
                        /*
                        var title = prompt('Event Title:');
                        if(title)
                        {
                            calendar.fullCalendar('renderEvent', {
                                title: title,
                                start: start,
                                end: end,
                                allDay: allDay
                            },
                            // to make the event "stick"
                            true);
                        }
                        */
                        calendar.fullCalendar('unselect');
                    },

                    eventRender: function(event, element, calEvent)
                    {
                        // Describe the contents of events being rendered.
                        var mediaObject = (event.media) ? event.media : '';
                        var description = (event.description) ? event.description : '';                        
                        element.find(".fc-event-title").after($("<span class=\"fc-event-icons\"><span>").html(mediaObject));                        
                        element.find(".fc-event-title").append('<small>' + description + '</small>');
                         
                    },

                    editable: true, // by default we set all events to editable, but override this property when rendering individual events.

                    events: new function () {

                        // get list of categories for this calendar
                        var eventList = [];

                        if (categoryList != undefined && categoryList.length) {

                            // You will render the existing events here.

                            // Logic:
                            //  Iterate through all the Categories
                            //      URL = Get the url for the category. You should know name of current category. then find the div as follows: $("#")
                            //      Iterate through all the items for current category
                            //          add these items into events[] array here
                            //          get the Id of the current item, and pass that in url.

                            $.each(categoryList, function () {

                                var currentCategory = $(this);
                                var categoryName = $(currentCategory).attr(fullDataTypes.CalendarCategory);
                                var categoryColor = $(currentCategory).attr('data-bg');
                                var categoryMedia = $(currentCategory).attr('data-media');
                                var categoryUrl = $(currentCategory).attr(fullDataTypes.Url);
                                var dragResizeUrl = $(currentCategory).attr(fullDataTypes.CalendarDragResizeAction);
                                var jsonAttributeForCurrentCategory = '[' + fullDataTypes.CalendarCategoryEventList + '=' + categoryName + ']';

                                var jsonContainer = $(calendarDiv).find(jsonAttributeForCurrentCategory);
                                if (jsonContainer != undefined) {

                                    var text = $(jsonContainer).text();
                                    if (text != undefined && text != '') {
                                        var serializedJson = JSON.parse(text);

                                        $.each(serializedJson, function () {

                                            var currentEventData = $(this)[0];

                                            var startSession = currentEventData.Start != undefined ? currentEventData.Start.substr(6) : new Date();
                                            var endSession = currentEventData.End != undefined ? currentEventData.End.substr(6) : new Date();

                                            // URL--> var categoryDiv = $(currentCalendar).children('div[data-title="@category.Type"]'); //where @category.Type is the current category being processed.
                                            var currentEvent =
                                            {
                                                id: currentEventData.Id,
                                                title: currentEventData.Title, // C# properties for CategoryItemViewModel Title, Description, Start and End (both DateTime).
                                                start: new Date(parseInt(startSession)),
                                                end: new Date(parseInt(endSession)),
                                                className: categoryColor,
                                                media: categoryMedia, // CategoryViewModel properties: Color, Icon,
                                                description: currentEventData.EventDescriptionHtml,
                                                url: currentEventData.Id,// javascriptUrl, 
                                                allDay: currentEventData.AllDay,
                                                dataUrl: categoryUrl + '/' + currentEventData.Id,
                                                dragUrl: dragResizeUrl == undefined ? undefined : (dragResizeUrl + '?id=' + currentEventData.Id),
                                                editable: dragResizeUrl != undefined && currentEventData.IsEditable   // if url is specified & isEditable (by default is set to true) then event is draggable and resizable.
                                            }; 
                                            eventList.push(currentEvent);
                                        });
                                        // Remove JSON data from markup.
                                        //$(jsonContainer).empty();
                                    }
                                }
                            });
                        }
                        return eventList;
                    }

                });

                function UpdateIds() {
                    $.each(categoryList, function () {

                        var currentCategory = $(this);
                        var categoryName = $(currentCategory).attr(fullDataTypes.CalendarCategory);
                        var categoryColor = $(currentCategory).attr('data-bg');
                        var categoryMedia = $(currentCategory).attr('data-media');
                        var categoryUrl = $(currentCategory).attr(fullDataTypes.Url);
                        var dragResizeUrl = $(currentCategory).attr(fullDataTypes.CalendarDragResizeAction);
                        var jsonAttributeForCurrentCategory = '[' + fullDataTypes.CalendarCategoryEventList + '=' + categoryName + ']';

                        var jsonContainer = $(calendarDiv).find(jsonAttributeForCurrentCategory);
                        if (jsonContainer != undefined) {

                            var text = $(jsonContainer).text();
                            if (text != undefined && text != '') {
                                var serializedJson = JSON.parse(text);

                                $.each(serializedJson, function () {

                                    var currentEventData = $(this)[0];

                                    $(".fc-event-container a[href='" + currentEventData.Id + "']").each(function () {
                                        var currentEvent = $(this);

                                        $(currentEvent).attr(fullDataTypes.Url, categoryUrl + '/' + currentEventData.Id);
                                        if (dragResizeUrl) {
                                            $(currentEvent).attr(fullDataTypes.CalendarDragResizeAction, dragResizeUrl + '?id=' + currentEventData.Id);
                                        }
                                        if (currentEventData.HoverDescriptionHtml) {
                                            $(currentEvent).html($(currentEvent).html() + ("<div class='event-mouseover'>" + currentEventData.HoverDescriptionHtml + "</div>"));//hoverText goes here....
                                        }

                                        $(currentEvent).mouseover(function (mouseInEvent) {
                                            $(currentEvent).find('.event-mouseover').each(function () {
                                                /*
                                                var tip = $(this);
                                                $(tip).show();
                                                $(tip).css('position', 'relative');
                                                $(tip).css('z-index', 999);
                                                //$(tip).css('top', -50);
                                                if (mouseInEvent.pageX > $(calendarDiv).width()) {
                                                    $(tip).css('left', -120);
                                                } else {
                                                    $(tip).css('left', 120);
                                                }
                                                //$(tip).css('top', mouseInEvent.pageY - window.pageYOffset);//mouseInEvent.pageY - window.pageYOffset                                            
                                                //tip.css("left", mouseInEvent.pageX - window.pageXOffset);
                                                */
                                            });

                                        });
                                        $(currentEvent).mouseout(function (mouseOutEvent) {
                                            $(currentEvent).find('.event-mouseover').each(function () {
                                                $(this).hide();
                                            });

                                        });

                                        return false;

                                    });


                                });
                                // Remove JSON data from markup.
                                $(jsonContainer).empty();
                            }
                        }
                    });
                }
              

                UpdateIds();

                         

                

                // Bind to 'AddEvent' link.
                var selector = 'a[' + fullDataTypes.CalendarEventAddBtn + '=true]';
                $(selector).click(function (e) {
                    startTime = date;
                    endTime = date;
                    HandleEventAddClick();
                });


                // Initialize the external events
                //var externalEvents = $(currentCalendar)
                $('#external-events .external-event').each(function () {

                    var eventObject = {
                        title: $.trim($(this).attr('data-title')),
                        className: $(this).attr('data-bg'),
                        media: $(this).attr('data-media'),
                        description: $(this).attr('data-desc')
                    };

                    $(this).data('eventObject', eventObject);

                    $(this).draggable({
                        zIndex: 999,
                        revert: true,
                        revertDuration: 0
                    });
                });
                
                return false;// So you only apply this on first calendar.

            });

           


            
            // Bind to 'enter event'.
            $('.fc-event-container .fc-event').each(function (eq) {
                var eventAnchor = $(this);
                $(this).bind('keypress', function (event) {
                    if (event.keyCode == 13 || event.which == 13) // bind to 'Enter'.
                    {
                        event.preventDefault();// prevent navigating to different page. 
                        var eventUrl = $(eventAnchor).attr(fullDataTypes.Url);//?? event.url;
                        editEvent(eventUrl);
                    }
                });
            });

            $(window).bind('resize', function () {

                 // TODO: Handle the re-rendering of calendar as it overwrites IDs embedded in anchor tag.

            });
        },

        dateBasedContent: function () {
            var $rhea = this;
            var root = $(this.element) || $(document);
            root.find('.zeus-date-based-content').each(function () {
                var container = $(this);
                var changeTimeout;
                var datePart = container.find('.zeus-date-part');
                var contentPart = container.find('.zeus-content-part');
                var url = datePart.attr(fullDataTypes.Url);
                var skipContentFetch = false;

                // Hook up content URI if there is one
                if (url) {
                    datePart.datepaginator({
                        injectStyle: false,
                        onSelectedDateChanged: function () {
                            var self = $(this);
                            if (skipContentFetch) { // Avoid infinite loops on startup by not firing this event on first setting of the date
                                skipContentFetch = false;
                                return;
                            }
                            window.clearTimeout(changeTimeout);
                            changeTimeout = window.setTimeout(function () {
                                var selectedDate = self.find('.dp-selected').attr('data-moment');
                                $.ajax({
                                    type: 'GET',
                                    dataType: 'html',
                                    data: { selectedDate: selectedDate },
                                    url: url,
                                    success: function (data) {
                                        contentPart.html(data);
                                        $rhea.prepareNewContent(contentPart);
                                    },
                                })
                            }, 500);
                        }
                    });
                }
             
                // Set date if there is one
                var currentDate = datePart.attr(fullDataTypes.DateTime);
                if (currentDate) {
                    skipContentFetch = true;
                    datePart.datepaginator("setSelectedDate", currentDate);
                }

                // Keyboard accessibility focus fixing
                !function fixKeyboard() {
                    datePart.find('a').each(function (index, element) {
                        $(element).on('click', function (event) {
                            window.setTimeout(function () {
                                datePart.find('a').eq(index).focus();
                                fixKeyboard();
                            }, 1);
                        });
                        $(element).on('keypress', function (event) {
                            window.setTimeout(function () {
                                if (event.which == 13 || event.which == 10) {
                                    datePart.find('a').eq(index).focus();
                                    fixKeyboard();
                                }
                            }, 1);
                        });
                    });
                }();

                // Fix width
                var first = datePart.find('a').first();
                var last = datePart.find('a').last();
                first.width(first.width() - 1);
                last.width(last.width() - 1);
             
                // Fix icon
                datePart.find('.glyphicon-calendar').removeClass('glyphicon').removeClass('glyphicon-calendar').addClass('fa').addClass('fa-calendar');
            });
        }
    };

    // Wrapper around constructor to prevent against multiple instantiations
    $.fn[pluginName] = function (options) {
        return this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName)) {
                $.data(this, 'plugin_' + pluginName,
                    new Zeus(this, options));
            }
        });
    };

})(jQuery, window, document);

$(document).ready(function () {
    $(document).zeus();


});


var koGridRowMapping = {
    rows: {
        create: function (options) {
            return koCreateRow(options.data);
        }
    }

};

var koCreateRow = function (row) {
    var result = ko.mapping.fromJS(row);
    result.dirtyFlag = ko.dirtyFlag(result);
    return result;
}
