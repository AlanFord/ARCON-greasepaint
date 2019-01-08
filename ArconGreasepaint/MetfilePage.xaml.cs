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
    /// Interaction logic for MetfilePage.xaml
    /// </summary>
    public partial class MetfilePage : Page
    {
        private ScenarioPage _prevPage;
        private int count;
        private TextBlock[] listOfTextBlocks = new TextBlock[10];
        private TextBox[] listOfTextBoxs= new TextBox[10];

        public MetfilePage(ScenarioPage previousPage)
        {
            _prevPage = previousPage;
            if (App.workingFile.NumberOfMetFiles == null)
            {
                // set a default value
                count = 1;
            }
            else
            {
                count = (int) App.workingFile.NumberOfMetFiles;
            }
            InitializeComponent();
            this.DataContext = App.workingFile;
            // populate the array for easier working
            listOfTextBlocks[0] = textBlock1;
            listOfTextBlocks[1] = textBlock2;
            listOfTextBlocks[2] = textBlock3;
            listOfTextBlocks[3] = textBlock4;
            listOfTextBlocks[4] = textBlock5;
            listOfTextBlocks[5] = textBlock6;
            listOfTextBlocks[6] = textBlock7;
            listOfTextBlocks[7] = textBlock8;
            listOfTextBlocks[8] = textBlock9;
            listOfTextBlocks[9] = textBlock10;
            listOfTextBoxs[0] = textBox1;
            listOfTextBoxs[1] = textBox2;
            listOfTextBoxs[2] = textBox3;
            listOfTextBoxs[3] = textBox4;
            listOfTextBoxs[4] = textBox5;
            listOfTextBoxs[5] = textBox6;
            listOfTextBoxs[6] = textBox7;
            listOfTextBoxs[7] = textBox8;
            listOfTextBoxs[8] = textBox9;
            listOfTextBoxs[9] = textBox10;
            for(int i=0; i < count; i++)
            {
                listOfTextBlocks[i].Visibility = Visibility.Visible;
                listOfTextBoxs[i].Visibility = Visibility.Visible;
            }
            for (int i = count; i < 10; i++)
            {
                listOfTextBlocks[i].Visibility = Visibility.Hidden;
                listOfTextBoxs[i].Visibility = Visibility.Hidden;
            }
        }

        private void Done_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_prevPage);
        }

        private void FileSearch_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
