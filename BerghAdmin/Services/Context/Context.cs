using System;
using BerghAdmin.Data;
using BerghAdmin.Services.Configuration;

namespace BerghAdmin.Services.Context
{
    public class Context
    {
        public SyncfusionConfiguration SyncfusionConfiguration { get; set; }
        public MailJetConfiguration MailJetConfiguration { get; set; }
        
        // reference to the persoon who is currently logged in as user
        // to be done: not used yet
        public Persoon CurrentUser { get; set;}
    }
}
