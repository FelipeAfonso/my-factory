using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZXing.Common;
using ZXing.QrCode;
using ZXing;
using ZXing.Mobile;
using System.Windows.Threading;
using Microsoft.Devices;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace myFactory {
    public partial class MainPage : PhoneApplicationPage {

        #region Escopo
        private readonly DispatcherTimer _timer;
        private PhotoCameraLuminanceSource _luminance;
        private QRCodeReader _reader;
        private PhotoCamera _photoCamera;
        #endregion

        #region Construtor
        public MainPage() {
            InitializeComponent();
            var phoneAccentBrush = new SolidColorBrush((App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush).Color);
            this._rectangle.Stroke = phoneAccentBrush;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(250);
            _timer.Tick += (o, arg) => ScanPreviewBuffer();
        }
        #endregion

        #region Métodos

        // --> Inicializa a camera toda vez que navegamos para a página
        protected override void OnNavigatedTo(NavigationEventArgs e) {

            _photoCamera = new PhotoCamera();
            _photoCamera.Initialized += OnPhotoCameraInitialized;
            _previewVideo.SetSource(_photoCamera);

            CameraButtons.ShutterKeyHalfPressed += (o, arg) => _photoCamera.Focus();

            base.OnNavigatedTo(e);
        }

        // --> Ao inicializar a camera ele ajusta ela ao quadrado
        private void OnPhotoCameraInitialized(object sender, CameraOperationCompletedEventArgs e) {
            int width = Convert.ToInt32(_photoCamera.PreviewResolution.Width);
            int height = Convert.ToInt32(_photoCamera.PreviewResolution.Height);

            _luminance = new PhotoCameraLuminanceSource(width, height);
            _reader = new QRCodeReader();

            Dispatcher.BeginInvoke(() => {
                _previewTransform.Rotation = _photoCamera.Orientation;
                _timer.Start();
            });
        }

        // --> Verifica a imagem recebida pela camera para tentar decodificar algum código QR presente nela
        private void ScanPreviewBuffer() {
            try {
                _photoCamera.GetPreviewBufferY(_luminance.PreviewBufferY);
                var binarizer = new HybridBinarizer(_luminance);
                var binBitmap = new BinaryBitmap(binarizer);
                var result = _reader.decode(binBitmap);
                if (qrTaskAssign(result.Text)) {
                    this._photoCamera.Dispose();
                    NavigationService.Navigate(new Uri("/MachineSettings.xaml", UriKind.Relative));
                } else {
                    MessageBox.Show(result.Text + " não é um equipamento registrado válido");
                }
            } catch {
            }
        }

        // --> Este método atribuirá o local para onde será levado dependendo do QR-Code
        private bool qrTaskAssign(string entry) {
            if (entry.Substring(0, 19) == "myFactory::Machine-") {
                return true;
            } else { return false; }

        }
        #endregion

    }
}