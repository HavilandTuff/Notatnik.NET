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
using WPFUtils;

namespace Notatnik.NET
{
    /// <summary>
    /// Interaction logic for Find.xaml
    /// </summary>
    public partial class FindWindow : Window
    {
        string textBuffer = "";
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

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
