using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashedCode;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            StrExCalculator sec;
            string formula;
            while (true)
            {
                try
                {
                    Console.WriteLine("式を入力してください（exitで終了）");
                    
                    formula = Console.ReadLine();
                    if (formula == "exit")
                        return;
                    sec = new StrExCalculator(formula);
                    sec.Compile();
                    Console.WriteLine("={0}\n", sec.Calc());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
        }
    }
}
