using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Flights.Data;
using Flights.ReadModels;
using Flights.Dtos;
using Flights.Domain.Errors;

namespace Flights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly Entities _entities;

        public BookingController(Entities entities)
        {
            _entities = entities;   
        }

        [HttpGet("{email}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(IEnumerable<BookingRm>), 200)]

        public ActionResult<IEnumerable<BookingRm>> List(string email)
        {
            var bookings = _entities.Flights.ToArray()
                .SelectMany(flight => flight.Bookings
                    .Where(booking => booking.PassengerEmail == email)
                    .Select(booking => new BookingRm(
                        flight.Id,
                        flight.Airline,
                        flight.Price.ToString(),
                        new TimePlaceRm(flight.Arrival.Place, flight.Arrival.Time),
                        new TimePlaceRm(flight.Departure.Place, flight.Departure.Time),
                        booking.NumberOfSeats,
                        email
                        )));

            return Ok(bookings);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Cancel(BookDto dto)
        {
            var flight = _entities.Flights.Find(dto.FlightId);

            var error = flight?.CancelBooking(dto.PassengerEmail, dto.NumberOfSeats);

            if (error == null)
            {
                _entities.SaveChanges();
                return NoContent();
            }

            if (error is NotFoundError)
                return NotFound();

            throw new Exception($"The error of type: {error.GetType().Name} occured while canceling the booking made by {dto.PassengerEmail}");
        }

    }
}
