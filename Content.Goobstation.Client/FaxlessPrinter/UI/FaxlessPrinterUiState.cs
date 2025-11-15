using Content.Shared.Fax;

namespace Content.Goobstation.Client.FaxlessPrinter.UI;

public sealed class FaxlessPrinterUiState
{
    public bool CanCopy { get; }
    public bool IsPaperInserted { get; }

    public FaxlessPrinterUiState(FaxUiState original)
    {
        CanCopy = original.CanCopy;
        IsPaperInserted = original.IsPaperInserted;
    }
}
