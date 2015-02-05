using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LR1
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
            var gramarG = new[] {
				"S' -> S", 
				"S -> C C",
				"C -> c C | d"
			};
				
//			gramarG = new[] {
//				"S' -> S", 
//				"S -> L igual R",
//				"S -> R",
//				"L -> por R",
//				"L -> id",
//				"R -> L"
//			};

//			gramarG = new []{ 
//				"S' -> S",
//				"S -> id | V igual E",
//				"V -> id",
//				"E -> V | num"
//			};

//			gramarG = new [] {
//				"S -> p S p | a",
//			};


			List<string[]> setC = Items (gramarG).ToList();
			LRTable (setC, gramarG);
		}


		public static void LRTable(List<string[]> setC, string[] gramarGp){
			var terminals = Tools.GetTerminals(gramarGp);
			var noTerminals = Tools.GetNoTerminals (gramarGp);
			var tokens = terminals.Union (noTerminals).ToArray();
			var action = new string[setC.Count(), tokens.Count()];

			for(int i = 0; i < setC.Count(); i++){
				for(int j = 0; j < tokens.Count(); j++) {
					var isTerminal = Tools.IsTerminal (tokens [j]);
					var itemSet = GoTo(setC.ElementAt(i), tokens[j], gramarGp);
					var index = Tools.IndexOfItemsSet (setC, itemSet);

					if (index != -1) {
						action [i, j] = isTerminal ? string.Format ("s{0}", index) : index.ToString();
					}
					if(isTerminal){
						//var production = setC[i].FirstOrDefault();
						//if (Tools.IsPointOfEnd (production)) {
							//Verificar que la gramatica este en el formato adecuado
							//index = IndexOfGramar(gramarGp, production);
							//action [i, j] = string.Format ("r{0}", index);
						//}
					}
				}
			}
		}

        public static string[] Closure(string[] sI, string[] gramarGp)
        {
			var I = new List<string>(sI);
			for (var i = 0; i < I.Count; i++) {
				var elements = Tools.GetElements(I[i]);
				var eB = elements [2];
				var beta = elements[3];
				var a = elements [4];
				var aBeta = new []{ beta, a };
				foreach (var gama in Tools.GetGama(eB, gramarGp)){
					foreach (var b in Tools.First(aBeta, gramarGp)){

						var prod = Tools.MakeProduction(eB, gama, b);
						I.Add(prod);
					}
				}
			}
			return I.ToArray();

		}

		public static string[] GoTo(string[] I, string x, string[] gramarGp){
			var j = new List<string>();
			foreach(var prod in I){
				var elements = Tools.GetElements (prod);
				var eA = elements[0];
				var alpha = elements [1];
			    var eX = elements[2];
				var beta = string.Join(" ", Tools.GetBeta(prod));
				var a = elements [4];
			    if (!string.IsNullOrEmpty(x) && eX == x)
			    {
			        //A -> αX.β, a
					var newElement = Tools.MakeProduction(eA, alpha, x, beta, a);
			        j.Add(newElement);
			    }
			}
            return Closure(j.ToArray(), gramarGp);
		}

		static ICollection<string[]> Items(string[] gramarGp){
			var sC = new List<string[]>();
            var n = Closure(new[] { "S' -> .S, $" }, gramarGp);
			sC.Add(n);
			for (var i = 0; i < sC.Count; i++){
				var I = sC[i];
				foreach (var prod in I) {
					var elements = Tools.GetElements (prod);
					var x = elements [2];
                    var itemSet = GoTo(I, x, gramarGp);
					if(itemSet.Any() && !Tools.ContainsItemsSet(sC, itemSet)) {
						sC.Add (itemSet);
					}
				}
			}
			return sC;
		}
	}
}
