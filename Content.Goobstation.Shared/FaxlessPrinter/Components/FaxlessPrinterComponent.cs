using Content.Shared.Containers.ItemSlots;
using Content.Shared.Paper;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.GameObjects;

namespace Content.Goobstation.Shared.FaxlessPrinter
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class FaxlessPrinterComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("faxMachine", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string FaxMachinePrototype { get; set; } = "FaxMachine";

        /// <summary>
        /// The actual spawned fax machine entity this wraps.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public EntityUid? FaxMachine;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public ItemSlot PaperSlot = new();

        [DataField]
        public string InsertSound = "/Audio/Machines/scanning.ogg";

        [DataField]
        public string EjectSound = "/Audio/Machines/tray_eject.ogg";
    }
}
