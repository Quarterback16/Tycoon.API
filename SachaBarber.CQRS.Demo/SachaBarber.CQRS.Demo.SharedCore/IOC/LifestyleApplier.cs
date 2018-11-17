using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Registration.Lifestyle;


namespace SachaBarber.CQRS.Demo.SharedCore.IOC
{
    public interface ILifestyleApplier
    {
        ComponentRegistration<T> ApplyLifeStyle<T>(LifestyleGroup<T> component) where T : class;
    }

    public class WcfLifestyleApplier : ILifestyleApplier
    {
        public ComponentRegistration<T> ApplyLifeStyle<T>(LifestyleGroup<T> component) where T : class
        {
            return component.PerWcfSession();
        }
    }

    public class TransientLifestyleApplier : ILifestyleApplier
    {
        public ComponentRegistration<T> ApplyLifeStyle<T>(LifestyleGroup<T> component) where T : class
        {
            return component.Transient;
        }
    }
}
