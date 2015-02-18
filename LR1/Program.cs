using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LR1
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			var gramatica = 
				"\nS' -> S\n" +
				"S -> C C\n" +
				"C -> c C | d\n";
				
			//Gramatica que se cicla
			gramatica = "exp_entera' : exp_entera\n" +
				"exp_entera: TOK_ENTERO" +
				"|exp_entera `+` exp_entera" +
				"|exp_entera `-` exp_entera" +
				"|exp_entera `*` exp_entera" +
				"|`(` exp_entera `)` \n" +
			"exp_real: TOK_REAL" +
			"|exp_real `+` exp_real" +
			"|exp_real `-` exp_real" +
			"|exp_real `*` exp_real" +
			"|exp_real `/` exp_real" +
			"|`(` exp_real `)`\n";

				
				
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

			var gramarG = Tools.GetProductions(gramatica);


			var setC = Parser.Items (gramarG).ToList();
			var tableAction = Parser.LRTable(setC, gramarG);


			foreach(var row in tableAction) {
				if (row == null) {
					System.Console.Write("\t\t");
					continue;
				}
				foreach(var cell in row){
					System.Console.Write (cell);
					System.Console.Write ("\t");
				}
				System.Console.WriteLine();
			}
		}

	}
}
