
;  (function ($, window, document, undefined) {

    var setValidationValues = function (options, ruleName, value) {
        options.rules[ruleName] = value;
        if (options.message) {
            options.messages[ruleName] = options.message;
        }
    };

    var $Unob = $.validator.unobtrusive;

    $Unob.adapters.add("isequalto", ["dependentproperty", "comparisontype", "passonnull", "failonnull"], function (options) {
        setValidationValues(options, "isequalto", {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        });
    });
    
    $Unob.adapters.add("isnotequalto", ["dependentproperty", "comparisontype", "passonnull", "failonnull"], function (options) {
        setValidationValues(options, "isnotequalto", {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        });
    });
    
    $Unob.adapters.add("isgreaterthan", ["dependentproperty", "comparisontype", "passonnull", "failonnull"], function (options) {
        setValidationValues(options, "isgreaterthan", {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        });
    });
    
    $Unob.adapters.add("isgreaterthanorequalto", ["dependentproperty", "comparisontype", "passonnull", "failonnull"], function (options) {
        setValidationValues(options, "isgreaterthanorequalto", {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        });
    });
    
    $Unob.adapters.add("islessthan", ["dependentproperty", "comparisontype", "passonnull", "failonnull"], function (options) {
        setValidationValues(options, "islessthan", {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        });
    });
    
    $Unob.adapters.add("islessthanorequalto", ["dependentproperty", "comparisontype", "passonnull", "failonnull"], function (options) {
        setValidationValues(options, "islessthanorequalto", {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        });
    });
    
    $Unob.adapters.add("isregexmatch", ["dependentproperty", "comparisontype", "passonnull", "failonnull"], function (options) {
        setValidationValues(options, "isregexmatch", {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        });
    });
    
    $Unob.adapters.add("isnotregexmatch", ["dependentproperty", "comparisontype", "passonnull", "failonnull"], function (options) {
        setValidationValues(options, "isnotregexmatch", {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        });
    });
    
    
    $Unob.adapters.add("requiredif", ["dependentproperty", "comparisontype", "value"], function (options) {
        var value = {
            dependentproperty: options.params.dependentproperty,
            comparisontype: options.params.comparisontype,
            value: options.params.value,
            passonnull: options.params.passonnull,
            failonnull: options.params.failonnull
        };
        setValidationValues(options, "requiredif", value);
    });

    // Override "required" so checkbox elements are mandatory if their property was decorated with the [Required] attribute
    $Unob.adapters.add("required", function (options) {
        setValidationValues(options, "required", true);
    });
    
    $Unob.adapters.addBool("crn");
    
    $Unob.adapters.addBool("abn");
    
    $Unob.adapters.addBool("currency");
    
})(jQuery, window, document);


