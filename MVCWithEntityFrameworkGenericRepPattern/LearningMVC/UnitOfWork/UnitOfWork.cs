using System;
using LearningMVC.GenericRepository;

namespace LearningMVC.UnitOfWork
{
   public class UnitOfWork : IDisposable
   {
      private readonly MVCEntities context = new MVCEntities();  //  wraps the EF dbContext which has all the entities

      private GenericRepository<User> userRepository;

      public GenericRepository<User> UserRepository
      {
         get { return userRepository ?? ( userRepository = new GenericRepository<User>( context ) ); }
      }

      public void Save()
      {
         context.SaveChanges();
      }

      private bool disposed = false;

      protected virtual void Dispose( bool disposing )
      {
         if (! disposed)
         {
            if (disposing)
               context.Dispose();
         }
         disposed = true;
      }

      public void Dispose()
      {
         Dispose( true );
         GC.SuppressFinalize( this );
      }
   }
}