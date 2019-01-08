using System;
using System.IO;
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
    /// Interaction logic for ScenarioInputPage.xaml
    /// </summary>
    public partial class ScenarioInputPage : Page
    {
        public ScenarioInputPage()
        {
            InitializeComponent();
        }
        private void Help_Menu_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Quit_Menu_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Continue_Button_Click(object sender, RoutedEventArgs e)
        {
            /* At this point the user has made a selection via the radio buttons and 
             * then pressed Continue.  If a new file is requested, a file open dialog
             * box should be presented.  Once a file is chosen, check to make sure that it's
             * valid (not an old file) and that the directory is writeable.  Then instantiate
             * and initialize an ArconFile object.  This will pass only one filename/path.
             * 
             * If an existing file is requested, a file open dialog box should be presented.
             * Once a file is chosen, check that it exists and is readable.  Prompt for the
             * name of the file post-modifications.  Verify that the new file is not already
             * present.  Instantiate and initialize an ArconFile object, passing both the old
             * file name and the new file name.  Is is expected that the old file will be read,
             * but the new file name will be stored for writing.
             */

            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            Microsoft.Win32.SaveFileDialog saveFileDlg = new Microsoft.Win32.SaveFileDialog();
            // Set filter for file extension and default file extension
            openFileDlg.DefaultExt = ".rsf";
            openFileDlg.Filter = "ARCON Files (*.rsf)|*.rsf|All Files (*.*)|*.*";
            saveFileDlg.DefaultExt = ".rsf";
            saveFileDlg.Filter = "ARCON Files (*.rsf)|*.rsf|All Files (*.*)|*.*";
            if ((bool)this.newFileRadioButton.IsChecked)
            {
                saveFileDlg.CheckFileExists = false;
                if (saveFileDlg.ShowDialog() == true)
                {
                    App.workingFile = new ArconFile(saveFileDlg.FileName);
                    ScenarioPage scenarioPage = new ScenarioPage();
                    this.NavigationService.Navigate(scenarioPage);
                }
            }
            else
            {
                openFileDlg.CheckFileExists = true;
                if (openFileDlg.ShowDialog() == true)
                {
                    string oldLongFileName = openFileDlg.FileName;
                    MessageBox.Show("Now enter the filename for the modified input file . . .");
                    if (saveFileDlg.ShowDialog() == true)
                    {
                        App.workingFile = new ArconFile(oldLongFileName, saveFileDlg.FileName);
                        ScenarioPage scenarioPage = new ScenarioPage();
                        this.NavigationService.Navigate(scenarioPage);
                    }
                }
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
