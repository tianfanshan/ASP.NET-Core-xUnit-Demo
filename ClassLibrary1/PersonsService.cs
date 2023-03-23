using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
	public class PersonsService : IPersonsService
	{

		private readonly List<Person> _persons;
		private readonly ICountriesService _countriesService;

		public PersonsService()
		{
			_persons = new List<Person>();
			_countriesService = new CountriesService();
		}


		private PersonResponse ConvertPersonToPersonResponse(Person person)
		{
			PersonResponse personResponse = person.personResponse();
			personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryID)?.CountryName;

			return personResponse;
		}


		public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
		{

			if (personAddRequest == null)
			{
				throw new ArgumentNullException(nameof(PersonAddRequest));
			}

			ValidationHelper.ModelValidation(personAddRequest);

			Person person = personAddRequest.ToPerson();

			person.PersonID = Guid.NewGuid();

			_persons.Add(person);

			return ConvertPersonToPersonResponse(person);
		}


		public List<PersonResponse> GetAllPersons()
		{
			return _persons.Select(temp => temp.personResponse()).ToList();
		}


		public PersonResponse? GetPersonByPersonID(Guid? id)
		{
			if (id == null)
			{
				return null;
			}

			Person? person = _persons.FirstOrDefault(temp => temp.PersonID == id);
			if (person == null)
			{
				return null;
			}

			return person.personResponse();
		}

		public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
		{
			List<PersonResponse> allPersons = GetAllPersons();
			List<PersonResponse> matchingPersons = allPersons;

			if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchString))
			{
				return matchingPersons;
			}

			switch (searchBy)
			{
				case nameof(Person.PersonName):
					matchingPersons =
						allPersons.Where(
							temp => (!string.IsNullOrEmpty(temp.PersonName) ?
							temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
							: true)).ToList();
					break;

				case nameof(Person.Email):
					matchingPersons =
						allPersons.Where(
							temp => (!string.IsNullOrEmpty(temp.Email) ?
							temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
							: true)).ToList();
					break;

				case nameof(Person.DateOfBirth):
					matchingPersons =
						allPersons.Where(
							temp => (temp.DateOfBirth != null) ?
							temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)
							: true).ToList();
					break;

				case nameof(Person.Gender):
					matchingPersons =
						allPersons.Where(
							temp => (!string.IsNullOrEmpty(temp.Gender) ?
							temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)
							: true)).ToList();
					break;

				case nameof(Person.CountryID):
					matchingPersons =
						allPersons.Where(
							temp => (!string.IsNullOrEmpty(temp.Country) ?
							temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase)
							: true)).ToList();
					break;

				case nameof(Person.Address):
					matchingPersons =
						allPersons.Where(
							temp => (!string.IsNullOrEmpty(temp.Address) ?
							temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)
							: true)).ToList();
					break;

				default: matchingPersons = allPersons; break;
			}

			return matchingPersons;
		}

		public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
		{
			if (string.IsNullOrEmpty(sortBy))
			{
				return allPersons;
			}

			List<PersonResponse> sortedPersons = (sortBy, sortOrder)
				switch
			{
				(nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
				(nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),
				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
				(nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
				_ => allPersons
			};

			return sortedPersons;
		}

		public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
		{
			if (personUpdateRequest == null)
			{
				throw new ArgumentNullException(nameof(Person));
			}

			//validation
			ValidationHelper.ModelValidation(personUpdateRequest);

			//get matching person object to update
			Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
			
			if(matchingPerson == null)
			{
				throw new ArgumentException("Given person id doesn't exist");
			}

			//update all details
			matchingPerson.PersonName = personUpdateRequest.PersonName;
			matchingPerson.Email = personUpdateRequest.Email;
			matchingPerson.Address = personUpdateRequest.Address;
			matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
			matchingPerson.CountryID = personUpdateRequest.CountryID;
			matchingPerson.Gender = personUpdateRequest.Gender.ToString();
			matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

			return matchingPerson.personResponse();
		}

		public bool DeletePerson(Guid? guid)
		{
			if(guid == null)
			{
				throw new ArgumentNullException(nameof(guid));
			}

			Person? person = _persons.FirstOrDefault(temp => temp.PersonID == guid);
			if(person == null)
			{
				return false;
			}

			_persons.Remove(person);
			return true;
		}
	}
}
