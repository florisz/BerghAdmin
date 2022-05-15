using Microsoft.AspNetCore.Components;

namespace BerghAdmin.Shared
{
    public partial class MailAddressComponent
    {
        [Parameter]
        public MailAddress Address { get; set; } = new(string.Empty, string.Empty);
    }
}
