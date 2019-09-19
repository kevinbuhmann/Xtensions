namespace Xtensions.Testing.Tests.ValueObjects.TestObjects
{
    using System;
    using System.Linq.Expressions;
    using Xtensions.Testing.ValueObjects;

    public class AppPermissionsValueObjectTestCases : ValueObjectTestCases<AppPermissions>
    {
        public override FactoryExpressionCollection<AppPermissions> GetDistinctFactoryExpressions()
        {
            return new FactoryExpressionCollection<AppPermissions>(new Expression<Func<AppPermissions>>[]
            {
                () => new AppPermissions(UserRights.Foo),
                () => new AppPermissions(AdminRights.Baz),
                () => new AppPermissions(new string[] { "claim", "claim" }),
            });
        }
    }
}
