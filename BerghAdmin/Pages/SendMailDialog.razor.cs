using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.RichTextEditor;

namespace BerghAdmin.Pages
{
    public partial class SendMailDialog
    {
        [Parameter]
        public bool IsVisible { get; set; } = false;

        [Inject]
        private ISendMailService _sendMailService { get; set; } = default!;

        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Bold },
            new ToolbarItemModel() { Command = ToolbarCommand.Italic },
            new ToolbarItemModel() { Command = ToolbarCommand.Underline },
            new ToolbarItemModel() { Command = ToolbarCommand.StrikeThrough },
            new ToolbarItemModel() { Command = ToolbarCommand.FontName },
            new ToolbarItemModel() { Command = ToolbarCommand.FontSize },
            new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
            new ToolbarItemModel() { Command = ToolbarCommand.BackgroundColor },
            new ToolbarItemModel() { Command = ToolbarCommand.LowerCase },
            new ToolbarItemModel() { Command = ToolbarCommand.UpperCase },
            new ToolbarItemModel() { Command = ToolbarCommand.SuperScript },
            new ToolbarItemModel() { Command = ToolbarCommand.SubScript },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Formats },
            new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
            new ToolbarItemModel() { Command = ToolbarCommand.NumberFormatList },
            new ToolbarItemModel() { Command = ToolbarCommand.BulletFormatList },
            new ToolbarItemModel() { Command = ToolbarCommand.Outdent },
            new ToolbarItemModel() { Command = ToolbarCommand.Indent },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
            new ToolbarItemModel() { Command = ToolbarCommand.Image },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.ClearFormat },
            new ToolbarItemModel() { Command = ToolbarCommand.Print },
            new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
            new ToolbarItemModel() { Command = ToolbarCommand.FullScreen },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo }
        };

        private MailMessage mailMessage = new();
        private SfRichTextEditor mailBody;
        //TODO: Create API call to set addresses
        //TODO: Use string lists instead of string
        //TODO: Validation
        public string? toAddresses;
        private string? ccAddresses;
        private string? bccAddresses;

        public void DialogOpen()
        {
            IsVisible = true;
            mailBody.RefreshUIAsync();
            StateHasChanged();
        }

        private void OnOverlayclick(MouseEventArgs arg)
        {
            IsVisible = false;
        }

        private async Task SendEMail()
        {
            string htmlContent = await mailBody.GetXhtmlAsync();
            string textContent = await mailBody.GetTextAsync();
            mailMessage.From = new MailAddress("test@berghinhetzadel.nl", "Test");
            if (!string.IsNullOrWhiteSpace(toAddresses))
            {
                mailMessage.To = new() { new MailAddress(toAddresses, null) };
            }
            if (!string.IsNullOrWhiteSpace(ccAddresses))
            {
                mailMessage.Cc = new() { new MailAddress(ccAddresses, null) };
            }
            if (!string.IsNullOrWhiteSpace(bccAddresses))
            {
                mailMessage.Bcc = new() { new MailAddress(bccAddresses, null) };
            }
            mailMessage.TextBody = textContent;
            mailMessage.HtmlBody = htmlContent;

            await _sendMailService.SendMail(mailMessage);

            DialogClose();
        }

        private void DialogClose()
        {
            IsVisible = false;
            StateHasChanged();
        }
    }
}
