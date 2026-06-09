using WorldCupPool.Application.Features.Matches.DTOs;

namespace WorldCupPool.Application.Features.Matches
{
    public interface IMatchesService
    {
        Task<IReadOnlyList<MatchListItemResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<MatchListItemResponseDto> UpdateResultAsync(UpdateMatchResultRequestDto request, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MatchListItemResponseDto>> UpdateResultsBulkAsync(BulkUpdateMatchResultRequestDto request, CancellationToken cancellationToken = default);
    }
}

