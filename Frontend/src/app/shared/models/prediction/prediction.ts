export interface PredictionPayloadDto {
  MatchId: string;
  HomeGoals: number | null;
  AwayGoals: number | null;
}

export interface PredictionSubmissionPayload {
  MatchId: string;
  HomeGoals: number;
  AwayGoals: number;
}

export interface MatchResultsBulkRequest {
  results: PredictionSubmissionPayload[];
}

export interface PredictionDto {
  id: string;
  matchId: string;
  homeGoals: number | null;
  awayGoals: number | null;
  points?: number | null;
  createdAtUtc?: string | null;
  updatedAtUtc?: string | null;
}
