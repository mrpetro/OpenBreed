using OpenBreed.Gui.Abstractions.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface ICheckFieldBuilder : IElementBuilder
    {
        void BindIsChecked<TTarget>(TTarget target, Expression<Func<TTarget, bool>> properyExpression);

        void BindProperty(PropertyBinding<bool>? binding);

        void SetChecked(bool isChecked);

    }
}
