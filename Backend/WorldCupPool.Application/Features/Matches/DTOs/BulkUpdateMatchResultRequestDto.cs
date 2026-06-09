namespace WorldCupPool.Application.Features.Matches.DTOs
{
    public sealed class BulkUpdateMatchResultRequestDto
    {
        public List<UpdateMatchResultRequestDto> Results { get; set; } = [];
    }
}

