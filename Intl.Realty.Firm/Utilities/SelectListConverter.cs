using Microsoft.AspNetCore.Mvc.Rendering;

namespace Intl.Realty.Firm.Utilities
{
    public class SelectListConverter
    {
        public static List<SelectListItem> CreateSelectList<T>(IList<T> entities,
                                            Func<T, object> funcToGetValue,
                                            Func<T, object> funcToGetText)
        {
            return entities
                    .Select(x => new SelectListItem
                    {
                        Value = funcToGetValue(x).ToString(),
                        Text = funcToGetText(x).ToString(),
                    }).ToList();
        }
    }
}
