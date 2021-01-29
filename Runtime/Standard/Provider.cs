using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin.GameFramework.Standard
{
    public interface IConfig
    {

    }

    public interface IProvider
    {
    }

    public interface IConfigProvider : IProvider
    {

    }

    public interface IConfigProvider<CFG> : IConfigProvider
    {
        List<CFG> Config { get; }
    }

    public interface IConfigProvider<K, V> : IConfigProvider<V>
    {
        Dictionary<K, V> ConfigDic { get; }
    }
}


