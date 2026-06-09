export interface ApiResponse<T>{
    success: boolean;
    statusCode: number;
    message: string;
    data: T;
    errors?: string[];
    traceId?: string;
    timestampUtc?: string;
}

export interface ApiHttpError {
    success: boolean;
    statusCode: number;
    message: string;
    errors?: string[];
}
