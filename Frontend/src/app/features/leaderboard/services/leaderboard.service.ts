import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../shared/models/apiResponse/apiResponse';
import { LeaderboardUserViewModel } from '../models/leaderboard.models';

@Injectable({ providedIn: 'root' })
export class LeaderboardService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/leaderboard`;

  getLeaderboard(): Observable<ApiResponse<LeaderboardUserViewModel[]>> {
    return this.http.get<ApiResponse<LeaderboardUserViewModel[]>>(this.baseUrl);
  }
}