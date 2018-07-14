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
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : UserControl
    {
        private readonly string _mainCode;
        private readonly string _chaildCode;
        private readonly DataBaseLite _dataBase;
        public ResultPage(string mainCode, string chaildCode,ref DataBaseLite data)
        {
            _mainCode = mainCode;
            _chaildCode = chaildCode;
            _dataBase = data;
            InitializeComponent();
            SetTextBoxes();
            SetListCompare();
        }

        private void SetTextBoxes()
        {
            MainCodeText.Text = _mainCode;
            ChildCodeText.Text = _chaildCode;
        }

        private void SetListCompare()
        {
            _dataBase.SetCodeMain(_dataBase.IdMainFileForHist);
            _dataBase.SetCodeChild(_dataBase.IdiDenticalFie);
            Compare();
        }
        private void Compare()
        {
            double resVarnFish = Double.Parse(String.Format("{0:0.##}", _dataBase.Code.ResultAlgorithm(1)));
            double resVShiling = Double.Parse(String.Format("{0:0.##}", _dataBase.Code.ResultAlgorithm(2)));
            double resHeskel = Double.Parse(String.Format("{0:0.##}", _dataBase.Code.ResultAlgorithm(0)));
            ResultCompareList.Items.Add("Levenshtein Distance :" + resVarnFish + "%");
            ResultCompareList.Items.Add("WShiling :" + resVShiling + "%");
            ResultCompareList.Items.Add("Heskel :" + resHeskel + "%");
            _dataBase.AddingHistiry(resVarnFish, resVShiling, resHeskel);
        }
    }
}
