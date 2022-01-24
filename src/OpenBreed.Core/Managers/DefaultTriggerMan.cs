using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Managers
{
    internal class DefaultTriggerMan : ITriggerMan
    {
        #region Private Fields

        private const int UNKNOWN_ACTION = -1;

        private readonly Dictionary<int, List<ActionPerformer>> onEachPerformers = new Dictionary<int, List<ActionPerformer>>();

        private readonly Dictionary<int, List<ActionPerformer>> onSinglePerformers = new Dictionary<int, List<ActionPerformer>>();

        private readonly Dictionary<string, int> namesToIds = new Dictionary<string, int>();

        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public DefaultTriggerMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public int Register(string actionName)
        {
            if (namesToIds.ContainsKey(actionName))
                throw new InvalidOperationException($"Action '{actionName}' already registered");

            var actionId = namesToIds.Count;
            namesToIds.Add(actionName, actionId);
            return actionId;
        }

        public int GetId(string actionName)
        {
            if (!namesToIds.TryGetValue(actionName, out int actionId))
                return UNKNOWN_ACTION;

            return actionId;
        }

        public void Fire(int actionId, params object[] args)
        {
            if (!ValidateActionId(actionId))
                return;

            //Process on each performers
            if (!onEachPerformers.TryGetValue(actionId, out List<ActionPerformer> performers))
                return;

            foreach (var performer in performers)
                performer.Invoke(args);

            //Process single performers
            if (!onSinglePerformers.TryGetValue(actionId, out performers))
                return;

            foreach (var performer in performers)
                performer.Invoke(args);

            onSinglePerformers.Remove(actionId);
        }

        public void Fire(string actionName, params object[] args)
        {
            if (!ValidateActionName(actionName, out int actionId))
                return;

            Fire(actionId, args);
        }

        public void OnEachAction(int actionId, ActionPerformer performer)
        {
            if (!onEachPerformers.TryGetValue(actionId, out List<ActionPerformer> performers))
            {
                performers = new List<ActionPerformer>();
                onEachPerformers.Add(actionId, performers);
            }

            performers.Add(performer);
        }

        public void OnEachAction(string actionName, ActionPerformer performer)
        {
            var actionId = GetId(actionName);

            if (actionId != UNKNOWN_ACTION)
                OnEachAction(actionId, performer);
        }

        public void OnSingleAction(int actionId, ActionPerformer performer)
        {
            if (!onSinglePerformers.TryGetValue(actionId, out List<ActionPerformer> performers))
            {
                performers = new List<ActionPerformer>();
                onSinglePerformers.Add(actionId, performers);
            }

            performers.Add(performer);
        }

        public void OnSingleAction(string actionName, ActionPerformer performer)
        {
            var actionId = GetId(actionName);

            if (actionId != UNKNOWN_ACTION)
                OnSingleAction(actionId, performer);
        }

        #endregion Public Methods

        #region Private Methods

        private bool ValidateActionName(string actionName, out int actionId)
        {
            actionId = GetId(actionName);

            if (actionId != UNKNOWN_ACTION)
                return true;

            logger.Error($"Action '{actionName}' not registered.");
            return false;
        }

        private bool ValidateActionId(int actionId)
        {
            if (actionId >= 0 && actionId < namesToIds.Count)
                return true;

            logger.Error($"Action ID '{actionId}' not registered.");
            return false;
        }

        #endregion Private Methods
    }
}