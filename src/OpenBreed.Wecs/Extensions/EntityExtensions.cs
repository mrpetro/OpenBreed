using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static IEnumerable<string> GetActionsOnTrigger(this IEntity entity, string triggerName)
        {
            var sc = entity.TryGet<OnTriggerComponent>();

            if (sc is null)
            {
                yield break;
            }

            foreach (var action in sc.Actions.Where(item => item.Trigger == triggerName))
            {
                yield return action.Action;
            }
        }

        #endregion Public Methods
    }
}