using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LR1
{

	public static class Tools
	{
		private const string _terminalPattern = "[a-z]+";
		private const string _noTerminalPattern = "[A-Z]+";
		private const string _producePattern = "->";


		public static string[] First(string[] elements, string[] gramar){
			var firsts = new List<string> ();
			foreach (var element in elements) {
				var terminals = First (element, gramar);
				firsts.AddRange(terminals);
			}
			if (!firsts.Any ())
				firsts.Add ("$");
			return firsts.Distinct().ToArray();
		}

		public static string[] First(string element, string[] gramar){
			var firsts = new List<string> ();
			if (!string.IsNullOrEmpty(element) && 
				IsTerminal (element) && 
				element != "$") {
				firsts.Add (element.Trim());
			} else {
				foreach (var production in gramar) {
					var elementA = Regex.Split (production, _producePattern).First ().Trim ();
					if (element == elementA) {
						var prodFirsts = First (production);
						firsts.AddRange (prodFirsts);
					}
				}
			}
			return firsts.Distinct().ToArray();
		}

		public static string[] First(string production){
			string patternTerminal1 = string.Format("{0}( )*{1}", _producePattern, _terminalPattern);
			string patternTerminal2 = string.Format("\\|( )*{0}", _terminalPattern);

			var terminal = Regex.Matches (production, patternTerminal1).Cast<Match>().Select(m => m.Value);
			var terminals = Regex.Matches(production, patternTerminal2).Cast<Match>().Select(m=>m.Value);

			terminal = terminal.Select(t => t.Replace(_producePattern, string.Empty).Trim());
			terminals = terminals.Select(t => t.Replace("|", string.Empty).Trim());

			var listTerminals = terminal.Union(terminals).ToList();
			if (!listTerminals.Any ())
				listTerminals.Add ("$");

			return listTerminals.ToArray();
		}

		public static string[] GetGama(string elemA, string[] gramar){
			var gamas = new List<string> ();
			foreach(var production in gramar){
				var elementA = Regex.Split(production, _producePattern).First().Trim();
				if (elementA == elemA) {
					var prodGamas = GetGama (production).ToList();
					gamas = gamas.Union(prodGamas).ToList();
				}
			}
			return gamas.ToArray();
		}


		public static string[] GetGama(string production){
			string patternTerminal1 = string.Format ("{0}( )*({1}|{2}| )+", _producePattern, _terminalPattern, _noTerminalPattern);
			string patternTerminal2 = string.Format ("\\|( )*({0}|{1}| )+", _terminalPattern, _noTerminalPattern);

			var terminal = Regex.Matches (production, patternTerminal1).Cast<Match>().Select(m => m.Value);
			var terminals = Regex.Matches(production, patternTerminal2).Cast<Match>().Select(m=>m.Value);

			terminal = terminal.Select(t => t.Replace(_producePattern, string.Empty).Trim());
			terminals = terminals.Select(t => t.Replace("|", string.Empty).Trim());

			var listTerminals = terminal.Union(terminals).ToList();
			return listTerminals.ToArray();
		}

		public static string[] GetBeta(string production){
			string patternBbeta = string.Format ("\\.({0}|{1}| )+", _terminalPattern, _noTerminalPattern);
			var matchesBbeta = Regex.Matches (production, patternBbeta).Cast<Match>().Select(m=>m.Value);
			var elementBbeta = matchesBbeta.FirstOrDefault();
			//			string elementB = null; 
			//			string beta = null;
			if(elementBbeta != null){
				var elements = elementBbeta.Split(' ');
				if (elements.Count () >= 2) {
					var betaElements = elements.ToList();
					betaElements.RemoveAt(0);
					return betaElements.ToArray();
				}
			}
			return new string[]{};
		}


		public static bool IsTerminal(string element)
		{
			return element.All(t => !char.IsUpper(t));
		}

		//[A -> α.Bβ, a]
		public static string[] GetElements(string production){
			string patternElement = string.Format ("{0}'?", _noTerminalPattern);
			string patternAlpha = string.Format("( )*({0}|{1})?( )*\\.", _noTerminalPattern, _terminalPattern);
			string patternBbeta = string.Format("\\.({0}|{1}| )+", _noTerminalPattern, _terminalPattern);
			string aPattern = string.Format (", *({0}|{1}|\\$)?", _noTerminalPattern, _terminalPattern);

			var matches = Regex.Matches(production, patternElement).Cast<Match>().Select(m=>m.Value);
			var matchesBbeta = Regex.Matches (production, patternBbeta).Cast<Match>().Select(m=>m.Value);

			var elementA = matches.FirstOrDefault();
			var alpha = Regex.Matches (production, patternAlpha).Cast<Match> ().Select (m => m.Value).FirstOrDefault();


			var elementBbeta = matchesBbeta.FirstOrDefault();
			string elementB = null; 
			string beta = null;
			if(elementBbeta != null){
				var elements = elementBbeta.Split(' ');
				if (elements.Any ()) {
					elementB = elements [0];
				}
				if (elements.Count () >= 2) {
					beta = elements[1];
				}
			}

			var a = Regex.Matches(production, aPattern).Cast<Match>().Select(m => m.Value).FirstOrDefault();

			if (alpha != null) {
				alpha = alpha.Replace(".",string.Empty).Trim();
			}
			if (elementB != null) {
				elementB = elementB.Replace (".", string.Empty).Trim();
			}
			a = a != null ? 
				a.Replace (",", string.Empty).Trim() : "$";

			return new []{ elementA, alpha, elementB, beta, a};
		}

		public static bool ContainsItemsSet(List<string[]> itemsSet, string[] item)
		{
			for (var i = 0; i < itemsSet.Count(); i++)
			{
				if(AreEqual(itemsSet[i], item))
					return true;
			}
			return false;
		}

		public static int IndexOfGramar(string[] gramar, string production){
			for (var i = 0; i < gramar.Count(); i++)
			{
				if(production == gramar[i]){
					return i;
				}
			}
			return -1;
		}

		public static int IndexOfItemsSet(List<string[]> itemsSet, string[] item)
		{
			for (var i = 0; i < itemsSet.Count(); i++)
			{
				if(AreEqual(itemsSet[i], item))
					return i;
			}
			return -1;
		}

		public static bool AreEqual(string[] array1, string[] array2)
		{
			if (array1.Count() != array2.Count())
				return false;
			for (var i = 0; i < array1.Count(); i++)
			{
				if (array1[i] != array2[i])
					return false;
			}
			return true;
		}


		private static bool IsPointOfEnd(string production){
			if (production.Contains (".,"))
				return true;
			return false;
		}

		public static string MakeProduction(string elementA, string alpha, string x, string beta, string a){
			var prod = elementA + " " + _producePattern; 
			if (alpha != null)
				prod += " " + alpha;
			if (x != null)
			{
				prod += " " + x;
			}
			prod += ".";
			if (beta != null)
				prod += beta;

			prod += ", " + a;
			return prod;
		}

		public static string MakeProduction(string eB, string gama, string b){
			return string.Format("{0} -> .{1}, {2}", eB, gama, b);
		}

		public static string[] GetTerminals(string[] gramarGp){
			var totalTerminals = new List<string> ();
			foreach(var production in gramarGp){
				var terminals = Regex.Matches(production, _terminalPattern).Cast<Match>().Select(m => m.Value).Distinct();
				totalTerminals = totalTerminals.Union(terminals).ToList();
			}
			totalTerminals.Add("$");
			return totalTerminals.ToArray();
		}

		public static string[] GetNoTerminals(string[] gramarGp){
			var totalTerminals = new List<string> ();
			foreach(var production in gramarGp){
				var terminals = Regex.Matches(production, _noTerminalPattern).Cast<Match>().Select(m => m.Value).Distinct();
				totalTerminals = totalTerminals.Union(terminals).ToList();
			}
			return totalTerminals.ToArray();
		}
	}
}

