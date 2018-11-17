### This is an example of a CQRS system combined with event sourcing ###

See also http://localhost/HomeWiki/ow.asp?ProjectCqrs

This solution needs to be running in Admin mode for the communication preocesses.
 - it has multiple start up projects the Domain and the WPF client

Starts at Program.cs
 1. OrderService is started
 2. Service will continue to wait for events until it is cancelled
 3. First thing the Service runner does is install the Domain which registers all the classes with 
    the ioc container
 4. The Read Model repository then creates the database (Raven DB)
 5. The client loads all the items using the repository and displays them


 The client communicates with the service by sending queries and commands.
 An Order create command will be sent to a command handler which will create a new
 object and commit it using a session.

 The session object implements ISession and uses the Repository to save the data.


