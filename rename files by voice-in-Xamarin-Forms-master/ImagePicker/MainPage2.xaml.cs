using ImagePicker.Model;
using ImagePicker.ViewModel;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFSpeechDemo;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace ImagePicker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]










    public partial class MainPage2 : ContentPage
    {


        private ISpeechToText _speechRecongnitionInstance;


        MainPageViewModel mainPageViewModel;
        Frame currentSelectedFrame; //store the selected image frame from collection view
        bool isSelected; // determines is selected of the image
        Frame selectedFrame; //stores the previous selected image frame of collection view
        MediaAssest selectedMediaAsset; //this holds the selected image details
        IMediaService mediaService;


        public MainPage2()
        {


            InitializeComponent();









            try
            {
                _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }


            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });

            MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
            {
                start.IsEnabled = true;
            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });






            mediaService = Xamarin.Forms.DependencyService.Get<IMediaService>();
            mainPageViewModel = new MainPageViewModel(mediaService);
            BindingContext = mainPageViewModel;
          






           // ImageSkipOrSelectImageClickEvent();// check preference already image selected if selected load the profile picture or else defult image
            imageselector.IsVisible = true; //make image picker stack layout visible
          //  bodyContent.Opacity = 0.3; //make behing content opacity so that image picker gets attention
            bodyContent.InputTransparent = true; // disable any user interaction on main content
            imageNext.IsVisible = false;
            //  imageSkip.IsVisible = true;
            imageselectorFrame.TranslateTo(0, imageselectorFrame.Y + 50, 300);
            mainPageViewModel.LoadMediaAssets(); //load the image from phone storage

        }


        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            imageselector.IsVisible = true; //make image picker stack layout visible
            bodyContent.Opacity = 0.3; //make behing content opacity so that image picker gets attention
            bodyContent.InputTransparent = true; // disable any user interaction on main content
            imageNext.IsVisible = false;
            //imageSkip.IsVisible = true;
            await imageselectorFrame.TranslateTo(0, imageselectorFrame.Y + 50, 300);
            await mainPageViewModel.LoadMediaAssets(); //load the image from phone storage
        }

      


        public void imageNextTapped(System.Object sender, System.EventArgs e)
        {
        }

            private void ImageSkipOrSelectImageClickEvent()
        {

            //common method to handle to display default image or seleceted image
            if (Preferences.ContainsKey("ProfileImage"))
            {
                string path = Preferences.Get("ProfileImage", null);
                if (path == "default")
                {
                    profilePicture.Source = "default5.png";
                }


                if (path == "")
                {
                    return;

                }


                else
                {


                    profilePicture.Source = path;
                    string filename = path.Substring(path.LastIndexOf("/") + 1);
                    
                    path_image.Text = filename;
                    path_image2.Text = path;


                }
            }


        }














     





      





        async void imageTapped(System.Object sender, System.EventArgs e)
        {
            
            var s = (StackLayout)sender;
            var ss = s.Children[0] as Grid;
            var sss = ss.Children[0] as Frame;
            selectedFrame = ss.Children[1] as Frame;
            var clicked = (TappedEventArgs)e;
            var mediaAssest = (MediaAssest)clicked.Parameter;
            selectedMediaAsset = mediaAssest;



            if (mediaAssest.PreviewPath == "group.png")
            {



                }




            else
            {


                   

                        if (currentSelectedFrame != null)
                {
                    if (selectedFrame != currentSelectedFrame)
                    {
                        selectedFrame.BackgroundColor = Color.Green;
                        currentSelectedFrame.BackgroundColor = Color.Transparent;
                        currentSelectedFrame = selectedFrame;
                        isSelected = true;
                    }
                    else
                    {
                        if (selectedFrame.BackgroundColor == Color.Green)
                        {
                            selectedFrame.BackgroundColor = Color.Transparent;

                            currentSelectedFrame = selectedFrame;
                            isSelected = false;
                        }
                        else
                        {
                            selectedFrame.BackgroundColor = Color.Green;
                            isSelected = true;
                        }
                    }
                }
                else
                {
                    selectedFrame.BackgroundColor = Color.Green;
                    currentSelectedFrame = selectedFrame;
                    isSelected = true;
                }

                //display next button
                if (isSelected)
                {


                    //display next button
                    imageNext.IsVisible = false;
                    await imageNext.TranslateTo(0, 0, 300);


                }
                else
                {
                    //display skip options
                    imageNext.IsVisible = false;
                    await imageNext.TranslateTo(0, 0, 300);
                }

            }



            try
            {

                //if (currentSelectedFrame == null)
                //{
                //    return;
                //}

                bodyContent.Opacity = 1;
                bodyContent.InputTransparent = false;
                // imageselector.IsVisible = false;

                Preferences.Set("ProfileImage", selectedMediaAsset.Path);


                //set the path of the image in preferences

                ImageSkipOrSelectImageClickEvent();
                isSelected = false;
                selectedMediaAsset = null;
                selectedFrame.BackgroundColor = Color.Transparent;

            }
            catch
            {

                return;

            }


        }








        async void SpeechToTextFinalResultRecieved(string args)
        {

            recon.Text = args;

          


         
            var answer = await DisplayAlert("Rename Picture", "Do you want to rename to "+ args + "", "Yes", "No");
            if (answer)
            {
                
                
                
                recon.Text = args;




                string filename = path_image2.Text.Substring(path_image2.Text.LastIndexOf("/") + 1);

                string extension = Path.GetExtension(path_image2.Text);

                string directory = Path.GetDirectoryName(path_image2.Text);

                string without = Path.GetFileNameWithoutExtension(path_image2.Text);

                if (File.Exists(directory + "/" + recon.Text + extension))
                {

                    await DisplayAlert("Duplicate", "Please change name", "OK");

                    return;

                }

                System.IO.File.Move(path_image2.Text, directory + "/" + recon.Text + extension);



                DependencyService.Get<IMediaService>().UpdateGallery(path_image2.Text);


                DependencyService.Get<IMediaService>().UpdateGallery(directory + "/" + recon.Text + extension);

                string path2 = directory + "/" + recon.Text + extension;
                string filename2 = path2.Substring(path2.LastIndexOf("/") + 1);

                path_image.Text = filename2;
                recon.Text = "";



            }
            else
            {
                recon.Text = "";

            }



          

        }



        async void refresh_Clicked(object sender, EventArgs e)
        {


            imageselector.IsVisible = false;
            var btn = sender as Button;
            btn.IsEnabled = false;
            activity.IsEnabled = true;
            activity.IsRunning = true;
            activity.IsVisible = true;
            await mainPageViewModel.LoadMediaAssets(); //load the image from phone storage
            activity.IsEnabled = false;
            activity.IsRunning = false;
            activity.IsVisible = false;
            imageselector.IsVisible = true;
            btn.IsEnabled = true;


            //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            //{

            //    btn.IsEnabled = true;
            //    return false;
            //});

        }






        async void Camera_Clicked(object sender, EventArgs e)
        {






            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Error", ":( Camera Not Available.", "OK");
                return;
            }

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {

                //===HERE IS THE PROBLEM, READ METHOD BUT NOT OPEN CAMERA! NOT ERRORS, NOT EXCEPTION, NOTHING===
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    //Directory = "Sample",
                    //Name = "test.jpg"
                });


                if (file == null)
                    return;

                // await DisplayAlert("File Location", file.Path, "OK");



                bodyContent.Opacity = 1;
                bodyContent.InputTransparent = false;
                // imageselector.IsVisible = false;

                string location = file.Path;

                string camera_image = location.Substring(location.LastIndexOf("/") + 1);
                //  await DisplayAlert("sdsdfsd", camera_image, "ok");
                string path3 = "storage/emulated/0/DCIM/Camera/" + camera_image;


                System.IO.File.Move(location, path3);



                DependencyService.Get<IMediaService>().UpdateGallery(location);


                DependencyService.Get<IMediaService>().UpdateGallery(path3);
                string temp = "storage/emulated/0/Pictures/temp/" + camera_image;

                System.IO.File.Delete(temp);
                DependencyService.Get<IMediaService>().UpdateGallery(temp);


                path_image2.Text = path3;
                path_image.Text = camera_image;



                profilePicture.Source = path3;


            }





        }





            private  void Start_Clicked(object sender, EventArgs e)
        {

            //   await  PurchaseItem("xdsd", "payload");


           task2();

            //if (path_image.Text=="")
            //{

            //  await  DisplayAlert("Error", "choose Picture", "ok");
            //    return;


            //}

            //if (!File.Exists(path_image2.Text))
            //{

            //    await DisplayAlert("Error", path_image.Text+" is not available now please refresh pictures", "ok");
            //    return;

            //}

            //try
            //{

            //    _speechRecongnitionInstance.StartSpeechToText();
            //}
            //catch (Exception ex)
            //{
            //    recon.Text = ex.Message;
            //}

            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    start.IsEnabled = false;
            //}



        }

        public async void task2()
        {



            if (path_image.Text == "")
            {

                await DisplayAlert("Error", "choose Picture", "ok");
                return;


            }

            if (!File.Exists(path_image2.Text))
            {

                await DisplayAlert("Error", path_image.Text + " is not available now please refresh pictures", "ok");
                return;

            }

            try
            {

                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                start.IsEnabled = false;
            }


        }








        public async Task<bool> PurchaseItem(string productId, string payload)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync(ItemType.Subscription);
                if (!connected)
                {
                    //we are offline or can't connect, don't try to purchase
                    return false;
                }
                //check purchases
                var purchase = await billing.PurchaseAsync(productId, ItemType.Subscription, payload);
                if (purchase.State == PurchaseState.FreeTrial)
                {

                    task2();


                }
                else
                {

                    //possibility that a null came through.
                    if (purchase == null)
                {
                    await DisplayAlert("Error", "Not purchased", "cancel");

                    //did not purchase
                }
                else
                {
                    //purchased!

                     task2();

                }
            } }
            
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Debug.WriteLine("Error: " + purchaseEx);
            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                Debug.WriteLine("Issue connecting: " + ex);
            }
            finally
            {
                await billing.DisconnectAsync();
            }
            return false;


        }















    }
}
