using System;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using WPFUtils;
using System.Windows.Media;
using System.Windows.Input;


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
            System.Threading.Thread.CurrentThread.CurrentUICulture =
new System.Globalization.CultureInfo("pl");
            InitializeComponent();

            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = Properties.Resources.ChooseTextFile;
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = Properties.Resources.DialogsFilter;
            openFileDialog.FilterIndex = 1;

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = Properties.Resources.SaveTextFile;
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
                statusBar.Background = new SolidColorBrush(Colors.Green);
            }
           
        }

        private void MenuItem_Zapisz_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(filePath))
            {
                File.WriteAllText(filePath, textBox.Text);
                isTextChanged = false;
                statusBar.Background = new SolidColorBrush(Colors.Green);
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
            if(!isTextChanged)
            {
                statusBar.Background = new SolidColorBrush(Colors.Orange);
                statusBarText.Text += Properties.Resources.NotSaved;
            }
            isTextChanged = true;
            undoToolBar.IsEnabled = textBox.CanUndo;
            redoToolBar.IsEnabled = textBox.CanRedo;
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
                    MessageBox.Show(Properties.Resources.SaveChanges, 
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
        private void MenuItem_Print_Click(object sender, RoutedEventArgs e)
        {
            Printing.PrintText(textBox.Text, Font.ExtractFrom(textBox));
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

        private void MenuItem_ZawijanieWierszy_Click(object sender, RoutedEventArgs e)
        {
            bool isPositionChecked = (sender as MenuItem).IsChecked;
            textBox.TextWrapping = isPositionChecked ? TextWrapping.Wrap : TextWrapping.NoWrap;

        }

        private void MenuItem_PasekNarzedzi_Click(object sender, RoutedEventArgs e)
        {
            bool isPositionChecked = (sender as MenuItem).IsChecked;
            toolBar.Visibility = isPositionChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MenuItem_PasekStanu_Click(object sender, RoutedEventArgs e)
        {
            bool isPositionChecked = (sender as MenuItem).IsChecked;
            statusBar.Visibility = isPositionChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MenuItem_KolorTla_Click(object sender, RoutedEventArgs e)
        {
            Color kolorTla = Colors.Yellow;
            if (textBox.Background is SolidColorBrush)
            {
                kolorTla = (textBox.Background as SolidColorBrush).Color;                
            }
            if (WindowsFormsHelper.ChooseColor(ref kolorTla))
            {
                textBox.Background = new SolidColorBrush(kolorTla);                
            }
        }

        private void MenuItem_Czcionka_Click(object sender, RoutedEventArgs e)
        {
            Font font = Font.ExtractFrom(textBox);
            if (WindowsFormsHelper.ChooseFont(ref font))
            {
                font.ApplyTo(textBox);
            }
        }

        //Shortcuts bindings
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.F5: MenuItem_Data_Click(sender, null);
                    break;
            }
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.N:
                        MenuItem_Nowy_Click(sender, null);
                        break;
                    case Key.O:
                        MenuItem_Open_Click(sender, null);
                        break;
                    case Key.S:
                        MenuItem_Zapisz_Click(sender, null);
                        break;
                    case Key.P:
                        MenuItem_Print_Click(sender, null);
                        break;
                }
            }
        }

        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            undo.IsEnabled = textBox.CanUndo;
            redo.IsEnabled = textBox.CanRedo;
        }
    }
}
