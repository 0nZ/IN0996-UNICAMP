﻿using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;

namespace IN0996_UNICAMP
{
	public partial class MediaPlayer : Window
	{
        private bool dragging = false;
        private bool playing = false;

		public MediaPlayer()
        {
            InitializeComponent();

            // Cria e configura um DispatcherTimer para atualizar o progresso do áudio em intervalos regulares
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Define o intervalo para 1 segundo
            timer.Tick += timer_Tick; // Associa o evento timer_Tick ao evento Tick do DispatcherTimer
            timer.Start(); // Inicia o DispatcherTimer para começar a atualizar o progresso do áudio
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            // Verifica se a origem do Player não é nula, a duração natural possui um TimeSpan e não há arraste do slide de progresso
            if ((Player.Source != null) && (Player.NaturalDuration.HasTimeSpan) && (!dragging))
            {
                slideProgress.Minimum = 0; // Define o valor mínimo do slide de progresso como 0
                slideProgress.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds; // Define o valor máximo do slide de progresso como a duração total da mídia em segundos
                slideProgress.Value = Player.Position.TotalSeconds; // Define o valor atual do slide de progresso como a posição atual de reprodução da mídia em segundos
            }
        }

		private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = true;
		}

		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            OpenFileDialog FileDialog = new OpenFileDialog();           // Cria uma instância do OpenFileDialog
            // Define o filtro de arquivo para exibir apenas arquivos de mídia com as extensões .mp3, .mpg e .mpeg
			FileDialog.Filter = "Media files (*.mp3;*.mp4;*.mpg;*.mpeg)|*.mp3;*.mp4;*.mpg;*.mpeg|All files (*.*)|*.*";      
			if(FileDialog.ShowDialog() == true)         
				Player.Source = new Uri(FileDialog.FileName);      // Define a origem do Player como um objeto Uri que representa o caminho completo do arquivo selecionado
		}

		private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
           /* bool possible= false;
            if((Player != null) && (Player.Source != null))         //Se o player existe e a fonte também, é possível tocar
            {
                possible = true;
            }*/
            e.CanExecute = (Player != null) && (Player.Source != null);;
		}

		private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            Player.Play();          //Faz o player tocar
			playing = true;         //Define o parâmetro como verdadeiro
		}

		private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = playing;         //Se está tocando, é possível pausar
		}

		private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            Player.Pause();     //Pausa o player
		}

		//Quando a lógica tiver feita, adicionar no .xaml os comandos e os botões para essas funcionalidades
		/*private void Next_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            //Verificar se há uma próxima música
		}

		private void Next_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            //Método que dá play na próxima música
		}

        private void Previous_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            //Verificar se há uma música anterior
		}

        private void Previous_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            //Método que dá play na música anterior
		}

        private void Repeat_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = playing;     //É possível deixar no repeat se algo está tocando
		}

        private void Repeat_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            //Método que deixa uma mídia tocando em loop
		}*/

        private void slideProgress_DragStarted(object sender, DragStartedEventArgs e)
		{
			dragging = true;    //Se o usuário está arrastando, a variável de controle dragging é definida como true
		}

		private void slideProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            dragging = false; // Define a variável de controle "dragging" como false, indicando que o slide de progresso não está mais sendo arrastado pelo usuário
            Player.Position = TimeSpan.FromSeconds(slideProgress.Value); // Define a posição de reprodução do áudio como o valor atual do slide de progresso convertido em um objeto TimeSpan
        }


		private void slideProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
            // Atualiza o texto do controle ProgressStatus com base no valor atual do slide de progresso
			ProgressStatus.Text = TimeSpan.FromSeconds(slideProgress.Value).ToString(@"hh\:mm\:ss");
		}

		private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
		{
            // Verifica a direção do movimento da roda do mouse
            // Se o valor de e.Delta for maior que 0, significa que a roda do mouse foi rolada para cima, caso contrário, foi rolada para baixo
            // Com base na direção, aumenta ou diminui o volume do player
			Player.Volume += (e.Delta > 0) ? 0.1 : -0.1;
		}
	}
}