using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1.Data;
using System.Collections.Generic;

namespace mp3parser
{
    internal class Program
    {


        public static string Get(string url)
        {
            string html = string.Empty;
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return html;
        }


        public static void Main(string[] args)
        {
            string url = "https://serpapi.com/search?q=" + "cat" + "&tbm=isch&ijn=0";
            JObject rss = JObject.Parse(Get(url));
        }
        public static void Main2(string[] args)
        {

            string path = "D:\\Fichiers_PERSO\\Music\\Autres\\";
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (var file in directory.GetFiles("*.mp3"))
            {

                
                string url = (fetchUrl(file.Name));
                Console.WriteLine(url);
                Console.ReadKey();
                JObject rss = JObject.Parse(Get(url));
                string rsst = (string)rss["images_results"][0]["original"].ToString();


                /*downloadImage(url);
                Bitmap image = new Bitmap("../../image.png");
                //Bitmap copy = ResizeImage(image, 0, 40, image.Width, 281);
                //copy.Save("../../image.bmp");
                image.Save("../../image.bmp");
                image.Dispose();
                //copy.Dispose();
                //Console.ReadKey();
                TagLib.File f = TagLib.File.Create(path + file.Name);
                f.Tag.Title = null;
                f.Tag.Album = null;
                f.Tag.AlbumArtists = null;
                f.Tag.Performers = null;
                f.Tag.Pictures = new TagLib.IPicture[]
                {
                    new TagLib.Picture("../../image.png")
                };
                f.Save();
                deleteImage();
                Console.WriteLine("Wrote : " + file.Name);*/
            }
        }
        
      

        static Bitmap ResizeImage(Image image,int x, int y, int width, int height)
        {
            Rectangle cropArea = new Rectangle(x, y, width, height);
                Bitmap bmpImage = new Bitmap(image);
                return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }
        static void downloadImage(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(url, "../../image.png");
            }
        }

        static void deleteImage()
        {
            if (File.Exists("../../image.png"))
                File.Delete("../../image.png");
        }

        


        static string fetchUrl(string path)
        {
            path = RemoveDiacritics(path.ToLower()).Substring(0,path.Length-4);
            string sp = "./\\!? *%$£^¨µ;,§&\"\'(-_)=+}{[|`@]¤ ";
            char[] arr = new char[path.Length];
            for (int i = 0,j = 0; i < arr.Length; i++,j++)
            {
                if (sp.Contains(path[i].ToString()))
                    if (j != 0 && arr[j - 1] == '+')
                        j--;
                    else
                        arr[j] = '+';
                else
                    arr[j] = path[i];
            }
            string final = new string(arr);
            return "https://serpapi.com/search?q=" + final.Remove(final.Length-2,2)+ "&tbm=isch&ijn=0";
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}