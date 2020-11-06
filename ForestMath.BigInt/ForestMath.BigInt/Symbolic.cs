using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestMath.BigInt {

	//Now only for decimal
	//This class will be moved in ForestMath.Modular in the future
	public class Symbolic {
		private static List<char[]> powers2Cache = new List<char[]>(new char[][] {new char[] { (char)1 }});

		const int START_POWER_2_CACHE = 255;

		static Symbolic() {
			Pow2(START_POWER_2_CACHE);
		}


		public static char[] SymbolicMultiply(char[] symNum1, char symbol) { 
			if(symbol == 0) {
				return new char[] { (char)0 };
			}

			int accum = 0;
			List<char> result = new List<char>(symNum1.Length);
			
			for(int index = 0; index < symNum1.Length; index++) { 
				accum += symNum1[index] * symbol;
				result.Add((char)(accum % 10));
				accum /= 10;
			}

			if(accum != 0)
				result.Add((char)accum);

			return result.ToArray();
		}

		public static char[] symbolicAdd(char[] symNum1, char[] symNum2) { 
			if(symNum1.Length < symNum2.Length) { 
				char[] buffer = symNum1;
				symNum1 = symNum2;
				symNum2 = buffer;
			}

			int accum = 0;
			List<char> result = new List<char>(symNum1.Length);
			for(int index = 0; index < symNum1.Length; index++) { 
				accum += index < symNum2.Length ? symNum1[index] + symNum2[index] : symNum1[index];
				result.Add((char)(accum % 10));
				accum /= 10;
			}

			if(accum != 0)
				result.Add((char)accum);
			return result.ToArray();
		}

		public static int SymbolicCmp(char[] symNum1, char[] symNum2) { 
			if(symNum1.Length != symNum2.Length) { 
				return symNum1.Length > symNum2.Length ? 1 : -1;
			}

			for(int index = symNum1.Length - 1; index >= 0; index--) { 
				if(symNum1[index] != symNum2[index])
					return symNum1[index] > symNum2[index] ? 1 : -1;
			}

			return 0;
		}


		//Returns null, if symNum1 < symNum2
		public static char[] SymbolicSubtract(char[] symNum1, char[] symNum2) { 
			int addition = 0;

			if(SymbolicCmp(symNum1, symNum2) == -1) {
				return null;
			}

			List<char> result = new List<char>(symNum1.Length);
			int lastNonZero = 0;
			for(int index = 0; index < symNum1.Length ; index++) { 
				int subtractValue = index < symNum2.Length ? symNum2[index] + addition : addition;
				addition = subtractValue <= symNum1[index] ? 0 : 10;
				int resultSymbol = symNum1[index] + addition - subtractValue;
				if(resultSymbol != 0)
					lastNonZero = index;
				result.Add((char)resultSymbol);
				addition /= 10;
			}
			return result.Take(lastNonZero + 1).ToArray();
		}


		public static char[] Pow2(int pow) {
			while(pow >= powers2Cache.Count) {
				powers2Cache.Add(SymbolicMultiply(powers2Cache.Last(), (char)2));
			}
			return powers2Cache[pow];
		}

		public static int CutFrontZeros(string str) {
			int cutIndex = 0;
			for(; cutIndex < str.Length && str[cutIndex] == '0'; cutIndex++);
			return cutIndex;
		}


		public static char[] FromString(string str) {
			return str.Substring(CutFrontZeros(str)).Select((char c) => (char)(c - '0')).Reverse().ToArray();
		}

		public static string ToString(char[] symNum) { 
			if(symNum.Length == 0)
				return "0";

			StringBuilder sb = new StringBuilder();
			foreach(char symbol in symNum.Reverse())
				sb.Append((char)(symbol + '0'));
			return sb.ToString();
		}
	}
}
