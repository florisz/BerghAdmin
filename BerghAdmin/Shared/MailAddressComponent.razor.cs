using BerghAdmin.Events;
using BerghAdmin.Pages;
using Microsoft.AspNetCore.Components;

namespace BerghAdmin.Shared
{
    public partial class MailAddressComponent
    {
        [Parameter]
        public MailAddress Address { get; set; } = new(string.Empty, string.Empty);

        [Parameter]
        public List<MailAddress> MailAddresses { get; set; } = new();

        private EditMailAddressDialog editMailAddressDialog = new();

        public void EditMailAddress()
        {
            editMailAddressDialog.Address = new MailAddress(Address.Address, Address.Name);
            editMailAddressDialog.DialogOpen();
        }

        public void DeleteMailAddress()
        {
            MailAddresses.Remove(Address);
        }

        public void MailAddressUpdated(MailAddressUpdatedEventArgs args)
        {
            Address = args.MailAddress;
            StateHasChanged();
        }
    }
}
