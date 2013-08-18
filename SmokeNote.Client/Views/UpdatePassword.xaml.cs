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
using System.Data;

namespace SmokeNote.Client.Views
{
    /// <summary>
    /// UpdatePassword.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePassword : Window
    {
        public user myuser;
        public UpdatePassword()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
        }

        public UpdatePassword(user u)
        {
            InitializeComponent();
            myuser = u;
            Application.Current.MainWindow = this;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBox1.Password.Trim()) && !string.IsNullOrEmpty(passwordBox2.Password.Trim())
                && !string.IsNullOrEmpty(passwordBox3.Password.Trim()))
            {
                if (passwordBox1.Password.Trim() == myuser.UserPassword)
                {
                    if (passwordBox2.Password.Trim() != passwordBox3.Password.Trim())
                    {
                        showDialog("提示", "确认密码不一致，请检查");
                    }
                    else
                    {
                        using (financepersonalEntities dbEntity = new financepersonalEntities())
                        {
                            user u = dbEntity.user.Where(m => m.UserId == myuser.UserId).FirstOrDefault();
                            u.UserPassword = passwordBox2.Password;
                            dbEntity.ObjectStateManager.ChangeObjectState(u, EntityState.Modified);
                            dbEntity.SaveChanges();
                            showDialog("提示", "恭喜，修改成功！");
                        }
                    }
                }
                else
                {
                    showDialog("提示", "原密码有误，请检查");
                }
            }
            else
            {
                showDialog("提示", "必填项不可为空，请检查");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void showDialog(string caption, string content)
        {
            Dialog.DialogWindow dg = new Dialog.DialogWindow(caption, content);
            dg.btnCancel.Visibility = Visibility.Collapsed;
            dg.ShowDialog();
        }
    }
}
