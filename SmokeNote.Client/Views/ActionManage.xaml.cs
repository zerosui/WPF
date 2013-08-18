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
    /// ActionManage.xaml 的交互逻辑
    /// </summary>
    public partial class ActionManage : UserControl
    {
        private List<int> selectUid = new List<int>();//保存多选用户ID  
        private List<int> allUid = new List<int>();//保存全选用户ID  
        List<mycheck> mylist;
        user myuser;
        public ActionManage()
        {
            InitializeComponent();
            this.DataBinding();  
        }
        public ActionManage(user u)
        {
            InitializeComponent();
            myuser = u;
            this.DataBinding();
        }
        public List<mycheck> SetSum(List<mycheck> mylist, List<role> r)
        {
            double BalanceIn = 0;
            double BalanceOut = 0;
            for (int i = 0; i < mylist.Count; i++)
            {
                if (mylist[i].ActionName == "2")
                {
                    mylist[i].ActionName = "支出";

                    BalanceOut += double.Parse(mylist[i].ActionMoney);
                }
                else
                {
                    mylist[i].ActionName = "收入";
                    BalanceIn += double.Parse(mylist[i].ActionMoney);
                }
                role me = r.Find(s => s.RoleId.ToString() == mylist[i].ActionRole);
                mylist[i].ActionRole = me.RoleDefine1;
            }
            return mylist;
        }

        public void DataBinding()
        {
            string userid = myuser.UserId.ToString();
            using (financepersonalEntities dbEntity = new financepersonalEntities())
            {
                IQueryable<mycheck> list = dbEntity.action.Join(
                    dbEntity.user, p => p.ActionUserId, m => m.UserId, (p, m) =>
                        new mycheck
                        {
                            ActionUser = m.UserName,
                            ActionId = p.ActionId,
                            ActionTime = p.ActionTime,
                            ActionRole = p.ActionRole,
                            ActionName = p.ActionName,
                            ActionMoney = p.ActionMoney,
                            ActionCommet = p.ActionCommet,
                            IsChecked=false
                        });
                List<role> r = dbEntity.role.Select(m => m).ToList();
                mylist = list.ToList();
                mylist = SetSum(mylist, r);
                this.listView1.ItemsSource = mylist;
            }
        }
      
        /// <summary>  
        /// 复选框删除用户  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteUsers(selectUid);
            DataBinding();
            this.listView1.Items.Refresh();//刷新数据  
        }

        /// <summary>  
        /// 由ChecBox的Click事件来记录被选中行的  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            int uid = Convert.ToInt32(cb.Tag.ToString()); //获取该行id  
            if (cb.IsChecked == true)
            {
                selectUid.Add(uid);  //如果选中就保存id  
            }
            else
            {
                selectUid.Remove(uid);   //如果选中取消就删除里面的id  
            }
        }

        /// <summary>  
        /// 批量删除用户  
        /// </summary>  
        private void DeleteUsers(List<int> selectUid)
        {
            if (selectUid.Count > 0)
            {
                using (financepersonalEntities dbEntity = new financepersonalEntities())
                {
                    foreach (int uid in selectUid)
                    {
                        action a = dbEntity.action.Where(m => m.ActionId == uid).FirstOrDefault();
                        dbEntity.DeleteObject(a);
                    }
                    dbEntity.SaveChanges();
                }
            }
        }

        /// <summary>  
        /// 按钮单行删除  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int uid = Convert.ToInt32(b.CommandParameter);
            this.DeleteUser(uid);
            DataBinding();
            this.listView1.Items.Refresh();
        }

        /// <summary>  
        /// 删除一个用户  
        /// </summary>  
        /// <param name="uid"></param>  
        private void DeleteUser(int uid)
        {
            using (financepersonalEntities dbEntity = new financepersonalEntities())
            {
                action a = dbEntity.action.Where(m => m.ActionId == uid).FirstOrDefault();
                dbEntity.DeleteObject(a);
                dbEntity.SaveChanges();
            }
        }

        /// <summary>  
        /// 全选  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked == true)
            {
                using (financepersonalEntities dbEntity = new financepersonalEntities())
                {
                    allUid = dbEntity.action.Select(l => l.ActionId).ToList();
                }
                for (int i = 0; i < mylist.Count; i++)
                {
                    mylist[i].IsChecked = true;
                }
            }
            else
            {
                allUid.Clear();
                for (int i = 0; i < mylist.Count; i++)
                {
                    mylist[i].IsChecked = false;
                }
            }
        }  
    }
}
