namespace Xylia.Preview.Data.Common.Abstractions;
public interface ITextProvider
{
	string this[string key] { get; }
}