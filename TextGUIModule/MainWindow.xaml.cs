using System;
using System.Collections.Generic;
using System.IO;
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
using System.Data;
using System.Data.SQLite;
using Microsoft.Win32;
using winForms = System.Windows.Forms;

namespace TextGUIModule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = "";
        private Analitics code;
        private DataBaseLite db;
        public MainWindow()
        {
            InitializeComponent();
            db = new DataBaseLite();
            textCode.FontFamily = new FontFamily("8693.ttf");
            textWrite("icons/Test2.cs", textCode);
            textWrite("icons/Test4.java", textCode2);
            code = new Analitics();
        }

        private void textWrite(string path, TextBox text)
        {
            using (StreamReader rw = new StreamReader(path))
            {
                while (!rw.EndOfStream)
                {
                    text.Text += rw.ReadLine();
                    text.Text += "\r\n";
                }
            }
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //page.SelectedIndex = 1;
            FineFile();
        }

        private void FineFile()
        {
            string[] allFoundFiles;
            using (var fbd = new winForms.FolderBrowserDialog())
            {
                winForms.DialogResult result = fbd.ShowDialog();

                if (result == winForms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    allFoundFiles = Directory.GetFiles(fbd.SelectedPath, "*.cs", SearchOption.AllDirectories);
                    foreach (string file in allFoundFiles)
                    {
                        string[] a = file.Split('\\');
                        FileList.Items.Add(a[a.Length-1]);
                    }
                }
            }

           
           page.SelectedIndex = 1;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                page.SelectedIndex = 0;
            }
        }

        private void LoadFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Исходные коды(*.cs;*.java;*.cpp;*.c)|*.cs;*.java;*.cpp;*.c" + "|Все файлы (*.*)|*.* ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                path = myDialog.FileName;
            }

            if (path != "" && FileListCompil.SelectedIndex > -1)
            {

                Complite.Visibility = Visibility.Visible;
            }
            
        }

        private void NewSubmit_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            page.SelectedIndex = 2;
            Complite.Visibility = Visibility.Hidden;
            StatsComple((string)charpRButton.Content);
        }

        private void Complite_OnClick(object sender, RoutedEventArgs e)
        {
            string TypeCompiler = (string)FileListCompil.SelectedItem;
            string lang = "";
            if (charpRButton.IsChecked==true)
            {
                lang = (string)charpRButton.Content;
            }
            else if (cppRButton.IsChecked == true)
            {
                lang = (string)cppRButton.Content;
            }
            else if (javaRButton.IsChecked == true)
            {
                lang = (string)javaRButton.Content;
            }
            else if (cRButton.IsChecked == true)
            {
                lang = (string)cRButton.Content;
            }
            db.AddingSubmit(NameBox.Text,BoxDesc.Text, TypeCompiler,path);
            MessageBox.Show("Complite!", "EvolPras", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CharpRButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            FileListCompil.Items.Clear();
            StatsComple((string) charpRButton.Content);
        }

        private void StatsComple(string langName)
        {
            List<string> typeCompil = db.GetCompile(langName);
            foreach (var type in typeCompil)
            {
                FileListCompil.Items.Add(type);
            }
        }

        private void JavaRButton_OnClick(object sender, RoutedEventArgs e)
        {
            FileListCompil.Items.Clear();
            StatsComple((string)javaRButton.Content);
        }

        private void CppRButton_OnClick(object sender, RoutedEventArgs e)
        {
            FileListCompil.Items.Clear();
            StatsComple((string)cppRButton.Content);
        }

        private void CRButton_OnClick(object sender, RoutedEventArgs e)
        {
            FileListCompil.Items.Clear();
            StatsComple((string)cRButton.Content);
        }
    }
}
