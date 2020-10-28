using System;

namespace ExBook.Extensions.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ServiceAttribute : Attribute
    {
    }
}
