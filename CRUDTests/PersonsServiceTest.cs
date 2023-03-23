using ServiceContracts;
using System;
using System.Collections.Generic;
using Xunit;
using Services;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using ClassLibrary1;
using Xunit.Abstractions;
using Xunit.Sdk;
using Entities;

namespace CRUDTests
{
	public class PersonsServiceTest
	{
		private readonly IPersonsService _personsService;
		private readonly ICountriesService _countriesService;
		private readonly ITestOutputHelper _testOutputHelper;

		public PersonsServiceTest(ITestOutputHelper testOutputHelper)
		{
			_personsService = new PersonsService();
			_countriesService = new CountriesService();
			_testOutputHelper = testOutputHelper;
		}

		#region AddPerson
		[Fact]
		public void AddPerson_NullPerson()
		{
			//Arrange
			PersonAddRequest? personAddRequest = null;

			//Act & assert
			Assert.Throws<ArgumentNullException>(() =>
			{
				_personsService.AddPerson(personAddRequest);
			});
		}

		[Fact]
		public void AddPerson_PersonNameIsNull()
		{
			//Arrange
			PersonAddRequest? personAddRequest = new PersonAddRequest { PersonName = null };

			//Act & assert
			Assert.Throws<ArgumentException>(() =>
			{
				_personsService.AddPerson(personAddRequest);
			});
		}


		[Fact]
		public void AddPerson_ProperPersonDetails()
		{
			//Arrange
			PersonAddRequest? personAddRequest = new PersonAddRequest()
			{
				PersonName = "Pepito",
				Email = "pepito@gmail.com",
				Address = "calle jose 123",
				CountryID = Guid.NewGuid(),
				Gender = GenderOptions.Male,
				DateOfBirth = DateTime.Parse("2000-01-01"),
				ReceiveNewsLetters = true
			};

			//Act
			PersonResponse personResponseFromAdd = _personsService.AddPerson(personAddRequest);
			List<PersonResponse> personResponses = _personsService.GetAllPersons();

			//Assert
			Assert.True(personResponseFromAdd.PersonID != null);
			Assert.Contains(personResponseFromAdd, personResponses);
		}
		#endregion

		#region GetPersonByPersonID
		[Fact]
		public void GetPersonByPersonID_NullPersonID()
		{
			//Arrange
			Guid? personID = Guid.Empty;

			//Act
			PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);

			//Assert
			Assert.Null(personResponse);
		}

		[Fact]
		public void GetPersonByPersonID_ValidPersonID()
		{
			//Arrange
			CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Canada" };
			CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);

			//Act
			PersonAddRequest personAddRequest = new PersonAddRequest()
			{
				PersonName = "Pepito",
				Email = "pepito@gmail.com",
				Address = "Calle ruben vela",
				CountryID = countryResponse.CountryID,
				DateOfBirth = DateTime.Parse("1999-02-01"),
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = false
			};
			PersonResponse personResponseFromAdd = _personsService.AddPerson(personAddRequest);
			PersonResponse personResponseFromGet = _personsService.GetPersonByPersonID(personResponseFromAdd.PersonID);

