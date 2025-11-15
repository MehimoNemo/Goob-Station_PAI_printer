using Content.Shared.Fax;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Content.Goobstation.Client.FaxlessPrinter.UI;

namespace Content.Goobstation.Client.FaxlessPrinter.UI;

[UsedImplicitly]
public sealed class FaxlessPrinterBoundUi : BoundUserInterface
{
    private FaxlessPrinterWindow? _window;

    public FaxlessPrinterBoundUi(EntityUid owner, Enum uiKey) : base(owner, uiKey) { }

    protected override void Open()
    {
        if (_window != null)
            return;

        _window = new FaxlessPrinterWindow();

        // Button events
        _window.OnSend += () =>
        {
            Logger.Info("Fax Send button pressed!");
            SendMessage(new FaxSendMessage());
        };
        _window.OnLoadPaper += () => SendMessage(new FaxFileMessage(null, "", true));
        _window.OnCopy += () => SendMessage(new FaxCopyMessage());

        _window.OnClose += () =>
        {
            _window?.CloseWindow();
            _window = null;
        };

        _window.OpenCentered();
    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage msg)
    {
        // Optional: handle custom messages
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        if (_window == null || state is not FaxUiState faxState)
            return;

        _window.UpdateState(new FaxlessPrinterUiState(faxState));
    }
}
