namespace DotnetMicroserviceArchitecture.DiscountAPI.Constants
{
    public class Route
    {
        #region DiscountController Routes Contant

        public const string HTTPGETORPOST_DISCOUNTS = "discounts";
        public const string HTTPGET_DISCOUNTS = "discounts/{id}";
        public const string HTTPGETBYCODE_DISCOUNTS = "discounts/GetByCode/{code}";

        #endregion
    }
}
