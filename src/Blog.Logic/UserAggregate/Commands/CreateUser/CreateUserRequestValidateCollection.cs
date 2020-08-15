﻿using Blog.Logic.Validators;

using FluentValidation;

namespace Blog.Logic.UserAggregate.Commands.CreateUser
{
	public class CreateUserRequestValidateCollection : AbstractValidator<CreateUserRequest>
	{
		public CreateUserRequestValidateCollection(
			IPasswordValidator passwordValidator,
			IEmailValidator emailValidator)
		{
			RuleFor(x => x.FirstName).MinimumLength(3).NotEmpty();
			RuleFor(x => x.LastName).MinimumLength(3).NotEmpty();
			RuleFor(x => x.Email).EmailAddress().Must(emailValidator.IsValid).NotEmpty();
			RuleFor(x => x.Password).Must(passwordValidator.Validate).NotEmpty();
		}
	}
}