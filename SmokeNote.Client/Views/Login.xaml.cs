using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using SmokeNote.Client.Models;

namespace SmokeNote.Client.Views
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
           
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            FindWord mwin = new FindWord();
            Application.Current.MainWindow = mwin;
            this.Close();
            mwin.Show();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter)
            {
                button1_Click(this.button1, null);
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()) && !string.IsNullOrEmpty(passwordBox1.Password.Trim()))
            {
                using (financepersonalEntities dbEntity = new financepersonalEntities())
                {
                    user u = dbEntity.user.Where(m => (m.UserName == textBox1.Text.Trim() && m.UserPassword == passwordBox1.Password.Trim())).FirstOrDefault();
                    if (u != null)
                    {
                        MainWindow mwin = new MainWindow(u);
                        Application.Current.MainWindow = mwin;
                        this.Close();
                        mwin.Show();
                    }
                    else
                    {
                        Dialog.DialogWindow.CreateAlertWindow("提示", "账户和口令不正确!", null).Show();
                    }
                }
            }
            else
            {
                Dialog.DialogWindow.CreateAlertWindow("提示", "账户和口令不可为空!", null).Show();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            this.Close();
            register.Show();
        }
    }
}
//uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
