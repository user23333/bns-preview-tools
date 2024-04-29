using IniParser;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Engine.DatData;
public struct Locale
{
    #region Fields
    public BnsVersion ProductVersion { get; set; }

    public EPublisher Publisher { get; set; }

    public ELanguage Language { get; set; }

    public EPublisher AdditionalPublisher { get; set; }

    public int Universe { get; set; }
    #endregion

    #region Methods
    public Locale(EPublisher publisher)
    {
        Publisher = publisher;
        Current = this;
    }

    public Locale(DirectoryInfo directory)
    {
        Load(directory);
        Current = this;
    }

    private void Load(DirectoryInfo directory)
    {
        #region mode
        var Win64 = directory.GetDirectories("Win64", SearchOption.AllDirectories).FirstOrDefault();
        if (Win64 is not null)
        {
            var version = Win64?.GetFiles("version.ini").FirstOrDefault();
            if (version is not null)
            {
                var config = new FileIniDataParser().ReadFile(version.FullName);
                ProductVersion = config["Version"]["ProductVersion"];
            }

            var local = Win64?.GetFiles("local.ini").FirstOrDefault();
            if (local is not null)
            {
                var config = new FileIniDataParser().ReadFile(local.FullName);

                Publisher = config["Locale"]["Publisher"].ToEnum<EPublisher>();
                Language = config["Locale"]["Language"].ToEnum<ELanguage>();
                Universe = config["Locale"]["Universe"].ToInt32();
                AdditionalPublisher = config["Locale"]["AdditionalPublisher"].ToEnum<EPublisher>();
                return;
            }
        }
        #endregion

        #region mode2
        var temp = (directory.GetDirectories("Content", SearchOption.AllDirectories).FirstOrDefault() ?? directory)
            .GetDirectories("local").FirstOrDefault()?
            .GetDirectories().FirstOrDefault();
        if (temp is not null)
        {
            Publisher = temp.Name.ToEnum<EPublisher>();
            Language = (temp.GetDirectories().Where(o => o.Name != "data").FirstOrDefault()?.Name).ToEnum<ELanguage>();

            return;
        }
        #endregion
    }
    #endregion


    /// <summary>
    /// Unable to exclude members in Time64, therefore as a global attribute
    /// </summary>
    internal static Locale Current { get; private set; }
}