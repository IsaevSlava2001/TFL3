using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace task2
{
    class Program
    {
        public static string word_read;
        public static char[] word_char;
        public static lexems[] lexems_tokens=new lexems[100];
        public static string[] tokens = new string[100];
        public enum lexems
        {
            BEGIN_OBJECT,
            END_OBJECT,
            BEGIN_ARRAY,
            END_ARRAY,
            COMMA,
            COLON,
            LITERAL,
            STRING,
            NUMBER
        }
        enum states
        {
            H,
            OPEN_BRACKET,
            CLOSE_BRACKET,
            OPEN_SQUARE,
            CLOSE_SQUARE,
            STRING,
            ENUMERABLE,
            REZ,
            BOOLEAN,
            NUMBER
        }
        public static bool getWord(string a)
        {
            if(a=="true"||a=="false"||a=="null")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void Main(string[] args)
        {
            string buf="";
            word_read = File.ReadAllText("word.txt");
            word_char = word_read.ToCharArray();
            int cur_token = 0;
            var a = states.H;
            int cur_pos = 0;
            while(cur_pos<word_char.Length)
            {
                switch(a)
                {
                    case states.H:
                        if(word_char[cur_pos] == ' '|| word_char[cur_pos] == '\t'|| word_char[cur_pos] == '\n'|| word_char[cur_pos] == '\r')
                        {
                            cur_pos++;
                        }
                        else if(word_char[cur_pos]=='{')
                        {
                            a = states.OPEN_BRACKET;
                        }
                        else if(word_char[cur_pos]=='[')
                        {
                            a = states.OPEN_SQUARE;
                        }
                        if (word_char[cur_pos] == '}')
                        {
                            a = states.CLOSE_BRACKET;
                        }
                        else if (word_char[cur_pos] == ']')
                        {
                            a = states.CLOSE_SQUARE;
                        }
                        else if(word_char[cur_pos] == '"')
                        {
                            a = states.STRING;
                        }
                        else if(word_char[cur_pos] == ',')
                        {
                            a = states.ENUMERABLE;
                        }
                        else if (word_char[cur_pos] == ':')
                        {
                            a = states.REZ;
                        }
                        else if (word_char[cur_pos] == 't'|| word_char[cur_pos] == 'f'|| word_char[cur_pos] == 'n')
                        {
                            buf = "";
                            a = states.BOOLEAN;
                        }
                        else if (Char.IsDigit(word_char[cur_pos]))
                        {
                            a = states.NUMBER;
                        }
                        break;
                    case states.OPEN_BRACKET:
                        lexems_tokens[cur_token] = lexems.BEGIN_OBJECT;
                        tokens[cur_token] = "{";
                        cur_token++;
                        cur_pos++;
                        a = states.H;
                        break;
                    case states.CLOSE_BRACKET:
                        lexems_tokens[cur_token] = lexems.END_OBJECT;
                        tokens[cur_token] = "}";
                        cur_token++;
                        cur_pos++;
                        a = states.H;
                        break;
                    case states.OPEN_SQUARE:
                        lexems_tokens[cur_token] = lexems.BEGIN_ARRAY;
                        tokens[cur_token] = "[";
                        cur_token++;
                        cur_pos++;
                        a = states.H;
                        break;
                    case states.CLOSE_SQUARE:
                        lexems_tokens[cur_token] = lexems.END_ARRAY;
                        tokens[cur_token] = "]";
                        cur_token++;
                        cur_pos++;
                        a = states.H;
                        break;
                    case states.STRING:
                        cur_pos++;
                        if (word_char[cur_pos] != '"')
                        {
                            lexems_tokens[cur_token] = lexems.STRING;
                            tokens[cur_token] = tokens[cur_token] += word_char[cur_pos];
                        }
                        else
                        {
                            cur_token++;
                            cur_pos++;
                            a = states.H;
                        }
                        break;
                    case states.ENUMERABLE:
                        lexems_tokens[cur_token] = lexems.COMMA;
                        tokens[cur_token] = ",";
                        cur_token++;
                        cur_pos++;
                        a = states.H;
                        break;
                    case states.REZ:
                        lexems_tokens[cur_token] = lexems.COLON;
                        tokens[cur_token] = ":";
                        cur_token++;
                        cur_pos++;
                        a = states.H;
                        break;
                    case states.BOOLEAN:
                        buf = buf + word_char[cur_pos];
                        if (getWord(buf))
                        {
                            lexems_tokens[cur_token] = lexems.LITERAL;
                            tokens[cur_token] = buf;
                            cur_token++;
                            cur_pos++;
                            a = states.H;
                        }
                        cur_pos++;
                        break;
                    case states.NUMBER:
                        if (word_char[cur_pos] != ','&& word_char[cur_pos] != ']'&& word_char[cur_pos] != '}')
                        {
                            lexems_tokens[cur_token] = lexems.NUMBER;
                            tokens[cur_token] = tokens[cur_token] += word_char[cur_pos];
                            cur_pos++;
                        }
                        else
                        {
                            cur_token++;
                            a = states.H;
                        }
                        break;
                    default:
                        Console.WriteLine("Необработанная лексема");
                        Console.ReadLine();
                        Environment.Exit(0);
                        break;
                }
            }
            for(int i=0;i<lexems_tokens.Length;i++)
            {
                if (tokens[i] != null)
                {
                    Console.WriteLine("{" + lexems_tokens[i] + ";'" + tokens[i] + "'}");
                }
            }
            Console.ReadLine();
        }
    }
}
