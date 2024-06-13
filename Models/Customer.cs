using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace CustomerApi.Models;

public class Customer
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(15)]
    public string Phone { get; set; }

    [JsonIgnore]
    public IList<Address> Addresses { get; set; }
}