using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestMath.BigInt {
	public static class Mask {

		private static UInt64[] masks = new UInt64[64];

		static Mask() {
			masks[0] = 0b1;
			for(int index = 1; index < masks.Length; index++) { 
				masks[index] = masks[index - 1] | (((UInt64)0b1) << index);
			}
		}

		public static UInt64 GetRightMask(int order) {
			if(order < 0 || 64 < order)
				throw new ArgumentException("An order argument must be in range [0; 64] (UInt64 masks variants, inclusive 0)", "order");
			if(order == 0)
				return 0;
			return masks[order - 1];
		}

		public static UInt64 GetLeftMask(int order) { 
			return ~GetRightMask(64 - order);
		}

	}
}
