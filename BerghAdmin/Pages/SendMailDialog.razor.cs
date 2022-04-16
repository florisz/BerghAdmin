using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.RichTextEditor;

namespace BerghAdmin.Pages
{
    public partial class SendMailDialog
    {
        [Parameter]
        public bool IsVisible { get; set; } = false;

        [Inject]
        private ISendMailService SendMailService { get; set; } = default!;

        private readonly List<ToolbarItemModel> Tools = new()
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


        private MailMessage _message = new();
        public MailMessage Message
        {
            get => _message;
            set
            {
                _message = value;
                _fromAddress = _message.From == null ? "" : _message.From.Address;
                _toAddresses = _message.To == null ? "" : string.Join(';', _message.To.Select(x => x.Address));
                _ccAddresses = _message.Cc == null ? "" : string.Join(';', _message.Cc.Select(x => x.Address));
                _bccAddresses = _message.Bcc == null ? "" : string.Join(';', _message.Bcc.Select(x => x.Address));
            }
        }

        private SfRichTextEditor _mailBody = new();
        private SfTextBox _subjectInput = new();

        private string _fromAddress = "";
        private string? _toAddresses = "";
        private string? _ccAddresses = "";
        private string? _bccAddresses = "";

        public async Task DialogOpen()
        {
            IsVisible = true;
            await _mailBody.RefreshUIAsync();
            StateHasChanged();
        }

        private void Opened(Syncfusion.Blazor.Popups.OpenEventArgs args)
        {
            args.PreventFocus = true;
            _subjectInput.FocusIn();
        }

        private async Task SendEMail()
        {
            string htmlContent = await _mailBody.GetXhtmlAsync();
            string textContent = await _mailBody.GetTextAsync();
            Message.From = new MailAddress(_fromAddress, null);
            if (!string.IsNullOrWhiteSpace(_toAddresses))
            {
                List<MailAddress> toAddresses = _toAddresses
                    .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(a => new MailAddress(a, null))
                    .ToList();
                Message.To = toAddresses;
            }
            if (!string.IsNullOrWhiteSpace(_ccAddresses))
            {
                List<MailAddress> ccAddresses = _ccAddresses
                    .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(a => new MailAddress(a, null))
                    .ToList();
                Message.Cc = ccAddresses;
            }
            if (!string.IsNullOrWhiteSpace(_bccAddresses))
            {
                List<MailAddress> bccAddresses = _bccAddresses
                    .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(a => new MailAddress(a, null))
                    .ToList();
                Message.Bcc = bccAddresses;
            }
            Message.TextBody = textContent;
            Message.HtmlBody = htmlContent;

            bool isSandboxMode = false; // If SandboxMode is set to true, no mails are actually sent, so great for testing.
            await SendMailService.SendMail(Message, isSandboxMode);

            DialogClose();
        }

        private void DialogClose()
        {
            IsVisible = false;
            StateHasChanged();
        }

        private void OnOverlayclick(MouseEventArgs arg)
        {
            DialogClose();
        }
    }
}
