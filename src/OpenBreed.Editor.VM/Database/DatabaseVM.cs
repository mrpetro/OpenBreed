//using OpenBreed.Database.Interface;
//using OpenBreed.Editor.VM.Base;
//using System;
//using System.Collections.Generic;

//namespace OpenBreed.Editor.VM.Database
//{
//    public enum ProjectState
//    {
//        Closed,
//        New,
//        Opened,
//        Closing
//    }

//    public class DatabaseVM : BaseViewModel, IDisposable
//    {
//        #region Private Fields

//        private readonly IUnitOfWork unitOfWork;
//        private string name;

//        #endregion Private Fields

//        #region Internal Constructors

//        internal DatabaseVM(IUnitOfWork unitOfWork)
//        {
//            this.unitOfWork = unitOfWork;

//            Name = unitOfWork.Name;
//        }

//        #endregion Internal Constructors

//        #region Public Properties

//        public bool IsModified { get; internal set; }

//        public string Name
//        {
//            get { return name; }
//            set { SetProperty(ref name, value); }
//        }

//        #endregion Public Properties

//        #region Public Methods

//        public void Dispose()
//        {
//        }

//        #endregion Public Methods

//        #region Internal Methods

//        internal IEnumerable<string> GetTableNames()
//        {
//            foreach (var repository in unitOfWork.Repositories)
//                yield return repository.Name;
//        }

//        #endregion Internal Methods
//    }
//}