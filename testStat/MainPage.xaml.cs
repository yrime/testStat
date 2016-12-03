using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace testStat
{
    public partial class MainPage : PhoneApplicationPage
    {
        //for texblock, placeholder  будет появляться еслт поле ввода пустое

        private string placeholder = "Ваше имя";

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

        private void btn_name_enter(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri(string.Format("/AcsPage.xaml?UserString={0}",tbMain.Text), UriKind.Relative));
        }

        private void tbMain_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbMain.Text.Trim() == this.placeholder)
            {
                tbMain.Text = "";
            }
            bClear.Visibility = Visibility.Visible;
        }

        private void tbMain_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbMain.Text.Trim() == "")
            {
                tbMain.Text = this.placeholder;
            }
            bClear.Visibility = Visibility.Collapsed;
        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            tbMain.Text = string.Empty;
            tbMain.Focus();
        }

    }
}