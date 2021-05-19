using System;

namespace OpenBreed.Common
{
    internal class DataLoader
    {
        private readonly IDataLoaderFactory dataLoaderFactory;

        public DataLoader(IDataLoaderFactory dataLoaderFactory)
        {
            this.dataLoaderFactory = dataLoaderFactory;
        }

        internal TInterface Load<TInterface>(string entryId)
        {
            var loader = dataLoaderFactory.GetLoader<TInterface>();

            return loader.Load(entryId);
        }
    }
}