using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XenfbotDN.Commands
{
    public abstract class BaseCommand
    {
        public string command;
        public string CommandAuthor;
        public string CommandVersion;
        public bool adminOnly;

        public virtual void init()
        {

        }
        public async virtual Task Execute(TGMessage message, string[] args,GroupConfigurationObject gco, string langcode)
        {

        }
    }
}
