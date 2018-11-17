using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using HearthCap.Data;
using HearthCap.Features.Core;
using HearthCap.Framework;
using HearthCap.Shell.Tabs;

namespace HearthCap.Features.Dashboard
{
    // [Export(typeof(ITab))]
    public class DashboardViewModel : TabViewModel
    {
        private readonly Func<HearthStatsDbContext> dbContext;

        private string header;

        private BindableCollection<Hero> heroes;

        [ImportingConstructor]
        public DashboardViewModel(Func<HearthStatsDbContext> dbContext)
        {
            this.dbContext = dbContext;
            DisplayName = "Dashboard";
            Header = "Dashboard!";
            Order = 0;
        }

        [Import]
        public IBusyWatcher Busy { get; set; }

        [Import]
        public GlobalData GlobalData { get; set; }

        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                NotifyOfPropertyChange(() => Header);
            }
        }

        /// <summary>
        ///     Called when initializing.
        /// </summary>
        protected override async void OnInitialize()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var data = await GlobalData.GetAsync();
            heroes = new BindableCollection<Hero>(data.Heroes);
        }

        public IObservableCollection<Hero> Heroes
        {
            get { return heroes; }
        }
    }
}
