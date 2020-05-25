![Logo of the project](https://raw.githubusercontent.com/jehna/readme-best-practices/master/sample-logo.png)

# SeasonHtml
> Generate the annual static menu of HTML reports 

Produce a nfl-yyyy.htm file that links to all the static NFL reports.


## Development approach

Develop features on a Use Case basis employing TDD.

See the architecture decisions in the Doco folder for further information.

Basically the development process is one of designing Commands and testing them

### Building

 1. Make sure all the test pass
 1. Build with mode set to Release

### Deploying / Publishing

 1. Run the Test
 1. xcopy the htm file to W:\medialists\dropbox\gridstat\html\
 1. browse it


## Configuration

The year (season) is passed into the constructor.  On a new season you need to manually update 
the W: drive menu