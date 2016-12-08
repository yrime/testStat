using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace testStat
{
    public partial class MainPage : PhoneApplicationPage
    {
        //for texblock, placeholder  будет появляться еслт поле ввода пустое
        private string placeholder = "Ваше имя";
        private IsolatedStorageFile file;

        public string Placeholder
        {
            get
            {
                return this.placeholder;
            }
            set
            {
                this.placeholder = value;
                tbMain.Text = this.placeholder;
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        /*
         * 
         * переход на другой фрейм с данными из поля воода
         * 
         */
        private void btn_name_enter(object sender, RoutedEventArgs e)
        {
            file = IsolatedStorageFile.GetUserStoreForApplication();
            file.Remove();
            NavigationService.Navigate(new Uri(string.Format("/AcsPage.xaml?UserString={0}",tbMain.Text), UriKind.Relative));
        }
        /*
         * 
         * если фокус попадает на поле ввода и оно содержит стандартную запись, то оно очищается
         * 
         */ 
        private void tbMain_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbMain.Text.Trim() == this.placeholder)
            {
                tbMain.Text = "";
            }
            bClear.Visibility = Visibility.Visible;
        }
        /*
         * 
         * если фокус спадает с поля ввода и оно пустое, то оно заполняется стандартной записью
         * 
         */ 
        private void tbMain_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbMain.Text.Trim() == "")
            {
                tbMain.Text = this.placeholder;
            }
            bClear.Visibility = Visibility.Collapsed;
        }
        /*
         * 
         * обработка клика по кнопке очистки поля (Х button)
         * 
         */ 
        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            tbMain.Text = string.Empty;
            tbMain.Focus();
        }

    }
}