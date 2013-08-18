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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmokeNote.Client.Models;

namespace SmokeNote.Client.Views
{
    /// <summary>
    /// AcountManage.xaml 的交互逻辑
    /// </summary>
    public partial class AcountManage : UserControl
    {
        user myuser;
        public AcountManage()
        {
            InitializeComponent();
            bindData();
        }
        public AcountManage(user u)
        {
            InitializeComponent();
            myuser = u;
            bindData();
        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            DataGridRow row = FindVisualParent<DataGridRow>(sender as Expander);
            row.DetailsVisibility = System.Windows.Visibility.Visible;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            DataGridRow row = FindVisualParent<DataGridRow>(sender as Expander);
            row.DetailsVisibility = System.Windows.Visibility.Collapsed;
        }

        public T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }

        public void bindData()
        {
            string userid = myuser.UserId.ToString();
            using (financepersonalEntities dbEntity = new financepersonalEntities())
            {
                IQueryable<myaction> list = dbEntity.action.Join(
                    dbEntity.user, p => p.ActionUserId, m => m.UserId, (p, m) =>
                        new myaction {
                    ActionUser = m.UserName,
                    ActionId=p.ActionId,
                    ActionTime=p.ActionTime,
                    ActionRole=p.ActionRole,
                    ActionMoney=p.ActionMoney,
                    ActionCommet=p.ActionCommet
                                     });
                List<myaction> mylist = list.ToList();
                myGrid.DataContext = mylist;

               
            }
        }
    }
}
//Linq 及 EntityFramework 联查
//using (financepersonalEntities dbEntity = new financepersonalEntities())
//{
//    string categoryName = myuser.UserName;
//    List<myaccount> mylist;
//    //var list = dbEntity.acount.Join(dbEntity.action, p => p.AcountId, m => m.ActionCountId, (p, m) => new myaccount{ p.AcountTotal, m.ActionName, m.ActionMoney }).FirstOrDefault();
//    IQueryable<myaccount> list = from myac in dbEntity.acount
//                                 join myact in dbEntity.action on myac.AcountId
//                                     equals myact.ActionCountId
//                                 where myac.AcountDefine1 == categoryName
//                                 select new myaccount
//                                 {
//                                     ActionUser = categoryName,
//                                     AcountTotal = myac.AcountTotal,
//                                     ActionId = myact.ActionId,
//                                     ActionName = myact.ActionName,
//                                     ActionRole = myact.ActionRole,
//                                     ActionMoney = myact.ActionMoney,
//                                     ActionTime = myact.ActionTime,
//                                     ActionCommet = myact.ActionCommet
//                                 };
//    mylist = list.ToList();
//    this.DataContext = mylist;
//}