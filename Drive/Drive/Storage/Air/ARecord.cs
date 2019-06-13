using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using System.Collections.Generic;

namespace Drive.Storage.Air
{
	public class ARecord : AirRecord
	{
		public const string SEQ = "#";
		public const string NAME = "Name";
		public const string TAGS = "Tags";
		public const string COMMENT = "Comment";
		public const string NOTES = "Notes";

		public ARecord() { }

		public int Seq {
			get => GetField<int>(SEQ);
		}

		public string Name {
			get => GetField<string>(NAME);
			set => SetField(NAME, value);
		}

		public List<string> TagLinks {
			get => GetLinks(TAGS);
			//set => SetLinks(TAGS, value);
		}


		public string Comment {
			get => GetField<string>(COMMENT);
			set => SetField(COMMENT, value);
		}

		public string Notes {
			get => GetField<string>(NOTES);
			set => SetField(NOTES, value);
		}
	}
}
