![Logo of the project](https://raw.githubusercontent.com/jehna/readme-best-practices/master/sample-logo.png)

# Gerard.HostServer
> Command Processor

A microservice that reads commands off an MSMQ queue in catchup mode and 
feeds them to command handlers who do various tasks.

Fed by the Internet Scanner service AND Gerard the butler.
 

## Development approach

Develop features on a Use Case basis employing TDD.

See the architecture decisions in the Doco folder for further information.

Basically the development process is one of designing Commands and testing them.

Use MSMQ Commander to see what is in the queue.
  * @ 2019-03-07 there are 40 DataFixCommands in gerard-work
  * @ 2019-03-07 there are 32 NewsArticleCommands in shuttle-work

### Commands so far 
 1. NewsArticleCommand; in progress
 2. DataFixCommand; in progress

### Building

 1. Make sure all the test pass
 1. Build with mode set to Release

### Deploying / Publishing

 1. shut down the app if running in Prod
 1. xcopy deploy to Prod
 1. run it

## Features

 1. Examine articles and if relevant update TFL database

## Configuration

App.config file has
  1. the name of the input queue
  2. log configuration

## Run and Operating Instructions
