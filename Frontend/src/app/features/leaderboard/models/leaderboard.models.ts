export interface TeamViewModel {
  id: string;
  name: string;
  code: string;
}

export interface PredictionHistoryViewModel {
  id: string;
  dateLabel: string;
  timeLabel: string;
  homeTeam: TeamViewModel;
  awayTeam: TeamViewModel;
  predictedHomeGoals: number;
  predictedAwayGoals: number;
  actualHomeGoals: number | null;
  actualAwayGoals: number | null;
  points: number | null;
  status: 'Pendiente' | 'Finalizado' | 'Enviado';
}

export interface LeaderboardUserViewModel {
  userId: string;
  userName: number;
  fullName: string;
  points: number;
  wonCount: number;
  correctOutcomeCount: number;
  lostCount: number;
  predictions?: PredictionHistoryViewModel[];
}

export interface LeaderboardUserRowViewModel {
  position: number;
  userId: string;
  fullName: string;
  points: number;
  wonCount: number;
  correctOutcomeCount: number;
  lostCount: number;
}

export interface LeaderboardPageViewModel {
  users: LeaderboardUserRowViewModel[];
  loading: boolean;
  errorMessage: string | null;
}