using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;

namespace CryptographyTasks {

    class Program {

        private static readonly List<char> Alphabet = new List<char> { //латинский алфавит
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        
        //первое задание - вычисление контрольной суммы файла
        public static string GetFileControlSum(string path) {
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
        //шифрование/дешифрование производится одной фукнцией
        public static string GetAtbashCode(string text) {   
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
        //шифрование
        //индекс замены вычисляется как остаток от деления на длину алфавита, т.е. если исходная буква - 'z'
        //являющаяся 26-ой в алфавите, то её замена будет на позиции (26 + 3) % 26 = 3 - т.е. её замена - буква 'c'
        public static string CaesarEncode(string text) {
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
        public static string CaesarDecode(string encodedText) {
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
        
        public static void Main(string[] args) {
            //#1
            Console.WriteLine("File contol sum - " + GetFileControlSum("../../input.txt") + '\n');
            
            //#2
            string text = File.ReadAllText("../../input.txt");
            string atbashEncoded = GetAtbashCode(text);
            string atbashDecoded = GetAtbashCode(atbashEncoded);
            Console.WriteLine("Atbash code:");
            Console.WriteLine("1. encoded - " + atbashEncoded);
            Console.WriteLine("2. decoded - " + atbashDecoded + '\n');
            
            //#3
            string caesarEncoded = CaesarEncode(text);
            string caesarDecoded = CaesarDecode(caesarEncoded);
            Console.WriteLine("Caesar code:");
            Console.WriteLine("1. encoded - " + caesarEncoded);
            Console.WriteLine("2. decoded - " + caesarDecoded + '\n');
            
            //#4
            
        }

    }

}