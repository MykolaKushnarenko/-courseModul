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
        private readonly DataBaseLite DataBase;
        private readonly string Name;
        private readonly string Description;
        private readonly string TypeCompile;
        private readonly string Path;
        private readonly bool IsSearch;
        public LoadWindow(DataBaseLite data, string name, string description, string type, string path, bool isSearch)
        {
            InitializeComponent();
            DataBase = data;
            Name = name;
            Description = description;
            TypeCompile = type;
            Path = path;
            IsSearch = isSearch;
            Load();
        }

        private async void Load()
        {
            await DataBase.AddingSubmit(Name, Description, TypeCompile, Path, IsSearch);
            this.Close();
        }
    }
}
