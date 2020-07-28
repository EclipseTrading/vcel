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
        public const string GetProp = @"PosSwimDelta";
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
        public const string MatchesComplex = @"'8969' matches '(?:.+,|^)(770|775|776|777|778|779|1359|1750|1751|1754|1755|1756|1757|1758|1759|2417|2418|2419|4420|4421|4422|4424|4425|4426|4427|4428|4429|6996|6997|6998|6999|8520|8524|8528|8529|8960|8961|8962|8963|8965|8968|8969)(?:,.+|$)'";
        public const string Between = @"5 between { 4, 6 }";
        public const string And = @"true and false";
        public const string Or = @"true or false";
        public const string Not = @"!true";
        public const string TestExpr0 = "1 - PosSwimDelta / OptionEquivalentSplitPosition";
        public const string TestExpr1 = "1 - (PosSwimDelta / OptionEquivalentSplitPosition)";
        public const string TestExpr2 = "(PosSwimDelta / OptionEquivalentSplitPosition) < 0";
        public const string TestExpr3 = "((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> -0.01";
        public const string TestExpr4 = "(PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs(PosSwimDelta / OptionEquivalentSplitPosition) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition) > -0.01";
        public const string TestExpr5 = @"(((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> -0.01) and 
(((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition)) <= 0.03)
    ? 0.01
    : 0.02";

        public const string TernaryArith1 = @"
(
    (
        (Kind matches 'Put') 
        ? (Strike - baseContractPrice)*Price_Multiplier 
        : (baseContractPrice - Strike)*Price_Multiplier
    ) - tradePrice
) * Multiplier * tradeQuantity";
        public const string TernaryArith2 = "(Barrier - UnderlyingPrice)/UnderlyingPrice between {0,0.01} ? Position*Price_Multiplier*totalPriceOffset:0";
        public const string TernaryArith3 = "(PricingParametersDerivativePricer_v1_autoPriceOffset ==null ?0:PricingParametersDerivativePricer_v1_autoPriceOffset )+ PricingParametersDerivativePricer_v1_manualPriceOffset";

        public const string NestedTernary1 = @"
(((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> -0.01) and 
(((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))<=0.03) 
    ? 0.01 
    : (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> 0.03) and 
      (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))<=0.10) 
       ? 0.05 
       : (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> 0.1) and 
         (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))<=0.225) 
         ? 0.15 
         : (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> 0.225) and 
           (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))<=0.4) 
           ? 0.3 
           : (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> 0.4) and 
             (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))<=0.6) 
             ? 0.5 
             : (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> 0.6) and 
               (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))<=0.775) 
               ? 0.7 
               : (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> 0.775) and 
                 (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))<=0.9) 
                 ? 0.85 
                 : (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))> 0.9) and 
                   (((PosSwimDelta / OptionEquivalentSplitPosition) < 0 ? T(System.Math).Abs((PosSwimDelta / OptionEquivalentSplitPosition)) : 1 - (PosSwimDelta / OptionEquivalentSplitPosition))<=0.97) 
                   ? 0.95 
                   : 0.99";
        public const string NestedTernary2 = @"
((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta) > -0.01) and 
((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)<=0.03) 
    ? 0.01 
    : ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)> 0.03) and 
      ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)<=0.10) 
        ? 0.05 
        : ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)> 0.1) and 
          ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)<=0.225) 
            ? 0.15 
            : ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)> 0.225) and 
              ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)<=0.4) 
                ? 0.3 
                : ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)> 0.4) and 
                  ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)<=0.6) 
                    ? 0.5 
                    : ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)> 0.6) and 
                      ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)<=0.775) 
                        ? 0.7 
                        : ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)> 0.775) and 
                          ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)<=0.9) 
                            ? 0.85 
                            : ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)> 0.9) and 
                              ((SwimDelta < 0 ? T(System.Math).Abs(SwimDelta) : 1 - SwimDelta)<=0.97) 
                                ? 0.95 
                                : 0.99";

        public const string LetGuard = @"
