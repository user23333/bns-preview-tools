namespace Xylia.Preview.Data.Common.Abstractions;
internal interface IGameDataKeyParser
{

}

internal interface IMultiKeyRefGenerator
{
	long Create(string[] keyTextList);
}