using System.ServiceProcess;

namespace SachaBarber.CQRS.Demo.Orders.Domain.Host
{
    partial class Service : ServiceBase
    {
        private OrderServiceRunner orderServiceRunner;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.orderServiceRunner = new OrderServiceRunner();
            this.orderServiceRunner.Start();
        }

        protected override void OnStop()
        {
            if (this.orderServiceRunner != null)
                this.orderServiceRunner.Stop();
        }
    }
}
