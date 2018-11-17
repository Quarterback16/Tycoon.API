//  Client Side representation of the data
var model = {
    //  to keep track of which part of the client interface is shown to the user.
    view: ko.observable("welcome"),
    rsvp: {
        name: ko.observable(""),  // auto update on change
        email: "",
        willattend: ko.observable("true")
    },
    //  an array of the names of the other attendees, which are obtained from the web
    //  service after the user has responded to the invitation
    attendees: ko.observableArray([])  // observable array
}

///
///  The showForm, sendRsvp, and getAttendees functions collectively form the client-side controller.
///

//  This function shows the user the HTML form that gathers their responses to the invitation and
//  allows the response to be sent to the web service.
var showForm = function () {
    model.view("form");
}

//  This function sends a POST request to the web service to submit the RSVP data.
var sendRsvp = function () {
    $.ajax("/api/rsvp", {
        type: "POST",
        data: {
            name: model.rsvp.name(),
            email: model.rsvp.email,
            willattend: model.rsvp.willattend()
        },
        success: function () {
            getAttendees();
        }
    });
}

//  This function sends a GET request to the web service to get the list of attendees, and it is called
//  after a successful POST request.
var getAttendees = function () {
    $.ajax("/api/rsvp", {
        type: "GET",
        success: function (data) {
            model.attendees.removeAll();
            model.attendees.push.apply(model.attendees, data.map(function (rsvp) {
                return rsvp.Name;
            }));
            model.view("thanks");
        }
    });
}

$(document).ready(function () {
    ko.applyBindings();  //  Knockout initialisation
})

