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
        public static void ResizeArray<T>(ref T[,] original, int newCoNum, int newRoNum)
        {
            var newArray = new T[newCoNum, newRoNum];
            int columnCount = original.GetLength(1);
            int columnCount2 = newRoNum;
            int columns = original.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
            original = newArray;
        }
        public static string[] delimeters;
        public static string[] key_word;
        public static string[] identificators = new string[0];
        public static int cur_ID=0, cur_num = 0;
        public static string[] numbers = new string[0];
        public static bool exp,symb,isLetter = false;
        public static bool sost = false;
        public static string word = "";
        public static int cur_pos = 0;
        public static char[] word_char = new char[word.Length];
        public static char[] Hex = new char[] {'A','B','C','D','E','F','a','b','c','d','e','f'};
        public static string cur_lex_lexem_word;
        public static int cur_lexem_number;
        public static int cur_lex_lexem = 0;
        public static string[] declared_identificators=new string[0];
        public static string[] num;
        public static string[] val;
        public static void ErrorParser(int error)
        {
            if(error==201)
            {
                Console.WriteLine("Неопознаная лексема на позиции "+cur_lex_lexem);
                Console.ReadLine();
                Environment.Exit(error);
            }
            Console.Write("Ошибка "+error+": ");
            switch(error)
            {
                case 201:
                    Console.Write(" Ожидался оператор группы умножения");
                    break;
                case 202:
                    Console.Write(" Ожидался оператор или описание");
                    break;
                case 203:
                    Console.Write(" Ожидалось : или перенос строки");
                    break;
                case 204:
                    Console.Write(" Ожидалось end");
                    break;
                case 205:
                    Console.Write(" Ожидался идентификатор");
                    break;
                case 206:
                    Console.Write(" Ожидался dim");
                    break;
                case 207:
                    Console.Write(" Ожидался тип переменной");
                    break;
                case 208:
                    Console.Write(" Ожидался ]");
                    break;
                case 209:
                    Console.Write(" Ожидался as");
                    break;
                case 210:
                    Console.Write(" Ожидался then");
                    break;
                case 211:
                    Console.Write(" Ожидался to");
                    break;
                case 212:
                    Console.Write(" Ожидалось выражение");
                    break;
                case 213:
                    Console.Write(" Ожидался do");
                    break;
                case 214:
                    Console.Write(" Ожидался оператор");
                    break;
                case 215:
                    Console.Write(" Ожидалась (");
                    break;
                case 216:
                    Console.Write(" Ожидалась операция группы сложения");
                    break;
                case 217:
                    Console.Write(" Ожидалась )");
                    break;
                case 218:
                    Console.Write(" Неопознанная ошибка");
                    break;
                case 219:
                    Console.Write(" Ожидалась операция группы отношения");
                    break;
                case 220:
                    Console.Write(" Неопознанная ошибка");
                    break;
                case 221:
                    Console.Write("Ожидалась унарная операция");
                    break;
                case 301:
                    Console.Write(" Повторное объявление переменной");
                    break;
                case 302:
                    Console.Write(" Использование необъявленной переменной");
                    break;
                default:
                    Console.Write(" Неопознанная ошибка");
                    break;
            }
            Console.Write(". Встречен " + cur_lex_lexem_word + " номер лексемы " + cur_lexem_number);
            Console.ReadLine();
            Environment.Exit(error);
        }
        public static void Parser()
        {
            Fill();
            prog();     
        }
        public static void Fill()
        {
            string[] cur_lex = File.ReadAllLines("lexems.txt");
            num=new string[cur_lex.Length];
            val = new string[cur_lex.Length]; ;
            for (int i = 0; i < cur_lex.Length; i++)
            {
               num[i] = cur_lex[i].Split(' ')[0];
               val[i] = cur_lex[i].Split(' ')[1];
            }
        }
        public static void get_lexem()
        {
            int counter = 1;
            try
            {
                switch (num[cur_lex_lexem])
                {
                    case "1":
                        foreach (var s in key_word)
                        {
                            if (counter.ToString() == val[cur_lex_lexem])
                            {
                                cur_lex_lexem_word = s;
                            }
                            counter++;
                        }
                        break;
                    case "2":
                        foreach (var s in delimeters)
                        {
                            if (counter.ToString() == val[cur_lex_lexem])
                            {
                                cur_lex_lexem_word = s;
                            }
                            counter++;
                        }
                        break;
                    case "3":
                        foreach (var s in identificators)
                        {
                            if (counter.ToString() == val[cur_lex_lexem])
                            {
                                cur_lex_lexem_word = s;
                            }
                            counter++;
                        }
                        break;
                    case "4":
                        foreach (var s in numbers)
                        {
                            if (counter.ToString() == val[cur_lex_lexem])
                            {
                                cur_lex_lexem_word = s;
                            }
                            counter++;
                        }
                        break;
                    default:
                        ErrorParser(201);
                        break;

                }
            }
            catch(IndexOutOfRangeException)
            {
                ErrorParser(204);
            }
            cur_lexem_number = cur_lex_lexem;
            cur_lex_lexem++;
        }
        public static bool equals(string S)
        {
            int counter = 1;
            switch(num[cur_lex_lexem-1])
            {
                case "1":
                    foreach(var i in key_word)
                    {
                        if(i==S)
                        {
                            if(counter.ToString()== val[cur_lex_lexem-1])
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        counter++;
                    } 
                    break;
                case "2":
                    foreach (var i in delimeters)
                    {
                        if (i == S)
                        {
                            if (counter.ToString() == val[cur_lex_lexem-1])
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        counter++;
                    }
                    break;
                case "3":
                    foreach (var i in identificators)
                    {
                        if (i == S)
                        {
                            if (counter.ToString() == val[cur_lex_lexem-1])
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        counter++;
                    }
                    break;
                case "4":
                    foreach (var i in numbers)
                    {
                        if (i == S)
                        {
                            if (counter.ToString() == val[cur_lex_lexem-1])
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        counter++;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }
        public static bool ID()
        {
            if(num[cur_lex_lexem-1]=="3")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool numer()
        {
            if (num[cur_lex_lexem - 1] == "4")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static  void add()
        {
            Array.Resize(ref declared_identificators, declared_identificators.Length + 1);
            declared_identificators[declared_identificators.Length - 1] = val[cur_lex_lexem-1];
        }
        public static bool check()
        {
            foreach(var i in declared_identificators)
            {
                if(identificators[Convert.ToInt32(val[cur_lex_lexem-1])-1]==identificators[Convert.ToInt32(i)-1]&&num[cur_lex_lexem-1]=="3")
                {
                    return true;
                }
            }
            return false;
        }
        public static void prog() //<программа> = {/ (<описание> | <оператор>) ( : | переход строки) /} end
        {
            do
            {
                get_lexem();
                if(equals("{"))
                {
                    comment();
                }
                if (equals("dim"))
                {
                    descrip();
                }
                else if (equals("[") || equals("if") || equals("for") || equals("while") || equals("read") || equals("write")||ID())
                {
                    oper();
                }
                else if (equals("end"))
                {
                    break;
                }
                else
                {
                    ErrorParser(202);
                }
                if (equals("{"))
                {
                    comment();
                }
                if (!equals(":") && !equals("\\n"))
                {

                    ErrorParser(203);
                }
            } while (equals(":") || equals("\\n"));
            if (equals("{"))
            {
                comment();
            }
            if (!equals("end"))
            {
                ErrorParser(204);
            }
        }
        public static void descrip()//<описание>::= dim <идентификатор> {, <идентификатор> } <тип>
        {
            if (equals("{"))
            {
                comment();
            }
            if (equals("dim"))
            {
                get_lexem();
                if(check())
                {
                    ErrorParser(301);
                }
                else
                {
                    add();
                }
                get_lexem();
                if (equals("{"))
                {
                    comment();
                }
                while (equals(","))
                {
                    if (equals("{"))
                    {
                        comment();
                    }
                    get_lexem();
                    if(!ID())
                    {
                        ErrorParser(205);
                    }
                    else
                    {
                        if (check())
                        {
                            ErrorParser(301);
                        }
                        else
                        {
                            add();
                        }
                    }
                    get_lexem();
                }
                type();
            }
            else
            {
                ErrorParser(206);
            }
        }
        public static void oper()//<оператор>::= 	<составной> | <присваивания> | <условный> |<фиксированного_цикла> | <условного_цикла> | <ввода> |<вывода>
        {
            if (equals("{"))
            {
                comment();
            }
            if (equals("["))
            {
                compare_oper();
            }
            else if (equals("if"))
            {
                if_oper();
            }
            else if (equals("for"))
            {
                for_cicle();
            }
            else if (equals("while"))
            {
                while_cicle();
            }
            else if (equals("read"))
            {
                input();
            }
            else if (equals("write"))
            {
                output();
            }
            else if (ID())
            {
                assign_oper();
            }
        }
        public static void type()//<тип>::= % | ! | $
        {
            if (equals("{"))
            {
                comment();
            }
            if (!equals("%")&&!equals("!")&&!equals("$"))
            {
                ErrorParser(207);
            }
            get_lexem();
        }
        public static void compare_oper()//<составной>::= «[» <оператор> { ( : | перевод строки) <оператор> } «]»
        {
            do
            {
                if (equals("{"))
                {
                    comment();
                }
                get_lexem();
                oper();
            } while (equals(":") || equals("\\n"));
            if (equals("{"))
            {
                comment();
            }
            if (!equals("]"))
                ErrorParser(208);
            get_lexem();
        }
        public static void assign_oper()//<присваивания>::= <идентификатор> as <выражение>
        {
            if (equals("{"))
            {
                comment();
            }
            if (!check())
            {
                ErrorParser(302);
            }
            get_lexem();
            if (equals("{"))
            {
                comment();
            }
            if (!equals("as"))
            {
                ErrorParser(209);
            }
            get_lexem();
            expression();
        }
        public static void if_oper()//<условный>::= if <выражение> then <оператор> [ else <оператор>]
        {
            if (equals("{"))
            {
                comment();
            }
            get_lexem();
            expression();
            if (equals("{"))
            {
                comment();
            }
            if (!equals("then"))
            {
                ErrorParser(210);
            }
            get_lexem();
            oper();
            if (equals("{"))
            {
                comment();
            }
            if (equals("else"))
            {
                get_lexem();
                oper();
            }
        }
        public static void for_cicle()//<фиксированного_цикла>::= for <присваивания> to <выражение> do <оператор>
        {
            if (equals("{"))
            {
                comment();
            }
            get_lexem();
            if(ID())
            {
                assign_oper();
            }
            if (equals("{"))
            {
                comment();
            }
            if (!equals("to"))
            {
                ErrorParser(211);
            }
            get_lexem();
            if (equals("{"))
            {
                comment();
            }
            if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
            {
                if (ID() && !check())
                    ErrorParser(302);
                expression();
            }
            else
            {
                ErrorParser(212);
            }
            if (equals("{"))
            {
                comment();
            }
            if (!equals("do"))
            {
                ErrorParser(213);
            }
            get_lexem();
            oper();
        }
        public static void while_cicle()//<условного_цикла>::= while <выражение> do <оператор>
        {
            if (equals("{"))
            {
                comment();
            }
            if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
            {
                if (ID() && !check())
                    ErrorParser(302);
                expression();
            }
            else
            {
                ErrorParser(212);
            }
            get_lexem();
            if (equals("{"))
            {
                comment();
            }
            if (!equals("do"))
            {
                ErrorParser(213);
            }
            if (equals("[") || equals("if") || equals("for") || equals("while") || equals("read") || equals("write") || ID())
            {
                oper();
            }
            else
            {
                ErrorParser(214);
            }
        }
        public static void input()//<ввода>::= read «(»<идентификатор> {, <идентификатор> } «)»
        {
            if (equals("{"))
            {
                comment();
            }
            get_lexem();
            if (!equals("("))
                ErrorParser(215);
            get_lexem();
            if (equals("{"))
            {
                comment();
            }
            if (!ID())
            {
                ErrorParser(205);
            }
            get_lexem();
            if (equals("{"))
            {
                comment();
            }
            while (equals(","))
            {
                if (equals("{"))
                {
                    comment();
                }
                get_lexem();
                if (ID() && !check())
                    ErrorParser(302);
                get_lexem();
                if (equals("{"))
                {
                    comment();
                }
            }
            if (!equals(")"))
                ErrorParser(217);
            get_lexem();
        }
        public static void output()//<вывода>::= write «(»<выражение> {, <выражение> } «)»
        {
            if (equals("{"))
            {
                comment();
            }
            get_lexem();
            if (equals("{"))
            {
                comment();
            }
            if (!equals("("))
                ErrorParser(215);
            get_lexem();
            if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
            {
                if (ID() && !check())
                    ErrorParser(302);
                expression();
                if (equals("{"))
                {
                    comment();
                }
                while (equals(","))
                {
                    if (equals("{"))
                    {
                        comment();
                    }
                    get_lexem();
                    if (equals("{"))
                    {
                        comment();
                    }
                    if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
                    {
                        if (ID() && !check())
                            ErrorParser(302);
                        expression();
                    }
                    else
                    {
                        ErrorParser(212);
                    }

                }
            }
            else
            {
                ErrorParser(212);
            }
            if (equals("{"))
            {
                comment();
            }
            if (!equals(")"))
                ErrorParser(217);
            get_lexem();
        }
        public static void expression()//<выражение>:: =	<операнд>{ <операции_группы_отношения> <операнд> }
        {
            if (equals("{"))
            {
                comment();
            }
            if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
            {
                if (equals("{"))
                {
                    comment();
                }
                if (ID() && !check())
                    ErrorParser(302);
                operand();
            }
            else ErrorParser(214);

            if (equals("{"))
            {
                comment();
            }
            if (equals("< >") || equals("=") || equals("<") || equals("<=") || equals(">") || equals(">="))
            {
                ratio();
                if (equals("{"))
                {
                    comment();
                }
                if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
                {
                    if (equals("{"))
                    {
                        comment();
                    }
                    if (ID() && !check())
                        ErrorParser(302);
                    operand();
                }
                else ErrorParser(214);
            }
        }
        public static void operand()//<операнд>::= 		<слагаемое> {<операции_группы_сложения> <слагаемое>}
        {
            if (equals("{"))
            {
                comment();
            }
            if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
            {
                if (ID() && !check())
                    ErrorParser(302);
                summand();
            }
            else ErrorParser(214);

            if (equals("{"))
            {
                comment();
            }
            if (equals("+") || equals("-") || equals("or"))
            {
                addition();
                if (equals("{"))
                {
                    comment();
                }
                if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
                {
                    if (ID() && !check())
                        ErrorParser(302);
                    summand();
                }
                else ErrorParser(214);
            }
        }
        public static void summand()//<слагаемое>::= 	<множитель> {<операции_группы_умножения> <множитель>}
        {
            if (equals("{"))
            {
                comment();
            }
            if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
            {
                if (ID() && !check())
                    ErrorParser(302);
                multiply();
            }
            else ErrorParser(214);
            if (equals("{"))
            {
                comment();
            }
            if (equals("*") || equals("/") || equals("and"))
            {
                multiplicate();
                if (equals("{"))
                {
                    comment();
                }
                if (ID() || numer() || equals("true") || equals("false") || equals("not") || equals("("))
                {
                    if (ID() && !check())
                        ErrorParser(302);
                    multiply();
                }
                else ErrorParser(214);
            }
        }
        public static void multiply()//<множитель>::= 	<идентификатор> | <число> | <логическая_константа> |<унарная_операция> <множитель> | «(»<выражение>«)»
        {
            if (equals("{"))
            {
                comment();
            }
            if (equals("("))
            {
                get_lexem();
                expression();
                if (equals("{"))
                {
                    comment();
                }
                if (!equals(")"))
                    ErrorParser(217);

            }
            if (equals("{"))
            {
                comment();
            }
            else if (equals("not"))
            {
                unar();
            }
            get_lexem();
        }
        public static void number()//<число>:: =		<целое> | <действительное>
        {
            if (equals("{"))
            {
                comment();
            }
            if (!numer())
                ErrorParser(219);
            get_lexem();
        }
        public static void ratio()//<операции_группы_отношения>:: = < > | = | < | <= | > | >=
        {
            if (equals("{"))
            {
                comment();
            }
            if (!(equals("< >") || equals("=") || equals("<") || equals("<=") || equals(">") || equals(">=")))
                ErrorParser(219);
            get_lexem();
        }
        public static void addition()//<операции_группы_сложения>:: = + | - | or
        {
            if (equals("{"))
            {
                comment();
            }
            if (!(equals("+") || equals("-") || equals("or")))
                ErrorParser(216);
            get_lexem();
        }
        public static void multiplicate()//<операции_группы_умножения>::= * | / | and
        {
            if (equals("{"))
            {
                comment();
            }
            if (!(equals("*") || equals("/") || equals("and")))
                ErrorParser(201);
            get_lexem();
        }
        public static void unar()//<унарная_операция>::= not
        {
            if (equals("{"))
            {
                comment();
            }
            if (!equals("not"))
                ErrorParser(223);
            get_lexem();
        }
        public static void comment()//<комментарий>::="{"<символ>"}"
        {
            while (!equals("}"))
            {
                get_lexem();
            }
            get_lexem();
        }
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
                    str += "Ожидалось end.";
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
                case 114:
                    str += "Ожидалось D или d или B или b или H или h или O или o или . или E или e или число.";
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
            catch (Exception)
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
            string[,] lexems = new string[0, 2];
            string[] key_word_read;
            string[] delimeters_read;
            int cur_lexem = 0;
            string CurCond = "H";
            word_char = word.ToCharArray();
            key_word_read = File.ReadAllLines("KeyWords.txt");
            key_word = new string[key_word_read.Length];
            for (int i = 0; i < key_word_read.Length; i++)
            {
                key_word[i] = key_word_read[i].Split(' ')[0];
            }
            delimeters_read = File.ReadAllLines("Delimeters.txt");
            delimeters = new string[delimeters_read.Length];
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
                        else if (word_char[cur_pos] == 'e'&& word_char[cur_pos+1] == 'n')
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
                        ResizeArray(ref lexems,cur_lexem+1, 2);
                        lexems[cur_lexem, 0] = 2.ToString();
                        lexems[cur_lexem, 1] = 22.ToString();
                        cur_lexem++;
                        cur_pos++;
                        CurCond = "H";
                        break;
                    case "FIN":
                        if (word_char[cur_pos + 1] == 'n' && word_char[cur_pos + 2] == 'd' && word_char.Length == cur_pos + 3)
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
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
                                ResizeArray(ref lexems, cur_lexem + 1, 2);
                                cur_pos++;
                                CurCond = "ID";
                                lexems[cur_lexem, 0] = GetLexem(buf_word)[0].ToString();
                                lexems[cur_lexem, 1] = GetLexem(buf_word)[1].ToString();
                                isId = true;
                            }
                            else
                            {
                                ResizeArray(ref lexems, cur_lexem + 1, 2);
                                isId = false;
                                lexems[cur_lexem, 0] = 3.ToString();
                                lexems[cur_lexem, 1] = (cur_ID+1).ToString();
                                cur_pos++;
                                CurCond = "ID";
                            }
                        }
                        else
                        {
                            if(!isId)
                            {
                                Array.Resize(ref identificators, cur_ID + 1);
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
                        if(word_char[cur_pos]!='O'&& word_char[cur_pos] != 'o'&& word_char[cur_pos] != 'd'&& word_char[cur_pos] != 'D'&& word_char[cur_pos] != 'H'&& word_char[cur_pos] != 'h'&& word_char[cur_pos] != 'B'&& word_char[cur_pos] != 'b'&& word_char[cur_pos] != 'E'&& word_char[cur_pos] != 'e'&& word_char[cur_pos] != '.'&&!Char.IsDigit(word_char[cur_pos]))
                        {
                            errorLex(114);
                        }
                        if (word_char[cur_pos] == 'O' || word_char[cur_pos] == 'o')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num + 1).ToString(); ;
                            numbers[cur_num] = GetNum(buf_num, ss.OCT);
                            cur_lexem++;
                            cur_num++;
                        }
                        else if (word_char[cur_pos] == 'd' || word_char[cur_pos] == 'D')
                        {
                            if (word_char[cur_pos] == 'd' && word_char[cur_pos + 1] == 'i')
                            {
                                errorLex(107);
                            }
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num + 1).ToString();
                            numbers[cur_num] = buf_num.Remove(buf_num.Length - 1);
                            cur_lexem++;
                            cur_num++;
                        }
                        else if (word_char[cur_pos] == 'H' || word_char[cur_pos] == 'h')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num + 1).ToString(); ;
                            numbers[cur_num] = GetNum(buf_num, ss.HEX);
                            cur_lexem++;
                            cur_num++;
                        }
                        else if (word_char[cur_pos] == 'B' || word_char[cur_pos] == 'b')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num + 1).ToString(); ;
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
                        else if (Convert.ToInt32(Convert.ToString(word_char[cur_pos])) > 1 && Convert.ToInt32(Convert.ToString(word_char[cur_pos])) < 8)
                        {
                            CurCond = "OCT";
                        }
                        else if (word_char[cur_pos] == '0' || word_char[cur_pos] == '1')
                        {
                            CurCond = "BIN";
                        }
                        else if (Convert.ToInt32(Convert.ToString(word_char[cur_pos])) > 7 || IsHex(word_char[cur_pos]))
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
                        if (word_char[cur_pos] != 'O' && word_char[cur_pos] != 'o' && word_char[cur_pos] != 'd' && word_char[cur_pos] != 'D' && word_char[cur_pos] != 'H' && word_char[cur_pos] != 'h' && word_char[cur_pos] != 'B' && word_char[cur_pos] != 'b' && word_char[cur_pos] != 'E' && word_char[cur_pos] != 'e' && word_char[cur_pos] != '.' && !Char.IsDigit(word_char[cur_pos]))
                        {
                            errorLex(114);
                        }
                        if (word_char[cur_pos] == 'O' || word_char[cur_pos] == 'o')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num+1).ToString(); ;
                            numbers[cur_num] = GetNum(buf_num, ss.OCT);
                            cur_lexem++;
                            cur_num++;
                        }
                        else if(word_char[cur_pos]=='d'||word_char[cur_pos]=='D')
                        {
                            if (word_char[cur_pos] == 'd' && word_char[cur_pos + 1] == 'i')
                            {
                                errorLex(107);
                            }
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num + 1).ToString();
                            numbers[cur_num] = buf_num.Remove(buf_num.Length - 1);
                            cur_lexem++;
                            cur_num++;
                        }
                        else if (word_char[cur_pos] == 'H' || word_char[cur_pos] == 'h')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num + 1).ToString();
                            numbers[cur_num] = GetNum(buf_num, ss.HEX);
                            cur_lexem++;
                            cur_num++;
                        }
                        else if (word_char[cur_pos] == 'e' || word_char[cur_pos] == 'E' || word_char[cur_pos] == '.')
                        {
                            cur_pos--;
                            buf_num = buf_num.Remove(buf_num.Length - 1);
                            CurCond = "EXP";
                        }
                        else if (Convert.ToInt32(Convert.ToString(word_char[cur_pos])) > 1 && Convert.ToInt32(Convert.ToString(word_char[cur_pos])) < 8)
                        {
                            CurCond = "OCT";
                        }
                        else if (Convert.ToInt32(Convert.ToString(word_char[cur_pos])) > 7 || IsHex(word_char[cur_pos]))
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
                        if (word_char[cur_pos] == 'd' || word_char[cur_pos] == 'D'|| word_char[cur_pos] == 'h' || word_char[cur_pos] == 'H'|| word_char[cur_pos] == 'o' || word_char[cur_pos] == 'O'|| word_char[cur_pos] == 'b' || word_char[cur_pos] == 'B')
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
                                ResizeArray(ref lexems, cur_lexem + 1, 2);
                                Array.Resize(ref numbers, cur_num + 1);
                                lexems[cur_lexem, 0] = 4.ToString();
                                lexems[cur_lexem, 1] = (cur_num+1).ToString();
                                numbers[cur_num] = buf_num;
                                cur_lexem++;
                                cur_num++;
                                CurCond = "H";
                                cur_pos--;
                            }
                        }
                        else if (word_char[cur_pos] == '\n' || word_char[cur_pos] == ':')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num+1).ToString();
                            numbers[cur_num] = buf_num.Remove(buf_num.Length - 1);
                            cur_lexem++;
                            cur_num++;
                            if (word_char[cur_pos] == '\n')
                            {
                                ResizeArray(ref lexems, cur_lexem + 1, 2);
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
                        if (word_char[cur_pos] != 'O' && word_char[cur_pos] != 'o' && word_char[cur_pos] != 'd' && word_char[cur_pos] != 'D' && word_char[cur_pos] != 'H' && word_char[cur_pos] != 'h' && word_char[cur_pos] != 'B' && word_char[cur_pos] != 'b' && word_char[cur_pos] != 'E' && word_char[cur_pos] != 'e' && word_char[cur_pos] != '.' && !Char.IsDigit(word_char[cur_pos]))
                        {
                            errorLex(114);
                        }
                        if (word_char[cur_pos] == 'H' || word_char[cur_pos] == 'h')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num+1).ToString(); ;
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
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            Array.Resize(ref numbers, cur_num + 1);
                            isLetter = true;
                            CurCond = "H";
                            lexems[cur_lexem, 0] = 4.ToString();
                            lexems[cur_lexem, 1] = (cur_num+1).ToString();
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
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            CurCond = "H";
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '$' || word_char[cur_pos] == '!' || word_char[cur_pos] == '%')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            CurCond = "H";
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '+' || word_char[cur_pos] == '-' || word_char[cur_pos] == '*' || word_char[cur_pos] == '/')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
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
                                    ResizeArray(ref lexems, cur_lexem + 1, 2);
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
                                ResizeArray(ref lexems, cur_lexem + 1, 2);
                                CurCond = "H";
                                lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos - 1] + "" + word_char[cur_pos])[0].ToString();
                                lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                                cur_pos++;
                                cur_lexem++;
                                sost = false;
                            }
                            else
                            {
                                ResizeArray(ref lexems, cur_lexem + 1, 2);
                                CurCond = "H";
                                lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                                lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                                cur_pos++;
                                cur_lexem++;
                            }
                        }
                        else if (word_char[cur_pos] == '{')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (word_char[cur_pos] == '}')
                        {
                            ResizeArray(ref lexems, cur_lexem + 1, 2);
                            CurCond = "H";
                            lexems[cur_lexem, 0] = GetLexem(word_char[cur_pos].ToString())[0].ToString();
                            lexems[cur_lexem, 1] = GetLexem(word_char[cur_pos].ToString())[1].ToString();
                            cur_pos++;
                            cur_lexem++;
                        }
                        else if (Char.IsDigit(word_char[cur_pos]) || Char.IsLetter(word_char[cur_pos]) || word_char[cur_pos] == ' ' || word_char[cur_pos] == '\n' || word_char[cur_pos] == '\r' || word_char[cur_pos] == '\t')
                        {
                            cur_pos++;
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
                string str = lexems[i, 0] + " " + lexems[i, 1] + "\n";
                File.AppendAllText("lexems.txt", str);
            }
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] != null)
                {
                    string str = numbers[i] + " " + (i+1) + "\n";
                    File.AppendAllText("Numbers.txt", str);
                }
            }
            for (int i = 0; i < identificators.Length; i++)
            {
                if (identificators[i] != null)
                {
                    string str = identificators[i] + " " + (i+1) + "\n";
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
                    Parser();
                    Console.WriteLine("Синтаксический и семантический анализ проведены успешно.");
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
