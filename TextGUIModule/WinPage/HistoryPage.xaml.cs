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
using TextGUIModule;

namespace CoDEmpare.WinPage
{
    /// <summary>
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : UserControl
    {
        private DataBaseLite _dataBase;
        public HistoryPage()
        {
            InitializeComponent();
        }

        private void UpdateHistoryList()
        {
            _dataBase = new DataBaseLite();
            List<string> listHistory = _dataBase.GetListHistory();
            FileListCompil.Items.Clear();
            foreach (var desc in listHistory)
            {
                FileListCompil.Items.Add(desc);
            }
        }
    }
}
