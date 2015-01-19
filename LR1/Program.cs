using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LR1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//var patternFullProduction = @"[A-Za-z]+'? *-> *([A-Za-z|] *)+";
			string[] Gp = new [] {
				"S' -> S", 
				"S -> C C",
				"C -> c C | d"
			};



//			var enhanced = new []{"S' -> .S, $"};

//			var dos = "S -> .C C, $";
//
//			var elements = GetElements(dos);
//
//			var A = elements[0];
//			var alpha = elements [1];
//			var B = elements [2];
//			var beta = elements [3];
//			var a = elements [4];
//			var I = Closure(enhanced, Gp);
//
//			var J = GoTo(new []{"S -> .C C, $"}, "C", Gp);

			Elements (Gp);

		}




		public static string[] Closure(string[] Iset, string[] Gp){
			var I = new List<string>(Iset);
//			int oldCount = 0;
//			do {
				for (var i = 0; i < I.Count; i++) {
					//oldCount = I.Count();
					var elements = GetElements(I[i]);
					var B = elements [2];
					var beta = elements [3];
					var a = elements [4];
					foreach(var gama in GetGama(B, Gp)){
						foreach(var b in First(new []{ beta, a }, Gp)){
							var prod = B + " -> ." + gama + ", " +  b;
							I.Add(prod);
						}
					}
				}

//			}
//			while(oldCount != I.Count());
			return I.ToArray();
		}

		public static string[] GoTo(string[] I, string X, string[] Gp){
			var J = new List<string>();
			foreach(var prod in I){
				var elements = GetElements (prod);
				var A = elements[0];
				var alpha = elements [1];
				var beta = elements [3];
				var a = elements [4];
				//A -> αX.β, a
				//var newElement = string.Format("{0} ->{1} {2} .{3}, {4}", A, alpha, X, beta, a);
				var newElement = MakeProduction (A, alpha, X, beta, a);
				J.Add (newElement);
			}
			return Closure (J.ToArray(), Gp);
		}

		static void Elements(string[] Gp){
			var C = new List<string[]>();
			var n = Closure (new[]{ "S' -> .S, $" }, Gp);
			C.Add(n);
			for (var i = 0; i < C.Count; i++){
				var I = C [i];
				foreach (var prod in I) {
					var elements = GetElements (prod);
					var X = elements [2];
					var itemSet = GoTo(new[]{prod}, X, Gp);
					if(itemSet.Any() && !ContainsItemsSet(C, itemSet)) {
						C.Add (itemSet);
					}
				}
			}
		}
			
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
					var elementA = Regex.Split (production, "->").First ().Trim ();
					if (element == elementA) {
						var prodFirsts = First (production);
						firsts.AddRange (prodFirsts);
					}
				}
			}
			return firsts.Distinct().ToArray();
		}

		public static string[] First(string production){
			const string patternTerminal1 = @"->( )*[a-z]+";
			const string patternTerminal2 = @"\|( )*[a-z]+";

			var terminal = Regex.Matches (production, patternTerminal1).Cast<Match>().Select(m => m.Value);
			var terminals = Regex.Matches(production, patternTerminal2).Cast<Match>().Select(m=>m.Value);

			terminal = terminal.Select(t => t.Replace("->", string.Empty).Trim());
			terminals = terminals.Select(t => t.Replace("|", string.Empty).Trim());

			var listTerminals = terminal.Union(terminals).ToList();
			if (!listTerminals.Any ())
				listTerminals.Add ("$");

			return listTerminals.ToArray();
		}

		public static string[] GetGama(string elemA, string[] gramar){
			var gamas = new List<string> ();
			foreach(var production in gramar){
				var elementA = Regex.Split(production, "->").First().Trim();
				if (elementA == elemA) {
					var prodGamas = GetGama (production).ToList();
					gamas = gamas.Union(prodGamas).ToList();
				}
			}
			return gamas.ToArray();
		}


		public static string[] GetGama(string production){
			const string patternTerminal1 = @"->( )*[A-Za-z ]+";
			const string patternTerminal2 = @"\|( )*[A-Za-z ]+";

			var terminal = Regex.Matches (production, patternTerminal1).Cast<Match>().Select(m => m.Value);
			var terminals = Regex.Matches(production, patternTerminal2).Cast<Match>().Select(m=>m.Value);

			terminal = terminal.Select(t => t.Replace("->", string.Empty).Trim());
			terminals = terminals.Select(t => t.Replace("|", string.Empty).Trim());

			var listTerminals = terminal.Union(terminals).ToList();
			return listTerminals.ToArray();
		}

		public static bool IsTerminal(string element)
		{
		    return element.All(t => !char.IsUpper(t));
		}

	    //[A -> α.Bβ, a]
		public static string[] GetElements(string production){
			const string patternElement = @"[A-Za-z.$]+'?";
			const string patternAlpha = @"( )*[A-Za-z]+( )*\.";
			const string patternBbeta = @"\.[A-Za-z ]+";
			const string pattern_a = @",[A-Za-z\/$ ]+";

			var matches = Regex.Matches(production, patternElement).Cast<Match>().Select(m=>m.Value);
			var matchesBbeta = Regex.Matches (production, patternBbeta).Cast<Match>().Select(m=>m.Value);

			var elementA = matches.FirstOrDefault();
			var alpha = Regex.Matches (production, patternAlpha).Cast<Match> ().Select (m => m.Value).FirstOrDefault();


			var elementBbeta = matchesBbeta.FirstOrDefault();
			string elementB = null; 
			string beta = null;
			if(elementBbeta != null){
				var elements = elementBbeta.Split(' ');
				if(elements.Any())
					elementB = elementBbeta.Split(' ')[0];
				if (elements.Count() == 2)
					beta = elementBbeta.Split (' ') [1];
			}

			var a = Regex.Matches (production, pattern_a).Cast<Match> ().Select (m => m.Value).FirstOrDefault();

			if (alpha != null) {
				alpha = alpha.Replace(".",string.Empty).Trim();
			}
			if (elementB != null) {
				elementB = elementB.Replace (".", string.Empty).Trim();
			}
			if (a != null) {
				a = a.Replace (",", string.Empty).Trim();
			} else {
				a = "$";
			}
				
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


		private static string MakeProduction(string A, string alpha, string X, string beta, string a){
			var prod = A + " ->"; 
			if (alpha != null)
				prod += " " + alpha;
		    if (X != null)
		    {
		        prod += " " + X;
		    }
            prod += ".";
		    if (beta != null)
				prod += beta;

			prod += ", " + a;
			return prod;
		}
	}
}
