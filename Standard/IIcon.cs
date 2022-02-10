using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin.GameFramework.Standard
{
    /// <summary>
    /// 可图标的
    /// </summary>
    public interface IIconable
    {
        Sprite Icon { get; }

        public Sprite GetIcon<T>(T option = default)
        {
            return Icon;
        }
    }
}
