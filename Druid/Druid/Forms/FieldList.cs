using System;
using System.Collections.Generic;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public interface IFiledList : IEnumerable<IField>
	{
	}

	public class FieldList : List<IField>, IFiledList
	{
		//static ClassRef @class = new ClassRef(typeof(FieldList));

		public FieldList(IEnumerable<IField> fields = null)
		{
			//Debug.EnableTracing(@class);

			if (fields != null) {
				AddRange(fields);
			}
		}

		public FieldList(params IField[] fields) : this((IEnumerable<IField>)fields) { }
	}

	public static partial class Extensions
	{
		public static Exception Validate(this IFiledList fields)
		{
			Exception error = null;
			foreach (var field in fields) {
				if (!field.IsValid) {
					error = field.Error;
					break;
				}
			}
			return error;
		}

		public static List<Exception> ValidateAll(this IFiledList fields)
		{
			List<Exception> errors = null;
			foreach (var field in fields) {
				if (!field.IsValid) {
					if (errors == null) {
						errors = new List<Exception>();
					}
					errors.Add(field.Error);
					break;
				}
			}
			return errors;
		}
	}
}
