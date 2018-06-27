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
        private Analysis code;
        private DataBaseLite db;
        private bool mainCodeIs = true;
        private bool childCodeIs = true;
        private bool searchFileNow = false;
        private bool searchSubmitNow = false;
        private bool notHistoryEnter = false;
        public MainWindow()
        {
            InitializeComponent();
            db = new DataBaseLite();
            code = new Analysis();
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
            page.SelectedIndex = 4;
        }

        private void FineFile(bool isHist)
        {
            if (!isHist)
            {
                List<string> s = db.DescSubm();
                FileList.Items.Clear();
                foreach (var desc in s)
                {
                    ListViewItem item = new ListViewItem { Content = desc };
                    FileList.Items.Add(item);
                }
            }
            else if (isHist)
            {
                List<string> s = db.GetListHistory();
                FileList.Items.Clear();
                foreach (var desc in s)
                {
                    ListViewItem item = new ListViewItem { Content = desc };
                    FileList.Items.Add(item);
                }
            }
           
           page.SelectedIndex = 1;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            var item = sender as ListViewItem;
            string content = item?.Content.ToString();
            string[] get = content?.Split(new char[] {'|'});
            if (item != null && item.IsSelected && notHistoryEnter)
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
                    textCode.ToolTip = db.GetInfoSubm(true);
                    textCode2.ToolTip = db.GetInfoSubm(false);
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
            if (searchFileNow && db.IsNotEnpty())
            {
                textCode.Text = db.GetOrignCodeFromId(db.IdMainFileForHist);
                textCode2.Text = db.GetOrignCodeFromId(db.IdiDenticalFie);
                db.SetCodeMain(db.IdMainFileForHist, code);
                db.SetCodeChild(db.IdiDenticalFie, code);
                Compare();
                textCode.ToolTip = db.GetInfoSubm(true);
                textCode2.ToolTip = db.GetInfoSubm(false);
            }

            if (!db.IsNotEnpty())
            {
                page.SelectedIndex = 3;
            }
            else
            {
                page.SelectedIndex = 0;
                charpRButton.IsChecked = true;
                NameBox.Text = "Имя и Фамилия";
                BoxDesc.Text = "Краткое описание";
            }
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


        private void Compare()
        {
            double resVarnFish = Double.Parse(String.Format("{0:0.##}", code.AlgVarnFish()));
            double resVShiling = Double.Parse(String.Format("{0:0.##}", code.AlgWShiling()));
            double resHeskel = Double.Parse(String.Format("{0:0.##}", code.AlgHeskel()));
            vFishAlg.Text = resVarnFish + "%";
            wShinAlg.Text = resVShiling + "%";
            heskelAlg.Text = resHeskel + "%";
            db.AddingHistiry(resVarnFish, resVShiling, resHeskel);
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
            notHistoryEnter = true;
            FineFile(false);
        }

        private void History_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FineFile(true);
            notHistoryEnter = false;
        }

        private void DescProgram_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            page.SelectedIndex = 3;
        }

        private void Titul_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
