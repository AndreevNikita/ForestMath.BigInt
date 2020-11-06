using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestMath.BigInt {
	public interface IMultipartInt<TYPE> : IComparable<TYPE>, IEquatable<TYPE>, IEnumerable<UInt64>, IFormattable {

		IEnumerable<byte> getBytes();
	}
}
