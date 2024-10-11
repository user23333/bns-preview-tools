using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Models.Sequence;
public enum EquipType
{
    None,

	[Text("Name.item.equip-type.weapon")]
	Weapon,

	[Text("Name.item.equip-type.costume")]
	Costume,

	[Text("Name.item.equip-type.earring")]
	Earring,

	[Text("Name.item.equip-type.eyeglass")]
	Eyeglass,

	[Text("Name.item.equip-type.hat")]
	Hat,

	[Text("Name.item.equip-type.ring")]
	Ring,

	[Text("Name.item.equip-type.necklace")]
	Necklace,

    [Text("Name.item.equip-type.Gem-1")]
    Gem1,

    [Text("Name.item.equip-type.Gem-2")]
    Gem2,

    [Text("Name.item.equip-type.Gem-3")]
    Gem3,

    [Text("Name.item.equip-type.Gem-4")]
    Gem4,

    [Text("Name.item.equip-type.Gem-5")]
    Gem5,

    [Text("Name.item.equip-type.Gem-6")]
    Gem6,

    [Text("Name.item.equip-type.Gem-7")]
    Gem7,

    [Text("Name.item.equip-type.Gem-8")]
    Gem8,

	[Text("Name.item.equip-type.attach")]
	Attach,

	[Text("Name.item.equip-type.belt")]
	Belt,

	[Text("Name.item.equip-type.bracelet")]
	Bracelet,

	[Text("Name.item.equip-type.soul")]
	Soul,

    [Text("Name.item.equip-type.soul-2")]
    Soul2,

	[Text("Name.item.equip-type.gloves")]
	Gloves,

    [Text("Name.item.equip-type.pet-1")]
    Pet1,

    [Text("Name.item.equip-type.pet-2")]
    Pet2,

    [Text("Name.item.equip-type.rune-1")]
    Rune1,

    [Text("Name.item.equip-type.rune-2")]
    Rune2,

    [Text("Name.item.equip-type.nova")]
    Nova,

    [Text("Name.item.equip-type.badge-1-premium")]
    Badge1Premium,

    [Text("Name.item.equip-type.badge-2-premium")]
    Badge2Premium,

    [Text("Name.item.equip-type.badge-3-premium")]
    Badge3Premium,

    [Text("Name.item.equip-type.badge-1-normal")]
    Badge1Normal,

    [Text("Name.item.equip-type.badge-2-normal")]
    Badge2Normal,

    [Text("Name.item.equip-type.badge-3-normal")]
    Badge3Normal,

    [Text("Name.item.equip-type.badge-appearance")]
    BadgeAppearance,

    [Text("Name.item.equip-type.vehicle")]
    Vehicle,

    [Text("Name.EquipSlot.NormalStateAppearance")]
	NormalStateAppearance,

    [Text("Name.EquipSlot.IdleStateAppearance")]
	IdleStateAppearance,

    [Text("Name.EquipSlot.ChattingSymbol")]
	ChattingSymbol,

    [Text("Name.EquipSlot.PortraitAppearance")]
	PortraitAppearance,

    [Text("Name.EquipSlot.HypermoveAppearance")]
	HypermoveAppearance,

    [Text("Name.EquipSlot.NameplateAppearance")]
	NamePlateAppearance,

    [Text("Name.EquipSlot.SpeechBubble")]
	SpeechBubbleAppearance,

	[Text("Name.EquipSlot.TalkSocialApperance")]
	TalkSocial,

	[Text("Name.EquipSlot.Armlet-1")]
	Armlet1,

	[Text("Name.EquipSlot.Armlet-2")]
	Armlet2,

	COUNT
}