/* Override of jQuery unobtrusive validation functions 'onErrors' and 'onError' */
$(function() {
    $('form').each(function() {

        var getDisplayName = function(element) {
            var displayName = $(element).data($.zeusDataTypes.DisplayName);
                    
            if (displayName == undefined) {
                displayName = $(element)[0].name;
            }

            return displayName;
        };
        
        // Copy of jquery function used by 'onError' (cannot access from outside)
        var escapeAttributeValue = function(value) {
            return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
        };

        // remove old handler for 'onErrors'
        $(this).unbind("invalid-form.validate");

        // set new handler for 'onErrors'
        $(this).bind("invalid-form.validate", function(event, validator) { // 'this' is the form element
            var container = $(this).find("section#validation-error-summary"),//[data-valmsg-summary=true]
                list = container.find("ul");

            if (list && list.length && validator.errorList.length) {
                list.empty();
                //alert('invalid-form.validate unobstrusive')
                container.addClass("validation-summary-errors");
                container.addClass("alert");
                container.addClass("alert-danger");
                container.removeClass("validation-summary-valid")
                container.removeClass("noErrors");
                var errorsToAdd = [];
                $.each(validator.errorList, function () {

                    var elementID = $(this.element)[0].id;
                    var li = undefined;



                    var dateTimePicker = $('#' + elementID + '_Date').length && $('#' + elementID + '_Time').length;
                    var s2element = $('#s2id_focus_' + elementID);
                    var select2element = $('#s2id_focus_select2_' + elementID);

                    if (dateTimePicker) {
                        elementID = elementID + '_Date';
                    }

                    if (s2element != undefined && s2element.length > 0) {
                        li = $("<li />").append('<a class=\'alert-link\' href="javascript:;" onclick="$(\'#' + elementID + '\').select2(\'focus\');">' + getDisplayName(this.element) + '</a> - ' + this.message);
                    } else if (select2element != undefined && select2element.length > 0) {
                        li = $("<li />").append('<a class=\'alert-link\' href="javascript:;" onclick="$(\'#select2_' + elementID + '\').select2(\'focus\');">' + getDisplayName(this.element) + '</a> - ' + this.message);
                    } else {
                        li = $("<li />").append('<a class=\'alert-link\' href="#' + elementID + '">' + getDisplayName(this.element) + '</a> - ' + this.message);
                    }

                    var errorToAdd = getDisplayName(this.element) + ' - ' + this.message;

                    // Don't add duplicate errors
                    if (li != undefined && $.inArray(errorToAdd, errorsToAdd) == -1) {
                        li.appendTo(list);

                        errorsToAdd.push(errorToAdd);
                    }
                });

            }
            // find all panels, iterate through them 
            //      then iterate through errors, 
            //          get closes section of the current error
            //          if panelId == closestSectionId then
            //              and add these errors to the panel.

            var panelContainers = $(".panel-body section[data-valmsg-summary=true]");//[data-valmsg-summary=true]
            $.each(panelContainers, function () {
                var panelContainer = $(this);
                var panelHasErrors = false;
                var panelList = panelContainer.find("ul");

                if (panelList && panelList.length && validator.errorList.length) {

                    panelList.empty();
                    //alert('invalid-form.validate unobstrusive')

                    var errorsToAddPanel = [];

                    $.each(validator.errorList, function () {
                    
                    var elementID = $(this.element)[0].id;
                    var li = undefined;
                    var duplicateLi = undefined;

                    // process individual element errors inside this panel.
                        var closestContainer = $('#' + elementID).closest('.form-horizontal').siblings('[data-valmsg-summary=true]');//
                   
                        if (closestContainer.length && closestContainer[0].id == panelContainer[0].id) {

                            panelHasErrors = true;

                    var dateTimePicker = $('#' + elementID + '_Date').length && $('#' + elementID + '_Time').length;
                    var s2element = $('#s2id_focus_' + elementID);
                    var select2element = $('#s2id_focus_select2_' + elementID);

                    if (dateTimePicker) {
                        elementID = elementID + '_Date';
                    }
                    
                    if (s2element != undefined && s2element.length > 0) {
                        li = $("<li />").append('<a class=\'alert-link\' href="javascript:;" onclick="$(\'#' + elementID + '\').select2(\'focus\');">' + getDisplayName(this.element) + '</a> - ' + this.message);
                    } else if (select2element != undefined && select2element.length > 0) {
                        li = $("<li />").append('<a class=\'alert-link\' href="javascript:;" onclick="$(\'#select2_' + elementID + '\').select2(\'focus\');">' + getDisplayName(this.element) + '</a> - ' + this.message);
                    } else {
                        li = $("<li />").append('<a class=\'alert-link\' href="#' + elementID + '">' + getDisplayName(this.element) + '</a> - ' + this.message);
                    }

                    var errorToAdd = getDisplayName(this.element) + ' - ' + this.message;
                    
                    // Don't add duplicate errors
                            if (li != undefined && $.inArray(errorToAdd, errorsToAddPanel) == -1) {
                                li.appendTo(panelList);
                                errorsToAddPanel.push(errorToAdd);
                            }
                        }
                    });

                    if (panelHasErrors) {
                        panelContainer.addClass("validation-summary-errors");
                        panelContainer.addClass("alert");
                        panelContainer.addClass("alert-danger");
                        panelContainer.removeClass("validation-summary-valid")
                        panelContainer.removeClass("noErrors");
                    }



                         
                        
                    }
                });
            //**********************************************

                
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
        });

        var validator = $.data($(this)[0], 'validator');
        var settings = validator.settings;

        // Whether to focus the first invalid property by default
        settings.focusInvalid = false;
        
        settings.ignore = function() {
            var content = $('#content');
            
            if (content != undefined) {
                var skip = (/^true$/i.test(content.data($.zeusDataTypes.SkipValidation))) ? true : false;
                
                if (skip) {
                    return true;
                }
            }

            var disabled = $(this).is(':disabled');
            var hidden = $(this).is(':hidden');
            
            if (hidden) {
                // Handle select2
                var select2 = $(this).is('[id^=select2_]');
                var s2id = $(this).is('[id^=s2id_]');

                // If current element is not a select2 element (could be a <select> element that is transformed to select2 - possibly is disabled)
                if (!select2 && !s2id) {
                    // If this element has a select2 element, ignore if it is hidden
                    if ($('#s2id_select2_' + this.id).length > 0) {
                        return $('#s2id_select2_' + this.id).is(':hidden');
                    }
                    
                    if ($('#s2id_' + this.id).length > 0) {
                        return $('#s2id_' + this.id).is(':hidden');
                    }
                }

                // Handle Date Time picker
                var dp = $('#' + this.id + '_Date');
                var tp = $('#' + this.id + '_Time');

                if (dp.length && tp.length) {
                    return dp.is(':hidden');
                }

                // Ignore hidden
                return true;
            }

            // Ignore if disabled
            return disabled ? true : false;
        };
        
        var form = $(this);
        settings.errorPlacement = function(error, inputElement) { // 'this' is the form element
            var container = form.find("[data-valmsg-for='" + escapeAttributeValue(inputElement[0].name) + "']"),
                replace = $.parseJSON(container.attr("data-valmsg-replace")) !== false;

            container.removeClass("field-validation-valid").addClass("field-validation-error");
            error.data("unobtrusiveContainer", container);

            if (replace) {
                container.empty();
                $('[' + $.zeusFullDataTypes.ErrorTipFor + '="' + escapeAttributeValue(inputElement[0].name) + '"]').hide();
                //$('#InnerContainerFor-' + inputElement[0].id).removeClass('parsley-error');
                $('#' +inputElement[0].id).removeClass('parsley-error');
                
                error.removeClass("input-validation-error").appendTo(container);
                error.removeClass("input-validation-error").appendTo($('[' + $.zeusFullDataTypes.ErrorTipFor + '="' + escapeAttributeValue(inputElement[0].name) + '"]'));
                
                if (error[0].innerHTML.length > 0) {
                    $('[' + $.zeusFullDataTypes.ErrorTipFor + '="' + escapeAttributeValue(inputElement[0].name) + '"]').show();
                    //$('#InnerContainerFor-' +inputElement[0].id).addClass('parsley-error');
                    $('#' +inputElement[0].id).addClass('parsley-error');
                }
            } else {
                error.hide();
                
                $('[' + $.zeusFullDataTypes.ErrorTipFor + '="' + escapeAttributeValue(inputElement[0].name) + '"]').hide();
                
                //$('#InnerContainerFor-' +inputElement[0].id).removeClass('parsley-error');
                $('#' +inputElement[0].id).removeClass('parsley-error');
            }
        };
        
        // Exact copy of jquery.validate.js file elements() function but with [disabled] exclusion removed which is now handled in our custom ignore() function
        // This is mainly for validation to trigger properly for <select> elements that are disabled and tranformed into select2 dropdowns
        validator.elements = function () {
            var validator = this,
                rulesCache = {};

            // select all valid inputs inside the form (no submit or reset buttons)
            return $(this.currentForm)
            .find("input, select, textarea")
            .not(":submit, :reset, :image")
            .not( this.settings.ignore )
            .filter(function() {
                if ( !this.name && validator.settings.debug && window.console ) {
                    console.error( "%o has no name assigned", this);
                }

                // select only the first element for each name, and only those with rules specified
                if ( this.name in rulesCache || !validator.objectLength($(this).rules()) ) {
                    return false;
                }

                rulesCache[this.name] = true;
                return true;
            });
        };
        
        //copy of method showLabel() in Jquery.validate-vsdoc.js
        //Process added to prevent addition of multiple span tags containing errors on elements.
        validator.showLabel = function(element, message) {
            var label = this.errorsFor(element);
            if (label.length) {
                    // refresh error/success class
                    label.removeClass(this.settings.validClass).addClass(this.settings.errorClass);

                    // check if we have a generated label, replace the message then
                    label.attr("generated") && label.html(message); 
               
            } else { 
                    // create label
                    var messageToDisplay = message !== undefined ? "Error: " + message : "";
                    label = $("<" + this.settings.errorElement + "/>")
                        .attr({ "for": this.idOrName(element), generated: true })
                        .addClass(this.settings.errorClass)
                        .html(messageToDisplay); 
                    if (this.settings.wrapper) {
                        // make sure the element is visible, even in IE
                        // actually showing the wrapped element is handled elsewhere
                        label = label.hide().show().wrap("<" + this.settings.wrapper + "/>").parent();
                    }
                    if ( !this.labelContainer.append(label).length )
					this.settings.errorPlacement
						? this.settings.errorPlacement(label, $(element) )
						: label.insertAfter(element);
            }

            if ( !message && this.settings.success ) {
				label.text("");
				typeof this.settings.success == "string"
					? label.addClass( this.settings.success )
					: this.settings.success( label );
			}
            this.toShow = this.toShow.add(label);
            
            // tidy element -> Preventing addition of multiple span tags on same element
            var name = this.idOrName(element);
             
            var errorTipAnchor = $('[' + $.zeusFullDataTypes.ErrorTipFor + '="' + name + '"]');
            
            if(errorTipAnchor.length == 0) {
                
                //check if name contains '_'
                while(name.indexOf('_') >= 0) 
                {
                    name = name.replace('_', '.');
                }
                errorTipAnchor = $('[' + $.zeusFullDataTypes.ErrorTipFor + '="' + name + '"]');
            }
            if(message === undefined) 
            {
                //remove span elements 
                errorTipAnchor.find('[generated="true"]').remove();
            } 
            else 
            {
                var spanCollection = errorTipAnchor.find('[generated="true"]');
                if (spanCollection.length > 1)
                { //if there is error(message) and if method has generated second span tag then remove it.
                    spanCollection.next().remove();
                } 
            }

        };
    });
});

(function ($) {
    $.validator.unobtrusive.parseDynamicContent = function (selector) {
        //use the normal unobstrusive.parse method
        $.validator.unobtrusive.parse(selector);

        //get the relevant form
        var form = $(selector).first().closest('form');

        //get the collections of unobstrusive validators, and jquery validators
        //and compare the two
        var unobtrusiveValidation = form.data('unobtrusiveValidation');
        var validator = form.validate();

        if (unobtrusiveValidation == undefined || unobtrusiveValidation.options == null || unobtrusiveValidation.options == undefined) {
            return;
        }

        $.each(unobtrusiveValidation.options.rules, function (elname, elrules) {
            if (validator.settings.rules[elname] == undefined) {
                var args = {};
                $.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                //edit:use quoted strings for the name selector
                $("[name='" + elname + "']").rules("add", args);
            } else {
                $.each(elrules, function (rulename, data) {
                    if (validator.settings.rules[elname][rulename] == undefined) {
                        var args = {};
                        args[rulename] = data;
                        args.messages = unobtrusiveValidation.options.messages[elname][rulename];
                        //edit:use quoted strings for the name selector
                        $("[name='" + elname + "']").rules("add", args);
                    }
                });
            }
        });
    }
})($);


