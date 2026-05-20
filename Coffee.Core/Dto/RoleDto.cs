using Coffee.Core.Entities;

namespace Coffee.Core.Dto;

public class RoleDto : DtoBase
{
    public string Name { get; set; } = string.Empty;

    public RoleDto() { }

    public RoleDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
    }
}