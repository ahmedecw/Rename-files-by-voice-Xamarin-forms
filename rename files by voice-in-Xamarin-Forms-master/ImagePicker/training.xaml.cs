using ImagePicker;
using System;
using Xamarin.Forms;
using XFSpeechDemo;

namespace ImagePicker
{
    public partial class training : ContentPage
    {
        private ISpeechToText2 _speechRecongnitionInstance2;
        public training()
        {
            InitializeComponent();



            try
            {
                _speechRecongnitionInstance2 = DependencyService.Get<ISpeechToText2>();
            }
            catch(Exception ex)
            {
                recon2.Text = ex.Message;
            }
            

            MessagingCenter.Subscribe<ISpeechToText2, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved2(args);
            });

            MessagingCenter.Subscribe<ISpeechToText2>(this, "Final", (sender) =>
            {
                start2.IsEnabled = true;
            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved2(args);
            });
        }

        private void SpeechToTextFinalResultRecieved2(string args)
        {
            recon2.Text = args;
        }
      
        private void Start2_Clicked(object sender, EventArgs e)
        {
            try
            {
                _speechRecongnitionInstance2.StartSpeechToText2();
            }
            catch(Exception ex)
            {
                recon2.Text = ex.Message;
            }
            
            if (Device.RuntimePlatform == Device.iOS)
            {
                start2.IsEnabled = false;
            }

            

        }

        
    }
}
