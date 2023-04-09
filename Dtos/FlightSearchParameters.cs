using System.ComponentModel;

namespace Flights.Dtos
{
    public record FlightSearchParameters(

        
        DateTime? FromDate,

        
        DateTime? ToDate,

        
        string? From,

        
        string? Destination,

        
        int? NumberOfPassengers
        
        );
    
}
