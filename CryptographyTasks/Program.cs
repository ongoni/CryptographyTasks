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
            Console.WriteLine("File contol sum - " + GetFileMd5("../../lorem ipsum.txt") + '\n');
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
            Console.WriteLine("1. encoded - " + atbashEncoded);
            Console.WriteLine("2. decoded - " + atbashDecoded + '\n');
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
            Console.WriteLine("1. encoded - " + caesarEncoded);
            Console.WriteLine("2. decoded - " + caesarDecoded + '\n');
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
            while (true) {
                Console.Write("Enter n: ");
                n = int.Parse(Console.ReadLine());
                Console.Write("Enter m: ");
                m = int.Parse(Console.ReadLine());

                if (n == m || n * m <= 0) {
                    Console.WriteLine("Wrong data, try again.");
                    continue;
                }
                
                Console.Write("Enter key word (" + m + " symbols): ");
                key = Console.ReadLine();

                if (key.Length != m) {
                    Console.Write("Key word must have " + m + "symbols.");
                    continue;
                }
                break;
            }

            string routeEncoded = RouteCipherEncode(RemoveSymbols(File.ReadAllText("../../lorem ipsum.txt")), n, m, key);
            //string routeDecoded = RouteCipherDecode(routeEncoded);
            Console.WriteLine("Route cipher code:");
            Console.WriteLine("1. encoded - " + routeEncoded);
            //Console.WriteLine("2. decoded - " + routeDecoded + '\n');
        }

        private static string AppendSymbols(string row, int length) {
            StringBuilder result = new StringBuilder(row);
            while (result.Length != length) {
                result.Append('x');
            }

            return result.ToString();
        }

        private static string RemoveSymbols(string text) {
            StringBuilder result = new StringBuilder(text);
            for (int i = 0; i < result.Length; i++) {
                if (!Char.IsLetter(result[i]) || result[i] == ' ') {
                    result.Remove(i, 1);
                    i--;
                }
            }

            return result.ToString();
        }

        private static ArrayList GetShowOrder(string key) {
            ArrayList result = new ArrayList();
            string sorted = String.Concat(key.OrderBy(c => c));

            for (int i = 0; i < key.Length; i++) {
                result.Add(key.IndexOf(sorted[i]));
            }
            
            return result;
        }
        
        //шифрование
        public static string RouteCipherEncode(string text, int n, int m, string key) {     
            ArrayList entries = new ArrayList();
            
            while (text.Length != 0) {
                if (text.Length < n * m) {
                    entries.Add(AppendSymbols(text, n * m));
                    break;
                }
                entries.Add(text.Substring(0, n * m));
                text = text.Remove(0, n * m);
            }

            ArrayList order = GetShowOrder(key);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < entries.Count; i++) {
                char[,] matrix = new char[n + 1, m];
                entries[i] += key;

                for (int j = 0; j < n + 1; j++) {
                    for (int k = 0; k < m; k++) {
                        int index = j + j * n + k;
                        matrix[j, k] = ((string) entries[i])[index];
                    }
                }

                foreach (int index in order) {
                    for (int j = 0; j < n; j++) {
                        result.Append(matrix[j, index]);
                    }
                }
            }

            return result.ToString();
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