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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public user u;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.Title += "       当前账户：" + u.UserName;
            Application.Current.MainWindow = this;
        }

        public MainWindow(user custom)
        {
            InitializeComponent();
            u = custom;
            this.WindowState = WindowState.Maximized;
            this.Title += "       当前账户：" + custom.UserName;
            AccountName.Text += custom.UserName;
            using (financepersonalEntities dbEntity = new financepersonalEntities())
            {
                acount ac = dbEntity.acount.Where(m => m.AcountDefine1 == custom.UserName).FirstOrDefault();
                AccountBalance.Text += ac.AcountTotal+" 元";
            }
            Application.Current.MainWindow = this;
        }

        private void miConfig_Click(object sender, RoutedEventArgs e)
        {
            ChildrenWinContent.Children.Clear();
            ChildrenWinContent.Children.Add(new AddAction(u));
            ChildrenWinContent.Children.Add(new AcountManage(u));
        }

        private void MaConfig_Click(object sender, RoutedEventArgs e)
        {
            ChildrenWinContent.Children.Clear();
            ChildrenWinContent.Children.Add(new BalanceView(u));
        }
        private void MgConfig_Click(object sender, RoutedEventArgs e)
        {
            ChildrenWinContent.Children.Clear();
            ChildrenWinContent.Children.Add(new ActionManage(u));
        }

        private void UpdatePass_Click(object sender, RoutedEventArgs e)
        {
            UpdatePassword up = new UpdatePassword(u);
            Application.Current.MainWindow = up;
            up.ShowDialog();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            showDialog("帮助", "WPF课程设计,个人记账软件V1.0");           
        }

        private void showDialog(string caption, string content)
        {
            Dialog.DialogWindow dg = new Dialog.DialogWindow(caption, content);
            dg.btnCancel.Visibility = Visibility.Collapsed;
            dg.ShowDialog();
        }
    }
}
