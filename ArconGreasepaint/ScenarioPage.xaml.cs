using System;
using System.Diagnostics;
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
    /// Interaction logic for scenario.xaml
    /// </summary>
    /// 
    public partial class ScenarioPage : Page
    {
        private int previousMetFileNumber;
        //==========================================================
        // constructor for the class
        //==========================================================
        public ScenarioPage()
        {
            InitializeComponent();
            this.DataContext = App.workingFile;
            previousMetFileNumber = 0;

        }
        // ============================================
        // main panel button actions
        //      these functions are called when one of the two main panel buttons are pressed.
        // ============================================
        private void Run_ButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO verify all input is correct before running
            App.workingFile.WriteFile();
            int returnCode = App.workingFile.RunFile();
            string title = "ARCON96 has run, and the Answer is:";
            string newMessage = String.Format("FORTRAN exit code: {0}", returnCode);
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon;
            if (returnCode == 0)
            {
                icon = MessageBoxImage.Information;
            }
            else
            {
                icon = MessageBoxImage.Warning;
            }
            MessageBox.Show(newMessage, title, button, icon);
        }

        private void Values_ButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO verify that all fields are valid
            if (inputMenuItem.IsEnabled)
            {
                inputMenuItem.IsEnabled = false;
            }
            else
            {
                inputMenuItem.IsEnabled = true;
            }
        }

        // ============================================
        // menu actions
        //      these functions are called when a menu item is selected
        // ============================================
        private void Quit_Menu_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Default_Menu_Click(object sender, RoutedEventArgs e)
        {
            DefaultInputPage defaultInputPage = new DefaultInputPage(this);
            this.NavigationService.Navigate(defaultInputPage);
        }

        private void OutputMenuClick(object sender, RoutedEventArgs e)
        {
            OutputFileGroupBox.Visibility = System.Windows.Visibility.Visible;
            OutputFileDefaultFocus.Focus();
        }

        private void SourceMenuClick(object sender, RoutedEventArgs e)
        {
            SourceInputGroupBox.Visibility = System.Windows.Visibility.Visible;
            InitialRadioButton.Focus();
        }

        private void ReceptorMenuClick(object sender, RoutedEventArgs e)
        {
            ReceptorInputGroupBox.Visibility = System.Windows.Visibility.Visible;
            InitialReceptorInputField.Focus();
        }

        private void MetFileNameMenuClick(object sender, RoutedEventArgs e)
        {
            MetfilePage metfileInputPage = new MetfilePage(this);
            this.NavigationService.Navigate(metfileInputPage);
        }

        private void Meteorological_Menu_Click(object sender, RoutedEventArgs e)
        {
            MetInputGroupBox.Visibility = System.Windows.Visibility.Visible;
            MetFileNumber.Focus();
        }

        // ============================================
        // sub-panel button actions
        //      these functions are called when a "Done" button is pressed on one of the
        //      four sub-panles of the Scenario Page.
        // ============================================
        private void Met_Done_Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO verify no errors on this panel
            MetInputGroupBox.Visibility = Visibility.Hidden;
        }

        private void Source_Done_Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO verify no errors on this panel
            SourceInputGroupBox.Visibility = Visibility.Hidden;
        }

        private void Receptor_Done_Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO verify no errors on this panel
            ReceptorInputGroupBox.Visibility = Visibility.Hidden;
        }

        private void Output_Done_Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO verify no errors on this panel
            OutputFileGroupBox.Visibility = Visibility.Hidden;
        }
        // ============================================
        // text box validation handler
        // ============================================

        private void HandleMetFileNumberLostFocus(object sender, RoutedEventArgs e)
        {
            // TODO some kind of error trapping on the number of met files
            // if the number of met files is out of range, show a dialog box, reset to 1, and continue.
            Int32 bongo = Convert.ToInt32(MetFileNumber.Text);
            if (bongo < 1 || bongo > 10)
            {
                string messageBoxText = "The number of met files must be between 1 and 10.  Resetting to the default value of 1 ...";
                MessageBox.Show(messageBoxText,"Oops!", MessageBoxButton.OK, MessageBoxImage.Error);
                MetFileNumber.Text = "1";
                bongo = 1;
            }
            // the trick here is we only want to display the met file input page if one of two scenarios has happened -
            // * this is the first time the scenario page has been displayed and the met file number loses focus
            // * or the number of met files changes
            if (previousMetFileNumber != bongo)
            {
                MessageBox.Show("Switching to the Met File Name Input Screen ...");
                MetfilePage metfileInputPage = new MetfilePage(this);
                this.NavigationService.Navigate(metfileInputPage);
            }
            previousMetFileNumber = bongo;
        }
    }
}
