using System;
using System.Collections.Generic;
using System.Text;

namespace TomadaStore.Models.DTOs.Customer
{
    public class CustomerResponseDTO
    {
        public int Id { get; init; } = 0;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? PhoneNumber { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;
    }
}
