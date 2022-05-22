using Microsoft.AspNetCore.Components;

namespace BerghAdmin.Shared
{
    public partial class MailAddressComponent
    {
        [Parameter]
        public MailAddress Address { get; set; } = new(string.Empty, string.Empty);
        [Parameter]
        public List<MailAddress> MailAddresses { get; set; } = new();

        private MailAddress _editMailAddress = new(string.Empty, string.Empty);

        private bool ShowDialog = false;

        public void EditMailAddress()
        {
            _editMailAddress = Address;
            ShowDialog = true;
        }

        public void SaveMailAddress()
        {
            Address = _editMailAddress;
            CloseDialog();
            StateHasChanged();
        }

        public void DeleteMailAddress()
        {
            MailAddresses.Remove(Address);
            StateHasChanged();
        }

        public void CloseDialog()
        {
            ShowDialog = false;
        }

        public void OnOverlayclick()
        {
            CloseDialog();
        }
    }
}
