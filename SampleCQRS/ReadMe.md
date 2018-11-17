### Simple CQRS ###

Downloaded from https://github.com/exceptionnotfound/SampleCQRS

### Concepts ###

#### Domain Objects ####
 Classes that represent how our data is modeled on the _read_ side.
 - Movie
 - Review

#### Commands ####
 Classes that represent commands made by the end user, that will be processed 
 by the application.

#### Command Handlers ####
 Classes that will interpret commands and kick off corresponding Events.

#### Events ####
 Classes that represent changes made to the data in the data store, and are 
 kicked off by Commands. These classes are serialized into the Event Store. 
 One Command may kick off many Events.

#### Event Handlers ####
 Classes that interpret the Events and store them into the Event Store.

#### Command Bus ####
 A class that represents the queue for the Commands. Commands placed on the 
 bus are processed by one or more appropriate Command Handlers.

#### Event Bus ####
 A class that represents the queue for the Events. Events placed on this bus 
 are processed by one or more appropriate Event Handlers.

#### Event Store ####
 The place where the Events are stored. For our application, this will be a 
 SQL database.

#### Command Interface ####
 The application which accepts Commands. For our solution, this will be an
 ASP.NET Web API application.

#### Query Interface ####
 The application which accepts Queries. Can be the same application that has 
 the Command Interface. For our solution, this will be an ASP.NET Web API 
 application.

### Dependencies ###
 - https://github.com/tyronegroves/SimpleCQRS
