using System;
using System.Collections.Generic;
using Dwares.Dwarf;


namespace Passket.Models
{
	public class Pattern : Entity
	{
		//static ClassRef @class = new ClassRef(typeof(Pattern));

		public struct Field
		{
			public string Name { get; set; }
			public EntryKind Kind { get; set; }
		}

		public Pattern()
		{
			//Debug.EnableTracing(@class);

			Fields = new List<Field>();
		}

		public Pattern(Pattern pat) :
			this()
		{
			//Debug.EnableTracing(@class);

			if (pat == null)
				return;

			Icon = pat.Icon;
			Name = pat.Name;
			Info = pat.Info;

			foreach (var field in pat.Fields) {
				Fields.Add(field);
			}
		}

		public List<Field> Fields { get; }


		public int GetIndexOf(string fieldName)
		{
			for (int index = 0; index < Fields.Count; index ++) {
				if (Fields[index].Name == fieldName)
					return index;
			}
			return -1;
		}

		public Field? GetField(string name)
		{
			foreach (var field in Fields) {
				if (field.Name == name)
					return field;
			}
			return null;
		}

		public bool HasField(string name) => GetField(name) != null;

		public void AddFields(IEnumerable<Field> fields, bool replaceFieldsWithSameName = false)
		{
			if (fields == null)
				return;

			foreach (var field in fields) {
				int index = GetIndexOf(field.Name);
				if (index >= 0) {
					if (replaceFieldsWithSameName) {
						Fields.RemoveAt(index);
					}
					else {
						continue;
					}
				}

				Fields.Add(field);
			}
		}

		//public void AddFields(Pattern pat, bool replaceFieldsWithSameName = false)
		//{
		//	AddFields(pat?.Fields, replaceFieldsWithSameName);
		//}

		public void RemoveFields(IEnumerable<Field> fields)
		{
			if (fields == null)
				return;

			foreach (var field in fields) {
				int index = GetIndexOf(field.Name);
				if (index >= 0) {
					Fields.RemoveAt(index);
				}
			}
		}

		//public void RemoveFields(Pattern pat)
		//{
		//	RemoveFields(pat?.Fields);
		//}

		public static Pattern Join(params Pattern[] patterns)
		{
			Pattern pattern = null;

			foreach (var pat in patterns) {
				if (pat == null)
					continue;

				if (pattern == null) {
					pattern = new Pattern(pat);
				} else {
					pattern.AddFields(pat.Fields);
				}
			}

			return pattern;
		}

		public static Pattern Exclude(Pattern source, params Pattern[] patterns)
		{
			if (source == null)
				return null;

			var pattern = new Pattern(source);

			foreach (var pat in patterns) {
				pattern.RemoveFields(pat.Fields);
			}

			return pattern;
		}
	}
}
