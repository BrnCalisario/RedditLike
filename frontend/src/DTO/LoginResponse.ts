export interface LoginResponse {
    userExists: boolean;
    success: boolean;
    jwt: string;
}
