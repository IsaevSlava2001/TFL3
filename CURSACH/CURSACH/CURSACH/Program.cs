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
        public static string[] delimeters = new string[100];
        public static string[] key_word = new string[100];
        public static string[] identificators = new string[100];
        public static int cur_ID=1, cur_num = 1;
        public static string[] numbers = new string[100];
        public static bool exp,symb,isLetter = false;
        public static bool sost = false;
        public static string word = "";
        public static int cur_pos = 0;
        public static char[] word_char = new char[word.Length];
        public static char[] Hex = new char[] {'A','B','C','D','E','F','a','b','c','d','e','f'};
        public static int[] GetLexem(string word)
        {
            int cur = 1;
            foreach (var s in key_word)
            {
                if (word == s)
                {
                    return new int[]{1,cur};
                }
                cur++;
            }
            cur = 1;
            foreach(var s in delimeters)
            {
                if (word == s)
                {
                    return new int[] { 2, cur };
                }
                cur++;
            }
            return new int[]{0,0 };
        }
        public enum ss
        {
            BIN,
            HEX,
            DEC,
            OCT
        }
        public static void errorLex(int num)
        {
            string str = "Ошибка" + num + ":";
            switch (num)
            {
                case 101:
                    str += "Ожидалось число.";
                    break;
                case 102:
                    str += "Ожидалось fin.";
                    break;
                case 103:
                    str += "Ожидалось число или e или E или. или b или B.";
                    break;
                case 104:
                    str += "Ожидалось число или e или E или. или o или O.";
                    break;
                case 105:
                    str += "Ожидалось отсутствие точки после e.";
                    break;
                case 106:
                    str += "Ожидалось e или E.";
                    break;
                case 107:
                    str += "Ожидалось отсутствие i после d.";
                    break;
                case 108:
                    str += "Ожидалось число или e или E или. или d или D или h или H.";
                    break;
                case 109:
                    str += "Ожидалось отсутствие < после >.";
                    break;
                case 110:
                    str += "Ожидался разделитель.";
                    break;
                case 111:
                    str += "Ожидалась буква для описания сс.";
                    break;
                case 112:
                    str += "Ожидалось продолжение действительного числа.";
                    break;
                case 113:
                    str += "Ожидалось продолжение действительного числа.";
                    break;
            }
            try
            {
                if (word_char[cur_pos] == '\n' || word_char[cur_pos] == '\r')
                {
                    str += " Встречен перенос строки на позиции " + (cur_pos - 2);
                }
                else
                {
                    str += " Встречен " + word_char[cur_pos] + " на позиции " + (cur_pos - 2);
                }
            }
            catch (Exception ex)
            {
                str += " Встречен конец файла";
            }
            Console.WriteLine(str);
            Console.ReadLine();
            Environment.Exit(num);
        }
        public static void Lexer()
        {
            bool isId = true;
            string buf_word = "";
            string buf_num = "";
            string[,] lexems = new string[100, 2];
            string[] key_word_read = new string[100];
            string[] delimeters_read = new string[100];
            int cur_lexem = 0;
            string CurCond = "H";
            word_char = word.ToCharArray();
            key_word_read = File.ReadAllLines("KeyWords.txt");
            for (int i = 0; i < key_word_read.Length; i++)
            {
                key_word[i] = key_word_read[i].Split(' ')[0];
            }
            delimeters_read = File.ReadAllLines("Delimeters.txt");
            for (int i = 0; i < delimeters_read.Length; i++)
            {
                delimeters[i] = delimeters_read[i].Split(' ')[0];
            }
            while (cur_pos < word_char.Length)
            {
                switch (CurCond)
                {
                    case "H":
                        if (word_char[cur_pos] == '\t' || word_char[cur_pos] == ' ' || word_char[cur_pos] == '\r')
                        {
                            cur_pos++;
                        }
                        else if (word_char[cur_pos] == 'e')
                        {
                            CurCond = "FIN";
                        }
                        else if (word_char[cur_pos] == '\n')
                        {
                            CurCond = "NewLine";
                        }
                        else if (word_char[cur_pos] == '<' || word_char[cur_pos] == '/' || word_char[cur_pos] == '>' || word_char[cur_pos] == '!' || word_char[cur_pos] == ':' || word_char[cur_pos] == '=' || word_char[cur_pos] == '-' || word_char[cur_pos] == '[' || word_char[cur_pos] == '{' || word_char[cur_pos] == '}' || word_char[cur_pos] == ')' || word_char[cur_pos] == ',' || word_char[cur_pos] == '*' || word_char[cur_pos] == ']' || word_char[cur_pos] == '%' || word_char[cur_pos] == '$' || word_char[cur_pos] == '(' || word_char[cur_pos] == '+')
                        {
                            sost = false;
                            CurCond = "DLM";
                        }
                        else if (Char.IsLetter(word_char[cur_pos]))
                        {
                            buf_word = "";
                            CurCond = "ID";
                        }
                        else if (Char.IsDigit(word_char[cur_pos]))
                        {
                            isLetter = false;
                            buf_num = "";
                            CurCond = "NM";
                        }
                        else
                        {
                            sost = false;
                            CurCond = "DLM";
                        }
                        break;
                    case "NewLine":
                        lexems[cur_lexem, 0] = 2.ToString();
                        lexems[cur_lexem, 1] = 22.ToString();
                        cur_lexem++;
                        cur_pos++;
                        CurCond = "H";
                        break;
                    case "FIN":
                        if (word_char[cur_pos + 1] == 'n' && word_char[cur_pos + 2] == 'd' && word_char.Length == cur_pos + 3)
                        {
                            lexems[cur_lexem, 0] = 1.ToString();
                            lexems[cur_lexem, 1] = 7.ToString();
                            cur_lexem++;
                            cur_pos = cur_pos + 3;
                            CurCond = "H";
                        }
                        else
                        {
                            errorLex(102); 
                        }
                        break;
                    case "ID":
                        if (Char.IsDigit(word_char[cur_pos]) || Char.IsLetter(word_char[cur_pos]))
                        {
                            buf_word += word_char[cur_pos];
                            if (GetLexem(buf_word)[0] != 0)
                            {
                                cur_pos++;
                                CurCond = "ID";
                                lexems[cur_lexem, 0] = GetLexem(buf_word)[0].ToString();
                                lexems[cur_lexem, 1] = GetLexem(buf_word)[1].ToString();
                                isId = true;
                            }
                            else
                            {
                                isId = false;
                                lexems[cur_lexem, 0] = 3.ToString();
                                lexems[cur_lexem, 1] = cur_ID.ToString();
                                cur_pos++;
                                CurCond = "ID";
                            }
                        }
                        else
                        {
                            if(!isId)
                            {
                                identificators[cur_ID] = buf_word;
                                cur_ID++;
                            }
                            cur_lexem++;
                            CurCond = "H";
                        }
                        break;
                    case "NM":
                        if (word_char[cur_pos] == '0' || word_char[cur_pos] == '1')
                        {
                            CurCond = "BIN";
                        }
                        else if (Convert.ToInt32(Convert.ToString(word_char[cur_pos])) > 1 && Convert.ToInt32(Convert.ToString(word_char[cur_pos])) < 8)
                        {
                            CurCond = "OCT";
                        }
                        else if (Convert.ToInt32(word_char[cur_pos]) > 7 || IsHex(word_char[cur_pos]))
                        {
                            CurCond = "DECHEX";
                        }
                        else
                        {
                            errorLex(101);
                        }
                        break;
                    case "BIN":
                        buf_num += word_char[cur_pos];
                        if (Convert.ToInt32(Convert.ToString(word_char[cur_pos])) > 1 && Convert.ToInt32(Convert.ToString(word_char[cur_pos])) < 8)
                        {
                            CurCond = "OCT";
                        }
                        else if (word_char[cur_pos] == 'B' || word_char[cur_pos] == 'b')
                        {
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = cur_num.ToString(); ;
                            numbers[cur_num] = GetNum(buf_num, ss.BIN);
                            cur_lexem++;
                            cur_num++;
                        }
                        else if (word_char[cur_pos] == 'e' || word_char[cur_pos] == 'E' || word_char[cur_pos] == '.')
                        {
                            cur_pos--;
                            buf_num = buf_num.Remove(buf_num.Length - 1);
                            CurCond = "EXP";
                        }
                        else if (word_char[cur_pos] == '0' || word_char[cur_pos] == '1')
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
                            errorLex(103);
                        }
                        cur_pos++;
                        break;
                    case "OCT":
                        buf_num += word_char[cur_pos];
                        if (word_char[cur_pos] == 'O' || word_char[cur_pos] == 'o')
                        {
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = cur_num.ToString(); ;
                            numbers[cur_num] = GetNum(buf_num, ss.OCT);
                            cur_lexem++;
                            cur_num++;
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
                        else if (Char.IsDigit(word_char[cur_pos]))
                        {

                        }
                        else
                        {
                            errorLex(104);
                        }
                        cur_pos++;
                        break;
                    case "EXP":
                        if(word_char[cur_pos] == 'd' || word_char[cur_pos] == 'D'|| word_char[cur_pos] == 'h' || word_char[cur_pos] == 'H'|| word_char[cur_pos] == 'o' || word_char[cur_pos] == 'O'|| word_char[cur_pos] == 'b' || word_char[cur_pos] == 'B')
                        {
                            errorLex(112);
                        }
                        if (word_char[cur_pos] == 'e' || word_char[cur_pos] == 'E')
                        {
                            isLetter = true;
                            buf_num += word_char[cur_pos];
                            exp = true;
                        }
                        else if (word_char[cur_pos] == '.' && exp == true)
                        {
                            errorLex(105);
                        }
                        else if (word_char[cur_pos] == '.' && exp == false)
                        {
                            buf_num += word_char[cur_pos];
                        }
                        else if (Char.IsDigit(word_char[cur_pos]))
                        {
                            buf_num += word_char[cur_pos];
                        }
                        else if ((word_char[cur_pos] == '+' || word_char[cur_pos] == '-') && exp == false)
                        {
                            errorLex(106);
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
                                lexems[cur_lexem, 0] = 4.ToString();
                                lexems[cur_lexem, 1] = cur_num.ToString();
                                numbers[cur_num] = buf_num;
                                cur_lexem++;
                                cur_num++;
                                CurCond = "H";
                                cur_pos--;
                            }
                        }
                        else if (word_char[cur_pos] == '\n' || word_char[cur_pos] == ':')
                        {
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = cur_num.ToString();
                            numbers[cur_num] = buf_num.Remove(buf_num.Length - 1);
                            cur_lexem++;
                            cur_num++;
                            if (word_char[cur_pos] == '\n')
                            {
                                lexems[cur_lexem, 0] = 2.ToString();
                                lexems[cur_lexem, 1] = 22.ToString();
                                cur_lexem++;
                            }
                            CurCond = "H";
                        }
                        else if(Char.IsLetter(word_char[cur_pos])&& (word_char[cur_pos]!='e'|| word_char[cur_pos]!='E'))
                        {
                            errorLex(113);
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
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = cur_num.ToString(); ;
                            numbers[cur_num] = GetNum(buf_num, ss.HEX);
                            cur_lexem++;
                            cur_num++;
                        }
                        else if (word_char[cur_pos] == 'd' || word_char[cur_pos] == 'D')
                        {
                            if (word_char[cur_pos] == 'd' && word_char[cur_pos + 1] == 'i')
                            {
                                errorLex(107);
                            }
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = cur_num.ToString();
                            numbers[cur_num] = buf_num.Remove(buf_num.Length - 1);
                            cur_lexem++;
                            cur_num++;
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
                            errorLex(108);
                        }
                        cur_pos++;
                        break;
                    case "DLM":
                        if (word_char[cur_pos] == '(' || word_char[cur_pos] == ')' || word_char[cur_pos] == ':' || word_char[cur_pos] == '[' || word_char[cur_pos] == ',' || word_char[cur_pos] == ']')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '$' || word_char[cur_pos] == '!' || word_char[cur_pos] == '%')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '+' || word_char[cur_pos] == '-' || word_char[cur_pos] == '*' || word_char[cur_pos] == '/')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
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
                                    lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos - 1] + "" + word_char[cur_pos])[0].ToString();
                                    lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                                    cur_pos++;
                                    cur_lexem++;
                                }
                                else if (word_char[cur_pos - 1] == '>' && word_char[cur_pos] == '<')
                                {
                                    errorLex(109);
                                }
                            }
                            else
                            {
                                sost = true;
                                cur_pos++;
                            }
                        }
                        else if (word_char[cur_pos] == '=')
                        {
                            if (sost == true)
                            {
                                CurCond = "H";
                                lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos - 1] + "" + word_char[cur_pos])[0].ToString();
                                lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                                cur_pos++;
                                cur_lexem++;
                                sost = false;
                            }
                            else
                            {
                                CurCond = "H";
                                lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                                lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                                cur_pos++;
                                cur_lexem++;
                            }
                        }
                        else if (word_char[cur_pos] == '{')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '}')
                        {
                            CurCond = "H";
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if(Char.IsDigit(word_char[cur_pos])||Char.IsLetter(word_char[cur_pos]))
                        {
                            if(sost==true)
                            {
                                lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos - 1].ToString())[0].ToString();
                                lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos - 1].ToString())[1].ToString();
                                cur_lexem++;
                            }
                            CurCond = "H";
                        }
                        else
                        {
                            errorLex(110);
                        }
                        break;
                }
            }
            if(isLetter==false)
            {
                errorLex(111);
            }
            for (int i = 0; i < cur_lexem; i++)
            {
                string str = "{ " + lexems[i, 0] + ", " + lexems[i, 1] + " }\n";
                File.AppendAllText("lexems.txt", str);
            }
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] != null)
                {
                    string str = numbers[i] + " " + i + "\n";
                    File.AppendAllText("Numbers.txt", str);
                }
            }
            for (int i = 0; i < identificators.Length; i++)
            {
                if (identificators[i] != null)
                {
                    string str = identificators[i] + " " + i + "\n";
                    File.AppendAllText("Identificators.txt", str);
                }
            }
        }
        public static void menu()
        {
            Console.Clear();
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
                    Lexer();
                    Console.WriteLine("Лексический анализ проведен успешно. Сформирован файл лексем lexems.txt");
                    Console.ReadLine();
                    break;
                case 3:
                    Process.Start("notepad.exe", "code.txt");
                    menu();
                    break;
                case 4:
                    Process.Start("notepad.exe", "KeyWords.txt");
                    menu();
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
                        menu();
                    }
                    else
                    {
                        Console.WriteLine("Ошибка во вводе");
                        Console.ReadLine();
                        menu();
                    }

                    break;
                default:
                    Console.WriteLine("Введен неверный номер команды.\nДля продолжение нажмите Enter");
                    Console.ReadLine();
                    Environment.Exit(2);
                    break;
            }
            menu();
        }
        static void Main(string[] args)
        {
            Console.Clear();
            File.WriteAllText("Numbers.txt",string.Empty);
            File.WriteAllText("Identificators.txt", string.Empty);
            File.WriteAllText("lexems.txt", string.Empty);
            menu();
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
