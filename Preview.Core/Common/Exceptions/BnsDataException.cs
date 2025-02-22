﻿using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Common.Exceptions;
public class BnsDataException : BnsException
{
	#region Constructors
	public BnsDataExceptionCode Code;

	internal BnsDataException(string message) : base(message)
	{

	}

	internal BnsDataException(string message, Exception innerException) : base(message, innerException)
	{

	}

	internal BnsDataException(BnsDataExceptionCode code, params object[] args)
		: base(Globals.TextProvider.Get("Exception_" + code, args))
	{
		Code = code;
	}
	#endregion

	#region	Definition
	internal static void ThrowIfMismatch(EPublisher expected, EPublisher code)
	{
		if (expected == EPublisher.Invalid || code == EPublisher.Invalid || expected == code) return;

		if (!Globals.MessageBox?.Show(Globals.TextProvider.Get("Exception_InvalidPublisher_Ask", expected)) ?? true)
			throw new BnsDataException(BnsDataExceptionCode.InvalidPublisher, code);
	}

	internal static BnsDataException InvalidDefinition(string message, Exception exception = null)
	{
		return new BnsDataException(message, exception);
	}

	internal static BnsDataException InvalidSequence(string message, string name)
	{
		return new BnsDataException($"seq `{name}` {message}");
	}
	#endregion

	#region Data
	internal static BnsDataException InvalidTable(string name)
	{
		return new BnsDataException($"Non-existing table '{name}'.");
	}
	#endregion

	#region Expression
	internal static BnsDataException InvalidExpressionType(BsonExpression expr, BsonExpressionType type)
	{
		return new BnsDataException($"Expression '{expr.Source}' must be a {type} type.");
	}

	internal static BnsDataException InvalidExpressionTypePredicate(BsonExpression expr)
	{
		return new BnsDataException($"Expression '{expr.Source}' are not supported as predicate expression.");
	}

	internal static BnsDataException UnexpectedToken(Token token, string expected = null)
	{
		var position = token?.Position - (token?.Value?.Length ?? 0) ?? 0;
		var str = token?.Type == TokenType.EOF ? "[EOF]" : token?.Value ?? "";
		var exp = expected == null ? "" : $" Expected `{expected}`.";

		return new BnsDataException($"Unexpected token `{str}` in position {position}.{exp}");
	}

	internal static BnsDataException UnexpectedToken(string message, Token token)
	{
		var position = token?.Position - (token?.Value?.Length ?? 0) ?? 0;

		return new BnsDataException(message);
	}
	#endregion
}

public enum BnsDataExceptionCode
{
	None,
	InvalidGame,
	InvalidPublisher,
	InvalidDefinition,
}