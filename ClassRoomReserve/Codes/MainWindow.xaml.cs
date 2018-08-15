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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace RegisterClassroom
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static string stuname;
        static string stuno;
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
             stuname = textbox1.Text.ToString().Trim();           
            stuno = passbox.Password.ToString().Trim();//取出password控件中的密码
            //label1.Content = "登录按钮被点击！！";          
            string connString = "server=localhost;database=student;uid=root;pwd=123456;SslMode = none";
            MySqlConnection conn = new MySqlConnection(connString);
            Console.WriteLine("数据库连接成功！！！");
            //label3.Content = "数据库连接成功！！！";
      
            try
            {
                conn.Open();
                String str1 = "select * from student2 where sname="+"'"+stuname+"'";
                //label2.Content = stuname;
                MySqlCommand cmd = new MySqlCommand(str1, conn); //在定义的conn对象上执行查询命令
                MySqlDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine("usrs表中的数据如下：");
                if (reader.Read())
                {
                    if (stuno.Equals(reader["sno"].ToString()))
                    {
                        //Console.WriteLine("登陆成功！！！");
                        label1.Content = "登录成功！！！即将跳转到教室预定页面....";
                        /*NavigationWindow window = new NavigationWindow();
                        window.Source = new Uri("Register.xaml");
                        window.Show();*/   //跳转页面
                        
                        Register re = new Register();
                        re.Show();
                        this.Close(); //登陆成功关闭登录页面


                    }
                    else
                    {
                        //Console.WriteLine("密码错误！！1");
                        label1.Content = "密码错误！！！";
                    }
                }
                else
                {
                         label1.Content= "不存在该用户！！您可以选择注册账户！！";
                }
                reader.Close();
                conn.Close();
                //Console.ReadKey();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void zhuce_Click(object sender, RoutedEventArgs e)
        {
            Zhuce z = new Zhuce();
            z.Show();
            //this.Hide();
        }
        //Console.ReadKey();
    }
       


    }


       

