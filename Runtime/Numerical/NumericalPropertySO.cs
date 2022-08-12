using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Megumin.GameFramework.Numerical
{
    public enum NumericalPropertySOType
    {
        Const,
        ItemAdd,
        ItemPropertyPostCombind,
    }

    public class NumericalPropertySO : ScriptableObject
    {
        public NumericalPropertySOType Type = NumericalPropertySOType.ItemPropertyPostCombind;
        public TextAsset Template;

        [Button]
        public void GenericCode()
        {
            CodeGenerator generator = null;
            switch (Type)
            {
                case NumericalPropertySOType.Const:
                    generator = GetCSCodeGenerator();
                    break;
                case NumericalPropertySOType.ItemAdd:
                    generator = GetTemlateGenerator();
                    break;
                case NumericalPropertySOType.ItemPropertyPostCombind:
                    generator = GetTemlateGenerator();
                    break;
                default:
                    break;
            }
            generator.GenerateNear(this);
        }

        private TemplateCodeGenerator GetTemlateGenerator()
        {
            TemplateCodeGenerator generator = new TemplateCodeGenerator();
            generator.Template = Template;
            return generator;
        }

        public CSCodeGenerator GetCSCodeGenerator()
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
                    generator.Push(@$"public const string {name} = nameof({name});");
                }
            }

            return generator;
        }


        public CSCodeGenerator GetMemberCSCodeGenerator()
        {
            CSCodeGenerator generator = new CSCodeGenerator();
            switch (Type)
            {
                case NumericalPropertySOType.Const:
                    generator.Push(@$"public ConstValuePorperty {name} = new() {{ Type = NumericalPropertyTypeDefine.{name} }};");
                    break;
                case NumericalPropertySOType.ItemAdd:
                    generator.Push(@$"public {name}_Property_Generic {name} = new();");
                    break;
                case NumericalPropertySOType.ItemPropertyPostCombind:
                    generator.Push(@$"public {name}_Property_Generic {name} = new();");
                    break;
                default:
                    generator.Push(@$"public {name}_Property_Generic {name} = new();");
                    break;
            }

            return generator;
        }

        public CSCodeGenerator GetMemberAddDicCSCodeGenerator()
        {
            CSCodeGenerator generator = new CSCodeGenerator();
            switch (Type)
            {
                case NumericalPropertySOType.Const:
                    generator.Push("");
                    generator.Push(@$"allP.Add({name}.Type, {name});");
                    break;
                case NumericalPropertySOType.ItemAdd:
                    GetMemberFromSub(generator);
                    break;
                case NumericalPropertySOType.ItemPropertyPostCombind:
                    GetMemberFromSub(generator);
                    break;
                default:
                    GetMemberFromSub(generator);
                    break;
            }
            return generator;
        }

        private void GetMemberFromSub(CSCodeGenerator generator)
        {
            generator.Push("");
            generator.Push(@$"foreach (var item in {name}.allP)");
            using (generator.NewScope)
            {
                generator.Push(@$"allP.Add(item.Key, item.Value);");
            }
        }
    }
}
