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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArconGreasepaint
{
    /// <summary>
    /// Interaction logic for DefaultInputPage.xaml
    /// </summary>
    public partial class DefaultInputPage : Page
    {
        private ScenarioPage _prevPage;
        public DefaultInputPage(ScenarioPage previousPage)
        {
            _prevPage = previousPage;
            InitializeComponent();
            this.DataContext = App.workingFile;
            //Binding binding = new Binding();
            //// set source object
            //binding.Source = App.workingFile;
            //// set  source property
            //binding.Path = new PropertyPath("surfaceRoughnessLength");
            //// attach to target property
            //aadaa.SetBinding(TextBox.TextProperty, binding);
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_prevPage);
        }
    }
}
