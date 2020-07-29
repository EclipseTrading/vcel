namespace VCEL.Test.Shared
{
    public static class Expressions
    {
        public static string GetExpression(string name)
        {
            var prop = typeof(Expressions).GetField(name);
            return (string)prop.GetValue(null);
        }

        public const string Abs = @"T(System.Math).Abs(-1.0)";
        public const string GetProp = @"P";
        public const string Add = @"0.1 + 0.2";
        public const string Subtract = @"0.1 - 0.2";
        public const string Divide = @"0.1 / 0.2";
        public const string Tern = @"true ? 0.1 : 0.2";
        public const string DateTime = @"T(System.DateTime).Now";
        public const string DivNull = @"1 / null";
        public const string Div0 = @"1 / 0";
        public const string In = @"'ABC' in { 'ABC', 'DEF', 'GHI' }";
        public const string InEnd = @"'XYZ' in { 'ABC', 'DEF', 'GHI', 'IJK', 'LMN', 'OPQ', 'RST', 'UVW', 'ZYZ' }";
        public const string Matches = @"'ABC' matches 'A.*'";
        public const string MatchesComplex = @"'8888' matches '(?:.+,|^)(690|700|780|799|768|750|1372|1821|1481|2954|1733|1760|1748|1770|1710|2421|2430|4458|4429|6999|6997|6888|6999|8520|8111|8222|8333|8444|8777|8888|8963|8965|8931)(?:,.+|$)'";
        public const string Between = @"5 between { 4, 6 }";
        public const string And = @"true and false";
        public const string Or = @"true or false";
        public const string Not = @"!true";
        public const string TestExpr0 = "1 - P / O";
        public const string TestExpr1 = "1 - (P / O)";
        public const string TestExpr2 = "(P / O) < 0";
        public const string TestExpr3 = "((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> -0.01";
        public const string TestExpr4 = "(P / O) < 0 ? T(System.Math).Abs(P / O) : 1 - (P / O) > -0.01";
        public const string TestExpr5 = @"(((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> -0.01) and 
(((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O)) <= 0.03)
    ? 0.01
    : 0.02";

        public const string TernaryArith1 = @"
(
    (
        (K matches 'XXX') 
        ? (S - C)*M 
        : (C - S)*M
    ) - tP
) * M * Q";
        public const string TernaryArith2 = "(B - Up)/Up between {0,0.01} ? PS*M*tO:0";
        public const string TernaryArith3 = "(aOs ==null ?0:aOs )+ mO";

        public const string NestedTernary1 = @"
(((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> -0.01) and 
(((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))<=0.03) 
    ? 0.01 
    : (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> 0.03) and 
      (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))<=0.10) 
       ? 0.05 
       : (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> 0.1) and 
         (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))<=0.225) 
         ? 0.15 
         : (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> 0.225) and 
           (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))<=0.4) 
           ? 0.3 
           : (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> 0.4) and 
             (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))<=0.6) 
             ? 0.5 
             : (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> 0.6) and 
               (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))<=0.775) 
               ? 0.7 
               : (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> 0.775) and 
                 (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))<=0.9) 
                 ? 0.85 
                 : (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))> 0.9) and 
                   (((P / O) < 0 ? T(System.Math).Abs((P / O)) : 1 - (P / O))<=0.97) 
                   ? 0.95 
                   : 0.99";
        public const string NestedTernary2 = @"
((D < 0 ? T(System.Math).Abs(D) : 1 - D) > -0.01) and 
((D < 0 ? T(System.Math).Abs(D) : 1 - D)<=0.03) 
    ? 0.01 
    : ((D < 0 ? T(System.Math).Abs(D) : 1 - D)> 0.03) and 
      ((D < 0 ? T(System.Math).Abs(D) : 1 - D)<=0.10) 
        ? 0.05 
        : ((D < 0 ? T(System.Math).Abs(D) : 1 - D)> 0.1) and 
          ((D < 0 ? T(System.Math).Abs(D) : 1 - D)<=0.225) 
            ? 0.15 
            : ((D < 0 ? T(System.Math).Abs(D) : 1 - D)> 0.225) and 
              ((D < 0 ? T(System.Math).Abs(D) : 1 - D)<=0.4) 
                ? 0.3 
                : ((D < 0 ? T(System.Math).Abs(D) : 1 - D)> 0.4) and 
                  ((D < 0 ? T(System.Math).Abs(D) : 1 - D)<=0.6) 
                    ? 0.5 
                    : ((D < 0 ? T(System.Math).Abs(D) : 1 - D)> 0.6) and 
                      ((D < 0 ? T(System.Math).Abs(D) : 1 - D)<=0.775) 
                        ? 0.7 
                        : ((D < 0 ? T(System.Math).Abs(D) : 1 - D)> 0.775) and 
                          ((D < 0 ? T(System.Math).Abs(D) : 1 - D)<=0.9) 
                            ? 0.85 
                            : ((D < 0 ? T(System.Math).Abs(D) : 1 - D)> 0.9) and 
                              ((D < 0 ? T(System.Math).Abs(D) : 1 - D)<=0.97) 
                                ? 0.95 
                                : 0.99";

        public const string LetGuard = @"
