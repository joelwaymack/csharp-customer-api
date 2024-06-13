using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace CustomerApi.Models;

public class Address
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Street { get; set; }

    [Required]
    [MaxLength(50)]
    public string City { get; set; }

    [Required]
    [MaxLength(2)]
    public string State { get; set; }

    [Required]
    [MaxLength(10)]
    public string Zip { get; set; }

    [MaxLength(3)]
    public string Country { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    [Required]
    public Guid CustomerId { get; set; }

    [JsonIgnore]
    public Customer Customer { get; set; }
}