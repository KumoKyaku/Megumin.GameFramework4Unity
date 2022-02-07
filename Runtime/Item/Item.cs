using Megumin.GameFramework.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin.GameFramework.Item
{
    //物品分为实例物品和虚物品.
    //虚物品指一个假象物品,没有实例ID,真实不存在.虚物品不受堆叠限制.
    //例如完成任务需要提供100个苹果,虚物品是苹果,堆叠数是100.
    //
    //背包和物品 不应该是强相关的, 应该是弱相关的. 
    //一个东西能不能放在背包里,和是不是物品无关.
    //
    //Q: 最大堆叠数是背包的属性还是物品属性?
    //A: 应该还是物品属性. 因为实例ID的关系, 如果超过最大堆叠,应该有2个实例ID.

    //物品  实例的和非实例的.
    //虚的和非虚的. 是不是虚物品和实例性无关.

    public partial class Item 
    {
        public bool IsVisual { get; protected set; }  
    }

    /// <summary>
    /// 用于任务物品显示
    /// <para>不是配置，因为可以指示数量；也不是真实物品</para>
    /// </summary>
    public partial class Item<CFG> : Item
    {
        public CFG Config { get; protected set; }
    }

}
