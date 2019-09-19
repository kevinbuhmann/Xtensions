namespace Xtensions.Testing.Tests.ValueObjects.TestObjects
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using CSharpFunctionalExtensions;
    using Newtonsoft.Json;

    [Flags]
    public enum UserRights
    {
        None = 0,
        Foo = 1,
        Bar = 2,
    }

    [Flags]
    public enum AdminRights
    {
        None = 0,
        Foo = 1,
        Baz = 2,
    }

    public class AppPermissions : ValueObject
    {
        public AppPermissions(UserRights userRights)
        {
            this.UserRights = userRights;
        }

        public AppPermissions(AdminRights adminRights)
        {
            this.AdminRights = adminRights;
        }

        public AppPermissions(IEnumerable<string> claims)
        {
            this.Claims = claims?.ToList();
        }

        [JsonConstructor]
        [SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Reflection")]
        private AppPermissions(UserRights userRights, AdminRights adminRights, IEnumerable<string> claims)
        {
            this.UserRights = userRights;
            this.AdminRights = adminRights;
            this.Claims = claims?.ToList();
        }

        public UserRights UserRights { get; }

        public AdminRights AdminRights { get; }

        public IReadOnlyCollection<string> Claims { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { this.UserRights, this.AdminRights };
        }
    }
}
