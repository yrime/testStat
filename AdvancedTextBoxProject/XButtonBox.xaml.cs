using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AdvancedTextBoxProject.Resources;

namespace AdvancedTextBoxProject
{
    public partial class MainPage : PhoneApplicationPage
    {
        //vars

        private string placeholder = "";
        private TextBox textout;

        public string Placeholder
        {
            get
            {
                return this.placeholder;
            }
            set
            {
                this.placeholder = value;
                texboxName.Text = this.placeholder;
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void textboxName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (texboxName.Text.Trim() == this.placeholder)
            {
                texboxName.Text = "";
            }
            bClear.Visibility = Visibility.Visible;
        }

        private void texboxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (texboxName.Text.Trim() == "")
            {
                texboxName.Text = placeholder;
            }
            bClear.Visibility = Visibility.Collapsed;
        }

        private void texboxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            textout = (TextBox)sender;
        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            texboxName.Text = string.Empty;
            texboxName.Focus();
        }
    }
}