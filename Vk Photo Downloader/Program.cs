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
            bool canDelete = false;
            List<string> filesToDelete = new List<string>();
            string mainPath = @"D:\VkPhotos";
            ChangeColor.whiteForeground();
            Console.Write("Хотите ли Вы изменить путь к главной папке?\n Все действия будут связаны с этой директорией ");
            ChangeColor.redForeground();
            Console.Write("yes/no");
            ChangeColor.greenForeground();
            Console.Write("\n По умолчанию: D:\\VkPhotos\n");
            ChangeColor.ResetColor();
            Console.Write("> ");
            while (true)
            {
                string yesNo = Console.ReadLine();
                if (yesNo == "no")
                {
                    break;
                }
                else if (yesNo == "yes")
                {
                    Console.Write("\nВведите новый путь. Пример D:\\VkPhotos\n> ");
                    mainPath = Console.ReadLine();
                    ChangeColor.greenForeground();
                    Console.WriteLine("Путь успешно изменен на {0}", mainPath);
                    ChangeColor.ResetColor();
                    break;
                }
                else
                {
                    Console.WriteLine("Введите ответ: yes или no");
                }
            }
           

            if (!Directory.Exists(mainPath))
            {
                DirectoryInfo dir = new DirectoryInfo(mainPath);
                dir.Create();
                Console.WriteLine("Создана директория {0}", mainPath);
            }
            while (true)
            {
                // ok
                #region console
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
                #endregion // ok
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
                        //ok
                    #region compare
                    case "compare":
                        string[] filesCollection = Directory.GetFiles(mainPath, "*.png"); // path of every single photo
                        
                        if (Directory.Exists(mainPath) && filesCollection.Length >= 2)
                        {
                            Array.Sort(filesCollection);
                            Bitmap[] bitmapCollection = new Bitmap[filesCollection.Length];
                            int i = 0;
                            foreach (string item in filesCollection)
                            {
                                bitmapCollection[i] = new Bitmap(item.ToString());
                                i++;
                                Console.WriteLine("{0} из {1}", i, filesCollection.Length);
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

                          //  int value = 1;
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
                                        TheSameX = TheSameX.Replace(mainPath + @"\", "");
                                        TheSameJ = TheSameJ.Replace(mainPath + @"\", "");

                                        repeatPhotosList.Add(new TrashPaths() { image = bitmapCollection[x], path = TheSameX });
                                        Console.WriteLine("Найден подобный. " + TheSameX + " похож с " + TheSameJ);
                                        
                                        ChangeColor.ResetColor();
                                        arrayX.Add(x);
                                        filesToDelete.Add(filesCollection[x]);
                                    }
                                    bFlag = false;
                                }
                                Console.WriteLine("{0} из {1} проверок", x, bitmapCollection.Length);
                                /*
                                //if (value < 100)
                               // {
                                    if (((double)x >= (percentsList[value] - 2)) && ((double)x <= percentsList[value]))
                                    {
                                        ChangeColor.redForeground();
                                        Console.WriteLine("{0} из {1} проверок", x,j);
                                        value++;
                                        ChangeColor.ResetColor();
                                    }
                               // }*/


                            }
                            Console.WriteLine();
                            ChangeColor.redForeground();
                            Console.WriteLine("100% Завершено");
                            ChangeColor.greenForeground();
                            Console.WriteLine("Копирование дубликатов...");
                            ChangeColor.ResetColor();
                            int kolvo = 0;
                            if (!Directory.Exists(mainPath + @"\Trash"))
                            {
                                DirectoryInfo dir2 = new DirectoryInfo(mainPath + @"\Trash");
                                dir2.Create();
                            }
                            foreach (var image in repeatPhotosList)
                            {
                                image.image.Save(string.Format(mainPath + @"\Trash\{0}", image.path), System.Drawing.Imaging.ImageFormat.Png);

                                kolvo++;
                            }
                            canDelete = true;
                            for (int d = 0; d < bitmapCollection.Length; d++)
                            {
                                bitmapCollection[d].Dispose();
                            }
                            if (canDelete == true)
                            {

                                ChangeColor.redForeground();
                                Console.WriteLine("Начинается процесс удаления лишних изображений в главной папке...");
                                foreach (var deleteFile in filesToDelete)
                                {
                                    File.Delete(deleteFile);
                                    Console.WriteLine("{0} удален", deleteFile.ToString());
                                }
                                ChangeColor.ResetColor();
                                canDelete = false;
                            }
                            filesToDelete.Clear();
                            Console.WriteLine("Нажмите Enter для продолжения...");
                            Console.ReadLine();
                        }
                        else
                        {
                            ChangeColor.redForeground();
                            Console.WriteLine("{0} не существует. Измените путь на подобающий. Пример: D:\\photos",mainPath);
                            Console.WriteLine("Или в {0} меньше 2 изображений.\n Пожалуйста, выполните сперва команду download", mainPath);
                            ChangeColor.ResetColor();
                        }
                            break;
                        
                    #endregion

                    #region DownloadPhotos
                    case "download":
                            if (Directory.Exists(mainPath))
                            {
                               
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
                                            webClient.DownloadFile(photo.Photo2560.AbsoluteUri.ToString(), string.Format(mainPath + @"\{0}.png", kol));
                                            Console.WriteLine(kol.ToString() + ".png");
                                        }
                                        else if (photo.Photo1280 != null)
                                        {
                                            webClient.DownloadFile(photo.Photo1280.AbsoluteUri.ToString(), string.Format(mainPath + @"\{0}.png", kol));
                                            Console.WriteLine(kol.ToString() + ".png");
                                        }
                                        else if (photo.Photo807 != null)
                                        {
                                            webClient.DownloadFile(photo.Photo807.AbsoluteUri.ToString(), string.Format(mainPath + @"\{0}.png", kol));
                                            Console.WriteLine(kol.ToString() + ".png");
                                        }
                                        else if (photo.Photo604 != null)
                                        {
                                            webClient.DownloadFile(photo.Photo604.AbsoluteUri.ToString(), string.Format(mainPath + @"\{0}.png", kol));
                                            Console.WriteLine(kol.ToString() + ".png");
                                        }
                                        else if (photo.Photo130 != null)
                                        {
                                            webClient.DownloadFile(photo.Photo130.AbsoluteUri.ToString(), string.Format(mainPath + @"\{0}.png", kol));
                                            Console.WriteLine(kol.ToString() + ".png");
                                        }
                                        else if (photo.Photo75 != null)
                                        {
                                            webClient.DownloadFile(photo.Photo75.AbsoluteUri.ToString(), string.Format(mainPath + @"\{0}.png", kol));
                                            Console.WriteLine(kol.ToString() + ".png");
                                        }
                                        kol++;
                                    }
                                }
                                ChangeColor.greenForeground();
                                Console.WriteLine("{0} фотографий успешно скачаны.", kol);
                                Console.WriteLine("Нажмите Enter для продолжения...");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("{0} не существует. Измените путь на подобающий. Пример: D:\\photos", mainPath);
                                
                            }
                        break;
                    #endregion
                }
                
            }
        }
    }
}
