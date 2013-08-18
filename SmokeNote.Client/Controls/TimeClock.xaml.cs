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
using System.Windows.Threading;

namespace SmokeNote.Client.Controls
{
    /// <summary>
    /// TimeClock.xaml 的交互逻辑
    /// </summary>
    public partial class TimeClock : UserControl
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }


        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(TimeClock), new PropertyMetadata(DateTime.Now));


        public DispatcherTimer MainTimer { get; set; }
        public TimeClock()
        {
            InitializeComponent();
            this.MainTimer = new DispatcherTimer();
            this.MainTimer.Tick += MainTimer_Tick;
            this.MainTimer.Interval = TimeSpan.FromSeconds(1);
            this.MainTimer.Start();

            this.MouseLeftButtonDown += TimeClock_MouseLeftButtonDown;
        }

        void TimeClock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // this.DragMove();
        }

        void MainTimer_Tick(object sender, EventArgs e)
        {
            this.CurrentTime = DateTime.Now;
        }
    }
}
