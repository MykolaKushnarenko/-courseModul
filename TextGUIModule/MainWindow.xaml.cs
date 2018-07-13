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
using CoDEmpare.WinPage;
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

        //private void FineFile(bool isHist)
        //{
        //    if (!isHist)
        //    {
        //        List<string> s = db.DescSubm();
        //        FileList.Items.Clear();
        //        foreach (var desc in s)
        //        {
        //            ListViewItem item = new ListViewItem { Content = desc };
        //            FileList.Items.Add(item);
        //        }
        //    }
           
        //}

        //private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
           
        //    var item = sender as ListViewItem;
        //    string content = item?.Content.ToString();
        //    string[] get = content?.Split(new char[] {'|'});
        //    if (item != null && item.IsSelected && notHistoryEnter)
        //    {
        //        if (mainCodeIs)
        //        {

        //            textCode.Text = db.GetOrignCode(get[get.Length - 1]);
        //            db.SetCodeMain(get[get.Length - 1], code);
        //            mainCodeIs = false;
        //            childCodeIs = true;
        //        }
        //        else if (childCodeIs)
        //        {

        //                textCode2.Text = db.GetOrignCode(get[get.Length - 1]);
        //                db.SetCodeChild(get[get.Length - 1], code);
        //                childCodeIs = false;
        //                mainCodeIs = true;
                    
        //        }
        //        if (searchSubmitNow)
        //        {

        //            db.SearchIn(get[get.Length - 1]);
        //            textCode2.Text = db.GetOrignCodeFromId(db.IdiDenticalFie);
        //            db.SetCodeChild(db.IdiDenticalFie, code);
        //            childCodeIs = false;
        //            mainCodeIs = true;
        //            searchSubmitNow = false;
        //            Compare();
        //            textCode.ToolTip = db.GetInfoSubm(true);
        //            textCode2.ToolTip = db.GetInfoSubm(false);
        //        }
        //        page.SelectedIndex = 0;
        //    }
        //}

        

        

        

        

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void SearshInBD_OnMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    searchSubmitNow = true;
        //    notHistoryEnter = true;
        //    FineFile(false);
        //}

        //private void History_OnMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    FineFile(true);
        //    notHistoryEnter = false;
        //}

        private void DescProgram_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Titul_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void LoOutBut_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpemMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpemMenuButton.Visibility = Visibility.Collapsed;
            CloseMenuButton.Visibility = Visibility.Visible;
        }

        private void CloseMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpemMenuButton.Visibility = Visibility.Visible;
            CloseMenuButton.Visibility = Visibility.Collapsed;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            GridContentAction.Children.Clear();
            GridContentAction.Children.Add(new ResultPage());
        }

        private void ListViewMenu_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            switch (index)
            {
                case 0:
                    GridContentAction.Children.Clear();
                    GridContentAction.Children.Add(new AddingSubmit(){SwichToResutl = Result, Search = false});
                    break;
                case 1:
                    GridContentAction.Children.Clear();
                    GridContentAction.Children.Add(new AddingSubmit(){SwichToResutl = Result, Search = true});
                    break;
                case 2:
                    GridContentAction.Children.Clear();
                    GridContentAction.Children.Add(new ResultPage());
                    break;
                case 3:
                    GridContentAction.Children.Clear();
                    GridContentAction.Children.Add(new HistoryPage());
                    break;
            }
            MoveOnSecectItem(index);
        }

        private void MoveOnSecectItem(int indexItem)
        {
            TransitionContentSlide.OnApplyTemplate();
            GridSelection.Margin = new Thickness(0, ((indexItem * 75)), 0, 0);
        }

        private void Result(DataBaseLite db)
        {
            ResultPage result = new ResultPage()
            {
                MainCode = db.GetOrignCodeFromId(db.IdMainFileForHist),
                ChaildCode = db.GetOrignCodeFromId(db.IdiDenticalFie)

            };
            

            //textCode.ToolTip = db.GetInfoSubm(true);
            //textCode2.ToolTip = db.GetInfoSubm(false);

            GridContentAction.Children.Clear();
            GridContentAction.Children.Add(result);

        }
    }
}

