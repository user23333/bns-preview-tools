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

		return ahead.Value.ToUpper() switch
		{
			"SELECT" or "EXPLAIN" => this.ParseSelect(),
			"INSERT" => this.ParseInsert(),
			"DELETE" => this.ParseDelete(),
			"UPDATE" => this.ParseUpdate(),
			"COMMIT" => this.ParseCommit(),
			_ => throw BnsDataException.UnexpectedToken(ahead),
		};
	}
}