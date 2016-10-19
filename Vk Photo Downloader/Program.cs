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
                        Array.Sort(filesCollection);
                        Bitmap[] bitmapCollection = new Bitmap[filesCollection.Length];
                        int i = 0;
                        foreach (string item in filesCollection)
                        {
                            bitmapCollection[i] = new Bitmap(item.ToString());
                            i++;
                            Console.WriteLine("{0} из {1}",i,filesCollection.Length);
                        }
                        // узнаем 1 процент от общего кол-ва элементов
                        double percent = (double)bitmapCollection.Length / (double)100;
                        // 100 %
                        List<double> percentsList = new List<double>();
                        for (int s = 1; s < 101; s++)
                        {   
                            percentsList.Add(percent * s);
                        }
                        // 
                        
                        Console.WriteLine("Коллекция фотографий успешно собрана.");
                        Console.WriteLine("Начинается проверка фотографий....");
                        
                        int value = 1;
                        bool bFlag = false;
                        List<TrashPaths> repeatPhotosList = new List<TrashPaths>();
                        List<int> arrayX = new List<int>(); // Чтобы J не повторял уже пройденные X
                            for (int x = 0; x < bitmapCollection.Length; x++)
                            {
                              //  Console.WriteLine("Цикл {0} из {1} итераций", x, bitmapCollection.Length - 1);
                                for (int j = 0; j < bitmapCollection.Length; j++)
                                {
                                    bool isNext = false;
                                    if (arrayX.Count >= 1)
                                    {
                                        foreach (var arrayItem in arrayX)
                                        {
                                            if (j == arrayItem)
                                            {
                                                isNext = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (isNext == true)
                                    {
                                        continue;
                                    }
                                    bFlag = PhotoComparing.ImageCompareArray(bitmapCollection[x], bitmapCollection[j]);
                                    if (bFlag == true && x != j)
                                    {
                                        
                                        ChangeColor.greenForeground();
                                        string TheSameX = filesCollection[x];
                                        string TheSameJ = filesCollection[j];
                                       TheSameX = TheSameX.Replace(@"D:\VkPhotos\","");
                                        TheSameJ = TheSameJ.Replace(@"D:\VkPhotos\", "");

                                        repeatPhotosList.Add(new TrashPaths() { image = bitmapCollection[x], path = TheSameX });
                                        Console.WriteLine("Найден подобный. " + TheSameX + " похож с " + TheSameJ);
                                        ChangeColor.ResetColor();
                                        arrayX.Add(x);
                                    }
                                    bFlag = false;
                                }
                                if (value < 100)
                                {
                                    if (((double)x >= (percentsList[value] - 2)) && ((double)x <= percentsList[value]))
                                    {
                                        ChangeColor.redForeground();
                                        Console.WriteLine("{0}%", value);
                                        value++;
                                        ChangeColor.ResetColor();
                                    }
                                }
                                
                              
                            }
                            Console.WriteLine();
                            ChangeColor.greenForeground();
                            Console.WriteLine("Копирование дубликатов...");
                            ChangeColor.ResetColor();
                            int kolvo = 0;
                        DirectoryInfo dir2 = new DirectoryInfo(@"D:\VkPhotos\Trash");
                        dir2.Create();
                            foreach (var image in repeatPhotosList)
                            {
                                image.image.Save(string.Format("D:\\VkPhotos\\Trash\\{0}.png", image.path), System.Drawing.Imaging.ImageFormat.Png);
                                
                                kolvo++;
                            }
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
