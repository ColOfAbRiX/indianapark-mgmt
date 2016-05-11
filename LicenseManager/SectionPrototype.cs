using System;

namespace IndianaPark.LicenseManager
{
    public class SectionPrototype
    {
        public SectionPrototype Parent { get; private set;  }

        public SectionPrototype( SectionPrototype parent )
        {
            this.Parent = parent;
        }
    }
}
