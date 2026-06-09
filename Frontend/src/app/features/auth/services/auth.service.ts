import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { Observable } from "rxjs";
import { AuthRequestPayload, AuthUser, SignupRequestPayload } from "../models/appUser";
import { ApiResponse } from "../../../shared/models/apiResponse/apiResponse";

export interface AuthApiResponse extends ApiResponse<AuthUser> {
    errors: string[];
    traceId: string;
    timestampUtc: string;
}

@Injectable({providedIn: 'root'})
export class AuthService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/auth`;
    
    login(authRequest: AuthRequestPayload): Observable<AuthApiResponse> {
        return this.http.post<AuthApiResponse>(`${this.baseUrl}/login`, authRequest);
    }

    signup(authRequest: SignupRequestPayload): Observable<AuthApiResponse> {
        return this.http.post<AuthApiResponse>(`${this.baseUrl}/signup`, authRequest);
    }
}
