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
using SmokeNote.Client.Consts;
using SmokeNote.Client.Models;
using System.Data;

namespace SmokeNote.Client.Views
{
    /// <summary>
    /// AddAction.xaml 的交互逻辑
    /// </summary>
    public partial class AddAction : UserControl
    {
        user AddUser;
        public AddAction()
        {
            InitializeComponent();
            BindData();
        }

        public AddAction(user u)
        {
            AddUser = u;
            InitializeComponent();
            BindData();
        }

        private void BindCombox(string index)
        {
            comboBox1.Items.Clear();
            using (financepersonalEntities dbEntity = new financepersonalEntities())
            {
                var Role = dbEntity.role.Where(m => m.RoleAttr == index).ToList();
                for (int i = 0; i < Role.Count; i++)
                {
                    DisplayFieldListItem item2 = new DisplayFieldListItem(Role[i].RoleId.ToString(), Role[i].RoleDefine1);
                    this.comboBox1.Items.Add(item2);
                }
            }
            this.comboBox1.SelectedIndex = 0;
        }

        private void BindData()
        {
            DisplayFieldListItem item = new DisplayFieldListItem( "1","收入");
            DisplayFieldListItem item1 = new DisplayFieldListItem( "2","支出");
            this.comboBox2.Items.Add(item);
            this.comboBox2.Items.Add(item1);
            this.comboBox2.SelectedIndex = 0;
            BindCombox("1");
            textBox3.Text = AddUser.UserName;
            textBox1.Text = "0";
        }

        private bool validForm(string ActionMoney,string ActionTime)
        {
            if ( !string.IsNullOrEmpty(ActionMoney) && !string.IsNullOrEmpty(ActionTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DisplayFieldListItem DFL = (DisplayFieldListItem)this.comboBox2.SelectedItem;
            DisplayFieldListItem DFL1 = (DisplayFieldListItem)this.comboBox1.SelectedItem;
           
            int ActionUserID = AddUser.UserId;
            string ActionName = DFL.ID;
            string ActionMoney = textBox1.Text.Trim();
            string ActionRole = DFL1.ID;
            string ActionTime = datePicker1.Text.Trim();
            string ActionComment = textBox6.Text.Trim();

            if (!validForm(ActionMoney, ActionTime))
            {
                Dialog.DialogWindow.CreateAlertWindow("提示", "时间及金额不可为空!", null).Show();
                return;
            }

            if (DFL.ID == "2")
            {
                ActionMoney = "-" + ActionMoney;
            }

            using (financepersonalEntities dbEntity = new financepersonalEntities())
            {
                acount ac = dbEntity.acount.Where(m => m.AcountDefine1 == AddUser.UserName).FirstOrDefault();
                ac.AcountTotal = (double.Parse(ActionMoney) + double.Parse(ac.AcountTotal)).ToString();
                dbEntity.ObjectStateManager.ChangeObjectState(ac, EntityState.Modified);

                action myAction = new action();
                myAction.ActionUserId = ActionUserID;
                myAction.ActionCountId = ac.AcountId;
                myAction.ActionName = ActionName;
                myAction.ActionRole = ActionRole;
                myAction.ActionTime = DateTime.Parse( ActionTime);
                myAction.ActionCommet = ActionComment;
                myAction.ActionMoney = ActionMoney;
                myAction.ActionDefine3 = "";
                dbEntity.action.AddObject(myAction);

                dbEntity.SaveChanges();
            }
            Dialog.DialogWindow.CreateAlertWindow("提示", "保存成功!", null).Show();
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox1.Items.Clear();
            DisplayFieldListItem item2 = (DisplayFieldListItem)this.comboBox2.SelectedItem;
            BindCombox(item2.ID);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "0") textBox.Text = "";
        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "") textBox.Text = "0";
        }
    }
}
