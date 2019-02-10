using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace Dwares.Rookie.Airtable
{
	public class AirException : Exception
	{
		protected AirException(HttpStatusCode errorCode, string errorName, string errorMessage) : 
			base($"{errorName} - {errorCode}: {errorMessage}")
		{
			ErrorCode = errorCode;
			ErrorName = errorName;
			ErrorMessage = errorMessage;
		}

		public HttpStatusCode ErrorCode { get; }
		public string ErrorName { get; }
		public string ErrorMessage { get; }

		public static async Task<AirException> CheckStatus(HttpResponseMessage response)
		{
			switch (response.StatusCode)
			{
			case HttpStatusCode.OK:
				return null;

			case HttpStatusCode.BadRequest:
				return new BadRequestException();

			case HttpStatusCode.Forbidden:
				return new ForbiddenException();

			case HttpStatusCode.NotFound:
				return new NotFoundException();

			case HttpStatusCode.PaymentRequired:
				return new PaymentRequiredException();

			case HttpStatusCode.Unauthorized:
				return new UnauthorizedException();

			case HttpStatusCode.RequestEntityTooLarge:
				return new RequestEntityTooLargeException();

			case (HttpStatusCode)422:        // There is no HttpStatusCode.InvalidRequest defined in HttpStatusCode Enumeration.
				var error = await ReadResponseErrorMessage(response);
				return new InvalidRequestException(error);

			default:
				throw new UnrecognizedException(response.StatusCode);
			}
		}

		private static async Task<string> ReadResponseErrorMessage(HttpResponseMessage response)
		{
			var content = await response.Content.ReadAsStringAsync();

			if (string.IsNullOrEmpty(content)) {
				return null;
			}

			var json = JObject.Parse(content);
			var errorMessage = json["error"]?["message"]?.Value<string>();

			return errorMessage;
		}
	}

	public class UnrecognizedException : AirException
	{
		public UnrecognizedException(HttpStatusCode statusCode) : base(statusCode, "Unrecognized Error", $"Airtable returned HTTP status code {statusCode}")
		{
		}
	}


	public class BadRequestException : AirException
	{
		public BadRequestException() : base(HttpStatusCode.BadRequest, "Bad Request", "The request encoding is invalid; the request can't be parsed as a valid JSON.")
		{
		}
	}

	public class UnauthorizedException : AirException
	{
		public UnauthorizedException() : base(HttpStatusCode.Unauthorized, "Unauthorized", "Accessing a protected resource without authorization or with invalid credentials.")
		{
		}
	}


	public class PaymentRequiredException : AirException
	{
		public PaymentRequiredException() : base(
			HttpStatusCode.PaymentRequired,
			"Payment Required",
			"The account associated with the API key making requests hits a quota that can be increased by upgrading the Airtable account plan.")
		{
		}
	}


	public class ForbiddenException : AirException
	{
		public ForbiddenException() : base(
			HttpStatusCode.Forbidden,
			"Forbidden",
			"Accessing a protected resource with API credentials that don't have access to that resource.")
		{
		}
	}


	public class NotFoundException : AirException
	{
		public NotFoundException() : base(
			HttpStatusCode.NotFound,
			"Not Found",
			"Route or resource is not found. This error is returned when the request hits an undefined route, or if the resource doesn't exist (e.g. has been deleted).")
		{
		}
	}


	public class RequestEntityTooLargeException : AirException
	{
		public RequestEntityTooLargeException() : base(
			HttpStatusCode.RequestEntityTooLarge,
			"Request Entity Too Large",
			"The request exceeded the maximum allowed payload size. You shouldn't encounter this under normal use.")
		{
		}
	}


	public class InvalidRequestException : AirException
	{
		public string DetailedErrorMessage { get; }

		public InvalidRequestException(string errorMessage = null) : base(
			(HttpStatusCode)422,
			"Invalid Request",
			"The request data is invalid. This includes most of the base-specific validations. The DetailedErrorMessage property contains the detailed error message string.")
		{
			DetailedErrorMessage = errorMessage;
		}
	}
}
