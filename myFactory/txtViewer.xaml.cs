using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace myFactory {
    public partial class txtViewer : PhoneApplicationPage {

        private string path = "";

        public txtViewer() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            string temp = string.Empty;
            if(NavigationContext.QueryString.TryGetValue("path", out temp)) {
                this.path = temp;
            }
            this._title.Text = Path.GetFileName(path).Remove(Path.GetFileName(path).Length - 4);
            try {
                using (StreamReader sr = new StreamReader(path)) {
                    this._textBlock.Text = sr.ReadToEnd();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }


    }
}