using System;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Models
{
    /// <summary>
    /// Represents a refresh token for JWT authentication
    /// </summary>
    public class RefreshToken : BaseEntity
    {
        /// <summary>
        /// The refresh token value (hashed for security)
        /// </summary>
        public string TokenHash { get; set; } = string.Empty;

        /// <summary>
        /// The user ID this token belongs to
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the user
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// When this token expires
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// When this token was last used
        /// </summary>
        public DateTime? LastUsedAt { get; set; }

        /// <summary>
        /// Whether this token has been revoked
        /// </summary>
        public bool IsRevoked { get; set; }

        /// <summary>
        /// The IP address that created this token
        /// </summary>
        public string? CreatedByIp { get; set; }

        /// <summary>
        /// The IP address that last used this token
        /// </summary>
        public string? LastUsedByIp { get; set; }

        /// <summary>
        /// Optional replacement token ID (for token rotation)
        /// </summary>
        public string? ReplacedByToken { get; set; }

        /// <summary>
        /// Reason for revocation
        /// </summary>
        public string? RevokedReason { get; set; }
    }
}