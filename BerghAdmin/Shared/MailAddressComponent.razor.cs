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

        [Parameter]
        public EventCallback<MailAddressUpdatedEventArgs> OnMailAddressUpdated { get; set; }

        private EditMailAddressDialog editMailAddressDialog = new();

        public void EditMailAddress()
        {
            editMailAddressDialog.Address = new MailAddress(Address.Address, Address.Name, Address.DonateurId);
            editMailAddressDialog.DialogOpen();
        }

        public async Task DeleteMailAddress()
        {
            MailAddresses.Remove(Address);
            await OnMailAddressUpdated.InvokeAsync(new MailAddressUpdatedEventArgs(null));
        }

        public void MailAddressUpdated(MailAddressUpdatedEventArgs args)
        {
            if (args?.MailAddress != null)
            {
                Address.Name = args.MailAddress.Name;
                Address.Address = args.MailAddress.Address;
            }
            StateHasChanged();
        }
    }
}
