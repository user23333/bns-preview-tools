using System.Diagnostics;
using Xylia.Preview.Data.Common.Exceptions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Client;
/// <summary>
/// parse and execute sql commands
/// </summary>
internal partial class SqlParser(BnsDatabase _engine, Tokenizer _tokenizer, AttributeDocument _parameters)
{
	public IDataReader Execute()
	{
		var ahead = _tokenizer.LookAhead().Expect(TokenType.Word);

		Debug.WriteLine($"executing `{ahead.Value.ToUpper()}`", "SQL");

		switch (ahead.Value.ToUpper())
		{
			case "SELECT":
			case "EXPLAIN":
				return this.ParseSelect();
			case "INSERT": return this.ParseInsert();
			case "DELETE": return this.ParseDelete();
			case "UPDATE": return this.ParseUpdate();
			case "COMMIT": return this.ParseCommit();

			default: throw BnsDataException.UnexpectedToken(ahead);
		}
	}
}