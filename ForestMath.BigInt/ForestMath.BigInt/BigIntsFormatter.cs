using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestMath.BigInt {

	public interface IBigIntFormatter { 

		string ConvertToString(IEnumerable<byte> bytes);

		string ConvertToString(IEnumerable<UInt64> parts);
		
	}

	public class BigIntBinaryFormatter : IBigIntFormatter {

		public static BigIntBinaryFormatter INSTANCE { get; } = new BigIntBinaryFormatter();

		private BigIntBinaryFormatter() { }

		public string ConvertToString(IEnumerable<byte> bytes) { 
			StringBuilder binaryString = new StringBuilder();
			foreach(byte b in bytes) {
				binaryString.Append(Convert.ToString(b, 2).PadLeft( 8, '0'));
				binaryString.Append(" ");
			}
			return new string(binaryString.ToString().Reverse().ToArray());
		}

		public string ConvertToString(IEnumerable<UInt64> parts) { 
			StringBuilder binaryString = new StringBuilder();
			bool isFirst = true;
			foreach(UInt64 part in parts) {
				UInt64 buffer = part;
				for(int index = 0; index < sizeof(UInt64) * 8; index++) { 
					if(index % 8 == 0) {
						if(!isFirst) {
							binaryString.Append(" ");
						} else { 
							isFirst = false;
						}
					}
					binaryString.Append((buffer & 0b1) == 1 ? '1' : '0');
					buffer >>= 1;
				}
			}
			return new string(binaryString.ToString().Reverse().ToArray());
		}
	}

	public class BigIntHexFormatter : IBigIntFormatter {

		public static BigIntHexFormatter INSTANCE { get; } = new BigIntHexFormatter();
		private BigIntHexFormatter() { }

		public string ConvertToString(IEnumerable<byte> bytes) { 
			StringBuilder hex = new StringBuilder();
			int counter = 0;
			bool isFirst = true;
			foreach(byte b in bytes.Reverse()) {
				hex.AppendFormat(counter % 64 == 0 && !isFirst ? "{0:x2} " : "{0:x2}", b);
				isFirst = false;
				counter++;
			}
			return hex.ToString();
		}

		public string ConvertToString(IEnumerable<UInt64> parts) { 
			StringBuilder hex = new StringBuilder();
			foreach(UInt64 part in parts.Reverse()) {
				hex.AppendFormat("{0:x16} ", part);
			}
			return hex.ToString();
		}
	}

	public class BigIntDecimalFormatter : IBigIntFormatter {

		public static BigIntDecimalFormatter INSTANCE { get; } = new BigIntDecimalFormatter();
		private BigIntDecimalFormatter() { }

		public string ConvertToString(IEnumerable<byte> bytes) {
			char[] result = new char[1] { (char)0 } ;
			int current2Power = 0;
			foreach(byte b in bytes) { 
				byte buffer = b;
				for(int index = 0; index < sizeof(byte) * 8; index++) { 
					if((buffer & 0b1) == 1) {
						result = Symbolic.symbolicAdd(result, Symbolic.Pow2(current2Power));
					}
					buffer >>= 1;
					current2Power++;
				}
			}
			return Symbolic.ToString(result);
		}

		public string ConvertToString(IEnumerable<UInt64> parts) {
			char[] result = new char[1] { (char)0 } ;
			int current2Power = 0;
			foreach(UInt64 part in parts) { 
				UInt64 buffer = part;
				for(int index = 0; index < sizeof(UInt64) * 8; index++) { 
					if((buffer & 0b1) == 1) {
						result = Symbolic.symbolicAdd(result, Symbolic.Pow2(current2Power));
					}
					buffer >>= 1;
					current2Power++;
				}
			}
			return Symbolic.ToString(result);
		}
	}

	public class BigIntNormalFormatter : IBigIntFormatter { 
		public static BigIntNormalFormatter INSTANCE { get; } = new BigIntNormalFormatter();
		private BigIntNormalFormatter() { }

		public string ConvertToString(IEnumerable<byte> bytes) {
			StringBuilder result = new StringBuilder();
			bool isFirst = true;
			foreach(UInt64 part in bytes.Reverse()) {
				
				if(!isFirst) 
					result.Append("-");
				else
					isFirst = false;
				result.Append(part);
			}

			return result.ToString();
		}

		public string ConvertToString(IEnumerable<ulong> parts) {
			StringBuilder result = new StringBuilder();
			bool isFirst = true;
			foreach(UInt64 part in parts) {
				if(!isFirst) 
					result.Append("-");
				else
					isFirst = false;
				result.Append(part);
			}

			return result.ToString();
		}
	}

	public class BigIntsFormatter {

		public static BigIntBinaryFormatter BINARY_FORMATTER { get; } = BigIntBinaryFormatter.INSTANCE; 
		public static BigIntDecimalFormatter DECIMAL_FORMATTER { get; } = BigIntDecimalFormatter.INSTANCE; 
		public static BigIntHexFormatter HEX_FORMATTER { get; } = BigIntHexFormatter.INSTANCE; 

		public static BigIntNormalFormatter NORMAL_FORMATTER { get; } = BigIntNormalFormatter.INSTANCE; 
	}
}
