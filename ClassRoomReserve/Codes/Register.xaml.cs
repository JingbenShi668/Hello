using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RegisterClassroom
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }*/

        private void Button_Click_1(object sender, RoutedEventArgs e) //查询页面
        {
            Search s = new Search();
            s.Show();
            this.Close(); //打开新页面，关闭该页面
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //预定页面
        {
            Select selection = new Select();
            selection.Show();//打开新页面，关闭该页面
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Help h = new Help();
            h.Show();
            this.Close();
        }
    }
}
