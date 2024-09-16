using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    public class CreateBookingRequestApiOut
    {
        public List<string>? offerLocations { get; set; }
    }

    public class BookingApiRequest
    {
        public UserRequest User { get; set; }
        public CreateBookingRequest CreateBooking { get; set; }
    }
}
