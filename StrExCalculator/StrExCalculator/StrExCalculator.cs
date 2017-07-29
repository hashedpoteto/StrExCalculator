using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace HashedCode
{
    public partial class StrExCalculator
    {
        LambdaExpression exFormula;
        dynamic comp=null;
        public StrExCalculator(string formula)
        {
            exFormula= reading(formula);
        }
        //式をコンパイルする
        public void Compile()
        {
            comp=exFormula.Compile();
        }
        //コンパイルした式を計算する（コンパイルしてなければコンパイルしてから動かす）
        public double Calc()
        {
            if (comp == null)
                Compile();
            return comp();
        }

    }
}
