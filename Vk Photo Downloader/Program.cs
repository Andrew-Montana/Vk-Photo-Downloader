using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.RequestParams;
using System.IO;
using System.Drawing;

namespace Vk_Photo_Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите что вы хотите делать:");
                ChangeColor.greenForeground();
                Console.Write("\ndownload");
                ChangeColor.ResetColor();
                Console.Write(" - скачать фотографии");
                ChangeColor.greenForeground();
                Console.Write("\ncompare");
                ChangeColor.ResetColor();
                Console.Write(" - сравнить фотографии и удалить повторяющиеся (если есть) ");
                ChangeColor.greenForeground();
                Console.Write("\nexit");
                ChangeColor.ResetColor();
                Console.Write(" - закрыть текущее приложение\n >");
                ChangeColor.redForeground();
                string selection = Console.ReadLine();
                ChangeColor.ResetColor();
                switch (selection)
                {
                    #region exit
                    case "exit":
                        Environment.Exit(0);
                        break;
                    #endregion
                    #region default
                    default:
                        Console.WriteLine("sometimes, you just gotta say.. what the fuck");
                        Console.WriteLine("Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    #endregion
                    #region compare
                    case "compare":
                        string[] filesCollection = Directory.GetFiles(@"D:\VkPhotos"); // path of every single photo
                        
                        Bitmap[] bitmapCollection = new Bitmap[filesCollection.Length];
                        int i = 0;
                        foreach (string item in filesCollection)
                        {
                            bitmapCollection[i] = new Bitmap(item.ToString());
                            i++;
                            Console.WriteLine("{0} из {1}",i,filesCollection.Length);
                        }
                        Console.WriteLine("Коллекция фотографий успешно собрана.");
                        Console.WriteLine("Начинается проверка фотографий....");
                        Console.WriteLine("Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    #endregion
                    #region DownloadPhotos
                    case "download":
                        DirectoryInfo dir = new DirectoryInfo(@"D:\VkPhotos");
                        dir.Create();
                        Console.WriteLine("Создана директория D:\\VkPhotos");
                        // Создание 2х объектов
                        VkApi Api = new VkApi();
                        PhotoGetParams photoParams = new PhotoGetParams();
                        // Свойства photoParams
                        photoParams.AlbumId = VkNet.Enums.SafetyEnums.PhotoAlbumType.Wall; // Фото со стены
                        photoParams.OwnerId = -102018175; // id паблика
                        ChangeColor.greenForeground();
                        Console.WriteLine("ID вашего паблика = {0}", photoParams.OwnerId);
                        //  string path = Console.ReadLine();
                        // 
                        var photos = Api.Photo.Get(photoParams);
                        int kol = 0;
                        ChangeColor.whiteForeground();
                        Console.Write("\nКоллекция готова. Количество фотографий: ");
                        ChangeColor.redForeground();
                        Console.Write(photos.TotalCount);
                        ChangeColor.whiteForeground();
                        Console.WriteLine("\nНачинаем скачивать.....");
                        ChangeColor.ResetColor();
                        // Скачиваем на Пк
                        using (WebClient webClient = new WebClient())
                        {
                            foreach (var photo in photos)
                            {
                                if (photo.Photo2560 != null)
                                {
                                    webClient.DownloadFile(photo.Photo2560.AbsoluteUri.ToString(), string.Format(@"D:\VkPhotos\{0}.png", kol));
                                    Console.WriteLine(kol.ToString() + ".png");
                                }
                                else if (photo.Photo1280 != null)
                                {
                                    webClient.DownloadFile(photo.Photo1280.AbsoluteUri.ToString(), string.Format(@"D:\VkPhotos\{0}.png", kol));
                                    Console.WriteLine(kol.ToString() + ".png");
                                }
                                else if (photo.Photo807 != null)
                                {
                                    webClient.DownloadFile(photo.Photo807.AbsoluteUri.ToString(), string.Format(@"D:\VkPhotos\{0}.png", kol));
                                    Console.WriteLine(kol.ToString() + ".png");
                                }
                                else if (photo.Photo604 != null)
                                {
                                    webClient.DownloadFile(photo.Photo604.AbsoluteUri.ToString(), string.Format(@"D:\VkPhotos\{0}.png", kol));
                                    Console.WriteLine(kol.ToString() + ".png");
                                }
                                else if (photo.Photo130 != null)
                                {
                                    webClient.DownloadFile(photo.Photo130.AbsoluteUri.ToString(), string.Format(@"D:\VkPhotos\{0}.png", kol));
                                    Console.WriteLine(kol.ToString() + ".png");
                                }
                                else if (photo.Photo75 != null)
                                {
                                    webClient.DownloadFile(photo.Photo75.AbsoluteUri.ToString(), string.Format(@"D:\VkPhotos\{0}.png", kol));
                                    Console.WriteLine(kol.ToString() + ".png");
                                }
                                kol++;
                            }
                        }
                        ChangeColor.greenForeground();
                        Console.WriteLine("{0} фотографий успешно скачаны.", kol);
                        Console.WriteLine("Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    #endregion
                }
            }
        }
    }
}
