using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rika_metronome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Image[] images = new Image[8];
        private int imgCount = 3;
        private int metoronomeCount = 0;
        int tempo = 120;
        double frame;
        long startTick;
        bool isStart = false;
        System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        const int MAX_TEMPO = 300;
        public MainWindow()
        {
            InitializeComponent();
            images[0] = img1;
            images[1] = img2;
            images[2] = img3;
            images[3] = img4;
            images[4] = img5;
            images[5] = img6;
            images[6] = img7;
            images[7] = img8;
            imgCount = 3;
            setImage();
            startTick = sw.ElapsedMilliseconds;
            slider.Maximum = MAX_TEMPO;
        }

        private async void ClickStartButton(object sender, RoutedEventArgs e)
        {
            isStart = !isStart;
            startTick = sw.ElapsedMilliseconds;
            metoronomeCount = 0;
            if (isStart) loop();
        }
        private async void loop()
        {
            while(true)
            {
                frame = 60000*2/tempo/14;
                await tick();
                if (!isStart && getIdx(imgCount) == 3) break;
            }
        }
        private async Task tick()
        {
            long goalTick = startTick + (long)(frame * metoronomeCount);
            int delayTime = (int)(goalTick - sw.ElapsedMilliseconds);
            if (delayTime < 0) delayTime = 0;
            await Task.Delay(delayTime);
            setImage();
        }
        private void setImage()
        {
            imgCount++;
            metoronomeCount++;
            if (imgCount >= 14) imgCount = 0;
            int idx = getIdx(imgCount);
            for (int i = 0; i < 8; i++) images[i].Visibility = Visibility.Hidden;
            images[idx].Visibility = Visibility.Visible;
        }
        private int getIdx(int k)
        {
            if (k >= 8) k = 14 - k;
            return k;
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (slider != null) tempo = (int)slider.Value;
            if (tempoText != null) tempoText.Text = tempo.ToString();
            startTick = sw.ElapsedMilliseconds;
            metoronomeCount = 0;
        }


        private void tempoTextChanged(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (tempoText != null) int.TryParse(tempoText.Text, out tempo);
            if (tempo > MAX_TEMPO) tempo = MAX_TEMPO;
            else if (tempo < 10) tempo = 10;
            if (tempoText != null) tempoText.Text = tempo.ToString();

            if (slider != null) slider.Value = tempo;
            startTick = sw.ElapsedMilliseconds;
            metoronomeCount = 0;
        }

    }
}
