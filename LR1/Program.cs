using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LR1
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
//			var gramatica = 
//				"\nS' : S\n" +
//				"S : C C\n" +
//				"C : `c` C | `d`";
				
			//Gramatica que se cicla
//			var gramatica = "lineas' : lineas\n" +
//				"lineas : linea"+
//				"|lineas linea\n" +
//				"linea: exp_entera " +
//				"| exp_real\n" +
//				"exp_entera: `entero`" +
//				"|exp_entera `+` exp_entera" +
//				"|exp_entera `-` exp_entera" +
//				"|exp_entera `*` exp_entera" +
//				"|`(` exp_entera `)` \n" +
//			"exp_real: `real`" +
//			"|exp_real `+` exp_real" +
//			"|exp_real `-` exp_real" +
//			"|exp_real `*` exp_real" +
//			"|exp_real `/` exp_real" +
//			"|`(` exp_real `)`\n";


			var gramatica = "E' : E\n"+
				"E : E `+` `n`\n" +
				"E : `n`";

				
				
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


		
			var terminals = Tools.GetTerminals(gramarG);
			var noTerminals = Tools.GetNoTerminals (gramarG);
			var tokens = terminals.Union (noTerminals).ToArray();

			System.Console.Write ("\t");
			foreach(var token in tokens){
				System.Console.Write (token + "\t");
			}
			System.Console.Write("\n");
			for(int i = 0; i < tableAction.GetLength(0); i++){
				System.Console.Write (i + "\t");
				for(int j = 0; j < tableAction.GetLength(1); j++){
					System.Console.Write (tableAction[i,j] + "\t");
				}
				System.Console.Write ("\n");
			}
		}

	}
}
