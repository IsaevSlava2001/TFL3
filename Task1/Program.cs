using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication18
{
    class Program
    {
        public static bool IsNumeric(char n)
        {
            if(n=='0'||n=='1'||n=='2'||n=='3'||n=='4'||n=='5'||n=='6'||n=='7'||n=='8'||n=='9')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int CheckPriority(char n)
        {
            if (n == '*' || n == '/') return 3;
            if (n == '+' || n == '-') return 2;
            if (n == '(') return 1;
            else return 0;
        }
        static void Main(string[] args)
        {
            string str;
            string OutputStr="";
            Console.WriteLine("Введите строку со всеми символами");
            str = Console.ReadLine();
            Stack<string> num = new Stack<string>();
            for(int i=0;i<str.Length;i++)
            {
                if(IsNumeric(str[i]))
                {
                    OutputStr += str[i];
                }
                else
                {
                    int pri = CheckPriority(str[i]);
                    int priStack=0;
                    string stack;
                    try
                    {
                        stack = num.Peek();
                    }
                    catch(Exception e)
                    {
                        stack = "";
                    }
                    if(stack!="")
                    {
                        priStack = CheckPriority(Convert.ToChar(stack));
                    }
                    //3В
                    if (str[i] == '(')
                    {
                        num.Push(Convert.ToString(str[i]));
                    }
                    //---------------------------
                    //3Г
                    else if (str[i] == ')')
                    {
                        while (num.Peek() != "(")
                        {
                            OutputStr += num.Pop();
                        }
                        num.Pop();
                    }
                    //-----------------
                    //3А
                    else if (stack==""||priStack<pri)
                    {
                        num.Push(Convert.ToString(str[i]));
                    }
                    //----------------------
                    //3Б
                    else if(priStack==pri||priStack>pri)
                    {
                        while(priStack == pri || priStack > pri)
                        {
                            string temp = num.Pop();
                            OutputStr += temp;
                            try
                            {
                                priStack = CheckPriority(Convert.ToChar(num.Peek()));
                            }
                            catch (Exception e)
                            {
                                break;
                            }
                        }
                        num.Push(Convert.ToString(str[i]));
                    }
                    //-----------------------
                }
            }
            bool IsStackFill = true;
            while (IsStackFill)
            {
                try
                {
                    OutputStr += num.Pop();

                }
                catch (Exception e)
                {
                    IsStackFill = false;
                }
            }
            Console.WriteLine(OutputStr);
            File.WriteAllText("rez.txt", OutputStr);
            Console.ReadLine();
        }
    }
}
