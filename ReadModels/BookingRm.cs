namespace Flights.ReadModels
{
    public record BookingRm(
        Guid FlightId,
        string Airline,
        string price,
        TimePlaceRm Arrival,
        TimePlaceRm Departure,
        int NumberOfBookedSeats,
        string PassengerEmail);
    
}