let
    ud = P / O,
    d = (ud < 0 ? abs(ud) : 1 - ud)
in match 
    | d <= 0.03  = 0.01
    | d <= 0.1   = 0.05
    | d <= 0.225 = 0.15
    | d <= 0.4   = 0.3
    | d <= 0.6   = 0.5
    | d <= 0.775 = 0.7
    | d <= 0.9   = 0.85
    | d <= 0.97  = 0.95
    | otherwise 0.99
";
        public const string LiteralExpr1 = "1.";
        public const string LiteralExpr2 = "1.0";
        public const string LiteralExpr3 = "'ORDER'";

        public const string DateExprTotalDays = "((D - T(DateTime).Today).TotalDays+1)";
        public const string TimeExprTotalExconds = "(Ts - CTs).TotalSeconds";
        public const string ArithExpr1 = "(_5B-BO)/C";
        public const string ArithExpr2 = "(1.001 ^ ((D - DateTime.Today).TotalDays/360) - 1) * CSD";
        public const string ArithExpr3 = "(B - ULPx)";
        public const string ArithExpr4 = "(b/C) -1";
        public const string ArithExpr5 = "(B-C)/C";
        public const string ArithExpr6 = "(a - (tA+tB)*0.5)*aQ*-1";
        public const string ArithExpr7 = "_5BVO- iVA";
        public const string ArithExpr8 = "(iAP/V) + SV";
        public const string ArithExpr9 = "(BTS * 365 / (D - T(System.DateTime).Now).TotalDays)/T(System.Math).Abs(SD)";
        public const string ArithExpr10 = "(tv+(bO+aO)/2-a)/((-bO+aO)/2)";
        public const string ArithExpr11 = "(AO - BO)/C";
        public const string ArithExpr12 = "(SG/100)/-VT";
        public const string ArithExpr13 = "(D5*-0.075 + D15*-0.0375 + D30*-0.0225) + (D70*0.015 + D85*0.03 + D95*0.045)";
        public const string ArithExpr14 = "1/1";
        public const string ArithExpr15 = "1/M";
        public const string ArithExpr16 = "2*Vg";
        public const string ArithExpr17 = "T(System.Math).Abs(C - B)/C";
        public const string ArithExpr18 = "T(System.Math).Max( (pA==null?0:pA) , (pB==null?0:pB) )";
        public const string ArithExpr19 = "T(System.Math).Min(T(System.Math).Min(N, bQ), 3000.0*S)";
        public const string ArithExpr20 = "T(System.Math).Round(B/1000,1)";
        public const string ArithExpr21 = "T(System.Math).Sqrt(T(System.Math).Abs(VT)*200/T(System.Math).Abs(SG))";
        public const string ArithExpr22 = "BTS * 365 / (D - T(System.DateTime).Now).TotalDays";
        public const string ArithExpr23 = "CallCurve";
        public const string ArithExpr24 = "(LV - PC) * 100 / PC";
        public const string ArithExpr25 = "(sV + (sn == 'XYZ CS' ? -2*sV : 0))*100";
        public const string ArithExpr26 = "(sVD + (UL == 'XYZ' ? -2*sVD : 0))*100";
        public const string ArithExpr27 = "(tC - T(System.DateTime).Now).TotalSeconds";
        public const string ArithExpr28 = "A / lA";
        public const string ArithExpr29 = "BPD > 0.01";
        public const string ArithExpr30 = "(tA - tB) / (a - b)";

        public const string BoolExpr1 = @"
(
    (
        (
            (VO > 0.8 or VO < -0.8) and 
            (
                (sD < 0.95 and sD > 0.05) or 
                (sD < -0.05 and sD > -0.95)
            )
        ) or 
        (
            (VO > 0.4 or VO < -0.4) and 
            (
                (sD < 0.7 and sD > 0.3) or 
                (sD < -0.3 and sD > -0.7)
            )
        )
    ) and 
    (TC > 400 or TC < -400)
)";
        public const string BoolExpr2 = @"
(
    (UL == 'ABC' or UL == 'DE') and 
    (SC > 0.0015 or SC < -0.0015 or Q > 10)
) or (
    UL == 'FG' and 
    (SC > 10 or SC < -10 or Q > 10)
) or (
    UL == 'HIJ' and 
    (SC > 0.01 or SC < -0.01 or Q > 10) and 
    K != 'Spot'
) or (
    UL == 'KLM' and 
    (SC > 0.01 or SC < -0.01 or Q > 10) and 
    K != 'Spot'
) or (
    UL == 'NOP' and 
    (SC > 0.01 or SC < -0.01 or Q > 10) and 
    K != 'Spot'))
";

        public const string BoolExpr3 = "((PS == null or PS <= 0) and msg.StartsWith(' started')) and ts.Hour >= 10 and ((ts - T(System.DateTime).Now).TotalSeconds > -15)";
        public const string BoolExpr4 = @"
(   
    (BDP and SDP) and 
    (hB and hA) and (qB and qA) and 
    (T(System.Math).Abs(tO - tO1) <= 0.001))
        ? ((mO > 0 and vO > 0)
            ? true 
            : false) 
        : true
";
        public const string VarFilter1 = "((BE != #BE and SE != #SE) and (fc == '2800' or fc == '2828'))";



    }
}
