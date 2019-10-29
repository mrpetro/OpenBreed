namespace OpenBreed.Core
{
    public class Item
    {
        #region Public Constructors

        public Item(string name)
        {
            Id = -1;
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; protected set; }
        public int Id { get; internal set; }

        #endregion Public Properties
    }
}