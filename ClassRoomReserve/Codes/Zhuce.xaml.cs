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
using MySql.Data.MySqlClient;

namespace RegisterClassroom
{
    /// <summary>
    /// Zhuce.xaml 的交互逻辑
    /// </summary>
    public partial class Zhuce : Window
    {
        public Zhuce()
        {
            InitializeComponent();
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();//关闭当前页面
        }*/

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String stuname = textbox1.Text.ToString().Trim();
            String stuno = textbox2.Text.ToString().Trim(); //取出用户的注册信息

            string connString = "server=localhost;database=student;uid=root;pwd=123456;SslMode = none";
            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                String str1 = "insert into student2 values('" + stuname + "','" + stuno + "')";
                MySqlCommand cmd = new MySqlCommand(str1, conn); //在定义的conn对象上执行查询命令
                //MySqlDataReader reader = cmd.ExecuteReader();
                cmd.ExecuteNonQuery(); //执行sql语句

                label1.Content = "注册成功！！您可以在主界面进行登录操作！！";


                conn.Close();
                //Console.ReadKey();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
