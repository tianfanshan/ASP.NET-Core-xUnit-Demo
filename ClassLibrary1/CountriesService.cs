using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace ClassLibrary1
{
	public class CountriesService : ICountriesService
	{
		private readonly List<Country> _countries;

		public CountriesService()
		{
			_countries = new List<Country>();
		}


		public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
		{
			if (countryAddRequest == null)
			{
				throw new ArgumentNullException(nameof(countryAddRequest));
			}

			if(countryAddRequest.CountryName == null)
			{
				throw new ArgumentException(nameof(countryAddRequest.CountryName));
			}

			if(_countries.Where(temp => temp.ContryName ==  countryAddRequest.CountryName).Count() > 0)
			{
				throw new ArgumentException("Given country name already exists");
			}

			Country country = countryAddRequest.ToCountry();

			country.ContryID = Guid.NewGuid();

			_countries.Add(country);

			return country.ToCountryResponse();
		}


		public List<CountryResponse> GetAllCountries()
		{
			return _countries.Select(country => country.ToCountryResponse()).ToList();
		}


		public CountryResponse? GetCountryByCountryId(Guid? id)
		{
			if(id == null)
			{
				return null;
			}

			Country? countryFromList = _countries.FirstOrDefault(temp => temp.ContryID == id);

			if(countryFromList == null)
			{
				return null;
			}

			return countryFromList.ToCountryResponse();
		}
	}
}