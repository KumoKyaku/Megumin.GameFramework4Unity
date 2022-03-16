using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin.GameFramework
{
    public interface ITimer 
    {
        ValueTask Wait(float seconds);
        ValueTask Wait(long frame);
    }
}


