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
            editMailAddressDialog.Address = Address;
            editMailAddressDialog.DialogOpen();
        }

        public void DeleteMailAddress()
        {
            MailAddresses.Remove(Address);
        }
    }
}
