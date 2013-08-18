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
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
        }

        private bool IsNull(string text)
        {
             return !string.IsNullOrEmpty(text);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (IsNull(textBox1.Text) && IsNull(passwordBox1.Password) && IsNull(textBox3.Text) && IsNull(passwordBox2.Password))
            {
                if (passwordBox1.Password == passwordBox2.Password)
                {
                    using (financepersonalEntities dbEntity = new financepersonalEntities())
                    {
                        user u = new user();
                        u.UserName = textBox1.Text.Trim();
                        u.UserPassword = passwordBox1.Password.Trim();
                        u.UserEmail = textBox3.Text.Trim();

                        acount ac = new acount();
                        ac.AcountTotal = "0";
                        ac.AcountUserId = "xxx";
                        ac.AcountDefine1 = u.UserName;

                        dbEntity.user.AddObject(u);
                        dbEntity.acount.AddObject(ac);
                        dbEntity.SaveChanges();             

                        Dialog.DialogWindow.CreateAlertWindow("提示", "注册成功!", null).Show();
                        Login login = new Login();
                        this.Close();
                        login.Show();
                    }
                }
                else
                {
                    Dialog.DialogWindow.CreateAlertWindow("提示", "两次密码必须一致!", null).Show();
                }
            }
            else
            {
                Dialog.DialogWindow.CreateAlertWindow("提示", "必填项不能为空!", null).Show(); 
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text =  textBox3.Text = "";
            passwordBox1.Clear();
            passwordBox2.Clear();
            Login login = new Login();
            this.Close();
            login.Show();
        }
    }
}
