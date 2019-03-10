![Logo of the project](https://raw.githubusercontent.com/jehna/readme-best-practices/master/sample-logo.png)

# EVENT STORE - LEARNING
> A learning play area to discover what Event Store (http://geteventstore.com/)
  can do and how it does it.

Sample code for interfacing with Greg Young's Event Store.

Need to run the EventStore first:-

  EventStore.ClusterNode.exe --db ./db --log ./logs

You can use the .NET API client to write and read events.

In my world, Catch up subscriptions could process all the events and create Projections

EventStore can be shutdown via the Admin UI http://127.0.0.1:2113

## Development approach



### Commands so far 
 1. Writing an event to Event Store via curl

 curl -i -d "@event.json" "http://127.0.0.1:2113/streams/newstream" -H "Content-Type:application/vnd.eventstore.events+json"

### Building

 1. Make sure all the test pass
 1. Build with mode set to Release

### Deploying / Publishing

 1. shut down the app if running in Prod
 1. xcopy deploy to Prod
 1. run it

## Features

 1. 

## Dependencies

 1. NUGET: EventStore.ClientAPI.NetCore -Version 4.1.0.23

## Configuration

Here you should write what are all of the configurations a user can enter when
using the project.