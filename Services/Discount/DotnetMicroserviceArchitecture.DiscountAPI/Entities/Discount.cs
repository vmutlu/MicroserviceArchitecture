using System;

namespace DotnetMicroserviceArchitecture.DiscountAPI.Entities
{
    //postgresql db de best practive olarak tablo isimleri küçük harfle başladığı için
    [Dapper.Contrib.Extensions.Table("discount")]
    public class Discount
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public DateTime InsertedDate { get; set; } = DateTime.UtcNow.Date;
    }
}
