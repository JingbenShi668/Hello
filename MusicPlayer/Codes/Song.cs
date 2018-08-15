using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicPlayer
{
    class Song     //声明song类的构造函数
    {
        public Windows.Storage.StorageFile SongFile { get; set; }

        public string SongTitle { get; set; }   //歌曲名字
        public string SongThumbNailPath { get; set; }  //用于存储或检索歌曲缩略图图像的路径
        public BitmapImage SongThumbnail  //用于SongThumbNailPath属性检索歌曲的缩略图位图图像
        {
            get
            {
                if (String.IsNullOrEmpty(SongThumbNailPath))

                    return new BitmapImage();
                else
                    return (new BitmapImage(new Uri(SongThumbNailPath)));

            }

        }
        //public string SongPath { get; set; }  //用于存储或检索歌曲文件的路径
        public Song(string title, string thumbnailImagePath, Windows.Storage.StorageFile file)
        {
            SongTitle = title;
            SongThumbNailPath = thumbnailImagePath;
            SongFile = file;   //歌曲路径，手动寻址  使用file变量中提供的初值初始化SongFile属性


        }



    }
}
