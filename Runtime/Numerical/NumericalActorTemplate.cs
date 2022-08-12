using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Numerical
{
    public class NumericalActorTemplate : ScriptableObject
    {
        public List<NumericalPropertySO> Property = new List<NumericalPropertySO>();

        [Button]
        public void GenericCode()
        {
            CSCodeGenerator generator = new CSCodeGenerator();

            generator.Push(@$"using System;");
            generator.Push(@$"using System.Collections;");
            generator.Push(@$"using System.Collections.Generic;");
            generator.Push(@$"using UnityEngine;");
            generator.Push("");
            generator.Push(@$"namespace Megumin.GameFramework.Numerical");

            using (generator.NewScope)
            {
                generator.Push(@$"public partial class {name}");
                using (generator.NewScope)
                {
                    //生成字段
                    generator.Push(@$"Dictionary<string, ChildPorperty> allP = new Dictionary<string, ChildPorperty>();");

                    foreach (var item in Property)
                    {
                        var g = item.GetMemberCSCodeGenerator();
                        generator.Push(g);
                    }

                    //构造函数
                    generator.Push(@$"");
                    generator.Push(@$"public {name}()");
                    using (generator.NewScope)
                    {
                        generator.Push(@$"allP.Clear();");

                        generator.Push(@$"");
                        foreach (var item in Property)
                        {
                            var g = item.GetMemberAddDicCSCodeGenerator();
                            generator.Push(g);
                        }

                        generator.Push(@$"");
                        foreach (var item in Property)
                        {
                            var g = item.GetMemberBindDicCSCodeGenerator();
                            generator.Push(g);
                        }

                        //开启值传播
                        generator.Push(@$"");
                        generator.Push(@$"foreach (var item in allP)");
                        using (generator.NewScope)
                        {
                            generator.Push(@$"item.Value.BroadCast = true;");
                        }
                    }
                }
            }

            generator.GenerateNear(this);
        }
    }
}
