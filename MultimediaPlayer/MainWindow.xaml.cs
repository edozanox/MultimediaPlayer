using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace MultimediaPlayer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        Random posTrackLst = new Random();
    
        public List<string> ListaNomiFile { get; } = new List<string>();
        public List<string> ListaFileAudio { get; } = new List<string>();
        public List<string> ListaFileVideo { get; } = new List<string>();
        string elem_path;
        string fileName;
        string nome_file;        
        bool PauseIsClicked = false;
        bool AudioIsActive = false;
        bool IsDragging = false;
        int totTracks = 0;
        
        public MainWindow()
        {
            InitializeComponent();        
            //dt = new DispatcherTimer(TimeSpan.FromMilliseconds(150), DispatcherPriority.Render, TimeBuffering, Dispatcher);            
            //CaricamentoListaBrani();
            //lstBrani.ItemsSource = ListaFileMultimediali;
            
        }

        public void BtnFileSelector_Click(object sender, RoutedEventArgs e)
        {

            lstBrani.Items.Clear();
            System.Windows.MessageBox.Show("È POSSIBILE CARICARE SOLO FILE CON ESTENSIONE .mp3 .mp4", "Media supportati", MessageBoxButton.OK, MessageBoxImage.Information);
            try
            {

                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    DialogResult rslt = dialog.ShowDialog();
                    if (rslt == System.Windows.Forms.DialogResult.OK)
                    {

                        elem_path = dialog.SelectedPath;                        
                        string[] array_elem = Directory.GetFiles(elem_path, "*.mp3");
                        string[] array_videos = Directory.GetFiles(elem_path, "*.mp4");

                        foreach (var track in array_elem)
                        {
                            fileName = System.IO.Path.GetFileNameWithoutExtension(track);
                            ListaFileAudio.Add(track);
                            lstBrani.Items.Add(fileName);
                            btnShuffle.IsEnabled = true;
                            btnShuffle.Background = Brushes.Black;

                        }

                        foreach(var video in array_videos)
                        {
                            fileName = System.IO.Path.GetFileNameWithoutExtension(video);
                            ListaFileVideo.Add(video);
                            lstBrani.Items.Add(fileName);
                            btnShuffle.IsEnabled = false;
                            btnShuffle.Background = Brushes.Gray;
                        }
                  

                    }

                }

                totTracks = lstBrani.Items.Count;
                lblCounter.Content = totTracks;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }

        
        public void CaricamentoListaBrani()
        {
            string[] multimedia_elements = Directory.GetFiles(@"H:\Zanotti_ProgrammazioneFITSTIC\FITSTIC_C#\MultimediaPlayer\MultimediaPlayer\Music", "*.mp3");

            for (int i = 0; i < multimedia_elements.Length; i++)
            {
                string file_name = new FileInfo(multimedia_elements[i]).Name;
                ListaNomiFile.Add(file_name);
                lstBrani.Items.Add(file_name);
            }


            foreach (var elem in multimedia_elements)
            {
                ListaFileAudio.Add(elem);
            }


        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {

            //meSupportPlayer.Source = new Uri("Videos\\FlyingMusicalNotes.mov", UriKind.Relative);           
            try
            {

                if (PauseIsClicked == false)
                {
                    
                    nome_file = lstBrani.SelectedItem.ToString();
                    //meAudioPlayer.Source = new Uri("../../Music/" + titolo_brano, UriKind.Relative); 
                    //Se in lista Brani o lista Video esiste un path con  nome_file indicato ed estensione giusta, allora imposta Source del player corrispondente
                    if (ListaFileAudio.Contains(elem_path + "\\" + nome_file + ".mp3"))
                    {
                        
                        meMultimediaPlayer.Stop();
                        meMultimediaPlayer.Visibility = Visibility.Collapsed;
                        meSupportPlayer.Visibility = Visibility.Visible;
                        meAudioPlayer.Source = new Uri(elem_path + "\\" + nome_file + ".mp3", UriKind.Absolute);                        
                        meAudioPlayer.Play();                        
                        AudioIsActive = true;
                        
                    }
                    else if (ListaFileVideo.Contains(elem_path + "\\" + nome_file + ".mp4"))
                    {
                        meMultimediaPlayer.Source = new Uri(elem_path + "\\" + nome_file + ".mp4", UriKind.Absolute);
                        meAudioPlayer.Stop();
                        meMultimediaPlayer.Play();
                        meMultimediaPlayer.Visibility = Visibility.Visible;
                        meSupportPlayer.Visibility = Visibility.Hidden;
                        AudioIsActive = false;
                        
                    }
                    else
                        System.Windows.MessageBox.Show("FILE NOT FOUND", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    PauseIsClicked = false;
                    

                }


                lblTitoloBrano.Content = nome_file;
                Player.Title = nome_file;
                lblInfoPlayer.Content = "PLAY";
                //meAudioPlayer.Play();
                if (!AudioIsActive)
                {
                    meMultimediaPlayer.Play();
                }
                else
                {
                    meAudioPlayer.Play();
                }
                
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        
        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PauseIsClicked = true;
                meAudioPlayer.Pause();
                meMultimediaPlayer.Pause();
                Console.Beep(500, 300);
                Console.Beep(400, 300);
                lblInfoPlayer.Content = "PAUSE";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }       

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                meAudioPlayer.Stop();
                meMultimediaPlayer.Stop();
                meSupportPlayer.Visibility = Visibility.Visible;
                timer.Stop();
                lblInfoPlayer.Content = "STOP";
                lblTitoloBrano.Content = "LIVE - Rainbow Multimedia Player";
                lblTime.Content = "#LIVE";

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                posTrackLst = new Random();
                int rnd_pos = posTrackLst.Next(0, totTracks);
                nome_file = ListaFileAudio[rnd_pos];
                meAudioPlayer.Source = new Uri(nome_file, UriKind.Absolute);
                meAudioPlayer.Play();
                string nome_brano = System.IO.Path.GetFileNameWithoutExtension(nome_file);
                lblInfoPlayer.Content = "PLAY";
                lblTitoloBrano.Content = nome_brano;
                this.Title = nome_brano;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
       

        void timer_Tick(object sender, EventArgs e)
        {
            if(!IsDragging)
            {
                slTime.Value = meAudioPlayer.Position.TotalSeconds;
            }

            long curMediaTicks = 0;
            long totMediaTicks = 0;

            
                 curMediaTicks = meAudioPlayer.Position.Ticks;
                 totMediaTicks = meAudioPlayer.NaturalDuration.TimeSpan.Ticks;

                if (totMediaTicks > 0)
                {
                    slTime.Value = (double)curMediaTicks / totMediaTicks;
                    lblTime.Content = TimeSpan.FromTicks(curMediaTicks).ToString(@"mm\:ss");
                }
                else
                    slTime.Value = 0;
            /*
            else
            {
                 curMediaTicks = meMultimediaPlayer.Position.Ticks;
                 totMediaTicks = meMultimediaPlayer.NaturalDuration.TimeSpan.Ticks;

                if (totMediaTicks > 0)
                {
                    slTime.Value = (double)curMediaTicks / totMediaTicks;
                    lblTime.Content = TimeSpan.FromTicks(curMediaTicks).ToString(@"mm\:ss");
                }
                else
                    slTime.Value = 0;
            }
            */
        }

        private void meAudioPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            //slTime.Maximum = meAudioPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        private void meMultimediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        /*
        private void slTime_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (meAudioPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
            {
                meAudioPlayer.Position = TimeSpan.FromSeconds(slTime.Value * meAudioPlayer.NaturalDuration.TimeSpan.TotalSeconds);
            } 
        }
        */

        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int SliderValue = (int)slTime.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            meAudioPlayer.Pause();
            meAudioPlayer.Position = ts;
            meAudioPlayer.Play();
        }
        private void meAudioPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            ///VERSIONE MADDALENA///
            lstBrani.SelectedIndex = lstBrani.SelectedIndex + 1;
            if (lstBrani.SelectedItem != null)
            {
                BtnPlay_Click(sender, e);
            }
            else
            {
                System.Windows.MessageBox.Show("LA PLAYLIST È ARRIVATA AL TERMINE", "InfoPlayer", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            ///VERSIONE PROF. VENTURI///
            /*
            int totTracks = lstBrani.Items.Count;
            int posEndedTrack = lstBrani.SelectedIndex;

            if(totTracks > posEndedTrack +1 )
            {

            }
            */
        }

        private void slTime_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            IsDragging = false;
            meAudioPlayer.Position = TimeSpan.FromSeconds(slTime.Value);
        }

        private void slTime_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            IsDragging = true;
        }
    }
}
