using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Kek
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool FullScreen = false;
        string ImgCollection = "";
        string Song = "";

        int IndexImage = 0;

        DispatcherTimer Timer;
        public MainWindow()
        {
            InitializeComponent();

            Timer = new DispatcherTimer();
            Timer.Interval += TimeSpan.FromSeconds(2);
            Timer.Tick += Timer_Tick;
            Timer.Start();
            //tempInfo.Content = "ID: ";

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SlideShow();
        }

        public void SlideShow()
        {
            //tempInfo.Content = $"ID: {IndexImage}";
            try
            {
                if (ImgCollection != "")
                {
                    var images = (from p in Directory.GetFiles(ImgCollection) where p.EndsWith(".png") | p.EndsWith(".webp") | p.EndsWith(".jpg") | p.EndsWith(".gif") select p).ToArray();
                    if(!(IndexImage >= images.Length))
                    {
                        imgBk.Source = new BitmapImage(new Uri(images[IndexImage]));
                        IndexImage++;
                    }
                    else
                    {
                        IndexImage = 0;
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Ошибка!\n{ex}");
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if(e.Key == Key.E)
            {
                WindowSettings windowSettings = new WindowSettings();
                if (windowSettings.ShowDialog() == false)
                {
                    ImgCollection = windowSettings.FolderPath;
                    Song = windowSettings.FilePath;
                }
            }

            if (e.Key == Key.F12 & !FullScreen)
            {
                FullScreen = true;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
            else if (e.Key == Key.F12 | e.Key == Key.Escape)
            {
                FullScreen = false;
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }


    }
}
