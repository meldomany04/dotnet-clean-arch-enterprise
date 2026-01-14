using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BaseApp.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoBindRowVersionAttribute : Attribute { }
}
