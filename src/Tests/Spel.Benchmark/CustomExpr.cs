using System;

namespace Spel.Benchmark;

public class CustomExpr
{
    public double? Evaluate(TestRow r)
    {
        if(r.P == null)
        {
            return null;
        }
        var v1 = r.P.Value;
        if(r.O == null)
        {
            return null;
        }
        var v2 = r.O.Value;
        var ud = (double?)v1 / v2;
        if(ud == null)
        {
            return null;
        }
        var v3 = ud.Value;
        var e_0 = (bool?)(v3 < 0);
        if(e_0 == null)
        {
            return null;
        }
        var v_e_0 = e_0.Value;
        var e_1 = (double?)Math.Abs(v3);
        if(e_1 == null)
        {
            return null;
        }
        var v_e_1 = e_1.Value;
        var e_2 = (double?)Math.Abs(v3);
        if(e_2 == null)
        {
            return null;
        }
        var v_e_2 = e_2.Value;
        var e_3 = (double?)(1 - v_e_2);
        if(e_3 == null)
        {
            return null;
        }
        var v_e_3 = e_3.Value;
        var d = (double?)(v_e_0 ? v_e_1 : v_e_3);
        if(d == null)
        {
            return null;
        }
        double? v_e_4;
        if(d <= 0.03)
        {
            v_e_4 = 0.01;
        }
        else if(d <= 0.1)
        {
            v_e_4 = 0.05;
        }
        else if(d <= 0.225)
        {
            v_e_4 = 0.15;
        }
        else if(d <= 0.4)
        {
            v_e_4 = 0.3;
        }
        else if(d <= 0.6)
        {
            v_e_4 = 0.5;
        }
        else if(d <= 0.775)
        {
            v_e_4 = 0.7;
        }
        else if(d <= 0.9)
        {
            v_e_4 = 0.85;
        }
        else if(d <= 0.97)
        {
            v_e_4 = 0.95;
        }
        else
        {
            v_e_4 = 0.99;
        }
        return v_e_4;
    }

    public double? EvaluateBucket(TestRow r)
    {
        if(r.P == null)
        {
            return null;
        }
        var v1 = r.P.Value;
        if(r.O == null)
        {
            return null;
        }
        var v2 = r.O.Value;
        var ud = (double?)v1 / v2;
        if(ud == null)
        {
            return null;
        }
        var v3 = ud.Value;
        var e_0 = (bool?)(v3 < 0);
        if(e_0 == null)
        {
            return null;
        }
        var v_e_0 = e_0.Value;
        var e_1 = (double?)Math.Abs(v3);
        if(e_1 == null)
        {
            return null;
        }
        var v_e_1 = e_1.Value;
        var e_2 = (double?)Math.Abs(v3);
        if(e_2 == null)
        {
            return null;
        }
        var v_e_2 = e_2.Value;
        var e_3 = (double?)(1 - v_e_2);
        if(e_3 == null)
        {
            return null;
        }
        var v_e_3 = e_3.Value;
        var d = (double?)(v_e_0 ? v_e_1 : v_e_3);
        if(d == null)
        {
            return null;
        }
        if(d > 0.97)
            return 0.99;
        if(d <= 0.03)
            return 0.01;

        if(d > 0.4)
        { // > 3 <= 7
            if(d > 0.775)
            { // > 5 <= 7
                if(d > 0.9)
                {  // > 6 <= 7
                    return 0.95;
                }
                // >5 <= 6
                return 0.85;
            }
            // > 3 <= 5
            if(d > 0.6)
            { // > 4 <= 5
                return 0.7;
            }
            // > 3 <= 4
            return 0.5;
        }
        else
        { // > 0 <= 3
            if(d > 0.1)
            { // > 1 <= 3
                if(d > 0.225)
                { // > 2 <=3
                    return 0.3;
                }
                return 0.15;
            }
            // > 0 < 1
            return 0.05;
        }
    }
}