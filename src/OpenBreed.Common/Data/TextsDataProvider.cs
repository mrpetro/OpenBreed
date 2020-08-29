using OpenBreed.Common.Texts;
using OpenBreed.Database.Interface.Items.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class TextsDataProvider
    {
        #region Public Constructors

        public TextsDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties


        private TextModel GetModelImpl(ITextFromMapEntry entry)
        {
            return TextsDataHelper.FromMapModel(Provider, entry);
        }

        private TextModel GetModelImpl(ITextEmbeddedEntry entry)
        {
            return TextsDataHelper.FromBinary(Provider, entry);
        }

        private TextModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        public TextModel GetText(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<ITextEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Text error: " + id);

            return GetModel(entry);
        }
    }
}

