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
    /// Select.xaml 的交互逻辑
    /// </summary>
    public partial class Select : Window
    {
        public Select()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connString = "server=localhost;database=student;uid=root;pwd=123456;SslMode = none";
            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                String RegisterName = box3.Text.ToString().Trim(); //预订者姓名
                int Isnoused = -1; //定义布尔类型变量，教室被占用时为false
                int Isnull = -1; //所预定教室没有时间限制
                String classname = "";//设置成为全局变量，预定教室名称 
                int classcapacity = 0; //全局变量，教室容量
                String time = ""; ////全局变量，预定时间
                if (box3.Text.ToString() =="") box4.Text += "抱歉！！预定失败！！预订信息中预定者姓名不能为空！！"+"\n";
                if (box1.Text.ToString() != "") //当查找教室名称不为空的时候
                {
                    classname = box1.Text.ToString().Trim(); //预定教室名称      
                    time = box21.Text.ToString().Trim() + ":" + box22.Text.ToString().Trim() + " -- " + box23.Text.ToString().Trim() + ":" + box24.Text.ToString().Trim();//预定时间
                    String str1 = "select * from classroom";
                    //string str2 = "";
                    //string str3 = "insert into classroom(ClassName,ClassCapacity,RegisterTime,RegisterName)values('103', '40' , '08:40 -- 09:20', 'ben')";
                    MySqlCommand cmd = new MySqlCommand(str1, conn); //在定义的conn对象上执行查询命令
                   
                    //MySqlCommand cmd3 = new MySqlCommand(str3, conn);
                    //cmd3.ExecuteNonQuery(); //执行插入操作
                    MySqlDataReader reader = cmd.ExecuteReader();
                    //Console.WriteLine("usrs表中的数据如下："); 
                    
                    while (reader.Read()) //存在该教室的信息,特定教室的查找输出值不会重复
                    {
                        /*while (reader.Read())
                        {*/
                        if ((reader["ClassName"].ToString()) == (classname)) //根据特定教室名称进行筛选,Equals不能用
                        {
                            box4.Text += " ";
                            String Time = reader["RegisterTime"].ToString();
                            if (box5.Text.ToString() != "") //当查询人数不为空的时候
                            {
                                int classCapacity = Convert.ToInt16(box5.Text.ToString().Trim()); //人数                                                                                                 
                                //教室查询时间限制,此时三个查询条件全部用上
                                if ((classCapacity < (Convert.ToInt16(reader["ClassCapacity"].ToString())) || (classCapacity == (Convert.ToInt16(reader["ClassCapacity"].ToString())))))
                                {
                                    classcapacity = (Convert.ToInt16(reader["ClassCapacity"].ToString()));
                                    //当前教室存在且教室容量允许的情况下
                                    if (box21.Text != "") //查询时间不为空  ""不等同与null
                                    {
                                        int t1 = Convert.ToInt16(box21.Text.ToString().Trim());
                                        int t2 = Convert.ToInt16(box22.Text.ToString().Trim());
                                        int t3 = Convert.ToInt16(box23.Text.ToString().Trim());
                                        int t4 = Convert.ToInt16(box24.Text.ToString().Trim());//查询时间的输入值
                                        int m1 = t1 * 60 + t2;
                                        int m2 = t3 * 60 + t4;
                                       
                                        //数据库中的此教室有RegisterTime时
                                        if (Time != "")
                                        {
                                            int s1 = Convert.ToInt16(Time.Substring(0, 2));
                                            int s2 = Convert.ToInt16(Time.Substring(3, 2));
                                            int s3 = Convert.ToInt16(Time.Substring(9, 2));
                                            int s4 = Convert.ToInt16(Time.Substring(12, 2));
                                            //label.Content = Time.Substring(0, 1) + Time.Substring(2, 2) + Time.Substring(8, 1) + Time.Substring(10, 2);
                                            int n1 = s1 * 60 + s2;
                                            int n2 = s3 * 60 + s4;
                                            if (m2 < n1 || m1 > n2) //当查询时间在已有预约时间之外
                                            {
                                                Isnoused = -2; //该教室名称下的该教室的该次查询时间满足
                                            }
                                            else
                                            {
                                                box4.Text += "抱歉！！当前时间的教室" + classname + "已被占用！！" + "\n";
                                                Isnoused = 1; //在教室名称相同的情况下，有一个教室不可用，这个教室就不可用
                          
                                            }
                                        }
                                        else //数据库中没有对此教室的预定
                                        {
                                            Isnull = 2;
                                            //str2 = "insert into classroom(ClassName,ClassCapacity,RegisterTime,RegisterName)values('" + classname + "', '' , '"+ time + "', '" + RegisterName+ "')";
                                            
                                            //cmd2.ExecuteNonQuery();
                                            /*box4.Text += "可预订：" + "教室" + classname + "  容量" + reader["ClassCapacity"].ToString();
                                            box4.Text += "您所预定的" + time + "的" + classname + "预定成功！！！" + "\n";*/
                                            break;
                                        }


                                    }
                                    else //没有查询时间，只有教室名称和教室容量两个限定条件，查询时间不能为空
                                    {
                                        box4.Text += "抱歉！！预定失败！！预订信息中预定时间不能为空！！" + "\n";
                                        break;
                                        //box4.Text += "可预订：" + "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString();
                                        //break;  //防止循环输出不可预定信息
                                    }
                                }
                                else
                                {
                                    //Console.WriteLine("密码错误！！1");
                                    box4.Text += "抱歉！！该教室的容量较小！！" + "\n";
                                    break;
                                }

                            }
                            else  //预定人数不能为空
                            {
                                box4.Text += "抱歉！！预定失败！！预订信息中预定人数不能为空！！" + "\n";
                                break;
                            }
                            if (Isnoused > 0) break;
                        }
                    }
                    
                        if (reader.Read()) { }
                        else
                        {
                            //label2.Content = "被调用！！！";
                            //box4.Text += "抱歉！！不存在您要找的大容量教室！！";
                            if (box4.Text == "") box4.Text += "抱歉！！预定失败！！不存在您要找的" + classname + "教室！！" + "\n";
                        }
                        reader.Close(); //释放资源
                       
                    }
                else//教室名称不可为空
                {
                    box4.Text += "抱歉！！预定失败！！预订信息中预定教室名称不能为空！！" + "\n";
                }
                if ((Isnoused == -2)||(Isnull==2))
                {
                    /*box4.Text += "可预订：" + "教室" + classname + "  容量" + reader["ClassCapacity"].ToString();
                    string str2 = "insert into classroom(ClassName,ClassCapacity,RegisterTime,RegisterName)values('" + classname + "', '' , '" + time + "', '" + RegisterName + "')";
                    MySqlCommand cmd2 = new MySqlCommand(str2, conn);
                    cmd2.ExecuteNonQuery();*/
                    //str2 = "insert into classroom(ClassName,ClassCapacity,RegisterTime,RegisterName)values('" + classname + "', '' , '" + time + "', '" + RegisterName + "')";
                    String str2 = "insert into classroom(ClassName, ClassCapacity, RegisterTime, RegisterName)values('" + classname + "', "+ classcapacity +", '" + time + "', '" + RegisterName + "')";
                    MySqlCommand cmd2 = new MySqlCommand(str2, conn);
                    cmd2.ExecuteNonQuery();
                    box4.Text += "您所预定的"  + time+"的"+classname +  "教室预定成功！！！" + "\n";
                    conn.Close();
                }

                
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Register re = new Register();
            re.Show();
            this.Close();//返回功能界面，关闭此页面
        }
    }
}
