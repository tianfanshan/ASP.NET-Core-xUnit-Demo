﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
	public class PersonResponse
	{
		public Guid? PersonID { get; set; }
		public string? PersonName { get; set; }
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Gender { get; set; }
		public Guid? CountryID { get; set; }
		public string? Country { get; set; }
		public string? Address { get; set; }
		public bool ReceiveNewsLetters { get; set; }
		public double? Age { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj == null) return false;

			if (obj.GetType() != typeof(PersonResponse)) return false;

			PersonResponse person = (PersonResponse)obj;

			return PersonID == person.PersonID &&
				PersonName == person.PersonName &&
				Email == person.Email &&
				DateOfBirth == person.DateOfBirth &&
				Gender == person.Gender &&
				CountryID == person.PersonID &&
				Address == person.Address &&
				ReceiveNewsLetters == person.ReceiveNewsLetters;
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return $"Person ID: {PersonID}, " +
				$"Person Name: {PersonName}," +
				$" Email: {Email}," +
				$" Date of birth: {DateOfBirth?.ToString("dd MM yyyy")}," +
				$" Gender: {Gender}," +
				$" Country ID: {CountryID}," +
				$" Address: {Address}" +
				$" Receive news letters: {ReceiveNewsLetters}";
		}

		public PersonUpdateRequest ToPersonUpdateRequest()
		{
			return new PersonUpdateRequest()
			{
				PersonID = PersonID,
				PersonName = PersonName,
				Email = Email,
				DateOfBirth = DateOfBirth,
				Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
				CountryID = CountryID,
				Address = Address,
				ReceiveNewsLetters = ReceiveNewsLetters,
			};
		}
	}

	public static class PersonExtensions
	{
		public static PersonResponse personResponse(this Person person)
		{
			return new PersonResponse()
			{
				PersonID = person.PersonID,
				PersonName = person.PersonName,
				Email = person.Email,
				DateOfBirth = person.DateOfBirth,
				Gender = person.Gender,
				CountryID = person.PersonID,
				Address = person.Address,
				ReceiveNewsLetters = person.ReceiveNewsLetters,
				Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
			};
		}
	}
}
