using System;
using System.Linq;
using System.Text;
using MoreLinq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheLostSpirit.Domain.Util
{
    /// <summary>
    ///     機率分布函數採樣， 在此預設各分部變化量為1/(n^2)
    /// </summary>
    /// <a href="https://reurl.cc/9n9eQa" />
    public class CdfSampler
    {
        readonly double[] _cdf;

        /// <param name="x">分布數量</param>
        public CdfSampler(int x) {
            if (x <= 0) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            var cdfDenormalize =
                Enumerable
                    .Range(1, x)
                    .Select(i => 1.0 / ((double)i * i))
                    .Scan((sum, t) => sum + t)
                    .ToArray();

            //正規化[0,1]
            var sum = cdfDenormalize.Last();
            _cdf = cdfDenormalize.Select(i => i / sum).ToArray();

            DebugMessage();
        }

        public int Next() {
            var u   = Random.value;
            var idx = Array.BinarySearch(_cdf, u);

            if (idx < 0) {
                idx = ~idx; // 第一個 ≥ u 的位置
            }

            if (idx >= _cdf.Length) {
                idx = _cdf.Length - 1; // u==1 時回最後一格
            }

            Debug.Log($"idx: {idx}");

            return idx;
        }

        void DebugMessage() {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < _cdf.Length; i++) {
                var item = _cdf[i];
                stringBuilder.Append($"[{i}]{item}\n");
            }

            Debug.Log($"{stringBuilder}");
        }
    }
}