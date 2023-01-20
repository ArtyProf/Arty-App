using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TelegramBot.Helpers;

public static class CurrencyHelper
{
    private static IDictionary<string, string> map;
    static CurrencyHelper()
    {
        map = CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(c => !c.IsNeutralCulture)
            .Select(culture => {
                try
                {
                    return new RegionInfo(culture.Name);
                }
                catch
                {
                    return null;
                }
            })
            .Where(ri => ri is not null)
            .GroupBy(ri => ri.ISOCurrencySymbol)
            .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);
    }
    public static bool TryGetCurrencySymbol(string ISOCurrencySymbol, out string symbol)
    {
        return map.TryGetValue(ISOCurrencySymbol, out symbol);
    }
}
