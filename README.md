# **Media Player usando C# e WPF**

## **Autores:**
* Henrique Alves de Fernando - 236538
* Willian Vaz - 182845

## **Descrição:**

Este projeto foi desenvolvido pelos alunos Henrique Alves de Fernando e Willian Vaz como parte da disciplina de extensão INF0996 - Desenvolvimento de Interface de Usuário, ministrada na Universidade Estadual de Campinas (Unicamp).

## **Instruções:**

* Certifique-se de que você possui o Visual Studio Code instalado em seu sistema, bem como o SDK do .NET Framework.
* Crie um novo projeto WPF no Visual Studio Code e adicione os arquivos de código fornecidos, incluindo as bibliotecas necessárias.
* Abra o arquivo MainWindow.xaml e verifique se a interface de usuário foi criada corretamente, incluindo a janela principal com as colunas e os elementos da interface, como MediaElement, botões, barras de progresso e controles de volume.
* Certifique-se de que as referências para as bibliotecas usadas, como System, System.Collections.Generic, System.IO, System.Windows, System.Windows.Input, System.Windows.Threading, System.Windows.Forms, Microsoft.Win32 e Ookii.Dialogs.Wpf, estão corretamente adicionadas ao arquivo do projeto.
* Compile o projeto para garantir que não haja erros de compilação. Caso haja algum erro, verifique se as bibliotecas estão corretamente referenciadas e se todos os arquivos de código estão devidamente adicionados ao projeto.
* Após a compilação bem-sucedida, você pode executar o projeto clicando no botão "Start" no Visual Studio Code ou digitando `dotnet run` em um terminal na IDE. Isso abrirá a janela principal do Media Player.
* Na interface do Media Player, você pode adicionar arquivos de mídia individuais ou playlists a partir de uma pasta local selecionando a opção correspondente nos Menus. Isso permitirá que você adicione músicas e vídeos à fila de reprodução.
* Use os botões de controle (play, pause, previous e next) para controlar a reprodução das mídias na fila de reprodução. Você também pode usar a barra de progresso para selecionar um ponto específico da reprodução e o controle de volume através do scroll do mouse.

## **Detalhes da Arquitetura:**

A interface gráfica foi construída no arquivo MainWindow.xaml. Foi criada uma janela que foi dividida em duas colunas. 

Na primeira coluna da interface, há um MediaElement, que é o reprodutor de mídia central da aplicação. Nessa coluna, também estão localizados os botões de controle, como play, pause, previous e next, permitindo ao usuário controlar a reprodução da mídia. Além disso, há uma barra de progresso que possibilita ao usuário selecionar o ponto específico da reprodução desejado, enquanto o controle de volume, implementado com o scroll do mouse, oferece uma maneira intuitiva de ajustar o áudio, sendo que o scroll “para cima” diminui o volume e “para baixo” aumenta.

Na segunda coluna, foram adicionados dois Menus, um para mídias e outro para playlists. Esses menus permitem ao usuário adicionar arquivos de mídia individuais ou playlists a partir de uma pasta local. A escolha do uso de MenuItems possibilita futuras expansões, permitindo a inclusão de novas funcionalidades relacionadas às mídias ou playlists no futuro.

O code behind da aplicação pode ser encontrado no arquivo MainWindow.xaml.cs e ele contém a implementação de todos os eventos relevantes ao projeto, como clique em botões, mudança de barras de progresso, controle do tempo de reprodução e adição de itens de mídia ao reprodutor.

## **Funcionamento do Sistema:**

Ao iniciar a aplicação, é possível adicionar uma mídia ao clicar no botão *Mídia* e *Adicionar mídia*. Feito isso, pode-se selecioanr um arquivo *.mp4* local. De forma semelhante, pode-se adicionar uma playlist clicando no botão *Playlist*, *Adicionar Playlist* e selecionar um folder local.

Para reproduzir uma mídia ou playlist, basta clicar nela com o botão esquerdo do mouse. Nesse caso, a fila de reprodução é zerada e o arquivo em questão é reproduzido. Para dar pause em uma mídia, basta clicar com o botão esquerdo do mouse no botão *pause*, logo abaixo do reprodutor. Para dar play, basta clicar com o botão esquerdo no botão de *play*. Para usar os botões *previous* e *next*, é necessário ter alguma mídia anterior ou posterior, respectivamente a ser reproduzida e clicar nos botões correspondentes.

É possível ver o tempo após o início da reprodução no canto inferior esquerdo e, para controlar o momento de reprodução, basta arrastar e soltar a barra de progresso que está abaixo do reprodutor. Para aumentar o volume, deve-se mover o scroll do mouse "para baixo" dentro da primeira coluna da janela. Abaixar o volume é feito de forma similar, mas com o arraste do scroll "para cima".

Após adicionar mídias ou playlists no sistema, é possível excluí-los ou adicioná-los na fila de reprodução. Isso pode ser feito apertando o botão direito do mouse sobre a imagem da mídia correspondente, na lista à direita. A opção de adicionar uma imagem à mídia ainda necessita de manutenção.