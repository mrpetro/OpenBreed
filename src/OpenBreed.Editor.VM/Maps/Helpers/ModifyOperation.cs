using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Editor.VM.Maps.Helpers
{
    class ModifyOperation
    {
        private readonly bool m_IsModifiedBefore;
        private readonly bool m_IsModifiedAfter;

        public ModifyOperation(bool isModifiedBefore, bool isModifiedAfter)
        {
            m_IsModifiedBefore = isModifiedBefore;
            m_IsModifiedAfter = isModifiedAfter;
        }
    }
}
