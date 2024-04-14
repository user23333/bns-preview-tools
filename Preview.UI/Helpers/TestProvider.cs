#if DEVELOP

using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.UI.Helpers;
internal class TestProvider : FolderProvider
{
	public TestProvider() : base(@"D:\资源\客户端相关\Auto\data")
	{

	}

	public static IDataProvider Provider
	{
		get
		{
			if(!status)
			{
				status = true;
				FileCache.Data = new BnsDatabase(new TestProvider());
			}

			return FileCache.Data.Provider;
		}
	}

	static bool status = false;
}

#endif