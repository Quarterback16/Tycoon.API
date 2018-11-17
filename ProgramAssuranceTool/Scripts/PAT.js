window.onerror = function (msg, url, line) {
    // You can view the information in an alert to see things working
    // like so:
    alert("Error: " + msg + "\nurl: " + url + "\nline #: " + line);
    alert("Javascript error occured.\nPlease refresh your browser cache by pressing Ctrl F5.");

    // TODO: Report this error via ajax so you can keep track
    //       of what pages have JS issues

    var suppressErrorAlert = false;
    // If you return true, then error alerts (like in older versions of 
    // Internet Explorer) will be suppressed.
    return suppressErrorAlert;
};

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

var pat = (function() {

    var projectGridBeforeRequest = function() {
        var savedSettings = $('#savedSettings').text();
        if (savedSettings.length > 0) {
            var $grid = $("#project");
            var $param = $grid.jqGrid('getGridParam');
            $param.postData.filters = savedSettings;
            $param.postData._search = true;
            $grid.jqGrid('setGridParam', { "search": "true" });
            $('#savedSettings').text('');
        }
    };

    var projectGridComplete = function() {
        var $grid = $("#project");
        var $param = $grid.jqGrid('getGridParam');
        if ($param.postData && $param.postData.filters) {
            var $rules = JSON.parse($param.postData.filters).rules;
            for (var i = 0; i < $rules.length; i++) {
                var rule = $rules[i];
                var id = '#gs_' + rule.field;
                var filterValue = rule.data;
                $(id).val(filterValue);
            }
            ;
            //$param.postData.filters = '';  //  this throws away the filter so if user pages page 2 is page 2 of full list.
            //$param.postData._search = false;
        }
        grid_OnGridComplete();
    };

    var reportGridOnLoadComplete = function(gridName) {
        var $theGrid = $('#' + gridName).jqGrid();
        var $gridParentShowResult = $theGrid.parents("div#gridResult");
        var $gridParentShowNoResult = $gridParentShowResult.next();

        var $rowCount = $theGrid.getRowData().length;
        if ($rowCount < 1) {
            // if have data then show the grid and hide the error message
            $gridParentShowResult.hide();

            $gridParentShowNoResult.removeClass("hidden");
            $gridParentShowNoResult.show();
    }
    else {
            // if no data then show the error message and hide the grid.
            $gridParentShowResult.show();
            $gridParentShowNoResult.hide();
        }

        grid_OnGridComplete();
    };


    var bulletinGridOn_EmptyCheck = function() {
        var $grid = $("#BulletinGrid");
        var $param = $grid.jqGrid('getGridParam');

        // are there any records?
        if ($param.records < 1) {
            ShowFlash('Bulletin/ FAQ not found! Please try again.', 'warning');
        }
    };


    var projectEditContract_SelectContracts = function() {

        //  get what has already been selected in the form of a comma delimited string of ids
        //var selectedIds = $projectEditContractsDiv.find(" #selections").html();
        var selectedIds = $("#contract-selections").html();

        var selection = selectedIds.split(',');

        //var grid = $projectEditContractsDiv.find(" #contracts");
        var grid = $("#contracts");
        var ids = grid.jqGrid("getDataIDs");

        for (var i = 0; i < ids.length; i++) {
            if (selection.indexOf(ids[i]) > -1)
                grid.jqGrid("setSelection", ids[i]);
        }
    };

    var projectIndex_EmptyCheck = function() {
        var $projectIndexForm = $("#projectIndexForm");

        var recs = $projectIndexForm.find(" #grid .jqgrow").length;
        if (recs == 0) {
            var msg = 'There are no projects with these criteria';
            ShowFlash(msg, 'warning');
        }
    };

    // ----------------------------------------------------
    // this is a function to show the flash from javascript
    // msg: the message
    // type: info, warning, error
    // ----------------------------------------------------
    var ShowFlash = function(msg, type) {
        var $flash = $('#flash');

        // reset the event binding to avoid multiple event binding
        $flash.off();

        // reset the flash type
        $flash.removeClass('info warning error');

        //set the flash type
        $flash.toggleClass(type, true);

        $flash.show();
        $flash.find(' #message').html(msg);
        $flash
            .on("click", function() {
                $(this).toggle('highlight');
                $(this).hide();
            })
            .on("keyup", function(event) {
                if (event.which == 13) {
                    $(this).toggle('highlight');
                    $(this).hide();
                }
            });

        // set the focus to this flash
        $flash.focus();
    };

    var closePageWarning = function() {
        if (window.dontShowWarning) return undefined;

        var unloadMessage = "Data has not been saved. If you proceed you will lose current changes";

        // get a specific unload message from the respective page e.g.
        // @Html.Hidden("unloadMessage", "Data has not been uploaded. If you proceed you will lose current changes")
        var $unloadMessage = $("#unloadMessage");
        if ($unloadMessage.length > 0) {
            unloadMessage = $unloadMessage.val();
        }

        return unloadMessage;
    };

    ///
    /// use `return` to keep the existing page values intact
    /// e.g. return DataNotSavedWarning(targetUrl);
    ///
    var DataNotSavedWarning = function(targetUrl) {
        var msg = "Data has not been saved. Pressing Discard Changes will continue to the previous/next review. Otherwise, Cancel will return to the current Review Checklist.";
        bootbox.dialog({
            message: msg,
            title: "Warning: Data has not been saved",
            buttons: {
                Discard: {
                    label: "Discard Changes",
                    className: "btn-succes",
                    callback: function() {
                        location.href = targetUrl;
                    }
                },
                Cancel: {
                    label: "Cancel",
                    className: "btn-primary",
                    callback: function() {
                        bootbox.hideAll();
                    }
                }
            }
        });
        return false; // return false so the value on the page still intact 
    };

    var reviewDetails_ConfirmDelete = function() {
        var $reviewDetailsDiv = $("#reviewDetailsDiv");

        window.dontShowWarning = true;

        var msg = $reviewDetailsDiv.find(' #delete-message').html();

        bootbox.dialog({
            message: msg,
            title: "Delete Reviews",
            buttons: {
                yes: {
                    label: "Yes",
                    className: "btn-success",
                    callback: function() {
                        $reviewDetailsDiv.find(' #reviewDetailsForm').submit();
                    }
                },
                no: {
                    label: "No",
                    className: "btn-primary",
                    callback: function() {
                        bootbox.hideAll();
                    }
                }
            }
        });
    };

    var reviewDetails_ManageOutcomeControls = function(setDirtyFlag) {
        var $reviewDetailsDiv = $("#reviewDetailsDiv");

        if (setDirtyFlag) {
            window.dontShowWarning = false;
            $reviewDetailsDiv.find(' #ChangesMade').val('Y');
            $reviewDetailsDiv.find(' #save-button', this).removeAttr('disabled').toggleClass("ui-state-disabled", false);
        }

        if ($reviewDetailsDiv.length > 0) {

            var assessmentCode = $reviewDetailsDiv.find(' #Review_AssessmentCode').val();
            var recoveryReason = $reviewDetailsDiv.find(' #Review_RecoveryReason').val();
            var assessmentAction = $reviewDetailsDiv.find(' #Review_AssessmentAction').val();

            $reviewDetailsDiv.find(" #Review_AssessmentAction").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
            $reviewDetailsDiv.find(" #Review_RecoveryReason").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
            $reviewDetailsDiv.find(" #Review_OutcomeCode").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
            $reviewDetailsDiv.find(" #Review_ClaimRecoveryAmount").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);

            if (assessmentCode.length > 0) {
                if (assessmentCode == 'VLD' || assessmentCode == 'VLQ') {

                    $reviewDetailsDiv.find(' #Review_OutcomeCode').val('VAN');
                    $reviewDetailsDiv.find(" #Review_OutcomeCode").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
                    reviewDetails_ResetRecoveryReason();
                    reviewDetails_ResetAssessmentAction();

                } else {
                    //  invalid
                    reviewDetails_EnableRecoveryReason();
                    if (recoveryReason.length > 0) {
                        reviewDetails_EnableAssessmentAction();
                        if (assessmentAction.length > 0) {
                            reviewDetails_EnableFinalOutcome();
                        } else {
                            reviewDetails_ResetFinalOutcome();
                        }
                    } else {
                        reviewDetails_ResetAssessmentAction();
                        reviewDetails_ResetFinalOutcome();
                    }
                }
            } else {
                reviewDetails_ResetRecoveryReason();
                reviewDetails_ResetAssessmentAction();
                reviewDetails_ResetFinalOutcome();
            }

        }

    };

    var reviewDetails_ResetRecoveryReason = function() {
        var $reviewDetailsDiv = $("#reviewDetailsDiv");
        $reviewDetailsDiv.find(' #Review_RecoveryReason').val('');
        $reviewDetailsDiv.find(" #Review_RecoveryReason").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
    };

    var reviewDetails_EnableRecoveryReason = function() {
        var $reviewDetailsDiv = $("#reviewDetailsDiv");
        $reviewDetailsDiv.find(" #Review_RecoveryReason").removeAttr('disabled').toggleClass('ui-state-disabled', false);
    };

    var reviewDetails_ResetAssessmentAction = function() {
        var $reviewDetailsDiv = $("#reviewDetailsDiv");
        $reviewDetailsDiv.find(' #Review_AssessmentAction').val('');
        $reviewDetailsDiv.find(" #Review_AssessmentAction").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
    };

    var reviewDetails_EnableAssessmentAction = function() {
        var $reviewDetailsDiv = $(" #reviewDetailsDiv");
        $reviewDetailsDiv.find(" #Review_AssessmentAction").removeAttr('disabled').toggleClass('ui-state-disabled', false);
    };

    var reviewDetails_ResetFinalOutcome = function() {
        var $reviewDetailsDiv = $(" #reviewDetailsDiv");
        $reviewDetailsDiv.find(' #Review_OutcomeCode').val('');
        $reviewDetailsDiv.find(" #Review_OutcomeCode").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
    };

    var reviewDetails_EnableFinalOutcome = function() {
        var $reviewDetailsDiv = $(" #reviewDetailsDiv");
        $reviewDetailsDiv.find(" #Review_OutcomeCode").removeAttr('disabled').toggleClass('ui-state-disabled', false);
    };

    var projectDetails_ExportReviews = function() {
        var $projectDetailsDiv = $("#projectDetailsDiv");

        var projectId = $projectDetailsDiv.find(' #projectId').text();
        //  glean the filter conditions from the known IDs
        var uploadId = $projectDetailsDiv.find(' #gs_UploadId').val();
        var reviewId = $projectDetailsDiv.find(' #gs_ReviewId').val();
        var orgCode = $projectDetailsDiv.find(' #gs_OrgCode').val();
        var esaCode = $projectDetailsDiv.find(' #gs_EsaCode').val();
        var siteCode = $projectDetailsDiv.find(' #gs_SiteCode').val();
        var stateCode = $projectDetailsDiv.find(' #gs_StateCode').val();
        var jobseekerId = $projectDetailsDiv.find(' #gs_JobseekerId').val();
        var claimId = $projectDetailsDiv.find(' #gs_ClaimId').val();
        var activityId = $projectDetailsDiv.find(' #gs_ActivityId').val();
        var assessmentCode = $projectDetailsDiv.find(' #gs_AssessmentCode').val();
        var recoveryReason = $projectDetailsDiv.find(' #gs_RecoveryReason').val();
        var assessmentAction = $projectDetailsDiv.find(' #gs_AssessmentAction').val();
        var outcomeCode = $projectDetailsDiv.find(' #gs_OutcomeCode').val();

        //  Save the filter criteria first
        $.ajax({
            url: '/Project/SaveExport/',
            type: 'post',
            data: {
                projectId: projectId,
                uploadId: uploadId,
                reviewId: reviewId,
                orgCode: orgCode,
                esaCode: esaCode,
                siteCode: siteCode,
                stateCode: stateCode,
                jobseekerId: jobseekerId,
                claimId: claimId,
                activityId: activityId,
                assessmentCode: assessmentCode,
                recoveryReason: recoveryReason,
                assessmentAction: assessmentAction,
                outcomeCode: outcomeCode
            },
            async: false
        });
        //  then go ahead and generate the CSV
        $projectDetailsDiv.find(' #exportForm').submit();
    };

    var projectDetails_OpenDeleteDialog = function() {
        var $projectDetailsDiv = $("#projectDetailsDiv");

        var deleteme = $projectDetailsDiv.find(' #projectId').text();
        var $dialog = $projectDetailsDiv.find(' #dialog-confirm')
            .dialog({
                resizable: false,
                autoOpen: false,
                height: 300,
                width: 350,
                modal: true,
                title: 'Delete Project ' + deleteme,
                buttons: {
                    "Delete this project": function() {
                        //  callback function
                        projectDetails_DeleteProject(deleteme);
                        //  call delete routine?
                        $(this).dialog("close");
                        location.href = "/Project/Index";
                    },
                    Cancel: function() {
                        $(this).dialog("close");
                    }
                }
            });

        $dialog.dialog('open');
    };

    var projectDetails_DeleteProject = function(id) {
        $.ajax({
            url: '/Project/Delete/',
            type: 'POST',
            data: { id: id },
            success: function() {
                //alert("Project " + id + " has been deleted");
            },
            Error: function(result) {
                alert(result.responseText);
                ShowFlash(result.responseText, 'error');
            }
        });
    };

    var reviewEdit_ManageOutcomeControls = function(setDirtyFlag) {
        var $reviewEditForm = $("#reviewEditForm");

        if (setDirtyFlag) {
            window.dontShowWarning = false;
            $reviewEditForm.find(' #save-button', this).removeAttr('disabled').toggleClass("ui-state-disabled", false);
        }
        var assessmentCode = $reviewEditForm.find(' #Review_AssessmentCode').val();
        var recoveryReason = $reviewEditForm.find(' #Review_RecoveryReason').val();
        var assessmentAction = $reviewEditForm.find(' #Review_AssessmentAction').val();
        var outcomeCode = $reviewEditForm.find(' #Review_OutcomeCode').val();

        $reviewEditForm.find(" #Review_AssessmentAction").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
        $reviewEditForm.find(" #Review_RecoveryReason").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
        $reviewEditForm.find(" #Review_OutcomeCode").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
        $reviewEditForm.find(" #Review_ClaimRecoveryAmount").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);

        if (assessmentCode.length > 0) {
            if (assessmentCode == 'VLD' || assessmentCode == 'VLQ') {
                outcomeCode = 'VAN';
                $reviewEditForm.find(' #Review_OutcomeCode').val(outcomeCode);
                $reviewEditForm.find(" #Review_OutcomeCode").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
                reviewEdit_ResetRecoveryReason($reviewEditForm);
                reviewEdit_ResetAssessmentAction($reviewEditForm);
            } else {
                //  invalid
                reviewEdit_EnableRecoveryReason($reviewEditForm);
                if (recoveryReason.length > 0) {
                    reviewEdit_EnableAssessmentAction($reviewEditForm);
                    if (assessmentAction.length > 0) {
                        reviewEdit_EnableFinalOutcome($reviewEditForm);
                    } else {
                        reviewEdit_ResetFinalOutcome($reviewEditForm);
                    }
                } else {
                    reviewEdit_ResetAssessmentAction($reviewEditForm);
                    reviewEdit_ResetFinalOutcome($reviewEditForm);
                }
            }
        } else {
            reviewEdit_ResetRecoveryReason($reviewEditForm);
            reviewEdit_ResetAssessmentAction($reviewEditForm);
            reviewEdit_ResetFinalOutcome($reviewEditForm);
            outcomeCode = '';
        }

        if (outcomeCode == 'INR')
            reviewEdit_EnableRecoveryAmount($reviewEditForm);
        else
            reviewEdit_ResetRecoveryAmount($reviewEditForm);

    };

    var reviewEdit_ResetRecoveryReason = function($form) {
        $form.find(' #Review_RecoveryReason').val('');
        $form.find(" #Review_RecoveryReason").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
    };

    var reviewEdit_EnableRecoveryReason = function($form) {
        $form.find(" #Review_RecoveryReason").removeAttr('disabled').toggleClass('ui-state-disabled', false);
    };

    var reviewEdit_ResetAssessmentAction = function($form) {
        $form.find(' #Review_AssessmentAction').val('');
        $form.find(" #Review_AssessmentAction").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
    };

    var reviewEdit_EnableAssessmentAction = function($form) {
        $form.find(" #Review_AssessmentAction").removeAttr('disabled').toggleClass('ui-state-disabled', false);
    };

    var reviewEdit_ResetFinalOutcome = function($form) {
        $form.find(' #Review_OutcomeCode').val('');
        $form.find(" #Review_OutcomeCode").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
        reviewEdit_ResetRecoveryAmount($form);
    };

    var reviewEdit_EnableFinalOutcome = function($form) {
        $form.find(" #Review_OutcomeCode").removeAttr('disabled').toggleClass('ui-state-disabled', false);
    };

    var reviewEdit_ResetRecoveryAmount = function($form) {
        $form.find(' #Review_ClaimRecoveryAmount').val('0.00');
        $form.find(" #Review_ClaimRecoveryAmount").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
    };

    var reviewEdit_EnableRecoveryAmount = function($form) {
        $form.find(" #Review_ClaimRecoveryAmount").removeAttr('disabled').toggleClass('ui-state-disabled', false);
    };

    var sampleClaimList_LoadEvent = function() {
        var $sampleClaimListDiv = $("#sampleClaimListDiv");

        //  get what has already been selected in the form of a comma delimited string of ids
        var selectedIds = $sampleClaimListDiv.find(" #selections").html();
        var selection = selectedIds.split(',');

        var grid = $sampleClaimListDiv.find(" #sample");
        var ids = grid.jqGrid("getDataIDs");

        for (var i = 0; i < ids.length; i++) {
            if (selection.indexOf(ids[i]) > -1)
                grid.jqGrid("setSelection", ids[i]);
        }
        grid_OnGridComplete();
    };

    var sampleClaimList_RowSelected = function() {
        var $sampleClaimListDiv = $("#sampleClaimListDiv");

        var grid = $sampleClaimListDiv.find(" #sample");
        var selected = grid.getGridParam('selarrrow');
        var selections = selected.length;
        var ids = grid.jqGrid("getDataIDs");
        $sampleClaimListDiv.find(" #selection-count").html(ids.length + " claims extracted and " + selections + " claims selected");
        if (selections == 0)
            $sampleClaimListDiv.find(" #submit-sample").attr('disabled', 'disabled').toggleClass('ui-state-disabled', true);
        else
            $sampleClaimListDiv.find(" #submit-sample").removeAttr('disabled').toggleClass('ui-state-disabled', false);
    };


    /// workaround to highlight the link that doesn't work in IE 
    /// usage: call this on the event of jqgrid complete.
    var grid_OnGridComplete = function() {
        // highlight the input/ checkbox in the table header
        $("table.ui-jqgrid-htable th input").on('focusin', function() {
            $(this).parent().css('background-color', '#eee');
        });

        $("table.ui-jqgrid-htable th input").on('focusout', function() {
            $(this).parent().css('background-color', '#666');
        });

        // highlight the input/ checkbox in the table row
        $("table.ui-jqgrid-btable tr.jqgrow a, table.ui-jqgrid-btable tr.jqgrow input").on('focusin', function() {
            $(this).parents("tr").toggleClass("ui-state-highlight", true);
        });

        $("table.ui-jqgrid-btable tr.jqgrow a, table.ui-jqgrid-btable tr.jqgrow input").on('focusout', function() {
            $(this).parents("tr").toggleClass("ui-state-highlight", false);
        });

        // add tooltip on the search textbox taken from its label header row 
        $("table.ui-jqgrid-htable").find(".ui-search-toolbar input[name], .ui-search-toolbar select[name]").each(function() {

            // find the row header with labels
            var $labelRow = $(this).closest("tr").prevAll(".ui-jqgrid-labels");

            // find the column title based on the column name
            var labelTitle = $labelRow.find("div[id$='" + this.name + "']").text();

            // set the title/ tooltip
            $(this).attr("title", labelTitle);
        });

        // add tooltip on the page number and page size.
        $(".ui-jqgrid .ui-pg-input").attr("title", "Current page number");
        $(".ui-jqgrid .ui-pg-selbox").attr("title", "Page size option");

        // hide the collapse icon
        $("a[role='link']").hide();
    };

    // declare here as global so we can debug it if needed
    var $blockUIoptions =
    {
        message: null,
        css: {
            padding: '15px',
            border: 'solid 1px #000',
            backgroundColor: '#ffffff',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            color: '#000'
        },
        overlayCSS: {
            backgroundColor: '#000',
            opacity: 0,
            cursor: 'wait'
        }
    };

    // check the css to know how to display the progresbar.gif
    var $blockUIdefaultMessage = '<div class="msgInfo"><p>Retrieving data please wait</p><img src="/Content/Images/ProgressBar.gif" /></div>';

    return {
        projectGridBeforeRequest: projectGridBeforeRequest,
        projectGridComplete: projectGridComplete,
        reportGridOnLoadComplete: reportGridOnLoadComplete,
        bulletinGridOn_EmptyCheck: bulletinGridOn_EmptyCheck,
        projectEditContract_SelectContracts: projectEditContract_SelectContracts,
        projectIndex_EmptyCheck: projectIndex_EmptyCheck,
        closePageWarning: closePageWarning,
        ShowFlash: ShowFlash,
        DataNotSavedWarning: DataNotSavedWarning,
        reviewDetails_ConfirmDelete: reviewDetails_ConfirmDelete,
        reviewDetails_ManageOutcomeControls: reviewDetails_ManageOutcomeControls,
        reviewDetails_ResetRecoveryReason: reviewDetails_ResetRecoveryReason,
        reviewDetails_EnableRecoveryReason: reviewDetails_EnableRecoveryReason,
        reviewDetails_ResetAssessmentAction: reviewDetails_ResetAssessmentAction,
        reviewDetails_EnableAssessmentAction: reviewDetails_EnableAssessmentAction,
        reviewDetails_ResetFinalOutcome: reviewDetails_ResetFinalOutcome,
        reviewDetails_EnableFinalOutcome: reviewDetails_EnableFinalOutcome,
        projectDetails_ExportReviews: projectDetails_ExportReviews,
        projectDetails_OpenDeleteDialog: projectDetails_OpenDeleteDialog,
        projectDetails_DeleteProject: projectDetails_DeleteProject,
        reviewEdit_ManageOutcomeControls: reviewEdit_ManageOutcomeControls,
        reviewEdit_ResetRecoveryReason: reviewEdit_ResetRecoveryReason,
        reviewEdit_EnableRecoveryReason: reviewEdit_EnableRecoveryReason,
        reviewEdit_ResetAssessmentAction: reviewEdit_ResetAssessmentAction,
        reviewEdit_EnableAssessmentAction: reviewEdit_EnableAssessmentAction,
        reviewEdit_ResetFinalOutcome: reviewEdit_ResetFinalOutcome,
        reviewEdit_EnableFinalOutcome: reviewEdit_EnableFinalOutcome,
        reviewEdit_ResetRecoveryAmount: reviewEdit_ResetRecoveryAmount,
        reviewEdit_EnableRecoveryAmount: reviewEdit_EnableRecoveryAmount,
        sampleClaimList_LoadEvent: sampleClaimList_LoadEvent,
        sampleClaimList_RowSelected: sampleClaimList_RowSelected,
        grid_OnGridComplete: grid_OnGridComplete,
        $blockUIoptions: $blockUIoptions,
        $blockUIdefaultMessage: $blockUIdefaultMessage
    };

})();
    
