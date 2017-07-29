using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace HashedCode
{
    public partial class StrExCalculator
    {
        //メソッド名を読み込む
        static (string,string) getMethodName(string formula, ref int I)
        {
            string Fname = "";
            string Cname = "";
            while (true)
            {
                while (I<formula.Length&&(char.IsLetter(formula, I) || char.IsDigit(formula, I) || formula[I] == '_'))
                {
                    Fname += formula[I];
                    I++;
                }
                break;
                /*
                if (formula[I] == '.')
                {
                    if (Cname != "")
                    {
                        Cname += '.';
                    }
                    Cname += Fname;
                    I++;
                }
                
                else
                    break;
                    */
            }
            cleanSpace(formula, ref I);

            if (Cname == "")
            {
                Cname = "System.Math";
            }
            if (I < formula.Length && formula[I] == '(')
            {
                
                I++;
                cleanSpace(formula, ref I);

                return (Cname,Fname);
            }
            throw new Exception(I + "番目の文字（メソッド名の後の文字）は\"(\"でなければならない");
            //throw new Exception("メソッド" + Cname + Fname + "が存在しない");
        }
        //メソッドコールエクスプレッションの作成
        static void setMethodCallEx (Stack<(string Cname,string Mname)> deep,Stack<Expression> ItemEx)
        {
            (string Cname, string Mname) methodName = deep.Pop();
            Stack<Expression> arguments = new Stack<Expression>();

            //引数の取り出し
            arguments.Push(ItemEx.Pop());
            while (methodName.Mname == ",")
            {
                arguments.Push(ItemEx.Pop());
                methodName = deep.Pop();
            }


            Type[] argstype = new Type[arguments.Count];
            Expression[] argsArray = arguments.ToArray();
            for (int i = 0; i < argstype.Length; i++)
                argstype[i] = typeof(double);

            if (typeof(System.Math).GetMethod(methodName.Mname, argstype) == null)
                throw new Exception("関数System.Math."+methodName.Mname +"(引数"+argstype.Length+ "つ)が存在しない");

            //メソッド式木の作成
            ItemEx.Push(Expression.Call(typeof(System.Math).GetMethod(methodName.Mname,argstype),argsArray));
        }

        //メソッドコールエクスプレッションの作成(引数なし)
        static void setMethodCallExArg0(Stack<(string Cname, string Mname)> deep, Stack<Expression> ItemEx)
        {
            (string Cname, string Mname) methodName = deep.Pop();
            Stack<Expression> arguments = new Stack<Expression>();

            


            Type[] argstype = new Type[arguments.Count];
            Expression[] argsArray = arguments.ToArray();
            for (int i = 0; i < argstype.Length; i++)
                argstype[i] = typeof(double);

            if (typeof(System.Math).GetMethod(methodName.Mname) == null)
                throw new Exception("関数System.Math." + methodName.Mname + "(引数なし)が存在しない");

            //メソッド式木の作成
            ItemEx.Push(Expression.Call(typeof(System.Math).GetMethod(methodName.Mname)));
        }
    }
}
