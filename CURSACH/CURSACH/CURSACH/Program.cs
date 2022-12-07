using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        public static bool exp,symb = false;
        public static char[] Hex = new char[] {'A','B','C','D','E','F','a','b','c','d','e','f'};
        public static bool IsKeyWord(string word, string[] words)
        {
            foreach (var s in words)
            {
                if (word == s)
                {
                    return true;
                }
            }
            return false;
        }
        public enum ss
        {
            BIN,
            HEX,
            DEC,
            OCT
        }
        static void Main(string[] args)
        {
            Console.Clear();
            string word = "";
            int cur_pos = 0;
            string buf_word = "";
            string buf_num = "";
            string[,] lexems = new string[100, 3];
            string[] key_word = new string[100];
            string[] world = new string[100];
            int cur_lexem = 0;
            string CurCond = "H";
            Console.WriteLine("Введите номер команды\n1.Ввод строки вручную\n2.Ввод строки из файла\n3.Изменить файл с кодом\n4.Изменить файл с кодовыми словами\n5.Выход");
            int g = Convert.ToInt32(Console.ReadLine());
            switch (g)
            {
                case 1:
                    Console.WriteLine("Введите строку");
                    word = Convert.ToString(Console.ReadLine());
                    break;
                case 2:
                    word = File.ReadAllText("code.txt");
                    Console.WriteLine("Считывание строки из файла.\nДля продолжения нажмите Enter");
                    Console.ReadLine();
                    break;
                case 3:
                    Process.Start("notepad.exe", "code.txt");
                    Main(world);
                    break;
                case 4:
                    Process.Start("notepad.exe", "KeyWords.txt");
                    Main(world);
                    break;
                case 5:
                    Console.WriteLine("Вы уверены, что хотите выйти? Введите y/n");
                    string k = Console.ReadLine().ToLower();
                    char j = Convert.ToChar(k);
                    if (j == 'y')
                    {
                        Environment.Exit(0);
                    }
                    else if (j == 'n')
                    {
                        Main(world);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка во вводе");
                        Console.ReadLine();
                        Main(world);
                    }

                    break;
                default:
                    Console.WriteLine("Введен неверный номер команды.\nДля продолжение нажмите Enter");
                    Console.ReadLine();
                    Environment.Exit(2);
                    break;
            }
            char[] word_char = new char[word.Length];
            word_char = word.ToCharArray();
            key_word = File.ReadAllLines("KeyWords.txt");
            Console.WriteLine("Текущий символ\tТекущее состояние");
            while (cur_pos < word_char.Length)
            {
                switch (CurCond)
                {
                    case "H":
                        Console.Write(word_char[cur_pos] + "\t\t");
                        if (word_char[cur_pos] == '\t' || word_char[cur_pos] == '\n' || word_char[cur_pos] == ' ' || word_char[cur_pos] == '\r')
                        {
                            cur_pos++;
                        }
                        else if (word_char[cur_pos] == 'e')
                        {
                            CurCond = "FIN";
                        }
                        else if (word_char[cur_pos] == '<' || word_char[cur_pos] == '/' || word_char[cur_pos] == '>' || word_char[cur_pos] == '!' || word_char[cur_pos] == ':' || word_char[cur_pos] == '=' || word_char[cur_pos] == '-' || word_char[cur_pos] == '[' || word_char[cur_pos] == '{' || word_char[cur_pos] == '}' || word_char[cur_pos] == ')' || word_char[cur_pos] == ',' || word_char[cur_pos] == '*' || word_char[cur_pos] == ']' || word_char[cur_pos] == '%' || word_char[cur_pos] == '$' || word_char[cur_pos] == '(' || word_char[cur_pos] == '+')
                        {
                            CurCond = "DLM";
                        }
                        else if (Char.IsLetter(word_char[cur_pos]))
                        {
                            buf_word = "";
                            CurCond = "ID";
                        }
                        else if (Char.IsDigit(word_char[cur_pos]))
                        {
                            buf_num = "";
                            CurCond = "NM";
                        }
                        else
                        {
                            CurCond = "DLM";
                        }
                        Console.WriteLine(CurCond);
                        break;
                    case "FIN":
                        if(word_char[cur_pos+1] == 'n'&& word_char[cur_pos+2] == 'd'&& word_char.Length==cur_pos + 3)
                        {
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = "end";
                            lexems[cur_lexem, 2] = "Конец";
                            cur_lexem++;
                            cur_pos = cur_pos + 3;
                            CurCond = "H";
                        }
                        else
                        {
                            throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                        }
                        break;
                    case "ID":
                        Console.Write(word_char[cur_pos] + "\t\t");
                        if (word_char[cur_pos] == '_' || Char.IsDigit(word_char[cur_pos]) || Char.IsLetter(word_char[cur_pos]))
                        {
                            buf_word += word_char[cur_pos];
                            if (IsKeyWord(buf_word, key_word))
                            {
                                cur_pos++;
                                CurCond = "ID";
                                lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                                lexems[cur_lexem, 1] = buf_word;
                                lexems[cur_lexem, 2] = "Ключевое слово";
                            }
                            else
                            {
                                lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                                lexems[cur_lexem, 1] = buf_word;
                                lexems[cur_lexem, 2] = "Идентификатор";
                                cur_pos++;
                                CurCond = "ID";
                            }
                        }
                        else
                        {
                            cur_lexem++;
                            CurCond = "H";
                        }
                        Console.WriteLine(CurCond);
                        break;
                    case "NM":
                        Console.Write(word_char[cur_pos] + "\t\t");
                        if(word_char[cur_pos]=='0'|| word_char[cur_pos] == '1')
                        {
                            CurCond = "BIN";
                        }
                        else if(Convert.ToInt32(Convert.ToString(word_char[cur_pos]))>1&& Convert.ToInt32(Convert.ToString(word_char[cur_pos])) <8)
                        {
                            CurCond = "OCT";
                        }
                        else if(Convert.ToInt32(word_char[cur_pos]) > 7 ||IsHex(word_char[cur_pos]))
                        {
                            CurCond = "DECHEX";
                        }
                        else
                        {
                            throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                        }
                        Console.WriteLine(CurCond);
                        break;
                    case "BIN":
                            buf_num += word_char[cur_pos];
                        if (Convert.ToInt32(Convert.ToString(word_char[cur_pos]))>1&& Convert.ToInt32(Convert.ToString(word_char[cur_pos])) <8)
                        {
                            CurCond = "OCT";
                        }
                        else if (word_char[cur_pos] == 'B' || word_char[cur_pos] == 'b')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = GetNum(buf_num, ss.BIN);
                            lexems[cur_lexem, 2] = "Число";
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == 'e' || word_char[cur_pos] == 'E' || word_char[cur_pos] == '.')
                        {
                            cur_pos--;
                            buf_num=buf_num.Remove(buf_num.Length - 1);
                            CurCond = "EXP";
                        }
                        else if(word_char[cur_pos]=='0'|| word_char[cur_pos]=='1')
                        {
                            CurCond = "BIN";
                        }
                        else if (Convert.ToInt32(word_char[cur_pos]) > 7 || IsHex(word_char[cur_pos]))
                        {
                            CurCond = "DECHEX";
                        }
                        else if (Char.IsDigit(word_char[cur_pos]))
                        {

                        }
                        else
                        {
                            throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                        }
                        cur_pos++;
                        break;
                    case "OCT":
                        
                            buf_num += word_char[cur_pos];
                        
                        if (word_char[cur_pos] == 'O' || word_char[cur_pos] == 'o')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = GetNum(buf_num, ss.OCT);
                            lexems[cur_lexem, 2] = "Число";
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == 'e' || word_char[cur_pos] == 'E' || word_char[cur_pos] == '.')
                        {
                            cur_pos--;
                            buf_num = buf_num.Remove(buf_num.Length - 1);
                            CurCond = "EXP";
                        }
                        else if (Convert.ToInt32(word_char[cur_pos]) > 1 && Convert.ToInt32(word_char[cur_pos]) < 8)
                        {
                            CurCond = "OCT";
                        }
                        else if (Convert.ToInt32(word_char[cur_pos]) > 7 || IsHex(word_char[cur_pos]))
                        {
                            CurCond = "DECHEX";
                        }
                        else if(Char.IsDigit(word_char[cur_pos]))
                        {

                        }
                        else
                        {
                            throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                        }
                        cur_pos++;
                        break;
                    case "EXP":
                        if (word_char[cur_pos] == 'e' || word_char[cur_pos] == 'E')
                        {
                            buf_num += word_char[cur_pos];
                            exp = true;
                        }
                        else if (word_char[cur_pos] == '.' && exp == true)
                        {
                            throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                        }
                        else if (word_char[cur_pos] == '.' && exp == false)
                        {
                            buf_num += word_char[cur_pos];
                        }
                        else if (Char.IsDigit(word_char[cur_pos]))
                        {
                            buf_num += word_char[cur_pos];
                        }
                        else if ((word_char[cur_pos] == '+' || word_char[cur_pos - 1] == '-') && exp == false)
                        {
                            throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                        }
                        else if ((word_char[cur_pos] == '+' || word_char[cur_pos] == '-'))
                        {
                            if (!symb)
                            {
                                buf_num += word_char[cur_pos];
                                symb = true;
                            }
                            else
                            {
                                lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                                lexems[cur_lexem, 1] = buf_num;
                                lexems[cur_lexem, 2] = "Число";
                                cur_lexem++;
                                CurCond = "H";
                                cur_pos--;
                            }
                        }
                        else if (word_char[cur_pos] == '\n' || word_char[cur_pos] == ':')
                        {
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = buf_num.Remove(buf_num.Length - 1);
                            lexems[cur_lexem, 2] = "Число";
                            cur_lexem++;
                            CurCond = "H";
                        }
                        else
                        {
                            buf_num += word_char[cur_pos];
                        }
                        cur_pos++;
                        break;
                    case "DECHEX":
                        
                            buf_num += word_char[cur_pos];
                         if (word_char[cur_pos] == 'H' || word_char[cur_pos] == 'h')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = GetNum(buf_num, ss.HEX);
                            lexems[cur_lexem, 2] = "Число";
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == 'd' || word_char[cur_pos] == 'D')
                        {
                            if(word_char[cur_pos] == 'd' && word_char[cur_pos+1] == 'i')
                            {
                                throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                            }
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = buf_num.Remove(buf_num.Length-1);
                            lexems[cur_lexem, 2] = "Число";
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == 'e' || word_char[cur_pos] == 'E' || word_char[cur_pos] == '.')
                        {
                            cur_pos--;
                            buf_num = buf_num.Remove(buf_num.Length - 1);
                            CurCond = "EXP";
                        }
                        else if (Char.IsDigit(word_char[cur_pos]))
                        {

                        }
                        else
                        {
                            throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                        }
                        cur_pos++;
                        break;
                    case "DLM":
                        bool sost = false;
                        Console.Write(word_char[cur_pos] + "\t\t");
                        if (word_char[cur_pos] == '(' || word_char[cur_pos] == ')' || word_char[cur_pos] == ':' || word_char[cur_pos] == '[' || word_char[cur_pos] == ',' || word_char[cur_pos] == ']')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = Convert.ToString(word_char[cur_pos]);
                            lexems[cur_lexem, 2] = "Разделитель";
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if(word_char[cur_pos] == '$' || word_char[cur_pos] == '!' || word_char[cur_pos] == '%')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = Convert.ToString(word_char[cur_pos]);
                            lexems[cur_lexem, 2] = "тип переменной";
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '+' || word_char[cur_pos] == '-' || word_char[cur_pos] == '*' || word_char[cur_pos] == '/')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = Convert.ToString(word_char[cur_pos]);
                            lexems[cur_lexem, 2] = "Операция";
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '<' || word_char[cur_pos] == '>')
                        {
                            if (sost == true)
                            {
                                if (word_char[cur_pos - 1] == '<' && word_char[cur_pos] == '>')
                                {
                                    CurCond = "H";
                                    lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                                    lexems[cur_lexem, 1] = Convert.ToString(word_char[cur_pos-1])+ Convert.ToString(word_char[cur_pos]);
                                    lexems[cur_lexem, 2] = "Операция отрицания";
                                    cur_pos++;
                                    cur_lexem++;
                                }
                                else if(word_char[cur_pos - 1] == '>' && word_char[cur_pos] == '<')
                                {
                                    throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                                }
                            }
                            else
                            {
                                sost = true;
                                cur_pos++;
                            }
                        }
                        else if(word_char[cur_pos] == '=')
                        {
                            if (sost == true)
                            {
                                CurCond = "H";
                                lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                                lexems[cur_lexem, 1] = Convert.ToString(word_char[cur_pos - 1]) + Convert.ToString(word_char[cur_pos]);
                                lexems[cur_lexem, 2] = "Операция";
                                cur_pos++;
                                cur_lexem++;
                                sost = false;
                            }
                            else
                            {
                                CurCond = "H";
                                lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                                lexems[cur_lexem, 1] = Convert.ToString(word_char[cur_pos]);
                                lexems[cur_lexem, 2] = "Операция";
                                cur_pos++;
                                cur_lexem++;
                            }
                        }
                        else if(word_char[cur_pos] == '{')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = Convert.ToString(word_char[cur_pos]);
                            lexems[cur_lexem, 2] = "Начало комментария";
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '{')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = Convert.ToString(cur_lexem + 1);
                            lexems[cur_lexem, 1] = Convert.ToString(word_char[cur_pos]);
                            lexems[cur_lexem, 2] = "Конец комментария";
                            cur_pos++;
                            cur_lexem++;
                        }
                        else
                        {
                            throw new Exception("error in position " + cur_pos + " error symbol '" + word_char[cur_pos] + "' after symbol '" + word_char[cur_pos - 1] + "'");
                        }
                        Console.WriteLine(CurCond);
                        break;
                }
            }
            Console.WriteLine("Таблица лексем");
            for (int i = 0; i < cur_lexem; i++)
            {
                Console.WriteLine(lexems[i, 0] + "\t" + lexems[i, 1] + "\t" + lexems[i, 2]);
            }

            Console.ReadLine();
            Main(world);
        }

        private static bool IsHex(char v)
        {
            foreach (char l in Hex)
            {
                if(l==v)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private static string GetNum(string buf_num, ss hEX)
        {
            if(buf_num[buf_num.Length-1]=='o'|| buf_num[buf_num.Length - 1] == 'O' || buf_num[buf_num.Length - 1] == 'h' || buf_num[buf_num.Length - 1] == 'H' || buf_num[buf_num.Length - 1] == 'b' || buf_num[buf_num.Length - 1] == 'B')
            {
                buf_num = buf_num.Remove(buf_num.Length-1);
            }
            if (hEX==ss.HEX)
            {
                return Convert.ToString(Convert.ToInt32(buf_num,16));
            }
            else if(hEX==ss.BIN)
            {
                return Convert.ToString(Convert.ToInt32(buf_num, 2));
            }
            else if(hEX==ss.OCT)
            {
                return Convert.ToString(Convert.ToInt32(buf_num, 8));
            }
            return "0";
        }
    }
}
