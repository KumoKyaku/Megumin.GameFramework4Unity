using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Megumin.GameFramework.Numerical
{
    public class NumericalPropertySO : ScriptableObject
    {
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
                generator.Push(@$"public partial class NumericalPropertyTypeDefine");
                using (generator.NewScope)
                {
                    generator.Push(@$"public const string 属性关联后{name} = nameof(属性关联后{name});");
                    generator.Push(@$"public const string {name} = nameof({name});");
                }
            }

            generator.GenerateNear(this);
        }
    }
}
