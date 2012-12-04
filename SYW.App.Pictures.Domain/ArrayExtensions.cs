using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PlatformClient
{
    public static class ArrayExtensions
    {
        public static string ToHexString(this byte[] bytes)
        {
            return bytes.Aggregate(new StringBuilder(bytes.Length * 2), (sb, i) => sb.Append(i.ToString("x2"))).ToString();
        }

		public static byte[] FromHexString(this string hexString)
		{
			var bytes = new List<byte>();
			for (var i = 0; i < hexString.Length; i += 2)
			{
				var byteString = new string(new[] { hexString[i], hexString[i + 1] });
				bytes.Add(byte.Parse(byteString, NumberStyles.HexNumber));
			}
			return bytes.ToArray();
		}
    }
}
