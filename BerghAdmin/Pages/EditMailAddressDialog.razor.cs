using Microsoft.AspNetCore.Components;

namespace BerghAdmin.Pages
{
    public partial class EditMailAddressDialog
    {

        public MailAddress Address { get; set; } = new(string.Empty, string.Empty);

        [Parameter]
        public bool ShowDialog { get; set; } = false;
        [Parameter]
        public List<MailAddress> MailAddresses { get; set; } = new();

        public void DialogOpen()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        public void SaveMailAddress()
        {
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
