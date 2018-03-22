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

using Microsoft.Win32;

using NAudio.Wave;

namespace Efectos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        WaveOutEvent waveOut;
        AudioFileReader reader;
        Efecto efectoProvider;
        Delay delayProvider;

        public MainWindow()
        {
            InitializeComponent();
            waveOut = new WaveOutEvent();
        }

        private void btnExaminar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if ( (bool) fileDialog.ShowDialog())
            {
                txtRuta.Text =
                    fileDialog.FileName;
                reader = new AudioFileReader(fileDialog.FileName);
            }
        }

        private void btnReproducir_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut !=  null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                waveOut.Init(reader);
               
                waveOut.Play();
            }
            
        }

        private void btnEfecto_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null && reader != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                efectoProvider = new Efecto(reader, (float)sldFactor.Value);
                waveOut.Init(efectoProvider);
                waveOut.Play();
            }
        }

        private void btnVolumenDelay_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null && reader != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                delayProvider = new Delay(reader, (float)sldAtenuadorDelay.Value);
                waveOut.Init(delayProvider);
                waveOut.Play();
            }

        }

        private void btnDelay_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null && reader != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                delayProvider = new Delay(reader);
                waveOut.Init(delayProvider);
                waveOut.Play();
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (efectoProvider != null)
            {
                efectoProvider.Factor =
                    (float)sldFactor.Value;

            }
        }

        private void sldDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (delayProvider != null)
            {
                delayProvider.OffsetTiempoMS =
                    (int)sldDelay.Value;

            }
        }

        private void sldAtenuadorDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (delayProvider != null)
            {
                delayProvider.Atenuador =
                    (float)sldAtenuadorDelay.Value;

            }
        }

    }
}
