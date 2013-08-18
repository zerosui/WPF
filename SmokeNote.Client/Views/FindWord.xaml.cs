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
using SmokeNote.Client.Models;

namespace SmokeNote.Client.Views
{
    /// <summary>
    /// FindWord.xaml 的交互逻辑
    /// </summary>
    public partial class FindWord : Window
    {
        public FindWord()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()) && !string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                using (financepersonalEntities dbEntity = new financepersonalEntities())
                {
                    var u = dbEntity.user.Where(m => m.UserName == textBox1.Text.Trim() && m.UserEmail == textBox2.Text.Trim())
                        .FirstOrDefault();
                    Dialog.DialogWindow.CreateAlertWindow("密码",u.UserPassword, null).Show();
                }
            }
            else
            {
                Dialog.DialogWindow.CreateAlertWindow("提示", "必填项不能为空!", null).Show(); 
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.Close();
            login.Show();
        }
    }
}
