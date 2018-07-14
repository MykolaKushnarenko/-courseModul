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
    /// Interaction logic for DataBasesSubmitList.xaml
    /// </summary>
    public partial class DataBasesSubmitList : UserControl
    {
        public Action<DataBaseLite, bool> _resultMethod;
        private DataBaseLite _dataBase;

        public DataBasesSubmitList(DataBaseLite data, Action<DataBaseLite, bool> method)
        {
            _dataBase = data;
            _resultMethod = method;
            InitializeComponent();
            LoadListSubmit();
        }

        private void LoadListSubmit()
        {
            List<string> listSubm = _dataBase.DescSubm();
            foreach (string submit in listSubm)
            {
                SubmitList.Items.Add(submit);
            }
        }

        private void SubmitList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string wordFromItemsList = SubmitList.SelectedValue.ToString();
            string[] get = wordFromItemsList?.Split(new char[] { '|' });
            _dataBase.SearchIn(get[get.Length - 1]);
            _resultMethod(_dataBase, true);
            

        }
    }
}
