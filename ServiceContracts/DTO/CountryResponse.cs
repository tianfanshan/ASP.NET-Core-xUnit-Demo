using System;
using Entities;

namespace ServiceContracts.DTO
{
	public class CountryResponse
	{
		public Guid CountryID { get; set; }
		public string? CountryName { get; set; }


		//It compares the current object to another object of CountryResponse type and returns true, if both values are same, otherwise returns false
		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}

			if(obj.GetType() != typeof(CountryResponse))
			{
				return false;
			}

			CountryResponse countryResponse = (CountryResponse)obj;

			return CountryID == countryResponse.CountryID && CountryName == countryResponse.CountryName;
		}


		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
	}

	public static class CountryResponseExtensions
	{
		public static CountryResponse ToCountryResponse(this Country country)
		{
			return new CountryResponse() { CountryID = country.ContryID, CountryName = country.ContryName };
		}
	}
}
