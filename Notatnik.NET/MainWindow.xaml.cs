using System;
using System.Windows;
using System.IO;
using Microsoft.Win32;

namespace Notatnik.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private string filePath = null;
        private bool isTextChanged = false;

        public MainWindow()
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik tekstowy.";
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Pliki tekstowe (*.txt) | *.txt | Pliki XML (*.xml) | *.xml | Pliki źródłowe (*.cs) | *.cs | " +
                "Wszystkie pliki (*.*) | *.*";
            openFileDialog.FilterIndex = 1;

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Zapisz plik tekstowy";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = openFileDialog.Filter;
            saveFileDialog.FilterIndex = 1;
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(filePath))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(filePath);
                openFileDialog.FileName = Path.GetFileName(filePath);
            }
            bool? result = openFileDialog.ShowDialog();
            if(result.HasValue && result.Value)
            {
                filePath = openFileDialog.FileName;
                textBox.Text = File.ReadAllText(filePath);
                statusBarText.Text = Path.GetFileName(filePath);
            }
        }

        private void MenuItem_ZapiszJako_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(filePath))
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(filePath);
                saveFileDialog.FileName = Path.GetFileName(filePath);
            }

            bool? result = saveFileDialog.ShowDialog();
            if(result.HasValue && result.Value)
            {
                filePath = saveFileDialog.FileName;
                File.WriteAllText(filePath, textBox.Text);
                statusBarText.Text = Path.GetFileName(filePath);
                isTextChanged = false;
            }
           
        }

        private void MenuItem_Zapisz_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(filePath))
            {
                File.WriteAllText(filePath, textBox.Text);
                isTextChanged = false;
            }
            else
            {
                MenuItem_ZapiszJako_Click(sender, e);
            }
            
        }

        private void MenuItem_Zamknij_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            isTextChanged = true;
        }
    }
}
