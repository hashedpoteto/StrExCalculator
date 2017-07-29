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
        static void comma() { }

        static LambdaExpression reading(string formula)
        {
            

            int I=0;//文字列の順番を調べる
            Stack<(MethodInfo ExMethod, int priority, int deepNum)> Operator = new Stack<(MethodInfo, int, int)>();//演算子　(演算子エクスプレッションメソッド,優先値,括弧の深さ)
            Stack<Expression> ItemEx = new Stack<Expression>(); //項
            Stack<(string className,string methodName)> deep = new Stack<(string,string)>();//括弧の深さ　関数なら関数名、ただの括弧なら""

            (MethodInfo ExMethod, int priority, int deep) latestOperator;

            Operator.Push((ExType.GetMethod("Add", ExTypes),0,-1));
            ItemEx.Push(Expression.Constant(0.0));
            //Expression latestItemEx = Expression.Constant(0);


            
            while (I<formula.Length)
            {
                cleanSpace(formula,ref I);

                if (char.IsDigit(formula[I]))
                {//数字なら
                    ItemEx.Push(getItemEx(formula, ref I));
                    //latestItemEx = getItemEx(formula,ref I);
                }
                else if (char.IsLetter(formula[I]))
                {//英字なら
                    deep.Push(getMethodName(formula,ref I));
                    if (formula[I] == ')')
                        ;
                }
                else
                {//計算記号なら
                    switch (formula[I])
                    {
                        case '(':
                            //括弧開始なら
                            deep.Push(("",""));
                            break;
                        case ')':

                            //残りの演算子も計算する
                            setOperatorEx2(Operator, ItemEx, deep.Count);


                            //メソッドかどうか
                            if (deep.Peek().methodName != "")
                                setMethodCallEx(deep, ItemEx);
                            else
                                deep.Pop();
                            break;
                        case ',':
                            //引数の区切りなら
                            setOperatorEx2(Operator, ItemEx, deep.Count);
                            deep.Push(("", ","));
                            break;
                        default:
                            
                            //演算子なら
                            latestOperator=getOperatorExMethod(formula,I,deep.Count);
                            //計算できる部分を計算する
                            setOperatorEx1(Operator,latestOperator,ItemEx,deep.Count);//オペレータ

                            Operator.Push(latestOperator);
                            
                            break;
                    }
                    I++;
                }
            }
            setOperatorEx2(Operator, ItemEx, deep.Count);
            return Expression.Lambda(ItemEx.Pop());
        }
        static void cleanSpace(string formula, ref int I)
        {
            while (I < formula.Length && formula[I] == ' ')
            {
                I++;
            }
        }
    }
}
