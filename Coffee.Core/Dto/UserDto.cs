namespace Coffee.Core.Dto;

public class UserDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}