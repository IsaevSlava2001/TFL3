using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication19
{
    class Program
    {
        public static bool IsNumeric(char n)
        {
            if (n == '0' || n == '1' || n == '2' || n == '3' || n == '4' || n == '5' || n == '6' || n == '7' || n == '8' || n == '9')
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
            Console.WriteLine("1-Ввод строки вручную\n2-ввод строки из файла");
            int a = Convert.ToInt32(Console.ReadLine());
            Stack<string> num = new Stack<string>();
            string InputStr="";
            switch(a)
            {
                case 1:
                    Console.WriteLine("Введите строку в ОПЗ");
                    InputStr = Console.ReadLine();
                    break;
                case 2:
                    InputStr = File.ReadAllText("C:/Users/1655299/Documents/Visual Studio 2015/Projects/ConsoleApplication18/ConsoleApplication18/bin/Debug/rez.txt");
                    break;
                default:
                    Console.WriteLine("Ошибка");
                        Console.ReadLine();
                        Environment.Exit(1);
                    break;
            }
            for (int i=0;i<InputStr.Length;i++)
            {
                if(IsNumeric(InputStr[i]))
                {
                    num.Push(Convert.ToString(InputStr[i]));
                }
                else
                {
                    int k = Convert.ToInt32(num.Pop());
                    int b = Convert.ToInt32(num.Pop());
                    int rez = 0;
                    switch(InputStr[i])
                    {
                        case '+':
                            rez = b + k;
                            break;
                        case '-':
                            rez = b - k;
                            break;
                        case '*':
                            rez = b * k;
                            break;
                        case '/':
                            rez = b / k;
                            break;
                    }
                    num.Push(Convert.ToString(rez));
                }
            }
            Console.WriteLine(num.Pop());
            Console.ReadLine();

        }
    }
}
