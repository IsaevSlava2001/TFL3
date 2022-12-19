using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace task5
{
    class Program
    {
        public static string[] states = new string[100];
        public static string[] buf_states = new string[100];
        public static string[] alphabet = new string[100];
        public static string[] transition = new string[100];
        public static string[,] transation_buff = new string[100,100];
        public static string start, finish;
        static void Main(string[] args)
        {
            string a = File.ReadAllText("states.txt");
            states = a.Split(' ');
            string b = File.ReadAllText("alphabet.txt");
            alphabet = b.Split(' ');
            string c = File.ReadAllText("transition.txt");
            transition = c.Split('(',')',' ');
            start = File.ReadAllText("start.txt");
            finish = File.ReadAllText("finish.txt");
            double len = states.Length;
            string bin;
            len = Math.Pow(2, len);
            for(int i=0;i<len;i++)
            {
                bin = Convert.ToString(i, 2);
                bin = UPDLen(bin);
                int[] mass=getcounts(bin);
                buf_states[0] = "null";
                for(int j=0;j<mass.Length;j++)
                {
                    if(mass[j]==1)
                    {
                        if (buf_states[i] != null)
                        {
                            buf_states[i] = buf_states[i] + ',' + states[j];
                        }
                        else
                        {
                            buf_states[i] = states[j];
                        }
                    }
                }
            }
            for(int i=0;i<buf_states.Length;i++)
            {
                
            }
            Console.ReadLine();
        }

        private static string UPDLen(string bin)
        {
            while(bin.Length<states.Length)
            {
                bin = '0' + bin;
            }
            return bin;
        }
        private static int[] getcounts(string h)
        {
            int[] k=new int[h.Length];
            int l = 0;
            int f = 0;
            while(f<h.Length)
            {
                if(h[f]=='1')
                {
                    k[l] = 1;
                }
                l++;
                f++;
            }
            return k;
        }
    }
}
