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

namespace SmokeNote.Client.Views
{
    /// <summary>
    /// NoteEditor.xaml 的交互逻辑
    /// </summary>
    public partial class NoteEditor : UserControl
    {
        public NoteEditor()
        {
            InitializeComponent();
        }

        private void NoteEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                grdNoNote.Visibility = System.Windows.Visibility.Collapsed;
                grdEditor.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                grdNoNote.Visibility = System.Windows.Visibility.Visible;
                grdEditor.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
