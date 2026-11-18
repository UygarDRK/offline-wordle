//Uygar Dirik - https://uygardirik.com.tr
//Version 1.0 - 18.11.2026

using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.InteropServices;

namespace offline_wordle
{
    internal class Program
    {
        //Variables - START
        static string[] wordlist = ReadEmbeddedLines("offline_wordle.wordlists.valid-wordle-words.txt");
        static int tourCount = 5;
        static string[] customWordlist = null;
        //Variables - END
        static void Main(string[] args)
        {
            PrintCharByChar("Offline Wordle by Uygar Dirik.\n", 50);
            Task.Delay(275).Wait();
            Menu();
        }
        static string[] ReadEmbeddedLines(string resourceFilePath)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFilePath);
            using var reader = new StreamReader(stream);

            var list = new System.Collections.Generic.List<string>();
            while (!reader.EndOfStream) list.Add(reader.ReadLine());
            return list.ToArray();
        }

        static void PrintCharByChar(string a, int delayTime)
        {
            bool color = false;
            foreach(char achar in a.ToCharArray()) {
                if (color)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    color = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    color = true;
                }
                Console.Write(achar);
                Task.Delay(delayTime).Wait();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();            
        }

        static void Menu()
        {
            Console.Clear();
            Console.WriteLine("  1. Play Wordle with default wordlist. (Press P)");
            Task.Delay(100).Wait();
            Console.WriteLine("  2. Play Wordle with custom wordlist. (Press C)");
            Task.Delay(100).Wait();
            Console.WriteLine("  3. About the application. (Press A)");
            Task.Delay(100).Wait();
            Console.WriteLine("  4. Exit the application. (Press E)\n");
            Console.Write("Choose any option to continiue: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.KeyChar.ToString().ToLower() == "p")
            {
                Console.WriteLine("\n");
                Play(wordlist);
            }
            if (keyInfo.KeyChar.ToString().ToLower() == "c")
            {
                Console.WriteLine("Your wordlist should be like this: \"word1\nword2\nword3...\"");
                if (customWordlist == null)
                {
                    Console.Write("Please provide the path to the wordlist: ");
                    string path = Console.ReadLine();
                    customWordlist = File.ReadAllLines(path);
                }
                Play(customWordlist);
            }
            if (keyInfo.KeyChar.ToString().ToLower() == "a")
            {
                Console.Clear();
                Console.WriteLine("project offline-wordle, made by Uygar Dirik.\nversion 1.0, build date 18.11.2026.\nCheck out my stuff at https://uygardirik.com.tr/");
                Console.Write("Press any key to continiue..."); Console.ReadKey();
                Menu();
            }
            if (keyInfo.KeyChar.ToString().ToLower() == "e")
            {
                Console.Clear();
                PrintCharByChar("Goodbye dear player!", 30);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("\nPlease select a valid option.");
                Console.Write("Press any key to continiue..."); Console.ReadKey();
                Menu();
            }
        }
        
        static void Play(string[] list)
        {
            Random rand = new Random();
            int a = rand.Next(0, list.Length - 1);
            string word = list[a];
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your word consists of " + word.Length + " characters.");
            Console.ForegroundColor = ConsoleColor.White;
            int count = 0;
            bool won = false;
            while (count < tourCount)
            {
                Console.Write("Please write a word: ");
                string written = Console.ReadLine();
                if (written.Length != word.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("-The word you have just written contains different count of characters than the word you are trying to guess.");
                    Console.WriteLine("-Discarding this word.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if(!list.Contains(written))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("-The word you have just written is not an actual word.");
                    Console.WriteLine("-Discarding this word.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\n-You have " + (tourCount - count - 1).ToString() + " guesses left.");
                    Console.ForegroundColor = ConsoleColor.White;
                    count++;
                    char[] charArrayWritten = written.ToCharArray();
                    char[] charArrayOriginal = word.ToCharArray();
                    char[] charArrayNumbers = new string('3', word.Length).ToCharArray();
                    int index = 0;
                    int guessedRight = 0;
                    foreach (var c in charArrayWritten)
                    {
                        if (charArrayWritten[index] == charArrayOriginal[index])
                        {
                            guessedRight++;
                            charArrayNumbers[index] = '1';
                            charArrayOriginal[index] = '/';
                        }
                        index++;
                    }
                    int index2 = 0;
                    foreach (var c in charArrayWritten)
                    {
                        if (charArrayNumbers[index2] != '1' && charArrayOriginal.Contains(charArrayWritten[index2]))
                        {
                            int index_alt = 0;
                            foreach (var c2 in charArrayOriginal)
                            {
                                if (c2 == charArrayWritten[index2])
                                    break;
                                index_alt++;
                            }
                            charArrayNumbers[index2] = '2';      // tahminin konumu
                            charArrayOriginal[index_alt] = '/';  // havuzdaki konum
                        }
                        index2++;
                    }
                    int index3 = 0;
                    foreach (var c in charArrayWritten)
                    {
                        if (charArrayWritten[index3] != charArrayOriginal[index3]&&charArrayOriginal[index3] is not '/')
                        {
                            charArrayNumbers[index3] = '3';
                            charArrayOriginal[index3] = '/';
                        }
                        index3++;
                    }
                    if(guessedRight == word.Length)
                    {
                        won = true;
                        break;
                    }
                    int index4 = 0;

                    //Render section
                    foreach (var c in charArrayNumbers)
                    {
                        if (c == '1')
                        {
                            //True character
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(charArrayWritten[index4]);
                        }
                        else if (c == '2')
                        {
                            //True but not there character
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(charArrayWritten[index4]);
                        }
                        else if (c == '3')
                        {
                            //False character
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(charArrayWritten[index4]);
                        }
                        index4++;
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
            }
            if (won)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("-Congratulations! You've won the game!");
                Console.ForegroundColor = ConsoleColor.White;
                Task.Delay(500).Wait();
                Console.Write("Press any key to continue.");
                Console.ReadKey();
                Menu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("You've lost the game. Good luck next time!");
                Console.ForegroundColor = ConsoleColor.White;
                Task.Delay(500).Wait();
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
                Menu();
            }
        }
    }
}
