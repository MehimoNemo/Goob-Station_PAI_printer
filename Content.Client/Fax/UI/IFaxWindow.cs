using Content.Shared.Fax;

public interface IFaxWindow
{
    event Action? FileButtonPressed;
    event Action? CopyButtonPressed;
    event Action? SendButtonPressed;
    event Action? RefreshButtonPressed;
    event Action<string>? PeerSelected;

    void UpdateState(FaxUiState state);
    void Dispose();
    bool Disposed { get; }
    bool OfficePaper { get; }
}
