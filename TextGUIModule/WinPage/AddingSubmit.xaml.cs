using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using TextGUIModule;

namespace CoDEmpare.WinPage
{
    /// <summary>
    /// Interaction logic for AddingSubmit.xaml
    /// </summary>
    public partial class AddingSubmit : UserControl
    {
        private Action<DataBaseLite> _swichToResutl;
        readonly DataBaseLite _dataBase;
        private string _path;
        private bool _search;
        public AddingSubmit(DataBaseLite data, Action<DataBaseLite> methodResult, bool isSearch)
        {
           _swichToResutl = methodResult;
            _dataBase = data;
            _search = isSearch;
            InitializeComponent();
            PrintCompilName(CsharpLanguage.Content.ToString());
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            string name = button.Name;
            switch (name)
            {
                case "CsharpLanguage":
                    JavaLanguage.IsChecked = false;
                    CppLanguage.IsChecked = false;
                    CompilName.Items.Clear();
                    PrintCompilName(CsharpLanguage.Content.ToString());
                    break;
                case "JavaLanguage":
                    CsharpLanguage.IsChecked = false;
                    CppLanguage.IsChecked = false;
                    CompilName.Items.Clear();
                    PrintCompilName(JavaLanguage.Content.ToString());
                    break;
                case "CppLanguage":
                    CsharpLanguage.IsChecked = false;
                    JavaLanguage.IsChecked = false;
                    CompilName.Items.Clear();
                    PrintCompilName(CppLanguage.Content.ToString());
                    break;
            }
        }

        private void PrintCompilName(string language)
        {
            List<string> typeCompils = _dataBase.GetCompile(language);
            foreach (var typeCompil in typeCompils)
            {
                CompilName.Items.Add(typeCompil);
            }
        }
        private void AddFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog
            {
                Filter = "Исходные коды(*.cs;*.java;*.cpp;*.c)|*.cs;*.java;*.cpp;*.c" + "|Все файлы (*.*)|*.* ",
                CheckFileExists = true,
                Multiselect = true
            };
            if (myDialog.ShowDialog() == true)
            {
                _path = myDialog.FileName;
            }

            if (_path != "" && CompilName.SelectedIndex > -1)
            {

                LoadFileToBD.Visibility = Visibility.Visible;
            }
        }

        private  void LoadFileToBD_OnClick(object sender, RoutedEventArgs e)
        {
            string typeCompiler = (string)CompilName.SelectedItem;
            string lang = "";
            if (CsharpLanguage.IsChecked == true)
            {
                lang = (string)CsharpLanguage.Content;
            }
            else if (CppLanguage.IsChecked == true)
            {
                lang = (string)CppLanguage.Content;
            }
            else if (JavaLanguage.IsChecked == true)
            {
                lang = (string)JavaLanguage.Content;
            }
            LoadWindow load = new LoadWindow(_dataBase, NameAuthor.Text, Description.Text, typeCompiler, _path, _search);
            load.ShowDialog();
            if (_search && _dataBase.IsNotEnpty())
            {
                _swichToResutl(_dataBase);
            }
            else
            {
                CsharpLanguage.IsChecked = true;
                NameAuthor.Text = "Имя и Фамилия";
                Description.Text = "Краткое описание";
            }
        }
    }
}
