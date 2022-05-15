using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Buttons;

namespace BerghAdmin.Shared
{
    public partial class MailAddressListComponent
    {
        [Parameter]
        public List<MailAddress> Addresses { get; set; } = new();

        public void AddMailAddress(MouseEventArgs args)
        {
            var mailAddress = new MailAddress("ad@test.xyz", "Adje");
            Addresses.Add(mailAddress);
        }
    }
}
