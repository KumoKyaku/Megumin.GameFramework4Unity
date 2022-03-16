using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework
{
    /// <summary>
    /// 可消耗的
    /// </summary>
    public interface IConsumable
    {
        /// <summary>
        /// 可消耗的数量
        /// </summary>
        int ConsumableCount { get; }

        /// <summary>
        /// 使用消耗
        /// </summary>
        /// <param name="count"></param>
        /// <param name="used">实际使用的数量</param>
        /// <returns>数量是否足够</returns>
        bool Use(int count, out int used);
    }
}