$(document).ready(
    function() {

        window.dontShowWarning = true;
        window.onbeforeunload = pat.closePageWarning;

            // Ajax indicator bound to ajax start/stop document events
        $(document).ajaxStart(function() {
            $.blockUI(pat.$blockUIoptions);
        }).ajaxSend(function() {
            $.blockUI(pat.$blockUIoptions);
        });

        $(document).ajaxComplete(function() {
            $.unblockUI();
            pat.$blockUIoptions.message = pat.$blockUIdefaultMessage;
        }).ajaxStop(function() {
            $.unblockUI();
            pat.$blockUIoptions.message = pat.$blockUIdefaultMessage;
        }).ajaxSuccess(function() {
            $.unblockUI();
            pat.$blockUIoptions.message = pat.$blockUIdefaultMessage;
        }).ajaxError(function() {
            $.unblockUI();
            pat.$blockUIoptions.message = pat.$blockUIdefaultMessage;
        });

        // applied for all submit input
        $(":submit").click(function() {
            
            // Change message to indicate data is being submitted, check the css to know how to display the progresbar.gif
            pat.$blockUIoptions.message = '<div class="msgInfo"><p>Sending data please wait</p><img src="/Content/Images/ProgressBar.gif" /></div>';

            // Block UI, relying on page to reload or redirect which results in an unblocked page
            $.blockUI(pat.$blockUIoptions);

            // Remove block after a short timeout (2.5 sec) so eventually the user will have control again if something went wrong
            var timeout = 1000 * 2.5;

            setTimeout(function() {
                $.unblockUI();
                pat.$blockUIoptions.message = pat.$blockUIdefaultMessage;
            }, timeout);
        });

        $('.date').datepicker({
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showOn: 'both',
            showButtonPanel: true,
            buttonImage: '/Content/Images/calendar.png',
            buttonText: 'Open the Calendar',
            buttonImageOnly: true,
        }); //  all dates are pickers

        // ********************************************************************************
        // ---------- Tabbed Skip Links ----------
        var skipLinks = $('#skipLinks');
        skipLinks.find('a')
            .focus(function() { skipLinks.removeClass('readers'); })
            .blur(function() { skipLinks.addClass('readers'); });

        $(".skip").focus(function() { $(this).removeClass('readers'); });
        $(".skip").blur(function() { $(this).addClass('readers'); });

        // ---------- Tabbed Navigation ----------
        $('ul a')
            .focus(function() { $(this).parents('li').addClass('hover'); })
            .blur(function() { $(this).parents('li').removeClass('hover'); });

        // ---------- Fix for Chrome/IE's skip-link-focus ----------
        $("a[href^='#']").click(function() {
            var id = $(this).attr('href');
            var el = $(id);
            if ((!el.is('a') || !el.attr('href')) && !el.is('input'))
                el.attr('tabindex', '-1');
            el.focus();
        });

        // ********************************************************************************
        setTimeout(function () {
            // After page load, if there are any error, warning, information or success messages then focus #content so the messages are read to screen-reader users
            var errors = $('#validation-error-summary ul li');
            var flash = $("#flash");
            
            // Summary exists and is not hidden and contains messages
            if (errors.length > 0 && errors.not(':hidden')) {
                // Focus main error form header
                var errorH2 = $('#validation-error-summary span');

                if (errorH2.length) {
                    // Temporarily add tabindex to allow focus on non <input>, <a> and <select> element
                    errorH2.attr('tabindex', '-1');

                    // Apply focus
                    errorH2.focus();

                    // Remove tabindex
                    // errorH2.removeAttr('tabindex');
                }
            } else if (flash.length > 0 && flash.not(':hidden')) {
                // Focus success header
                var successH2 = flash; 

                if (successH2.length) {
                    // Temporarily add tabindex to allow focus on non <input>, <a> and <select> element
                    successH2.attr('tabindex', '-1');

                    // Apply focus
                    successH2.focus();

                    // Remove tabindex
                    // successH2.removeAttr('tabindex');
                }
            } 
        }, 1);

        // ********************************************************************************
        // Obey maxlength on textarea
        $('textarea[maxlength]').change(function () {
            var property = $(this);
            var maxlength = property.attr('maxlength');
                      
            var maxlengthExtra = parseInt(maxlength, 10);
            var value = property.val();

            // Normalize line breaks to single character of \n
            value = value.replace(/\r\n/g, "\n").replace(/\r/g, "");

            var linebreaks = (value.match(/\n/g) || []).length;

            // Length including linebreaks
            var length = value.length + linebreaks;

            var textLength = length;

            if (length > maxlengthExtra) {
                // Maximum exceeded
                value = value.substr(0, maxlengthExtra - linebreaks);

                textLength = value.length;
                alert("The field content must be within the maximum length of " + maxlengthExtra + ".\nIt has been truncated automatically.");
            }

            var range = property.textrange('get');

            property.val(value);
            property.textrange('set', range.start, range.end);

            property.attr("title", "Text Length: " + textLength);
        });

        // ---------- Count & display the textarea length ----------
        $('textarea[maxlength]').focus(function () {
            var property = $(this);
            var value = property.val();

            // Normalize line breaks to single character of \n
            value = value.replace(/\r\n/g, "\n").replace(/\r/g, "");

            var linebreaks = (value.match(/\n/g) || []).length;

            // Length including linebreaks
            var length = value.length + linebreaks;

            $(this).attr("title", "Text Length: " + length);
        });

        $('textarea[maxlength]').on('paste', function () {
            // relocated here from inside the setTimeout scope
            // so it will not cause the javascript error
            var property = $(this);
            
            setTimeout(function () {
                var maxlength = property.attr('maxlength');

                var maxlengthExtra = parseInt(maxlength, 10);
                var value = property.val();

                // Normalize line breaks to single character of \n
                value = value.replace(/\r\n/g, "\n").replace(/\r/g, "");

                var linebreaks = (value.match(/\n/g) || []).length;

                // Length including linebreaks
                var length = value.length + linebreaks;

                var textLength = length;

                if (length > maxlengthExtra) {
                    // Maximum exceeded
                    value = value.substr(0, maxlengthExtra - linebreaks);

                    textLength = value.length;
                    alert("The field content must be within the maximum length of " + maxlengthExtra + ". \nIt has been truncated automatically.");
                }

                var range = property.textrange('get');

                property.val(value);
                property.textrange('set', range.start, range.end);

                property.attr("title", "Text Length: " + textLength);
            }, 100);

        });
        // ********************************************************************************
        var $reportSearchForm = $("#reportSearchForm, #complianceReportSearchForm");

        var extraMessage = "<span  id='extraMessage'>Project ID or Project Type are a required field</span>";

        // event handler to determine the search type upon loading the screen 
        if ($reportSearchForm.find(" #AdvanceSearchType:checked").val() == "True") {
            $reportSearchForm.find("#OrgCode").parent().hide();
            $reportSearchForm.find("#ESACode").parent().hide();
            $reportSearchForm.find("#SiteCode").parent().hide();
            $reportSearchForm.find("#ProjectID").parent().show();
            $reportSearchForm.find("#ProjectType").parent().show();
            $reportSearchForm.find("#ProjectType").prev().find("span.label-required").show();
            $reportSearchForm.find(" #RequiredFieldsNote #message").hide();
            $reportSearchForm.find(" #RequiredFieldsNote #message").before(extraMessage);
        }
        else {
            $reportSearchForm.find("#OrgCode").parent().show();
            $reportSearchForm.find("#ESACode").parent().show();
            $reportSearchForm.find("#SiteCode").parent().show();
            $reportSearchForm.find("#ProjectID").parent().hide();

            if ($reportSearchForm.attr("id") == 'reportSearchForm') {
                $reportSearchForm.find("#ProjectType").parent().hide();
            }
            else {
                // show the Project Type but hide the red asterisk
                $reportSearchForm.find("#ProjectType").parent().show();
                $reportSearchForm.find("#ProjectType").prev().find("span.label-required").hide();
            }
            
            $reportSearchForm.find(" #RequiredFieldsNote #extraMessage").remove();
            $reportSearchForm.find(" #RequiredFieldsNote #message").show();
        }

        var $inputFields = " input, textarea, select";

        // event handler to determine the search type when clicking the 'Search type' radio button
        $reportSearchForm.find(" #AdvanceSearchType, #BasicSearchType").change(function() {

            $reportSearchForm.find(".validation-summary-errors").hide();

            if ($(this).val() == "True") {
                // advance search type
                $reportSearchForm.find(" #OrgCode").parent().hide();
                $reportSearchForm.find(" #ESACode").parent().hide();
                $reportSearchForm.find(" #SiteCode").parent().hide();
                $reportSearchForm.find(" #ProjectID").parent().show();
                $reportSearchForm.find(" #ProjectType").parent().show();
                $reportSearchForm.find(" #ProjectType").prev().find("span.label-required").show();               
                $reportSearchForm.find(" #RequiredFieldsNote #message").hide();
                $reportSearchForm.find(" #RequiredFieldsNote #message").before(extraMessage);
            }
            else {
                // basic search type
                $reportSearchForm.find(" #OrgCode").parent().show();
                $reportSearchForm.find(" #ESACode").parent().show();
                $reportSearchForm.find(" #SiteCode").parent().show();
                $reportSearchForm.find(" #ProjectID").parent().hide();

                if ($reportSearchForm.attr("id") == 'reportSearchForm') {
                    $reportSearchForm.find("#ProjectType").parent().hide();
                }
                else {
                    // show the Project Type but hide the red asterisk
                    $reportSearchForm.find("#ProjectType").parent().show();
                    $reportSearchForm.find("#ProjectType").prev().find("span.label-required").hide();
                }
                
                $reportSearchForm.find(" #RequiredFieldsNote #extraMessage").remove();
                $reportSearchForm.find(" #RequiredFieldsNote #message").show();
            }
        });

        $reportSearchForm.find(" #ProjectID").autocomplete({
            minLength: 2,
            delay: 500,
            source: function (request, response) {
                $.ajax({
                    url: "/Project/LookupProject",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, maxResults: 10 },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return { label: item.ProjectId + " - " + item.ProjectName, value: item.ProjectId + " - " + item.ProjectName, id: item.ProjectId, projectType: item.ProjectType };
                        }));
                    }
                });
            },
            select: function(event, ui) {
                $reportSearchForm.find(" #ProjectType").val(ui.item.projectType);
            }
        });

        $reportSearchForm.find(" #OrgCode").autocomplete({
            minLength: 2,
            delay: 500,            
            source: function(request, response) {
                $.ajax({
                    url: "/Project/LookupOrg",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, maxResults: 10 },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.Value + " - " + item.Text,
                                value: item.Value + " - " + item.Text,
                                id: item.Value
                            };
                        }));
                    }
                });
            }
        });

        $reportSearchForm.find(" #ESACode").autocomplete({
            minLength: 2,
            delay: 500,
            source: function (request, response) {
                var $orgCode = $reportSearchForm.find(" #OrgCode").val().split('-')[0].trim();
                $.ajax({
                    url: "/Report/LookupESABy",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, orgCode: $orgCode, maxResults: 10 },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.Value + " - " + item.Text,
                                value: item.Value + " - " + item.Text,
                                id: item.Value
                            };
                        }));
                    }
                });
            }
        });

        $reportSearchForm.find(" #SiteCode").autocomplete({
            minLength: 2,
            delay: 500,
            source: function (request, response) {
                var $orgCode = $reportSearchForm.find(" #OrgCode").val().split('-')[0].trim();
                var $esaCode = $reportSearchForm.find(" #ESACode").val().split('-')[0].trim();
                $.ajax({
                    url: "/Report/LookupSiteBy",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, orgCode: $orgCode, esaCode: $esaCode, maxResults: 10 },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.Value + " - " + item.Text,
                                value: item.Value + " - " + item.Text,
                                id: item.Value
                            };
                        }));
                    }
                });
            }
        });

        // ********************************************************************************
        var $createBulletinForm = $("#bulletinCreateForm, #bulletinEditForm");

        // event handler to serialize the original input values of create bulletin form
        var $createBulletinFormOriginalValue = $createBulletinForm.find($inputFields).serialize();
        
        $createBulletinForm.find(" #ProjectField").autocomplete({
            minLength: 2,
            delay: 500,
            source: function (request, response) {
                $.ajax({
                    url: "/Project/LookupProject",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, maxResults: 10 },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return { label: item.ProjectId + " - " + item.ProjectName, value: item.ProjectId + " - " + item.ProjectName, id: item.ProjectId, projectType: item.ProjectType };
                        }));
                    }
                });
            }
        });

        $createBulletinForm.find(" #btnDeletePrompt").click(function() {
            bootbox.confirm("Are you sure you want to delete this Bulletin?", function(result) {
                if (result) {
                    // trigger/ submit the actual delete button to backend
                    $createBulletinForm.find(" #btnDelete").trigger("click");
                };
            });
        });

        $createBulletinForm.find(" #StartDate").change(function () {
            var startDate = $createBulletinForm.find(" #StartDate").val();
            $.post("/Bulletin/Get4WeeksDate?startDate=" + startDate, function (endDate) {

                // set the end date with 4 weeks date apart
                $createBulletinForm.find(" #EndDate").val(endDate);
            });
        });

        $createBulletinForm.submit(function() {
            window.dontShowWarning = true;
        });

        $createBulletinForm.change(function() {
            window.dontShowWarning = false;

            var $createBulletinFormCurrentValue = $createBulletinForm.find($inputFields).serialize();

            if ($createBulletinFormOriginalValue == $createBulletinFormCurrentValue) {
                window.dontShowWarning = true;
            }
        });

        // enforce the validation on other event on other input since IE compatibility issue
        $createBulletinForm.find($inputFields).on("keyup", function () {
            window.dontShowWarning = false;

            var $createBulletinFormCurrentValue = $createBulletinForm.find($inputFields).serialize();

            if ($createBulletinFormOriginalValue == $createBulletinFormCurrentValue) {
                window.dontShowWarning = true;
            }
        });

        // ********************************************************************************
        var $checkListForm = $("#checkListEditForm");

        var hasChanges = $checkListForm.find(" #hasChanges").is(":checked");
        if (hasChanges) {
            window.dontShowWarning = false;
        }

        $checkListForm.find(" #btnPrevious, #btnNext").click(function () {           
            window.dontShowWarning = true;
            var $reviewID = $checkListForm.find(" #ReviewID").val();
            var targetUrl = '/CheckList/Navigate/?id=' + $reviewID + "&command=" + $(this).val();
            location.href = targetUrl;
        });
        
        $checkListForm.find(" #btnSave").click(function () {
            window.dontShowWarning = true;
        });

        // event handler to serialize the original input values of checklist form
        var $checkListFormOriginalValue = $checkListForm.find($inputFields).serialize();

        // enforce the validation on other event on other input since IE compatibility issue
        $checkListForm.find($inputFields).on("keyup change", function () {
            window.dontShowWarning = false;

            // by default enable the save button but we have to disable it if no changes as per below
            $checkListForm.find(" #btnSave").attr("disabled", false);

            var $checkListFormCurrentValue = $checkListForm.find($inputFields).serialize();

            if ($checkListFormOriginalValue == $checkListFormCurrentValue) {
                window.dontShowWarning = true;
                
                // only if there is no diff & no previous changes then disable the save button
                if (!hasChanges) {
                    $checkListForm.find(" #btnSave").attr("disabled", true);
                }
            }
        });

        // ********************************************************************************
        var $detailBulletinForm = $("#bulletinDetailsDiv");

        $detailBulletinForm.find(".bulletinDetail").focus(function () {
            // first remove other highlighted div
            $("div.bulletinDetail").css("background-color", "");

            // then set the current background color
            $(this).css("background-color", "chartreuse");
        });

        // ********************************************************************************
        var $attachmentForm = $("#attachmentEditForm");

        $attachmentForm.find(" #btnDeletePrompt").click(function () {
            bootbox.confirm("Are you sure you want to delete this Attachment?", function (result) {
                if (result) {
                    // trigger/ submit the actual delete button to backend
                    $attachmentForm.find(" #btnDelete").trigger("click");
                };
            });
        });

        // ********************************************************************************
        var $projectCreateForm = $('#projectCreateForm');

        $projectCreateForm.submit(function () {
            $projectCreateForm.find(' input[type=submit]', this).attr('disabled', 'disabled');  //  turn button off after one click
        });
        
        $projectCreateForm.find(" :input:not(:button)").css("background-color", "lemonchiffon");
        $projectCreateForm.find(" #Project_Organisation").addClass("form-control");
        $projectCreateForm.find(".form-group").change(function () {
            window.dontShowWarning = false;
        });

        $projectCreateForm.find(" #Project_Organisation").autocomplete({
            minLength: 2,
            delay: 500,
            source: function (request, response) {
                window.dontShowWarning = false;
                $.ajax({
                    url: "/project/findorgs",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, maxResults: 7 },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Code + " - " + item.Description, value: item.Code + " - " + item.Description, id: item.Code };
                        }));
                    }
                });
            }
        });
               
        $projectCreateForm.find(" #create-button").click(function () {
            window.dontShowWarning = true;
        });

        // ********************************************************************************
        var $projectEditForm = $('#projectEditForm');

        $projectEditForm.submit(function () {
            $projectEditForm.find(' input[type=submit]', this).attr('disabled', 'disabled'); //  turn button off after one click
        });

        $projectEditForm.find(" #Project_ProjectName").addClass("form-control");
        $projectEditForm.find(" #Project_Organisation").addClass("form-control");
        $projectEditForm.find(" :input:not(:button)").css("background-color", "lemonchiffon");
        $projectEditForm.find(".form-group").change(function () {
            window.dontShowWarning = false;
        });
        $projectEditForm.find(' .tabs').button();

        $projectEditForm.find(" #Project_Organisation").autocomplete({
            minLength: 2,            
            delay: 500,
            source: function (request, response) {
                window.dontShowWarning = false;
                $.ajax({
                    url: "/project/findorgs",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, maxResults: 7 },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Code + " - " + item.Description, value: item.Code + " - " + item.Description, id: item.Code };
                        }));
                    }
                });
            }
        });
        
        $projectEditForm.find(" #save-button").click(function () {
            window.dontShowWarning = true;
        });

        // ********************************************************************************

        //  attach a function to the submit button
        $('#submit-contract-changes').click(function () {
            var selected = $('#contracts').getGridParam('selarrrow');
            if (selected.length < 1) {
                bootbox.alert('There must be at least one Contract selected.');
                return false;
            }

            //  AJAX call the Controller
            $.ajax({
                url: "/Project/SaveContracts",
                type: 'post',
                data: { ids: selected },
                async: false
            });
            var projectId = $("#projectId").html();
            window.location.href = '/Project/Details/' + projectId + '?tabNo=4';
            return true;
        });

        // ********************************************************************************
        var $projectIndexForm = $("#projectIndexForm");

        $projectIndexForm.submit(function () {
            var uploadFrom = $projectIndexForm.find(" #UploadFrom").val();
            var earliest = $projectIndexForm.find(" #EarliestDate").val();

            if (uploadFrom == '1/01/0001' || uploadFrom == '' ) {
                $projectIndexForm.find(" #UploadFrom").val(earliest);
            }

            var uploadTo = $projectIndexForm.find(" #UploadTo").val();
            var latest = $projectIndexForm.find(" #LatestDate").val();
            if (uploadTo == '1/01/0001' || uploadTo == '') {
                $projectIndexForm.find(" #UploadTo").val(latest);
            }
        });
        
        // restore the previous grid search if any
        /*
        var $grid = $("#BulletinGrid");
        var $title = $("#gs_Title").val('');
        var $param = $grid.jqGrid('getGridParam');
        $param.postData.filters = '{"groupOp":"AND","rules":[{"field":"Title","op":"bw","data":"' + $title.val() + '"}]}';
        $grid.jqGrid('setGridParam', { "search": "true" }).trigger('reloadGrid');
        */

        
        // ********************************************************************************
        var $projectQuestionsForm = $("#projectQuestionsForm");
        var $projectQuestionsButton = $projectQuestionsForm.find(" #submit-questions-button");
        
        $projectQuestionsForm.find(" :input:not(:button)").css("background-color", "lemonchiffon");

        $projectQuestionsForm.submit(function () {
            window.dontShowWarning = true;
            $projectQuestionsButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
        });

        // initialise/ disable the submit  button
        $projectQuestionsButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
      
        $projectQuestionsForm.find(" .form-group").change(function () {
            window.dontShowWarning = false;

            var fileName = $projectQuestionsForm.find(' #source-file').val();
            if (fileName == undefined || fileName.length == 0) {
                $projectQuestionsButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
            } else {
                $projectQuestionsButton.removeAttr('disabled').toggleClass("ui-state-disabled", false);
            }
        });


        // ********************************************************************************
        var $reviewDetailsDiv = $("#reviewDetailsDiv");
        
        $reviewDetailsDiv.find(' #editForm').submit(function () {
            $reviewDetailsDiv.find(' input[type=submit]', this).attr('disabled', 'disabled');  //  turn button off after one click
        });

        $reviewDetailsDiv.find(' #delete-button').on('click', function (e) {
            e.preventDefault();
            $reviewDetailsDiv.find(' #button').val('delete');
        });

        var nav = $reviewDetailsDiv.find(" #nav").html();
        if (nav == "first")
            $reviewDetailsDiv.find(" #previous-button").attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);

        if (nav == "last")
            $reviewDetailsDiv.find(" #next-button").attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);

        // intialise the UI
        $reviewDetailsDiv.find(' #save-button', this).attr('disabled', 'disabled'); //  turn button off to begin with
        var changesMadeFlag = $reviewDetailsDiv.find(' #ChangesMade').val();
        var changesMade = false;
        if (changesMadeFlag == undefined) {
            changesMade = false;
        }
        else if (changesMadeFlag == 'Y') {
            changesMade = true;
        }
        if ($reviewDetailsDiv.length > 0) {
            if (changesMade) {
                $reviewDetailsDiv.find(' #save-button', this).removeAttr('disabled').toggleClass("ui-state-disabled", false);
                pat.reviewDetails_ManageOutcomeControls(true);
            } else {
                pat.reviewDetails_ManageOutcomeControls(false);
            }
        }
        
        $reviewDetailsDiv.find(" .form-control").change(function () {
            pat.reviewDetails_ManageOutcomeControls(true);
        });

        $reviewDetailsDiv.find(" textarea").keyup(function () {
            pat.reviewDetails_ManageOutcomeControls(true);
        });


        // ********************************************************************************
        var $projectDetailsDiv = $("#projectDetailsDiv");

        var tabNo = $projectDetailsDiv.find(' #tabNo').html();
        var tabName = "tab" + tabNo;
        $projectDetailsDiv.find(' #' + tabName).addClass('active');

        var tabLink = "tabLink" + tabNo;
        $projectDetailsDiv.find(' #' + tabLink).addClass('active');


        // ********************************************************************************
        var $uploadAppendForm = $("#uploadAppendForm");
        var $uploadAppendButton = $uploadAppendForm.find(" #submit-button");

        $uploadAppendForm.find(" :input:not(:button)").css("background-color", "lemonchiffon");

        $uploadAppendForm.submit(function () {
            window.dontShowWarning = true;
            $uploadAppendButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
        });

        // initialise/ disable the button
        $uploadAppendButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);

        $uploadAppendForm.find(" .form-group").change(function () {
            window.dontShowWarning = false;

            var fileName = $uploadAppendForm.find(' #source-file').val();
            if (fileName == undefined || fileName.length == 0) {
                $uploadAppendButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
            } else {
                $uploadAppendButton.removeAttr('disabled').toggleClass("ui-state-disabled", false);
            }
        });


        // ********************************************************************************
        var $uploadCreateForm = $("#uploadCreateForm");
        var $uploadCreateButton = $uploadCreateForm.find(" #submit-button");

        $uploadCreateForm.find(" :input:not(:button)").css("background-color", "lemonchiffon");

        $uploadCreateForm.submit(function () {
            window.dontShowWarning = true;
            $uploadCreateButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
        });

        // initialise/ disable the button
        $uploadCreateButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);

        $uploadCreateForm.find(" .form-group").change(function () {
            window.dontShowWarning = false;

            var fileName = $uploadCreateForm.find(' #source-file').val();
            if (fileName == undefined || fileName.length == 0) {
                $uploadCreateButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
            } else {
                $uploadCreateButton.removeAttr('disabled').toggleClass("ui-state-disabled", false);
            }
        });

        // ********************************************************************************
        var $uploadEditForm = $("#uploadEditForm");
        var $uploadEditButton = $uploadEditForm.find(" #submit-button");

        $uploadEditForm.find(" #SampleName").addClass("form-control");

        $uploadEditForm.find(" :input:not(:button)").css("background-color", "lemonchiffon");

        $uploadEditForm.submit(function () {
            window.dontShowWarning = true;
            $uploadEditButton.attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
        });

        $uploadEditForm.find(" .form-group").change(function () {
            window.dontShowWarning = false;
        });

        $uploadEditForm.find(" #accept-button").click(function() {
            window.dontShowWarning = true;

            bootbox.dialog({
                message: 'Please confirm that you wish to Accept this Sample/Upload',
                title: "Accept Sample/Upload",
                buttons: {
                    yes: {
                        label: "Yes",
                        className: "btn-success",
                        callback: function () {
                            $uploadEditForm.find(' #IsAccepted').val('true');
                            $uploadEditForm.submit();
                        }
                    },
                    no: {
                        label: "No",
                        className: "btn-primary",
                        callback: function () {
                            bootbox.hideAll();
                        }
                    }
                }
            });

            return false;   // use this to block and enforce the display of confirmation as modal dialog 
        });
        

        // ********************************************************************************
        var $uploadCustomiseForm = $("#uploadCustomiseForm");
        var $uploadRefreshButton = $uploadCustomiseForm.find(" #refresh-button");

        $uploadCustomiseForm.find(" :input:not(:button)").css("background-color", "lemonchiffon");

        $uploadCustomiseForm.submit(function () {
            window.dontShowWarning = true;
        });

        $uploadRefreshButton.click(function () {
            $uploadCustomiseForm.submit();
        });

        $uploadCustomiseForm.find(" .form-group").change(function () {
            window.dontShowWarning = false;
        });

        // ********************************************************************************
        var $sampleCreateForm = $("#sampleCreateForm");

        $sampleCreateForm.find(' #SampleStartDate').attr('disabled', 'disabled');

        $sampleCreateForm.submit(function () {
            $sampleCreateForm.find(" #submit-button").attr('disabled', 'disabled').toggleClass("ui-state-disabled", true);
            $sampleCreateForm.find(' input[type=submit]', this).attr('disabled', 'disabled');  //  turn button off after one click
        });

        $sampleCreateForm.find(" :input:not(:button)").css("background-color", "lemonchiffon");

        $sampleCreateForm.find(" #Criteria_Organisation").addClass("form-control");
        $sampleCreateForm.find(" #Criteria_Organisation").autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: "/project/findorgs", type: "POST", dataType: "json",
                    data: { searchText: request.term, maxResults: 7 },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Code + " - " + item.Description, value: item.Code + " - " + item.Description, id: item.Code };
                        }));
                    }
                });
            }
        });

        $sampleCreateForm.find(" #Criteria_Esa").addClass("form-control");
        $sampleCreateForm.find(" #Criteria_Esa").autocomplete({
            minLength: 2,
            delay: 500,
            source: function (request, response) {
                $.ajax({
                    url: "/Project/LookupESA",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, orgCode: $sampleCreateForm.find(" #Criteria_Organisation").val().split('-')[0].trim(), maxResults: 10 },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Value + " - " + item.Text,
                                value: item.Value + " - " + item.Text,
                                id: item.Value
                            };
                        }));
                    }
                });
            }
        });

        $sampleCreateForm.find(" #Criteria_Site").addClass("form-control");
        $sampleCreateForm.find(" #Criteria_Site").autocomplete({
            minLength: 2,
            delay: 500,
            source: function (request, response) {
                $.ajax({
                    url: "/Project/LookupSite",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, orgCode: $sampleCreateForm.find(" #Criteria_Site").val().split('-')[0].trim(), maxResults: 10 },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Value + " - " + item.Text,
                                value: item.Value + " - " + item.Text,
                                id: item.Value
                            };
                        }));
                    }
                });
            }
        });

        $sampleCreateForm.find(" #Criteria_ClaimTypeDescription").addClass("form-control");
        $sampleCreateForm.find(" #Criteria_ClaimTypeDescription").autocomplete({
            delay: 500,
            source: function (request, response) {
                $.ajax({
                    url: "/Sample/LookupClaimType",
                    type: "POST",
                    dataType: "json",
                    data: { searchText: request.term, orgCode: $sampleCreateForm.find(" #Criteria_ClaimTypeDescription").val().split('-')[0].trim(), maxResults: 10 },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Value + " - " + item.Text,
                                value: item.Value + " - " + item.Text,
                                id: item.Value
                            };
                        }));
                    }
                });
            }
        });

        // ********************************************************************************
        var $reviewEditForm = $("#reviewEditForm");

        $reviewEditForm.find("#delete-button").click(function () {
            var msg = $('#review-delete-message').html();

            bootbox.dialog({
                message: msg,
                title: "Delete Review",
                buttons: {
                    yes: {
                        label: "Yes",
                        className: "btn-success",
                        callback: function () {
                            $reviewEditForm.submit();
                        }
                    },
                    no: {
                        label: "No",
                        className: "btn-primary",
                        callback: function () {
                            bootbox.hideAll();
                        }
                    }
                }
            });

            return false; // use this to block and enforce the display of confirmation as modal dialog 
        });

        $reviewEditForm.submit(function () {
            $reviewEditForm.find(' input[type=submit]', this).attr('disabled', 'disabled'); //  turn button off after one click
        });

        $reviewEditForm.find(" #Review_ClaimRecoveryAmount").addClass("form-control");

        // set initialise/ disable the form
        if ($reviewEditForm.length > 0) {
            $reviewEditForm.find(' #save-button', this).attr('disabled', 'disabled'); //  turn button off to begin with
            pat.reviewEdit_ManageOutcomeControls(false);
        }

        $reviewEditForm.find(" .form-control").change(function () {
            pat.reviewEdit_ManageOutcomeControls(true);
        });
        $reviewEditForm.find(" textarea").keyup(function () {
            pat.reviewEdit_ManageOutcomeControls(true);
        });

        // ********************************************************************************
        var $sampleClaimListDiv = $("#sampleClaimListDiv");

        $sampleClaimListDiv.submit(function () {
            $sampleClaimListDiv.find(' input[type=submit]', this).attr('disabled', 'disabled'); //  turn button off after one click
        });

        //  attach a function to the submit sample button
        $sampleClaimListDiv.find(' #submit-sample').click(function () {
            var selected = $sampleClaimListDiv.find(' #sample').getGridParam('selarrrow');
            if (selected.length == 0) {
                bootbox.alert('There are no claims selected.');
                return false;
            }

            var additionalReview = $sampleClaimListDiv.find(" #Additional")[0].checked;
            var outOfScopeReview = $sampleClaimListDiv.find(" #OutOfScope")[0].checked;
            var dueDate = $sampleClaimListDiv.find(" #DueDate").html();
            if (additionalReview && outOfScopeReview) {
                bootbox.alert("A Sample cannot be both Additional and Out of Scope");
                return false;
            } else {
                //disableButton();
                //  AJAX call the Controller
                $.ajax({
                    url: "/Sample/SaveSample",
                    type: 'post',
                    data: { ids: selected, additional: additionalReview, outOfScope: outOfScopeReview, due: dueDate },
                    async: false
                });
            }
            return true;
        });

        //  attach a function to the add more button
        $sampleClaimListDiv.find(' #addmore-button').click(function () {
            var selected = $sampleClaimListDiv.find(' #sample').getGridParam('selarrrow');

            //  AJAX call the Controller
            $.ajax({
                url: "/Sample/AddMore",
                type: 'post',
                data: { ids: selected },
                async: false
            });
            return true;
        });

        // ********************************************************************************
        var $uploadDetailsDiv = $("#uploadDetailsDiv");

        var myMarg = $uploadDetailsDiv.find(" #myMargin").html();
        $uploadDetailsDiv.find(" .container").css("margin", myMarg);

        //  attach a function to the edit button
        $uploadDetailsDiv.find(' #bulk-update').click(function () {
            var selected = $uploadDetailsDiv.find(' #reviews').getGridParam('selarrrow');
            if (selected.length == 0) {
                bootbox.alert("There are no reviews selected.");
                return false;
            }
            if (selected.length == 1) {
                window.location.href = '/Review/Edit/' + selected[0];
                return true;
            }

            //  AJAX call the Controller to save the selections
            $.ajax({
                url: "/Upload/SaveSelections",
                type: 'post',
                data: { ids: selected },
                async: false
            });
            //var projectId = $("#projectId").html();
            window.location.href = '/Review/Details/' + selected[0];
            return true;
        });

        //  attach a function to the edit checklist button
        $uploadDetailsDiv.find(' #edit-checklist').click(function () {
            var selected = $uploadDetailsDiv.find(' #reviews').getGridParam('selarrrow');
            if (selected.length == 0) {
                bootbox.alert("There are no reviews selected.");
                return false;
            }

            //  AJAX call the Controller to save the selections
            $.ajax({
                url: "/Upload/SaveSelections",
                type: 'post',
                data: { ids: selected },
                async: false
            });
            window.location.href = '/CheckList/Edit/' + selected[0];
            return true;
        });

        $uploadDetailsDiv.find(' #delete-sample').click(function () {
            window.dontShowWarning = true;

            var msg = "This will remove the entire Upload/Sample with all the Reviews it contains.";

            bootbox.dialog({
                message: msg,
                title: "Delete Reviews",
                buttons: {
                    yes: {
                        label: "Yes",
                        className: "btn-success",
                        callback: function () {
                            var uploadId = $("#uploadId").html();
                            window.location.href = '/Upload/Delete/' + uploadId;
                            return true;
                        }
                    },
                    no: {
                        label: "No",
                        className: "btn-primary",
                        callback: function () {
                            bootbox.hideAll();
                        }
                    }
                }
            });
        });

        // ********************************************************************************








    }
);