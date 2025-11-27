namespace TicketManagementSystem.API
{
    /// <summary>
    /// Constants used throughout the application
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// User roles constants
        /// </summary>
        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string Agent = "Agent";
            public const string User = "User";
        }

        /// <summary>
        /// Ticket statuses constants
        /// </summary>
        public static class TicketStatuses
        {
            public const string Open = "Open";
            public const string InProgress = "InProgress";
            public const string Resolved = "Resolved";
            public const string Closed = "Closed";
        }

        /// <summary>
        /// Ticket priorities constants
        /// </summary>
        public static class TicketPriorities
        {
            public const string Low = "Low";
            public const string Medium = "Medium";
            public const string High = "High";
            public const string Urgent = "Urgent";
        }
    }
}