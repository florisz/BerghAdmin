using System;
using BerghAdmin.Services.Configuration;
using Microsoft.Extensions.Configuration;

namespace BerghAdmin.Services.Context
{
    public class ContextService : IContextService
    {
        private Context _context;
        private IConfiguration _configuration;

        public ContextService(IConfiguration configuration)
        {
            _context = new Context();
            _configuration = configuration;

            
            _context.SyncfusionConfiguration = _configuration.GetSection("SyncfusionConfiguration").Get<SyncfusionConfiguration>();
            if (_context.SyncfusionConfiguration == null)
            {
                throw new ApplicationException("Secrets for Syncfusion configuration can not be found in configuration");
            }
            _context.MailJetConfiguration = _configuration.GetSection("MailJetConfiguration").Get<MailJetConfiguration>();
            if (_context.MailJetConfiguration == null)
            {
                throw new ApplicationException("Secrets for MailJet configuration can not be found in configuration");
            }
        }

        public Context Context 
        { 
            get => _context; 
        }

    }
}
