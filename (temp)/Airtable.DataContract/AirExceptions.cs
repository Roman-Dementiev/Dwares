using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Drudge.Airtable
{
	public class AirException : Exception
	{
		protected AirException(HttpStatusCode errorCode, string errorName, string errorExplain, string errorDetails) : 
			base(GetMessage(errorCode, errorName, errorDetails))
		{
			ErrorCode = errorCode;
			ErrorName = errorName;
			ErrorExplain = errorExplain;
			ErrorDetails = errorDetails;
		}

		public HttpStatusCode ErrorCode { get; }
		public string ErrorName { get; }
		public string ErrorExplain { get; }
		public string ErrorDetails { get; }

		public static string GetMessage(HttpStatusCode errorCode, string errorName, string errorDetails)
		{
			if (string.IsNullOrEmpty(errorDetails)) {
				return $"{errorName} - {errorCode}";
			} else {
				return $"{errorName} - {errorCode}: {errorDetails}";
			}
		}

		public static async Task<AirException> CheckStatus(HttpResponseMessage response)
		{
			string errorDetails = "";

			switch (response.StatusCode)
			{
			case HttpStatusCode.OK:
				return null;

			case HttpStatusCode.BadRequest:
				return new BadRequestException(errorDetails);

			case HttpStatusCode.Forbidden:
				return new ForbiddenException(errorDetails);

			case HttpStatusCode.NotFound:
				return new NotFoundException(errorDetails);

			case HttpStatusCode.PaymentRequired:
				return new PaymentRequiredException(errorDetails);

			case HttpStatusCode.Unauthorized:
				return new UnauthorizedException(errorDetails);

			case HttpStatusCode.RequestEntityTooLarge:
				return new RequestEntityTooLargeException(errorDetails);

			case (HttpStatusCode)422:        // There is no HttpStatusCode.InvalidRequest defined in HttpStatusCode Enumeration.
				errorDetails = await ReadResponseErrorMessage(response);
				return new InvalidRequestException(errorDetails);

			default:
				throw new UnrecognizedException(response.StatusCode, errorDetails);
			}
		}

		private static async Task<string> ReadResponseErrorMessage(HttpResponseMessage response)
		{
			var content = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(content)) {
				return null;
			}

			var errorResponse = Serialization.DeserializeJson<ErrorResponse>(content);
			var errorMessage = errorResponse?.Message;

			return errorMessage;
		}
	}

	public class UnrecognizedException : AirException
	{
		public UnrecognizedException(HttpStatusCode statusCode, string errorDetails) : 
			base(statusCode, 
				"Unrecognized Error", 
				$"Airtable returned HTTP status code {statusCode}",
				errorDetails)
		{
		}
	}


	public class BadRequestException : AirException
	{
		public BadRequestException(string errorDetails) : 
			base(HttpStatusCode.BadRequest, 
				"Bad Request",
				"The request encoding is invalid; the request can't be parsed as a valid JSON.",
				errorDetails)
		{
		}
	}

	public class UnauthorizedException : AirException
	{
		public UnauthorizedException(string errorDetails) : 
			base(HttpStatusCode.Unauthorized,
				"Unauthorized", 
				"Accessing a protected resource without authorization or with invalid credentials.",
				errorDetails)
		{
		}
	}


	public class PaymentRequiredException : AirException
	{
		public PaymentRequiredException(string errorDetails) : 
			base(HttpStatusCode.PaymentRequired,
				"Payment Required",
				"The account associated with the API key making requests hits a quota that can be increased by upgrading the Airtable account plan.",
				errorDetails)
		{
		}
	}


	public class ForbiddenException : AirException
	{
		public ForbiddenException(string errorDetails) : 
			base(HttpStatusCode.Forbidden,
				"Forbidden",
				"Accessing a protected resource with API credentials that don't have access to that resource.",
				errorDetails)
		{
		}
	}


	public class NotFoundException : AirException
	{
		public NotFoundException(string errorDetails) : 
			base(HttpStatusCode.NotFound,
				"Not Found",
				"Route or resource is not found. This error is returned when the request hits an undefined route, or if the resource doesn't exist (e.g. has been deleted).",
				errorDetails)
		{
		}
	}


	public class RequestEntityTooLargeException : AirException
	{
		public RequestEntityTooLargeException(string errorDetails) : 
			base(
				HttpStatusCode.RequestEntityTooLarge,
				"Request Entity Too Large",
				"The request exceeded the maximum allowed payload size. You shouldn't encounter this under normal use.",
				errorDetails)
		{
		}
	}


	public class InvalidRequestException : AirException
	{
		public string DetailedErrorMessage { get; }

		public InvalidRequestException(string errorDetail) :
			base((HttpStatusCode)422,
				"Invalid Request",
				"The request data is invalid. This includes most of the base-specific validations. The DetailedErrorMessage property contains the detailed error message string.",
				errorDetail)
		{
		}
	}


	public class ErrorResponse
	{
		[DataMember(Name = "error", EmitDefaultValue = false)]
		public string Error { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }
	}
}
