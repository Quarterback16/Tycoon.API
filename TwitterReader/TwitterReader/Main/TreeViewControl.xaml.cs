using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TwitterReader
{    
    public partial class TreeViewControl : UserControl
    {
        public TreeViewControl()
        {
            InitializeComponent();

            GroupTreeView.SelectedItemChanged += TreeViewControl_SelectedItemChanged;
        }

        private void TreeViewControl_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {           
            var userModel = (sender as TreeView)?.SelectedItem as UserModel;
            if (userModel != null)
            {
                (DataContext as MainViewModel)?.ExecuteSelectCommand(userModel);
            }
        }        
    }
}
