using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class DependentAttribute : Attribute
    {
        private string _foreignKey;
        public DependentAttribute(string foreignKey)
        {
            _foreignKey = foreignKey;
        }

        public virtual string ForeignKey
        {
            get
            {
                return _foreignKey;
            }
            set
            {
                _foreignKey = value;
            }
        }
    }
}
