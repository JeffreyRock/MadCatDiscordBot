using System;
using System.IO;
using System.Linq;


namespace AI
{
    public class ImageHandler
    {
        public string GetLatestImageFromFolder(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    throw new DirectoryNotFoundException($"The folder '{folderPath}' does not exist.");
                }
                string[] imageFiles = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly);

                if (imageFiles.Length > 0)
                {

                    string latestImage = imageFiles.OrderByDescending(file => new FileInfo(file).CreationTime).First();

                    return latestImage;
                }
                else
                {
                    throw new FileNotFoundException($"No image files found in the folder '{folderPath}'.");
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}