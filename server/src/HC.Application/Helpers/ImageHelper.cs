using System.IO;

namespace HC.Application.Helpers;

public static class ImageHelper
{
    public static byte[] ImageToByteArrayFromFilePath(string imagefilePath)
    {
        byte[] imageArray = File.ReadAllBytes(imagefilePath);
        return imageArray;
    }
}