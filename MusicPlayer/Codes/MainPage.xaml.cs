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
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Windows;
using System.Net;
using System.Net.Http;
using System.Text;



// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MusicPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TextBlock txtPlayStatus = new TextBlock();

        bool isPlaying = false;
        List<Song> playList = new List<Song>();
        //Song类已经在Song.cs定义过,创建列表playList,其中playList是用于存储或检索歌曲的集合
        Song currentSong = null;    //currentSong是用于存储或检索当前播放歌曲的引用
        int currentSongIndex;       //定义于存储或检索当前播放歌曲在playList集合中的索引号

        public MainPage()
        {
            this.InitializeComponent();
            Storyboard1.Begin();   //开始底部TextBlock的动画
            txtPlayStatus.Name = "txtPlayStatus";
            txtPlayStatus.Margin = new Thickness(390, 535, 35, 198);
            txtPlayStatus.FontSize = 18;
            txtPlayStatus.TextAlignment = TextAlignment.Center;
            grdMsicPlayer.Children.Add(txtPlayStatus);
        }

        private void addSongToPlayList()            //往音乐列表中添加音乐的方法
        {
            /*playList.Add(new Song("Flo Rida-Whistle", "ms-appx:///MediaCollections/Thumbnails/1.jpg",
                "ms-appx:///MediaCollections/Music/Flo Rida-Whistle.mp3"));
            playList.Add(new Song("Sakura Tears", "ms-appx:///MediaCollections/Thumbnails/3.jpg",
                "ms-appx:///MediaCollections/Music/Sakura Tears.mp3"));
            playList.Add(new Song("Lirenchou", "ms-appx:///MediaCollections/Thumbnails/5.jpg",
                "ms-appx:///MediaCollections/Music/Lirenchou.mp3"));*/
            //文件路径中的ms=-appx:/// 前缀引用应用的安装目录
        }

        async void loadSelectedSong(string songToSelect) //async异步执行方法
        {
            try
            {
                pauseSong();             //停止正在播放的歌曲
                mediaSource.Stop();      //重新设置media元素
                foreach (Song s in playList)                //foreach方法循环访问数组或集合
                    if (s.SongTitle == songToSelect.Trim())  //Trim()即从当前 String 对象移除所有前导空白字符和尾部空白字符
                        currentSong = s;
                mediaSource.PosterSource = currentSong.SongThumbnail;
                fadeinThumbnail.Begin();   //开始执行淡入动画
                //OpenAsync用指定的选项在指定文件中打开一个随机访问流
                var streamedSong = await currentSong.SongFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                //mediaSource.Source = new Uri(currentSong.SongPath);  //添加当前选中的歌曲的路径给媒体路径
                mediaSource.SetSource(streamedSong, currentSong.SongFile.ContentType);//使用指定的流设置Source属性
            }
            catch
            {

            }


        }

        void loadPlayList()
        {
            lstViewPlaylist.Items.Clear();//每次执行该应用时播放列表为空，即歌曲添加到播放列表前，清空列表 
            foreach (Song s in playList)
                lstViewPlaylist.Items.Add(" " + s.SongTitle);
        }

        private void lstViewPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*currentSongIndex = lstViewPlaylist.SelectedIndex;
            loadSelectedSong(lstViewPlaylist.SelectedItem as string);
            mediaSource.Stop();                 //媒体控件自带的stop()方法
            bttnPlay.Visibility = Windows.UI.Xaml.Visibility.Visible;
            bttnPause.Visibility = Windows.UI.Xaml.Visibility.Collapsed;*/
            fadeoutThumbnail.Begin();//开始执行淡出动画

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)   //函数重载
        {
            //addSongToPlayList();
            loadPlayList();

            sldrVolume.Value = sldrVolume.Maximum;
            prgsBarSongPosition.Value = 0.0;
            prgsBarSongPosition.ValueChanged += prgsBarSongPosition_ValueChanged;
            //lstViewPlaylist.SelectedIndex = 0;


        }
        //lstViewPlaylist_RightTapped
        private async void lstViewPlaylist_RightTapped(Object sender, RightTappedRoutedEventArgs e)
        {

            PopupMenu pMenu = new PopupMenu();//定义弹出菜单
            UICommand cmd = new UICommand(); //UiCommand类的实例，向弹出菜单添加命令
            ListBoxItem rightTappedSong = null;
            UIElement controls = e.OriginalSource as UIElement; //e.OriginalSource获取引发事件的对象的引用
            while (controls != null && controls != sender as UIElement)
            {
                if (controls is ListBoxItem)
                {
                    rightTappedSong = controls as ListBoxItem;//存储用户右击的歌曲的引用
                    if (currentSong != null && currentSong.SongTitle == (rightTappedSong.Content as string).Trim() && isPlaying)
                    {
                        cmd.Label = "Pause"; //如果右击正在播放的歌曲，则右击弹出菜单显示pause
                        cmd.Id = "pause";
                    }
                    else
                    {
                        cmd.Label = "Play";
                        cmd.Id = "play";
                    }
                    pMenu.Commands.Add(cmd); //cmd中的命令添加到了弹出菜单里
                    UICommand cmdDeleteFile = new UICommand();
                    cmdDeleteFile.Id = "Delete";
                    cmdDeleteFile.Label = "Delete";
                    pMenu.Commands.Add(cmdDeleteFile);
                    break;
                }
                controls = VisualTreeHelper.GetParent(controls) as UIElement;// 返回可视树中某一对象的父对
            }

            UICommand cmdaddFile = new UICommand();//创建UICommand的实例，该对象负责在弹出菜单上显示一个标签，并且具有唯一Id
            cmdaddFile.Label = "Add";
            cmdaddFile.Id = "Add";
            pMenu.Commands.Add(cmdaddFile); //将cmdaddFile的命令添加到弹出菜单

            //pMenu的ShowForSelectionAsync()异步方法，用于显示弹出菜单
            var selectedCommand = await pMenu.ShowForSelectionAsync(new Rect(e.GetPosition(null), e.GetPosition(null)));
            if (selectedCommand != null)
            {
                if (selectedCommand.Id.ToString() == "Add")
                {
                    Windows.Storage.Pickers.FileOpenPicker fop = new
                        Windows.Storage.Pickers.FileOpenPicker();//创建文件打开捡取器的实例
                    fop.FileTypeFilter.Add(".mp3");
                    fop.FileTypeFilter.Add(".wav");
                    fop.FileTypeFilter.Add(".mp4");//可播放的文件类型，文件类型过滤器的Add方法
                    fop.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
                    //设置到Music文件夹默认的浏览位置
                    var selectedFile = await fop.PickSingleFileAsync();
                    if (selectedFile != null) //判断用户是否选择了文件
                    {
                        playList.Add(new Song(selectedFile.DisplayName, "ms-appx:///MediaCollections/Thumbnails/1.jpg", selectedFile));
                        loadPlayList();  //重新加载音乐播放列表
                    }
                }
                else
                {
                    for (int i = 0; i < playList.Count; i++) //构建播放列表
                    {
                        if (playList[i].SongTitle.Trim() == rightTappedSong.Content.ToString().Trim())
                        {
                            if (selectedCommand.Id.ToString() == "play")
                            {
                                lstViewPlaylist.SelectedIndex = i; //用户选择播放歌曲
                                await Task.Delay(1500);  //延迟1.5秒
                                playSong();   //开始播放选中的歌曲
                                break;
                            }
                            //如果用户想停止当前播放的音乐
                            else if (selectedCommand.Id.ToString() == "pause")
                            {
                                pauseSong();   //
                                break;
                            }
                            else if (selectedCommand.Id.ToString() == "Delete")//删除音乐
                            {
                                if (currentSong != null && currentSong.SongFile == playList[i].SongFile)
                                {
                                    currentSong = null;
                                    mediaSource.Source = null;
                                    prgsBarSongPosition.Value = 0.0; //重置进度条的位置
                                    pauseSong();  //停止播放音乐

                                }
                                playList.RemoveAt(i); //从播放列表中移除歌曲
                                loadPlayList();   //重新加载播放列表
                                break;
                            }

                        }
                    }
                }
            }

        }



        public void fadeoutThumbnail_Completed(Object sender, object e)
        {   //一旦淡出动画开始，设置当前歌曲索引，加载选中歌曲
            currentSongIndex = lstViewPlaylist.SelectedIndex;
            loadSelectedSong(lstViewPlaylist.SelectedItem as String);
        }


        //进度条进度的变化
        void prgsBarSongPosition_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Math.Abs(e.NewValue - e.OldValue) > 100)
                mediaSource.Position = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(prgsBarSongPosition.Value));

            txtPlayStatus.Text = mediaSource.Position.Hours + " :" +
                mediaSource.Position.Minutes + " :" + mediaSource.Position.Seconds + " /" +
                mediaSource.NaturalDuration.TimeSpan.Hours + " :" +
                 mediaSource.NaturalDuration.TimeSpan.Minutes + " :" +
                  mediaSource.NaturalDuration.TimeSpan.Seconds;

        }


        void playSong()
        {
            prgsBarSongPosition.Minimum = 0.0;
            prgsBarSongPosition.Maximum =
                mediaSource.NaturalDuration.TimeSpan.TotalMilliseconds;
            bttnPause.Visibility = Windows.UI.Xaml.Visibility.Visible;
            bttnPlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            mediaSource.Play();
            isPlaying = true;
            displayProgress();
        }

        async void displayProgress() //异步方法
        {
            await Task.Delay(1000);
            prgsBarSongPosition.Value = mediaSource.Position.TotalMilliseconds;
            if (isPlaying)
                if (mediaSource.Position.TotalMilliseconds <
                    mediaSource.NaturalDuration.TimeSpan.TotalMilliseconds)
                    displayProgress();
                else
                {
                    if (!mediaSource.IsLooping)
                    {
                        isPlaying = false;
                        bttnPause.Visibility =
                            Windows.UI.Xaml.Visibility.Collapsed;
                        bttnPlay.Visibility =
                             Windows.UI.Xaml.Visibility.Visible;

                    }
                    else
                        displayProgress();

                }

        }

        private void bttnPlay_Click(object sender, RoutedEventArgs e)
        {
            playSong(); //调用playSong()方法

        }

        void pauseSong()
        {
            bttnPause.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            bttnPlay.Visibility = Windows.UI.Xaml.Visibility.Visible;
            mediaSource.Pause();
            isPlaying = false;


        }

        private void bttnPause_Click(object sender, RoutedEventArgs e)
        {
            pauseSong();
        }

        private void bttnNext_Click(Object sender, RoutedEventArgs e)
        {

            if (currentSongIndex < playList.Count - 1)

                currentSongIndex++;

            if (isPlaying)
                mediaSource.Stop();
            lstViewPlaylist.SelectedIndex = currentSongIndex;

        }

        private void bttnPrevious_Click(Object sender, RoutedEventArgs e)
        {

            if (currentSongIndex > 0)
                currentSongIndex--;


            if (currentSongIndex > 0 && currentSongIndex < playList.Count)
            {
                mediaSource.Play();
                lstViewPlaylist.SelectedIndex = currentSongIndex;
                playSong();
            }




        }

        private void sldrVolume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaSource.Volume = sldrVolume.Value / 100;

        }

        private void tglSwtchMediaRepeat_Toggled(object sender, RoutedEventArgs e)
        {
            mediaSource.IsLooping = tglSwtchMediaRepeat.IsOn; //循环播放当前歌曲

        }

        public void click1(object sender, RoutedEventArgs e)
        {
            string url = "http://www2.kugou.kugou.com/yueku/v8/html/home.html";
            //web.Source= url;
        }
        public void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(SearchContent != null)
            {
                String ur = "https://y.qq.com/portal/search.html#page=1&searchid=1&remoteplace=txt.yqq.top&t=song&w=";
                String s = SearchContent.Text;
                byte[] bytes = Encoding.UTF8.GetBytes(s);


                foreach (int i in bytes)
                {
                    ur += '%' + i.ToString("X");  //.ToString返回该对象的字符串表示
                }
                //SearchContent.Text = ur;
                Uri u = new Uri(ur);
                web.Source = u;

            }

        }

        public void back_click(object sender, RoutedEventArgs e)
        {
            //web.CanGoForward();
        }

        private void web_Checked(object sender, RoutedEventArgs e)
        {
            Button s = sender as Button;
            if (s.Name == "yueku")
            {
                Uri u = new Uri("http://www2.kugou.kugou.com/yueku/v8/html/home.html");
                web.Source = u;
            }
            if (s.Name == "fm")
            {
                Uri u = new Uri("http://www2.kugou.com/fm2/index.html?ver=");
                web.Source = u;
            }
            if (s.Name == "mv")
            {
                Uri u = new Uri("http://www2.kugou.kugou.com/mv/v8/mtv/index/getData.js?cdn=cdn");
                web.Source = u;
            }
            if (s.Name == "show")
            {
                Uri u = new Uri("http://www2.kugou.com/show/html/index.html?ver=7677");
                web.Source = u;
            }
            if (s.Name == "jiemu")
            {
                Uri u = new Uri("http://fanxing.kugou.com/index.php?action=miniIndexNew&id=0&ver=7677");
                web.Source = u;
            }
            if (s.Name == "lrc")
            {


            }
        }

        /*private void web_Checked(object sender, RoutedEventArgs e)
        {
            if (web == null) return;

            string urll = "http://www2.kugou.kugou.com/yueku/v8/html/home.html";

            web.Visibility = Visibility.Visible;
            CanvasLyric.Visibility = Visibility.Collapsed;

            RadioButton rd = sender as RadioButton;
            if (rd.Name == "yueku")
            {
                //urll = "http://www2.kugou.kugou.com/yueku/v8/html/home.html";
                Uri u = new Uri("http://baidu.com");
                web.Source = u;
            }
            if (rd.Name == "fm")
            {
                urll = "http://www2.kugou.com/fm2/index.html?ver=";
                //web.Source = 

            }
            if (rd.Name == "mv")
            {
                urll = "http://www2.kugou.kugou.com/mv/v8/mtv/index/getData.js?cdn=cdn";

            }
            if (rd.Name == "show")
            {
                urll = "http://www2.kugou.com/show/html/index.html?ver=7677";

            }
            if (rd.Name == "jiemu")
            {
                urll = "http://fanxing.kugou.com/index.php?action=miniIndexNew&id=0&ver=7677";

            }
            if (rd.Name == "lrc")
            {
                web.Visibility = Visibility.Collapsed;
                CanvasLyric.Visibility = Visibility.Visible;

            }


            //if (web != null)
            //{
            //}
                //web.Source = "http://fanxing.kugou.com/index.php?action=miniIndexNew&id=0&ver=7677";
                //Uri uri = new Uri(www.baidu.com);
                //Uri s = new Uri(urll);
                //this.web.Source = s;
                //  this.web.Navigate(urll);

        }*/

        private void Helpbtn_clicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Help));//帮助界面跳转
        }

        private void WebView_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }
        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            //WindowState = WindowState.Minimized;
            //this.Width = MinWidth;
            //this.Height = MinHeight;


        }


        private void OnBtnClose(object sender, RoutedEventArgs e)
        {

            // 在此处添加事件处理程序实现。
            //e.Cancel = false;//关闭窗口
            //Application.Current.shutdown();  //button不能强制关闭应用程序，但是可以关闭本窗口
            //this.hide
            //MainPage m = new MainPage();
            //this.Visibility = false;
            //Application.
            /*messageBox.SetText("\t\t您是否要退出？");
            var closeConfirm = messageBox.ShowDialog();
            if (closeConfirm.Value)
            {
                Application.Current.Shutdown();
            }*/
            /*Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog("Are you determined to exit?");
            UICommand continueCommand = new UICommand();
            continueCommand.Label = "Continue";
            continueCommand.Id = 1;
            msg.Commands.Add(continueCommand);

            UICommand cancelCommand = new UICommand();
            continueCommand.Label = "Cancel";
            continueCommand.Id = 2;
            msg.Commands.Add(cancelCommand);

            Windows.UI.Popups.IUICommand selectedCommand = await msg.ShowAsync();
            if((int)selectedCommand.Id == 2)
            {
                
            }*/

        }
    }
}
