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
 // Menu File
        private void MenuItem_Nowy_Click( object sender, RoutedEventArgs e)
        {
            bool isCancel;
            AskToSaveFile( sender, out isCancel);
            if (!isCancel)
                {
                textBox.Text = "";
                }
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
        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool isCancel;
            AskToSaveFile( sender, out isCancel);
            e.Cancel = isCancel;
            
        }
        private void AskToSaveFile(object sender, out bool isCancel )
            {
                isCancel = false;
                
                if(isTextChanged)
            {
                MessageBoxResult result = 
                    MessageBox.Show("Czy zapisać zmiany w edytowanym dokumencie?", 
                                    this.Title,
                                    MessageBoxButton.YesNoCancel,
                                    MessageBoxImage.Question,
                                    MessageBoxResult.Cancel);
                switch(result)
                {
                    case MessageBoxResult.Yes:
                        MenuItem_Zapisz_Click(sender, null);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                    default:
                        isCancel = true;
                        break;
                }
            }

            }
// Menu Edit
        private void MenuItem_Cofnij_Click(object sender, RoutedEventArgs e)
        {
            textBox.Undo();
        }
        private void MenuItem_Powtorz_Click( object sender, RoutedEventArgs e)
        {
            textBox.Redo();
        }
        private void MenuItem_Kopiuj_Click( object sender, RoutedEventArgs e)
        {
            textBox.Copy();
        }
        private void MenuItem_Wytnij_Click ( object sender, RoutedEventArgs e)
        {
            textBox.Cut();
        }
        private void MenuItem_Wklej_Click ( object sender, RoutedEventArgs e)
        {
            textBox.Paste();
        }
        private void MenuItem_Usun_Click ( object sender, RoutedEventArgs e)
        {
            textBox.SelectedText = "";
        }
        private void MenuItem_Zaznacz_Click ( object sender, RoutedEventArgs e)
        {
            textBox.SelectAll();
        }
        private void MenuItem_Data_Click ( object sender, RoutedEventArgs e)
        {
            textBox.SelectedText = System.DateTime.Now.ToString();
        }
// Menu View
        
    }
}
