using System;
using System.Collections;
using System.Collections.Generic;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Support
{
	[Flags]
	public enum AutoSuggestionsMatchMode
	{
		Default = 0,
		MatchStart = 1,
		MatchWords = 2,
		Substrings = 3,
		IgnoreCase = 4,
		MatchStartAndIgnoreCase = MatchStart + IgnoreCase,
		MatchWordsAndIgnoreCase = MatchStart + IgnoreCase,
		SubstringsAndIgnoreCase = Substrings + IgnoreCase,

		Mathcing = MatchStart | MatchWords | Substrings
	}

	public interface IAutoSuggestions
	{
		string DisplayProperty { get; }
		string DisplayText(object suggestion);
		IList GetSuggestions(string input, AutoSuggestionsMatchMode mode, string separators);
	}

	public interface IAutoSuggestionStrings
	{
		IList<string> GetSuggestionStrings(string input, AutoSuggestionsMatchMode mode, string separators);
	}

	public abstract class AutoSuggestionsBase : IAutoSuggestions, IAutoSuggestionStrings
	{
		public string DisplayProperty { get; set; }

		public virtual string DisplayText(object suggestion)
		{
			object value = null;
			if (DisplayProperty != null) {
				value = Reflection.GetPropertyValue(suggestion, DisplayProperty, false);
			}
			if (value == null) {
				value = suggestion;
			}
			return value.ToString();
		}

		public IList GetSuggestions(string input, AutoSuggestionsMatchMode mode, string separators)
		{
			var suggestions = new List<object>();
			CollectSuggestions(input, mode, separators, suggestions, null);
			return suggestions;
		}

		public IList<string> GetSuggestionStrings(string input, AutoSuggestionsMatchMode mode, string separators)
		{
			var strings = new List<string>();
			CollectSuggestions(input, mode, separators, null, strings);
			return strings;
		}


		protected abstract void CollectSuggestions(string input, AutoSuggestionsMatchMode mode, string separators, IList suggestions, IList<string> strings);

		public virtual string Match(string input, object item, AutoSuggestionsMatchMode mode, char[] separators)
		{
			if (item == null)
				return null;

			var text = DisplayText(item);
			if (String.IsNullOrEmpty(input))
				return text;

			if ((mode & AutoSuggestionsMatchMode.IgnoreCase) != 0) {
				text = text.ToLower();
				input = input.ToLower();
			}

			bool match;
			var matching = mode & (AutoSuggestionsMatchMode.MatchStart | AutoSuggestionsMatchMode.MatchWords | AutoSuggestionsMatchMode.Substrings);
			switch (matching) {
			default:
				//case AutoSuggestionsMatchMode.Default:
				match = text.Contains(input);
				break;
			case AutoSuggestionsMatchMode.MatchStart:
				match = text.StartsWith(input);
				break;

			case AutoSuggestionsMatchMode.MatchWords:
			case AutoSuggestionsMatchMode.Substrings:
				var subs = input.Split(separators);
				var words = (matching == AutoSuggestionsMatchMode.MatchWords) ? text.Split(separators) : null;
				match = true;
				foreach (var sub in subs) {
					if (String.IsNullOrEmpty(sub))
						continue;
					if (!ContainsSub(sub, text, words)) {
						match = false;
						break;
					}
				}
				break;
			}
			return match ? text : null;
		}

		static bool ContainsSub(string sub, string text, string[] words)
		{
			//string wordsStr = "";
			//foreach (var w in words)
			//	wordsStr += "|" + w;
			//wordsStr += "|";
			//Debug.Print("ContainsSub: sub={0}, text={1}, words={2}", sub, text, wordsStr);

			if (words == null) {
				return text.Contains(sub);
			} else {
				foreach (var word in words) {
					if (word == sub)
						return true;
				}
				return false;
			}
		}

		public virtual bool ExactMatch(string input, object item, AutoSuggestionsMatchMode mode)
		{
			if (item == null || String.IsNullOrEmpty(input))
				return false;

			var text = DisplayText(item);
			if ((mode & AutoSuggestionsMatchMode.IgnoreCase) != 0) {
				text = text.ToLower();
				input = input.ToLower();
			}

			return input == text;
		}
	}

	public class AutoSuggestions : AutoSuggestionsBase
	{
		public IEnumerable SuggestionSource { get; set; }

		protected override void CollectSuggestions(string input, AutoSuggestionsMatchMode mode, string separators, IList suggestions, IList<string> strings)
		{
			if (SuggestionSource == null)
				return;

			//Debug.Print("CollectSuggestions: {0}, mode={1}", input, mode);

			//var seps = new char[separators.Length];
			//for (int i = 0; i < separators.Length; i++) {
			//	seps[i] = separators[i];
			//}

			foreach (var item in SuggestionSource) {
				var text = Match(input, item, mode, separators.ToCharArray());
				if (text != null) {
					if (suggestions != null) {
						suggestions.Add(item);
					}
					if (strings != null) {
						strings.Add(text);
					}
				}
			}
		}
	}
}
