using System.Windows;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Views.Pages;
public partial class AbilitySetting
{
    #region Constructor
    public AbilitySetting()
    {
        InitializeComponent();
    }
    #endregion

    #region Methods
    private void GetFactor_Click(object sender, RoutedEventArgs e)
    {
        #region Initialize
        int value1 = Page1_Value1.Text.To<int>();
		int value2 = Page1_Value2.Text.To<int>();
        double extra = Txt_Inital.Text.To<double>();
        double percent1 = Page1_Percent1.Text.To<double>() - extra;
        double percent2 = Page1_Percent2.Text.To<double>() - extra;
        #endregion

        #region Calculate
        double k, A;

        if (Page1_k_Lock.IsChecked == true)
        {
            k = Page1_k.Text.To<double>();
            A = ((value1 * k / percent1 - value1) + (value2 * k / percent2 - value2)) / 2;
        }
        else
        {
            k = (value2 * percent1 * percent2 - value1 * percent1 * percent2) / (value2 * percent1 - value1 * percent2);
            A = (-value1 * value2 * percent2 + value2 * value1 * percent1) / (value1 * percent2 - value2 * percent1);
        }

        Page1_k.Text = k.ToString();
        Page1_A.Text = A.ToString();
        #endregion
    }

    private void GetParams_Click(object sender, RoutedEventArgs e)
    {
        if (!double.TryParse(Page2_Value1.Text, out double Value1)) return;
        if (!double.TryParse(Page2_Value2.Text, out double Value2)) return;
        if (!double.TryParse(Page2_Percent1.Text, out double Percent1)) return;
        if (!double.TryParse(Page2_Percent2.Text, out double Percent2)) return;

        var level1 = (sbyte)Level1.Value;
        var level2 = (sbyte)Level2.Value;

        var ability = new AbilityFunction()
        {
            C = int.Parse(Txt_Inital.Text),
            K = int.Parse(Params_k.Text),
        };
        var Factor1 = new AbilityFunction.LevelFactor(level1, ability.GetFactor(Value1, Percent1));
        var Factor2 = new AbilityFunction.LevelFactor(level2, ability.GetFactor(Value2, Percent2));

        ability.GetFactorParam(Factor1, Factor2);

        Params_k.Text = ability.Φ.ToString();
        Params_Φ.Text = ability.μ.ToString();
    }
    #endregion
}