			//Assert
			Assert.Equal(personResponseFromAdd, personResponseFromGet);

		}
		#endregion

		#region GetAllPersons
		[Fact]
		public void GetAllPersons_EmptyList()
		{
			//Act
			List<PersonResponse> personResponsesList = _personsService.GetAllPersons();
		
			//Assert
			Assert.Empty(personResponsesList);
		}


		[Fact]
		public void GetAllPersons_AddFewPersons()
		{
			//Arrange
			CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "India" };

			CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
			CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

			PersonAddRequest personAddRequest1 = new PersonAddRequest()
			{
				PersonName = "Pepito",
				Address = "casa de pepito",
				CountryID = countryResponse1.CountryID, 
				DateOfBirth = DateTime.Parse("1998-10-03"), 
				Email = "pepito@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};
			
			PersonAddRequest personAddRequest2 = new PersonAddRequest()
			{
				PersonName = "Pepapi",
				Address = "casa de Pepapi",
				CountryID = countryResponse2.CountryID, 
				DateOfBirth = DateTime.Parse("1928-11-11"), 
				Email = "pepapi@gmail.com",
				Gender = GenderOptions.Female,
				ReceiveNewsLetters = false
			};
			
			PersonAddRequest personAddRequest3 = new PersonAddRequest()
			{
				PersonName = "Antonio",
				Address = "casa de Antonio",
				CountryID = countryResponse1.CountryID, 
				DateOfBirth = DateTime.Parse("1968-7-12"), 
				Email = "antonio@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};

			List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };
			List<PersonResponse> personResponses = new List<PersonResponse>();


			foreach(PersonAddRequest personAddRequest in personAddRequests)
			{
				PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
				personResponses.Add(personResponse);
			}


			_testOutputHelper.WriteLine("Expected:");
			foreach(PersonResponse personResponse in personResponses)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}


			//Act
			List<PersonResponse> personResponsesFromService = _personsService.GetAllPersons();

			_testOutputHelper.WriteLine("Actual:");
			foreach (PersonResponse personResponse in personResponsesFromService)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}


			//Assert
			foreach (PersonResponse personResponse in personResponses) 
			{
				Assert.Contains(personResponse, personResponsesFromService);
			}


		}
		#endregion

		#region GetFilteredPersons
		//If the search text is empty and search by is "PersonName", it should return all persons
		[Fact]
		public void GetFilteredPersons_EmptySearchText()
		{
			//Arrange
			CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "India" };

			CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
			CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

			PersonAddRequest personAddRequest1 = new PersonAddRequest()
			{
				PersonName = "Pepito",
				Address = "casa de pepito",
				CountryID = countryResponse1.CountryID,
				DateOfBirth = DateTime.Parse("1998-10-03"),
				Email = "pepito@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};

			PersonAddRequest personAddRequest2 = new PersonAddRequest()
			{
				PersonName = "Pepapi",
				Address = "casa de Pepapi",
				CountryID = countryResponse2.CountryID,
				DateOfBirth = DateTime.Parse("1928-11-11"),
				Email = "pepapi@gmail.com",
				Gender = GenderOptions.Female,
				ReceiveNewsLetters = false
			};

			PersonAddRequest personAddRequest3 = new PersonAddRequest()
			{
				PersonName = "Antonio",
				Address = "casa de Antonio",
				CountryID = countryResponse1.CountryID,
				DateOfBirth = DateTime.Parse("1968-7-12"),
				Email = "antonio@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};

			List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };
			List<PersonResponse> personResponses = new List<PersonResponse>();


			foreach (PersonAddRequest personAddRequest in personAddRequests)
			{
				PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
				personResponses.Add(personResponse);
			}


			_testOutputHelper.WriteLine("Expected:");
			foreach (PersonResponse personResponse in personResponses)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}


			//Act
			List<PersonResponse> personResponsesFromSearch = _personsService.GetFilteredPersons(nameof(Person.PersonName),"");

			_testOutputHelper.WriteLine("Actual:");
			foreach (PersonResponse personResponse in personResponsesFromSearch)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}


			//Assert
			foreach (PersonResponse personResponse in personResponses)
			{
				Assert.Contains(personResponse, personResponsesFromSearch);
			}


		}


		//First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
		[Fact]
		public void GetFilteredPersons_SearchByPersonName()
		{
			//Arrange
			CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "India" };

			CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
			CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

			PersonAddRequest personAddRequest1 = new PersonAddRequest()
			{
				PersonName = "Pepito",
				Address = "casa de pepito",
				CountryID = countryResponse1.CountryID,
				DateOfBirth = DateTime.Parse("1998-10-03"),
				Email = "pepito@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};

			PersonAddRequest personAddRequest2 = new PersonAddRequest()
			{
				PersonName = "Pepapi",
				Address = "casa de Pepapi",
				CountryID = countryResponse2.CountryID,
				DateOfBirth = DateTime.Parse("1928-11-11"),
				Email = "pepapi@gmail.com",
				Gender = GenderOptions.Female,
				ReceiveNewsLetters = false
			};

			PersonAddRequest personAddRequest3 = new PersonAddRequest()
			{
				PersonName = "Antonio",
				Address = "casa de Antonio",
				CountryID = countryResponse1.CountryID,
				DateOfBirth = DateTime.Parse("1968-7-12"),
				Email = "antonio@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};

			List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };
			List<PersonResponse> personResponses = new List<PersonResponse>();


			foreach (PersonAddRequest personAddRequest in personAddRequests)
			{
				PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
				personResponses.Add(personResponse);
			}


			_testOutputHelper.WriteLine("Expected:");
			foreach (PersonResponse personResponse in personResponses)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}


			//Act
			List<PersonResponse> personResponsesFromSearch = _personsService.GetFilteredPersons(nameof(Person.PersonName), "pe");

			_testOutputHelper.WriteLine("Actual:");
			foreach (PersonResponse personResponse in personResponsesFromSearch)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}


			//Assert
			foreach (PersonResponse personResponse in personResponses)
			{
				if(personResponse.PersonName != null)
				{
					if (personResponse.PersonName.Contains("pe", StringComparison.OrdinalIgnoreCase))
					{
						Assert.Contains(personResponse, personResponsesFromSearch);
					}
				}
			}
		}
		#endregion

		#region GetSortedPersons
		//When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
		[Fact]
		public void GetSortedPersons()
		{
			//Arrange
			CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
			CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "India" };

			CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
			CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

			PersonAddRequest personAddRequest1 = new PersonAddRequest()
			{
				PersonName = "Pepito",
				Address = "casa de pepito",
				CountryID = countryResponse1.CountryID,
				DateOfBirth = DateTime.Parse("1998-10-03"),
				Email = "pepito@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};

			PersonAddRequest personAddRequest2 = new PersonAddRequest()
			{
				PersonName = "Pepapi",
				Address = "casa de Pepapi",
				CountryID = countryResponse2.CountryID,
				DateOfBirth = DateTime.Parse("1928-11-11"),
				Email = "pepapi@gmail.com",
				Gender = GenderOptions.Female,
				ReceiveNewsLetters = false
			};

			PersonAddRequest personAddRequest3 = new PersonAddRequest()
			{
				PersonName = "Antonio",
				Address = "casa de Antonio",
				CountryID = countryResponse1.CountryID,
				DateOfBirth = DateTime.Parse("1968-7-12"),
				Email = "antonio@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};

			List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };
			List<PersonResponse> personResponses = new List<PersonResponse>();


			foreach (PersonAddRequest personAddRequest in personAddRequests)
			{
				PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
				personResponses.Add(personResponse);
			}


			_testOutputHelper.WriteLine("Expected:");
			foreach (PersonResponse personResponse in personResponses)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}

			List<PersonResponse> allPersons = _personsService.GetAllPersons();

			//Act
			List<PersonResponse> personResponsesFromSort = _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

			_testOutputHelper.WriteLine("Actual:");
			foreach (PersonResponse personResponse in personResponsesFromSort)
			{
				_testOutputHelper.WriteLine(personResponse.ToString());
			}

			personResponses = personResponses.OrderByDescending(temp => temp.PersonName).ToList();


			//Assert
			for(int i = 0; i < personResponses.Count; i++)
			{
				Assert.Equal(personResponses[i], personResponsesFromSort[i]);
			}
		}
		#endregion

		#region UpdatePerson
		//When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
		[Fact]
		public void UpdatePerson_NullPerson()
		{
			//Arrange
			PersonUpdateRequest? personUpdateRequest = null;

			//Assert
			Assert.Throws<ArgumentNullException>(() => {
				//Act
				_personsService.UpdatePerson(personUpdateRequest);
				});
		}


		//When we supply invalid person id, it should throw ArgumentException
		[Fact]
		public void UpdatePerson_InvalidPersonID()
		{
			//Arrange
			PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid()};

			//Assert
			Assert.Throws<ArgumentException>(() => {
				//Act
				_personsService.UpdatePerson(personUpdateRequest);
			});
		}


		//When PersonName is null, it should throw ArgumentException
		[Fact]
		public void UpdatePerson_NullPersonName()
		{
			//Arrange
			CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
			CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

			PersonAddRequest personAddRequest = new PersonAddRequest() 
			{
				PersonName = "Jason",
				CountryID = countryResponse.CountryID,
				Email = "jason@gmail.com",
				Address = "casa de jason",
				Gender = GenderOptions.Male
			};
			PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
			PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
			personUpdateRequest.PersonName = null;


			//Assert
			Assert.Throws<ArgumentException>(() => {
				//Act
				_personsService.UpdatePerson(personUpdateRequest);
			});
		}


		//First add a new person and try to update the person name and email
		[Fact]
		public void UpdatePerson_PersonFullDetailsUpdation()
		{
			//Arrange
			CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
			CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

			PersonAddRequest personAddRequest = new PersonAddRequest()
			{
				PersonName = "Jason",
				CountryID = countryResponse.CountryID,
				Address = "Abc road",
				DateOfBirth = DateTime.Parse("2000-01-01"),
				Email = "jason@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};
			PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
			PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
			personAddRequest.PersonName = "Shan";
			personAddRequest.Email = "shan@gmail.com";

			PersonResponse? personResponseFromUpdate = _personsService.UpdatePerson(personUpdateRequest);

			PersonResponse? personResponseFromGet = _personsService.GetPersonByPersonID(personResponseFromUpdate.PersonID);

			//Assert
			Assert.Equal(personResponseFromGet, personResponseFromUpdate);
		}
		#endregion

		#region DeletePerson
		//If you supply an invalid PersonID, it should return false
		[Fact]
		public void DeletePerson_ValidPersonID()
		{
			//Arrange
			CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "USA" };
			CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

			PersonAddRequest personAddRequest = new PersonAddRequest() 
			{
				PersonName = "Jason",
				Address = "address",
				CountryID = countryResponse.CountryID,
				DateOfBirth = Convert.ToDateTime("2000-01-01"),
				Email = "jason@gmail.com",
				Gender = GenderOptions.Male,
				ReceiveNewsLetters= true
			};

			PersonResponse personResponse = _personsService.AddPerson(personAddRequest);


			//Act
			bool IsDeleted = _personsService.DeletePerson(personResponse.PersonID);

			//Assert
			Assert.True(IsDeleted);
		}

		//If you supply an valid PersonID, it should return true
		[Fact]
		public void DeletePerson_InvalidPersonID()
		{
			//Act
			bool IsDeleted = _personsService.DeletePerson(Guid.NewGuid());

			//Assert
			Assert.False(IsDeleted);
		}
		#endregion
	}
}





















