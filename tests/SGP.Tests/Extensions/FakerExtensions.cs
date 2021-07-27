namespace SGP.Tests.Extensions
{
    using Bogus;
    using System;

    public static class FakerExtensions
    {
        public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker) where T : class
        {
            return faker.CustomInstantiator(_
                => Activator.CreateInstance(typeof(T), true) as T);
        }
    }
}