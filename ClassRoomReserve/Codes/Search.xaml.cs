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
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class Search : Window
    {
        public Search()
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

                if (box1.Text.ToString() != "") //当查找教室名称不为空的时候
                {
                    String classname = box1.Text.ToString().Trim();
                    //String str1 = "select * from people  where peopleId in (select   peopleId from   people group by peopleId having count(peopleId) > 1)";

                    String str1 = "select * from classroom";
                    MySqlCommand cmd = new MySqlCommand(str1, conn); //在定义的conn对象上执行查询命令
                    MySqlDataReader reader = cmd.ExecuteReader();
                    //Console.WriteLine("usrs表中的数据如下："); 
                    int Isnoused = -1; //定义布尔类型变量，教室被占用时为false
                    while (reader.Read()) //存在该教室的信息,特定教室的查找输出值不会重复
                    {
                        /*while (reader.Read())
                        {*/
                        if ((reader["ClassName"].ToString()) == (classname)) //根据特定教室名称进行筛选,Equals不能用
                        {
                            String Time = reader["RegisterTime"].ToString();
                            if (box3.Text.ToString() != "") //当查询人数不为空的时候
                            {
                                int classCapacity = Convert.ToInt16(box3.Text.ToString().Trim()); //人数
                                                                                                  //如果该教室容量可以承载预定人数
                                if ((classCapacity < (Convert.ToInt16(reader["ClassCapacity"].ToString())) || (classCapacity == (Convert.ToInt16(reader["ClassCapacity"].ToString())))))
                                {
                                    //教室查询时间限制,此时三个查询条件全部用上
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
                                                Isnoused = -2;
                                                //box4.Text += "可预订：" + "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString();

                                            }
                                            else
                                            {
                                                box4.Text += "抱歉！！当前时间的教室" + classname + "已被占用！！";
                                                Isnoused = 1; //在教室名称相同的情况下，有一个教室不可用，这个教室就不可用
                                            }
                                        }
                                        else //数据库中没有对此教室的预定
                                        {
                                            box4.Text += "可预订：" + "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString();
                                        }


                                    }

                                    else //没有查询时间，只有教室名称和教室容量两个限定条件
                                    {
                                        box4.Text += "可预订：" + "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString();
                                        break;  //防止循环输出不可预定信息
                                    }

                                }
                                else
                                {
                                    //Console.WriteLine("密码错误！！1");
                                    box4.Text += "抱歉！！该教室的容量较小！！";
                                    break;
                                }
                            }

                            else   //查询人数为空
                            {
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
                                            Isnoused = -2;
                                            //box4.Text += "可预订：" + "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString();
                                        }
                                        else
                                        {
                                            box4.Text += "抱歉！！当前时间的教室" + classname + "已被占用！！";
                                            Isnoused = 1; //在教室名称相同的情况下，有一个教室不可用，这个教室就不可用
                                        }
                                    }
                                    else //数据库中没有对此教室的预定
                                    {
                                        box4.Text += "可预订：" + "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString();
                                    }


                                }

                                else //没有查询时间，只有教室名称和教室容量两个限定条件
                                {
                                    Isnoused = -2;
                                    //box4.Text += "可预订：" + "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString();
                                }
                            }

                            //}
                            // reader.Close();//一定要加，释放资源

                            //}

                        }
                        if (Isnoused > 0) break;
                     }
                    if(Isnoused == -2) box4.Text += "可预订：" + "教室" + classname + "  容量" + reader["ClassCapacity"].ToString();
                    if (reader.Read()) { }
                    else
                    {
                        //label2.Content = "被调用！！！";
                        //box4.Text += "抱歉！！不存在您要找的大容量教室！！";
                        if (box4.Text == "") box4.Text += "抱歉！！不存在您要找的"+classname+"教室！！";
                    }
                    reader.Close();
                }
                //******1教室+容量+时间 2教室+容量 3教室+时间 4教室 ******//

                //******5容量+时间 6容量  ******//
                else//当教室名称为空时
                {
                    int Hasit = 0;
                    if (box3.Text.ToString() != "") //容量不为空时
                    {
                        int classCapacity = Convert.ToInt16(box3.Text.ToString().Trim()) - 1;
                        String str1 = "select * from classroom where ClassCapacity>" + "'" + classCapacity + "'";
                        MySqlCommand cmd = new MySqlCommand(str1, conn); //在定义的conn对象上执行查询命令
                        MySqlDataReader reader = cmd.ExecuteReader();
                        //if (reader.Read()) //存在容量限制下的教室    
                        String [] name = new String[30]; //保存教室名称的数组
                        int i = 0;name[0] = "";
                        while (reader.Read()) //先执行循环有条件的输出
                        {
                            i++;//保存通过数据库读取的下一个教室名称
                            name[i] = reader["ClassName"].ToString();//将数据库遍历的教室名称先存下来
                            if (box21.Text != "") //查询时间不为空  ""不等同与null
                            {
                                int t1 = Convert.ToInt16(box21.Text.ToString().Trim());
                                int t2 = Convert.ToInt16(box22.Text.ToString().Trim());
                                int t3 = Convert.ToInt16(box23.Text.ToString().Trim());
                                int t4 = Convert.ToInt16(box24.Text.ToString().Trim());//查询时间的输入值
                                int m1 = t1 * 60 + t2;
                                int m2 = t3 * 60 + t4;
                                String Time2 = reader["RegisterTime"].ToString();
                                //数据库中的此教室有RegisterTime时
                                if (Time2 != "")
                                {
                                    int s1 = Convert.ToInt16(Time2.Substring(0, 2));
                                    int s2 = Convert.ToInt16(Time2.Substring(3, 2));
                                    int s3 = Convert.ToInt16(Time2.Substring(9, 2));
                                    int s4 = Convert.ToInt16(Time2.Substring(12, 2));
                                    int n1 = s1 * 60 + s2;
                                    int n2 = s3 * 60 + s4;
                                    if (m2 < n1 || m1 > n2) //当查询时间在已有预约时间之外，即可以预定，只显示符合要求的教室
                                    {
                                        for (int m = 0; m < i; m++)
                                        {
                                            //label3.Content += name[m];
                                            if ((reader["ClassName"].ToString()) != name[m]) { continue; }
                                            if ((reader["ClassName"].ToString()) == name[m]) //保证教室名称不重复输出
                                            {
                                                Hasit = 1;  //教室名字已经存在
                                                            //box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                                            //break;
                                            }
                                        }
                                        if (Hasit == 0) box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                        Hasit = 0;//执行清零操作，把上一个符合条件的教室的Hasit清除

                                    }

                                }
                                else //数据库中没有对此教室的预定
                                {
                                    box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                    /*for (int m = 0; m < i + 1; m++)
                                    {
                                        if ((reader["ClassName"].ToString()) == name[m]) //保证教室名称不重复输出
                                        {
                                            Hasit = 1;  //教室名字已经存在
                                          //box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                                        //break;
                                        }
                                    }*/
                                   
                                    //box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                }

                            }

                            else //没有查询时间，只有教室容量一个限定条件
                            {
                                for (int m = 0; m <i; m++)
                                {
                                    //label3.Content += name[m];
                                    if ((reader["ClassName"].ToString()) != name[m]) { continue; }
                                        if ((reader["ClassName"].ToString()) == name[m]) //保证教室名称不重复输出
                                    {
                                        Hasit = 1;  //教室名字已经存在
                                        //box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                        //break;
                                    }
                                }
                                if(Hasit == 0) box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                Hasit = 0; //执行清零操作，把上一个符合条件的教室的Hasit清除
                                /*if((box4.Text =="") && (name[0]!=null))
                                box4.Text += "可预订：" +"教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";*/
                                //box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                            }
                            



                        }
        
                        if (reader.Read()) { }
                        else
                        //if(reader.Read())
                        {
                            //label2.Content = "被调用！！！";
                            //box4.Text += "抱歉！！不存在您要找的大容量教室！！";
                            if (box4.Text == "") box4.Text += "抱歉！！不存在您要找的大容量教室！！";

                        }
                        reader.Close();  //一定要有关闭资源
                       
                    }

                    /****** 7时间 ******/
                    else //容量为空时，只有时间作为限制
                    {
                        String str1 = "select * from classroom ";  //遍历所有数据然后筛选
                        MySqlCommand cmd = new MySqlCommand(str1, conn); //在定义的conn对象上执行查询命令
                        MySqlDataReader reader = cmd.ExecuteReader();
                        //if (reader.Read()) //存在容量限制下的教室    
                        String[] name = new string[30];
                        int i = 0;name[0] = "";
                        //int Iscanuse = 0; //定义布尔类型变量，教室被占用时为false
                        while (reader.Read()) //先执行循环有条件的输出
                        {
                            i++;//保存通过数据库读取的下一个教室名称
                            name[i] = reader["ClassName"].ToString();//将数据库遍历的教室名称先存下来
                                                                     //if (box21.Text != "") //查询时间不为空  ""不等同与null
                            int t1 = Convert.ToInt16(box21.Text.ToString().Trim());
                                int t2 = Convert.ToInt16(box22.Text.ToString().Trim());
                                int t3 = Convert.ToInt16(box23.Text.ToString().Trim());
                                int t4 = Convert.ToInt16(box24.Text.ToString().Trim());//查询时间的输入值
                                int m1 = t1 * 60 + t2;
                                int m2 = t3 * 60 + t4;
                                String Time2 = reader["RegisterTime"].ToString();
                                //数据库中的此教室有RegisterTime时
                                if (Time2 != "")
                                {
                                int s1 = Convert.ToInt16(Time2.Substring(0, 2));
                                int s2 = Convert.ToInt16(Time2.Substring(3, 2));
                                int s3 = Convert.ToInt16(Time2.Substring(9, 2));
                                int s4 = Convert.ToInt16(Time2.Substring(12, 2));
                                int n1 = s1 * 60 + s2;
                                    int n2 = s3 * 60 + s4;
                                    if (m2 < n1 || m1 > n2) //当查询时间在已有预约时间之外，即可以预定，只显示符合要求的教室
                                    {
                                    for (int m = 0; m < i ; m++)
                                    {                       
                                        if ((reader["ClassName"].ToString()) == name[m]) //保证教室名称不重复输出
                                        {
                                            Hasit = 1;  //教室名字已经存在
                                                        //box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";                                                   //break;
                                        }

                                        /************相同名称教室中的第一个教室的预定时间不满足输出正常，但是第二个.....***********/
                                    }
                                    if ((Hasit == 0)) box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                    Hasit = 0; //清除原来数据
                                               //label3.Content += reader["ClassName"].ToString();

                                    /*for (int m = 0; m < i + 1; m++) //在该教室名称下，第一个出现的该教室不满足时间限制，则直接跳过接下来同名称的该教室
                                    {
                                        if ((reader["ClassName"].ToString()) == name[m]) 
                                        {
                                            Hasit = 1;  
                                                        //box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";                                     //break;
                                        }

                                    }//此处之后不加代码，会漏掉数据库中最后一个满足条件的教室*/

                                    /***********有待完善！！**********/


                                    // box4.Text += "可预订：" +"教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                    //box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                    //if ((reader["ClassName"].ToString()) == name[i + 1]) { box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n"; }
                                }
                                    
                               

                            }
                                else //数据库中没有对此教室的预定
                                {
                                    //box4.Text += "可预订：" +"教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                    box4.Text += "教室" + reader["ClassName"].ToString() + "  容量" + reader["ClassCapacity"].ToString() + "\n";
                                }
                           

                        }

                        if (reader.Read()) { }
                        else
                        //if(reader.Read())
                        {
                            //label2.Content = "被调用！！！";
                            //box4.Text += "抱歉！！不存在您要找的大容量教室！！";
                            if (box4.Text == "") box4.Text += "抱歉！！在该时间不存在可预订的教室！！";

                        }
                        reader.Close();
                    }

                    conn.Close();  //一定要加，释放资源
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
            this.Close();
        }
    }
    }

