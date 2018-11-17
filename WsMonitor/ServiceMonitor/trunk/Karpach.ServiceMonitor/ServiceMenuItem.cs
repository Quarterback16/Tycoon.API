using System.ServiceProcess;
using System.Windows.Forms;

namespace Karpach.ServiceMonitor
{
    public class ServiceMenuItem : ToolStripMenuItem
    {
        public ServiceMenuItem(string text) : base(text)
        {
        }
        public ServiceMonitor Service;
        public ServiceControllerStatus Status = ServiceControllerStatus.Stopped;
    }
}