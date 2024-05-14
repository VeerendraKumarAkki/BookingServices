using BookingServices.Controllers;
using BookingServices.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Tests
{
    public class BookingControllerTests
    {
        private readonly BookingController _controller;

        public BookingControllerTests()
        {
            _controller = new BookingController();
        }

        [Fact]
        public void Book_InvalidTimeFormat_ReturnsBadRequest()
        {
            // Arrange
            var request = new BookingRequest
            {
                bookingTime = "invalid time",
                name = "John Doe"
            };

            // Act
            var result = _controller.Book(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Booking time is invalid", badRequestResult.Value);
        }

        [Fact]
        public void Book_EmptyName_ReturnsBadRequest()
        {
            // Arrange
            var request = new BookingRequest
            {
                bookingTime = "10:00",
                name = ""
            };

            // Act
            var result = _controller.Book(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Name cannot be empty.", badRequestResult.Value);
        }

        [Fact]
        public void Book_OutsideBusinessHours_ReturnsBadRequest()
        {
            // Arrange
            var request = new BookingRequest
            {
                bookingTime = "18:00",
                name = "John Doe"
            };

            // Act
            var result = _controller.Book(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Booking time is outside of business hours.", badRequestResult.Value);
        }

        [Fact]
        public void Book_ConflictingBooking_ReturnsConflict()
        {
            // Arrange
            var request1 = new BookingRequest
            {
                bookingTime = "10:00",
                name = "John Doe"
            };

            var request2 = new BookingRequest
            {
                bookingTime = "10:00",
                name = "Jane Doe"
            };

            var request3 = new BookingRequest
            {
                bookingTime = "10:00",
                name = "Jim Doe"
            };

            var request4 = new BookingRequest
            {
                bookingTime = "10:00",
                name = "Jake Doe"
            };

            var request5 = new BookingRequest
            {
                bookingTime = "10:00",
                name = "Jill Doe"
            };

            _controller.Book(request1);
            _controller.Book(request2);
            _controller.Book(request3);
            _controller.Book(request4);

            // Act
            var result = _controller.Book(request5);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("All settlements at the booking time are reserved.", conflictResult.Value);
        }

        [Fact]
        public void Book_ValidBooking_ReturnsOk()
        {
            // Arrange
            var request = new BookingRequest
            {
                bookingTime = "11:00",
                name = "John Doe"
            };

            // Act
            var result = _controller.Book(request);

            // Assert
             Assert.IsType<OkObjectResult>(result);           
        }
    }
}
