namespace Application.Features.Identity.Users.Queries
{
    public class ChangeUserStatusRequest
    {
        public string UserId { get; set; }
        public bool Activation { get; set; }
    }
}
