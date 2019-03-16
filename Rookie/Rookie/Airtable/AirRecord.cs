using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Dwares.Rookie.Airtable
{
	public class AirRecord
	{
		//static ClassRef @class = new ClassRef(typeof(AirRecord));

		public AirRecord()
		{
			//Debug.EnableTracing(@class);
		}

		[JsonProperty("id")]
		public string Id { get; internal set; }

		[JsonProperty("createdTime")]
		public DateTime CreatedTime { get; internal set; }

		[JsonProperty("fields")]
		public FeildSet Fields { get; internal set; } = new FeildSet();

		public object GetField(string fieldName)
		{
			if (Fields.ContainsKey(fieldName)) {
				return Fields[fieldName];
			}
			return null;
		}

		public void SetField(string fieldName, object value, IEnumerable<string> fields = null)
		{
			if (fields == null || fields.Contains(fieldName))
				Fields[fieldName] = value;
		}

		public T GetField<T>(string fieldName, T defaultValue=default(T))
		{
			var obj = GetField(fieldName);
			if (obj != null) {
				try {
					object value;
					if (typeof(T) == typeof(DateOnly)) {
						value = DateOnly.ToDateOnly(obj);
					} else {
						value = Convert.ChangeType(obj, typeof(T));
					}
					if (value is T) {
						return (T)value;
					}
				} catch (Exception exc) {
					//Debug.ExceptionCaught(exc);
					Debug.Print("Exception caught in AirRecord.GetField(): fieldName={0}, obj={1}", fieldName, obj);
				}
			}
			return defaultValue;
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

		public virtual void CopyFieldsToProperties() { }
		public virtual void CopyPropertiesToFields(IEnumerable<string> fieldNames = null) { }

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

	//public class AirRecordList
	//{
	//	[JsonProperty("offset")]
	//	public string Offset { get; internal set; }

	//	[JsonProperty("records")]
	//	public AirRecord[] Records { get; internal set; }
	//}

	public class AirRecordList<TRecord> where TRecord : AirRecord
	{
		[JsonProperty("offset")]
		public string Offset { get; internal set; }

		[JsonProperty("records")]
		public TRecord[] Records { get; internal set; }

		public void CopyFieldsToProperties()
		{
			foreach (var record in Records) {
				record.CopyFieldsToProperties();
			}
		}

		public void CopyPropertiesToFields(IEnumerable<string> fieldNames)
		{
			foreach (var record in Records) {
				record.CopyPropertiesToFields(fieldNames);
			}
		}
	}

	public class AirRecordList : AirRecordList<AirRecord>
	{
	}

}
