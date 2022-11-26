using System;
using System.Net.Sockets;
using Xamarin.Forms;

namespace ImagePicker
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            masterPage2.listView.ItemSelected += OnItemSelected;



            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }



        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;
            // Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            Detail.Navigation.PushAsync((Page)Activator.CreateInstance(page));
            IsPresented = false;
            ((ListView)sender).SelectedItem = null;


        }
    }
}
