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
        _window = new FaxlessPrinterWindow();

        _window.OnSend += () => SendMessage(new FaxSendMessage());

        _window.OnLoadPaper += () =>
        {
            // Reuse FaxFileMessage to simulate loading paper
            SendMessage(new FaxFileMessage(null, string.Empty, true));
        };

        _window.OnRecipientChanged += recipient => SendMessage(new FaxDestinationMessage(recipient));

        _window.OpenCentered();
    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage msg)
    {
        // Handle only messages if you have any custom ones for FaxlessPrinter
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        if (_window == null)
            return;

        if (state is FaxUiState faxState)
        {
            // Wrap the original FaxUiState into your FaxlessPrinterUiState
            _window.UpdateState(new FaxlessPrinterUiState(faxState));
        }
    }
}
