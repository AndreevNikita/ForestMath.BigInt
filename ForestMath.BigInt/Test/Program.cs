using ForestMath.BigInt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
	class Program {
		static void Main(string[] args) {
			UInt64 a = 1;
			Console.WriteLine(Symbolic.ToString(Symbolic.FromString("001203000")));
			Console.WriteLine(Symbolic.ToString(Symbolic.FromString("000")));
			/*
			Console.WriteLine("Right masks");
			for(int index = 0; index <= 64; index++)
				Console.WriteLine(BigIntsFormatter.BINARY_FORMATTER.ConvertToString(new UInt64[] { Mask.GetRightMask(index) }));

			Console.WriteLine();
			Console.WriteLine("Left masks");
			for(int index = 0; index <= 64; index++)
				Console.WriteLine(BigIntsFormatter.BINARY_FORMATTER.ConvertToString(new UInt64[] { Mask.GetLeftMask(index) }));
			*/
			UInt128 test = 1;
			test |= test << 64;
			for(int counter = 0; counter < 128; counter++)
				Console.WriteLine("{0:B}", test << counter);
			test <<= 63;
			for(int counter = 1; counter < 128; counter++)
				Console.WriteLine("{0:B}", test >> counter);
			
			Console.WriteLine("{0:D}", new UInt128(10, 1039) + 1);
			Console.WriteLine("{0:D}", new UInt128(10, 4) - new UInt128(11, 0));
			Console.WriteLine("{0}\nis\n{0:D}\nin decimal\nor\n{0:X}\nin hex or\n{0:B}\nin binary", new UInt128(10, 4));

			Console.WriteLine();
			Console.WriteLine("Parse tests");
			UInt128 testUint128 = new UInt128(100500, 10);
			Console.WriteLine("Test UInt128 orig:   {0:B}", testUint128);
			UInt128 testUInt128Parse = UInt128.Parse(testUint128.ToString());
			Console.WriteLine("Test UInt128 parsed: {0:B}", testUInt128Parse);
			Console.WriteLine();
			Console.WriteLine("Test UInt128:             {0}", testUint128);
			Console.WriteLine("Test string parse result: {0}", testUInt128Parse);
			Console.ReadKey();

		}
	}
}
