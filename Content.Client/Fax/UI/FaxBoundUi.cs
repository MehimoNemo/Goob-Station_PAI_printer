// SPDX-FileCopyrightText: 2023 Arimah Greene <30327355+arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2024 Guilherme Ornel <86210200+joshepvodka@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 exincore <me@exin.xyz>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.IO;
using Content.Shared.Fax;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Log;

namespace Content.Client.Fax.UI;

[UsedImplicitly]
public sealed class FaxBoundUi : BoundUserInterface
{
    [Dependency] private readonly IFileDialogManager _fileDialogManager = default!;

    [ViewVariables]
    private FaxWindow? _window;

    private bool _dialogIsOpen = false;
    private FaxUiState? _lastState;

    public FaxBoundUi(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        Logger.InfoS("fax", $"Opening FaxBoundUi for entity {Owner}");

        if (_lastState?.IsFaxless == true)
        {
            Logger.InfoS("fax", "Creating FaxlessWindow");
            _window = this.CreateWindow<FaxWindow>();
            HookFaxlessWindowEvents();
        }
        else
        {
            Logger.InfoS("fax", "Creating FaxWindow");
            _window = this.CreateWindow<FaxWindow>();
            HookFaxWindowEvents();
        }
    }


    private void OpenFaxlessUI()
    {
        _window?.Dispose();
        _window = this.CreateWindow<FaxWindow>();
        HookFaxlessWindowEvents();
    }

    private void OpenFaxUI()
    {
        _window?.Dispose();
        _window = this.CreateWindow<FaxWindow>();
        HookFaxWindowEvents();
    }


    private void HookFaxlessWindowEvents()
    {
        if (_window == null || _window.Disposed)
            return;
        _window.FileButtonPressed += OnFileButtonPressed;
        _window.CopyButtonPressed += OnCopyButtonPressed;
    }
    private void HookFaxWindowEvents()
    {
        if (_window == null || _window.Disposed)
            return;
        _window.FileButtonPressed += OnFileButtonPressed;
        _window.CopyButtonPressed += OnCopyButtonPressed;
        _window.SendButtonPressed += OnSendButtonPressed;
        _window.RefreshButtonPressed += OnRefreshButtonPressed;
        _window.PeerSelected += OnPeerSelected;
    }


    private async void OnFileButtonPressed()
    {
        if (_dialogIsOpen)
            return;

        _dialogIsOpen = true;
        var filters = new FileDialogFilters(new FileDialogFilters.Group("txt"));
        await using var file = await _fileDialogManager.OpenFile(filters);
        _dialogIsOpen = false;

        if (_window == null || _window.Disposed || file == null)
        {
            return;
        }

        using var reader = new StreamReader(file);

        var firstLine = await reader.ReadLineAsync();
        string? label = null;
        var content = await reader.ReadToEndAsync();

        if (firstLine is { })
        {
            if (firstLine.StartsWith('#'))
            {
                label = firstLine[1..].Trim();
            }
            else
            {
                content = firstLine + "\n" + content;
            }
        }

        SendMessage(new FaxFileMessage(
            label?[..Math.Min(label.Length, FaxFileMessageValidation.MaxLabelSize)],
            content[..Math.Min(content.Length, FaxFileMessageValidation.MaxContentSize)],
            _window.OfficePaper));
    }

    private void OnSendButtonPressed()
    {
        SendMessage(new FaxSendMessage());
    }

    private void OnCopyButtonPressed()
    {
        SendMessage(new FaxCopyMessage());
    }

    private void OnRefreshButtonPressed()
    {
        SendMessage(new FaxRefreshMessage());
    }

    private void OnPeerSelected(string address)
    {
        SendMessage(new FaxDestinationMessage(address));
    }

    private bool _isFaxless;

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not FaxUiState cast)
        {
            Logger.WarningS("fax", "UpdateState received non-FaxUiState!");
            return;
        }

        Logger.InfoS("fax", $"UpdateState called. IsFaxless: {cast.IsFaxless}, CanSend: {cast.CanSend}");

        _lastState = cast;

        if (cast.IsFaxless != _isFaxless)
        {
            _isFaxless = cast.IsFaxless;
            Logger.InfoS("fax", $"Switching UI to {(_isFaxless ? "Faxless" : "Fax")} mode");
            if (_isFaxless)
                OpenFaxlessUI();
            else
                OpenFaxUI();
        }

        _window?.UpdateState(cast);
    }
}
