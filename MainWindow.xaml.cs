using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Windows.Controls;

namespace IN0996_UNICAMP
{
	public partial class MediaPlayer : Window
	{
        private bool dragging = false;
        private bool playing = false;
		private List<string> playlistPaths;
		private string[] mediaFiles = {};
		private int currentMusicIndex;

		public MediaPlayer()
        {
            InitializeComponent();
			playlistPaths = new List<string>();

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

		/*private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = true;
		}

		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog();           // Cria uma instância do OpenFileDialog
            // Define o filtro de arquivo para exibir apenas arquivos de mídia com as extensões .mp3, .mpg e .mpeg
			FileDialog.Filter = "Media files (*.mp3;*.mp4;*.mpg;*.mpeg)|*.mp3;*.mp4;*.mpg;*.mpeg|All files (*.*)|*.*";      
			if(FileDialog.ShowDialog() == true)         
				Player.Source = new Uri(FileDialog.FileName);      // Define a origem do Player como um objeto Uri que representa o caminho completo do arquivo selecionado
		}*/

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

        /*private void Repeat_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
            e.CanExecute = playing;     //É possível deixar no repeat se algo está tocando
		}

        private void Repeat_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            //Método que deixa uma mídia tocando em loop
		}*/

		private void Next_Media(object sender, RoutedEventArgs e)
		{
			if (currentMusicIndex < mediaFiles.Length - 1)
			{
				// Avançar para a próxima mídia
				currentMusicIndex++;
				Player.Source = new Uri(mediaFiles[currentMusicIndex]);

				// Iniciar a reprodução
				Player.Play();
			}
		}

		private void Previous_Media(object sender, RoutedEventArgs e)
		{
			if (currentMusicIndex - 1 >= 0)
			{
				// Voltar para a mídia anterior
				currentMusicIndex--;
				Player.Source = new Uri(mediaFiles[currentMusicIndex]);

				// Iniciar a reprodução
				Player.Play();
			}
		}

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

		private void AddPlaylistMenuItem_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                string playlistPath = dialog.SelectedPath;
                playlistPaths.Add(playlistPath);

                // Atualizar a lista de playlists
                RefreshPlaylistListBox();
            }
        }

		private void PlaylistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlaylistListBox.SelectedIndex != -1)
            {
                // Obter o caminho da playlist selecionada
                string playlistPath = playlistPaths[PlaylistListBox.SelectedIndex];
                // Reproduzir a playlist no MediaElement
                PlayPlaylist(playlistPath);
            }
        }

		private void RefreshPlaylistListBox()
        {
            // Limpar a lista de playlists
            PlaylistListBox.Items.Clear();

            // Adicionar as playlists à lista
            foreach (string playlistPath in playlistPaths)
            {
                string playlistName = Path.GetFileName(playlistPath);
                PlaylistListBox.Items.Add(new PlaylistItem { Name = playlistName, Path = playlistPath });
            }
        }

		private void PlaylistBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			// Verificar se o item da playlist foi selecionado
			if (e.ChangedButton == MouseButton.Left && PlaylistListBox.SelectedItem is PlaylistItem playlistItem)
			{
				// Reproduzir a playlist
				PlayPlaylist(playlistItem.Path);
			}
		}

		private void PlaylistBorder_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			// Verificar se o evento foi acionado pelo botão direito do mouse
			if (e.ChangedButton == MouseButton.Right)
			{
				// Obter o Border que acionou o evento
				Border border = (Border)sender;

				// Exibir o ContextMenu na posição do mouse
				ContextMenu contextMenu = border.FindResource("ContextMenu") as ContextMenu;
				contextMenu.PlacementTarget = border;
				contextMenu.IsOpen = true;
			}
		}


		
		private void SelectImageMenuItem_Click(object sender, RoutedEventArgs e)
		{
			// Abrir um diálogo para selecionar uma imagem
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			openFileDialog.Filter = "Imagens|*.png;*.jpg;*.jpeg";
			if (openFileDialog.ShowDialog() == true)
			{
				// Obter o caminho da imagem selecionada
				string imagePath = openFileDialog.FileName;

				// Lógica para associar a imagem à playlist selecionada
				if (PlaylistListBox.SelectedItem is PlaylistItem playlistItem)
				{
					playlistItem.ImagePath = imagePath;
				}
			}
		}

		private void DeletePlaylistMenuItem_Click(object sender, RoutedEventArgs e)
		{
			// Confirmar a exclusão da playlist
			MessageBoxResult result = System.Windows.MessageBox.Show("Tem certeza de que deseja excluir esta playlist?", "Excluir Playlist", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes)
			{
				// Lógica para excluir a playlist
				if (PlaylistListBox.SelectedItem is PlaylistItem playlistItem)
				{
					// Remover o item da lista
					playlistPaths.Remove(playlistItem.Path);
					RefreshPlaylistListBox();
				}
			}
		}

		private void PlayPlaylist(string playlistPath)
		{
			// Obter os arquivos de música da playlist, só está selecionando .mp4
			mediaFiles = Directory.GetFiles(playlistPath, "*.mp4", SearchOption.AllDirectories);

			// Definir a primeira música da playlist como a fonte do MediaElement
			if (mediaFiles.Length > 0)
			{
				currentMusicIndex = 0;
				Player.Source = new Uri(mediaFiles[currentMusicIndex]);

				// Iniciar a reprodução
				Player.Play();

				// Registrar o evento MediaEnded para reproduzir a próxima música
				Player.MediaEnded += Player_MediaEnded;
			}
		}

		private void Player_MediaEnded(object sender, RoutedEventArgs e)
		{
			// Verificar se há mais músicas na playlist
			if (currentMusicIndex < mediaFiles.Length - 1)
			{
				// Avançar para a próxima música
				currentMusicIndex++;
				Player.Source = new Uri(mediaFiles[currentMusicIndex]);

				// Iniciar a reprodução
				Player.Play();
			}
			else
			{
				// A reprodução da playlist foi concluída, é possível realizar alguma ação adicional, como uma mensagem de que a PL acabou
			}
		}

		public class PlaylistItem
    	{
        	public string? Name { get; set; }
        	public string? Path { get; set; }
            public string? ImagePath { get; internal set; }
        }

	}
}