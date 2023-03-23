using ClassLibrary1;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
	public class CountriesServiceTest
	{
		private readonly ICountriesService _contriesService;

		public CountriesServiceTest()
		{
			_contriesService = new CountriesService();
		}

		#region AddCountry
		//When CountryAddRequest is null, it should throw ArgumentNullException
		[Fact]
		public void AddCountry_NullCountry()
		{
			//Arrange
			CountryAddRequest? request = null;

			//Assert
			Assert.Throws<ArgumentNullException>(() =>
			{
				//Act
				_contriesService.AddCountry(request);
			});
		}


		//When the CountryName is null, it should throw ArgumentException
		[Fact]
		public void AddCountry_CountryNameIsNull()
		{
			//Arrange
			CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

			//Assert
			Assert.Throws<ArgumentException>(() =>
			{
				//Act
				_contriesService.AddCountry(request);
			});
		}


		//When the CountryName is duplicate, it should throw ArgumentException
		[Fact]
		public void AddCountry_DuplicateCountryName()
		{
			//Arrange
			CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "Spain" };
			CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "Spain" };

			//Assert
			Assert.Throws<ArgumentException>(() =>
			{
				//Act
				_contriesService.AddCountry(request1);
				_contriesService.AddCountry(request2);
			});
		}


		//When you supply proper country name, it should insert (add) the country to the existing list of countries
		[Fact]
		public void AddCountry_ProperCountryDetails()
		{
			//Arrange
			CountryAddRequest? request = new CountryAddRequest() { CountryName = "Spain" };

			//Act
			CountryResponse response = _contriesService.AddCountry(request);
			List<CountryResponse> countriesFromGetAllCountires = _contriesService.GetAllCountries();

			//Assert
			Assert.True(response.CountryID != Guid.Empty);
			Assert.Contains(response, countriesFromGetAllCountires);
		}
		#endregion


		#region GetAllCountries
		//The list of countries should be empty by default (before adding any countries)
		[Fact]
		public void GetAllCountries_EmptyList()
		{
			//Acts
			List<CountryResponse> actualCountryResponseList = _contriesService.GetAllCountries();

			//Assert
			Assert.Empty(actualCountryResponseList);
		}

		[Fact]
		public void GetCountries_AddFewCountires()
		{
			//Arrange
			List<CountryAddRequest> countryRequestList = new List<CountryAddRequest> {
				new CountryAddRequest() { CountryName = "Spain" },
				new CountryAddRequest() { CountryName = "China"}
			};

			//Acts
			List<CountryResponse> countryResponses = new List<CountryResponse>();
			foreach (CountryAddRequest countryAddRequest in countryRequestList)
			{
				countryResponses.Add(_contriesService.AddCountry(countryAddRequest));
			}

			List<CountryResponse> actualCountryResponseList = _contriesService.GetAllCountries();
			foreach (CountryResponse countryResponse in countryResponses)
			{
				//Assert
				Assert.Contains(countryResponse, actualCountryResponseList);
			}
		}
		#endregion


		#region GetCountryByCountryId
		[Fact]
		public void GetCountryByCountryId_NullCountryId()
		{
			//Arrange
			Guid? countryId = null;

			//Act
			CountryResponse? countryResponse = _contriesService.GetCountryByCountryId(countryId);

			//Assert
			Assert.Null(countryResponse);
		}

		[Fact]
		public void GetCountryByCountryId_ValidCountryId()
		{
			//Arrange
			CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "China"};
			CountryResponse countryResponseFromAdd = _contriesService.AddCountry(countryAddRequest);

			//Act
			CountryResponse? countryResponseFromGet = _contriesService.GetCountryByCountryId(countryResponseFromAdd.CountryID);

			//Assert
			Assert.Equal(countryResponseFromAdd, countryResponseFromGet);
		}
		#endregion
	}
}
