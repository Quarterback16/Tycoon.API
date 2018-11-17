Real-World CQRS/ES with ASP.NET and Redis Demo Project
====

Blog posts:

1. [Overview](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-1-overview/)
2. [The Write Model](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-2-the-write-model/)
3. [The Read Model](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-3-the-read-model/)
4. [Creating the APIs](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-4-creating-the-apis/)
5. [Running the APIs](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-5-running-the-apis/)

 1. Build
 2. Run Initializer to load some seed data
 3. Set the startup project to Commands and run
  - ignore the IE error The resource cannot be found. (If we had swagger ...)
 4. BOTH apis will run (see IISExpress in the system tray)
 5. When POST ing with Postman use the JSON header and data like this (u might have to scroll)
  - {"EmployeeID": "1","FirstName": "John","LastName": "Smith",	"DateOfBirth": "12/1/1983",	"JobTitle": "General Manager","LocationID":"1"}
  - {  "Message": "The request is invalid.",  "Errors": {    "request.EmployeeID": [      "An Employee with this ID already exists."    ]  }
}
 6. Data is stored in a Redis database
  - http://localhost:62291/locations/all