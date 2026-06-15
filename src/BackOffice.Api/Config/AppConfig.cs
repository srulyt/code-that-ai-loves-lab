namespace BackOffice.Api.Config
{
    // Grab-bag of constants. Some thresholds are duplicated as literals inside services
    // and helpers anyway, so changing a number here does NOT change it everywhere.
    public static class AppConfig
    {
        public const decimal GoldDiscountRate = 0.10m;       // 10% off for Gold
        public const decimal CorporateDiscountRate = 0.10m;  // 10% off corporate agreement

        public const decimal StandardShippingCost = 7.95m;
        public const decimal ExpressShippingCost = 19.95m;
        public const decimal FragileSurcharge = 4.50m;

        // Gold free-express threshold. NOTE: several files hardcode 150 directly instead.
        public const decimal GoldFreeExpressThreshold = 150.00m;

        public const int ReturnWindowDays = 30;
        public const int CorporateReturnWindowDays = 60;
    }
}
