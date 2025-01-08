using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{
    public class Payment
    {
        [Key]
        public int Id_Payment { get; set; }

        public int Id_Order {  get; set; }
        public virtual Order? Order { get; set; }
        [Required(ErrorMessage = "Metoda de plata este obligatorie")]
        public string? Method { get; set; }
        [Required(ErrorMessage = "Adresa de livrare este obligatorie")]
        public string? Address { get; set; }

        public DateTime Order_Date { get; set; }
    }
}
