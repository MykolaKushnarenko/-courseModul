using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        private bool mainCodeIs = true;
        private bool childCodeIs = true;
        private bool searchFileNow = false;
        private bool searchSubmitNow = false;
        public MainWindow()
        {
            InitializeComponent();
            db = new DataBaseLite();
            textCode.FontFamily = new FontFamily("8693.ttf");
            //textWrite("icons/Test2.cs", textCode);
            TextWrite("icons/Test4.java", textCode2);
            code = new Analitics();
        }

        private void TextWrite(string path, TextBox text)
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
            //string[] allFoundFiles;
            //using (var fbd = new winForms.FolderBrowserDialog())
            //{
            //    winForms.DialogResult result = fbd.ShowDialog();

            //    if (result == winForms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //    {
            //        allFoundFiles = Directory.GetFiles(fbd.SelectedPath, "*.cs", SearchOption.AllDirectories);
            //        foreach (string file in allFoundFiles)
            //        {
            //            string[] a = file.Split('\\');
            //            FileList.Items.Add(a[a.Length-1]);
            //        }
            //    }
            //}

            List<string> s = db.DescSubm();
            FileList.Items.Clear();
            foreach (var desc in s)
            {
                ListViewItem item = new ListViewItem {Content = desc};
                FileList.Items.Add(item);
            }
           
           page.SelectedIndex = 1;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            var item = sender as ListViewItem;
            string content = item.Content.ToString();
            string[] get = content.Split(new char[] {'|'});
            if (item != null && item.IsSelected)
            {
                if (mainCodeIs)
                {

                    textCode.Text = db.GetOrignCode(get[get.Length - 1]);
                    db.SetCodeMain(get[get.Length - 1], code);
                    mainCodeIs = false;
                    childCodeIs = true;
                }
                else if (childCodeIs)
                {

                        textCode2.Text = db.GetOrignCode(get[get.Length - 1]);
                        db.SetCodeChild(get[get.Length - 1], code);
                        childCodeIs = false;
                        mainCodeIs = true;
                    
                }
                if (searchSubmitNow)
                {

                    db.SearchIn(get[get.Length - 1]);
                    textCode2.Text = db.GetOrignCodeFromId(db.IdiDenticalFie);
                    db.SetCodeChild(db.IdiDenticalFie, code);
                    childCodeIs = false;
                    mainCodeIs = true;
                    searchSubmitNow = false;
                    Compare();
                }
                page.SelectedIndex = 0;
            }
        }

        private void LoadFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog
            {
                Filter = "Исходные коды(*.cs;*.java;*.cpp;*.c)|*.cs;*.java;*.cpp;*.c" + "|Все файлы (*.*)|*.* ",
                CheckFileExists = true,
                Multiselect = true
            };
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
            searchFileNow = false;
            SubmPage();
        }

        private void SubmPage()
        {
            page.SelectedIndex = 2;
            Complite.Visibility = Visibility.Hidden;
            FileListCompil.Items.Clear();
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
            db.AddingSubmit(NameBox.Text,BoxDesc.Text, TypeCompiler,path, searchFileNow);
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

        private void MainCode_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void Compare()
        {
            vFishAlg.Text = code.AlgVarnFish().ToString() + "%";
            wShinAlg.Text = code.AlgVShiling().ToString() + "%";
            heskelAlg.Text = code.AlgHeskel().ToString() + "%";
        }
        private void Compare_OnClick(object sender, RoutedEventArgs e)
        {
            //db.SetCodeMain(1,code);
            //db.SetCodeChild(1,code);
            Compare();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SearchIn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            searchFileNow = true;
            SubmPage();
        }

        private void SearshInBD_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            searchSubmitNow = true;
            FineFile();
        }
    }
}
