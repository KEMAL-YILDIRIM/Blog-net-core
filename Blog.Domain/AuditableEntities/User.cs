﻿using Blog.Domain.CrossCuttingConcerns;
using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;

using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.AuditableEntities
{
	public class User : IEntity
	{

		#region Constructor
		public User()
		{
			Phones = new HashSet<Phone>();
		}

		public User(
			string fullName,
			string email,
			string password,

			ICollection<Phone> phones)
		{
			FullName = fullName;
			Email = email;
			Password = password;

			Phones = phones ?? new HashSet<Phone>();
		}

		public User(string fullName,
			string email,
			string password,
			string id,

			ICollection<Phone> phones)
		{
			FullName = fullName;
			Email = email;
			Password = password;
			Id = id;

			Phones = phones ?? new HashSet<Phone>();
		}
		#endregion

		public string FullName { get; private set; }
		public string Email { get; private set; }
		public string Id { get; private set; }
		public string Password { get; private set; }


		public ICollection<Phone> Phones { get; private set; }

		#region Behaviour

		public bool UpsertPhone(Phone phone)
		{
			if (phone is null) throw new EntityNotFoundException("User -> Phone");

			var current = Phones.FirstOrDefault(r => r.Type == phone.Type);
			if (current != null)
				current = phone;
			else
				Phones.Add(phone);

			return true;
		}

		public bool RemovePhone(Phone phone)
		{
			if (phone is null) throw new EntityNotFoundException("User -> Phone");

			if (!Phones.Contains(phone)) return false;

			Phones.Remove(phone);
			return true;
		}
		#endregion
	}
}
