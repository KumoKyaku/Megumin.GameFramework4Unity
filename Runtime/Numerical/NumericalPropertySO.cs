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
        ItemPropertyPostCombind,
    }

    public class NumericalPropertySO : ScriptableObject
    {
        public NumericalPropertySOType Type = NumericalPropertySOType.ItemPropertyPostCombind;
        public TextAsset Template;

        [Button]
        public void GenericCode()
        {
            TemplateCodeGenerator generator = new TemplateCodeGenerator();
            generator.Template = Template;
            generator.GenerateNear(this);
        }


        public void GenericCode2()
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
                    generator.Push(@$"public const string {name}基础值 = nameof({name}基础值);");

                    generator.Push("");
                    generator.Push(@$"public const string {name}装备固定加成 = nameof({name}装备固定加成);");
                    generator.Push(@$"public const string {name}装备系数加成 = nameof({name}装备系数加成);");
                    generator.Push(@$"public const string {name}装备加成后总计 = nameof({name}装备加成后总计);");

                    generator.Push("");
                    generator.Push(@$"public const string {name}属性关联固定加成 = nameof({name}属性关联固定加成);");
                    generator.Push(@$"public const string {name}属性关联系数加成 = nameof({name}属性关联系数加成);");
                    generator.Push(@$"public const string {name}属性关联后总计 = nameof({name}属性关联后总计);");

                    generator.Push("");
                    generator.Push(@$"public const string {name}后期固定加成 = nameof({name}后期固定加成);");
                    generator.Push(@$"public const string {name}后期系数加成 = nameof({name}后期系数加成);");
                    generator.Push(@$"public const string {name} = nameof({name});");
                }

                generator.Push("");
                generator.Push(@$"public partial class {name}_Property_Generic");
                using (generator.NewScope)
                {
                    generator.Push(@$"public ConstValuePorperty 基础值 = new() {{Type = NumericalPropertyTypeDefine.{name}基础值}};");

                    generator.Push("");
                    generator.Push(@$"public SumChildPopperty 装备固定加成 = new() {{Type = NumericalPropertyTypeDefine.{name}装备固定加成}};");
                    generator.Push(@$"public SumChildPopperty 装备系数加成 = new() {{Type = NumericalPropertyTypeDefine.{name}装备系数加成}};");
                    generator.Push(@$"public LayerProperty 装备加成后总计 = new() {{Type = NumericalPropertyTypeDefine.{name}装备加成后总计}};");

                    generator.Push("");
                    generator.Push(@$"public SumChildPopperty 属性关联固定加成 = new() {{Type = NumericalPropertyTypeDefine.{name}属性关联固定加成}};");
                    generator.Push(@$"public SumChildPopperty 属性关联系数加成 = new() {{Type = NumericalPropertyTypeDefine.{name}属性关联系数加成}};");
                    generator.Push(@$"public LayerProperty 属性关联后总计 = new() {{Type = NumericalPropertyTypeDefine.{name}属性关联后总计}};");

                    generator.Push("");
                    generator.Push(@$"public SumChildPopperty 后期固定加成 = new() {{Type = NumericalPropertyTypeDefine.{name}后期固定加成}};");
                    generator.Push(@$"public SumChildPopperty 后期系数加成 = new() {{Type = NumericalPropertyTypeDefine.{name}后期系数加成}};");
                    generator.Push(@$"public LayerProperty 面板值 = new() {{Type = NumericalPropertyTypeDefine.{name}}};");
                }
            }

            generator.GenerateNear(this);
        }

        public CSCodeGenerator GetMemberBindDicCSCodeGenerator()
        {
            CSCodeGenerator generator = new CSCodeGenerator();
            //generator.Push("");
            //generator.Push(@$"{name}装备加成后总计.Add({name}基础值);");
            //generator.Push(@$"{name}装备加成后总计.Add({name}装备固定加成);");
            //generator.Push(@$"{name}装备加成后总计.SetLayerScale({name}装备系数加成);");

            //generator.Push(@$"{name}属性关联后总计.Add({name}装备加成后总计);");
            //generator.Push(@$"{name}属性关联后总计.Add({name}属性关联固定加成);");
            //generator.Push(@$"{name}属性关联后总计.SetLayerScale({name}属性关联系数加成);");

            //generator.Push(@$"{name}.Add({name}属性关联后总计);");
            //generator.Push(@$"{name}.Add({name}后期固定加成);");
            //generator.Push(@$"{name}.SetLayerScale({name}后期系数加成);");
            return generator;
        }

        public CSCodeGenerator GetMemberAddDicCSCodeGenerator()
        {
            CSCodeGenerator generator = new CSCodeGenerator();
            //ExAddDic(generator);
            generator.Push("");
            generator.Push(@$"foreach (var item in {name}.allP)");
            using (generator.NewScope)
            {
                generator.Push(@$"allP.Add(item.Key, item.Value);");
            }
            return generator;
        }

        private void ExAddDic(CSCodeGenerator generator)
        {
            generator.Push("");
            generator.Push(@$"allP.Add({name}基础值.Type, {name}基础值);");
            generator.Push(@$"allP.Add({name}装备固定加成.Type, {name}装备固定加成);");
            generator.Push(@$"allP.Add({name}装备系数加成.Type, {name}装备系数加成);");
            generator.Push(@$"allP.Add({name}装备加成后总计.Type, {name}装备加成后总计);");

            generator.Push(@$"allP.Add({name}属性关联固定加成.Type, {name}属性关联固定加成);");
            generator.Push(@$"allP.Add({name}属性关联系数加成.Type, {name}属性关联系数加成);");
            generator.Push(@$"allP.Add({name}属性关联后总计.Type, {name}属性关联后总计);");

            generator.Push(@$"allP.Add({name}后期固定加成.Type, {name}后期固定加成);");
            generator.Push(@$"allP.Add({name}后期系数加成.Type, {name}后期系数加成);");
            generator.Push(@$"allP.Add({name}.Type, {name});");
        }

        public CSCodeGenerator GetMemberCSCodeGenerator()
        {
            CSCodeGenerator generator = new CSCodeGenerator();
            generator.Push(@$"public {name}_Property_Generic {name} = new();");
            //ExPORTpORP(generator);
            return generator;
        }

        /// <summary>
        /// 展开属性
        /// </summary>
        /// <param name="generator"></param>
        private void ExPORTpORP(CSCodeGenerator generator)
        {
            generator.Push("");
            generator.Push(@$"//{name}相关字段");
            generator.Push(@$"public ConstValuePorperty {name}基础值 = new() {{Type = NumericalPropertyTypeDefine.{name}基础值}};");

            generator.Push("");
            generator.Push(@$"public SumChildPopperty {name}装备固定加成 = new() {{Type = NumericalPropertyTypeDefine.{name}装备固定加成}};");
            generator.Push(@$"public SumChildPopperty {name}装备系数加成 = new() {{Type = NumericalPropertyTypeDefine.{name}装备系数加成}};");
            generator.Push(@$"public LayerProperty {name}装备加成后总计 = new() {{Type = NumericalPropertyTypeDefine.{name}装备加成后总计}};");

            generator.Push("");
            generator.Push(@$"public SumChildPopperty {name}属性关联固定加成 = new() {{Type = NumericalPropertyTypeDefine.{name}属性关联固定加成}};");
            generator.Push(@$"public SumChildPopperty {name}属性关联系数加成 = new() {{Type = NumericalPropertyTypeDefine.{name}属性关联系数加成}};");
            generator.Push(@$"public LayerProperty {name}属性关联后总计 = new() {{Type = NumericalPropertyTypeDefine.{name}属性关联后总计}};");

            generator.Push("");
            generator.Push(@$"public SumChildPopperty {name}后期固定加成 = new() {{Type = NumericalPropertyTypeDefine.{name}后期固定加成}};");
            generator.Push(@$"public SumChildPopperty {name}后期系数加成 = new() {{Type = NumericalPropertyTypeDefine.{name}后期系数加成}};");
            generator.Push(@$"public LayerProperty {name} = new() {{Type = NumericalPropertyTypeDefine.{name}}};");
        }
    }
}
