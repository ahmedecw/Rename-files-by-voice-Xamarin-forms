using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImagePicker.Model;

namespace ImagePicker
{
    public interface IMediaService
    {
        event EventHandler<MediaEventArgs> OnMediaAssetLoaded;
        bool IsLoading { get; }
        Task<IList<MediaAssest>> RetrieveMediaAssetsAsync(CancellationToken? token = null);
        Task<string> StoreProfileImage(string path);
        Task<string> GetImageWithCamera();

        byte[] ResizeImage(string imagePath, float width, float height);

        string UpdateGallery(string filePath);

    }
}
