using System.ComponentModel.DataAnnotations;

namespace WeatherForecasts.Api;

public record CustomerDto(
    int Id,
    [MaxLength(60)] string Name,
    [Range(0, 100)] int Age);
