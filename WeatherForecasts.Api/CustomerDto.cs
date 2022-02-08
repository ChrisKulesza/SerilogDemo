using System.ComponentModel.DataAnnotations;

namespace WeatherForecasts.Api;

public record CustomerDto(
    int Id,
    [MaxLength(60)] string Firstname,
    [MaxLength(60)] string Lastname,
    [MaxLength(60)] string City,
    [Range(0, 100)] int Age);
