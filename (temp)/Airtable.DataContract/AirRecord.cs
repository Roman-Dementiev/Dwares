using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Data;


namespace Dwares.Drudge.Airtable
{
    [DataContract]
    public class AirRecord
	{
		//static ClassRef @class = new ClassRef(typeof(AirRecord));

		public AirRecord()
		{
			//Debug.EnableTracing(@class);

			FieldNames = new List<string> { nameof(Id), nameof(CreatedTime), nameof(Fields) };
			Fields =  new FeildSet();
		}

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; internal set; }

		[DataMember(Name = "createdTime", EmitDefaultValue = false)]
		public DateTime CreatedTime { get; internal set; }

		[DataMember(Name = "fields", EmitDefaultValue = false)]
		public FeildSet Fields { get; internal set; }

		//public string RecordId => Id;
		//public IEnumerable<string> GetFieldNames() => FieldNames;

		public List<string> FieldNames { get; }

		//protected void AddFieldNames(params string[] names)
		//{
		//	FieldNames.AddRange(names);
		//}

		public object GetField(string fieldName)
		{
			if (Fields.ContainsKey(fieldName)) {
				return Fields[fieldName];
			}
			return null;
		}

		public void SetField<T>(string fieldName, T value)
		{
			SetField(fieldName, value, null);
		}

		public void SetField(string fieldName, object value, IEnumerable<string> fields)
		{
			if (fields == null || fields.Contains(fieldName)) {
				//if (value is Currency currency) {
				//	value = currency.Value;
				//}
				Fields[fieldName] = value;
			}
		}

		bool GetField<T>(string fieldName, out T value, T defaultValue)
		{
			var obj = GetField(fieldName);
			if (obj != null) {
				try {
					//if (typeof(T) == typeof(Currency)) {
					//	value = Currency.ToCurrency(obj);
					//}
					if (typeof(T) == typeof(DateOnly)) {
						obj = DateOnly.ToDateOnly(obj);
					}

					if (obj is T _value) {
						value = _value;
					} else {
						obj = Convert.ChangeType(obj, typeof(T));
						value = (T)obj;
					}
					return true;
				}
				catch (Exception exc) {
					//Debug.ExceptionCaught(exc);
					Debug.Print("Exception caught in AirRecord.GetField(): fieldName={0}, obj={1}:  {2}", fieldName, obj, exc);
				}
			}
			value = defaultValue;
			return false;
		}

		public bool GetField<T>(string fieldName, out T value)
		{
			return GetField(fieldName, out value, default(T));
		}

		public T GetField<T>(string fieldName, T defaultValue=default(T))
		{
			T value;
			GetField(fieldName, out value, defaultValue);
			return value;
		}

		//public Dictionary<string, object> GetFields(params string[] fieldNames)
		//{
		//	var fields = new Dictionary<string, object>();
		//	foreach (var name in fieldNames) {
		//		if (Fields.ContainsKey(name)) {
		//			fields.Add(name, Fields[name]);
		//		}
		//	}
		//	return fields;
		//}

		public FeildSet GetFields(IEnumerable<string> fieldNames = null)
		{
			if (fieldNames == null) {
				fieldNames = Fields.Keys;
			}

			var fields = new FeildSet();
			foreach (var name in fieldNames) {
				if (Fields.ContainsKey(name)) {
					fields.Add(name, Fields[name]);
				}
			}
			return fields;
		}

		public Dictionary<string, object> GetFields(params string[] fieldNames)
			=> GetFields((IEnumerable<string>)fieldNames);

		//public IEnumerable<AirAttachment> GetAttachmentField(string attachmentsFieldName)
		//{
		//	var attachmentField = GetField(attachmentsFieldName);
		//	if (attachmentField == null) {
		//		return null;
		//	}

		//	//
		//	// At this point, attachmentField is an array of nested objects representing the attachment list.
		//	// Take advantage of the serialization and deserialization of JsonConvert
		//	// to take care of the AirtableAttachment construction for us.
		//	//

		//	var attachments = new List<AirAttachment>();
		//	try {
		//		var json = JsonConvert.SerializeObject(attachmentField);
		//		var rawAttachments = JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, object>>>(json);

		//		foreach (var rawAttachment in rawAttachments) {
		//			json = JsonConvert.SerializeObject(rawAttachment);
		//			attachments.Add(JsonConvert.DeserializeObject<AirAttachment>(json));
		//		}
		//	}
		//	catch (Exception error) {
		//		throw new ArgumentException("Field '" + attachmentsFieldName + "' is not an Attachments field." +
		//			Environment.NewLine +
		//			"It has caused the exception: " +  error.Message);
		//	}
		//	return attachments;
		//}
	}
}
