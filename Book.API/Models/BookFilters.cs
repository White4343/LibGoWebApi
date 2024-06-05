namespace Book.API.Models
{
    public class BookFilters
    {
        public bool? AlphabeticalOrder { get; set; }
        public bool? IsAlphabeticalOrderReversed { get; set; }
        public bool? IsFree { get; set; }
        public bool? DateNewest { get; set; }
        public bool? DateOldest { get; set; }
        public int? PriceLowest { get; set; }
        public int? PriceHighest { get; set; }
        public bool? FromLowestPrice { get; set; }
        public bool? FromHighestPrice { get; set; }
    }
}