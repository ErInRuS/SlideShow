using Microsoft.Win32;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Media;
using NAudio.Wave;

namespace Kek
{
    /// <summary>
    /// Логика взаимодействия для WindowSettings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        public string FolderPath { get; set; }
        public string FilePath { get; set; }
        public WindowSettings()
        {
            InitializeComponent();
            btnTest.IsEnabled = false;
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            string folderName = "";
            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            dlg.AllowNonFileSystemItems = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok   )
            {
                if(dlg.FileName != "")
                {
                    FolderPath = dlg.FileName;
                    folderName = dlg.FileName;
                }
            }
            FolderPath = folderName;
            lblFolder.Content = $"Папка колекции: {folderName}";
        }

        private void btnOpenSong_Click(object sender, RoutedEventArgs e)
        {
            string songName = "";
            var dlg = new OpenFileDialog();
            //dlg.Filter = "Файл (*.wav;*.mp3)|*.wav;*.mp3|Все файлы |*.*";
            dlg.Filter = "Файл (*.wav)|*.wav;|Все файлы |*.*";
            if (dlg.ShowDialog() == true)
            {
                if(dlg.FileName != "")
                {
                    FilePath = dlg.FileName;
                    songName = FilePath;
                }
            }
            lblSong.Content = $"Песня: {songName}";
            if (FilePath != "" | FilePath != null)
            {
                btnTest.IsEnabled = true;
            }
            else
            {
                btnTest.IsEnabled = false;
            }
        }
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FilePath.EndsWith(".wav"))
                {
                    SoundPlayer player = new SoundPlayer(FilePath);
                    player.Play();
                }
                else if (FilePath.EndsWith(".mp3"))
                {
                    using (var waveOut = new WaveOutEvent())
                    {
                        // для чтения файла MP3
                        using (var mp3Reader = new Mp3FileReader(FilePath))
                        {
                            waveOut.Init(mp3Reader);
                            waveOut.Play();
                        }
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Ошибка!\n{ex}");
            }

            //using (var reader = new Mp3FileReader(FilePath))
            //using (var waveOut = new WaveOutEvent())
            //{
            //    waveOut.Init(reader);
            //    waveOut.PlaybackStopped += (eventSender, args) =>
            //    {
            //        waveOut.Play();
            //    };
            //    waveOut.Play();

            //    while (waveOut.PlaybackState == PlaybackState.Playing)
            //    {
            //        waveOut.Play();
            //    }
            //}

        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FilePath.EndsWith(".wav"))
                {
                    SoundPlayer player = new SoundPlayer(FilePath);
                    player.PlayLooping();
                } else if (FilePath.EndsWith(".mp3"))
                {
                    using (var waveOut = new WaveOutEvent())
                    {
                        // для чтения файла MP3
                        using (var mp3Reader = new Mp3FileReader(FilePath))
                        {
                            LoopStream loop = new LoopStream(mp3Reader);
                            waveOut.Init(loop);
                            waveOut.Play();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка!\n{ex}");
            }
            this.Close();
        }

    }
}
