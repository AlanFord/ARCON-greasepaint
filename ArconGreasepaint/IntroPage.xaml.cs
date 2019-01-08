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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class IntroPage : Page
    {
        public IntroPage()
        {
            InitializeComponent();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Continue_Button_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "The author makes no warranty, expressed or implied, or assumes any legal liability or responsibilities for any third party's use, or the results of such use, of any portion of this program or represents that its use by such third party would not infringe privately owned rights.";
            string caption = "DISCLAMER";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch(result)
            {
                case MessageBoxResult.OK:
                    ScenarioInputPage scenarioInputPage = new ScenarioInputPage();
                    this.NavigationService.Navigate(scenarioInputPage);
                    break;
                case MessageBoxResult.Cancel:
                    System.Windows.Application.Current.Shutdown();
                    break;
            }
        }
    }
}
