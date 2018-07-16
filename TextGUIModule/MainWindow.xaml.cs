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
        private DataBaseLite _db;
        public MainWindow()
        {
            InitializeComponent();
            _db = new DataBaseLite();
            GridContentAction.Children.Add(new AddingSubmit(_db, Result, false));
            
        }

        private void LoOutBut_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpemMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpemMenuButton.Visibility = Visibility.Collapsed;
            CloseMenuButton.Visibility = Visibility.Visible;
            UserPhoto.Visibility = Visibility.Visible;
        }

        private void CloseMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpemMenuButton.Visibility = Visibility.Visible;
            CloseMenuButton.Visibility = Visibility.Collapsed;
            UserPhoto.Visibility = Visibility.Collapsed;
        }

        
        private void ListViewMenu_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            switch (index)
            {
                case 0:
                    GridContentAction.Children.Clear();
                    GridContentAction.Children.Add(new AddingSubmit(_db, Result, false));
                    break;
                case 1:
                    GridContentAction.Children.Clear();
                    GridContentAction.Children.Add(new AddingSubmit(_db, Result, true));
                    break;
                case 2:
                    GridContentAction.Children.Clear();
                    GridContentAction.Children.Add(new DataBasesSubmitList(_db, Result));
                    break;
                case 3:
                    GridContentAction.Children.Clear();
                    GridContentAction.Children.Add(new HistoryPage(_db));
                    break;
            }
            MoveOnSecectItem(index);
        }

        private void MoveOnSecectItem(int indexItem)
        {
            TransitionContentSlide.OnApplyTemplate();
            GridSelection.Margin = new Thickness(0, ((indexItem * 75)), 0, 0);
        }

        private void Result(DataBaseLite data, bool compareFromListSubmit)
        {
            ResultPage result;
            if (!compareFromListSubmit)
            {
                result = new ResultPage(data.GetOrignCodeFromId(data.IdMainFileForHist),
                    data.GetOrignCodeFromId(data.IdiDenticalFie), ref _db);
            }
            else
            {
                result = new ResultPage(data.GetOrignCodeFromId(data.IdMainFileForHist),
                    data.GetOrignCodeFromId(data.IdiDenticalFie), ref _db);
            }
            


            //textCode.ToolTip = db.GetInfoSubm(true);
            //textCode2.ToolTip = db.GetInfoSubm(false);

            GridContentAction.Children.Clear();
            GridContentAction.Children.Add(result);

        }

        private void Title_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void HelpButt_OnClick(object sender, RoutedEventArgs e)
        {
            GridContentAction.Children.Clear();
            GridContentAction.Children.Add(new HelpPage());
        }
    }
}

