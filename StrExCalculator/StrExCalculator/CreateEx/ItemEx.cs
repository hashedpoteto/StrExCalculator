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
        static ConstantExpression getItemEx(String formula,ref int I)
        {
            
            int element=0;//最初の要素をとる
            double numeric = element;//読み取った数値を格納
            int Decimal = 0;//0なら整数、それ以外は小数位を読み取る

            
            while (I<formula.Length)
            {
                
                if (int.TryParse(formula[I].ToString(), out element))
                {//数字なら
                    //小数部分かどうか
                    if (Decimal == 0)//整数部分なら
                    {
                        numeric *= 10;
                        numeric += element;
                    }
                    else//小数部分なら
                    {
                        numeric += element * Math.Pow(10, -Decimal);
                    }

                }
                else if (formula[I] == '.' && Decimal == 0)
                //小数点なら
                {
                    Decimal++;
                    I++;
                    continue;
                }
                else
                    //それ以外の文字なら
                    break;
                I++;//charごとに回していく
            }
            //格納した数を返す
            return Expression.Constant(numeric);
        }
    }
}
