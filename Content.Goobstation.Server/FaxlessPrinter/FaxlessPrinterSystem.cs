using Content.Server.Fax;
using Content.Shared.Fax;
using Content.Shared.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Player;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;
using Content.Goobstation.Shared.FaxlessPrinter;
using Content.Shared.Fax.Components;

namespace Content.Server.FaxlessPrinter
{
    public sealed class FaxlessPrinterSystem : EntitySystem
    {
        [Dependency] private readonly FaxSystem _faxSystem = default!;
        [Dependency] private readonly UserInterfaceSystem _ui = default!;
        [Dependency] private readonly IGameTiming _timing = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<FaxlessPrinterComponent, AfterActivatableUIOpenEvent>(OnUiOpened);
            SubscribeLocalEvent<FaxlessPrinterComponent, FaxFileMessage>(OnPrintFileRequested);
            SubscribeLocalEvent<FaxlessPrinterComponent, FaxCopyMessage>(OnCopyRequested);
            SubscribeLocalEvent<FaxlessPrinterComponent, FaxSendMessage>(OnSendRequested);
            SubscribeLocalEvent<FaxlessPrinterComponent, FaxRefreshMessage>(OnRefreshRequested);
            SubscribeLocalEvent<FaxlessPrinterComponent, FaxDestinationMessage>(OnDestinationSelected);
        }

        private void OnUiOpened(EntityUid uid, FaxlessPrinterComponent comp, AfterActivatableUIOpenEvent args)
        {
            UpdateUi(uid, comp);
        }

        private void OnPrintFileRequested(EntityUid uid, FaxlessPrinterComponent comp, FaxFileMessage args)
        {
            if (comp.FaxMachine is not { } faxUid)
                return;

            FaxMachineComponent? faxComp = null;
            if (!Resolve(faxUid, ref faxComp))
                return;

            _faxSystem.PrintFile(faxUid, faxComp, args);
            UpdateUi(uid, comp);
        }

        private void OnCopyRequested(EntityUid uid, FaxlessPrinterComponent comp, FaxCopyMessage args)
        {
            if (comp.FaxMachine is not { } faxUid)
                return;

            FaxMachineComponent? faxComp = null;
            if (!Resolve(faxUid, ref faxComp))
                return;

            _faxSystem.Copy(faxUid, faxComp, args);
            UpdateUi(uid, comp);
        }

        private void OnSendRequested(EntityUid uid, FaxlessPrinterComponent comp, FaxSendMessage args)
        {
            if (comp.FaxMachine is not { } faxUid)
                return;

            FaxMachineComponent? faxComp = null;
            if (!Resolve(faxUid, ref faxComp))
                return;

            _faxSystem.Send(faxUid, faxComp, args);
            UpdateUi(uid, comp);
        }

        private void OnRefreshRequested(EntityUid uid, FaxlessPrinterComponent comp, FaxRefreshMessage args)
        {
            if (comp.FaxMachine is not { } faxUid)
                return;

            FaxMachineComponent? faxComp = null;
            if (!Resolve(faxUid, ref faxComp))
                return;

            _faxSystem.Refresh(faxUid, faxComp);
            UpdateUi(uid, comp);
        }

        private void OnDestinationSelected(EntityUid uid, FaxlessPrinterComponent comp, FaxDestinationMessage args)
        {
            if (comp.FaxMachine is not { } faxUid)
                return;

            FaxMachineComponent? faxComp = null;
            if (!Resolve(faxUid, ref faxComp))
                return;

            _faxSystem.SetDestination(faxUid, args.Address, faxComp);
            UpdateUi(uid, comp);
        }

        public void UpdateUi(EntityUid uid, FaxlessPrinterComponent comp)
        {
            if (comp.FaxMachine is not { } faxUid)
                return;

            FaxMachineComponent? faxComp = null;
            if (!Resolve(faxUid, ref faxComp))
                return;

            var state = new FaxUiState(
                faxComp.FaxName,
                faxComp.KnownFaxes,
                faxComp.PaperSlot.Item != null && faxComp.DestinationFaxAddress != null && faxComp.SendTimeoutRemaining <= 0,
                faxComp.PaperSlot.Item != null && faxComp.SendTimeoutRemaining <= 0,
                faxComp.PaperSlot.Item != null,
                faxComp.DestinationFaxAddress);

            _ui.SetUiState(uid, FaxlessPrinterUiKey.Key, state);
        }
    }
}
