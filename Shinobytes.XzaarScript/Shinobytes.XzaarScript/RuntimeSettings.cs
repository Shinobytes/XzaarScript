namespace Shinobytes.XzaarScript
{
    public class RuntimeSettings
    {
        public static RuntimeSettings Default => new RuntimeSettings();

        public bool CanAccessPrivateMembers { get; }

        public RuntimeSettings(bool canAccessPrivateMembers)
        {
            CanAccessPrivateMembers = canAccessPrivateMembers;
        }

        private RuntimeSettings() : this(true)
        {
        }
    }
}