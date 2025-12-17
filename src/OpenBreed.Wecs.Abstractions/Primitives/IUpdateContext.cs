namespace OpenBreed.Wecs.Abstractions.Primitives
{
    public interface IUpdateContext
    {
        #region Public Properties

        float Dt { get; }

        float DtMultiplier { get; }

        bool Paused { get; set; }

        int WorldId { get; }

        #endregion Public Properties
    }
}