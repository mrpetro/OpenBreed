//using OpenBreed.Gui.Interface.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Gui.Interface.Extensions
//{
//    internal static class ElementBuilderExtensions
//    {
//        internal static IElementBuilder AddLabel(this ElementBuilder parentBuilder, Action<ILabelBuilder> setter)
//        {
//            var builder = new LabelBuilder(parentBuilder);

//            setter.Invoke(builder);

//            return builder.FinishElement();
//        }

//        internal static IElementBuilder AddPanel(this ElementBuilder parentBuilder, Action<IPanelBuilder> setter)
//        {
//            var builder = new PanelBuilder(parentBuilder);

//            setter.Invoke(builder);

//            return builder.FinishElement();
//        }

//        internal static IElementBuilder AddButton(this ElementBuilder parentBuilder, Action<IButtonBuilder> setter)
//        {
//            var builder = new ButtonBuilder(parentBuilder);

//            setter.Invoke(builder);

//            return builder.FinishElement();
//        }

//        internal static IElementBuilder AddCheckbox(this ElementBuilder parentBuilder, Action<ICheckboxBuilder> setter)
//        {
//            var builder = new CheckboxBuilder(parentBuilder);

//            setter.Invoke(builder);

//            return builder.FinishElement();
//        }
//    }
//}
