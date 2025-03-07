﻿using Finbuckle.MultiTenant.Abstractions;

namespace Infrastructure.Tenancy
{
    public class EduTenantInfo : ITenantInfo
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string DatabaseName { get; set; }
        public string AdminEmail { get; set; }
        public DateTime ValidUpTo { get; set; }
        public bool IsActive { get; set; }
    }
}
