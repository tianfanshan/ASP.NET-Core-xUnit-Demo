using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
	public class PersonAddRequest
	{
		[Required(ErrorMessage = "Person Name can not be blank")]
		public string? PersonName { get; set; }

		[Required(ErrorMessage = "Email can not be blank")]
		[EmailAddress(ErrorMessage = "Email value should be a valid email")]
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public GenderOptions? Gender { get; set; }
		public Guid? CountryID { get; set; }
		public string? Address { get; set; }
		public bool ReceiveNewsLetters { get; set; }

		public Person ToPerson()
		{
			return new Person() 
			{
				PersonName = PersonName,
				Email = Email,
				DateOfBirth = DateOfBirth,
				Gender = Gender.ToString(),
				CountryID = CountryID, 
				Address = Address,
				ReceiveNewsLetters = ReceiveNewsLetters
			};
		}
	}
}
