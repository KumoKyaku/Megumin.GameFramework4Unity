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

        //[UnityEngine.Scripting.Preserve]
        public Sprite GetIcon<T>(T option = default)
        {
#if MEGUMIN_DEBUG_IL2CPP
            Debug.Log($"IIconable GetIcon T:{typeof(T).Name} {(Icon ? Icon.name : "NullIcon")}");
#endif
            return Icon;
        }
    }
}
