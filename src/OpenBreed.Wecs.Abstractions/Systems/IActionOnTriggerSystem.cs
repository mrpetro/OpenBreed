namespace OpenBreed.Wecs.Abstractions.Systems
{
    public interface IActionOnTriggerSystem : ISystem
    {
        /// <summary>
        /// Name of trigger
        /// </summary>
        string TriggerName { get; }

        /// <summary>
        /// Name of callback action
        /// </summary>
        string ActionName { get; }

    }
}