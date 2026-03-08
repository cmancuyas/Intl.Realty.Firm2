using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

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
        public static List<SelectListItem> CreateSelectListFromEnum<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = Convert.ToInt32(e).ToString(),  // Convert enum to its underlying int value
                    Text = GetEnumDescription(e)
                })
                .ToList();
        }

        private static string GetEnumDescription<TEnum>(TEnum enumValue)
        {
            var field = enumValue!.GetType().GetField(enumValue.ToString()!);
            var descriptionAttribute = field!.GetCustomAttributes(typeof(DisplayAttribute), false)
                                             .FirstOrDefault() as DisplayAttribute;

            return descriptionAttribute?.GetName() ?? enumValue.ToString()!;
        }
    }
}
