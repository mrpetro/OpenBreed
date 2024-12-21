using OpenBreed.Gui.Abstractions.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface IStateboxBuilder : IElementBuilder
    {
        void BindIsChecked<TTarget>(TTarget target, Expression<Func<TTarget, bool>> properyExpression);

        void BindIsChecked(PropertyBinding<bool>? binding);

        void SetChecked(bool isChecked);

    }
}
