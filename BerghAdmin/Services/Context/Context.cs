using System;
using BerghAdmin.Data;
using BerghAdmin.Services.Configuration;
using Microsoft.AspNetCore.Identity;

namespace BerghAdmin.Services.Context
{
    public class Context
    {
        public SyncfusionConfiguration SyncfusionConfiguration { get; set; }
        public MailJetConfiguration MailJetConfiguration { get; set; }
        public User CurrentUser { get; set; }
    }
}
