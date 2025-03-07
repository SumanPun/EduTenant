namespace Application.Features.Commands
{
    public class CreateTenantRequest
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string DatabaseName { get; set; }
        public string AdminEmail { get; set; }
        public DateTime ValidUpTo { get; set; }
        public bool IsActive { get; set; }
    }
}
