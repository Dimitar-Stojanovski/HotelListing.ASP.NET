﻿namespace HotelListing.API.Exceptions
{
    public class BadRequestException:ApplicationException
    {
        public BadRequestException(string name):base(name)
        {

        }
    }
}
