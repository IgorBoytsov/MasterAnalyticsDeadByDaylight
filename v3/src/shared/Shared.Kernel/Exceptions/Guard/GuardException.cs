namespace Shared.Kernel.Exceptions.Guard
{
    public static class GuardException
    {
        /// <summary>
        /// Предоставляет доступ к набору стандартных проверок.
        /// </summary>
        public static IGuardClause Against { get; } = new GuardClause();
    }
}