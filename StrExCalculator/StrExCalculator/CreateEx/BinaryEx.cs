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
        static Type ExType = typeof(Expression);
        static Type[] ExTypes = { typeof(Expression), typeof(Expression) };
        static (MethodInfo ExMethod, int priority,int deep) getOperatorExMethod(string formula,int I,int deep)
        {
            
            
            switch (formula[I]) {
                //1
                case '+':
                    return (ExType.GetMethod("Add", ExTypes), 1,deep);
                    
                case '-':
                    return (ExType.GetMethod("Subtract", ExTypes), 1,deep);
                    
                //3
                case '*':
                    return (ExType.GetMethod("Multiply", ExTypes), 3,deep);
                    
                case '/':
                    return (ExType.GetMethod("Divide", ExTypes), 3,deep);
                    
                //5
                case '^':
                    return (ExType.GetMethod("Power", ExTypes), 5, deep);
                    

            }
            throw new Exception(I+"の"+formula[I]+"が不正");

        }

        //途中計算
        static void setOperatorEx1(Stack<(MethodInfo ExMethod, int priority, int deepNum)> Operator, (MethodInfo ExMethod, int priority, int deepNum) latestOperator, Stack<Expression> ItemEx, int deepNum)
        {
            
            Expression latestItemEx = ItemEx.Pop();
            while (Operator.Peek().priority >= latestOperator.priority&&Operator.Peek().deepNum==deepNum)
            {
                latestItemEx = (Expression)(Operator.Pop().ExMethod.Invoke(null, new Expression[] { ItemEx.Pop(), latestItemEx }));
            }
            //式木を代入
            ItemEx.Push(latestItemEx);
        }

        //括弧とカンマの中の計算
        static void setOperatorEx2(Stack<(MethodInfo ExMethod, int priority, int deepNum)> Operator, Stack<Expression> ItemEx, int deepNum)
        {
            Expression latestItemEx=ItemEx.Pop();
            while (Operator.Peek().deepNum== deepNum)
            {
                latestItemEx = (Expression)(Operator.Pop().ExMethod.Invoke(null, new Expression[] { ItemEx.Pop(), latestItemEx }));
            }
            //式木を代入
            ItemEx.Push(latestItemEx);
        }
    }
}
