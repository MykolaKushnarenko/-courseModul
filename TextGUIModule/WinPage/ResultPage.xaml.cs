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
        public string MainCode { get; set; } = "";
        public string ChaildCode { get; set; } = "";
        public readonly Analysis code;
        public ResultPage()
        {
            InitializeComponent();
            SetTextBoxes();
        }

        private void SetTextBoxes()
        {
            MainCodeText.Text = MainCode;
            ChildCodeText.Text = ChaildCode;
        }

        public void SetListCompare(ref DataBaseLite db)
        {
            db.SetCodeMain(db.IdMainFileForHist, code);
            db.SetCodeChild(db.IdiDenticalFie, code);
            Compare(ref db);
        }
        private void Compare(ref DataBaseLite data)
        {
            double resVarnFish = Double.Parse(String.Format("{0:0.##}", code.ResultAlgorithm(1)));
            double resVShiling = Double.Parse(String.Format("{0:0.##}", code.ResultAlgorithm(2)));
            double resHeskel = Double.Parse(String.Format("{0:0.##}", code.ResultAlgorithm(0)));
            ResultCompareList.Items.Add("Levenshtein Distance :" + resVarnFish + "%");
            ResultCompareList.Items.Add("WShiling :" + resVShiling + "%");
            ResultCompareList.Items.Add("Heskel :" + resHeskel + "%");
            data.AddingHistiry(resVarnFish, resVShiling, resHeskel);
        }
    }
}
