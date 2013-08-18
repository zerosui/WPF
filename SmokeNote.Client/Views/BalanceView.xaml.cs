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
    /// BalanceView.xaml 的交互逻辑
    /// </summary>
    public partial class BalanceView : UserControl
    {
        user myuser;
        public BalanceView()
        {
            InitializeComponent();
            BindData();
        }
        public BalanceView(user u)
        {
            InitializeComponent();
            myuser = u;
            BindData();
        }
        public List<myaction> SetSum(List<myaction> mylist, List<role> r)
        {
            double BalanceIn = 0;
            double BalanceOut = 0;
            for (int i = 0; i < mylist.Count; i++)
            {
                if (mylist[i].ActionName == "2")
                {
                    mylist[i].ActionName = "支出";
                   
                    BalanceOut+=double.Parse(mylist[i].ActionMoney);
                }
                else
                {
                    mylist[i].ActionName = "收入";
                    BalanceIn+=double.Parse(mylist[i].ActionMoney);
                }
                role me = r.Find(s => s.RoleId.ToString() == mylist[i].ActionRole);
                mylist[i].ActionRole = me.RoleDefine1;
            }
            textBlock2.Text += BalanceOut.ToString();
            textBlock3.Text += BalanceIn.ToString();
            textBlock4.Text += (BalanceIn - BalanceOut) > 0 ? "+"+(BalanceIn - BalanceOut).ToString() : "-"+(BalanceOut - BalanceIn).ToString();
            textBlock5.Text = string.Format("共{0}条记录", mylist.Count.ToString());
            return mylist;
        }

        public void BindData()
        {
            string userid = myuser.UserId.ToString();
            using (financepersonalEntities dbEntity = new financepersonalEntities())
            {
                IQueryable<myaction> list = dbEntity.action.Join(
                    dbEntity.user, p => p.ActionUserId, m => m.UserId, (p, m) =>
                        new myaction
                        {
                            ActionUser = m.UserName,
                            ActionId = p.ActionId,
                            ActionTime = p.ActionTime,
                            ActionRole = p.ActionRole,
                            ActionName=p.ActionName,
                            ActionMoney = p.ActionMoney,
                            ActionCommet = p.ActionCommet
                        });
                List<role> r = dbEntity.role.Select(m => m).ToList();
                List<myaction> mylist = list.ToList();
                mylist=SetSum(mylist,r);
                this.listView1.ItemsSource = mylist;     
            }
        }
    }
}
