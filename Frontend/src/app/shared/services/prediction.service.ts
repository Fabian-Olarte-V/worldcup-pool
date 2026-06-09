import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/apiResponse/apiResponse';
import { PredictionDto, PredictionSubmissionPayload } from '../models/prediction/prediction';


@Injectable({ providedIn: 'root' })
export class PredictionService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/predictions`;

  getPredictions(): Observable<ApiResponse<PredictionDto[]>> {
    return this.http.get<ApiResponse<PredictionDto[]>>(`${this.baseUrl}`);
  }

  getPredictionsByUserId(userId: string): Observable<ApiResponse<PredictionDto[]>> {
    return this.http.get<ApiResponse<PredictionDto[]>>(`${this.baseUrl}/user/${userId}`);
  }

  createPrediction(prediction: PredictionSubmissionPayload): Observable<ApiResponse<PredictionDto>> {
    return this.http.post<ApiResponse<PredictionDto>>(`${this.baseUrl}`, prediction);
  }
}
