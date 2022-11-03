using BerghAdmin.Events;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.RichTextEditor;

namespace BerghAdmin.Pages
{
    public partial class SendMailDialog
    {
        [Parameter]
        public EventCallback<MailMessageConfiguredEventArgs> OnMailMessageConfigured { get; set; }

        public bool IsVisible { get; set; } = false;

        private MailMessage Message { get; set; } = new MailMessage();

        private bool _showCc = false;

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

        private SfRichTextEditor _mailBodyEditor = new();
        private SfTextBox _subjectEditor = new();

        public void DialogOpen(MailMessage message)
        {
            Message = message;
            IsVisible = true;
            StateHasChanged();
        }

        private async Task Opened(Syncfusion.Blazor.Popups.OpenEventArgs args)
        {
            args.PreventFocus = true;
            await _subjectEditor.FocusIn();
            await _mailBodyEditor.RefreshUIAsync();
        }

        private async Task SaveMessage()
        {
            string textContent = await _mailBodyEditor.GetTextAsync();
            Message.TextBody = textContent;
            Message.HtmlBody = _mailBodyEditor.Value;

            await OnMailMessageConfigured.InvokeAsync(new MailMessageConfiguredEventArgs(Message));

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
