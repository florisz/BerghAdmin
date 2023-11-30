using BerghAdmin.Events;
using BerghAdmin.Pages.Mail;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BerghAdmin.Shared
{
    public partial class MailAddressListComponent
    {
        [Parameter]
        public List<MailAddress> Addresses { get; set; } = new();

        private EditMailAddressDialog editMailAddressDialog = new();

        public void AddMailAddress(MouseEventArgs args)
        {
            editMailAddressDialog.Address = new MailAddress(string.Empty, string.Empty);
            editMailAddressDialog.DialogOpen();
        }

        public void MailAddressUpdated(MailAddressUpdatedEventArgs args)
        {
            if (args?.MailAddress != null)
            {
                Addresses.Add(args.MailAddress);
            }

            StateHasChanged();
        }
    }
}
