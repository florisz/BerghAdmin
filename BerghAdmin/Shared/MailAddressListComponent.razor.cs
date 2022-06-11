using BerghAdmin.Pages;
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
            var mailAddress = new MailAddress(string.Empty, string.Empty);
            Addresses.Add(mailAddress);
            StateHasChanged();

            editMailAddressDialog.Address = mailAddress;
            editMailAddressDialog.DialogOpen();
        }
    }
}
