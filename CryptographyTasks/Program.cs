//StringBuilder используется для оптимизации, т.к. в C# при изменении объекта типа stirng создаётся его копия, например:
//string hello = "привет";
//hello = hello + '!';
//в результате выполнения кода в памяти останется строка "привет" и создастся новая строка "привет!"
//а т.к. в коде часто происходит добавление по 1 символу к строке, то гораздо лучше использовать StringBuilder
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CryptographyTasks {

    class Program {

        private static readonly List<char> Alphabet = new List<char> { //латинский алфавит
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        
        //первое задание - вычисление контрольной суммы файла
        public static void Task1() {
            Console.WriteLine("File contol sum - " + GetFileMd5("../../lorem ipsum.txt") + '\n'); //вывод контрольной суммы
        }
        
        private static string GetFileMd5(string path) {
            MD5 md5 = MD5.Create(); //создаём экземпляр класса md5
            StringBuilder result = new StringBuilder(); //сюда будем записывать результат
            FileStream fileStream = File.OpenRead(path); //создаём битовый файловый поток на основе файла
            
            byte[] bytes = md5.ComputeHash(fileStream); //создаём контрольную сумму на основе файлового потока
            foreach (byte b in bytes) { //каждый байт преобразуем в 16-ную систему счисления
                result.Append(b.ToString("x2")); //пример преобразования - 255 (11111111) -> ff
            }

            return result.ToString(); //возвращаем результат, приведённый к типу string
        }

        //второе задание, часть 1 - шифр "атбаш"
        public static void Task2_1() {
            string atbashEncoded = GetAtbashCode(File.ReadAllText("../../lorem ipsum.txt"));
            string atbashDecoded = GetAtbashCode(atbashEncoded);
            Console.WriteLine("Atbash cipher code:");
            Console.WriteLine("1. encoded - " + atbashEncoded); //вывод закодированных данных
            Console.WriteLine("2. decoded - " + atbashDecoded + '\n'); //вывод декодированных данных
        }
        
        //шифрование/дешифрование производится одной фукнцией
        private static string GetAtbashCode(string text) {   
            StringBuilder result = new StringBuilder(); //сюда будем записывать результат
            
            for (int i = 0; i < text.Length; i++) { //для каждого символа из текста
                if (Char.IsLetter(text[i])) { //если символ является буквой, то
                    if (Char.IsUpper(text[i])) { //если буква заглавная, то приводим её к маленькой, находим замену, и результат приводим к заглавной
                        result.Append(Char.ToUpper(Alphabet[Alphabet.Count - Alphabet.FindIndex(x => x == Char.ToLower(text[i])) - 1]));
                    } else { //иначе просто заменяем
                        result.Append(Alphabet[Alphabet.Count - Alphabet.FindIndex(x => x == text[i]) - 1]);
                    }
                } else { //иначе добавляем символ в результат без изменений
                    result.Append(text[i]);
                }
            }
            return result.ToString(); //возвращаем результат, приведённый к типу string
        }

        //второе задание, часть 2 - шифр Цезаря
        public static void Task2_2() {
            string caesarEncoded = CaesarCipherEncode(File.ReadAllText("../../lorem ipsum.txt"));
            string caesarDecoded = CaesarCipherDecode(caesarEncoded);
            Console.WriteLine("Caesar cipher code:");
            Console.WriteLine("1. encoded - " + caesarEncoded); //вывод закодированных данных
            Console.WriteLine("2. decoded - " + caesarDecoded + '\n'); //вывод декодированных данных
        }
        
        //шифрование
        //индекс замены вычисляется как остаток от деления на длину алфавита, т.е. если исходная буква - 'z'
        //являющаяся 26-ой в алфавите, то её замена будет на позиции (26 + 3) % 26 = 3 - т.е. её замена - буква 'c'
        private static string CaesarCipherEncode(string text) {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; i++) { //для каждого символа из текста
                if (Char.IsLetter(text[i])) { //если символ является буквой, то
                    if (Char.IsUpper(text[i])) { //если буква заглавная, то приводим её к маленькой, находим замену, и результат приводим к заглавной
                        result.Append(Char.ToUpper(Alphabet[(Alphabet.FindIndex(x => x == Char.ToLower(text[i])) + 3) % Alphabet.Count]));
                    } else { //иначе просто заменяем
                        result.Append(Alphabet[(Alphabet.FindIndex(x => x == text[i]) + 3) % Alphabet.Count]);
                    }
                } else { //иначе добавляем символ в результат без изменений
                    result.Append(text[i]);
                }
            }
            
            return result.ToString(); //возвращаем результат, приведённый к типу string
        }

        //дешифрование
        //пусть нужно дешифровать букву 'a', которая ялвяется первой в алфавите, тогда 
        //её замена будет на позиции (26 + 1 - 3) % 26 = 24 - т.е её замена - бувка 'x'
        private static string CaesarCipherDecode(string encodedText) {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < encodedText.Length; i++) { //для каждого символа из текста
                if (Char.IsLetter(encodedText[i])) { //если символ является буквой, то
                    if (Char.IsUpper(encodedText[i])) { //если буква заглавная, то приводим её к маленькой, находим замену, и результат приводим к заглавной
                        result.Append(Char.ToUpper(Alphabet[(Alphabet.Count + Alphabet.FindIndex(x => x == Char.ToLower(encodedText[i])) - 3) % Alphabet.Count]));
                    } else { //иначе просто заменяем
                        result.Append(Alphabet[(Alphabet.Count + Alphabet.FindIndex(x => x == encodedText[i]) - 3) % Alphabet.Count]);
                    }
                } else { //иначе добавляем символ в результат без изменений
                    result.Append(encodedText[i]);
                }
            }
            
            return result.ToString(); //возвращаем результат, приведённый к типу string
        }
        
        //третье задание - маршрутное шифрование
        public static void Task3() {
            int n, m;
            string key;
            while (true) { //требуем ввести данные, пока они не будут корректны
                Console.Write("Enter n: ");
                n = int.Parse(Console.ReadLine()); 
                Console.Write("Enter m: ");
                m = int.Parse(Console.ReadLine());

                if (n == m || n * m <= 0) { //n и m не должны быть равны и должны быть больше единицы
                    Console.WriteLine("Wrong data, try again.");
                    continue; //возвращаемся к вводу n и m
                }
                
                Console.Write("Enter key word (" + m + " symbols): "); 
                key = Console.ReadLine();

                if (key.Length != m) { //ключ должен быть длины m
                    Console.Write("Key word must have " + m + "symbols.");
                    continue;
                }
                break; //если всё корректно введено, то здесь цикл прервётся
            }

            //удаляем лишние символы и вызываем функцию шифрования
            string routeEncoded = RouteCipherEncode(RemoveSymbols(File.ReadAllText("../../lorem ipsum.txt")), n, m, key);
            //string routeDecoded = RouteCipherDecode(routeEncoded);
            Console.WriteLine("Route cipher code:");
            Console.WriteLine("1. encoded - " + routeEncoded);
            //Console.WriteLine("2. decoded - " + routeDecoded + '\n');
        }
        
        //добавление символов в конец
        private static string AppendSymbols(string row, int length) {
            StringBuilder result = new StringBuilder(row); //здесь будет результат
            while (result.Length != length) { //добавляем в конец символ 'x', пока длина блока не достигнет нужной длины
                result.Append('x');
            }

            return result.ToString(); //возвращаем результат
        }

        //удаление всех символов кроме букв
        private static string RemoveSymbols(string text) {
            StringBuilder result = new StringBuilder(text); //здесь будет результат
            for (int i = 0; i < result.Length; i++) { //пока не прошли все символы
                if (!Char.IsLetter(result[i])) { //если символ не является буквой, то удаляем его и уменьшаем i
                    result.Remove(i, 1);
                    i--;
                }
            }

            return result.ToString();
        }

        //выясняем порядок вывода столбцов
        private static ArrayList GetShowOrder(string key) {
            ArrayList result = new ArrayList(); //здесь будет результат
            string sorted = String.Concat(key.OrderBy(x => x)); //сортируем буквы ключа в алфавитном порядке

            for (int i = 0; i < key.Length; i++) { //добавляем индексы столбцов в массив в нужном порядке
                //ищем позицию элемента из отсортированных букв ключа в самом ключе
                //пусть ключ 'qwert', отсортированные буквы ключа - 'eqrtw'
                //'e' находится на 3 позиции в ключе => первым выводим третий столбец и т.д.
                result.Add(key.IndexOf(sorted[i]));
            }
            
            return result;
        }
        
        //шифрование
        public static string RouteCipherEncode(string text, int n, int m, string key) {     
            ArrayList entries = new ArrayList(); //здесь будут храниться блоки из n * m символов
            
            while (text.Length != 0) { //делим входной текст на блоки
                if (text.Length < n * m) { //если длина оставшегося текст меньше нужной длины n * m, тогда добавляем в конец символы
                    entries.Add(AppendSymbols(text, n * m));
                    break; //и выходим из цикла, т.к. получено максимум возможных блоков
                }
                entries.Add(text.Substring(0, n * m)); //иначе добавляем блок в массив
                text = text.Remove(0, n * m); //и удаляем добавленные данные из строки
            }

            ArrayList order = GetShowOrder(key); //получаем нужный порядок вывода столбцов
            StringBuilder result = new StringBuilder(); //здесь будет результат шифрования
            for (int i = 0; i < entries.Count; i++) { //для каждого блока
                char[,] matrix = new char[n + 1, m]; //создаём новую матрицу
                entries[i] += key; //добавляем в конец блока ключ

                for (int j = 0; j < n + 1; j++) {
                    for (int k = 0; k < m; k++) {
                        //заполняем матрицу символами из текущего блока
                        //j - индекс строки, k - индекс столбца. пусть n = 4, тогда
                        //при j = 0 index = j + j * n + k принимает значения от 0 до 4 включительно
                        //при j = 1 - от 5 до 9 и т.д.
                        //таким образом index пройдёт все значения от 0 до m^2 невключительно, т.е. весь блок будет просмотрен
                        matrix[j, k] = ((string) entries[i])[j + j * n + k];
                    }
                }

                foreach (int index in order) { //добавляем содержимое столбцов в результат
                    for (int j = 0; j < n; j++) { //index - индекс столбца, j - индекс строки
                        result.Append(matrix[j, index]);
                    }
                }
            }

            return result.ToString(); //возвращаем результат
        }
        
//        //дешифрование        
//        public static string RouteCipherDecode() {
//            
//        }

        public static void Main(string[] args) {
//            //#1
//            Task1();
//            //#2.1
//            Task2_1();
//            //#2.2
//            Task2_2();
            //#3
            Task3();
            //#4
            
            //#5
        }

    }

}