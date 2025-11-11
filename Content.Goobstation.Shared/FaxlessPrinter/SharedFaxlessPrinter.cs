// SPDX-FileCopyrightText: 2022 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Arimah Greene <30327355+arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 Guilherme Ornel <86210200+joshepvodka@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 exincore <me@exin.xyz>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.FaxlessPrinter;

[Serializable, NetSerializable]
public enum FaxlessPrinterUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class FaxlessPrinterUiState : BoundUserInterfaceState
{
    public string DeviceName { get; }
    public Dictionary<string, string> AvailablePeers { get; }
    public string? DestinationAddress { get; }
    public bool IsPaperInserted { get; }
    public bool CanSend { get; }
    public bool CanCopy { get; }

    public FaxlessPrinterUiState(string deviceName,
        Dictionary<string, string> peers,
        bool canSend,
        bool canCopy,
        bool isPaperInserted,
        string? destAddress)
    {
        DeviceName = deviceName;
        AvailablePeers = peers;
        IsPaperInserted = isPaperInserted;
        CanSend = canSend;
        CanCopy = canCopy;
        DestinationAddress = destAddress;
    }
}

[Serializable, NetSerializable]
public sealed class FaxlessPrinterFileMessage : BoundUserInterfaceMessage
{
    public string? Label;
    public string Content;
    public bool OfficePaper;

    public FaxlessPrinterFileMessage(string? label, string content, bool officePaper)
    {
        Label = label;
        Content = content;
        OfficePaper = officePaper;
    }
}

public static class FaxlessPrinterFileMessageValidation
{
    public const int MaxLabelSize = 50; // parity with Content.Server.Labels.Components.HandLabelerComponent.MaxLabelChars
    public const int MaxContentSize = 10000;
}

[Serializable, NetSerializable]
public sealed class FaxlessPrinterCopyMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class FaxlessPrinterSendMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class FaxlessPrinterRefreshMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class FaxlessPrinterDestinationMessage : BoundUserInterfaceMessage
{
    public string Address { get; }

    public FaxlessPrinterDestinationMessage(string address)
    {
        Address = address;
    }
}