let
    ud = PosSwimDelta / OptionEquivalentSplitPosition,
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

        public const string DateExprTotalDays = "((Expiry_Date - T(DateTime).Today).TotalDays+1)";
        public const string TimeExprTotalExconds = "(BBO_gatewayTs - Tv_createTs).TotalSeconds";
        public const string ArithExpr1 = "(_5mAvgBidPO-impliedBidPriceOffset)/baseContractPrice";
        public const string ArithExpr2 = "(1.001 ^ ((Expiry_Date - DateTime.Today).TotalDays/360) - 1) * Position_Cash_Swim_Delta";
        public const string ArithExpr3 = "(Barrier - ULPx)";
        public const string ArithExpr4 = "(barrier/baseContractPrice) -1";
        public const string ArithExpr5 = "(Barrier-baseContractPrice)/baseContractPrice";
        public const string ArithExpr6 = "(BBO_ask - (Tv_tvAsk+Tv_tvBid)*0.5)*BBO_askQuantity*-1";
        public const string ArithExpr7 = "_5dAvgBidVO- impliedAskVolOffset";
        public const string ArithExpr8 = "(CalculatedColumn_impliedAskPriceOffset/PrimaryPriceAndGreeks_vega) + PrimaryPriceAndGreeks_strikeVolatility";
        public const string ArithExpr9 = "(CalculatedColumn_impliedBidPriceOffsetToSpot * 365 / (Instrument_expiryDate - T(System.DateTime).Now).TotalDays)/T(System.Math).Abs(CalculatedColumn_optSkewDelta)";
        public const string ArithExpr10 = "(CalculatedColumn_tv+(BidAskOffset_bidOffset+BidAskOffset_askOffset)/2-BBO_ask)/((-BidAskOffset_bidOffset+BidAskOffset_askOffset)/2)";
        public const string ArithExpr11 = "(impliedAskPriceOffset - impliedBidPriceOffset)/baseContractPrice";
        public const string ArithExpr12 = "(Position_Cash_Swim_Gamma/100)/-Position_Vol_Theta";
        public const string ArithExpr13 = "(Position_Delta_5_Risk*-0.075 + Position_Delta_15_Risk*-0.0375 + Position_Delta_30_Risk*-0.0225) + (Position_Delta_70_Risk*0.015 + Position_Delta_85_Risk*0.03 + Position_Delta_95_Risk*0.045)";
        public const string ArithExpr14 = "1/1";
        public const string ArithExpr15 = "1/Price_Multiplier";
        public const string ArithExpr16 = "2*Position_Vega";
        public const string ArithExpr17 = "T(System.Math).Abs(baseContractPrice - Barrier)/baseContractPrice";
        public const string ArithExpr18 = "T(System.Math).Max( (CalculatedColumn_profitAsk==null?0:CalculatedColumn_profitAsk) , (CalculatedColumn_profitBid==null?0:CalculatedColumn_profitBid) )";
        public const string ArithExpr19 = "T(System.Math).Min(T(System.Math).Min(NetPosition_netPosition , BBO_bidQuantity), 3000.0*Listing_lotSize)";
        public const string ArithExpr20 = "T(System.Math).Round(Barrier/1000,1)";
        public const string ArithExpr21 = "T(System.Math).Sqrt(T(System.Math).Abs(Position_Vol_Theta)*200/T(System.Math).Abs(Position_Cash_Swim_Gamma))";
        public const string ArithExpr22 = "CalculatedColumn_impliedBidPriceOffsetToSpot * 365 / (Instrument_expiryDate - T(System.DateTime).Now).TotalDays";
        public const string ArithExpr23 = "CallCurve";
        public const string ArithExpr24 = "(Statistic_LastValue - Statistic_PreviousCloseValue) * 100 / Statistic_PreviousCloseValue";
        public const string ArithExpr25 = "(swim_volatility + (surfaceName == 'HSI CS' ? -2*swim_volatility : 0))*100";
        public const string ArithExpr26 = "(swim_volatilityDifference + (underlying == 'HSI' ? -2*swim_volatilityDifference : 0))*100";
        public const string ArithExpr27 = "(timeCreated - T(System.DateTime).Now).TotalSeconds";
        public const string ArithExpr28 = "Amount / LimitAmount";
        public const string ArithExpr29 = "annualizedBidPOpctNormByDelta > 0.01";
        public const string ArithExpr30 = "(Tv_tvAsk - Tv_tvBid) / (BBO_ask - BBO_bid)";

        public const string BoolExpr1 = @"
(
    (
        (
            (impliedVolatilityOffset > 0.8 or impliedVolatilityOffset < -0.8) and 
            (
                (swimDelta < 0.95 and swimDelta > 0.05) or 
                (swimDelta < -0.05 and swimDelta > -0.95)
            )
        ) or 
        (
            (impliedVolatilityOffset > 0.4 or impliedVolatilityOffset < -0.4) and 
            (
                (swimDelta < 0.7 and swimDelta > 0.3) or 
                (swimDelta < -0.3 and swimDelta > -0.7)
            )
        )
    ) and 
    (signedTotalCredit > 400 or signedTotalCredit < -400)
)";
        public const string BoolExpr2 = @"
(
    (Underlying == 'CUS' or Underlying == 'UC') and 
    (signedCredit > 0.0015 or signedCredit < -0.0015 or tradeQuantity > 10)
) or (
    Underlying == 'CN' and 
    (signedCredit > 10 or signedCredit < -10 or tradeQuantity > 10)
) or (
    Underlying == 'CSA' and 
    (signedCredit > 0.01 or signedCredit < -0.01 or tradeQuantity > 10) and 
    Kind != 'Spot'
) or (
    Underlying == 'A50' and 
    (signedCredit > 0.01 or signedCredit < -0.01 or tradeQuantity > 10) and 
    Kind != 'Spot'
) or (
    Underlying == 'AMC' and 
    (signedCredit > 0.01 or signedCredit < -0.01 or tradeQuantity > 10) and 
    Kind != 'Spot'))
";

        public const string BoolExpr3 = "((Position == null or Position <= 0) and msg.StartsWith(' started')) and ts.Hour >= 10 and ((ts - T(System.DateTime).Now).TotalSeconds > -15)";
        public const string BoolExpr4 = @"
(   
    (WarrantAutomationInstrumentTradeDeflectionParameters_buyDeflectPercent and WarrantAutomationInstrumentTradeDeflectionParameters_sellDeflectPercent) and 
    (HitterParameters_bidOffset and HitterParameters_askOffset) and (QuoterParameters_bidOffset and QuoterParameters_askOffset) and 
    (T(System.Math).Abs(WarrantAutomationInstrumentPOState_totalPriceOffset - PricingParametersDerivativePricer_v1_totalPriceOffset) <= 0.001))
        ? ((PricingParametersDerivativePricer_v1_manualPriceOffset > 0 and PricingParametersDerivativePricer_v1_volatilityOffset > 0)
            ? true 
            : false) 
        : true
";
        public const string VarFilter1 = "((buyEnabled != #buyEnabled and sellEnabled != #sellEnabled) and (feedcode == '2800' or feedcode == '2828'))";



    }
}
