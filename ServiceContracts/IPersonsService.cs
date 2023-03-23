using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;

namespace ServiceContracts
{
	public interface IPersonsService
	{
		PersonResponse AddPerson(PersonAddRequest? personAddRequest);

		List<PersonResponse> GetAllPersons();

		PersonResponse? GetPersonByPersonID(Guid? id);

		List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

		List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

		PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest);

		/// <summary>
		/// Deletes a person based on the given person id
		/// </summary>
		/// <param name="guid">PersonID to delete</param>
		/// <returns>Returns true, if the deletion is successful; otherwise false</returns>
		bool DeletePerson(Guid? guid);
	}
}
