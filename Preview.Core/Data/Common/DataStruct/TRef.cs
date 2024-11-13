using System.Diagnostics;
using System.Runtime.InteropServices;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Common.DataStruct;

[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Table}:{Ref}")]
public struct TRef
{
	#region Constructors
	public int Table;
	public Ref Ref;

	public TRef(ushort table, int id, int variant = 0)
	{
		Table = table;
		Ref = new Ref(id, variant);
	}

	public TRef(ushort table, Ref Ref) : this(table, Ref.Id, Ref.Variant)
	{
	}

	public TRef(Record record)
	{
		if (record is null) return;

		Table = record.Owner.Type;
		Ref = record.PrimaryKey;
	}
	#endregion

	#region Methods
	public static bool operator ==(TRef a, TRef b)
	{
		return a.Table == b.Table && a.Ref == b.Ref;
	}

	public static bool operator !=(TRef a, TRef b)
	{
		return !(a == b);
	}

	public readonly bool Equals(TRef other)
	{
		return Table == other.Table && Ref == other.Ref;
	}

	public override readonly bool Equals(object obj) => obj is TRef other && Equals(other);

	public override readonly int GetHashCode() => HashCode.Combine(Table, Ref);

	public readonly Record GetRecord(IDataProvider provider)
	{
		if (this == default) return null;

		return provider.Tables[(ushort)this.Table]?[Ref];
	}
	#endregion
}