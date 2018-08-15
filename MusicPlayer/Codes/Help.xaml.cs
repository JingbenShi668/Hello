using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Documents;
using System.Reflection;
using Windows.UI;
using Windows.Media;
using Windows.Foundation.Metadata;
using System.Globalization;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MusicPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Help : Page
    {
        const double WIDTH = 400d; //文本块的宽度
        const double HEIGHT = 500d; //文本块的高度
        const double MARGIN = 25d; //文本块的边距

        //本帮助界面，功能听起来挺简单，但在UWP下编程实现有难度
        public Help()
        {
            this.InitializeComponent();
            ComColor.ItemsSource = typeof(Windows.UI.Colors).GetProperties(); //得到Combox中各种颜色类型的对象源
            //ComFont.ItemsSource = typeof(Windows.UI.Xaml.Media.FontFamily).GetProperties();
            ComFont_Fill();  //加载系统字体库
           
           //ComFont.Items.Add("red");
        }

        public void ComFont_Fill()
        {
            ComFont.Items.Clear(); //打开帮助界面直接加载字体，为防止返回主界面后再次打开帮助界面后系统字体再次重复添加一遍，故每次Help界面加载时先清空原来的字体库
            //ComFont.Items.Add("");
            try
            {
                InstalledFont f = new InstalledFont();
                List<InstalledFont> list = new List<InstalledFont>();  //新建InstalledFont类型的数据集合
                list = f.GetFonts();
                ComFont.Items.Add(list.Count.ToString());
                //ComFont.Items.Add("red");
                for (int i = 0; i < list.Count; i++)
                {
                    ComFont.Items.Add(list[i].Name);  //将系统字体名称放入组合框中,尽管list集合中每个元素有3个属性，但是我们只需要字体名称就够了
                }
            }
            catch (Exception ex)
            {
            }
        }

       

        //动态添加控件,动态布局，RichBlock承载Paragraph的内容，当字体大小太大溢出时，将溢出内容放在挨着排列的RichTextBlockOverflow
        private void FontSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            stPanel.Children.Clear();//清除前一次字体大小下的文本内容
            String msg = "1、本播放器是一个本地播放器，用户只能从本地添加歌曲进行播放，但是时间允许的话我们可以链接服务器实现在线音乐加载。"
                  + "2、虽然不能实现在线播放，但是我们通过WebView控件可以实现在线浏览音乐功能，乐库、电台、MV、直播、节目按钮均可点击使用。"
                  + "3、该音乐播放器支持歌曲在线搜索功能，在搜索框输入歌曲名称即可搜索，其实质也就是对歌曲名称进行UTF8编码，通过字符串的拼接得到歌曲网址即得到该音乐在QQ音乐官网上的网址链接，将歌曲网页放在WebView中实现歌曲在线搜索显示。"
                  + "4、书上的音乐播放器的功能均已经实现5、帮助界面耗时很久，对于我们新手实现有难度，此界面核心控件是RichTextBlock富文本控件，因为为了实现内容字体放大后的容纳，需要用到RichTexTBlock中的RichTextBlockOverflow，配合ScrollViewer的控件实现溢出内容的滚动显示。"
                  + "其中UWP下获得系统库的所有字体有难度，但在DirectX渲染的帮助下很好的解决了。其中系统库中所有颜色的获取显示是通过数据模板解决的。"
                  + "6、由于时间有限，能力有限，好多功能都没有实现，但是如果加入了网络编程，实现连接服务器之后，我们可以在主界面放置服务器中的音乐，这样播放器就能实现真正的在线播放功能了.";

            RichTextBlock tbContent = new RichTextBlock(); //定义RichTextBlock的实例,动态插入控件
            tbContent.Width = WIDTH;
            tbContent.Height = HEIGHT;
            tbContent.TextWrapping = TextWrapping.Wrap;
            tbContent.Margin = new Thickness(MARGIN);
            Paragraph ph = new Paragraph();  //声明Paragraph的实例
            ph.TextIndent = 33;  //第一行文本的缩进量

            Run txtRun = new Run(); //格式化文本或非格式化文本的离散区域
            txtRun.Text = msg;
            ph.Inlines.Add(txtRun);
            tbContent.Blocks.Add(ph);
            tbContent.FontSize = (int)FontSlider.Value; //获取字体大小,将滑块的值设置为字体大小，滑块有问题,有时value报错(以为是浮点数的原因),强转int类型也还是不行，没有解决问题
            stPanel.Children.Add(tbContent); //向面板子元素添加
            // 更新一下状态，方便获取是否有溢出的文本
            tbContent.UpdateLayout();
            bool isflow = tbContent.HasOverflowContent;
            // 因为除了第一个文本块是RichTextBlock，
            // 后面的都是RichTextBlockOverflow一个一个接起来的
            // 所以我们需要两个变量
            RichTextBlockOverflow oldFlow = null, newFlow = null;
            if (isflow)
            {
                oldFlow = new RichTextBlockOverflow(); //初始化RichTextBlockOverflow的新实例
                oldFlow.Width = WIDTH;
                oldFlow.Height = HEIGHT;
                oldFlow.Margin = new Thickness(MARGIN);
                tbContent.OverflowContentTarget = oldFlow;
                stPanel.Children.Add(oldFlow);
                oldFlow.UpdateLayout();
                // 继续判断是否还有溢出
                isflow = oldFlow.HasOverflowContent;
            }
            while (isflow)
            {
                newFlow = new RichTextBlockOverflow();
                newFlow.Height = HEIGHT;
                newFlow.Width = WIDTH;
                newFlow.Margin = new Thickness(MARGIN);
                oldFlow.OverflowContentTarget = newFlow;
                stPanel.Children.Add(newFlow);
                newFlow.UpdateLayout();
                // 继续判断是否还有溢出的文本
                isflow = newFlow.HasOverflowContent;
                // 当枪一个变量填充了文本后，
                // 把第一个变量的引用指向当前RichTextBlockOverflow
                // 确保OverflowContentTarget属性可以前后相接
                oldFlow = newFlow;
            }
        }



        //组合框中所选择的值发生改变时调用此函数，将文字的背景颜色设置为选中的颜色
        private void ComColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            gridd.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.White);
            //ComColor.Items.Clear();   //清除子选项的内容
            //Color color1 = (Color)ComColor.SelectedValue;//提取出用户的颜色选择
            Color selectedColor = (Color)(ComColor.SelectedItem as PropertyInfo).GetValue(null, null); //直接把Object类型转化为Color类型取出属性值，不要转化为String类型，没法把String转化为Color类型，UWP与WPF命名空间有差异
            gridd.Background = new Windows.UI.Xaml.Media.SolidColorBrush(selectedColor);
            //SolidColorBrush mySolidColorBrush2 = new SolidColorBrush();
            //Color color = color1;
            //mySolidColorBrush2.Color=color.c
            //Color color = (Color)ColorConverter.ConvertFromString(color1);


            //Brush br = new SolidColorBrush(Color.FromArgb(255,0,0,0));

            //Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));



            /*if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {

                Color color3 = (Color)typeof(Colors).GetProperty(color1.GetType);

            }*/
        }

        private void ComFont_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stPanel.Children.Clear();//清除前一次字体大小下的文本内容
            String msg = "1、本播放器是一个本地播放器，用户只能从本地添加歌曲进行播放，但是时间允许的话我们可以链接服务器实现在线音乐加载。"
                  + "2、虽然不能实现在线播放，但是我们通过WebView控件可以实现在线浏览音乐功能，乐库、电台、MV、直播、节目按钮均可点击使用。"
                  + "3、该音乐播放器支持歌曲在线搜索功能，在搜索框输入歌曲名称即可搜索，其实质也就是对歌曲名称进行UTF8编码，通过字符串的拼接得到歌曲网址即得到该音乐在QQ音乐官网上的网址链接，将歌曲网页放在WebView中实现歌曲在线搜索显示。"
                  + "4、书上的音乐播放器的功能均已经实现5、帮助界面耗时很久，对于我们新手实现有难度，此界面核心控件是RichTextBlock富文本控件，因为为了实现内容字体放大后的容纳，需要用到RichTexTBlock中的RichTextBlockOverflow，配合ScrollViewer的控件实现溢出内容的滚动显示。"
                  + "其中UWP下获得系统库的所有字体有难度，但在DirectX渲染的帮助下很好的解决了。其中系统库中所有颜色的获取显示是通过数据模板解决的。"
                  + "6、由于时间有限，能力有限，好多功能都没有实现，但是如果加入了网络编程，实现连接服务器之后，我们可以在主界面放置服务器中的音乐，这样播放器就能实现真正的在线播放功能了.";

            RichTextBlock tbContent = new RichTextBlock(); //定义RichTextBlock的实例,动态插入控件
            tbContent.Width = WIDTH;
            tbContent.Height = HEIGHT;
            tbContent.TextWrapping = TextWrapping.Wrap;
            tbContent.Margin = new Thickness(MARGIN);
            Paragraph ph = new Paragraph();  //声明Paragraph的实例
            ph.TextIndent = 33;  //第一行文本的缩进量

            Run txtRun = new Run(); //格式化文本或非格式化文本的离散区域
            txtRun.Text = msg;   //文本内容
            ph.Inlines.Add(txtRun);
            tbContent.Blocks.Add(ph);
            
            //Windows.UI.Xaml.Media.FontFamily  fontfamily = (Windows.UI.Xaml.Media.FontFamily)(ComFont.SelectedValue); //获取字体样式
            //Windows.UI.Xaml.Media.FontFamily fontfamily = (Windows.UI.Xaml.Media.FontFamily)(ComFont.SelectedValue);
            tbContent.FontFamily = new Windows.UI.Xaml.Media.FontFamily(ComFont.SelectedValue.ToString()); //此处类似背景颜色先生成一个FontFamily类的实例，然后直接赋给RichBlock的背景不行，必须new FontFamily(string)
            //t1.FontFamily = new Windows.UI.Xaml.Media.FontFamily("宋体");
            stPanel.Children.Add(tbContent); //向面板子元素添加
            // 更新一下状态，方便获取是否有溢出的文本
            tbContent.UpdateLayout();
            bool isflow = tbContent.HasOverflowContent;
            // 因为除了第一个文本块是RichTextBlock，
            // 后面的都是RichTextBlockOverflow一个一个接起来的
            // 所以我们需要两个变量
            RichTextBlockOverflow oldFlow = null, newFlow = null;
            if (isflow)
            {
                oldFlow = new RichTextBlockOverflow(); //初始化RichTextBlockOverflow的新实例
                oldFlow.Width = WIDTH;
                oldFlow.Height = HEIGHT;
                oldFlow.Margin = new Thickness(MARGIN);
                tbContent.OverflowContentTarget = oldFlow;
                stPanel.Children.Add(oldFlow);
                oldFlow.UpdateLayout();
                // 继续判断是否还有溢出
                isflow = oldFlow.HasOverflowContent;
            }
            while (isflow)
            {
                newFlow = new RichTextBlockOverflow();
                newFlow.Height = HEIGHT;
                newFlow.Width = WIDTH;
                newFlow.Margin = new Thickness(MARGIN);
                oldFlow.OverflowContentTarget = newFlow;
                stPanel.Children.Add(newFlow);
                newFlow.UpdateLayout();
                // 继续判断是否还有溢出的文本
                isflow = newFlow.HasOverflowContent;
                // 当枪一个变量填充了文本后，
                // 把第一个变量的引用指向当前RichTextBlockOverflow
                // 确保OverflowContentTarget属性可以前后相接
                oldFlow = newFlow;
            }
        }




        private void return_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));//返回主页面
        }  
    }

    /// <summary>
    /// 接下来的这个类用于得到系统的字体库，由于WPF中有个字典样式(系统字体库)可在xaml中与Combobox直接绑定
    /// 而在Window10UWP中不能使用，接下来的这个类中的GetFonts()方法用于得到系统的数据库，想要获得系统字体必须通过DirectX渲染
    /// 需要通过工具按钮选项中的NuGet移入包SharpDX
    /// </summary>
    public class InstalledFont
    {
        public string Name { get; set; }

        public int FamilyIndex { get; set; }

        public int Index { get; set; }

        public  List<InstalledFont> GetFonts() //该方法返回系统的所有字体
        {
            var fontList = new List<InstalledFont>();
            var factory = new SharpDX.DirectWrite.Factory();   //引入了SharpDX这个包，，，枚举类型
            var fontCollection = factory.GetSystemFontCollection(false);
            var familyCount = fontCollection.FontFamilyCount;

            for (int i = 0; i < familyCount; i++)
            {
                var fontFamily = fontCollection.GetFontFamily(i);
                var familyNames = fontFamily.FamilyNames;
                int index;

                if (!familyNames.FindLocaleName(CultureInfo.CurrentCulture.Name, out index))
                {
                    if (!familyNames.FindLocaleName("en-us", out index))
                    {
                        index = 0;
                    }
                }

                string name = familyNames.GetString(index);
                fontList.Add(new InstalledFont()
                {
                    Name = name,
                    FamilyIndex = i,
                    Index = index   //共3个属性，当需要获取系统字体库的时候，只需要Name属性就行了，将该List集合中的所有Name属性添加到ComFont控件中即可
                });
            }

            return fontList;
            
        }

    }
}

