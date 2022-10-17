using BerghAdmin.Events;
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
        [Parameter]
        public EventCallback<MailAddressUpdatedEventArgs> OnMailAddressUpdated { get; set; }

        public void DialogOpen()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        public async Task SaveMailAddress()
        {
            CloseDialog();
            StateHasChanged();

            await OnMailAddressUpdated.InvokeAsync(new MailAddressUpdatedEventArgs(Address));
        }

        public async Task DeleteMailAddress()
        {
            MailAddresses.Remove(Address);
            StateHasChanged();

            await OnMailAddressUpdated.InvokeAsync(new MailAddressUpdatedEventArgs(Address));
        }

        public void CloseDialog()
        {
            ShowDialog = false;
        }

        public void OnOverlayModalClick()
        {
            CloseDialog();
        }

    }
}
