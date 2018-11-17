using System;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace CastleIoc
{
    class Program
    {
        static void Main( string[] args )
        {
            var ioc = new WindsorContainer();

            ioc.Install(new AppNameInstaller());

            var shopper = ioc.Resolve<Shopper>();
            shopper.Charge();
            Console.WriteLine(shopper.ChargesForCurrentCard);

            var shopper2 = ioc.Resolve<Shopper>();
            shopper2.Charge();
            Console.WriteLine( shopper2.ChargesForCurrentCard );

            Console.Read();
        }
    }

    internal class AppNameInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer ioc, IConfigurationStore store)
        {
            ioc.Register( Component.For<Shopper>()
                .LifeStyle.Transient );  // to alter the singleton which is the default!

            ioc.Register( Component.For<ICreditCard>().ImplementedBy<Visa>().Named( "defaultCard" )
                .LifeStyle.Transient );

            //  by using name we can get the right one not just the first one
            ioc.Register( Component.For<ICreditCard>().ImplementedBy<MasterCard>().Named( "secondaryCard" ) );  
        }
    }

    public class Visa : ICreditCard
    {
        public string Charge()
        {
            return "Visa...Visa";
        }

        public int ChargeCount
        {
            get { return 0; }
        }
    }

    public class MasterCard : ICreditCard
    {
        public string Charge()
        {
            return "MC...MC";
        }

        public int ChargeCount
        {
            get { return 0; }
        }
    }

    internal class Shopper
    {
        private readonly ICreditCard creditCard;

        public Shopper( ICreditCard creditCard)
        {
            this.creditCard = creditCard;
        }

        public void Charge()
        {
            Console.WriteLine(creditCard.Charge());
        }

        public int ChargesForCurrentCard
        {
            get { return creditCard.ChargeCount; }
        }
    }

    internal interface ICreditCard
    {
        string Charge();
        int ChargeCount { get; }
    }
    
}
