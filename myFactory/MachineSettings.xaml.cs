using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;

using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Windows.Storage;
using Microsoft.Phone.Tasks;

namespace myFactory {
    public partial class MachineSettings : PhoneApplicationPage {

        #region Escopo
        private int selectedMode=-1;

        private ImageBrush ecoModeUnactive = new ImageBrush();
        private ImageBrush ecoModeActive = new ImageBrush();
        private ImageBrush medModeUnactive = new ImageBrush();
        private ImageBrush medModeActive = new ImageBrush();
        private ImageBrush fastModeUnactive = new ImageBrush();
        private ImageBrush fastModeActive = new ImageBrush();
        private ImageBrush txtImage = new ImageBrush();
        private ImageBrush pdfImage = new ImageBrush();
        #endregion

        #region Construtor
        public MachineSettings() {
            InitializeComponent();

            // --> Inicializa as ImageBrush, que são as imagens dos botões
            this.ecoModeUnactive.ImageSource = new BitmapImage(new Uri(@"/Assets/Buttons/buttonEcoModeUnactivated.png",UriKind.RelativeOrAbsolute));
            this.ecoModeActive.ImageSource = new BitmapImage(new Uri(@"/Assets/Buttons/buttonEcoModeActivated.png", UriKind.RelativeOrAbsolute));
            this.medModeUnactive.ImageSource = new BitmapImage(new Uri(@"/Assets/Buttons/buttonMedModeUnactive.png", UriKind.RelativeOrAbsolute));
            this.medModeActive.ImageSource = new BitmapImage(new Uri(@"/Assets/Buttons/buttonMedModeActive.png", UriKind.RelativeOrAbsolute));
            this.fastModeUnactive.ImageSource = new BitmapImage(new Uri(@"/Assets/Buttons/buttonFastModeUnactive.png", UriKind.RelativeOrAbsolute));
            this.fastModeActive.ImageSource = new BitmapImage(new Uri(@"/Assets/Buttons/buttonFastModeActive.png", UriKind.RelativeOrAbsolute));

            this.txtImage.ImageSource = new BitmapImage(new Uri(@"/Assets/Buttons/buttonTXTicon.png", UriKind.RelativeOrAbsolute));
            this.pdfImage.ImageSource = new BitmapImage(new Uri(@"/Assets/Buttons/buttonPDFicon.png", UriKind.RelativeOrAbsolute));
            
            // --> Isto evita que ao ser clicado o botão seja iluminado
            this._ecoMode.ClickMode = ClickMode.Press;
            this._medMode.ClickMode = ClickMode.Press;
            this._fastMode.ClickMode = ClickMode.Press;

            Loaded += MachineSettings_Loaded;

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"/Tutorial/Teste/");
            int i = 0;
            foreach (string file in files) {
                if (file.Substring(file.Length - 3) == "txt") {
                    RowDefinition rd = new RowDefinition() { Height = new GridLength(80) };
                    this._tutorialGrid.RowDefinitions.Add(rd);

                    TextBlock txt = new TextBlock();
                    txt.Text = Path.GetFileName(file).Remove(Path.GetFileName(file).Length - 4);
                    txt.SetValue(Grid.RowProperty, i);
                    txt.SetValue(Grid.ColumnProperty, 0);
                    txt.VerticalAlignment = VerticalAlignment.Center;


                    Button temp = new Button();
                    temp.HorizontalAlignment = HorizontalAlignment.Left;
                    temp.VerticalAlignment = VerticalAlignment.Top;
                    temp.Background = txtImage;
                    temp.ClickMode = ClickMode.Press;
                    temp.Name = file;
                    temp.Width = 80;
                    temp.Height = 80;
                    temp.Click += _txtButton_Click;
                    temp.SetValue(Grid.RowProperty, i);
                    temp.SetValue(Grid.ColumnProperty, 1);
                    temp.BorderThickness = new Thickness(0);

                    this._tutorialGrid.Children.Add(txt);
                    this._tutorialGrid.Children.Add(temp);
                    i++;
                }
                if (file.Substring(file.Length - 3) == "pdf") {
                    RowDefinition rd = new RowDefinition() { Height = new GridLength(80) };
                    this._tutorialGrid.RowDefinitions.Add(rd);

                    TextBlock txt = new TextBlock();
                    txt.Text = Path.GetFileName(file).Remove(Path.GetFileName(file).Length - 4);
                    txt.SetValue(Grid.RowProperty, i);
                    txt.SetValue(Grid.ColumnProperty, 0);
                    txt.VerticalAlignment = VerticalAlignment.Center;


                    Button temp = new Button();
                    temp.HorizontalAlignment = HorizontalAlignment.Left;
                    temp.VerticalAlignment = VerticalAlignment.Top;
                    temp.Background = pdfImage;
                    temp.ClickMode = ClickMode.Press;
                    temp.Name = file;
                    temp.Width = 80;
                    temp.Height = 80;
                    temp.Click += _pdfButton_Click;
                    temp.SetValue(Grid.RowProperty, i);
                    temp.SetValue(Grid.ColumnProperty, 1);
                    temp.BorderThickness = new Thickness(0);

                    this._tutorialGrid.Children.Add(txt);
                    this._tutorialGrid.Children.Add(temp);
                    i++;
                }
            }

