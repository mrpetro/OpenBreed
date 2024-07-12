//using OpenBreed.Editor.VM.Base;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Editor.VM
//{
//    public class ViewFinder
//    {
//        private readonly Dictionary<BaseViewModel, IView> _viewDictionary = new Dictionary<BaseViewModel, IView>();

//        public void BindView(BaseViewModel vm, IView view)
//        {
//            if (_viewDictionary.ContainsKey(vm))
//                throw new InvalidOperationException($"There is already view registered for {vm}.");

//            _viewDictionary.Add(vm, view);
//        }

//        public IView GetView<T>(BaseViewModel vm) where T : IView
//        {
//            IView foundView = null;

//            _viewDictionary.TryGetValue(vm, out foundView);

//            return foundView;
//        }
//    }
//}
