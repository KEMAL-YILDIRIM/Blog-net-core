﻿using Blog.Domain.Exceptions;
using Blog.Logic.CrossCuttingConcerns.Exceptions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using System;
using System.Net;
using System.Threading.Tasks;

namespace Blog.Api.Common
{
	public class CustomExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public CustomExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex).ConfigureAwait(false);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var code = HttpStatusCode.InternalServerError;

			var result = string.Empty;

			switch (exception)
			{
				case ValidationException validationException:
					code = HttpStatusCode.BadRequest;
					result = JsonConvert.SerializeObject(validationException.Failures);
					break;
				case BadRequestException _:
					code = HttpStatusCode.BadRequest;
					break;
				case PropertyNotFoundException _:
					code = HttpStatusCode.NotFound;
					break;
				case EntityNotFoundException _:
					code = HttpStatusCode.NotFound;
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;

			if (string.IsNullOrEmpty(result))
			{
				result = JsonConvert.SerializeObject(new { error = exception.Message });
			}

			return context.Response.WriteAsync(result);
		}
	}

	public static class CustomExceptionHandlerMiddlewareExtensions
	{
		public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
		}
	}
}
