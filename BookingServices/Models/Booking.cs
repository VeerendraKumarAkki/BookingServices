namespace BookingServices.Models
{
    public class Booking
    {
        public Guid bookingId  { get; set; }
        public string name { get; set; }      
        public string bookingTime { get; set; }
    }
}
