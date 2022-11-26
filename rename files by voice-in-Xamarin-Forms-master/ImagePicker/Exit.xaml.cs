using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagePicker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Exit : ContentPage
    {
        public Exit()
        {
            InitializeComponent();
            exit2();


        }


        public async void exit2()
        {

            var answer = await DisplayAlert("Exit", "Do you want to exit Application? ", "Yes", "No");
            if (answer)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

            }
            else
            {


            }



            }
        }
}