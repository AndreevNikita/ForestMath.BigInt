using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ForestMath.BigInt
{
	public struct UInt128 : IMultipartInt<UInt128> { 
		
		UInt64 lowPart;
		UInt64 highPart;

		public static readonly UInt128 MIN_VALUE = new UInt128(UInt64.MinValue, UInt64.MinValue);
		public static readonly UInt128 MAX_VALUE = new UInt128(UInt64.MaxValue, UInt64.MaxValue);
		public static readonly int BITS_COUNT = 128; 

		public UInt128(UInt64 lowPart, UInt64 highPart) { 
			this.lowPart = lowPart;
			this.highPart = highPart;
		}

		public UInt128(int value) : this((UInt64)value, 0) {  }

		public int CompareTo(UInt128 other) {
			if(this.highPart == other.highPart) { 
				if(this.lowPart != other.lowPart)
					return this.lowPart > other.lowPart ? 1 : -1;
			} else { 
				return this.highPart > other.highPart ? 1 : -1;
			}
			return 0;
		}

		public override int GetHashCode() {
			return (int)((UInt64)lowPart.GetHashCode() + (UInt64)highPart.GetHashCode());
		}

		public static bool IsNumericType(Type type) {
			switch (Type.GetTypeCode(type)) {
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}


		public override bool Equals(object obj) {
			if(obj is UInt128) { 
				return Equals((UInt128)obj);
			} else if(IsNumericType(obj.GetType())) { 
				return Equals((UInt128)obj);
			} else { 
				return false;
			}
		}

		public bool Equals(UInt128 other) {
			return (this.highPart == other.highPart) && (this.lowPart == other.lowPart);
		}

		public static implicit operator UInt128(byte value) { return new UInt128(value, 0); }
		public static implicit operator UInt128(UInt16 value) { return new UInt128(value, 0); }
		public static implicit operator UInt128(UInt32 value) { return new UInt128(value, 0); }
		public static implicit operator UInt128(UInt64 value) { return new UInt128(value, 0); }
		public static implicit operator UInt128((UInt64 lowpart, UInt64 highPart) t) { return new UInt128(t.lowpart, t.highPart); }
		public static explicit operator UInt128(sbyte value) { return new UInt128((UInt64)value, 0); }
		public static explicit operator UInt128(Int16 value) { return new UInt128((UInt64)value, 0); }
		public static explicit operator UInt128(Int32 value) { return new UInt128((UInt64)value, 0); }
		public static explicit operator UInt128(Int64 value) { return new UInt128((UInt64)value, 0); }
		public static implicit operator UInt128(float value) { return new UInt128((UInt64)value, 0); }
		public static explicit operator UInt128(double value) { return new UInt128((UInt64)value, 0); }

		public static explicit operator byte(UInt128 value) { return (byte)value.lowPart; } 
		public static explicit operator UInt16(UInt128 value) { return (UInt16)value.lowPart; }
		public static explicit operator UInt32(UInt128 value) { return (UInt32)value.lowPart; }
		public static explicit operator UInt64(UInt128 value) { return value.lowPart; }
		public static explicit operator sbyte(UInt128 value) { return (sbyte)value.lowPart; } 
		public static explicit operator Int16(UInt128 value) { return (Int16)value.lowPart; }
		public static explicit operator Int32(UInt128 value) { return (Int32)value.lowPart; }
		public static explicit operator Int64(UInt128 value) { return (Int64)value.lowPart; }
		public static explicit operator (UInt64 lowpart, UInt64 highPart)(UInt128 value) { return (value.lowPart, value.highPart); }



		public static bool operator>(UInt128 a, UInt128 b) { 
			return a.highPart > b.highPart || ((a.highPart == b.highPart) && (a.lowPart > b.lowPart));
		}

		public static bool operator<(UInt128 a, UInt128 b) { 
			return a.highPart < b.highPart || ((a.highPart == b.highPart) && (a.lowPart < b.lowPart));
		}

		public static bool operator>=(UInt128 a, UInt128 b) { 
			return a.highPart > b.highPart || ((a.highPart == b.highPart) && (a.lowPart >= b.lowPart));
		}

		public static bool operator<=(UInt128 a, UInt128 b) { 
			return a.highPart < b.highPart || ((a.highPart == b.highPart) && (a.lowPart <= b.lowPart));
		}

		public static bool operator==(UInt128 a, UInt128 b) { 
			return (a.highPart == b.highPart) && (a.lowPart == b.lowPart);
		}

		public static bool operator!=(UInt128 a, UInt128 b) { 
			return (a.highPart != b.highPart) || (a.lowPart != b.lowPart);
		}


		public static UInt128 operator+(UInt128 a, UInt128 b) { 
			UInt128 result = new UInt128(a.lowPart + b.lowPart, a.highPart + b.highPart);

			if(UInt64.MaxValue - a.lowPart < b.lowPart)
				result.highPart++;
			
			return result;
		}

		public static UInt128 operator-(UInt128 a, UInt128 b) { 
			if(a < b) {
				return UInt128.MAX_VALUE - (b - a);
			} else { 
				UInt128 result = new UInt128(a.lowPart - b.lowPart, a.highPart - b.highPart);

				if(a.lowPart < b.lowPart) { 
					result.highPart--;
					//UInt64.MaxValue - (b.lowPart - a.lowPart) mod (UInt64.MaxValue + 1) = a.lowPart - b.lowPart mod (UInt64.MaxValue + 1)
				}
				return result;
			}
		}

		public static UInt128 operator+(UInt128 a, UInt64 b) { 
			UInt128 result = new UInt128(a.lowPart + b, a.highPart);

			if(UInt64.MaxValue - a.lowPart < b)
				result.highPart++;
			
			return result;
		}

		public static UInt128 operator-(UInt128 a, UInt64 b) { 
			UInt128 result = new UInt128(a.lowPart - b, a.highPart);

			if(UInt64.MaxValue - a.lowPart < b)
				result.highPart++;
			
			return result;
		}


		public static UInt128 operator+(UInt128 a, int b) { 
			if(b < 0)
				return a - (UInt64)b;

			return a + (UInt64)b;
		}

		public static UInt128 operator-(UInt128 a, int b) { 
			if(b < 0)
				return a + (UInt64)b;

			return a - (UInt64)b;
		}

		public static UInt128 operator&(UInt128 a, UInt128 b) { 
			return new UInt128(a.lowPart & b.lowPart, a.highPart & b.highPart);
		}

		public static UInt128 operator|(UInt128 a, UInt128 b) { 
			return new UInt128(a.lowPart | b.lowPart, a.highPart | b.highPart);
		}

		public static UInt128 operator^(UInt128 a, UInt128 b) { 
			return new UInt128(a.lowPart ^ b.lowPart, a.highPart ^ b.highPart);
		}

		public static UInt128 operator~(UInt128 a) { 
			return new UInt128(~a.lowPart, ~a.highPart);
		}

		public static UInt128 operator>>(UInt128 a, int shiftCount) { 
			if(shiftCount > 63)
				return new UInt128(a.highPart >> (shiftCount - 64), 0);

			return new UInt128((a.lowPart >> shiftCount) | (a.highPart << (64 - shiftCount)), a.highPart >> shiftCount);
		}

		public static UInt128 operator<<(UInt128 a, int shiftCount) {
			if(shiftCount > 63)
				return new UInt128(0, a.lowPart << (shiftCount - 64));
			return new UInt128(a.lowPart << shiftCount, (a.highPart << shiftCount) | (a.lowPart >> (64 - shiftCount) ));
		}
		
		public IEnumerable<byte> getBytes() { 

			foreach(byte b in BitConverter.GetBytes(highPart))
				yield return b;

			foreach(byte b in BitConverter.GetBytes(lowPart))
				yield return b;

		}

		public IEnumerator<ulong> GetEnumerator() { 
			yield return lowPart;
			yield return highPart;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public override string ToString() {
			return BigIntsFormatter.NORMAL_FORMATTER.ConvertToString(this);
		}

		public string ToString(string format, IFormatProvider formatProvider) {
			if(format == null)
				return BigIntsFormatter.NORMAL_FORMATTER.ConvertToString(this);

			switch (format.ToUpperInvariant())
			{
				case "X":
					return BigIntsFormatter.HEX_FORMATTER.ConvertToString(this);
				case "B":
					return BigIntsFormatter.BINARY_FORMATTER.ConvertToString(this);
				case "D":
					return BigIntsFormatter.DECIMAL_FORMATTER.ConvertToString(this);
			}
			throw new ArgumentException($"Invalid format {format.ToUpperInvariant()}");
		}

		private static UInt128[] powers = new UInt128[128];


		static UInt128() { 
			powers[0] = 1;
			for(int index = 1; index < powers.Length; index++) {
				powers[index] = powers[index - 1] << 1;
			}
		}

		public static bool TryParseDecimal(string str, out UInt128 result) { 
			char[] symbols = Symbolic.FromString(str);
			
			UInt128 one = 1;
			UInt128 _result = 0;

			
			if(Symbolic.SymbolicCmp(symbols, Symbolic.Pow2(BITS_COUNT)) > -1) {
				result = default(UInt128);
				return false;
			}

			for(int startPower = 127; startPower >= 0; startPower--) {
				char[] subResult = Symbolic.SymbolicSubtract(symbols, Symbolic.Pow2(startPower));
				if(subResult != null) {
					_result |= one << startPower;
					symbols = subResult;
				}
			}	

			result = _result;
			return true;
		}

		public static UInt128 ParseDecimal(string str) { 

			if(str == null)
				throw new ArgumentNullException("str", "str is null");

			if(TryParseDecimal(str, out UInt128 result))
				return result;


			throw new FormatException($"Bad format of \"{str}\"");
		}

		public static bool TryParse(string str, out UInt128 result) { 
			string[] splittedString = str.Split('-');
			if(splittedString.Length != 2) {
				result = default(UInt128);
				return false;
			}
			Console.WriteLine("parts: {0}, {1}", splittedString[0], splittedString[1]);
			if(UInt64.TryParse(splittedString[0], out UInt64 lowPart) && UInt64.TryParse(splittedString[1], out UInt64 highPart)) { 
				result = new UInt128(lowPart, highPart);
				return true;
			} else { 
				result = default(UInt128);
				return false;
			}
		}

		public static UInt128 Parse(string str) { 

			if(str == null)
				throw new ArgumentNullException("str", "str is null");

			if(TryParse(str, out UInt128 result))
				return result;


			throw new FormatException($"Bad format of \"{str}\"");
		}
	}
}
