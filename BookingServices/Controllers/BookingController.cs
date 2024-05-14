using BookingServices.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : Controller
    {
   
        private static List<Booking> bookingsList = new List<Booking>();
        public Booking booking = new Booking();
        private const int MaxSimultaneousBookings = 4;
        private static readonly TimeSpan BusinessStartTime = new TimeSpan(9, 0, 0);
        private static readonly TimeSpan BusinessEndTime = new TimeSpan(17, 0, 0);
       


        [HttpPost]
        public IActionResult Book([FromBody] BookingRequest request)
        {
            // Check if booking time is within business hours
            TimeSpan bookingTime;
            if (!TimeSpan.TryParse(request.bookingTime, out bookingTime))
            {
                return BadRequest("Booking time is invalid");
            }

            // Check if name is non-empty
            if (string.IsNullOrWhiteSpace(request.name))
            {
                return BadRequest("Name cannot be empty.");
            }

            if (bookingTime < BusinessStartTime || bookingTime > BusinessEndTime)
            {
                return BadRequest("Booking time is outside of business hours.");
            }


            // Check if all settlements at a booking time are reserved
            if (bookingsList.Where(c => c.bookingTime == request.bookingTime).Count() >= MaxSimultaneousBookings) //&& bookings.Select(c=> c.Key == request.bookingTime).Count() >= MaxSimultaneousBookings)
            {                
                 return Conflict("All settlements at the booking time are reserved.");
            }            
                       

            // Generate booking Id and store      

            booking.bookingId = Guid.NewGuid();
            booking.name    = request.name;
            booking.bookingTime = request.bookingTime;

            bookingsList.Add(booking);
            

            // Respond with OK and booking Id
            return Ok(new { BookingId = booking.bookingId });
        }
        
    }
}