![Logo of the project](https://raw.githubusercontent.com/jehna/readme-best-practices/master/sample-logo.png)

# HS Event Store
> In Memory Event Store

Create a class that functions as an event sourcing Event Store.

## Development approach

Develop features on a Use Case basis employing TDD.

See the architecture decisions in the Doco folder for further information.

Basically the development process is one of designing Commands and testing them

### Commands so far 
 1. CreateSeason; done
 2. RegisterTeam; in progress

### Building

 1. Make sure all the test pass
 1. Build with mode set to Release

### Deploying / Publishing

 1. shut down the app if running in Prod
 1. xcopy deploy to Prod
 1. run it

## Features

 1. Load Season - will be a way to intialise a season with teams that have their
 positions initally assigned.

## Configuration

Here you should write what are all of the configurations a user can enter when
using the project.