            if (this._tutorialGrid.RowDefinitions[0] != null) {
                this._tutorialGrid.RowDefinitions[0].Height = new GridLength(80);
            }

            this._tutorialSV.Content = _tutorialGrid;
        }

        private void MachineSettings_Loaded(object sender, RoutedEventArgs e) {
            
        }
        #endregion

        // --> Este método é chamado toda vez que um botão é clicado,
        //     ele verifica o modo ativo e seleciona o que deve ser ativado
        private void checkActiveMode() {
            
            this._txt_ActiveOption.Text = "Nenhum modo selecionado";

            if (this._fastMode.Background != fastModeUnactive)
                this._fastMode.Background = fastModeUnactive;

            if (this._medMode.Background != medModeUnactive)
                this._medMode.Background = medModeUnactive;

            if (this._ecoMode.Background != ecoModeUnactive)
                this._ecoMode.Background = ecoModeUnactive;

            if (selectedMode == 0) {
                this._ecoMode.Background = ecoModeActive;
                this._txt_ActiveOption.Text = "Econômico";
            } else if (selectedMode == 1) {
                this._medMode.Background = medModeActive;
                this._txt_ActiveOption.Text = "Balanceado";
            } else if (selectedMode == 2) {
                this._fastMode.Background = fastModeActive;
                this._txt_ActiveOption.Text = "Rápido";
            } 
        }

        #region Buttons Click
        private void _ecoMode_Click(object sender, RoutedEventArgs e) {
            if (this.selectedMode != 0) {
                this.selectedMode = 0;
                checkActiveMode();
            } else {
                MessageBox.Show("Modo de Operação já selecionado!");
            }
        }

        private void _medMode_Click(object sender, RoutedEventArgs e) {
            if (this.selectedMode != 1) {
                this.selectedMode = 1;
                checkActiveMode();
            } else {
                MessageBox.Show("Modo de Operação já selecionado!");
            }
        }

        private void _fastMode_Click(object sender, RoutedEventArgs e) {
            if (this.selectedMode != 2) {
                this.selectedMode = 2;
                checkActiveMode();
            } else {
                MessageBox.Show("Modo de Operação já selecionado!");
            }
        }

        private void _txtButton_Click(object sender, RoutedEventArgs e) {
            Button btn = (Button)sender;
            NavigationService.Navigate(new Uri(@"/txtViewer.xaml?path="+ btn.Name, UriKind.RelativeOrAbsolute));
        }

        private async void _pdfButton_Click(object sender, RoutedEventArgs e) {
            Button btn = (Button)sender;
            //var sucess = await Windows.System.Launcher.LaunchUriAsync(new Uri(btn.Name, UriKind.Relative));
        }
        #endregion

    }
}