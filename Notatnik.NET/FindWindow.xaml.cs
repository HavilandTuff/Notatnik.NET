using System.Windows;
using System.Windows.Controls;

namespace Notatnik.NET
{
    /// <summary>
    /// Interaction logic for Find.xaml
    /// </summary>
    public partial class FindWindow : Window
    {
        string textBuffer = "";
        int index = 0;
        bool stringFound = false;
        public FindWindow(string text)
        {
            InitializeComponent();
            textBuffer = text;
        }

        private void SearchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(SearchField.Text))
            {
                FindButton.IsEnabled = true;
            }
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            SearchString(textBuffer, SearchField.Text);       
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //Helper functions
        private void SearchString(string text, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                index = text.IndexOf(key, index);

                if (index > -1)
                {
                    Window parentWindow = Application.Current.MainWindow;
                    ((MainWindow)parentWindow).textBox.CaretIndex = index;
                    ((MainWindow)parentWindow).textBox.Focus();
                    ((MainWindow)parentWindow).textBox.Select(index, key.Length);
                    index++;
                    stringFound = true;
                }
                else if (index == -1 && stringFound == false)
                {
                    MessageBox.Show("String not found!", "",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    index = 0;
                }
                else if (index == -1 && stringFound == true)
                {
                    MessageBox.Show("Search finished!", "",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    index = 0;
                }
            }
        }
    }
}
