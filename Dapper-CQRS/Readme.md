## Dapper CQRS ##

Command Query Responsibility Segregation (CQRS) is a pattern that separates domain logic 
into single responsibility classes. With CQRS we command our domain to perform a state 
mutating action, or query our domain to retrieve a value. It is a great pattern that 
makes enforcing the single responsibility principle and separation of concerns trivial.

CQRS has pretty much replaced the repository pattern in most of my latest projects. 
I've found the trade off of writing the minimal boiler plate code in each command/query 
is well worth the straight forward nature of the handlers versus their often god-class 
repository counter parts.

### CQRS Handlers ###

Handlers are the do'ers of this pattern. They should perform one action per class, 
otherwise we quickly turn into a repository pattern with an identity crisis. To help 
keep in line with SRP we follow the interface segregation principle and end up with 
two interfaces: ICommand and IQuery.

 - The ICommand interface will handle data manipulation.
 - The IQuery<T> interface is our data selector.

You might have noticed that both Execute methods accept ISession, remnants of NHibernate 
before I swapped in dapper. I make use of ISession now with a minimal unit of work 
implementation providing access to a custom IDapperContext.

### Dapper Context ###

A context class helps manage the connection and transaction boilerplate code. 
Without this class each of our commands/queries would require nested using statements 
and connection/transaction management. This context currently only handles the most 
basic scenario so if you're wanting reads outside a transaction or to run multiple 
queries per transaction you'll need to add some oomph.

### Simple queries ###

This query handler is so simplistic that it almost seems like a lot of hassle to go 
through the whole pattern, but honestly, after months of usage I fell in love with 
its straight forwardness and cannot stand the repository pattern anymore.

The GetAllAnimals query handler does not accept any parameters, it simply returns 
a list of all animals, how straightforward and simplistic can you get?

Calling the handler looks something like:

`var animals = _database.Query(new GetAllAnimals());


### A Simple Command ###

This command example shows how parameters are accepted and used. The only time this 
CQRS pattern accepts parameters is during instantiation. This makes it easy to manage 
state and not break the principle of least astonishment.

public class SaveAnimal : ICommand
{
    private readonly Animal _animal;

    public SaveAnimal(Animal animal)
    {
        _animal = animal;
    }

    public void Execute(ISession session)
    {
        if (_animal.Id > 0)
        {
            session.Execute("UPDATE Animals SET Name = @@Name, CommonName = @@CommonName WHERE Id = @@Id", new { _animal.Id, _animal.Name, _animal.CommonName });
            return;
        }
                
        session.Execute("INSERT INTO Animals (Name, CommonName) VALUES (@@Name, @@CommonName)", new { _animal.Name, _animal.CommonName });
    }
}

You'll notice a bit of logic happening in the Execute method, perfectly acceptable! 
Normally I would make this a merge statement and this wouldn't be necessary, but 
were striving for ease in this one! Just remember to keep business logic in these 
handlers to a minimum; you created a service layer for that, right?



