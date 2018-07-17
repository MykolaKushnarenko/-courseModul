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
using System.Windows.Shapes;
using TextGUIModule;

namespace CoDEmpare.WinPage
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private Action visibleMainWindow;
        private DataBaseLite _dataBase;
        public AuthorizationWindow(Action method, DataBaseLite data)
        {
            _dataBase = data;
            InitializeComponent();
            visibleMainWindow += method;
        }

        private void SkipButton_OnClick(object sender, RoutedEventArgs e)
        {
            visibleMainWindow();
            this.Close();
        }

        private void SinglInButtn_OnClick(object sender, RoutedEventArgs e)
        {
            new RegistrationWindow(_dataBase).ShowDialog();
        }
    }
}
