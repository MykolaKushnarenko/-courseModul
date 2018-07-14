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
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        private readonly DataBaseLite _dataBase;
        private readonly string _name;
        private readonly string _description;
        private readonly string _typeCompile;
        private readonly string _path;
        private readonly bool _isSearch;
        public LoadWindow(DataBaseLite data, string name, string description, string type, string path, bool isSearch)
        {
            InitializeComponent();
            _dataBase = data;
            _name = name;
            _description = description;
            _typeCompile = type;
            _path = path;
            _isSearch = isSearch;
            Load();
        }

        private async void Load()
        {
            await _dataBase.AddingSubmit(_name, _description, _typeCompile, _path, _isSearch);
            this.Close();
        }
    }
}
