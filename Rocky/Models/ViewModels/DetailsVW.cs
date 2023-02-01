namespace Rocky.Models.ViewModels
{
    public class DetailsVW
    {
        public DetailsVW()
        {
            Product = new Product();
        }
        public Product Product { get; set; }
        public bool ExistsInCart { get; set; }
    }
}
