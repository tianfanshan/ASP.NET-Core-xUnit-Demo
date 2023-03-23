using ServiceContracts.DTO;

namespace ServiceContracts
{
	public interface ICountriesService
	{
		CountryResponse AddCountry(CountryAddRequest? country);

		List<CountryResponse> GetAllCountries();

		CountryResponse? GetCountryByCountryId(Guid? id);
	}
}