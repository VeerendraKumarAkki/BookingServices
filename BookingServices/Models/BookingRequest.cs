using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookingServices.Models
{
    public class BookingRequest 
    {
        [Required]
        public string bookingTime {  get; set; }  
        public string name { get; set; }    
    }
}
