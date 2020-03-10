using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XenfbotDN.Filters
{
    public abstract class BaseFilter
    {
        public string FilterName;
        public string FilterAuthor;
        public string FilterVersion;

        public abstract void init();
        public virtual async void Execute(TGMessage message, GroupConfigurationObject gco, string langcode) { }

    }
}
