//StringBuilder используется для оптимизации, т.к. в C# при изменении объекта типа stirng создаётся его копия, например:
//string hello = "привет";
//hello = hello + '!';
//в результате выполнения кода в памяти останется строка "привет" и создастся новая строка "привет!"
//а т.к. в коде часто происходит добавление по 1 символу к строке, то гораздо лучше использовать StringBuilder
//1-индексация - естественная индексация с 1, т.е. первый элемент находится на 1 месте, второй - на 2 и т.д.
//0-индексация - "компьютерная" индексация с 0, т.е. первый элемент находится на 0 месте, второй - на 1 и т.д.
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CryptographyTasks {

    class Program {

        private static readonly string LoremIpsumPath = "../../lorem ipsum.txt"; //путь к файлу с обычными текстом
        private static readonly string LoremIpsumLongPath = "../../lorem ipsum (long).txt"; //путь к файлу с большим числом строк
        private static readonly string LoremIpsumClassicPath = "../../lorem ipsum (classic).txt"; //путь к файлу с большим количеством обычноготекста
        private static readonly List<char> Alphabet = new List<char> { //латинский алфавит
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        }; //латинский алфавит
        
        //первое задание - вычисление контрольной суммы файла
        public static void Task1() {
            Console.WriteLine("File contol sum - " + GetFileMd5(LoremIpsumPath) + '\n'); //вывод контрольной суммы
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
            string atbashEncoded = GetAtbashCode(File.ReadAllText(LoremIpsumPath));
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
            string caesarEncoded = CaesarCipherEncode(File.ReadAllText(LoremIpsumPath));
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
            Console.WriteLine("Route cipher code:");
            
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
            string routeEncoded = RouteCipherEncode(RemoveSymbols(File.ReadAllText(LoremIpsumPath)), n, m, key);
            string routeDecoded = RouteCipherDecode(routeEncoded, n, m, key);
            Console.WriteLine("1. encoded - " + routeEncoded);
            Console.WriteLine("2. decoded - " + routeDecoded + '\n');
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
                        //заполняем матрицу символами из текущего i-го блока
                        //j - индекс строки, k - индекс столбца. пусть n = 4, тогда
                        //при j = 0 index = j + j * n + k принимает значения от 0 до 4 включительно (в 0-индексации)
                        //при j = 1 - от 5 до 9 и т.д.
                        //таким образом index пройдёт все значения от 0 до m^2 невключительно, т.е. весь блок будет просмотрен
                        matrix[j, k] = ((string) entries[i])[j + j * n + k];
                    }
                }
                
                //сначала добавляем содержимое столбец, который имеет первую по алфавиту букву ключа, потому вторую и т.д.
                //пусть ключ - 'qwert', отсортированные буквы ключа - 'eqrtw' => сначала запишем третий (в 1-индексации) 
                //столбец в результат, потом первый, затем четвёртый, пятый и второй
                foreach (int index in order) { 
                    for (int j = 0; j < n; j++) {
                        result.Append(matrix[j, index]);
                    }
                }
            }

            return result.ToString(); //возвращаем результат
        }
        
        //дешифрование        
        public static string RouteCipherDecode(string text, int n, int m, string key) {
            ArrayList entries = new ArrayList(); //здесь будут храниться блоки из n * m символов
            
            while (text.Length != 0) { //делим текст на блоки
                entries.Add(text.Substring(0, n * m)); //иначе добавляем блок в массив
                text = text.Remove(0, n * m); //и удаляем добавленные данные из строки
            }
            
            ArrayList order = GetShowOrder(key); //получаем порядок в котором выводились столбцы при шифровании
            StringBuilder result = new StringBuilder(); //здесь будет результат дешифрования

            for (int i = 0; i < entries.Count; i++) { //для каждого блока
                char[,] matrix = new char[n, m]; //создаём новую матрицу

                for (int j = 0; j < m; j++) {
                    for (int k = 0; k < n; k++) {
                        //заполняем матрицу символами из текущего i-го блока
                        //k - индекс строки, j - индекс столбца. пусть n = 4, тогда
                        //при j = 0 index = j + j * (n - 1) + k принимает значения от 0 до 3 включительно (в 0-индексации)
                        //при j = 1 - от 4 до 7 и т.д.
                        //таким образом index пройдёт все значения от 0 до n * m невключительно, т.е. весь блок будет просмотрен
                        //получается, что мы считываем по 4 символа и заполняем ими столбцы матрицы
                        matrix[k, j] = ((string) entries[i])[j + j * (n - 1) + k];
                    }
                }
                
                //создаём новую матрицу, в которой будут находиться элементы в том порядке,
                //в каком они были до шифрования
                char[,] resultMatrix = new char[n, m]; 
                for (int j = 0; j < m; j++) { 
                    for (int k = 0; k < n; k++) {
                        //в order хранится порядок, в котором выводились столбцы матрицы при шифровании
                        //поэтому, чтобы восстановить матрицу полученную перед шифрованием нужно 
                        //в k-ую строки и столбец order[j] новой записать элемент с индексами k и j
                        //из матрицы, созданной на основе зашифрованных данных
                        resultMatrix[k, (int) order[j]] = matrix[k, j];
                    }
                }

                //поэлементно слева направо записиываем элементы матрицы в результат
                for (int j = 0; j < n; j++) { 
                    for (int k = 0; k < m; k++) {
                        result.Append(resultMatrix[j, k]);
                    }
                }
            }

            return result.ToString(); //возвращаем результат
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

            return result.ToString(); //возвращаем результат
        }

        //выясняем порядок вывода столбцов
        private static ArrayList GetShowOrder(string key) {
            ArrayList result = new ArrayList(); //здесь будет результат
            string sorted = String.Concat(key.OrderBy(x => x)); //сортируем буквы ключа в алфавитном порядке

            for (int i = 0; i < key.Length; i++) { //добавляем индексы столбцов в массив в нужном порядке
                //ищем позицию элемента из отсортированных букв ключа в самом ключе
                //пусть ключ 'qwert', отсортированные буквы ключа - 'eqrtw'
                //'e' находится на 3 позиции в ключе (в 1-индесации) => первым выводим третий столбец и т.д.
                result.Add(key.IndexOf(sorted[i]));
            }
            
            return result; //возвращаем результат
        }

        //четвёртное задание
        public static void Task4() {
            Console.WriteLine("Task 4 Bit cipher code:");
            Console.Write("Enter phrase to encode: ");
            string data = Console.ReadLine();
            
            //вызываем функцию шифрования, сразу отправляя туда биты из двоичного представления данных, которые нужно зашифровать
            BitCipherEncode(LoremIpsumLongPath, GetBits(data)); 
            string bitCipherDecoded = BitCipherDecode(LoremIpsumLongPath);
            Console.WriteLine("Decoded phrase - " + bitCipherDecoded + '\n');
        }
        
        //шифрование
        //после шифрования данные в файле переписываются вместе с зашифрованными данными
        public static void BitCipherEncode(string path, string data) {
            StringBuilder result = new StringBuilder(); //здесь будет результат
            //из входного файла считываем весь текст, разделив его на части по переводу строки, т.е. текст
            //привет мир,
            //я вернулся!
            //разделится на две части - "привет мир," и "я вернулся!"
            string[] text = File.ReadAllText(path).Split('\n'); 
            for (int i = 0; i < text.Length; i++) { 
                //для каждой части текста проверяем наличие пробела в конце строки
                if (text[i].EndsWith(" ")) {
                    //если пробел в конце есть, то удаляем его, чтобы он не помешал при дешифровании
                    //т.е. обрезаем строку с 0-го символа по предпоследний
                    text[i] = text[i].Substring(0, text[i].Length - 1); 
                }
            }
            
            for (int i = 0; i < data.Length; i++) { //для каждого бита из фразы, которую нужно зашифровать
                if (data[i] == '1') { //если бит равен 1, то добавляем пробел
                    text[i] += ' ';
                } //иначе ничего не делаем
            }
            
            //добавляем перевод строки каждой строке, чтобы записать текст в исходном виде,
            //но с уже зашифрованными данными
            foreach (string s in text) { 
                result.Append(s + '\n'); //добавляем текущую строку, прибавив перевод строки
            }

            File.WriteAllText(path, result.ToString()); //заполняем файл по указанном пути path полученными данными result
            
            Console.WriteLine("Secret phrase encoded to file " + path); //выводим сообщение о том, что данные зашифрованы
        }
        
        //дешифрование
        //после дешифрования файл с зашифрованными данными не изменяется, сами зашифрованные данные возвращаются в виде строки типа string
        public static string BitCipherDecode(string path) {
            StringBuilder bits = new StringBuilder(); //здесь будут биты, полученные из текста с зашифрованными данными
            //из входного файла считываем весь текст, разделив его на части по переводу строки
            string[] text = File.ReadAllText(path).Split('\n'); 

            foreach (string s in text) { 
                //для каждой части текста проверяем наличие пробела в конце строки
                if (s.EndsWith(" ")) {
                    bits.Append('1'); //если он есть, то в bits добавляем '1'
                } else {
                    bits.Append('0'); //иначе добавляем '0'
                }
            }
            
            //вызываем функцию преобразования битов в строку и возвращаем полученный из неё результат как результат дешифрования
            return GetStringFromBits(bits.ToString()); 
        }
        
        //преобразование текста в последовательность битов
        private static string GetBits(string text) {
            StringBuilder result = new StringBuilder(); //здесь будет результат

            foreach (byte b in Encoding.UTF8.GetBytes(text)) { //используя кодировку ASCII преобразуем текст в байты
                //каждый байт переводим в биты (в двоичную систему счисления)
                //а затем в тип string и добавляем в результат
                string bits = Convert.ToString(b, 2);
                //если длина последовательности битов меньше 8, то в двоичном представлении есть незначащие нули (например, 00010110)
                //тогда принудительно их добавляем в начало, т.к. они могут обрезаться функций toString()
                while (bits.Length != 8) { 
                    //то добавляем их 
                    bits = "0" + bits;
                }
                result.Append(bits); 
            }

            return result.ToString(); //возвращаем результат
        }
        
        //преобразование битов в текст
        private static string GetStringFromBits(string bits) {
            List<byte> bytes = new List<byte>(); //здесь будут байты (байт - 8 бит)
            
            for (int i = 0; i + 8 < bits.Length; i += 8) { //рассматриваем каждые 8 бит
                //отделяем 8 бит от строки битов - bits.Substring(i, 8)
                //переводим её в байт (из двоичной системы счисления) и добавляем к списку байтов bytes
                bytes.Add(Convert.ToByte(bits.Substring(i, 8), 2)); 
            }
            
            //используя кодировку ASCII преобразуем байты в текст
            //toArray используется для того, чтобы List<byte> привести к типу массива bytes[], который необходим
            //функции GetString() для получения string значения данных
            return Encoding.UTF8.GetString(bytes.ToArray()); 
        }
        
        //пятое задание
        public static void Task5() {
            Console.WriteLine("Task 5 Bit cipher code:");
            Console.Write("Enter text to encode: ");
            string data = Console.ReadLine();
            
            //вызываем функцию шифрования, сразу отправляя туда биты из двоичного представления данных, которые нужно зашифровать
            BitCipherEncode2(LoremIpsumClassicPath, GetBits(data)); 
            string bitCipherDecoded = BitCipherDecode2(LoremIpsumClassicPath);
            Console.WriteLine("Decoded phrase - " + bitCipherDecoded + '\n');
        }
        
        //шифрование
        //после шифрования данные в файле перезаписываются вместе с зашифрованными данными
        public static void BitCipherEncode2(string path, string data) {
            //используя считанные из файла данные, заполняем StringBuilder, т.к. нам нужно будет производить
            //много вставок символов (пробелов) в текст для шифровки введённых данных
            StringBuilder text = new StringBuilder(File.ReadAllText(path));

            //предварительно форматируем текст, удаляя лишние пробелы, чтобы они не мешали шифрованию и дешифрованию
            for (int i = 0; i < text.Length; i++) { 
                if (text[i] == ' ' && text[i + 1] == ' ') { //если текущий и следующий символы являются пробелами
                    //то удаляем i-ый пробел и уменьшаем счётчик на 1, чтобы не пропустить ещё один возможный лишний пробел
                    text.Remove(i, 1);
                    i--;
                }
            }
            
            //проверяем, достаточно ли пробелов в тексте для шифровки введённых данных
            if (GetSpacesCount(text) < data.Length * 8) {
                Console.WriteLine("Text doesn't contain enough spaces."); //если нет, то выводим сообщение об этом
                return; //и выходим из функции, не выполняя остальной код
            }

            int k = 0; //счётчик для прохода по битам из data
            for (int i = 0; k < data.Length; i++) { //пока все биты шифруемых данных не просмотрены
                //если текущий символ в тексте является пробелом
                if (text[i] == ' ') {
                    if (data[k] == '1') { //если текущий бит равен единице
                        //то добавляем ещё один пробел и дополнительно увеличивем i, чтобы не рассматривать добавленный нами пробел
                        text.Insert(i, " ");
                        i++;
                        k++;
                    } else { //иначе бит равен 0 и пробел добавлять не нужно, переходим к следующему биту
                        k++;
                    }
                }
            }

            File.WriteAllText(path, text.ToString()); //заполняем файл по указанном пути path полученными данными result

            Console.WriteLine("Secret phrase encoded to file " + path); //выводим сообщение о том, что данные зашифрованы
        }

        //дешифрование
        //после дешифрования файл с зашифрованными данными не изменяется, сами зашифрованные данные возвращаются в виде строки типа string
        public static string BitCipherDecode2(string path) {
            StringBuilder bits = new StringBuilder(); //здесь будут биты полученные из текста с зашифрованными данными
            string text = File.ReadAllText(path); //считываем весь текст

            for (int i = 0; i + 1 < text.Length; i++) { //просматриваем текст из файла
                if (text[i] == ' ') { //если текущий символ является пробелом
                    if (text[i + 1] == ' ') { //и следующий тоже является пробелом
                        //то в список битов добавляем единицу и уделичиваем счётчик на 1, чтобы не рассматривать второй пробел
                        //иначе данные будут дешифрованы неправильно, т.к. при рассмотрении второго пробела появится лишний нуль
                        bits.Append('1');
                        i++;
                    } else { //иначе пробел только один, в список битов добавляем нуль
                        bits.Append('0');
                    }
                }
            }

            //вызываем функцию преобразования битов в строку и возвращаем полученный из неё результат как результат дешифрования
            return GetStringFromBits(bits.ToString()); 
        }
        
        //получение количества пробелом в тексте
        private static int GetSpacesCount(StringBuilder text) {
            int count = 0; //счётчик пробелом
            for (int i = 0; i < text.Length; i++) { 
                if (text[i] == ' ') { //если текущий символ является пробелом, то увеличиваем счётчик на 1
                    count++;
                }
            }

            return count; //возвращаем число пробелом
        }
        
        public static void Main(string[] args) {
            //#1
            Task1();
            //#2.1
            Task2_1();
            //#2.2
            Task2_2();
            //#3
            Task3();
            //#4
            Task4();
            //#5
            Task5();
        }

    }

}