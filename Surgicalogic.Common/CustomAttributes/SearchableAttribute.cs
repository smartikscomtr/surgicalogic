using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class SearchableAttribute : Attribute
    {
        private bool searchableNestedProperty;
        public SearchableAttribute(bool SearchableNestedProperty = false)
        {
            searchableNestedProperty = SearchableNestedProperty;
        }

        public virtual bool SearchableNestedProperty
        {
            get
            {
                return searchableNestedProperty;
            }
            set
            {
                searchableNestedProperty = value;
            }
        }
    }
}
