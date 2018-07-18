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
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private DataBaseLite _dataBase;
        public RegistrationWindow(DataBaseLite data)
        {
            _dataBase = data;
            InitializeComponent();
        }

        private void SinglUp_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (Password.Password == PasswordSecond.Password && Password.Password.Replace(" ", "") != ""
                && Name.Text.Replace(" ", "") != "" && Email.Text.Replace(" ", "") != "")
            {
                
                _dataBase.RegistsAccount(Name.Text, Email.Text, Password.Password);
                MessageBox.Show("OK!", "Result");
                this.Close();
            }
            else
            {
                Error.Visibility = Visibility.Visible;
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
