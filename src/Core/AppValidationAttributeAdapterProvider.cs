using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace NsaIpa.Web.Core;

public class AppValidationAttributeAdapterProvider : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
{
    public AppValidationAttributeAdapterProvider() { }

    IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(
        ValidationAttribute attribute,
        IStringLocalizer stringLocalizer)
    {
        IAttributeAdapter adapter;
        if (attribute is ModelAwareValidationAttribute modelAwareValidAttrb)
            adapter = new AppValidationAdapter(modelAwareValidAttrb, stringLocalizer);
        else
            adapter = GetAttributeAdapter(attribute, stringLocalizer);

        return adapter;
    }
}